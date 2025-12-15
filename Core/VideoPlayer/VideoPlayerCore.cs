using LibVLCSharp.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

/// <summary>
/// Core video player logic independent of UI.
/// Handles LibVLC media playback, frame capture, and texture management.
/// Can be used by UI elements, tiles, or any other system that needs video playback.
/// </summary>
public class VideoPlayerCore(int videoWidth = 1280, int videoHeight = 720) : IDisposable
{
    // Video playback components
    private MediaPlayer _mediaPlayer;
    private Media _currentMedia;
    private VideoFrameHandler _frameHandler;
    private Texture2D _videoTexture;
    private readonly TexturePool _texturePool = new();

    // Video dimensions
    private readonly int _videoWidth = videoWidth;
    private readonly int _videoHeight = videoHeight;

    // Playback state
    private bool _isInitialized;
    private bool _isPlaying;
    private bool _isPaused;
    private string _currentVideoPath;
    private bool _isLoading;
    private float _loadingRotation;
    private bool _isPreparing;

    // Session and request tracking
    private Guid _sessionId = Guid.NewGuid();
    private Guid _currentRequestId;
    private bool _isDisposed = false;

    // Display settings
    private bool _maintainAspectRatio = true;

    // Frame rate limiting
    private double _lastFrameTime;
    private const double TARGET_FRAME_TIME = 1.0 / 30.0; // Limit to 30 FPS for video updates

    private bool _textureNeedsUpdate;

    #region Events

    public event EventHandler PlaybackStarted;
    public event EventHandler PlaybackReady;
    public event EventHandler PlaybackPaused;
    public event EventHandler PlaybackStopped;
    public event EventHandler PlaybackEnded;
    public event EventHandler PlaybackError;
    public event EventHandler<bool> LoadingStateChanged;

    #endregion

    #region Properties

    public bool IsPlaying => _isPlaying;
    public bool IsPaused => _isPaused;
    public bool IsInitialized => _isInitialized;
    public bool IsLoading => _isLoading;
    public bool IsPreparing => _isPreparing;
    public string CurrentVideoPath => _currentVideoPath;
    public Texture2D CurrentTexture => _videoTexture;
    public int VideoWidth => _videoWidth;
    public int VideoHeight => _videoHeight;
    public float LoadingRotation => _loadingRotation;
    public Guid SessionId => _sessionId;
    public bool MediaParsed => _currentMedia != null;

    #endregion

    #region Initialization

