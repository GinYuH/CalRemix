using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.SingularPoint
{
    public class VirisitePlaced : ModTile
    {
        public static Asset<Texture2D> glow;
        public override void Load()
        {
            glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(10, 23, 17));
            HitSound = SoundID.Tink;
            DustType = DustID.Stone;
            Main.tileBlendAll[Type] = true;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowtex = glow.Value;
            Tile t = Main.tile[i, j];
            spriteBatch.Draw(glowtex, new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset, new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Color.Lerp(Color.SeaGreen, Color.LightSeaGreen, Utils.PingPongFrom01To010(0.5f + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly))));
        }
    }
}