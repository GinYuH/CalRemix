using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class RegisterFannyDialogModCall : ModCallProvider
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "RegisterFannyDialog";
            }
        }

        public override string Name => "RegisterFannyDialog";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(FannyMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
            }
        }

        protected override object Process(params object[] args)
        {
            FannyMessage message = (FannyMessage)args[0];
            FannyManager.LoadFannyMessage(message);

            return null;
        }
    }
}
