using CalRemix.Core.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;

namespace CalRemix.UI.VideoPlayer;

/// <summary>
/// A reusable video player UI element that wraps VideoPlayerCore.
/// Handles UI-specific rendering and user interaction.
/// </summary>
public class VideoPlayerUIElement : UIElement, IDisposable
{
    private VideoPlayerCore _playerCore;
    private Color _backgroundColor = Color.Black;

    internal bool _isDisposed = false;

    /// <summary>
    /// Creates a new video player with specified dimensions.
    /// </summary>
    public VideoPlayerUIElement(int width, int height, int videoWidth = 1280, int videoHeight = 720)
    {
        Width.Set(width, 0f);
        Height.Set(height, 0f);

        _playerCore = new VideoPlayerCore(videoWidth, videoHeight);

        // Subscribe to events if you want to react to state changes
        _playerCore.PlaybackStarted += OnPlaybackStarted;
        _playerCore.PlaybackStopped += OnPlaybackStopped;
        _playerCore.PlaybackError += OnPlaybackError;
    }

    #region Event Handlers

    private void OnPlaybackStarted(object sender, EventArgs e)
    {
        Main.NewText("Playback started", Color.LightGreen);
    }

    private void OnPlaybackStopped(object sender, EventArgs e)
    {
        Main.NewText("Playback stopped", Color.Gray);
    }

    private void OnPlaybackError(object sender, EventArgs e)
    {
        Main.NewText("Video playback error!", Color.Red);
    }

    #endregion

    #region Playback Control

    /// <summary>
    /// Play media from any source: file path, YouTube URL, or search query.
    /// </summary>
    public void Play(string input) => _playerCore.Play(input);

    public void Pause() => _playerCore.Pause();
    public void Resume() => _playerCore.Resume();
    public void Stop() => _playerCore.Stop();
    public void Seek(float position) => _playerCore.Seek(position);
    public void SetVolume(int volume) => _playerCore.SetVolume(volume);
    public void SetMute(bool muted) => _playerCore.SetMute(muted);

    #endregion

    #region Getters

    public bool IsPlaying => _playerCore.IsPlaying;
    public bool IsPaused => _playerCore.IsPaused;
    public bool IsInitialized => _playerCore.IsInitialized;
    public bool IsLoading => _playerCore.IsLoading;
    public string CurrentVideoPath => _playerCore.CurrentVideoPath;
    public float GetPosition() => _playerCore.GetPosition();
    public long GetDuration() => _playerCore.GetDuration();
    public int GetVolume() => _playerCore.GetVolume();

    #endregion

    #region Settings

    public void SetMaintainAspectRatio(bool maintain) => _playerCore.SetMaintainAspectRatio(maintain);
    public void SetBackgroundColor(Color color) => _backgroundColor = color;

    #endregion

    #region Update and Draw

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _playerCore.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        Rectangle drawArea = dimensions.ToRectangle();

        // Draw background
        spriteBatch.Draw(ExampleVideoUISystem.Background.Value, drawArea, _backgroundColor);

        // Draw video if available and playing
        if (_playerCore.CurrentTexture != null && _playerCore.IsPlaying)
        {
            Rectangle videoRect = _playerCore.CalculateRenderRectangle(drawArea);
            spriteBatch.Draw(_playerCore.CurrentTexture, videoRect, Color.White);
        }

        // Draw loading spinner
        if (_playerCore.IsLoading)
        {
            Vector2 center = new Vector2(drawArea.Center.X, drawArea.Center.Y);
            DrawLoadingSpinner(spriteBatch, center, _playerCore.LoadingRotation);
        }
    }

    private void DrawLoadingSpinner(SpriteBatch spriteBatch, Vector2 center, float rotation)
    {
        for (int i = 0; i < 8; i++)
        {
            float angle = rotation + (i * MathHelper.TwoPi / 8f);
            float alpha = 0.3f + (0.7f * (i / 8f));
            Vector2 offset = new Vector2(
                (float)Math.Cos(angle) * 20f,
                (float)Math.Sin(angle) * 20f
            );

            Rectangle dotRect = new Rectangle(
                (int)(center.X + offset.X - 3),
                (int)(center.Y + offset.Y - 3),
                6, 6
            );

            spriteBatch.Draw(
                ExampleVideoUISystem.Background.Value,
                dotRect,
                Color.White * alpha
            );
        }
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

        if (disposing)
        {
            // Unsubscribe from events
            if (_playerCore != null)
            {
                _playerCore.PlaybackStarted -= OnPlaybackStarted;
                _playerCore.PlaybackStopped -= OnPlaybackStopped;
                _playerCore.PlaybackError -= OnPlaybackError;

                _playerCore.Dispose();
                _playerCore = null;
            }
        }

        _isDisposed = true;
    }

    #endregion
}