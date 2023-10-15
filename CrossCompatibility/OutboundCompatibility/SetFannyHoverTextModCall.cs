using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class SetFannyHoverTextModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "SetFannyHoverText";
            }
        }

        public override string Name => "SetFannyHoverText";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(FannyMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(string); // The hover text.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            FannyMessage message = (FannyMessage)args[0];
            message.SetHoverTextOverride((string)args[1]);

            return message;
        }
    }
}
