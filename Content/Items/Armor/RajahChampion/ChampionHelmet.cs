using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Armor.RajahChampion
{
    [AutoloadEquip(EquipType.Head)]
    public class ChampionHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Champion Greathelm");
            /* Tooltip.SetDefault(@"35% increased Melee damage & critical strike chance
10% increased non-melee damage
18% increased melee speed
The armor of a champion feared across the land"); */
        }

        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.value = Item.sellPrice(3, 0, 0, 0);
            Item.rare = 9;
            //AARarity = 14;
            Item.defense = 39;
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

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("ChampionChestplate").Type && legs.type == Mod.Find<ModItem>("ChampionGreaves").Type;
		}

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.CalRemix.Equipset.ChampionHelmetBonus");
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.ChampionMe = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += .25f;
            player.GetCritChance(DamageClass.Melee) += 35;
            player.GetDamage(DamageClass.Generic) += .1f;
            player.GetAttackSpeed(DamageClass.Melee) += .15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "HoodlumHood", 1);
            recipe.AddIngredient(null, "ChampionPlate", 10);
            //recipe.AddTile(null, "ACS");
            recipe.Register();
        }
    }
}