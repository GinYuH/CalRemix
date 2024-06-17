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
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
            }
        }

        protected override object Process(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            //ScreenHelperManager.LoadMessage(message);

            return null;
        }
    }
}
