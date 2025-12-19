using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace CalRemix.Core.VideoPlayer.VideoUrlExtractors;

/// <summary>
/// Video URL extractor using YoutubeExplode.
/// Lightweight but doesn't support livestreams.
/// </summary>
public class YoutubeExplodeExtractor : IVideoUrlExtractor
{
    private YoutubeClient _youtube;

    public string Name => "YoutubeExplode";
    public bool SupportsLivestreams => false;
    public bool IsAvailable => _youtube != null;

    public Task<bool> InitializeAsync()
    {
        try
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            _youtube = new YoutubeClient(httpClient);

            CalRemix.instance.Logger.Info("YoutubeExplode initialized successfully");
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Failed to initialize YoutubeExplode: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public async Task<string> GetDirectUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable)
        {
            CalRemix.instance.Logger.Warn("YoutubeExplode not available");
            return null;
        }

        try
        {
            var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(url, cancellationToken);

            // Try muxed streams first (video + audio)
            var muxedStream = streamManifest
                .GetMuxedStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (muxedStream != null)
            {
                CalRemix.instance.Logger.Info($"YoutubeExplode found muxed stream: {muxedStream.VideoQuality.Label}");
                return muxedStream.Url;
            }

            // Fallback to video-only
            var videoStream = streamManifest
                .GetVideoOnlyStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            if (videoStream != null)
            {
                CalRemix.instance.Logger.Warn("YoutubeExplode: Only video-only stream available (no audio)");
                return videoStream.Url;
            }

            // Last resort: audio-only
            var audioStream = streamManifest
                .GetAudioOnlyStreams()
                .OrderByDescending(s => s.Bitrate)
                .FirstOrDefault();

            if (audioStream != null)
            {
                CalRemix.instance.Logger.Warn("YoutubeExplode: Only audio-only stream available (no video)");
                return audioStream.Url;
            }

            CalRemix.instance.Logger.Error("YoutubeExplode: No suitable streams found");
            return null;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("YoutubeExplode URL extraction cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YoutubeExplode URL extraction failed: {ex.Message}");
            return null;
        }
    }

    public async Task<string> SearchAsync(string searchQuery, int resultIndex, int maxResults, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable)
        {
            CalRemix.instance.Logger.Warn("YoutubeExplode not available");
            return null;
        }

        try
        {
            var searchClient = _youtube.Search;
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
                CalRemix.instance.Logger.Error("GetAsyncEnumerator not found");
                return null;
            }

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
                CalRemix.instance.Logger.Error("MoveNextAsync or Current not found");
                return null;
            }

            try
            {
                List<VideoSearchResult> results = new List<VideoSearchResult>();
                int fetchLimit = resultIndex >= 0 ? resultIndex + 1 : maxResults;

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
                    CalRemix.instance.Logger.Warn("YoutubeExplode search: No results found");
                    return null;
                }

                VideoSearchResult selectedVideo;
                if (resultIndex == -1)
                {
                    selectedVideo = results[Main.rand.Next(results.Count)];
                }
                else if (resultIndex >= results.Count)
                {
                    selectedVideo = results[results.Count - 1];
                }
                else
                {
                    selectedVideo = results[resultIndex];
                }

                string videoUrl = $"https://youtube.com/watch?v={selectedVideo.Id}";
                CalRemix.instance.Logger.Info($"YoutubeExplode search found: {selectedVideo.Title}");
                return videoUrl;
            }
            finally
            {
                // Dispose the enumerator
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
            CalRemix.instance.Logger.Info("YoutubeExplode search cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YoutubeExplode search failed: {ex.Message}");
            return null;
        }
    }

    public async Task<List<string>> GetPlaylistVideosAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        var videoUrls = new List<string>();

        if (!IsAvailable)
        {
            CalRemix.instance.Logger.Warn("YoutubeExplode not available");
            return videoUrls;
        }

        try
        {
            // Fallback to web scraping method
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

            string playlistUrl = $"https://www.youtube.com/playlist?list={playlistId}";
            string html = await httpClient.GetStringAsync(playlistUrl, cancellationToken);

            var videoIdMatches = System.Text.RegularExpressions.Regex.Matches(
                html,
                @"/watch\?v=([A-Za-z0-9_-]{11})"
            );

            foreach (System.Text.RegularExpressions.Match match in videoIdMatches)
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

            videoUrls = videoUrls.Distinct().ToList();
            CalRemix.instance.Logger.Info($"YoutubeExplode extracted {videoUrls.Count} videos from playlist");
            return videoUrls;
        }
        catch (OperationCanceledException)
        {
            CalRemix.instance.Logger.Info("YoutubeExplode playlist fetch cancelled");
            throw;
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"YoutubeExplode playlist fetch failed: {ex.Message}");
            return videoUrls;
        }
    }

    public Task<bool> IsLivestreamAsync(string url, CancellationToken cancellationToken = default)
    {
        // YoutubeExplode doesn't support livestreams
        return Task.FromResult(false);
    }
}

