using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions.Recovery
{
    public class SuperEnragePotion : ModItem
    {
        public override bool CanUseItem(Player player) => !player.Calamity().rageModeActive && player.Calamity().RageEnabled;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SuperHealingPotion);
            Item.healLife = 0;
            Item.buffType = 0;
            Item.potion = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("ItemName"));
            if (line != null)
            {
                TooltipLine lineAdd = new TooltipLine(Mod, "CalRemix:RestorePotion", "Restores 20% rage");
                tooltips.Insert(tooltips.IndexOf(line) + 1, lineAdd);
            }
        }
        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.Orange, (int)(player.Calamity().rageMax * 0.2f));
            player.Calamity().rage += player.Calamity().rageMax * 0.2f;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient<GreaterEnragePotion>(4).
                AddIngredient<AstralBar>().
                AddTile(TileID.Bottles).
                Register();
        }
    }
}