    private async Task EnsureInitializedAsync()
    {
        if (_isInitialized || CalRemix.instance == null || CalRemix.LibVLCInstance == null)
            return;

        try
        {
            await Task.Run(() => {
                _frameHandler = new VideoFrameHandler(_videoWidth, _videoHeight);
            });

            Main.QueueMainThreadAction(() => {
                try
                {
                    _frameHandler = new VideoFrameHandler(_videoWidth, _videoHeight);
                    _mediaPlayer = new MediaPlayer(CalRemix.LibVLCInstance);
                    _frameHandler.SetupMediaPlayer(_mediaPlayer);

                    _mediaPlayer.Playing += OnPlaying;
                    _mediaPlayer.Paused += OnPaused;
                    _mediaPlayer.Stopped += OnStopped;
                    _mediaPlayer.EndReached += OnEndReached;
                    _mediaPlayer.EncounteredError += OnError;

                    _isInitialized = true;

                    CalRemix.instance.Logger.Info($"VideoPlayerCore initialized (session {_sessionId})");
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Error($"VideoPlayerCore initialization failed: {ex}");
                }
            });
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"VideoPlayerCore async initialization failed: {ex}");
        }
    }

    #endregion

    #region Playback Control

    public void Play(string input)
    {
        if (!_isInitialized)
        {
            Task.Run(() => EnsureInitializedAsync());
            return;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        try
        {
            if (_isPlaying || _isPaused)
            {
                _mediaPlayer?.Stop();
            }

            VideoUrlHelper.CancelRequest(_currentRequestId);

            if (_videoTexture != null)
            {
                _texturePool.ReturnTexture(_videoTexture);
                _videoTexture = null;
            }
            _frameHandler?.ClearBuffers();

            _currentMedia?.Dispose();


            if (VideoUrlHelper.IsYouTubeUrl(input))
            {
                PlayYouTubeUrl(input);
            }
            else if (IsFilePath(input))
            {
                PlayLocalFile(input);
            }
            else if (IsMediaLink(input))
            {
                PlayMediaUrl(input);
            }
            else
            {
                PlayYouTubeSearch(input);
            }
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Play failed: {ex.Message}");
        }
    }

    private static bool IsFilePath(string input)
    {
        if (System.IO.Path.IsPathRooted(input))
            return true;

        if (input.Where(c => c == '.').Count() != 1)
            return false;

        if ((input.Contains('/') || input.Contains('\\')) && (input.Contains(".mp4") || input.Contains(".avi") || input.Contains(".mkv") || input.Contains(".mov")))
            return true;

        return false;
    }

    private static bool IsMediaLink(string input) => input.Contains(".mp4") || input.Contains(".avi") || input.Contains(".mkv") || input.Contains(".mov");

    private void PlayLocalFile(string filePath)
    {
        string resolvedPath = ResolveVideoPath(filePath);

        if (resolvedPath == null)
        {
            CalRemix.instance.Logger.Error($"Video file not found: {filePath}");
            return;
        }

        _isPreparing = true;
        _currentMedia = new Media(CalRemix.LibVLCInstance, resolvedPath, FromType.FromPath);
        _currentVideoPath = filePath;
        _mediaPlayer.Play(_currentMedia);
    }

    private void PlayYouTubeUrl(string url)
    {
        _currentRequestId = Guid.NewGuid();
        var requestId = _currentRequestId;
        var sessionId = _sessionId;

        SetLoadingState(true);

        VideoUrlHelper.ProcessUrlAsync(url, (processedUrl) =>
        {
            Main.QueueMainThreadAction(() =>
            {
                SetLoadingState(false);

                if (_sessionId != sessionId || _currentRequestId != requestId || _isDisposed)
                    return;

                if (processedUrl == null)
                {
                    CalRemix.instance.Logger.Error("Failed to extract YouTube URL");
                    return;
                }

                try
                {
                    if (_videoTexture != null)
                    {
                        _texturePool.ReturnTexture(_videoTexture);
                        _videoTexture = null;
                    }
                    _frameHandler?.ClearBuffers();

                    _currentMedia?.Dispose();

                    _isPreparing = true;
                    _currentMedia = new Media(CalRemix.LibVLCInstance, processedUrl, FromType.FromLocation);
                    _currentVideoPath = url;
                    _mediaPlayer.Play(_currentMedia);
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Error($"Failed to play YouTube URL: {ex.Message}");
                }
            });
        }, requestId);
    }

    private void PlayMediaUrl(string url)
    {
        SetLoadingState(false);

        if (url == null)
        {
            CalRemix.instance.Logger.Error("Failed to extract YouTube URL");
            return;
        }

        try
        {
            if (_videoTexture != null)
            {
                _texturePool.ReturnTexture(_videoTexture);
                _videoTexture = null;
            }
            _frameHandler?.ClearBuffers();

            _currentMedia?.Dispose();

            _isPreparing = true;
            _currentMedia = new Media(CalRemix.LibVLCInstance, url, FromType.FromLocation);
            _currentVideoPath = url;
            _mediaPlayer.Play(_currentMedia);
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Failed to play YouTube URL: {ex.Message}");
        }
    }

    private void PlayYouTubeSearch(string query)
    {
        _currentRequestId = Guid.NewGuid();
        var requestId = _currentRequestId;
        var sessionId = _sessionId;

        SetLoadingState(true);

        VideoUrlHelper.ProcessSearchAsync(query, (processedUrl) =>
        {
            Main.QueueMainThreadAction(() =>
            {
                SetLoadingState(false);

                if (_sessionId != sessionId || _currentRequestId != requestId || _isDisposed)
                    return;

                if (processedUrl == null)
                    return;

                try
                {
                    if (_videoTexture != null)
                    {
                        _texturePool.ReturnTexture(_videoTexture);
                        _videoTexture = null;
                    }
                    _frameHandler?.ClearBuffers();

                    _currentMedia?.Dispose();

                    _isPreparing = true;
                    _currentMedia = new Media(CalRemix.LibVLCInstance, processedUrl, FromType.FromLocation);
                    _currentVideoPath = query;
                    _mediaPlayer.Play(_currentMedia);
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Error($"Failed to play search result: {ex.Message}");
                }
            });
        }, requestId);
    }

    private static string ResolveVideoPath(string filePath)
    {
        if (System.IO.Path.IsPathRooted(filePath))
        {
            return System.IO.File.Exists(filePath) ? filePath : null;
        }

        string[] extensionsToTry = filePath.Contains(".")
            ? new[] { "" }
            : new[] { ".mp4", ".avi", ".mkv", ".mov" };

        foreach (string ext in extensionsToTry)
        {
            string testPath = filePath + ext;

            string modPath = System.IO.Path.Combine(CalRemix.instance.GetType().Namespace, testPath);
            string fullModPath = ModLoader.ModPath.Replace("Mods", "ModSources") +
                                System.IO.Path.DirectorySeparatorChar + modPath;

            if (System.IO.File.Exists(fullModPath))
            {
                return fullModPath;
            }

            string contentPath = System.IO.Path.Combine(ModLoader.ModPath, CalRemix.instance.Name, testPath);
            if (System.IO.File.Exists(contentPath))
            {
                return contentPath;
            }

            if (System.IO.File.Exists(testPath))
            {
                return System.IO.Path.GetFullPath(testPath);
            }
        }

        return null;
    }

    public void Pause() => _mediaPlayer?.SetPause(true);
    public void Resume() => _mediaPlayer?.SetPause(false);

    public void Stop()
    {
        if (_isLoading)
        {
            Main.NewText("Loading cancelled", Color.Orange);
            _isLoading = false;
        }

        VideoUrlHelper.CancelRequest(_currentRequestId);
        _mediaPlayer?.Stop();

        if (_videoTexture != null)
        {
            _texturePool.ReturnTexture(_videoTexture);
            _videoTexture = null;
        }
        _frameHandler?.ClearBuffers();
    }

    public void Seek(float position)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Position = Math.Clamp(position, 0f, 1f);
        }
    }

    public void SetVolume(int volume)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Volume = Math.Clamp(volume, 0, 100);
        }
    }

    public void SetMute(bool muted)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Mute = muted;
        }
    }

    public float GetPosition() => _mediaPlayer?.Position ?? 0f;
    public long GetDuration() => _mediaPlayer?.Length ?? 0;
    public int GetVolume() => _mediaPlayer?.Volume ?? 0;

    #endregion

    #region Settings

    public void SetMaintainAspectRatio(bool maintain) => _maintainAspectRatio = maintain;

    #endregion

    #region Event Handlers

    private void OnPlaying(object sender, EventArgs e)
    {
        _isPlaying = true;
        _isPreparing = false;
        _isPaused = false;
        PlaybackStarted?.Invoke(this, EventArgs.Empty);

        var currentSessionId = _sessionId;
        Task.Run(async () =>
        {
            await Task.Delay(500);
            Main.QueueMainThreadAction(() =>
            {
                if (_isPlaying && _sessionId == currentSessionId && !_isDisposed)
                {
                    PlaybackReady?.Invoke(this, EventArgs.Empty);
                }
            });
        });
    }

    private void OnPaused(object sender, EventArgs e)
    {
        _isPaused = true;
        PlaybackPaused?.Invoke(this, EventArgs.Empty);
    }

    private void OnStopped(object sender, EventArgs e)
    {
        _isPlaying = false;
        _isPaused = false;
        PlaybackStopped?.Invoke(this, EventArgs.Empty);
    }

    private void OnEndReached(object sender, EventArgs e)
    {
        _isPlaying = false;
        _isPaused = false;
        PlaybackEnded?.Invoke(this, EventArgs.Empty);
    }

    private void OnError(object sender, EventArgs e)
    {
        _isPlaying = false;
        PlaybackError?.Invoke(this, EventArgs.Empty);
    }

    private void SetLoadingState(bool loading)
    {
        if (_isLoading != loading)
        {
            _isLoading = loading;
            LoadingStateChanged?.Invoke(this, loading);
        }
    }

    #endregion

    #region Update and Texture Management

    public void Update(GameTime gameTime)
    {
        if (!_isInitialized && CalRemix.instance != null && CalRemix.LibVLCInstance != null)
        {
            Task.Run(() => EnsureInitializedAsync());
        }

        if (_isLoading)
        {
            _loadingRotation += 0.1f;
        }

        if (!_isInitialized || _frameHandler == null) return;

        double currentTime = gameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastFrameTime < TARGET_FRAME_TIME)
        {
            return; // Skip this frame
        }
        _lastFrameTime = currentTime;

        if (!_isPlaying && !_isPaused) return;

        if (_frameHandler.HasNewFrame())
        {
            _textureNeedsUpdate = true;
        }

        if (_textureNeedsUpdate)
        {
            byte[] frameData = _frameHandler.GetLatestFrame(out int stride);
            if (frameData != null)
            {
                UpdateTexture(frameData, stride);
                _textureNeedsUpdate = false;
            }
        }
    }

    private void UpdateTexture(byte[] rgbaData, int stride)
    {
        if (_videoTexture == null || _videoTexture.Width != _videoWidth || _videoTexture.Height != _videoHeight)
        {
            if (_videoTexture != null)
            {
                _texturePool.ReturnTexture(_videoTexture);
            }
            _videoTexture = _texturePool.GetTexture(_videoWidth, _videoHeight);
        }

        try
        {
            _videoTexture.SetData(rgbaData);
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Texture update failed: {ex}");
        }
    }

    public Rectangle CalculateRenderRectangle(Rectangle containerRect)
    {
        if (!_maintainAspectRatio)
        {
            return containerRect;
        }

        float videoAspect = (float)_videoWidth / _videoHeight;
        float containerAspect = (float)containerRect.Width / containerRect.Height;

        int drawWidth, drawHeight;

        if (containerAspect > videoAspect)
        {
            drawHeight = containerRect.Height;
            drawWidth = (int)(drawHeight * videoAspect);
        }
        else
        {
            drawWidth = containerRect.Width;
            drawHeight = (int)(drawWidth / videoAspect);
        }

        int x = containerRect.X + (containerRect.Width - drawWidth) / 2;
        int y = containerRect.Y + (containerRect.Height - drawHeight) / 2;

        return new Rectangle(x, y, drawWidth, drawHeight);
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        CalRemix.instance.Logger.Info($"Disposing VideoPlayerCore (session {_sessionId})");

        if (disposing)
        {
            VideoUrlHelper.CancelRequest(_currentRequestId);

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Playing -= OnPlaying;
                _mediaPlayer.Paused -= OnPaused;
                _mediaPlayer.Stopped -= OnStopped;
                _mediaPlayer.EndReached -= OnEndReached;
                _mediaPlayer.EncounteredError -= OnError;

                try
                {
                    _mediaPlayer.Stop();
                    _mediaPlayer.Dispose();
                }
                catch (Exception ex)
                {
                    CalRemix.instance.Logger.Error($"Error disposing media player: {ex.Message}");
                }
                _mediaPlayer = null;
            }

            _currentMedia?.Dispose();
            _currentMedia = null;

            _frameHandler?.Dispose();
            _frameHandler = null;

            if (_videoTexture != null)
            {
                _texturePool.ReturnTexture(_videoTexture);
                _videoTexture = null;
            }
            _frameHandler?.ClearBuffers();
        }

        _sessionId = Guid.Empty;
        _isDisposed = true;
        _isInitialized = false;
        _isPlaying = false;
        _isPaused = false;
        _currentVideoPath = null;

        CalRemix.instance.Logger.Info("VideoPlayerCore disposed successfully.");
    }

    #endregion
}

