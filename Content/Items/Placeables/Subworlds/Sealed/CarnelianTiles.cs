using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;

namespace CalRemix.Content.Items.Placeables.Subworlds.Sealed
{
    public class CarnelianStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CarnelianStonePlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<CarnelianStoneWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class CarnelianDirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CarnelianDirtPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<CarnelianDirtWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class CarnelianWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CarnelianWoodPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<CarnelianWoodWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class CarnelianStoneWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<CarnelianStoneWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<CarnelianStone>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class CarnelianDirtWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<CarnelianDirtWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<CarnelianDirt>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class CarnelianWoodWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<CarnelianWoodWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<CarnelianWood>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}