using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions.Recovery
{
    public class GreaterFlightPotion : ModItem
    {
        public override bool CanUseItem(Player player) => player.wingTimeMax > 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GreaterHealingPotion);
            Item.healLife = 0;
            Item.buffType = 0;
            Item.potion = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("ItemName"));
            if (line != null)
            {
                TooltipLine lineAdd = new TooltipLine(Mod, "CalRemix:RestorePotion", "Restores 21% flight");
                tooltips.Insert(tooltips.IndexOf(line) + 1, lineAdd);
            }
        }
        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.BlueViolet, Math.Round((player.wingTimeMax * 0.21f) / 60f, 3).ToString());
            player.wingTime += (player.wingTime < player.wingTimeMax - player.wingTimeMax * 0.21f) ? player.wingTimeMax * 0.21f : player.wingTimeMax - player.wingTime;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient(ItemID.BottledWater, 3).
                AddIngredient<EssenceofSunlight>(3).
                AddIngredient(ItemID.FallenStar).
                AddTile(TileID.Bottles).
                Register();
        }
    }
}