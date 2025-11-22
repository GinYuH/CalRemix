using CalamityMod;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Prefixes
{ 
    public class FolvsPrefix : ModPrefix, ILocalizedModType
    {
        public new string LocalizationCategory => "Prefixes";
        public override PrefixCategory Category => PrefixCategory.Custom;
        public override bool CanRoll(Item item) => !item.Calamity().donorItem;
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            if (!item.Calamity().donorItem)
                yield return new TooltipLine(Mod, "FolvsDonor", CalamityUtils.ColorMessage("- Donor Item -", new Microsoft.Xna.Framework.Color(196, 35, 44)));
        }
    }
}