using CalamityMod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions.Recovery
{
    public class AdrenalinePotion : ModItem
    {
        public override bool CanUseItem(Player player) => !player.Calamity().adrenalineModeActive && player.Calamity().AdrenalineEnabled;
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
                TooltipLine lineAdd = new TooltipLine(Mod, "CalRemix:RestorePotion", "Restores 6% Adrenaline");
                tooltips.Insert(tooltips.IndexOf(line) + 1, lineAdd);
            }
        }
        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.GreenYellow, (int)(player.Calamity().adrenalineMax * 0.06f));
            player.Calamity().adrenaline += player.Calamity().adrenalineMax * 0.06f;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient<LesserAdrenalinePotion>(2).
                AddIngredient(ItemID.Bone).
                AddTile(TileID.Bottles).
                Register();
        }
    }
}