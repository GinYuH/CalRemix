using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRemix.Content.Items.Armor.RajahHoodlum
{
    [AutoloadEquip(EquipType.Body)]
	public class HoodlumShirt : ModItem
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hopping Hoodlum Shirt");
            //Tooltip.SetDefault(@"10% increased melee speed\n+1 max minion\nEnemies are more likely to target you\nHopping Mad.");
        }


        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = 8;
            Item.defense = 28;
		}

        public override void UpdateEquip(Player player)
		{
            player.GetAttackSpeed(DamageClass.Melee) += .1f;
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