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
        public List<ContentEntry> Entries { get; set; } = [];
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

    /// <summary>
    /// Defines how often a special event can repeat.
    /// </summary>
    public enum RepeatMode
    {
        /// <summary>Can trigger every time condition is met (useful for repeating events)</summary>
        AlwaysAvailable,
        /// <summary>Can trigger once per real-world hour</summary>
        OncePerHour,
        /// <summary>Can trigger once per game session (until game closes)</summary>
        OncePerSession,
        /// <summary>Can trigger once per world (resets when world is unloaded)</summary>
        OncePerWorld,
        /// <summary>Can trigger once per player character (stored in player data)</summary>
        OncePerPlayer,
        /// <summary>Can trigger once per Steam/account (stored in mod config)</summary>
        OncePerAccount,
        /// <summary>Can only trigger once ever, then never again</summary>
        NoRepeat
    }

    /// <summary>
    /// Defines a special event video that can override all TVs when triggered.
    /// </summary>
    public class SpecialEventVideo
    {
        public string VideoUrl { get; set; }
        public Func<bool> TriggerCondition { get; set; }
        public RepeatMode RepeatBehavior { get; set; }
        public int Priority { get; set; } // Higher priority plays first if multiple conditions are true

        // Tracking data
        internal bool HasPlayedThisSession { get; set; } = false;
        internal bool HasPlayedThisWorld { get; set; } = false;
        internal DateTime LastPlayedTime { get; set; } = DateTime.MinValue;
        internal HashSet<string> PlayersWhoHaveSeen { get; set; } = [];

        public SpecialEventVideo(string videoUrl, Func<bool> triggerCondition, RepeatMode repeatBehavior = RepeatMode.OncePerSession, int priority = 0)
        {
            VideoUrl = videoUrl;
            TriggerCondition = triggerCondition;
            RepeatBehavior = repeatBehavior;
            Priority = priority;
        }

        /// <summary>
        /// Check if this event can play based on its repeat mode.
        /// </summary>
        public bool CanPlay(string currentPlayerName = null)
        {
            switch (RepeatBehavior)
            {
                case RepeatMode.AlwaysAvailable:
                    return true;

                case RepeatMode.OncePerHour:
                    return (DateTime.Now - LastPlayedTime).TotalHours >= 1.0;

                case RepeatMode.OncePerSession:
                    return !HasPlayedThisSession;

                case RepeatMode.OncePerWorld:
                    return !HasPlayedThisWorld;

                case RepeatMode.OncePerPlayer:
                    if (string.IsNullOrEmpty(currentPlayerName))
                        return false;
                    return !PlayersWhoHaveSeen.Contains(currentPlayerName);

                case RepeatMode.OncePerAccount:
                    // TODO: Implement persistent storage via ModConfig
                    return !HasPlayedThisSession; // Fallback to session for now

                case RepeatMode.NoRepeat:
                    return !HasPlayedThisSession && !HasPlayedThisWorld;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Mark this event as played.
        /// </summary>
        public void MarkAsPlayed(string currentPlayerName = null)
        {
            LastPlayedTime = DateTime.Now;

            switch (RepeatBehavior)
            {
                case RepeatMode.AlwaysAvailable:
                    // No tracking needed
                    break;

                case RepeatMode.OncePerHour:
                    // Time already tracked
                    break;

                case RepeatMode.OncePerSession:
                    HasPlayedThisSession = true;
                    break;

                case RepeatMode.OncePerWorld:
                    HasPlayedThisWorld = true;
                    break;

                case RepeatMode.OncePerPlayer:
                    if (!string.IsNullOrEmpty(currentPlayerName))
                        PlayersWhoHaveSeen.Add(currentPlayerName);
                    break;

                case RepeatMode.OncePerAccount:
                    HasPlayedThisSession = true;
                    // TODO: Save to ModConfig
                    break;

                case RepeatMode.NoRepeat:
                    HasPlayedThisSession = true;
                    HasPlayedThisWorld = true;
                    break;
            }
        }
    }

    private static readonly List<(int ChannelId, ChannelContent Content)> PresetChannels = [];
    private static readonly List<SpecialEventVideo> SpecialEventVideos = [];

    #endregion

    // Shared VideoPlayerCore instances (one per channel + special override channel)
    private readonly Dictionary<int, VideoPlayerCore> _channelPlayers = [];

    // Special override channel player (separate from regular channels)
    private VideoPlayerCore _overrideChannelPlayer = null;

    // Track which entries were last used (to avoid repeats)
    private readonly Dictionary<int, ContentEntry> _lastPlayedEntry = [];

    // Track if special override channel is currently active
    private bool _isOverrideChannelActive = false;
    private SpecialEventVideo _currentSpecialEvent = null;

    #region Initialization

    public override void Load()
    {
        PresetChannels.Clear();
        SpecialEventVideos.Clear();

        InitializeDefaultChannels();
        InitializeDefaultSpecialEvents();
    }

    private static void InitializeDefaultChannels()
    {
        AddPresetChannel(0, new ChannelContent(
            new ContentEntry("https://youtube.com/playlist?list=PLbrAnF1cQ0SCKw3yfO2SzJ1g5DaxXOYkv")
        )
        { IsPlaylist = true });

        AddPresetChannel(1, new ChannelContent(
            new ContentEntry("minecraft gameplay", -1, 15),
            new ContentEntry("minecraft building", -1, 15),
            new ContentEntry("minecraft funny moments", -1, 15)
        ));

        AddPresetChannel(2, new ChannelContent(
            new ContentEntry("cute animals", -1, 20),
            new ContentEntry("baby animals", -1, 20),
            new ContentEntry("funny cats", -1, 20)
        ));

        AddPresetChannel(3, new ChannelContent(
            new ContentEntry("music video", -1, 15),
            new ContentEntry("concert performance", -1, 15)
        ));

        AddPresetChannel(4, new ChannelContent(
            new ContentEntry("funny videos", -1, 20),
            new ContentEntry("comedy sketches", -1, 15)
        ));

        AddPresetChannel(5, new ChannelContent(
            new ContentEntry("science experiment", -1, 15),
            new ContentEntry("educational documentary", -1, 10)
        ));

        AddPresetChannel(6, new ChannelContent(
            new ContentEntry("sports highlights", -1, 15),
            new ContentEntry("amazing sports moments", -1, 15)
        ));

        AddPresetChannel(7, new ChannelContent(
            new ContentEntry("movie trailer", -1, 15),
            new ContentEntry("film clips", -1, 15)
        ));

        AddPresetChannel(8, new ChannelContent(
            new ContentEntry("DM Dokuro", -1, 15),
            new ContentEntry("dm dokuro skullposts", -1, 15),
            new ContentEntry("DM Dokuro Ultrakill", -1, 15)
        ));

        AddPresetChannel(9, new ChannelContent(new ContentEntry("")));
        AddPresetChannel(10, new ChannelContent(new ContentEntry("")));
        AddPresetChannel(11, new ChannelContent(new ContentEntry("")));
    }

    private static void InitializeDefaultSpecialEvents()
    {
        AddSpecialEventVideo(
            "https://www.youtube.com/watch?v=FkcDoUE8_Ec",
            () => NPC.AnyNPCs(Terraria.ID.NPCID.MoonLordCore),
            RepeatMode.OncePerWorld,
            priority: 10
        );

        AddSpecialEventVideo(
            "https://www.youtube.com/watch?v=suiXOsHaY4E",
            () => Main.bloodMoon,
            RepeatMode.OncePerHour,
            priority: 5
        );
    }

    /// <summary>
    /// Add a preset channel. Can be called by other mods via mod call.
    /// </summary>
    public static void AddPresetChannel(int channelId, ChannelContent content)
    {
        if (channelId < MIN_CHANNEL || channelId > MAX_PRESET_CHANNEL)
        {
            ModContent.GetInstance<CalRemix>().Logger.Error($"Invalid preset channel ID: {channelId}. Must be between {MIN_CHANNEL} and {MAX_PRESET_CHANNEL}");
            return;
        }

        PresetChannels.RemoveAll(c => c.ChannelId == channelId);

        PresetChannels.Add((channelId, content));
        ModContent.GetInstance<CalRemix>().Logger.Info($"Added preset channel {channelId}");
    }

    /// <summary>
    /// Add a special event video. Can be called by other mods via mod call.
    /// </summary>
    public static void AddSpecialEventVideo(string videoUrl, Func<bool> triggerCondition, RepeatMode repeatBehavior = RepeatMode.OncePerSession, int priority = 0)
    {
        var specialEvent = new SpecialEventVideo(videoUrl, triggerCondition, repeatBehavior, priority);
        SpecialEventVideos.Add(specialEvent);
        ModContent.GetInstance<CalRemix>().Logger.Info($"Added special event video with priority {priority} and repeat mode {repeatBehavior}");
    }

    #endregion

    #region Channel Management

    public static bool IsPresetChannel(int channelId) =>
        channelId >= MIN_CHANNEL && channelId <= MAX_PRESET_CHANNEL;

    public static bool IsCustomChannel(int channelId) =>
        channelId >= MIN_CUSTOM_CHANNEL && channelId <= MAX_CHANNEL;

    public bool IsOverrideChannelActive() => _isOverrideChannelActive;

    /// <summary>
    /// Get the shared player for a channel. Returns null if channel doesn't exist.
    /// If override channel is active, returns that player instead.
    /// </summary>
    public VideoPlayerCore GetChannelPlayer(int channelId)
    {
        if (_isOverrideChannelActive && _overrideChannelPlayer != null)
            return _overrideChannelPlayer;

        if (channelId < MIN_CHANNEL || channelId > MAX_CHANNEL)
            return null;

        return _channelPlayers.TryGetValue(channelId, out var player) ? player : null;
    }

    /// <summary>
    /// Get or create the shared player for a channel.
    /// Note: Override channel is handled separately and not created through this method.
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

        if (player != null && (player.IsPlaying || player.IsLoading || player.IsPreparing))
            return;

        player = GetOrCreateChannelPlayer(channelId);
        if (player == null) return;

        var channelContent = PresetChannels.FirstOrDefault(c => c.ChannelId == channelId).Content;
        if (channelContent == null)
        {
            CalRemix.instance.Logger.Warn($"Preset channel {channelId} has no content");
            return;
        }

        PlayChannelContent(channelId, channelContent, player);
    }

    #endregion

    #region Special Override Channel

    /// <summary>
    /// Check for and trigger special event videos.
    /// Called every update.
    /// </summary>
    private void CheckSpecialEvents()
    {
        if (_isOverrideChannelActive)
            return;

        string currentPlayerName = Main.LocalPlayer?.name;

        var triggeredEvents = SpecialEventVideos
            .Where(e => e.CanPlay(currentPlayerName) && e.TriggerCondition())
            .OrderByDescending(e => e.Priority)
            .ToList();

        if (triggeredEvents.Count == 0)
            return;

        var eventToPlay = triggeredEvents[0];
        PlaySpecialEvent(eventToPlay);
    }

    /// <summary>
    /// Play a special event video on the override channel.
    /// </summary>
    private void PlaySpecialEvent(SpecialEventVideo specialEvent)
    {
        if (_overrideChannelPlayer == null)
        {
            CalRemix.instance.Logger.Error($"[PlaySpecialEvent] Override channel player is null!");
            return;
        }

        CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Starting special event");
        CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Video URL: {specialEvent.VideoUrl}");
        CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Repeat mode: {specialEvent.RepeatBehavior}");
        CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Player initialized: {_overrideChannelPlayer.IsInitialized}");

        string currentPlayerName = Main.LocalPlayer?.name;
        specialEvent.MarkAsPlayed(currentPlayerName);

        EventHandler readyHandler = null;
        EventHandler errorHandler = null;

        readyHandler = (sender, e) =>
        {
            CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Video loaded and ready, activating override");

            _overrideChannelPlayer.PlaybackReady -= readyHandler;
            _overrideChannelPlayer.PlaybackError -= errorHandler;

            Main.QueueMainThreadAction(() =>
            {
                int pausedCount = 0;
                foreach (var kvp in _channelPlayers)
                {
                    var channelPlayer = kvp.Value;
                    if (channelPlayer.IsPlaying)
                    {
                        channelPlayer.Pause();
                        pausedCount++;
                    }
                }
                CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Paused {pausedCount} regular channels");

                _isOverrideChannelActive = true;
                _currentSpecialEvent = specialEvent;
                Main.NewText("⚠ SPECIAL BROADCAST INTERRUPTION ⚠", Color.Yellow);
                CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Override channel activated");
            });
        };

        errorHandler = (sender, e) =>
        {
            CalRemix.instance.Logger.Error($"[PlaySpecialEvent] Video failed to load, canceling special event");

            _overrideChannelPlayer.PlaybackReady -= readyHandler;
            _overrideChannelPlayer.PlaybackError -= errorHandler;

            _overrideChannelPlayer.Stop();
        };

        _overrideChannelPlayer.PlaybackReady += readyHandler;
        _overrideChannelPlayer.PlaybackError += errorHandler;

        CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Starting to load video: {specialEvent.VideoUrl}");
        _overrideChannelPlayer.Play(specialEvent.VideoUrl, forcePlay: true);
        CalRemix.instance.Logger.Info($"[PlaySpecialEvent] Video loading initiated");
    }

    /// <summary>
    /// Handle special channel ending.
    /// </summary>
    private void OnSpecialChannelEnded(object sender, EventArgs e)
    {
        _isOverrideChannelActive = false;
        _currentSpecialEvent = null;

        CalRemix.instance.Logger.Info("Special event video ended, returning to regular channels");
        Main.NewText("Returning to regular programming...", Color.LightGreen);

        int resumedCount = 0;
        foreach (var kvp in _channelPlayers)
        {
            var channelPlayer = kvp.Value;
            if (channelPlayer.IsPaused)
            {
                channelPlayer.Resume();
                resumedCount++;
            }
        }
        CalRemix.instance.Logger.Info($"[OnSpecialChannelEnded] Resumed {resumedCount} regular channels");

        _overrideChannelPlayer?.Stop();
    }

    /// <summary>
    /// Manually stop the special override channel.
    /// </summary>
    public void StopSpecialChannel()
    {
        if (!_isOverrideChannelActive)
            return;

        if (_overrideChannelPlayer != null)
        {
            _overrideChannelPlayer.Stop();
        }

        _isOverrideChannelActive = false;
        _currentSpecialEvent = null;
        CalRemix.instance.Logger.Info("Special channel manually stopped");
    }

    #endregion

    #region Content Playback

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
            VideoUrlHelper.IsSupportedVideoUrl(query) ||
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

                    List<string> reorderedVideos = [];
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
        var content = PresetChannels.FirstOrDefault(c => c.ChannelId == channelId).Content;
        if (content == null)
            return;

        var player = GetChannelPlayer(channelId);
        if (player == null) return;

        if (!content.IsPlaylist)
            Main.QueueMainThreadAction(() =>
            {
                PlayChannelContent(channelId, content, player);
            });
    }

    /// <summary>
    /// Stop a channel if no TVs are watching it.
    /// </summary>
    public void StopChannelIfUnused(int channelId)
    {
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

        if (activeTVCount == 0)
        {
            var player = GetChannelPlayer(channelId);
            if (player != null)
            {
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

    #endregion

    #region ModSystem Overrides

    public override void PostUpdateEverything()
    {
        CheckSpecialEvents();

        _overrideChannelPlayer?.Update(Main.gameTimeCache);

        foreach (var kvp in _channelPlayers)
        {
            var player = kvp.Value;
            player.Update(Main.gameTimeCache);
        }
    }

    public override void OnWorldLoad()
    {
        _lastPlayedEntry.Clear();

        foreach (var specialEvent in SpecialEventVideos)
            specialEvent.HasPlayedThisWorld = false;

        _overrideChannelPlayer = new VideoPlayerCore(1280, 720);
        _overrideChannelPlayer.PlaybackEnded += OnSpecialChannelEnded;
        _overrideChannelPlayer.PlaybackReady += (s, e) => CalRemix.instance.Logger.Info("[OverrideChannel] PlaybackReady fired");
        _overrideChannelPlayer.PlaybackError += (s, e) => CalRemix.instance.Logger.Error("[OverrideChannel] PlaybackError fired");

        CalRemix.instance.Logger.Info($"Created override channel player (session: {_overrideChannelPlayer.SessionId})");

        CalRemix.instance.Logger.Info("VideoChannelManager: World loaded");
    }

    public override void OnWorldUnload()
    {
        _overrideChannelPlayer?.Dispose();
        _overrideChannelPlayer = null;

        foreach (var kvp in _channelPlayers)
            kvp.Value.Dispose();

        _channelPlayers.Clear();
        _lastPlayedEntry.Clear();
        _isOverrideChannelActive = false;
        _currentSpecialEvent = null;

        CalRemix.instance.Logger.Info("VideoChannelManager: All channels disposed");
    }

    #endregion

    #region Mod Call Support
    /*
    public override object Call(params object[] args)
    {
        try
        {
            string command = args[0] as string;

            switch (command)
            {
                case "AddPresetChannel":
                    {
                        int channelId = Convert.ToInt32(args[1]);
                        ChannelContent content = args[2] as ChannelContent;
                        AddPresetChannel(channelId, content);
                        return true;
                    }

                case "AddSpecialEvent":
                    {
                        string videoUrl = args[1] as string;
                        Func<bool> condition = args[2] as Func<bool>;
                        RepeatMode repeatMode = args.Length > 3 ? (RepeatMode)args[3] : RepeatMode.OncePerSession;
                        int priority = args.Length > 4 ? Convert.ToInt32(args[4]) : 0;
                        AddSpecialEventVideo(videoUrl, condition, repeatMode, priority);
                        return true;
                    }

                case "GetChannelManager":
                    return this;

                default:
                    CalRemix.instance.Logger.Warn($"Unknown mod call command: {command}");
                    return false;
            }
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Mod call failed: {ex.Message}");
            return false;
        }
    }
    */
    #endregion
}