public class VideoFrameHandler : IDisposable
{
    private byte[] _currentFrame;
    private byte[] _nextFrame;
    private readonly int _width;
    private readonly int _height;
    private readonly int _frameSize;
    private bool _hasNewFrame;
    private readonly object _frameLock = new object();
    private GCHandle _frameHandle;

    private byte[] _conversionBuffer;

    public VideoFrameHandler(int width, int height)
    {
        _width = width;
        _height = height;
        _frameSize = width * height * 4;
        _currentFrame = new byte[_frameSize];
        _nextFrame = new byte[_frameSize];
        _conversionBuffer = new byte[_frameSize];
    }

    public void SetupMediaPlayer(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetVideoFormat("RGBA", (uint)_width, (uint)_height, (uint)(_width * 4));
        mediaPlayer.SetVideoCallbacks(LockCallback, null, DisplayCallback);
    }

    private nint LockCallback(nint opaque, nint planes)
    {
        _frameHandle = GCHandle.Alloc(_nextFrame, GCHandleType.Pinned);
        nint ptr = _frameHandle.AddrOfPinnedObject();
        Marshal.WriteIntPtr(planes, ptr);
        return ptr;
    }

    private void DisplayCallback(nint opaque, nint picture)
    {
        lock (_frameLock)
        {
            (_nextFrame, _currentFrame) = (_currentFrame, _nextFrame);
            _hasNewFrame = true;
        }

        if (_frameHandle.IsAllocated)
        {
            _frameHandle.Free();
        }
    }

