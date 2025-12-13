using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using YoutubeExplode;

namespace CalRemix.UI.VideoPlayer;

/// <summary>
/// Helper class for handling video URLs, including YouTube.
/// Uses bundled yt-dlp.exe for YouTube support.
/// </summary>
/// <summary>
/// Helper class for handling video URLs, including YouTube.
/// Uses bundled yt-dlp.exe for YouTube support with async loading.
/// </summary>

public static class VideoUrlHelper
{
    private static YoutubeClient _youtubeClient;

    /// <summary>
    /// Get or create the YouTube client instance.
    /// </summary>
    private static YoutubeClient GetYoutubeClient()
    {
        if (_youtubeClient == null)
        {
            _youtubeClient = new YoutubeClient();
        }
        return _youtubeClient;
    }

    /// <summary>
    /// Check if a URL is a YouTube link.
    /// </summary>
    public static bool IsYouTubeUrl(string url)
    {
        return url.Contains("youtube.com") || url.Contains("youtu.be");
    }

    /// <summary>
    /// Get direct stream URL from YouTube using YoutubeExplode.
    /// Returns null if extraction fails.
    /// </summary>
    public static async Task<string> GetDirectUrlFromYouTubeAsync(string youtubeUrl)
    {
        try
        {
            var youtube = GetYoutubeClient();

            // Get stream manifest
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(youtubeUrl);

            // Try to get a muxed stream (video + audio in one)
            // Prefer highest quality muxed stream
            var muxedStream = streamManifest
                .GetMuxedStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (muxedStream != null)
            {
                CalRemix.instance.Logger.Info($"Found muxed stream: {muxedStream.VideoQuality.Label} @ {muxedStream.Bitrate}");
                return muxedStream.Url;
            }

            // Fallback: Get separate video and audio streams
            // NOTE: LibVLC can't play these directly - you'd need to mux them
            // For now, we'll just try to find the best video-only stream with decent quality
            var videoStream = streamManifest
                .GetVideoOnlyStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (videoStream != null)
            {
                CalRemix.instance.Logger.Warn("Only found video-only stream (no audio available)");
                CalRemix.instance.Logger.Info($"Using video-only stream: {videoStream.VideoQuality.Label}");
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
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YouTube URL extraction failed: {ex.Message}");
            CalRemix.instance.Logger.Error($"Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    /// <summary>
    /// Process a URL asynchronously - if it's YouTube, extract direct URL, otherwise return as-is.
    /// Callback is invoked on completion with the result URL (or null on failure).
    /// </summary>
    public static void ProcessUrlAsync(string url, Action<string> onComplete)
    {
        if (IsYouTubeUrl(url))
        {
            Main.NewText("Extracting YouTube stream URL (this may take a few seconds)...", Color.LightBlue);

            // Run extraction on background thread
            Task.Run(async () =>
            {
                string directUrl = await GetDirectUrlFromYouTubeAsync(url);

                // Invoke callback
                onComplete?.Invoke(directUrl);
            });
        }
        else
        {
            // Not a YouTube URL, return immediately
            onComplete?.Invoke(url);
        }
    }
}
