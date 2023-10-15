using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class ChainFannyDialogModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "ChainFannyDialog";
            }
        }

        public override string Name => "ChainFannyDialog";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(FannyMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(FannyMessage); // The dialog to draw after.
                yield return typeof(float); // The appear delay, in seconds.
                yield return typeof(bool); // Whether the parent still needs to be clicked off.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            FannyMessage message = (FannyMessage)args[0];
            FannyMessage parent = (FannyMessage)args[1];
            float appearDelay = (float)args[2];
            bool parentNeedsToBeClickedOff = (bool)args[3];

            // Apply activation requirements relative to the parent message.
            message = message.NeedsActivation(appearDelay);
            parent.AddEndEvent(message.ActivateMessage);
            parent.NeedsToBeClickedOff = parentNeedsToBeClickedOff;

            return message;
        }
    }
}
