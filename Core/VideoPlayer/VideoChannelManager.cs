using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

/// <summary>
/// Manages shared video channels with spatial audio.
/// Each channel has ONE VideoPlayerCore that all TVs share (for synced video).
/// Volume is controlled by TVVolumeManager based on which TV is closest.
/// </summary>
public class VideoChannelManager : ModSystem
{
    public const int DEFAULT_CHANNEL = 0;
    public const int MIN_CHANNEL = 0;
    public const int MAX_CHANNEL = 99;
    public const int MAX_PRESET_CHANNEL = 11;
    public const int MIN_CUSTOM_CHANNEL = 12;

    #region Content Definitions

    public class ContentEntry
    {
        public string Query { get; set; }
        public int ResultIndex { get; set; } = -1;
        public int MaxResults { get; set; } = 10;

        public ContentEntry(string query, int resultIndex = -1, int maxResults = 10)
        {
            Query = query;
            ResultIndex = resultIndex;
            MaxResults = maxResults;
        }
    }

    public class ChannelContent
    {
        public List<ContentEntry> Entries { get; set; } = new List<ContentEntry>();
        public bool IsPlaylist { get; set; } = false;

        public ChannelContent(params ContentEntry[] entries)
        {
            Entries = new List<ContentEntry>(entries);
        }

        public ChannelContent(string query, int resultIndex = -1, int maxResults = 10)
        {
            Entries.Add(new ContentEntry(query, resultIndex, maxResults));
        }
    }

    private static readonly Dictionary<int, ChannelContent> PresetChannelContent = new()
    {
        { 0, new ChannelContent(new ContentEntry("https://youtube.com/playlist?list=PLbrAnF1cQ0SCKw3yfO2SzJ1g5DaxXOYkv")) { IsPlaylist = true } },
        { 1, new ChannelContent(
            new ContentEntry("minecraft gameplay", -1, 15),
            new ContentEntry("minecraft building", -1, 15),
            new ContentEntry("minecraft funny moments", -1, 15))
        },
        { 2, new ChannelContent(
            new ContentEntry("cute animals", -1, 20),
            new ContentEntry("baby animals", -1, 20),
            new ContentEntry("funny cats", -1, 20))
        },
        { 3, new ChannelContent(
            new ContentEntry("music video", -1, 15),
            new ContentEntry("concert performance", -1, 15))
        },
        { 4, new ChannelContent(
            new ContentEntry("funny videos", -1, 20),
            new ContentEntry("comedy sketches", -1, 15))
        },
        { 5, new ChannelContent(
            new ContentEntry("science experiment", -1, 15),
            new ContentEntry("educational documentary", -1, 10))
        },
        { 6, new ChannelContent(
            new ContentEntry("sports highlights", -1, 15),
            new ContentEntry("amazing sports moments", -1, 15))
        },
        { 7, new ChannelContent(
            new ContentEntry("movie trailer", -1, 15),
            new ContentEntry("film clips", -1, 15))
        },
        { 8, new ChannelContent(
            new ContentEntry("DM Dokuro", -1, 15),
            new ContentEntry("dm dokuro skullposts", -1, 15),
            new ContentEntry("DM Dokuro Ultrakill", -1, 15))
        },
        { 9, new ChannelContent(new ContentEntry("")) },
        { 10, new ChannelContent(new ContentEntry("")) },
        { 11, new ChannelContent(new ContentEntry("")) },
    };

    #endregion

    // Shared VideoPlayerCore instances (one per channel)
    private readonly Dictionary<int, VideoPlayerCore> _channelPlayers = new();

    // Track which entries were last used (to avoid repeats)
    private readonly Dictionary<int, ContentEntry> _lastPlayedEntry = new();

    public bool IsPresetChannel(int channelId) =>
        channelId >= MIN_CHANNEL && channelId <= MAX_PRESET_CHANNEL;

    public bool IsCustomChannel(int channelId) =>
        channelId >= MIN_CUSTOM_CHANNEL && channelId <= MAX_CHANNEL;

