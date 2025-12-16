using CalamityMod;
using CalRemix.Content.Items.Tools;
using CalRemix.Core.VideoPlayer;
using CalRemix.UI.VideoPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.TVs;

/// <summary>
/// Base class for all TV tiles.
/// Refactored: Each TV now manages its own VideoPlayerCore instance.
/// </summary>
public abstract class BaseTVTile : ModTile
{
    public abstract Point GetTVDimensions();
    public abstract string GetTVStyleName();

    public override bool RightClick(int i, int j)
    {
        TVTileEntity tvEntity = CalamityUtils.FindTileEntity<TVTileEntity>(
            i, j, GetTVDimensions().X, GetTVDimensions().Y, 16);

        if (tvEntity == null)
        {
            Main.NewText("No TV entity found!", Color.Red);
            return false;
        }

        Player player = Main.LocalPlayer;

        // Check if player is holding a TV Remote
        if (player.HeldItem.type == ModContent.ItemType<TVRemoteItem>())
        {
            ModContent.GetInstance<TVRemoteUISystem>().OpenUI(tvEntity);
            return true;
        }

        // Without remote, toggle on/off
        bool wasOn = tvEntity.IsOn;
        tvEntity.IsOn = !tvEntity.IsOn;
        Main.NewText($"TV turned {(tvEntity.IsOn ? "ON" : "OFF")}", Color.Cyan);

        // If we just turned OFF, check if channel should stop
        if (wasOn && !tvEntity.IsOn)
        {
            var manager = ModContent.GetInstance<VideoChannelManager>();
            manager?.StopChannelIfUnused(tvEntity.CurrentChannel);
        }

        return true;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Point16 origin = GetTileOrigin(i, j, GetTVDimensions().X, GetTVDimensions().Y, 16);

        // The TV entity's OnKill will handle disposing its VideoPlayerCore
        ModContent.GetInstance<TVTileEntity>().Kill(origin.X, origin.Y);
    }

    protected static Point16 GetTileOrigin(int i, int j, int width, int height, int sheetSquare)
    {
        Tile tile = Main.tile[i, j];
        int x = i - tile.TileFrameX % (width * sheetSquare) / sheetSquare;
        int y = j - tile.TileFrameY % (height * sheetSquare) / sheetSquare;
        return new Point16(x, y);
    }
}