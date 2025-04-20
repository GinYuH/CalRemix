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
	public class EssentialEssenceBarPlant : ModTile
	{
		public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateWidth = 60;
            TileObjectData.newTile.CoordinateHeights = [80];
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.DrawYOffset = -64;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            AddMapEntry(Color.Magenta, CreateMapEntryName());
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
            RegisterItemDrop(ModContent.ItemType<EssentialEssenceBar>());
            DustType = DustID.DungeonSpirit;
        }
        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);
            return tile.TileFrameX > 8 && tile.TileFrameX < 72 && tile.TileType == ModContent.TileType<BarPlanterTile>();
        }
        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            tile.TileFrameY = (short)((tile.TileFrameY < 80) ? 80 : 160);
            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, i, j, 1);
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            yield return new Item(ModContent.ItemType<EssentialEssenceBar>(), (tile.TileFrameY >= 160) ? 3 : 1);
        }
        public override bool IsTileSpelunkable(int i, int j) => Framing.GetTileSafely(i, j).TileFrameY >= 160;
    }
}