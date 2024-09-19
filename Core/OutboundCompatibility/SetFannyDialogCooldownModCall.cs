using CalamityMod;
using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.Core.OutboundCompatibility
{
    public class SetFannyDialogCooldownModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "SetFannyDialogCooldown";
            }
        }

        public override string Name => "SetFannyDialogCooldown";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(float); // The dialog cooldown in seconds. This defaults to 60, or one minute, if this mod call is not used.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            float dialogCooldown = (float)args[1];
            message.CooldownTime = CalamityUtils.SecondsToFrames(dialogCooldown);

            return message;
        }
    }
}
