using CalamityMod;
using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class SetFannyDialogDurationModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "SetFannyDialogDuration";
            }
        }

        public override string Name => "SetFannyDialogDuration";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(float); // The dialog duration in seconds. This defaults to 5 seconds if this mod call is not used.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            float dialogDuration = (float)args[1];
            message.messageDuration = CalamityUtils.SecondsToFrames(dialogDuration);

            return message;
        }
    }
}
