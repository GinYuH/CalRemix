using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class GetFannyHoverInfoModCall : ModCallProvider<Tuple<bool, int>>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "GetFannyItemHoverInfo";
            }
        }

        public override string Name => "GetFannyItemHoverInfo";

        // No inputs are required, this simply provides information for ease of use with lore items.
        public override IEnumerable<Type> InputTypes => Array.Empty<Type>();

        protected override Tuple<bool, int> ProcessGeneric(params object[] args)
        {
            return new(FannyManager.ReadLoreItem, FannyManager.previousHoveredItem);
        }
    }
}
