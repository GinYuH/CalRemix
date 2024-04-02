using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles.PlaguedJungle;

namespace CalRemix.Items.Placeables.PlaguedJungle
{
    public class PlaguedGrass : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedGrass>();
		}
	}
	public class PlaguedMud : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedMud>();
		}
		public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient(ModContent.ItemType<Items.Placeables.PlaguedJungle.PlaguedMudWall>(), 1).
            AddTile(TileID.WorkBenches).
            Register();
		}
	}
	public class PlaguedStone : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedStone>();
		}
	}
	public class PlaguedSand : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedSand>();
		}
	}
	public class PlaguedSilt : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedSilt>();
		}
	}
	public class OvergrownPlaguedStone : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.OvergrownPlaguedStone>();
		}
	}
	public class PlaguedPipe : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedPipe>();
		}
	}
	public class Sporezol : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.Sporezol>();
		}
	}
	public class PlaguedHive : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedHive>();
		}
	}
	public class PlaguedClay : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<Tiles.PlaguedJungle.PlaguedClay>();
		}
	}
	public class PlaguedMudWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plagued Mud Wall");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.createWall = ModContent.WallType<PlaguedMudWallSafe>();
		}

		public override void AddRecipes()
        {
            CreateRecipe(4).
            AddIngredient(ModContent.ItemType<Items.Placeables.PlaguedJungle.PlaguedMud>(), 1).
            AddTile(TileID.WorkBenches).
            Register();
		}
	}
	public class PlaguedStoneWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plagued Stone Wall");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.createWall = ModContent.WallType<PlaguedStoneWallSafe>();
		}

		public override void AddRecipes()
        {
            CreateRecipe(4).
			AddIngredient(ModContent.ItemType<Items.Placeables.PlaguedJungle.PlaguedStone>(), 1).
			AddTile(TileID.WorkBenches).
			Register();
		}
	}
	public class PlaguedVineWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plagued Vine Wall");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.createWall = ModContent.WallType<PlaguedVineWallSafe>();
		}
	}
	public class PlaguedPipeWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plagued Pipe Wall");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.createWall = ModContent.WallType<Tiles.PlaguedJungle.PlaguedPipeWall>();
		}
	}
}