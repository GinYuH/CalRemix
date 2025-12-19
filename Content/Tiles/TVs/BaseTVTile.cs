using CalamityMod;
using CalRemix.Content.Items.Tools;
using CalRemix.Core.VideoPlayer;
using CalRemix.UI.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.TVs;

/// <summary>
/// Base class for all TV tiles.
/// </summary>
public abstract class BaseTVTile : ModTile
{
    public abstract Point16 GetTVDimensions();
    public abstract string GetTVStyleName();

    /// <summary>
    /// Get the shader effect for this TV type.
    /// Return null for no shader (default).
    /// </summary>
    public virtual Asset<Effect> GetShaderEffect() => null;

    /// <summary>
    /// Optional: Configure shader parameters before drawing.
    /// Called each frame before the video is rendered with a shader.
    /// Only called if GetShaderEffect() returns a non-null shader.
    /// </summary>
    /// <param name="shader">The shader effect to configure</param>
    /// <param name="tvEntity">The TV entity being drawn</param>
    public virtual void ConfigureShader(Effect shader, TVTileEntity tvEntity) { }

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

        if (player.HeldItem.type == ModContent.ItemType<TVRemoteItem>())
        {
            ModContent.GetInstance<TVRemoteUISystem>().OpenUI(tvEntity);
            return true;
        }

        // Without remote, toggle on/off
        bool wasOn = tvEntity.IsOn;
        tvEntity.IsOn = !tvEntity.IsOn;
        Main.NewText($"TV turned {(tvEntity.IsOn ? "ON" : "OFF")}", Color.Cyan);

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