using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalamityMod.Items;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Placeables.Subworlds.Sealed
{
    public class SealedStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedStonePlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SealedStoneWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class SealedDirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedDirtPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SealedDirtWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class SealedIce : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedIcePlaced>());
        }
    }
    public class SealedLamp : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedLampPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(50).
                AddIngredient(ModContent.ItemType<TurnipFruit>()).
                AddIngredient(ModContent.ItemType<FrozenSealedTear>()).
                AddIngredient(ModContent.ItemType<RefinedCarnelianite>()).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class SealedStoneBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedStoneBrickPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SealedStone>()).
                AddTile(TileID.HeavyWorkBench).
                Register();
        }
    }
    public class SealedBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedBrickPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SealedStone>(), 2).
                AddTile(TileID.Furnaces).
                Register();
        }
    }
    public class SealedBlackSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedBlackSandPlaced>());
        }
    }
    public class SealedWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SealedWoodPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SealedWoodWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class FrozenSealedTearOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<FrozenSealedTearOrePlaced>());
        }
    }
    public class PeatOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PeatOrePlaced>());
        }
    }
    public class MonoriumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MonoriumOrePlaced>());
        }
    }
    public class CarnelianiteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CarnelianiteOrePlaced>());
        }
    }
    public class SealedStoneWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SealedStoneWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<SealedStone>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class SealedDirtWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SealedDirtWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<SealedDirt>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class SealedWoodWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SealedWoodWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<SealedWood>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}