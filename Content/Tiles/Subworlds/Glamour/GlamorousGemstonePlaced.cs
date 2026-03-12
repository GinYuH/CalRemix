using CalamityMod;
using CalamityMod.Rarities;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.Glamour
{
    public class GlamorousGemstonePlaced : ModTile
    {
        public static Asset<Texture2D> gray;
        public override void Load()
        {
            gray = ModContent.Request<Texture2D>(Texture + "_Gray");
        }
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(196, 45, 207));
            DustType = DustID.PinkCrystalShard;
            Main.tileBlendAll[Type] = true;
            HitSound = BetterSoundID.ItemIceBreak;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = gray.Value;
            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
            float edge = Main.maxTilesX * 0.86f * 16;
            float edgeEnd = Main.maxTilesX * 0.91f * 16;
            float fadeOff = Utils.GetLerpValue(edgeEnd, edge, Main.LocalPlayer.Center.X, true);
            if (!SubworldSystem.IsActive<GlamourSubworld>())
                fadeOff = 1;
            Color col = (Color.Lerp(Color.DarkSlateBlue, Color.DarkViolet, Utils.PingPongFrom01To010(i % (50 + MathF.Sin(Main.GlobalTimeWrappedHourly + 2)) / 50f)));
            if (SubworldSystem.IsActive<GlamourSubworld>())
            {
                col = Color.Lerp(Color.Black, col, fadeOff);
                col = Color.Lerp(col, Lighting.GetColor(i, j).MultiplyRGBA(col), Utils.GetLerpValue((float)Main.worldSurface, (float)Main.worldSurface + 5, j, true));
            }
            spriteBatch.Draw(tex, new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition, new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), col);
            return false;
        }
    }
}