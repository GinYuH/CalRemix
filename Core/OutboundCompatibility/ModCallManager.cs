using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace CalRemix.Core.OutboundCompatibility
{
    // NOTE -- This system is almost functionally equivalent to the one I wrote for the Noxus Boss mod a while ago. - Dominic
    public class ModCallManager : ModSystem
    {
        private static readonly List<ModCallProvider> modCalls = new();

        private static readonly List<string> usedLegacyCommands = new();

        public override void OnModLoad()
        {
            // Load all mod calls.
            foreach (Type t in AssemblyManager.GetLoadableTypes(Mod.Code))
            {
                if (!t.IsSubclassOf(typeof(ModCallProvider)) || t.IsAbstract)
                    continue;

                ModCallProvider call = (ModCallProvider)Activator.CreateInstance(t);
                call.Load();
                modCalls.Add(call);
            }
        }

        internal static object Call(params object[] args)
        {
            if (!args.Any())
                throw new ArgumentException("There must be at least one argument in order to use mod calls!");
            if (args[0] is not string command)
                throw new ArgumentException("The first argument must supply a string that specifies the mod call's type!");

            foreach (ModCallProvider call in modCalls)
            {
                bool legacyMatch = call.LegacyCallCommands?.Any(c => c.Equals(command, StringComparison.OrdinalIgnoreCase)) ?? false;
                bool callMatch = call.CallCommands.Any(c => c.Equals(command, StringComparison.OrdinalIgnoreCase));

                // Keep track of which legacy commands have been used, if any. If a new one is used, it will log a warning for said legacy call and store it in a list.
                // This list exists to ensure that said warnings appear only once, and not every single time the call is used.
                if (legacyMatch && !usedLegacyCommands.Contains(command))
                {
                    CalRemix.instance.Logger.Warn($"The '{command}' mod call has been used! This mod call is not and will not become defunct, but it is considered legacy code. It may be useful to consider renaming it.");
                    usedLegacyCommands.Add(command);
                }

                // Process the mod call if a match was found. The first argument is ommitted since that's simply the command, which was already used.
                // Error handling regarding inputs is performed via the ProcessInternal method.
                if (callMatch || legacyMatch)
                    return call.ProcessInternal(args.Skip(1).ToArray());
            }

            return null;
        }
    }
}
