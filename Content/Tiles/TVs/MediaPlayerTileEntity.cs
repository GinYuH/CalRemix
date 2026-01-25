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
public class MediaPlayerEntity : ModTileEntity
{
    public readonly List<Point16> ConnectedTVs = [];

    public VideoPlayerCore player = null;

    public string CurrentContentPath = string.Empty;
    public int StoredItem = -1;

    public Rectangle TileArea = Rectangle.Empty;

    private bool IsOn = false;

    public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
    {
        TileArea.X = i;
        TileArea.Y = i;
        TileArea.Width = 2;
        TileArea.Height = 1;

        player = new VideoPlayerCore(1280, 720);

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            NetMessage.SendTileSquare(Main.myPlayer, i, j, TileArea.Width, TileArea.Height);
            NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
            return -1;
        }

        return Place(i, j);
    }

    public override void OnNetPlace()
    {
        player = new VideoPlayerCore(1280, 720);
    }

    public override void Update()
    {
        if (!IsTileValidForEntity(Position.X, Position.Y))
        {
            Kill(Position.X, Position.Y);
            return;
        }
        
        player ??= new VideoPlayerCore(1280, 720);

        var manager = ModContent.GetInstance<VideoChannelManager>();
        if (manager.IsOverrideChannelActive() && IsOn && player.IsPlaying)
        {
            player.Pause();
            return;
        }
        else if (!manager.IsOverrideChannelActive() && IsOn && player.IsPaused)
            player.Resume();

        if (CurrentContentPath != string.Empty && !IsOn)
        {
            player.Play(CurrentContentPath);
            IsOn = true;
        }
        else if (IsOn && CurrentContentPath == string.Empty)
        {
            player.Stop();
            IsOn = false;
        }
        else if (IsOn && !player.IsLoading && !player.IsPreparing && !player.IsPlaying && !player.IsPaused)
        {
            Item.NewItem(Item.GetSource_NaturalSpawn(), Position.ToWorldCoordinates(), StoredItem);
            StoredItem = -1;
            CurrentContentPath = string.Empty;
        }
        
        player?.Update(Main.gameTimeCache);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["content"] = CurrentContentPath;
        tag["posX"] = TileArea.X;
        tag["posY"] = TileArea.Y;
        tag["isOn"] = IsOn;
    }

    public override void LoadData(TagCompound tag)
    {
        CurrentContentPath = tag.GetString("content");
        TileArea = new Rectangle(tag.GetInt("posX"), tag.GetInt("posY"), 2, 1);
        IsOn = tag.GetBool("isOn");

        ConnectedTVs.Clear();
    }

    public override bool IsTileValidForEntity(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && ModContent.GetModTile(tile.TileType) is BaseMediaPlayerTile;
    }

    public override void OnKill()
    {
        if (StoredItem != -1)
            Item.NewItem(Item.GetSource_NaturalSpawn(), Position.ToWorldCoordinates(), StoredItem);
        player.Stop();
        player.Dispose();
    }
}