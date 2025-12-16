using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace CalRemix.Core.VideoPlayer;

public static class VideoUrlHelper
{
    private static readonly ConcurrentDictionary<string, (string Url, DateTime Timestamp)> _urlCache = [];
    private static readonly ConcurrentDictionary<Guid, CancellationTokenSource> _activeRequests = [];
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
    private static readonly SemaphoreSlim _requestLock = new(1, 1);
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(45);
    private static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan SemaphoreTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Check if a URL is a YouTube playlist link.
    /// </summary>
    public static bool IsYouTubePlaylist(string url)
    {
        return (url.Contains("youtube.com") || url.Contains("youtu.be")) &&
               (url.Contains("list=") || url.Contains("/playlist?"));
    }

    /// <summary>
    /// Check if a URL is a YouTube link.
    /// </summary>
    public static bool IsYouTubeUrl(string url) => url.Contains("youtube.com") || url.Contains("youtu.be");

    /// <summary>
    /// Extract playlist ID from a YouTube URL.
    /// Returns null if not a playlist or extraction fails.
    /// </summary>
    public static string ExtractPlaylistId(string url)
    {
        try
        {
            // Format: youtube.com/playlist?list=PLAYLIST_ID
            // Or: youtube.com/watch?v=VIDEO_ID&list=PLAYLIST_ID
            int listIndex = url.IndexOf("list=");
            if (listIndex == -1)
                return null;

            string afterList = url.Substring(listIndex + 5);

            // Extract until next & or end of string
            int ampersandIndex = afterList.IndexOf('&');
            string playlistId = ampersandIndex != -1
                ? afterList.Substring(0, ampersandIndex)
                : afterList;

            return playlistId;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Failed to extract playlist ID: {ex.Message}");
            return null;
        }
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
    /// Cleans out the URL cach of any expired links.
    /// </summary>
    public static void CleanupCache()
    {
        var now = DateTime.Now;
        var keysToRemove = _urlCache
            .Where(kvp => now - kvp.Value.Timestamp >= CacheDuration)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _urlCache.TryRemove(key, out _);
        }

        if (keysToRemove.Count > 0)
        {
            CalRemix.instance.Logger.Info($"Cleaned up {keysToRemove.Count} expired cache entries");
        }
    }

    /// <summary>
    /// Remove a specific URL from cache.
    /// </summary>
    public static void RemoveFromCache(string url)
    {
        _urlCache.TryRemove(url, out _);
    }

