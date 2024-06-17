using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    // This mod call is a bit of a niche case, but it's useful for instances where a mod defines lore items that do not use a strong reference to base Calamity, and as such do not
    // derive from their LoreItem base class.
    public class MakeItemCountAsLoreItemModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "MakeItemCountAsLoreItem";
            }
        }

        public override string Name => "MakeItemCountAsLoreItem";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(int); // The item ID.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            int itemID = (int)args[0];
            if (!ScreenHelperManager.manuallyDefinedLoreItems.Contains(itemID))
                ScreenHelperManager.manuallyDefinedLoreItems.Add(itemID);

            return null;
        }
    }
}
