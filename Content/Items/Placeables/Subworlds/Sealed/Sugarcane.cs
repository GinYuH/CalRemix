using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalamityMod.Items;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;
using Terraria.ObjectData;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Placeables.Subworlds.Sealed
{
    public class PeatSpire : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PeatSpirePlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<PeatOre>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class Neoncane : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<NeoncanePlaced>());
        }
    }
    public class CookieTower : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CookieTowerPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ChocolateChipCookie, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class LightColumn : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<LightColumnPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<LightResidue>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class SealedBush : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedBushPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RottedTendril>())
                .AddIngredient(ModContent.ItemType<Neoncane>())
                .AddIngredient(ModContent.ItemType<LightColumn>())
                .AddIngredient(ModContent.ItemType<PeatSpire>())
                .AddIngredient(ModContent.ItemType<CookieTower>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}