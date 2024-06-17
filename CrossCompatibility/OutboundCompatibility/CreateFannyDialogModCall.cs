using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    // An object is returned due to the fact that the dialog relies considerably on custom-made types within this mod, which of course cannot be used by the mod using the call
    // unless they use a dedicated reference. It still fundamentally represents a FannyMessage type, just boxed.

    // Modifiers are applied via separate mod calls on the resulting object, rather than stuffing all of the various possibilities in a single, terrifying parameter list.
    public class CreateFannyDialogModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "CreateFannyDialog";
            }
        }

        public override string Name => "CreateFannyDialog";

        public override IEnumerable<Type> InputTypes
        {
            get
            {
                yield return typeof(string); // Identifier. It is advisable to prefixe with the mod who performed the call, to prevent collisions.
                yield return typeof(string); // Dialog text.
                yield return typeof(string); // Portrait identifier. Refer to Fanny.LoadFannyPortraits for a list of available options.
            }
        }

        protected override object ProcessGeneric(params object[] args)
        {
            string identifier = (string)args[0];
            string dialog = (string)args[1];
            string portrait = (string)args[2];
            return new HelperMessage(identifier, dialog, portrait);
        }
    }
}
