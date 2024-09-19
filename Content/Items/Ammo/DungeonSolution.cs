using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.Items.Ammo
{
	public abstract class DungeonSolution : ModItem
	{
		public override void SetDefaults()
		{
			Item.ammo = AmmoID.Solution;
			Item.width = 12;
			Item.height = 14;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
			Item.consumable = true;
		}

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
			return player.itemAnimation >= player.ActiveItem().useAnimation - 3;
		}
    }
    public class PinkGreySolution : DungeonSolution
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Grey Solution");
            Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Dungeon");
        }

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<PinkGreySpray>() - 145;
            base.SetDefaults();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return base.CanConsumeAmmo(ammo, player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GreenSolution).
                AddIngredient(ItemID.PinkBrick, 40).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class GreenGreySolution : DungeonSolution
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Grey Solution");
            Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Dungeon");
        }

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<GreenGreySpray>() - 145;
            base.SetDefaults();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return base.CanConsumeAmmo(ammo, player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GreenSolution).
                AddIngredient(ItemID.GreenBrick, 40).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
    public class BlueGreySolution : DungeonSolution
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Grey Solution");
            Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Dungeon");
        }

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<BlueGreySpray>() - 145;
            base.SetDefaults();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return base.CanConsumeAmmo(ammo, player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GreenSolution).
                AddIngredient(ItemID.BlueBrick, 40).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}