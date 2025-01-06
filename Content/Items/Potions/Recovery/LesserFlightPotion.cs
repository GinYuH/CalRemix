using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Recovery
{
    public class LesserFlightPotion : ModItem
    {
        public override bool CanUseItem(Player player) => player.wingTimeMax > 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LesserHealingPotion);
            Item.healLife = 0;
            Item.buffType = 0;
            Item.potion = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("ItemName"));
            if (line != null)
            {
                TooltipLine lineAdd = new(Mod, "CalRemix:RestorePotion", CalRemixHelper.LocalText($"Items.{Name}.RestorePotion").Value);
                tooltips.Insert(tooltips.IndexOf(line) + 1, lineAdd);
            }
        }
        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.BlueViolet, Math.Round((player.wingTimeMax * 0.07f) / 60f, 3).ToString());
            player.wingTime += (player.wingTime < player.wingTimeMax - player.wingTimeMax * 0.07f) ? player.wingTimeMax * 0.07f : player.wingTimeMax - player.wingTime;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient(ItemID.Feather, 2).
                AddIngredient(ItemID.Bottle, 2).
                AddTile(TileID.Bottles).
                Register();
        }
    }
}