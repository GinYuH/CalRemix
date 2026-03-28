using CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class TableFungus : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 3;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.DrawXOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.GreenBlood;
            AddMapEntry(new Color(140, 83, 69));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
    public class TableFungusAlt : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 3;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile, 2, 0);
            TileObjectData.newTile.DrawXOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.GreenBlood;
            AddMapEntry(new Color(140, 83, 69));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }

    public class TableFungusEcho : TableFungus
    {
        public override string Texture => "CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/TableFungus";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(ModContent.ItemType<MacroMoss>());
            FlexibleTileWand.RubblePlacementLarge.AddVariation(ModContent.ItemType<MacroMoss>(), Type, 0);
        }
    }

    public class TableFungusAltEcho : TableFungus
    {
        public override string Texture => "CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/TableFungusAlt";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(ModContent.ItemType<MacroMoss>());
            FlexibleTileWand.RubblePlacementLarge.AddVariation(ModContent.ItemType<MacroMoss>(), Type, 0);
        }
    }
}
