using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRemix.Content.Items.Armor.RajahHoodlum
{
    [AutoloadEquip(EquipType.Legs)]
	public class HoodlumPants : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("");
            //Tooltip.SetDefault(@"");
        }

		public override void SetDefaults()
		{
            Item.width = 22;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.defense = 17;
            Item.rare = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += .08f;
            player.GetCritChance(DamageClass.Melee) += 8;
            player.maxMinions += 1;
            player.aggro += 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "RajahPelt", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}