using CalamityMod;
using CalamityMod.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class Baroclaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Baroclaw");
            // Tooltip.SetDefault("The crab secret revealed!\n"+"Press x to chain a nearby enemy with crab claws");
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.baroclaw = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CrabStatue, 1).
                AddIngredient(ItemID.StoneBlock, 50).
                AddTile(TileID.Anvils).
                Register();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine t = tooltips.Find((TooltipLine t) => t.Text.Contains("{0}"));
            if (t != null)
            {
                string newText = t.Text.Replace("{0}", CalRemixKeybinds.BaroClawHotKey.TooltipHotkeyString());
                t.Text = newText;
            }
        }
    }
}
