using CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Items.Weapons.Stormbow;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class CitrusPeelFungus : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new(222, 96, 13));
            DustType = DustID.GemTopaz;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
    public class CitrusPeelFungusSmall : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new(222, 96, 13));
            DustType = DustID.GemTopaz;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }

    public class  CitrusPeelFungusEcho : CitrusPeelFungus
    {
        public override string Texture => "CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/CitrusPeelFungus";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(ModContent.ItemType<MacroMoss>());
            FlexibleTileWand.RubblePlacementLarge.AddVariation(ModContent.ItemType<MacroMoss>(), Type, 0);
        }
    }
}
