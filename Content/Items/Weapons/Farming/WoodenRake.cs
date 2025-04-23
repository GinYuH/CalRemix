using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.DamageClasses;

namespace CalRemix.Content.Items.Weapons.Farming
{
	public class WoodenRake : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults() 
		{
            Item.useTime = 12; 
			Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<FarmingDamageClass>();
            Item.damage = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(RecipeGroupID.Wood, 10).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
