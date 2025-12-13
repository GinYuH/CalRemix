using LibVLCSharp.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.UI;

namespace CalRemix.UI.VideoPlayer;

/// <summary>
/// A reusable video player UI element that can be added to any UI state.
/// Handles its own video playback, frame capture, and rendering.
/// </summary>
public class VideoPlayerUIElement : UIElement, IDisposable
{
    // Video playback components
    private MediaPlayer _mediaPlayer;
    private Media _currentMedia;
    private VideoFrameHandler _frameHandler;
    private Texture2D _videoTexture;
    public static Asset<Texture2D> Background;

    // Video dimensions
    private int _videoWidth;
    private int _videoHeight;

    // Playback state
    private bool _isInitialized;
    private bool _isPlaying;
    private string _currentVideoPath;

    // Display settings
    private bool _maintainAspectRatio = true;
    private Color _backgroundColor = Color.Black;

    private bool _isDisposed = false;

    /// <summary>
    /// Creates a new video player with specified dimensions.
    /// </summary>
    /// <param name="width">Width of the video display area</param>
    /// <param name="height">Height of the video display area</param>
    public VideoPlayerUIElement(int width, int height)
    {
        Width.Set(width, 0f);
        Height.Set(height, 0f);

        _videoWidth = 1280;
        _videoHeight = 720;
    }

    /// <summary>
    /// Creates a new video player with specified dimensions and video resolution.
    /// </summary>
    public VideoPlayerUIElement(int width, int height, int videoWidth, int videoHeight)
    {
        Width.Set(width, 0f);
        Height.Set(height, 0f);

        _videoWidth = videoWidth;
        _videoHeight = videoHeight;
    }

