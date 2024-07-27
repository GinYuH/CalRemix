using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions.Recovery
{
    public class StealthPotion : ModItem
    {
        public override bool CanUseItem(Player player) => player.Calamity().wearingRogueArmor;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.HealingPotion);
            Item.healLife = 0;
            Item.buffType = 0;
            Item.potion = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("ItemName"));
            if (line != null)
            {
                TooltipLine lineAdd = new TooltipLine(Mod, "CalRemix:RestorePotion", "Restores 15% stealth");
                tooltips.Insert(tooltips.IndexOf(line) + 1, lineAdd);
            }
        }
        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.MediumPurple, (int)(player.Calamity().rogueStealthMax * 100f * 0.15f));
            player.Calamity().rogueStealth += player.Calamity().rogueStealthMax * 0.15f;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient<LesserStealthPotion>(2).
                AddIngredient<SulphuricScale>().
                AddTile(TileID.Bottles).
                Register();
        }
    }
}