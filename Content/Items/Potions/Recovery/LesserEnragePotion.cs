using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions.Recovery
{
    public class LesserEnragePotion : ModItem
    {
        public override bool CanUseItem(Player player) => !player.Calamity().rageModeActive && player.Calamity().RageEnabled;
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
            CombatText.NewText(player.getRect(), Color.Orange, (int)(player.Calamity().rageMax * 0.05f));
            player.Calamity().rage += player.Calamity().rageMax * 0.05f;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient<AncientBoneDust>(2).
                AddIngredient(ItemID.Bottle, 2).
                AddTile(TileID.Bottles).
                Register();
        }
    }
}