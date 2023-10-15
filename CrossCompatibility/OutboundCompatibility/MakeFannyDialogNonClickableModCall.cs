using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    // By default dialog states will be saved across worlds. This undoes that.
    public class MakeFannyDialogNonClickableModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "MakeFannyDialogNonClickable";
            }
        }

        public override string Name => "MakeFannyDialogNonClickable";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(FannyMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            FannyMessage message = (FannyMessage)args[0];
            message.NeedsToBeClickedOff = false;

            return message;
        }
    }
}