    /// <summary>
    /// Get the shared player for a channel. Returns null if channel doesn't exist.
    /// </summary>
    public VideoPlayerCore GetChannelPlayer(int channelId)
    {
        if (channelId < MIN_CHANNEL || channelId > MAX_CHANNEL)
            return null;

        return _channelPlayers.TryGetValue(channelId, out var player) ? player : null;
    }

    /// <summary>
    /// Get or create the shared player for a channel.
    /// </summary>
    public VideoPlayerCore GetOrCreateChannelPlayer(int channelId)
    {
        if (channelId < MIN_CHANNEL || channelId > MAX_CHANNEL)
        {
            CalRemix.instance.Logger.Error($"Invalid channel ID: {channelId}");
            return null;
        }

        if (!_channelPlayers.TryGetValue(channelId, out var player))
        {
            player = new VideoPlayerCore(1280, 720);
            _channelPlayers[channelId] = player;

            // Set up auto-repeat for preset channels
            if (IsPresetChannel(channelId))
            {
                player.PlaybackEnded += (sender, e) => OnChannelEnded(channelId);
            }

            CalRemix.instance.Logger.Info($"Created shared player for channel {channelId}");
        }

        return player;
    }

    /// <summary>
    /// Start a preset channel (if not already playing).
    /// </summary>
    public void StartChannel(int channelId)
    {
        if (!IsPresetChannel(channelId))
        {
            CalRemix.instance.Logger.Error($"Channel {channelId} is not a preset channel");
            return;
        }

        var player = GetChannelPlayer(channelId);

        // If already playing, don't restart
        if (player != null && (player.IsPlaying || player.IsLoading || player.IsPreparing))
        {
            return;
        }

        // Get or create the player
        player = GetOrCreateChannelPlayer(channelId);
        if (player == null) return;

        if (!PresetChannelContent.TryGetValue(channelId, out var channelContent))
        {
            CalRemix.instance.Logger.Warn($"Preset channel {channelId} has no content");
            return;
        }

        PlayChannelContent(channelId, channelContent, player);
    }

    /// <summary>
    /// Play content on a channel.
    /// </summary>
    private void PlayChannelContent(int channelId, ChannelContent channelContent, VideoPlayerCore player)
    {
        if (channelContent.Entries.Count == 0)
        {
            CalRemix.instance.Logger.Warn($"Channel {channelId} has no entries");
            return;
        }

        ContentEntry entry;
        if (channelContent.Entries.Count == 1)
        {
            entry = channelContent.Entries[0];
        }
        else
        {
            ContentEntry lastEntry = _lastPlayedEntry.ContainsKey(channelId)
                ? _lastPlayedEntry[channelId]
                : null;

            if (channelContent.Entries.Count == 2)
            {
                entry = channelContent.Entries[0] == lastEntry
                    ? channelContent.Entries[1]
                    : channelContent.Entries[0];
            }
            else
            {
                do
                {
                    entry = channelContent.Entries[Main.rand.Next(channelContent.Entries.Count)];
                } while (entry == lastEntry && channelContent.Entries.Count > 1);
            }

            _lastPlayedEntry[channelId] = entry;
        }

        PlayEntry(entry, player, channelContent.IsPlaylist);
    }

    /// <summary>
    /// Play a content entry.
    /// </summary>
    private static void PlayEntry(ContentEntry entry, VideoPlayerCore player, bool isPlaylist)
    {
        string query = entry.Query;

        if (VideoPlayerCore.IsFilePath(query) ||
            VideoUrlHelper.IsYouTubeUrl(query) ||
            VideoPlayerCore.IsMediaLink(query))
        {
            player.Play(query, forcePlay: true);
        }
        else if (VideoUrlHelper.IsYouTubePlaylist(query))
        {
            PlayPlaylist(query, entry, player);
        }
        else
        {
            player.PlayWithSearchParams(query, entry.ResultIndex, entry.MaxResults, forcePlay: true);
        }
    }

