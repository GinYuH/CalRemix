using CalRemix.UI;
using System;
using System.Collections.Generic;
using Terraria.GameContent;

namespace CalRemix.Core.OutboundCompatibility
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
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(int); // The max dialog width in pixels. Defaults to 380 without this mod call.
                yield return typeof(float); // The font size factor. Defaults to 1 without this mod call.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            int dialogWidth = (int)args[1];
            float fontSizeFactor = (float)args[2];
            message.maxTextWidth = dialogWidth;
            message.textSize = fontSizeFactor;
            message.FormatText(FontAssets.MouseText.Value, dialogWidth);

            return message;
        }
    }
}