    /// <summary>
    /// Search YouTube for videos and return the URL of the specified result (0-based index).
    /// Uses reflection to avoid JIT compilation issues with IAsyncEnumerable.
    /// Returns null if no results found or index out of range.
    /// </summary>
    /// <param name="searchQuery">The search query</param>
    /// <param name="resultIndex">Which result to return (0 = first, 1 = second, etc.). Use -1 for random.</param>
    /// <param name="maxResults">Maximum results to fetch when using random selection (default 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task<string> SearchYouTubeAsync(string searchQuery, int resultIndex = 0, int maxResults = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            CalRemix.instance.Logger.Error("Empty search query");
            return null;
        }

        CalRemix.instance.Logger.Info($"Searching YouTube for: '{searchQuery}' (result index: {resultIndex})");

        try
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = ConnectionTimeout;
            var youtube = new YoutubeClient(httpClient);

            var searchClient = youtube.Search;
            var getVideosMethod = searchClient.GetType().GetMethod("GetVideosAsync");

            if (getVideosMethod == null)
            {
                CalRemix.instance.Logger.Error("GetVideosAsync method not found");
                return null;
            }

            // Invoke GetVideosAsync
            object searchResultsObj = getVideosMethod.Invoke(searchClient, new object[] { searchQuery, cancellationToken });

            var asyncEnumerableInterface = searchResultsObj.GetType().GetInterfaces()
                .FirstOrDefault(i => i.Name.Contains("IAsyncEnumerable"));

            if (asyncEnumerableInterface == null)
            {
                CalRemix.instance.Logger.Error("IAsyncEnumerable interface not found");
                return null;
            }

            var getEnumeratorMethod = asyncEnumerableInterface.GetMethod("GetAsyncEnumerator");
            if (getEnumeratorMethod == null)
            {
                CalRemix.instance.Logger.Error("GetAsyncEnumerator not found on interface");
                return null;
            }

            // Call GetAsyncEnumerator
            object enumeratorObj = getEnumeratorMethod.Invoke(searchResultsObj, new object[] { cancellationToken });

            var asyncEnumeratorInterface = enumeratorObj.GetType().GetInterfaces()
                .FirstOrDefault(i => i.Name.Contains("IAsyncEnumerator"));

            if (asyncEnumeratorInterface == null)
            {
                CalRemix.instance.Logger.Error("IAsyncEnumerator interface not found");
                return null;
            }

            var moveNextMethod = asyncEnumeratorInterface.GetMethod("MoveNextAsync");
            var currentProperty = asyncEnumeratorInterface.GetProperty("Current");

            if (moveNextMethod == null || currentProperty == null)
            {
                CalRemix.instance.Logger.Error("MoveNextAsync or Current not found on enumerator interface");
                return null;
            }

            try
            {
                List<VideoSearchResult> results = new List<VideoSearchResult>();
                int fetchLimit = resultIndex >= 0 ? resultIndex + 1 : maxResults;

                // Fetch results up to the needed amount
                while (results.Count < fetchLimit)
                {
                    dynamic moveNextTask = moveNextMethod.Invoke(enumeratorObj, null);
                    bool hasResult = await moveNextTask;

                    if (!hasResult)
                        break;

                    VideoSearchResult video = (VideoSearchResult)currentProperty.GetValue(enumeratorObj);
                    results.Add(video);
                }

                if (results.Count == 0)
                {
                    CalRemix.instance.Logger.Warn($"No video results found for: '{searchQuery}'");
                    return null;
                }

                // Select the appropriate result
                VideoSearchResult selectedVideo;

                if (resultIndex == -1)
                {
                    // Random selection
                    selectedVideo = results[Main.rand.Next(results.Count)];
                    CalRemix.instance.Logger.Info($"Randomly selected video {results.IndexOf(selectedVideo) + 1}/{results.Count}: '{selectedVideo.Title}' by {selectedVideo.Author}");
                }
                else if (resultIndex >= results.Count)
                {
                    // Index out of range, use last result
                    selectedVideo = results[results.Count - 1];
                    CalRemix.instance.Logger.Warn($"Result index {resultIndex} out of range (only {results.Count} results), using last result");
                }
                else
                {
                    selectedVideo = results[resultIndex];
                    CalRemix.instance.Logger.Info($"Selected result {resultIndex + 1}: '{selectedVideo.Title}' by {selectedVideo.Author}");
                }

                string videoUrl = $"https://youtube.com/watch?v={selectedVideo.Id}";
                return videoUrl;
            }
            finally
            {
                // Properly dispose the enumerator
                var asyncDisposableInterface = enumeratorObj.GetType().GetInterfaces()
                    .FirstOrDefault(i => i.Name.Contains("IAsyncDisposable"));

                if (asyncDisposableInterface != null)
                {
                    var disposeMethod = asyncDisposableInterface.GetMethod("DisposeAsync");
                    if (disposeMethod != null)
                    {
                        dynamic disposeTask = disposeMethod.Invoke(enumeratorObj, null);
                        await disposeTask;
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Error("YouTube search was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YouTube search failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get all video URLs from a YouTube playlist.
    /// Returns empty list if extraction fails.
    /// </summary>
    public static async Task<List<string>> GetPlaylistVideosAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        var videoUrls = new List<string>();

        if (string.IsNullOrWhiteSpace(playlistId))
        {
            CalRemix.instance.Logger.Error("Empty playlist ID");
            return videoUrls;
        }

        CalRemix.instance.Logger.Info($"Direct scraping playlist: {playlistId}");

        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

            // Fetch the playlist page
            string playlistUrl = $"https://www.youtube.com/playlist?list={playlistId}";
            string html = await httpClient.GetStringAsync(playlistUrl, cancellationToken);
            CalRemix.instance.Logger.Debug($"Fetched playlist page, length: {html.Length} chars");

            // Strategy 1: Try to extract from JSON-LD structured data (more reliable)
            var jsonLdMatch = System.Text.RegularExpressions.Regex.Match(
                html,
                @"<script type=\""application/ld\+json\"">(.*?)</script>",
                System.Text.RegularExpressions.RegexOptions.Singleline
            );

            if (jsonLdMatch.Success)
            {
                string json = jsonLdMatch.Groups[1].Value;
                CalRemix.instance.Logger.Debug("Found JSON-LD data, extracting video URLs...");

                // Look for video URLs in the JSON
                var videoIdMatches = System.Text.RegularExpressions.Regex.Matches(
                    json,
                    @"""url"":\s*\""https:\\/\\/www\.youtube\.com\\/watch\\?v=([A-Za-z0-9_-]{11})"""
                );

                foreach (System.Text.RegularExpressions.Match match in videoIdMatches)
                {
                    if (match.Groups.Count > 1)
                    {
                        string videoId = match.Groups[1].Value.Replace("\\", "");
                        string videoUrl = $"https://www.youtube.com/watch?v={videoId}";
                        if (!videoUrls.Contains(videoUrl))
                        {
                            videoUrls.Add(videoUrl);
                        }
                    }
                }

                CalRemix.instance.Logger.Debug($"Extracted {videoUrls.Count} videos from JSON-LD");
            }

            // Strategy 2: Fallback to general HTML regex search (less reliable)
            if (videoUrls.Count == 0)
            {
                CalRemix.instance.Logger.Debug("JSON-LD extraction failed, falling back to HTML regex...");

                // Look for video IDs in various HTML patterns
                var fallbackMatches = System.Text.RegularExpressions.Regex.Matches(
                    html,
                    @"/watch\?v=([A-Za-z0-9_-]{11})"
                );

                foreach (System.Text.RegularExpressions.Match match in fallbackMatches)
                {
                    if (match.Groups.Count > 1)
                    {
                        string videoId = match.Groups[1].Value;
                        string videoUrl = $"https://www.youtube.com/watch?v={videoId}";
                        if (!videoUrls.Contains(videoUrl))
                        {
                            videoUrls.Add(videoUrl);
                        }
                    }
                }

                CalRemix.instance.Logger.Debug($"Extracted {videoUrls.Count} videos from HTML regex");
            }

            // Strategy 3: Look for initial data (YouTube's internal data structure)
            if (videoUrls.Count == 0)
            {
                var ytInitialDataMatch = System.Text.RegularExpressions.Regex.Match(
                    html,
                    @"var ytInitialData\s*=\s*({.*?});\s*</script>",
                    System.Text.RegularExpressions.RegexOptions.Singleline
                );

                if (ytInitialDataMatch.Success)
                {
                    CalRemix.instance.Logger.Debug("Found ytInitialData, attempting extraction...");
                    // Note: This JSON contains the full playlist structure but is complex to parse
                    // You could use Newtonsoft.Json or System.Text.Json here if needed
                }
            }

            // Remove duplicates while preserving order
            int initialCount = videoUrls.Count;
            videoUrls = videoUrls.Distinct().ToList();

            if (initialCount != videoUrls.Count)
            {
                CalRemix.instance.Logger.Debug($"Removed {initialCount - videoUrls.Count} duplicate video entries");
            }

            CalRemix.instance.Logger.Info($"Direct scraping found {videoUrls.Count} unique videos in playlist");

            return videoUrls;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Error("Direct playlist fetch was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Direct playlist fetch failed: {ex.Message}\n{ex.StackTrace}");
            return videoUrls;
        }
    }

    /// <summary>
    /// Search YouTube and get the direct stream URL of the first result.
    /// Callback is invoked on completion with the result URL (or null on failure).
    /// </summary>
    public static void ProcessSearchAsync(string searchQuery, Action<string> onComplete, Guid requestId, int resultIndex = 0, int maxResults = 10)
    {
        Main.NewText("Searching YouTube...", Color.LightBlue);

        var cts = new CancellationTokenSource();
        _activeRequests[requestId] = cts;

        Task.Run(async () =>
        {
            try
            {
                // First, search to get video URL
                string videoUrl = await SearchYouTubeAsync(searchQuery, resultIndex, maxResults, cts.Token)
                    .ConfigureAwait(false);

                if (videoUrl == null)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        // Cancelled - don't show message
                    }
                    else
                    {
                        // No results
                        Main.QueueMainThreadAction(() => {
                            Main.NewText("No search results found!", Color.Orange);
                        });
                    }

                    _activeRequests.TryRemove(requestId, out var removedCts);
                    removedCts?.Dispose();
                    Main.QueueMainThreadAction(() => onComplete?.Invoke(null));
                    return;
                }

                Main.QueueMainThreadAction(() => {
                    Main.NewText($"Found video, extracting stream...", Color.LightGreen);
                });

                // Then extract direct stream URL from the found video
                string directUrl = await GetDirectUrlFromYouTubeAsync(videoUrl, cts.Token)
                    .ConfigureAwait(false);

                _activeRequests.TryRemove(requestId, out var completedCts);
                completedCts?.Dispose();
                Main.QueueMainThreadAction(() => onComplete?.Invoke(directUrl));
            }
            catch (OperationCanceledException)
            {
                CalRemix.instance.Logger.Info($"Search request {requestId} was cancelled by user");
                _activeRequests.TryRemove(requestId, out var cancelledCts);
                cancelledCts?.Dispose();
            }
            catch (Exception ex)
            {
                CalRemix.instance.Logger.Error($"Search processing failed: {ex.Message}");
                _activeRequests.TryRemove(requestId, out var errorCts);
                errorCts?.Dispose();
                Main.QueueMainThreadAction(() => onComplete?.Invoke(null));
            }
        }, cts.Token);
    }

    /// <summary>
    /// Get direct stream URL from YouTube using YoutubeExplode.
    /// Returns null if extraction fails.
    /// </summary>
    public static async Task<string> GetDirectUrlFromYouTubeAsync(string youtubeUrl, CancellationToken cancellationToken = default)
    {
        CalRemix.instance.Logger.Info($"GetDirectUrlFromYouTubeAsync callback invoked, URL: {youtubeUrl}");

        // Check cache first
        if (_urlCache.TryGetValue(youtubeUrl, out var cached) &&
            DateTime.Now - cached.Timestamp < CacheDuration)
        {
            CalRemix.instance.Logger.Info($"Direct url found in cache.");
            return cached.Url;
        }

        int maxRetries = 2;
        for (int retry = 0; retry <= maxRetries; retry++)
        {
            try
            {
                string directUrl = await FetchSingleAttemptAsync(youtubeUrl, cancellationToken);
                _urlCache[youtubeUrl] = (directUrl, DateTime.Now);
                CalRemix.instance.Logger.Info($"YouTube direct url found.");
                return directUrl;
            }
            catch (OperationCanceledException)
            {
                CalRemix.instance.Logger.Warn($"YouTube request cancelled.");
                return null;
            }
            catch (Exception ex) when (retry < maxRetries)
            {
                CalRemix.instance.Logger.Warn($"Attempt {retry + 1} failed: {ex.Message}");
                await Task.Delay(1000 * (retry + 1), cancellationToken); // Exponential backoff
            }
            catch (Exception ex)
            {
                CalRemix.instance.Logger.Error($"All attempts failed: {ex.Message}");
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
        bool lockAcquired = false;
        try
        {
            lockAcquired = await _requestLock.WaitAsync(SemaphoreTimeout, cancellationToken);
            if (!lockAcquired)
            {
                CalRemix.instance.Logger.Error("Timed out waiting for request lock");
                return null;
            }

            var httpClient = new HttpClient();
            httpClient.Timeout = ConnectionTimeout;

            var youtube = new YoutubeClient(httpClient);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCts.CancelAfter(RequestTimeout);

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(
                youtubeUrl,
                linkedCts.Token
            ).ConfigureAwait(false);

            var muxedStream = streamManifest
                .GetMuxedStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (muxedStream != null)
            {
                CalRemix.instance.Logger.Info($"Found muxed stream: {muxedStream.VideoQuality.Label}");
                return muxedStream.Url;
            }

            var videoStream = streamManifest
                .GetVideoOnlyStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (videoStream != null)
            {
                CalRemix.instance.Logger.Warn("Only found video-only stream (no audio available)");
                return videoStream.Url;
            }

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
            // User requested cancellation - this is intentional, not an error
            throw new OperationCanceledException(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // Timeout - this IS an error
            CalRemix.instance.Logger.Error("YouTube request timed out (likely connection issue)");
            throw;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // User requested cancellation - rethrow without logging
            throw;
        }
        catch (OperationCanceledException)
        {
            // Timeout cancellation
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
    /// Process a URL asynchronously - handles YouTube URLs and local paths.
    /// For searches, use ProcessSearchAsync instead.
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
                    string directUrl = await GetDirectUrlFromYouTubeAsync(url, cts.Token)
                        .ConfigureAwait(false);

                    _activeRequests.TryRemove(requestId, out _);

                    Main.QueueMainThreadAction(() => onComplete?.Invoke(directUrl));
                }
                catch (OperationCanceledException)
                {
                    // This is intentional cancellation, not an error
                    CalRemix.instance.Logger.Info($"URL request {requestId} was cancelled by user");
                    _activeRequests.TryRemove(requestId, out _);
                    // Don't call onComplete - the operation was cancelled
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Error($"ProcessUrlAsync failed: {ex.Message}");
                    _activeRequests.TryRemove(requestId, out _);
                    Main.QueueMainThreadAction(() => onComplete?.Invoke(null));
                }
            }, cts.Token);
        }
        else
        {
            // Not a YouTube URL, return as-is (for local files)
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

public class UrlCachClearing : ModSystem
{
    public override void PostUpdateEverything()
    {
        if ((int)(Main.GlobalTimeWrappedHourly * 60) % 36000 == 0)
            VideoUrlHelper.CleanupCache();
    }
}