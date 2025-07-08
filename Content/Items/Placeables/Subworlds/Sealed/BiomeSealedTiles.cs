using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalamityMod.Items;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;

namespace CalRemix.Content.Items.Placeables.Subworlds.Sealed
{
    public class Badrock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BadrockPlaced>());
        }
    }
    public class InactivePlumestone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<InactivePlumestonePlaced>());
        }
    }
    public class ActivePlumestone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<ActivePlumestonePlaced>());
        }
    }
    public class Desoilite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DesoilitePlaced>());
        }
    }
    public class TurnipMesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TurnipMeshPlaced>());
        }
    }
    public class TurnipFruit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TurnipFruitPlaced>());
        }
    }
    public class TurnipLeaf: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TurnipLeafPlaced>());
        }
    }
    public class TurnipFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TurnipFleshPlaced>());
        }
    }
    public class PorswineManure: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PorswineManurePlaced>());
        }
    }
    public class RichMud : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<RichMudPlaced>());
        }
    }
    public class Darnwood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DarnwoodPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<DarnwoodWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class VoidInfusedStone: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<VoidInfusedStonePlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<VoidInfusedStoneWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class DarnwoodWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<DarnwoodWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<Darnwood>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class VoidInfusedStoneWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<VoidInfusedStoneWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ModContent.ItemType<VoidInfusedStone>()).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}