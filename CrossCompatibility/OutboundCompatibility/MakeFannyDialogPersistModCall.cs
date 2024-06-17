using CalRemix.UI;
using System;
using System.Collections.Generic;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    // By default dialog states will be saved across worlds. This undoes that.
    public class MakeFannyDialogNotPersistModCall : ModCallProvider<object>
    {
        public override IEnumerable<string> CallCommands
        {
            get
            {
                yield return "MakeFannyDialogNotPersist";
            }
        }

        public override string Name => "MakeFannyDialogNotPersist";

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
            message.PersistsThroughSaves = false;

            return message;
        }
    }
}
