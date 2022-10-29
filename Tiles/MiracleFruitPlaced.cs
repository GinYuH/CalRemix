using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CalamityMod.Items.PermanentBoosters;

namespace CalRemix.Tiles
{
    public class MiracleFruitPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Miracle Froota");
            AddMapEntry(new Color(66, 245, 158), name);
        }

        public override void KillMultiTile(int i, int j, int TileFrameX, int TileFrameY)
        {
            Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<MiracleFruit>());
        }
    }
}