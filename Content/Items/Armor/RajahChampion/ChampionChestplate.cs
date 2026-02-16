using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor.RajahChampion
{
    [AutoloadEquip(EquipType.Body)]
	public class ChampionChestplate : ModItem
	{
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Champion Chestplate");
            /* Tooltip.SetDefault(@"15% increased damage
The armor of a champion feared across the land"); */
        }


        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(3, 0, 0, 0);
            Item.rare = 9;
            //AARarity = 14;
            Item.defense = 55;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = new Color(255, 22, 0);
                }
            }
        }

        public override void UpdateEquip(Player player)
		{
            player.GetDamage(DamageClass.Generic) += .15f;
        }
		
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "HoodlumShirt", 1);
            recipe.AddIngredient(null, "ChampionPlate", 10);
            //recipe.AddTile(null, "ACS");
            recipe.Register();
        }
    }
}