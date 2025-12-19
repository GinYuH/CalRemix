using CalRemix.Core.VideoPlayer.VideoUrlExtractors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace CalRemix.Core.VideoPlayer;

/// <summary>
/// Video URL extractor using yt-dlp.
/// Supports livestreams and is highly reliable.
/// </summary>
public class YtDlpExtractor : IVideoUrlExtractor
{
    private YoutubeDL _ytdl;
    private string _ytdlpPath;
    private bool _isInitialized = false;

    // yt-dlp release info
    private const string YTDLP_DOWNLOAD_URL_WINDOWS = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";
    private const string YTDLP_DOWNLOAD_URL_LINUX = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp";
    private const string YTDLP_VERSION_CHECK_URL = "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest";

    public string Name => "yt-dlp";
    public bool SupportsLivestreams => true;
    public bool IsAvailable => _isInitialized && _ytdl != null;

    public YtDlpExtractor()
    {
        // Set up path for yt-dlp binary
        string ytdlpFolder = Path.Combine(ModLoader.ModPath, "..", "yt-dlp");
        Directory.CreateDirectory(ytdlpFolder);

        _ytdlpPath = Path.Combine(ytdlpFolder, GetBinaryName());
    }

    private static string GetBinaryName()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT
            ? "yt-dlp.exe"
            : "yt-dlp";
    }

    public async Task<bool> InitializeAsync()
    {
        if (_isInitialized)
            return true;

        try
        {
            // Check if yt-dlp binary exists
            if (!File.Exists(_ytdlpPath))
            {
                CalRemix.instance.Logger.Info("yt-dlp not found, downloading...");
                bool downloaded = await DownloadYtDlpAsync();
                if (!downloaded)
                {
                    CalRemix.instance.Logger.Error("Failed to download yt-dlp");
                    return false;
                }
            }

            // Make executable on Linux/Mac
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                try
                {
                    var process = new System.Diagnostics.Process
                    {
                        StartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "chmod",
                            Arguments = $"+x \"{_ytdlpPath}\"",
                            UseShellExecute = false
                        }
                    };
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Warn($"Could not make yt-dlp executable: {ex.Message}");
                }
            }

            // Initialize YoutubeDL instance
            _ytdl = new YoutubeDL();
            _ytdl.YoutubeDLPath = _ytdlpPath;
            _ytdl.OutputFolder = Path.GetTempPath();

            // Test if it works
            var version = await _ytdl.RunUpdate();
            CalRemix.instance.Logger.Info($"yt-dlp initialized successfully");

            _isInitialized = true;
            return true;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Failed to initialize yt-dlp: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> DownloadYtDlpAsync()
    {
        try
        {
            string downloadUrl = Environment.OSVersion.Platform == PlatformID.Win32NT
                ? YTDLP_DOWNLOAD_URL_WINDOWS
                : YTDLP_DOWNLOAD_URL_LINUX;

            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(5);

            CalRemix.instance.Logger.Info($"Downloading yt-dlp from {downloadUrl}");

            var response = await httpClient.GetAsync(downloadUrl);
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(_ytdlpPath, bytes);

            CalRemix.instance.Logger.Info($"yt-dlp downloaded successfully to {_ytdlpPath}");
            return true;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Failed to download yt-dlp: {ex.Message}");
            return false;
        }
    }

    public async Task<string> GetDirectUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable)
        {
            CalRemix.instance.Logger.Warn("yt-dlp not available for GetDirectUrlAsync");
            return null;
        }

        try
        {
            // Don't specify format - let yt-dlp choose the best available
            // This works better across different platforms
            var options = new OptionSet()
            {
                NoPlaylist = true
            };
            // Format is intentionally not set - yt-dlp will use default "bv*+ba/b"

            var result = await _ytdl.RunVideoDataFetch(url, ct: cancellationToken, overrideOptions: options);

            if (!result.Success || result.Data == null)
            {
                CalRemix.instance.Logger.Warn($"yt-dlp failed to extract URL: {string.Join(", ", result.ErrorOutput ?? new string[0])}");
                return null;
            }

            string directUrl = result.Data.Url;
            if (string.IsNullOrWhiteSpace(directUrl))
            {
                CalRemix.instance.Logger.Warn("yt-dlp returned empty URL");
                return null;
            }

            CalRemix.instance.Logger.Info($"yt-dlp extracted URL successfully");
            return directUrl;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("yt-dlp URL extraction cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"yt-dlp URL extraction failed: {ex.Message}");
            return null;
        }
    }

    public async Task<string> SearchAsync(string searchQuery, int resultIndex, int maxResults, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable)
        {
            CalRemix.instance.Logger.Warn("yt-dlp not available for SearchAsync");
            return null;
        }

        try
        {
            // Use yt-dlp to search YouTube
            var options = new OptionSet()
            {
                FlatPlaylist = true,
                SkipDownload = true
            };
            options.AddCustomOption("--get-id", "");

            // Search and get video IDs
            var result = await _ytdl.RunVideoPlaylistDownload(
                $"ytsearch{maxResults}:{searchQuery}",
                ct: cancellationToken,
                overrideOptions: options
            );

            if (!result.Success || result.Data == null || result.Data.Length == 0)
            {
                CalRemix.instance.Logger.Warn("yt-dlp search returned no results");
                return null;
            }

            var videoResults = result.Data;

            // Select based on index
            int selectedIndex;
            if (resultIndex == -1)
            {
                selectedIndex = Main.rand.Next(videoResults.Length);
            }
            else if (resultIndex >= videoResults.Length)
            {
                selectedIndex = videoResults.Length - 1;
            }
            else
            {
                selectedIndex = resultIndex;
            }

            string videoIdOrUrl = videoResults[selectedIndex];

            // Check if it's already a full URL or just an ID
            string videoUrl;
            if (videoIdOrUrl.StartsWith("http"))
            {
                // It's already a URL, extract the ID
                var match = System.Text.RegularExpressions.Regex.Match(videoIdOrUrl, @"[?&]v=([^&]+)");
                if (match.Success)
                {
                    videoUrl = $"https://youtube.com/watch?v={match.Groups[1].Value}";
                }
                else
                {
                    videoUrl = videoIdOrUrl; // Use as-is
                }
            }
            else
            {
                // It's just an ID
                videoUrl = $"https://youtube.com/watch?v={videoIdOrUrl}";
            }

            CalRemix.instance.Logger.Info($"yt-dlp search found video: {videoUrl}");
            return videoUrl;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("yt-dlp search cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"yt-dlp search failed: {ex.Message}");
            return null;
        }
    }

    public async Task<List<string>> GetPlaylistVideosAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable)
        {
            CalRemix.instance.Logger.Warn("yt-dlp not available for GetPlaylistVideosAsync");
            return new List<string>();
        }

        try
        {
            string playlistUrl = $"https://youtube.com/playlist?list={playlistId}";

            var options = new OptionSet()
            {
                FlatPlaylist = true,
                SkipDownload = true
            };
            options.AddCustomOption("--get-id", "");

            var result = await _ytdl.RunVideoPlaylistDownload(
                playlistUrl,
                ct: cancellationToken,
                overrideOptions: options
            );

            if (!result.Success || result.Data == null || result.Data.Length == 0)
            {
                CalRemix.instance.Logger.Warn("yt-dlp playlist fetch returned no results");
                return new List<string>();
            }

            var videoUrls = new List<string>();
            foreach (var item in result.Data)
            {
                string videoUrl;
                if (item.StartsWith("http"))
                {
                    // Already a URL, extract ID and normalize
                    var match = System.Text.RegularExpressions.Regex.Match(item, @"[?&]v=([^&]+)");
                    if (match.Success)
                    {
                        videoUrl = $"https://youtube.com/watch?v={match.Groups[1].Value}";
                    }
                    else
                    {
                        videoUrl = item;
                    }
                }
                else
                {
                    // Just an ID
                    videoUrl = $"https://youtube.com/watch?v={item}";
                }
                videoUrls.Add(videoUrl);
            }

            CalRemix.instance.Logger.Info($"yt-dlp extracted {videoUrls.Count} videos from playlist");
            return videoUrls;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("yt-dlp playlist fetch cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"yt-dlp playlist fetch failed: {ex.Message}");
            return new List<string>();
        }
    }

    public async Task<bool> IsLivestreamAsync(string url, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable)
            return false;

        try
        {
            var options = new OptionSet()
            {
                DumpSingleJson = true,
                NoPlaylist = true
            };

            var result = await _ytdl.RunVideoDataFetch(url, ct: cancellationToken, overrideOptions: options);

            if (!result.Success || result.Data == null)
                return false;

            // Check if it's a live stream
            bool isLive = result.Data.IsLive ?? false;
            return isLive;
        }
        catch
        {
            return false;
        }
    }
}