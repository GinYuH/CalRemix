using CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest.Temple;
using CalRemix.Content.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest.Temple
{
    public class Phyllite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<PhyllitePlaced>());
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PhylliteWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class EtchedPhylliteBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<EtchedPhylliteBrickPlaced>());
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PhylliteBrick>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class PhylliteBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<PhylliteBrickPlaced>());
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PhylliteBrickWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Phyllite>())
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
    public class IdolizedPhylliteBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<IdolizedPhylliteBrickPlaced>());
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PhylliteBrick>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class LargePhylliteBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<LargePhylliteBrickPlaced>());
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PhylliteBrick>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class PhylliteBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableWall(ModContent.WallType<PhylliteBrickWallPlaced>());
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<PhylliteBrick>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class PhylliteWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() => Item.DefaultToPlaceableWall(ModContent.WallType<PhylliteWallPlaced>());
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<Phyllite>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

}