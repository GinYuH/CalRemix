using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class VoidInfusedStoneWallPlaced : ModWall
    {
        public static Color bg = new(125, 0, 125);
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(155, 0, 155), null);
            DustType = DustID.ArgonMoss;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (CalamityUtils.ParanoidTileRetrieval(i, j).IsTileSolid())
                return false;
            int squareSpace = 4;
            int xMod = i % squareSpace;
            int yMod = j % squareSpace;
            bool purple = true;
            if ((xMod <= 1 && yMod <= 1) || (xMod >= 2 && yMod >= 2))
            {
                purple = false;
            }
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition, new Rectangle(0, 0, 16, 16), purple ? bg : Color.Black);
            return false;
        }
    }
}