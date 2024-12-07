using CalamityMod;
using CalamityMod.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class InfraredSights : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().infraredSights = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine t = tooltips.Find((TooltipLine t) => t.Text.Contains("{0}"));
            if (t != null)
            {
                string newText = t.Text.Replace("{0}", CalRemixKeybinds.InfraredSightsKeybind.TooltipHotkeyString());
                t.Text = newText;
            }
        }
    }
}
