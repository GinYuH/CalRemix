using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Lore
{
    public abstract class RemixLoreItem : ModItem, IHoldShiftTooltipItem
    {
        public new string LocalizationCategory => "Items.Lore";

        // All lore items initially have a short tooltip which indicates there is more to be read.
        public override LocalizedText Tooltip => CalamityUtils.GetText($"{LocalizationCategory}.ShortTooltip");

        // When holding SHIFT on a lore item, the default tooltip is removed entirely.
        public bool HidesNormalTooltip => true;

        // Lore items have a flavorful extension indicator tooltip.
        public string ExtensionIndicatorKey => $"{LocalizationCategory}.ShortTooltip";

        // Each line of the extension indicator tooltip is manually colored, so don't provide an override color.
        public Color? ExtensionIndicatorColor => null;

        // The localization key for all lore items' full lore content is just "Lore".
        public string TooltipExtensionKey => "Lore";

        // By default, lore text appears in white, but this can be changed.
        public virtual Color? LoreColor => null;
        public virtual string LoreText => CalRemixHelper.LocalText($"Lore.Items.{Name}").Value;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override bool CanUseItem(Player player) => false;

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.LoreItems;
        }
    }
}
