using CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class MossyTitanodendronWoodPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(23, 117, 71));
            DustType = DustID.GreenMoss;
            HitSound = SoundID.Grass;
            Main.tileBlendAll[Type] = true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            fail = true;
            if (!effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<TitanodendronWoodPlaced>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Item.NewItem(new EntitySource_TileBreak(i, j), Utils.CenteredRectangle(new Vector2(i, j - 1) * 16, Vector2.One * 16), ModContent.ItemType<MacroMoss>());
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            g = 0.4f;
            b = 0.2f;
        }
    }
}