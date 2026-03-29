using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class LichenTitanodendronWoodWallPlaced : ModWall
    {
        public static Asset<Texture2D> glow;

        public override void Load()
        {
            glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(18, 59, 44), null);
            DustType = DustID.Clentaminator_Cyan;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].HasTile)
                return;
            r = 0.1f;
            g = 0.7f;
            b = 0.5f;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = glow.Value;
            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
            spriteBatch.Draw(tex, new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset + new Vector2(-8, -8), new Rectangle(t.WallFrameX, t.WallFrameY, 32, 32), Color.White * 0.1f, 0, Vector2.Zero, 1, 0, 0);
        }
    }
    public class LichenUnsafeTitanodendronWoodWallPlaced : LichenTitanodendronWoodWallPlaced
    {
        public override string Texture => "CalRemix/Content/Walls/LichenTitanodendronWoodWallPlaced";
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(18, 59, 44), null);
            DustType = DustID.Clentaminator_Cyan;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}
