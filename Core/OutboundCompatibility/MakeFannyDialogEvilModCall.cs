using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.Core.OutboundCompatibility
{
    public class MakeFannyDialogEvilModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "MakeFannyDialogEvil";
                yield return "MakeFannyDialogSpokenByEvilFanny";
            }
        }

        public override string Name => "MakeFannyDialogEvil";

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
            return message.SpokenByEvilFanny();
        }
    }
}
