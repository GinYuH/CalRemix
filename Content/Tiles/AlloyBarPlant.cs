using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
	public class AlloyBarPlant : ModTile
	{
		public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateWidth = 40;
            TileObjectData.newTile.CoordinateHeights = [60];
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.DrawYOffset = -44;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            AddMapEntry(Color.Pink, CreateMapEntryName());
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
            RegisterItemDrop(ModContent.ItemType<AlloyBar>());
            DustType = DustID.BubbleBurst_Pink;
        }
        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);
            return tile.TileFrameX > 8 && tile.TileFrameX < 72 && tile.TileType == ModContent.TileType<BarPlanterTile>();
        }
        public override void RandomUpdate(int i, int j)
        {
            if (!Main.rand.NextBool(20))
                return;
            Tile tile = Framing.GetTileSafely(i, j);
            tile.TileFrameY = (short)((tile.TileFrameY < 60) ? 60 : 120);
            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, i, j, 1);
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            yield return new Item(ModContent.ItemType<AlloyBar>(), (tile.TileFrameY >= 120) ? 2 : 1);
        }
        public override bool IsTileSpelunkable(int i, int j) => Framing.GetTileSafely(i, j).TileFrameY >= 120;
    }
}