    /// <summary>
    /// Play a YouTube playlist.
    /// </summary>
    private static void PlayPlaylist(string playlistUrl, ContentEntry entry, VideoPlayerCore player)
    {
        string playlistId = VideoUrlHelper.ExtractPlaylistId(playlistUrl);
        if (string.IsNullOrEmpty(playlistId))
        {
            CalRemix.instance.Logger.Error("Failed to extract playlist ID");
            return;
        }

        Task.Run(async () =>
        {
            try
            {
                var cts = new CancellationTokenSource();
                List<string> videoUrls = await VideoUrlHelper.GetPlaylistVideosAsync(playlistId, cts.Token);

                Main.QueueMainThreadAction(() =>
                {
                    if (videoUrls.Count == 0)
                    {
                        CalRemix.instance.Logger.Error("No videos found in playlist");
                        return;
                    }

                    int startIndex = entry.ResultIndex == -1
                        ? Main.rand.Next(videoUrls.Count)
                        : Math.Clamp(entry.ResultIndex, 0, videoUrls.Count - 1);

                    List<string> reorderedVideos = new List<string>();
                    for (int i = startIndex; i < videoUrls.Count; i++)
                        reorderedVideos.Add(videoUrls[i]);
                    for (int i = 0; i < startIndex; i++)
                        reorderedVideos.Add(videoUrls[i]);

                    player.PlayPlaylistFromUrls(reorderedVideos, forcePlay: true);
                    CalRemix.instance.Logger.Info($"Loaded playlist with {videoUrls.Count} videos (starting at #{startIndex + 1})");
                });
            }
            catch (Exception ex)
            {
                CalRemix.instance.Logger.Error($"Playlist loading failed: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Handle channel ending (for auto-repeat).
    /// </summary>
    private void OnChannelEnded(int channelId)
    {
        if (!PresetChannelContent.TryGetValue(channelId, out var content))
            return;

        var player = GetChannelPlayer(channelId);
        if (player == null) return;

        // If it was a playlist, it will auto-advance
        // Otherwise, play next content
        if (!content.IsPlaylist)
        {
            Main.QueueMainThreadAction(() =>
            {
                PlayChannelContent(channelId, content, player);
            });
        }
    }

    /// <summary>
    /// Stop a channel if no TVs are watching it.
    /// </summary>
    public void StopChannelIfUnused(int channelId)
    {
        // Count how many TVs are currently watching this channel
        int activeTVCount = 0;

        foreach (var kvp in Terraria.DataStructures.TileEntity.ByID)
        {
            if (kvp.Value is Content.Tiles.TVs.TVTileEntity tvEntity &&
                tvEntity.CurrentChannel == channelId &&
                tvEntity.IsOn)
            {
                activeTVCount++;
            }
        }

        // Only stop if no TVs are watching
        if (activeTVCount == 0)
        {
            var player = GetChannelPlayer(channelId);
            if (player != null)
            {
                // Only stop if actually playing something
                if (player.IsPlaying || player.IsLoading || player.IsPreparing)
                {
                    player.Stop();
                    CalRemix.instance.Logger.Info($"Stopped unused channel {channelId}");
                }
            }
        }
        else
        {
            CalRemix.instance.Logger.Debug($"Channel {channelId} still has {activeTVCount} active TV(s)");
        }
    }

    #region ModSystem Overrides

    public override void PostUpdateEverything()
    {
        // Update all active channel players
        foreach (var kvp in _channelPlayers)
        {
            var player = kvp.Value;
            if (player.IsPlaying || player.IsLoading || player.IsPreparing)
            {
                player.Update(Main.gameTimeCache);
            }
        }
    }

    public override void OnWorldLoad()
    {
        _lastPlayedEntry.Clear();
        CalRemix.instance.Logger.Info("VideoChannelManager: World loaded");
    }

    public override void OnWorldUnload()
    {
        // Dispose all channel players
        foreach (var kvp in _channelPlayers)
        {
            kvp.Value.Dispose();
        }

        _channelPlayers.Clear();
        _lastPlayedEntry.Clear();
        CalRemix.instance.Logger.Info("VideoChannelManager: All channels disposed");
    }

    #endregion
}