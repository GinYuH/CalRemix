using CalRemix.UI;
using System;
using System.Collections.Generic;
using Terraria;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public class AddFannyDialogConditionModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "AddFannyDialogCondition";
            }
        }

        public override string Name => "AddFannyDialogCondition";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(HelperMessage); // The dialog instance. This accepts the boxed object variant from CreateFannyDialog.
                yield return typeof(Func<IEnumerable<NPC>, bool>); // The condition. The IEnumerable<NPC> represents currently on-screen NPCs.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            HelperMessage message = (HelperMessage)args[0];
            var appearCondition = (Func<IEnumerable<NPC>, bool>)args[1];
            message.Conditions += metrics => appearCondition(metrics.onscreenNPCs);

            return message;
        }
    }
}