    private void EnsureInitialized()
    {
        if (_isInitialized) return;

        if (CalRemix.LibVLCInstance == null)
        {
            Main.NewText("LibVLC not initialized! Check mod logs.", Color.Red);
            CalRemix.instance.Logger.Error("LibVLCInstance is null when trying to create VideoPlayerUIElement");
            return;
        }

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

            CalRemix.instance.Logger.Info("VideoPlayerUIElement initialized successfully");
        }
        catch (Exception ex)
        {
            Main.NewText($"Failed to initialize video player: {ex.Message}", Color.Red);
            CalRemix.instance.Logger.Error($"VideoPlayerUIElement initialization failed: {ex.Message}");
            CalRemix.instance.Logger.Error($"Stack trace: {ex.StackTrace}");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }
            _currentMedia?.Dispose();
            _currentMedia = null;
            _frameHandler = null;
            _videoTexture?.Dispose();
            _videoTexture = null;
        }

        _isDisposed = true;
        _isInitialized = false;

        CalRemix.instance.Logger.Info("VideoPlayerUIElement disposed.");
    }

    #region Playback Control

    /// <summary>
    /// Load and play a video from a file path.
    /// </summary>
    public void PlayVideo(string filePath)
    {
        EnsureInitialized();

        if (!_isInitialized)
        {
            Main.NewText("Video player failed to initialize!", Color.Red);
            return;
        }

        try
        {
            _videoTexture?.Dispose();
            _videoTexture = null;

            _currentMedia?.Dispose();

            _currentMedia = new Media(CalRemix.LibVLCInstance, filePath, FromType.FromPath);
            _currentVideoPath = filePath;
            _mediaPlayer.Play(_currentMedia);
        }
        catch (Exception ex)
        {
            Main.NewText($"Failed to play video: {ex.Message}", Color.Red);
        }
    }

    /// <summary>
    /// Load and play a video from a URL (including YouTube, Twitch, etc.)
    /// </summary>
    public void PlayUrl(string url)
    {
        EnsureInitialized();

        if (!_isInitialized)
        {
            Main.NewText("Video player failed to initialize!", Color.Red);
            return;
        }

        VideoUrlHelper.ProcessUrlAsync(url, (processedUrl) =>
        {
            if (_isDisposed)
            {
                CalRemix.instance.Logger.Info("Ignoring YouTube callback because the player UI was closed/disposed.");
                return; // Exit the callback entirely
            }

            Main.QueueMainThreadAction(() =>
            {
                if (_mediaPlayer == null || !_isInitialized || _isDisposed)
                    return; // Player was closed, do nothing

                if (processedUrl == null)
                {
                    Main.NewText("Failed to extract YouTube URL. Check logs.", Color.Red);
                    return;
                }

                Main.NewText("YouTube URL extracted successfully!", Color.Green);

                try
                {
                    _videoTexture?.Dispose();
                    _videoTexture = null;

                    _currentMedia?.Dispose();

                    _currentMedia = new Media(CalRemix.LibVLCInstance, processedUrl, FromType.FromLocation);
                    _currentVideoPath = url;
                    _mediaPlayer.Play(_currentMedia);
                }
                catch (Exception ex)
                {
                    Main.NewText($"Failed to play URL: {ex.Message}", Color.Red);
                }
            });
        });
    }

    /// <summary>
    /// Pause playback.
    /// </summary>
    public void Pause()
    {
        _mediaPlayer?.SetPause(true);
    }

    /// <summary>
    /// Resume playback.
    /// </summary>
    public void Resume()
    {
        _mediaPlayer?.SetPause(false);
    }

    /// <summary>
    /// Stop playback and clear the screen.
    /// </summary>
    public void Stop()
    {
        _mediaPlayer?.Stop();
        _videoTexture?.Dispose();
        _videoTexture = null;
    }

    /// <summary>
    /// Seek to a specific position (0.0 to 1.0).
    /// </summary>
    public void Seek(float position)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Position = Math.Clamp(position, 0f, 1f);
        }
    }

    /// <summary>
    /// Set volume (0 to 100).
    /// </summary>
    public void SetVolume(int volume)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Volume = Math.Clamp(volume, 0, 100);
        }
    }

    /// <summary>
    /// Mute or unmute audio.
    /// </summary>
    public void SetMute(bool muted)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Mute = muted;
        }
    }

    #endregion

    #region Getters

    public bool IsPlaying => _isPlaying;
    public bool IsInitialized => _isInitialized;
    public string CurrentVideoPath => _currentVideoPath;

    /// <summary>
    /// Get current playback position (0.0 to 1.0).
    /// </summary>
    public float GetPosition()
    {
        return _mediaPlayer?.Position ?? 0f;
    }

    /// <summary>
    /// Get video duration in milliseconds.
    /// </summary>
    public long GetDuration()
    {
        return _mediaPlayer?.Length ?? 0;
    }

    /// <summary>
    /// Get current volume (0 to 100).
    /// </summary>
    public int GetVolume()
    {
        return _mediaPlayer?.Volume ?? 0;
    }

    #endregion

    #region Settings

    /// <summary>
    /// Set whether to maintain aspect ratio when scaling.
    /// </summary>
    public void SetMaintainAspectRatio(bool maintain)
    {
        _maintainAspectRatio = maintain;
    }

    /// <summary>
    /// Set background color shown when no video is playing.
    /// </summary>
    public void SetBackgroundColor(Color color)
    {
        _backgroundColor = color;
    }

    #endregion

    #region Event Handlers

    private void OnPlaying(object sender, EventArgs e)
    {
        _isPlaying = true;
    }

    private void OnPaused(object sender, EventArgs e)
    {
        
    }

    private void OnStopped(object sender, EventArgs e)
    {
        _isPlaying = false;

        _videoTexture?.Dispose();
        _videoTexture = null;

        _currentMedia?.Dispose();
    }

    private void OnEndReached(object sender, EventArgs e)
    {
        _isPlaying = false;

        _videoTexture?.Dispose();
        _videoTexture = null;

        _currentMedia?.Dispose();
    }

    private void OnError(object sender, EventArgs e)
    {
        Main.NewText("Video playback error!", Color.Red);
        _isPlaying = false;
    }

    #endregion

    #region Update and Draw

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!_isInitialized || _frameHandler == null) return;

        if (_frameHandler.HasNewFrame())
        {
            byte[] frameData = _frameHandler.GetLatestFrame();
            if (frameData != null)
            {
                UpdateTexture(frameData);
            }
        }
    }

    private void UpdateTexture(byte[] rgbaData)
    {
        _videoTexture ??= new Texture2D(Main.graphics.GraphicsDevice, _videoWidth, _videoHeight);

        try
        {
            _videoTexture.SetData(rgbaData);
        }
        catch (Exception ex)
        {
            Main.NewText($"Failed to update video texture: {ex.Message}", Color.Red);
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        Rectangle drawArea = dimensions.ToRectangle();

        // Draw background
        spriteBatch.Draw(Background.Value, drawArea, _backgroundColor);

        // Draw video if available and playing
        if (_videoTexture != null && _isPlaying)
        {
            Rectangle videoRect = CalculateVideoRectangle(drawArea);
            spriteBatch.Draw(_videoTexture, videoRect, Color.White);
        }
    }

    private Rectangle CalculateVideoRectangle(Rectangle containerRect)
    {
        if (!_maintainAspectRatio)
        {
            return containerRect;
        }

        // Calculate aspect ratios
        float videoAspect = (float)_videoWidth / _videoHeight;
        float containerAspect = (float)containerRect.Width / containerRect.Height;

        int drawWidth, drawHeight;

        if (containerAspect > videoAspect)
        {
            // Container is wider - fit to height
            drawHeight = containerRect.Height;
            drawWidth = (int)(drawHeight * videoAspect);
        }
        else
        {
            // Container is taller - fit to width
            drawWidth = containerRect.Width;
            drawHeight = (int)(drawWidth / videoAspect);
        }

        // Center the video
        int x = containerRect.X + (containerRect.Width - drawWidth) / 2;
        int y = containerRect.Y + (containerRect.Height - drawHeight) / 2;

        return new Rectangle(x, y, drawWidth, drawHeight);
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Clean up resources. Call this when removing the element.
    /// </summary>
    public void RemoveCleanup()
    {
        _mediaPlayer?.Stop();

        if (_mediaPlayer != null)
        {
            _mediaPlayer.Playing -= OnPlaying;
            _mediaPlayer.Paused -= OnPaused;
            _mediaPlayer.Stopped -= OnStopped;
            _mediaPlayer.EndReached -= OnEndReached;
            _mediaPlayer.EncounteredError -= OnError;
        }

        _currentMedia?.Dispose();
        _mediaPlayer?.Dispose();
        _videoTexture?.Dispose();
        _frameHandler?.Dispose();

        _isInitialized = false;
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

    public VideoFrameHandler(int width, int height)
    {
        _width = width;
        _height = height;
        _frameSize = width * height * 4;
        _currentFrame = new byte[_frameSize];
        _nextFrame = new byte[_frameSize];
    }

    public void SetupMediaPlayer(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetVideoFormat("RV32", (uint)_width, (uint)_height, (uint)(_width * 4));
        mediaPlayer.SetVideoCallbacks(LockCallback, null, DisplayCallback);
    }

    private IntPtr LockCallback(IntPtr opaque, IntPtr planes)
    {
        _frameHandle = GCHandle.Alloc(_nextFrame, GCHandleType.Pinned);
        IntPtr ptr = _frameHandle.AddrOfPinnedObject();
        Marshal.WriteIntPtr(planes, ptr);
        return ptr;
    }

    private void DisplayCallback(IntPtr opaque, IntPtr picture)
    {
        lock (_frameLock)
        {
            var temp = _currentFrame;
            _currentFrame = _nextFrame;
            _nextFrame = temp;
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

    public byte[] GetLatestFrame()
    {
        lock (_frameLock)
        {
            if (!_hasNewFrame) return null;

            _hasNewFrame = false;

            byte[] copy = new byte[_frameSize];
            Buffer.BlockCopy(_currentFrame, 0, copy, 0, _frameSize);

            // Swap BGR to RGB
            for (int i = 0; i < _frameSize; i += 4)
            {
                byte temp = copy[i];
                copy[i] = copy[i + 2];
                copy[i + 2] = temp;
            }

            return copy;
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
