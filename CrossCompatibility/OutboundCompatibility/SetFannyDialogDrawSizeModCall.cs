using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class SetFannyDialogDrawSizeModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "SetFannyDialogDrawSize";
            }
        }

        public override string Name => "SetFannyDialogDrawSize";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(FannyMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(int); // The max dialog width in pixels. Defaults to 380 without this mod call.
                yield return typeof(float); // The font size factor. Defaults to 1 without this mod call.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            FannyMessage message = (FannyMessage)args[0];
            int dialogWidth = (int)args[1];
            float fontSizeFactor = (float)args[2];
            message.CooldownTime = dialogWidth;
            message.textSize = fontSizeFactor;

            return message;
        }
    }
}
