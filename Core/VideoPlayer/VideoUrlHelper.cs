using CalRemix.Core.VideoPlayer.VideoUrlExtractors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using YoutubeExplode.Common;

namespace CalRemix.Core.VideoPlayer;

public static class VideoUrlHelper
{
    private static readonly ConcurrentDictionary<string, (string Url, DateTime Timestamp, bool IsLivestream)> _urlCache = new();
    private static readonly ConcurrentDictionary<Guid, CancellationTokenSource> _activeRequests = new();
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan LivestreamCacheDuration = TimeSpan.FromMinutes(2); // Livestream URLs expire quickly
    private static readonly SemaphoreSlim _requestLock = new(1, 1);
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(45);
    private static readonly TimeSpan SemaphoreTimeout = TimeSpan.FromSeconds(30);

    // Hybrid extractor (yt-dlp + YoutubeExplode)
    private static HybridVideoExtractor _extractor;
    private static bool _isInitialized = false;

    /// <summary>
    /// Initialize the video extractor system.
    /// Call this during mod load.
    /// </summary>
    public static async Task InitializeAsync()
    {
        if (_isInitialized)
            return;

        CalRemix.instance.Logger.Info("Initializing video URL extraction system...");

        _extractor = new HybridVideoExtractor();
        bool success = await _extractor.InitializeAsync();

        if (!success)
        {
            CalRemix.instance.Logger.Error("Failed to initialize video extraction system!");
        }
        else
        {
            CalRemix.instance.Logger.Info($"Video extraction system ready: {(_extractor as HybridVideoExtractor)?.GetActiveExtractor()}");
            CalRemix.instance.Logger.Info($"Livestream support: {(_extractor.SupportsLivestreams ? "ENABLED" : "DISABLED")}");
        }

        _isInitialized = success;
    }

    /// <summary>
    /// Check if the extractor is ready.
    /// </summary>
    public static bool IsReady => _isInitialized && _extractor != null && _extractor.IsAvailable;

    /// <summary>
    /// Check if livestreams are supported.
    /// </summary>
    public static bool SupportsLivestreams => IsReady && _extractor.SupportsLivestreams;

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
    /// Check if a URL is from a supported video platform.
    /// This includes YouTube, Twitch, Vimeo, and many others supported by yt-dlp.
    /// </summary>
    public static bool IsSupportedVideoUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        string[] supportedDomains = new[]
        {
            // Most reliable
            "youtube.com", "youtu.be",
            "twitch.tv",
            "vimeo.com",
            
            // Sometimes work (depends on video)
            "twitter.com", "x.com",
            "tiktok.com",
            "reddit.com",
            "dailymotion.com",
            "facebook.com", "fb.watch",
            
            // Direct video files
            ".mp4", ".webm", ".mkv", ".avi", ".mov", ".flv", ".m3u8"
        };

