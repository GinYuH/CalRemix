using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Armor.RajahChampion
{
    [AutoloadEquip(EquipType.Head)]
    public class ChampionHeaddress : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Champion Headdress");
            /* Tooltip.SetDefault(@"70% increased minion damage
10% increased non-minion damage
+7 maximum Minions
+2 maximum sentries 
The armor of a champion feared across the land"); */
        }

        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.value = Item.sellPrice(3, 0, 0, 0);
            Item.rare = 9;
            //AARarity = 14;
            Item.defense = 27;
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
            player.setBonus = Language.GetTextValue("Mods.CalRemix.Equipset.ChampionHeaddressBonus");
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.Baron = true;
            modPlayer.ChampionSu = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(Mod.Find<ModBuff>("BaronBuff").Type) == -1)
                {
                    player.AddBuff(Mod.Find<ModBuff>("BaronBuff").Type, 3600, true);
                }
                if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("BaronBunny").Type] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, -1f, Mod.Find<ModProjectile>("BaronBunny").Type, 100, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }


        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += .6f;
            player.GetDamage(DamageClass.Generic) += .1f;
            player.maxMinions += 7;
            player.maxTurrets += 2;
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