    public bool HasNewFrame()
    {
        lock (_frameLock)
        {
            return _hasNewFrame;
        }
    }

    public byte[] GetLatestFrame(out int stride)
    {
        lock (_frameLock)
        {
            if (!_hasNewFrame)
            {
                stride = 0;
                return null;
            }

            _hasNewFrame = false;
            stride = _width * 4;

            Buffer.BlockCopy(_currentFrame, 0, _conversionBuffer, 0, _frameSize);
            return _conversionBuffer;
        }
    }

    public void ClearBuffers()
    {
        lock (_frameLock)
        {
            Array.Clear(_currentFrame, 0, _frameSize);
            Array.Clear(_nextFrame, 0, _frameSize);
            Array.Clear(_conversionBuffer, 0, _frameSize);
            _hasNewFrame = false;
        }
    }

    public void Dispose()
    {
        if (_frameHandle.IsAllocated)
        {
            _frameHandle.Free();
        }
    }
}

public class TexturePool
{
    private readonly System.Collections.Generic.Dictionary<(int, int), System.Collections.Generic.Queue<Texture2D>> _pool = new();
    private const int MAX_POOL_SIZE = 3;

    public Texture2D GetTexture(int width, int height)
    {
        var key = (width, height);

        if (_pool.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            return queue.Dequeue();
        }

        return new Texture2D(Main.graphics.GraphicsDevice, width, height);
    }

    public void ReturnTexture(Texture2D texture)
    {
        if (texture == null || texture.IsDisposed) return;

        var key = (texture.Width, texture.Height);

        if (!_pool.ContainsKey(key))
        {
            _pool[key] = new System.Collections.Generic.Queue<Texture2D>();
        }

        if (_pool[key].Count < MAX_POOL_SIZE)
        {
            _pool[key].Enqueue(texture);
        }
        else
        {
            texture.Dispose();
        }
    }

    public void Clear()
    {
        foreach (var queue in _pool.Values)
        {
            while (queue.Count > 0)
            {
                queue.Dequeue()?.Dispose();
            }
        }
        _pool.Clear();
    }
}