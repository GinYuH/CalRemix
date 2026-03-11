using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class GlamorousGemWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(113, 22, 115));
            DustType = DustID.PinkCrystalShard;
            HitSound = BetterSoundID.ItemIceBreak;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Gray").Value;
            Tile t = Main.tile[i, j];
            spriteBatch.Draw(tex, new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition, new Rectangle(t.WallFrameX, t.WallFrameY, 32, 32), (Color.Lerp(Color.MediumBlue, Color.Magenta, Utils.PingPongFrom01To010(j % (20 + MathF.Sin(Main.GlobalTimeWrappedHourly)) / 20f))) * 0.7f, 0, new Vector2(0, 0), 1, 0, 0);
            return false;
        }
    }
}