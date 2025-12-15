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
/// Base tile entity for all TV types.
/// Handles channel management, rendering, and persistence.
/// </summary>
public class TVTileEntity : ModTileEntity
{
    public static Dictionary<int, (Point TileSize, Rectangle ScreenOffsets)> TileData = [];

    public bool IsOn { get; set; } = false;

    // Channel this TV is tuned to
    public int CurrentChannel { get; set; } = VideoChannelManager.DEFAULT_CHANNEL;

    // Position of the TV in the world
    public Point16 TilePosition { get; set; }

    // Get the video player for this TV's current channel
    public VideoPlayerCore GetVideoPlayer()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        return manager?.GetChannel(CurrentChannel);
    }

    // Get or create the video player for this TV's channel
    public VideoPlayerCore GetOrCreateVideoPlayer()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        return manager?.GetOrCreateChannel(CurrentChannel);
    }

    /// <summary>
    /// Switch to a different channel.
    /// </summary>
    public void SetChannel(int channelId)
    {
        if (channelId < VideoChannelManager.MIN_CHANNEL || channelId > VideoChannelManager.MAX_CHANNEL)
        {
            Main.NewText($"Invalid channel: {channelId}", Color.Red);
            return;
        }

        CurrentChannel = channelId;
        Main.NewText($"Switched to channel {channelId}", Color.Cyan);
    }

    /// <summary>
    /// Play media on this TV's channel.
    /// </summary>
    public void Play(string input)
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        manager?.PlayOnChannel(CurrentChannel, input);
    }

    /// <summary>
    /// Stop playback on this TV's channel.
    /// </summary>
    public void Stop()
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        manager?.StopChannel(CurrentChannel);
    }

    public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
    {
        TilePosition = new Point16(i, j);

        // If in multiplayer, tell the server to place the tile entity and DO NOT place it yourself. That would mismatch IDs.
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            int tile = Main.tile[i, j].TileType;
            NetMessage.SendTileSquare(Main.myPlayer, i, j, TileData[tile].TileSize.X, TileData[tile].TileSize.Y);
            NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
            return -1;
        }

        return Place(i, j);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["channel"] = CurrentChannel;
        tag["posX"] = TilePosition.X;
        tag["posY"] = TilePosition.Y;
        tag["isOn"] = IsOn; // Save on/off state
    }

    public override void LoadData(TagCompound tag)
    {
        CurrentChannel = tag.GetInt("channel");
        TilePosition = new Point16(tag.GetShort("posX"), tag.GetShort("posY"));
        IsOn = tag.GetBool("isOn"); // Load on/off state
    }

    public override bool IsTileValidForEntity(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && TileData.ContainsKey(tile.TileType);
    }

    private Rectangle? _cachedScreenArea;
    private int _cachedTileType;

    /// <summary>
    /// Render the video on this TV.
    /// Called from the tile's PostDraw.
    /// </summary>
    public void DrawVideoContent(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
    {
        if (!IsOn)
            return;

        var player = GetVideoPlayer();

        if (player != null && player.IsLoading)
        {
            Vector2 center = position + size / 2f;
            DrawLoadingSpinner(spriteBatch, center, player.LoadingRotation);
            return;
        }

        if (player != null && player.IsPlaying && player.CurrentTexture != null)
        {
            // Use position and scale for sub-pixel precision
            Vector2 scale = new Vector2(size.X / player.CurrentTexture.Width, size.Y / player.CurrentTexture.Height);
            spriteBatch.Draw(player.CurrentTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            return;
        }

        if (player == null || (!player.IsPlaying && !player.IsLoading && !player.IsPreparing))
        {
            DrawStatic(spriteBatch, position, size);
        }
    }

    private Asset<Texture2D> _staticTexture;
    private Vector2 _staticOffset;
    private int _staticFrameCounter;

    private void DrawStatic(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
    {
        _staticTexture ??= ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/StaticNoise");

        _staticFrameCounter++;
        if (_staticFrameCounter > 3)
        {
            _staticFrameCounter = 0;
            _staticOffset = new Vector2(
                Main.rand.Next(0, Math.Max(1, _staticTexture.Width() - (int)size.X)),
                Main.rand.Next(0, Math.Max(1, _staticTexture.Height() - (int)size.Y))
            );
        }

        Rectangle sourceRect = new Rectangle(
            (int)_staticOffset.X,
            (int)_staticOffset.Y,
            Math.Min(_staticTexture.Width(), (int)size.X),
            Math.Min(_staticTexture.Height(), (int)size.Y)
        );

        Vector2 scale = new Vector2(size.X / sourceRect.Width, size.Y / sourceRect.Height);
        spriteBatch.Draw(_staticTexture.Value, position, sourceRect, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    private static void DrawLoadingSpinner(SpriteBatch spriteBatch, Vector2 center, float rotation)
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
                (int)(center.X + offset.X - 2),
                (int)(center.Y + offset.Y - 2),
                4, 4
            );

            spriteBatch.Draw(pixel, dotRect, Color.White * alpha);
        }
    }
}