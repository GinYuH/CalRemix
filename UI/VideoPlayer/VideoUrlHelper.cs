using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using YoutubeExplode;

namespace CalRemix.UI.VideoPlayer;

public static class VideoUrlHelper
{
    private static readonly ConcurrentDictionary<string, (string Url, DateTime Timestamp)> _urlCache = [];
    private static readonly ConcurrentDictionary<Guid, CancellationTokenSource> _activeRequests = [];
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
    private static readonly SemaphoreSlim _requestLock = new SemaphoreSlim(1, 1);
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(45);
    private static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan SemaphoreTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Check if a URL is a YouTube link.
    /// </summary>
    public static bool IsYouTubeUrl(string url)
    {
        return url.Contains("youtube.com") || url.Contains("youtu.be");
    }

    /// <summary>
    /// Clear the URL cache.
    /// </summary>
    public static void ClearCache()
    {
        _urlCache.Clear();
        CalRemix.instance.Logger.Info("Video URL cache cleared");
    }

    /// <summary>
    /// Remove a specific URL from cache.
    /// </summary>
    public static void RemoveFromCache(string url)
    {
        _urlCache.TryRemove(url, out _);
    }

    /// <summary>
    /// Get direct stream URL from YouTube using YoutubeExplode.
    /// Returns null if extraction fails.
    /// </summary>
    public static async Task<string> GetDirectUrlFromYouTubeAsync(string youtubeUrl, CancellationToken cancellationToken = default)
    {
        // Check cache first
        if (_urlCache.TryGetValue(youtubeUrl, out var cached) &&
            DateTime.Now - cached.Timestamp < CacheDuration)
        {
            return cached.Url;
        }

        // Fetch with retry logic
        int maxRetries = 2;
        for (int retry = 0; retry <= maxRetries; retry++)
        {
            try
            {
                string directUrl = await FetchSingleAttemptAsync(youtubeUrl, cancellationToken);

                // Cache the result (even null results to avoid repeated failures)
                _urlCache[youtubeUrl] = (directUrl, DateTime.Now);
                return directUrl;
            }
            catch (OperationCanceledException)
            {
                CalRemix.instance.Logger.Warn($"YouTube request for {youtubeUrl} was cancelled.");
                return null;
            }
            catch (Exception ex) when (retry < maxRetries)
            {
                CalRemix.instance.Logger.Warn($"Attempt {retry + 1} failed for {youtubeUrl}: {ex.Message}");
                await Task.Delay(1000 * (retry + 1), cancellationToken); // Exponential backoff
            }
            catch (Exception ex)
            {
                CalRemix.instance.Logger.Error($"All attempts failed for {youtubeUrl}: {ex.Message}");
                // Cache the failure to avoid repeated attempts
                _urlCache[youtubeUrl] = (null, DateTime.Now);
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Single attempt at fetching from YouTube.
    /// </summary>
    private static async Task<string> FetchSingleAttemptAsync(string youtubeUrl, CancellationToken cancellationToken)
    {
        // Add timeout for semaphore acquisition
        bool lockAcquired = false;
        try
        {
            lockAcquired = await _requestLock.WaitAsync(SemaphoreTimeout, cancellationToken);
            if (!lockAcquired)
            {
                CalRemix.instance.Logger.Error("Timed out waiting for request lock");
                return null;
            }

            // Use a short-lived HttpClient with custom timeout for the YoutubeClient
            var httpClient = new HttpClient();
            httpClient.Timeout = ConnectionTimeout;

            var youtube = new YoutubeClient(httpClient);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCts.CancelAfter(RequestTimeout);

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(
                youtubeUrl,
                linkedCts.Token
            ).ConfigureAwait(false);

            // Try to get a muxed stream (video + audio in one)
            var muxedStream = streamManifest
                .GetMuxedStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (muxedStream != null)
            {
                CalRemix.instance.Logger.Info($"Found muxed stream: {muxedStream.VideoQuality.Label}");
                return muxedStream.Url;
            }

            // Fallback: video only
            var videoStream = streamManifest
                .GetVideoOnlyStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (videoStream != null)
            {
                CalRemix.instance.Logger.Warn("Only found video-only stream (no audio available)");
                return videoStream.Url;
            }

            // Last resort: audio only
            var audioStream = streamManifest
                .GetAudioOnlyStreams()
                .OrderByDescending(s => s.Bitrate)
                .FirstOrDefault();

            if (audioStream != null)
            {
                CalRemix.instance.Logger.Warn("Only found audio-only stream (no video available)");
                return audioStream.Url;
            }

            CalRemix.instance.Logger.Error("No suitable streams found");
            return null;
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            CalRemix.instance.Logger.Error("YouTube request was cancelled by the user.");
            throw new OperationCanceledException(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            CalRemix.instance.Logger.Error("YouTube request timed out (likely connection issue)");
            throw;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Error("YouTube request was cancelled by timeout");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YouTube URL extraction failed: {ex.Message}");
            throw;
        }
        finally
        {
            if (lockAcquired)
            {
                _requestLock.Release();
            }
        }
    }

    /// <summary>
    /// Process a URL asynchronously - if it's YouTube, extract direct URL, otherwise return as-is.
    /// Callback is invoked on completion with the result URL (or null on failure).
    /// </summary>
    public static void ProcessUrlAsync(string url, Action<string> onComplete, Guid requestId)
    {
        if (IsYouTubeUrl(url))
        {
            Main.NewText("Extracting YouTube stream URL...", Color.LightBlue);

            var cts = new CancellationTokenSource();
            _activeRequests[requestId] = cts;

            Task.Run(async () =>
            {
                try
                {
                    // Pass the cancellation token to the fetch method
                    string directUrl = await GetDirectUrlFromYouTubeAsync(url, cts.Token)
                        .ConfigureAwait(false);

                    // Remove from active requests
                    _activeRequests.TryRemove(requestId, out _);

                    Main.QueueMainThreadAction(() => onComplete?.Invoke(directUrl));
                }
                catch (OperationCanceledException)
                {
                    CalRemix.instance.Logger.Info($"Request {requestId} was cancelled");
                    _activeRequests.TryRemove(requestId, out _);
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Error($"ProcessUrlAsync failed: {ex.Message}");
                    _activeRequests.TryRemove(requestId, out _);
                    Main.QueueMainThreadAction(() => onComplete?.Invoke(null));
                }
            }, cts.Token); // Pass token to Task.Run as well
        }
        else
        {
            onComplete?.Invoke(url);
        }
    }

    /// <summary>
    /// Cancel a pending request by its ID.
    /// </summary>
    public static void CancelRequest(Guid requestId)
    {
        if (_activeRequests.TryRemove(requestId, out var cts))
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}