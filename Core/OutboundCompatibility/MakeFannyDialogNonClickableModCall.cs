using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.Core.OutboundCompatibility
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
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            message.CantBeClickedOff = true;

            return message;
        }
    }
}