        string lowerUrl = url.ToLower();
        return supportedDomains.Any(domain => lowerUrl.Contains(domain));
    }

    /// <summary>
    /// Extract playlist ID from a YouTube URL.
    /// </summary>
    public static string ExtractPlaylistId(string url)
    {
        try
        {
            int listIndex = url.IndexOf("list=");
            if (listIndex == -1)
                return null;

            string afterList = url.Substring(listIndex + 5);
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
    /// Clean up expired cache entries.
    /// </summary>
    public static void CleanupCache()
    {
        var now = DateTime.Now;
        var keysToRemove = new List<string>();

        foreach (var kvp in _urlCache)
        {
            var cacheDuration = kvp.Value.IsLivestream ? LivestreamCacheDuration : CacheDuration;
            if (now - kvp.Value.Timestamp >= cacheDuration)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

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
    /// Get direct stream URL from YouTube URL.
    /// </summary>
    public static async Task<string> GetDirectUrlFromYouTubeAsync(string youtubeUrl, CancellationToken cancellationToken = default)
    {
        if (!IsReady)
        {
            CalRemix.instance.Logger.Error("Video extraction system not ready");
            return null;
        }

        // Check cache first (but never cache livestreams - their URLs expire too quickly)
        if (_urlCache.TryGetValue(youtubeUrl, out var cached))
        {
            // Skip cache for livestreams - always fetch fresh
            if (cached.IsLivestream)
            {
                _urlCache.TryRemove(youtubeUrl, out _);
                CalRemix.instance.Logger.Debug("Livestream detected in cache - fetching fresh URL");
            }
            else if (DateTime.Now - cached.Timestamp < CacheDuration)
            {
                CalRemix.instance.Logger.Debug("Using cached URL for regular video");
                return cached.Url;
            }
            else
            {
                // Expired, remove from cache
                _urlCache.TryRemove(youtubeUrl, out _);
                CalRemix.instance.Logger.Debug("Cached URL expired, fetching new one");
            }
        }

        bool lockAcquired = false;
        try
        {
            lockAcquired = await _requestLock.WaitAsync(SemaphoreTimeout, cancellationToken);
            if (!lockAcquired)
            {
                CalRemix.instance.Logger.Error("Timed out waiting for request lock");
                return null;
            }

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCts.CancelAfter(RequestTimeout);

            // Check if it's a livestream first
            bool isLivestream = false;
            try
            {
                isLivestream = await _extractor.IsLivestreamAsync(youtubeUrl, linkedCts.Token);
                if (isLivestream)
                {
                    CalRemix.instance.Logger.Info("Detected livestream - using short cache duration");
                }
            }
            catch
            {
                // If check fails, assume not a livestream
            }

            string directUrl = await _extractor.GetDirectUrlAsync(youtubeUrl, linkedCts.Token);

            // Cache the result - but NOT for livestreams (their URLs expire too quickly)
            if (directUrl != null)
            {
                if (isLivestream)
                {
                    CalRemix.instance.Logger.Debug("Livestream URL not cached (expires too quickly)");
                    // Don't cache livestreams at all
                }
                else
                {
                    _urlCache[youtubeUrl] = (directUrl, DateTime.Now, isLivestream);
                    CalRemix.instance.Logger.Debug("Cached URL for regular video");
                }
            }

            return directUrl;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            CalRemix.instance.Logger.Info("YouTube URL extraction cancelled by user");
            throw;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Error("YouTube URL extraction timed out");
            return null;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YouTube URL extraction failed: {ex.Message}");
            return null;
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
    /// Search YouTube and return video URL.
    /// </summary>
    public static async Task<string> SearchYouTubeAsync(string searchQuery, int resultIndex = 0, int maxResults = 10, CancellationToken cancellationToken = default)
    {
        if (!IsReady)
        {
            CalRemix.instance.Logger.Error("Video extraction system not ready");
            return null;
        }

        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            CalRemix.instance.Logger.Error("Empty search query");
            return null;
        }

        try
        {
            return await _extractor.SearchAsync(searchQuery, resultIndex, maxResults, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("YouTube search cancelled");
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
    /// </summary>
    public static async Task<List<string>> GetPlaylistVideosAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        if (!IsReady)
        {
            CalRemix.instance.Logger.Error("Video extraction system not ready");
            return new List<string>();
        }

        if (string.IsNullOrWhiteSpace(playlistId))
        {
            CalRemix.instance.Logger.Error("Empty playlist ID");
            return new List<string>();
        }

        try
        {
            return await _extractor.GetPlaylistVideosAsync(playlistId, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("Playlist fetch cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Playlist fetch failed: {ex.Message}");
            return new List<string>();
        }
    }

    /// <summary>
    /// Check if a URL is a livestream.
    /// </summary>
    public static async Task<bool> IsLivestreamAsync(string url, CancellationToken cancellationToken = default)
    {
        if (!IsReady)
            return false;

        try
        {
            return await _extractor.IsLivestreamAsync(url, cancellationToken);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Process a URL asynchronously.
    /// Supports YouTube, Twitch, Vimeo, and many other platforms via yt-dlp.
    /// </summary>
    public static void ProcessUrlAsync(string url, Action<string> onComplete, Guid requestId)
    {
        if (IsSupportedVideoUrl(url))
        {
            // Determine the platform for better user feedback
            string platform = "video";
            if (IsYouTubeUrl(url))
                platform = "YouTube";
            else if (url.Contains("twitch.tv"))
                platform = "Twitch";
            else if (url.Contains("vimeo.com"))
                platform = "Vimeo";
            else if (url.Contains("tiktok.com"))
                platform = "TikTok";
            else if (url.Contains("twitter.com") || url.Contains("x.com"))
                platform = "Twitter";
            else if (url.Contains("reddit.com"))
                platform = "Reddit";

            Main.NewText($"Extracting {platform} stream URL...", Color.LightBlue);

            var cts = new CancellationTokenSource();
            _activeRequests[requestId] = cts;

            Task.Run(async () =>
            {
                try
                {
                    string directUrl = await GetDirectUrlFromYouTubeAsync(url, cts.Token);
                    _activeRequests.TryRemove(requestId, out _);
                    Main.QueueMainThreadAction(() => onComplete?.Invoke(directUrl));
                }
                catch (OperationCanceledException)
                {
                    CalRemix.instance.Logger.Info($"URL request {requestId} was cancelled by user");
                    _activeRequests.TryRemove(requestId, out _);
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
            // Direct URL pass-through (like direct .mp4 links)
            onComplete?.Invoke(url);
        }
    }

    /// <summary>
    /// Process a search query asynchronously.
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
                string videoUrl = await SearchYouTubeAsync(searchQuery, resultIndex, maxResults, cts.Token);

                if (videoUrl == null)
                {
                    if (!cts.Token.IsCancellationRequested)
                    {
                        Main.QueueMainThreadAction(() =>
                        {
                            Main.NewText("No search results found!", Color.Orange);
                        });
                    }
                    _activeRequests.TryRemove(requestId, out _);
                    Main.QueueMainThreadAction(() => onComplete?.Invoke(null));
                    return;
                }

                Main.QueueMainThreadAction(() =>
                {
                    Main.NewText("Found video, extracting stream...", Color.LightGreen);
                });

                string directUrl = await GetDirectUrlFromYouTubeAsync(videoUrl, cts.Token);
                _activeRequests.TryRemove(requestId, out _);
                Main.QueueMainThreadAction(() => onComplete?.Invoke(directUrl));
            }
            catch (OperationCanceledException)
            {
                CalRemix.instance.Logger.Info($"Search request {requestId} was cancelled by user");
                _activeRequests.TryRemove(requestId, out _);
            }
            catch (Exception ex)
            {
                CalRemix.instance.Logger.Error($"Search processing failed: {ex.Message}");
                _activeRequests.TryRemove(requestId, out _);
                Main.QueueMainThreadAction(() => onComplete?.Invoke(null));
            }
        }, cts.Token);
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

public class VideoUrlHelperSystem : ModSystem
{
    public override void OnModLoad()
    {
        Task.Run(async () =>
        {
            await VideoUrlHelper.InitializeAsync();
        });
    }

    public override void PostUpdateEverything()
    {
        if ((int)(Main.GlobalTimeWrappedHourly * 60) % 36000 == 0)
            VideoUrlHelper.CleanupCache();
    }

    public override void OnModUnload()
    {
        VideoUrlHelper.ClearCache();
    }
}