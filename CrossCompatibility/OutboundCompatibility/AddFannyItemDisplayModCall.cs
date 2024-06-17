using CalRemix.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class AddFannyItemDisplayModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "AddFannyItemDisplay";
            }
        }

        public override string Name => "AddFannyItemDisplay";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(int); // The item ID.
                yield return typeof(float); // The draw scale. Should be 1 for most contexts.
                yield return typeof(Vector2); // The draw offset. Should be <0, 0> for most contexts.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            int itemID = (int)args[1];
            float itemDrawScale = (float)args[2];
            Vector2 drawOffset = (Vector2)args[3];
            message.AddItemDisplay(itemID, itemDrawScale, drawOffset);

            return message;
        }
    }
}
