using CalamityMod;
using CalRemix.Core.VideoPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.TVs;

/// <summary>
/// Base class for all TV tiles.
/// Handles common tile behavior (placement, breaking, interaction).
/// </summary>
public abstract class BaseTVTile : ModTile
{
    public abstract Point GetTVDimensions();
    public abstract string GetTVStyleName();

    public override bool RightClick(int i, int j)
    {
        TVTileEntity tvEntity = CalamityUtils.FindTileEntity<TVTileEntity>(i, j, GetTVDimensions().X, GetTVDimensions().Y, 16);
        if (tvEntity == null)
        {
            Main.NewText("No TV entity found!", Color.Red);
            return false;
        }

        var manager = ModContent.GetInstance<VideoChannelManager>();
        var player = manager?.GetChannel(tvEntity.CurrentChannel);

        // Check if this is the only TV on this channel
        int tvsOnChannel = 0;
        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is TVTileEntity tv && tv.CurrentChannel == tvEntity.CurrentChannel && tv.IsOn)
            {
                tvsOnChannel++;
            }
        }

        bool isOnlyTV = tvsOnChannel == 1 && tvEntity.IsOn;

        // If video is playing and this is the only TV, stop the channel
        if (player != null && player.IsPlaying && isOnlyTV)
        {
            manager.StopChannel(tvEntity.CurrentChannel);
            Main.NewText($"Channel {tvEntity.CurrentChannel} stopped", Color.Yellow);
            tvEntity.IsOn = false;
            Main.NewText($"TV turned OFF", Color.Cyan);
        }
        // If no video is playing and TV is on, start playing
        else if ((player == null || !player.IsPlaying) && tvEntity.IsOn)
        {
            manager.PlayOnChannel(tvEntity.CurrentChannel, "Calamity Mod");
            Main.NewText($"Playing on channel {tvEntity.CurrentChannel}", Color.Yellow);
        }
        // Otherwise just toggle this TV on/off
        else
        {
            tvEntity.IsOn = !tvEntity.IsOn;
            Main.NewText($"TV turned {(tvEntity.IsOn ? "ON" : "OFF")}", Color.Cyan);
        }

        return true;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Point16 origin = GetTileOrigin(i, j, GetTVDimensions().X, GetTVDimensions().Y, 16);

        // Get the TV entity before killing it
        TVTileEntity tvEntity = CalamityUtils.FindTileEntity<TVTileEntity>(origin.X, origin.Y, GetTVDimensions().X, GetTVDimensions().Y, 16);
        int channelId = tvEntity?.CurrentChannel ?? -1;

        // Kill the tile entity
        ModContent.GetInstance<TVTileEntity>().Kill(origin.X, origin.Y);

        // If this was the last TV on this channel, stop the channel
        if (channelId >= 0)
        {
            if (!VideoChannelManager.IsChannelInUse(channelId))
            {
                var manager = ModContent.GetInstance<VideoChannelManager>();
                manager.StopChannel(channelId);
                Main.NewText($"Channel {channelId} stopped (no active TVs)", Color.Gray);
            }
        }
    }

    protected static Point16 GetTileOrigin(int i, int j, int width, int height, int sheetSquare)
    {
        Tile tile = Main.tile[i, j];
        int x = i - tile.TileFrameX % (width * sheetSquare) / sheetSquare;
        int y = j - tile.TileFrameY % (height * sheetSquare) / sheetSquare;
        return new Point16(x, y);
    }

    /*
    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Point16 orig = GetTileOrigin(i, j, GetTVDimensions().X, GetTVDimensions().Y, 16);
        if (orig.X != i || orig.Y != j)
            return;

        // Only register once per frame
        if (Main.drawToScreen)
            return;

        TVTileEntity tvEntity = CalamityUtils.FindTileEntity<TVTileEntity>(i, j, GetTVDimensions().X, GetTVDimensions().Y, 16);
        if (tvEntity == null)
            return;

        // Calculate screen area in world coordinates
        int tile = Main.tile[i, j].TileType;
        Rectangle worldArea = new(i * 16, j * 16, TVTileEntity.TileData[tile].TileSize.X * 16, TVTileEntity.TileData[tile].TileSize.Y * 16);
        worldArea.X += TVTileEntity.TileData[tile].ScreenOffsets.X;
        worldArea.Y += TVTileEntity.TileData[tile].ScreenOffsets.Y;
        worldArea.Width -= TVTileEntity.TileData[tile].ScreenOffsets.X;
        worldArea.Height -= TVTileEntity.TileData[tile].ScreenOffsets.Y;
        worldArea.Width += TVTileEntity.TileData[tile].ScreenOffsets.Width;
        worldArea.Height += TVTileEntity.TileData[tile].ScreenOffsets.Height;

        // Convert to screen space (relative to camera)
        Vector2 worldPos = new Vector2(worldArea.X, worldArea.Y);
        Vector2 screenPos = worldPos - Main.screenPosition;

        // Apply zoom manually - scale both position and size around screen center
        float zoom = Main.GameZoomTarget;
        Vector2 screenCenter = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);

        // Scale position around screen center correctly
        Vector2 finalPos = (screenPos * zoom) + (screenCenter - screenCenter * zoom);
        Vector2 finalSize = new Vector2(worldArea.Width, worldArea.Height) * zoom;

        Rectangle screenArea = new Rectangle(
            (int)finalPos.X,
            (int)finalPos.Y,
            (int)finalSize.X,
            (int)finalSize.Y
        );

        // Register for drawing in the separate system
        TVDrawSystem.RegisterTVForDrawing(screenArea, tvEntity);

    }
    */
}