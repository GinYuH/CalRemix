using CalamityMod;
using CalRemix.Core.VideoPlayer;
using CalRemix.UI.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Content.Tiles.TVs;

/// <summary>
/// TV tile entity that displays video from shared channels.
/// All TVs on the same channel show the same video (perfectly synced).
/// </summary>
public class TVTileEntity : ModTileEntity
{
    public static Dictionary<int, (Point TileSize, Rectangle ScreenOffsets)> TileData = [];

    public bool IsOn { get; set; } = false;
    public int Volume { get; set; } = 100;
    public Point16 Size { get; set; }

    private bool _hasStartedChannel = false;

    public int CurrentChannel
    {
        get => _currentChannel;
        set
        {
            if (_currentChannel != value)
            {
                int oldChannel = _currentChannel;
                _currentChannel = value;
                _hasStartedChannel = false;
                OnChannelChanged(oldChannel, value);
            }
        }
    }
    private int _currentChannel = VideoChannelManager.DEFAULT_CHANNEL;

    public Point16 TilePosition { get; set; }

    public VideoPlayerCore GetVideoPlayer()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        return manager?.GetChannelPlayer(CurrentChannel);
    }

    public string GetDebugInfo() =>
        $"TV Entity ID: {ID}, Position: {Position}, TilePosition: {TilePosition}, " +
        $"Channel: {CurrentChannel}, IsOn: {IsOn}, Volume: {Volume}";

    /// <summary>
    /// Switch to a different channel.
    /// </summary>
    public void SetChannel(int channelId)
    {
        if (channelId < VideoChannelManager.MIN_CHANNEL ||
            channelId > VideoChannelManager.MAX_CHANNEL)
        {
            Main.NewText($"Invalid channel: {channelId}", Color.Red);
            return;
        }

        CurrentChannel = channelId;
        Main.NewText($"Switched to channel {channelId}", Color.Cyan);
    }

    /// <summary>
    /// Handle channel change.
    /// </summary>
    private void OnChannelChanged(int oldChannel, int newChannel)
    {
        if (!IsOn) return;

        var manager = ModContent.GetInstance<VideoChannelManager>();
        manager.StopChannelIfUnused(oldChannel);

        if (manager.IsPresetChannel(newChannel))
        {
            manager.StartChannel(newChannel);
            _hasStartedChannel = true;
        }
    }

    /// <summary>
    /// Stop playback (stops channel if no other TVs watching).
    /// </summary>
    public void Stop()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        manager?.StopChannelIfUnused(CurrentChannel);
    }

    public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
    {
        TilePosition = new Point16(i, j);
        Size = ((BaseTVTile)TileLoader.GetTile(type)).GetTVDimensions();

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            int tile = Main.tile[i, j].TileType;
            NetMessage.SendTileSquare(Main.myPlayer, i, j,
                TileData[tile].TileSize.X, TileData[tile].TileSize.Y);
            NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
            return -1;
        }

        return Place(i, j);
    }

    public override void Update()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        if (manager == null) return;

        if (!IsOn)
        {
            if (_hasStartedChannel)
            {
                manager.StopChannelIfUnused(CurrentChannel);
                _hasStartedChannel = false;
            }
            return;
        }

        if (manager.IsPresetChannel(CurrentChannel))
        {
            var player = manager.GetChannelPlayer(CurrentChannel);

            if (player == null || (!player.IsPlaying && !player.IsLoading && !player.IsPreparing))
            {
                manager.StartChannel(CurrentChannel);
                _hasStartedChannel = true;
            }
            else if (!_hasStartedChannel)
                _hasStartedChannel = true;
        }
    }

    public override void SaveData(TagCompound tag)
    {
        tag["channel"] = CurrentChannel;
        tag["posX"] = TilePosition.X;
        tag["posY"] = TilePosition.Y;
        tag["isOn"] = IsOn;
        tag["volume"] = Volume;
    }

    public override void LoadData(TagCompound tag)
    {
        CurrentChannel = tag.GetInt("channel");
        TilePosition = new Point16(tag.GetShort("posX"), tag.GetShort("posY"));
        IsOn = tag.GetBool("isOn");
        Volume = tag.GetInt("volume");
    }

    public override bool IsTileValidForEntity(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && TileData.ContainsKey(tile.TileType);
    }

    /// <summary>
    /// Render the video on this TV.
    /// </summary>
    public void DrawVideoOrLoading(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
    {
        if (!IsOn) return;

        var player = GetVideoPlayer();
        if (player == null) return;

        if (player.IsLoading)
        {
            Vector2 center = position + size / 2f;
            DrawLoadingSpinner(spriteBatch, center, player.LoadingRotation);
            return;
        }

        if (player.IsPlaying && player.CurrentTexture != null)
        {
            try
            {
                Vector2 scale = new Vector2(
                    size.X / player.CurrentTexture.Width,
                    size.Y / player.CurrentTexture.Height
                );
                spriteBatch.Draw(player.CurrentTexture, position, null,
                    Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            catch (Exception ex)
            {
                CalRemix.instance.Logger.Error($"Error drawing video texture: {ex.Message}");
            }
        }
    }

    private static Asset<Texture2D> _staticTexture;
    private Vector2 _staticOffset;

    public void DrawStatic(SpriteBatch spriteBatch, Rectangle screenArea)
    {
        try
        {
            _staticTexture ??= ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/StaticNoise");

            if ((int)(Main.GlobalTimeWrappedHourly * 60) % 3 == 0)
            {
                _staticOffset = new Vector2(
                    Main.rand.Next(0, Math.Max(1, _staticTexture.Width() - screenArea.Width)),
                    Main.rand.Next(0, Math.Max(1, _staticTexture.Height() - screenArea.Height))
                );
            }

            Rectangle sourceRect = new Rectangle(
                (int)_staticOffset.X, (int)_staticOffset.Y,
                Math.Min(_staticTexture.Width(), screenArea.Width),
                Math.Min(_staticTexture.Height(), screenArea.Height)
            );

            spriteBatch.Draw(_staticTexture.Value, screenArea, sourceRect, Color.White);
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Error drawing static: {ex.Message}");
        }
    }

    private static void DrawLoadingSpinner(SpriteBatch spriteBatch, Vector2 center, float rotation)
    {
        try
        {
            Texture2D pixel = ExampleVideoUISystem.Background.Value;

            for (int i = 0; i < 8; i++)
            {
                float angle = rotation + (i * MathHelper.TwoPi / 8f);
                float alpha = 0.3f + (0.7f * (i / 8f));
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * 10f,
                    (float)Math.Sin(angle) * 10f
                );

                Rectangle dotRect = new Rectangle(
                    (int)(center.X + offset.X - 2), (int)(center.Y + offset.Y - 2), 4, 4
                );

                spriteBatch.Draw(pixel, dotRect, Color.White * alpha);
            }
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Error drawing loading spinner: {ex.Message}");
        }
    }

    private Vector2? _cachedVideoPosition;
    private Vector2? _cachedVideoSize;
    private Rectangle? _cachedStaticArea;
    private float _lastCachedZoom = -1f;
    private Vector2 _lastCachedScreenPos;

    public (Vector2 Position, Vector2 Size, Rectangle StaticArea) CalculateScreenAreas()
    {
        bool cacheValid = _lastCachedZoom == Main.GameViewMatrix.Zoom.X &&
                          _lastCachedScreenPos == Main.screenPosition;

        if (cacheValid && _cachedVideoPosition.HasValue &&
            _cachedVideoSize.HasValue && _cachedStaticArea.HasValue)
        {
            return (_cachedVideoPosition.Value, _cachedVideoSize.Value, _cachedStaticArea.Value);
        }

        int tileType = Main.tile[Position.X, Position.Y].TileType;
        if (!TileData.TryGetValue(tileType, out var tileInfo))
            return (Vector2.Zero, Vector2.Zero, Rectangle.Empty);

        Rectangle worldArea = new Rectangle(
            Position.X * 16, Position.Y * 16,
            tileInfo.TileSize.X * 16, tileInfo.TileSize.Y * 16
        );

        worldArea.X += tileInfo.ScreenOffsets.X;
        worldArea.Y += tileInfo.ScreenOffsets.Y;
        worldArea.Width -= tileInfo.ScreenOffsets.X;
        worldArea.Height -= tileInfo.ScreenOffsets.Y;
        worldArea.Width += tileInfo.ScreenOffsets.Width;
        worldArea.Height += tileInfo.ScreenOffsets.Height;

        Vector2 worldPos = new Vector2(worldArea.X, worldArea.Y);
        Vector2 screenPos = worldPos - Main.screenPosition;
        float zoom = Main.GameViewMatrix.Zoom.X;
        Vector2 screenCenter = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);

        Vector2 finalPos = (screenPos - screenCenter) * zoom + screenCenter;
        Vector2 finalSize = new Vector2(worldArea.Width, worldArea.Height) * zoom;

        Rectangle staticArea = new Rectangle(
            (int)screenPos.X, (int)screenPos.Y, worldArea.Width, worldArea.Height
        );

        _cachedVideoPosition = finalPos;
        _cachedVideoSize = finalSize;
        _cachedStaticArea = staticArea;
        _lastCachedZoom = zoom;
        _lastCachedScreenPos = Main.screenPosition;

        return (finalPos, finalSize, staticArea);
    }

    public override void OnKill()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        manager?.StopChannelIfUnused(CurrentChannel);

        base.OnKill();
    }
}