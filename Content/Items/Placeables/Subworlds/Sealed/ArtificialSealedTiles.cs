using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Placeables.Subworlds.Sealed
{
    public class GastrogelBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GastrogelBlockPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ModContent.ItemType<Gastrogel>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class AstrogelBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<AstrogelBlockPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ModContent.ItemType<Astrogel>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class GildedHardlightBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GildedHardlightBlockPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ModContent.ItemType<GildedShard>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class VoidInfusedTurnipFruit: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<VoidInfusedTurnipFruitPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TurnipFruit>())
                .AddIngredient(ModContent.ItemType<VoidInfusedStone>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class GroundFleshBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GroundFleshBlockPlaced>());
        }
    }
    public class CornSquare : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CornSquarePlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VoidInfusedTurnipFruit>())
                .AddIngredient(ModContent.ItemType<PeatOre>())
                .AddIngredient(ModContent.ItemType<FrozenSealedTearOre>())
                .AddIngredient(ModContent.ItemType<CarnelianiteOre>())
                .AddIngredient(ModContent.ItemType<LightResidue>())
                .AddIngredient(ModContent.ItemType<GildedHardlightBlock>())
                .AddIngredient(ModContent.ItemType<AstrogelBlock>())
                .AddIngredient(ModContent.ItemType<GastrogelBlock>())
                .AddIngredient(ModContent.ItemType<GroundFleshBlock>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}