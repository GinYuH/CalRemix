using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CalRemix.Core.VideoPlayer.VideoUrlExtractors;

/// <summary>
/// Interface for video URL extraction from various sources.
/// Allows multiple implementations (yt-dlp, YoutubeExplode, etc.) with fallback support.
/// </summary>
public interface IVideoUrlExtractor
{
    /// <summary>
    /// Name of this extractor (for logging/debugging).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Whether this extractor supports YouTube livestreams.
    /// </summary>
    bool SupportsLivestreams { get; }

    /// <summary>
    /// Whether this extractor is currently available/working.
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// Initialize the extractor (download binaries, check dependencies, etc.).
    /// </summary>
    Task<bool> InitializeAsync();

    /// <summary>
    /// Extract direct stream URL from a YouTube video URL.
    /// </summary>
    /// <param name="url">YouTube video URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Direct stream URL, or null if extraction fails</returns>
    Task<string> GetDirectUrlAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search YouTube and return the URL of a specific result.
    /// </summary>
    /// <param name="searchQuery">Search query</param>
    /// <param name="resultIndex">Which result to return (0-based, -1 for random)</param>
    /// <param name="maxResults">Maximum results to fetch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>YouTube video URL, or null if no results</returns>
    Task<string> SearchAsync(string searchQuery, int resultIndex, int maxResults, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all video URLs from a YouTube playlist.
    /// </summary>
    /// <param name="playlistId">YouTube playlist ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of video URLs</returns>
    Task<List<string>> GetPlaylistVideosAsync(string playlistId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a URL is a YouTube livestream.
    /// </summary>
    /// <param name="url">YouTube URL to check</param>
    /// <returns>True if the URL is a livestream</returns>
    Task<bool> IsLivestreamAsync(string url, CancellationToken cancellationToken = default);
}

