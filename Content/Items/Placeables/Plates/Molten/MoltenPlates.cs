using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Plates;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.World;
using CalamityMod.Items.Placeables.Plates;
using CalRemix.Content.Tiles.Plates.Molten;

namespace CalRemix.Content.Items.Placeables.Plates.Molten
{
    public abstract class BaseMoltenPlate : ModItem
    {
        public abstract string LabName { get; }
        public abstract int PlacedID { get; }
        public abstract int TileCounter { get; }
        public abstract int BasePlate { get; }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.createTile = PlacedID;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.sellPrice(0, 0, 3, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(BasePlate)
                .AddCondition(new Condition("Inside the " + LabName + " core (plates must exist)", () => TileCounter > 22))
                .AddCondition(Condition.NearLava)
                .Register();
        }
    }

    public class MoltenAeroplate : BaseMoltenPlate
    {
        public override string LabName => "Exosphere Lab";

        public override int BasePlate => ModContent.ItemType<Aeroplate>();

        public override int TileCounter => CalRemixWorld.aeroplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenAeroplatePlaced>();
    }

    public class MoltenBloodplate : BaseMoltenPlate
    {
        public override string LabName => "??? Lab";

        public override int BasePlate => ModContent.ItemType<Bloodplate>();

        public override int TileCounter => CalRemixWorld.bloodplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenBloodplatePlaced>();
    }

    public class MoltenHavocplate : BaseMoltenPlate
    {
        public override string LabName => "Underworld Lab";

        public override int BasePlate => ModContent.ItemType<Havocplate>();

        public override int TileCounter => CalRemixWorld.havocplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenHavocplatePlaced>();
    }

    public class MoltenElumplate : BaseMoltenPlate
    {
        public override string LabName => "Snow Lab";

        public override int BasePlate => ModContent.ItemType<Elumplate>();

        public override int TileCounter => CalRemixWorld.elumplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenElumplatePlaced>();
    }

    public class MoltenCinderplate : BaseMoltenPlate
    {
        public override string LabName => "Space Lab";

        public override int BasePlate => ModContent.ItemType<Cinderplate>();

        public override int TileCounter => CalRemixWorld.cinderplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenCinderplatePlaced>();
    }

    public class MoltenNavyplate : BaseMoltenPlate
    {
        public override string LabName => "Sunken Sea Lab";

        public override int BasePlate => ModContent.ItemType<Navyplate>();

        public override int TileCounter => CalRemixWorld.navyplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenNavyplatePlaced>();
    }

    public class MoltenPlagueplate : BaseMoltenPlate
    {
        public override string LabName => "Jungle Lab";

        public override int BasePlate => ModContent.ItemType<Plagueplate>();

        public override int TileCounter => CalRemixWorld.plagueplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenPlagueplatePlaced>();
    }

    public class MoltenOnyxplate : BaseMoltenPlate
    {
        public override string LabName => "Cavern Lab";

        public override int BasePlate => ModContent.ItemType<Onyxplate>();

        public override int TileCounter => CalRemixWorld.onyxplateTiles;

        public override int PlacedID => ModContent.TileType<MoltenOnyxplatePlaced>();
    }
}