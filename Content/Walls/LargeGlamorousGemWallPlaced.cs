using CalamityMod;
using CalamityMod.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class LargeGlamorousGemWallPlaced : MultiVariantModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;

            AddMapEntry(new Color(113, 22, 115));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.PinkCrystalShard, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void PopulateWallVariant(int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = (i % 2) * 468;
            frameYOffset = (j % 2) * 180;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int offsetX = 0;
            int offsetY = 0;

            PopulateWallVariant(i, j, ref offsetX, ref offsetY);

            Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Gray").Value;
            Tile t = Main.tile[i, j];
            spriteBatch.Draw(tex, new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition, new Rectangle(t.WallFrameX + offsetX, t.WallFrameY + offsetY, 32, 32), (Color.Lerp(Color.MediumBlue, Color.Magenta, Utils.PingPongFrom01To010(j % (20 + MathF.Sin(Main.GlobalTimeWrappedHourly)) / 20f))) * 0.7f, 0, new Vector2(0, 0), 1, 0, 0);
            return false;
        }
    }
}
