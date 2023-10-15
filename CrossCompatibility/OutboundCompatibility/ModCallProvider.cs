using System;
using System.Collections.Generic;
using System.Linq;

namespace CalRemix.CrossCompatibility.OutboundCompatibility
{
    public abstract class ModCallProvider
    {
        // This encompasses call commands that are outdated. If these calls are used, the call will still be executed as usual, but with a warning log to indicate that it's outdated.
        public virtual IEnumerable<string> LegacyCallCommands
        {
            get;
        }

        // Once a call makes it to a public version, NEVER delete it completely.
        // If it ceases to be valid (such as if the command references content that has been renamed), add it to LegacyCallCommands so that modders can be informed of this change
        // without having their loading operations potentially crash.
        public abstract IEnumerable<string> CallCommands
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract IEnumerable<Type> InputTypes
        {
            get;
        }

        internal object ProcessInternal(params object[] args)
        {
            IEnumerable<Type> expectedInputTypes = InputTypes;
            int expectedInputCount = expectedInputTypes?.Count() ?? 0;

            // Verify that there are a correct amount of arguments.
            if (args.Length != expectedInputCount)
                throw new ArgumentException($"The inputted arguments for the '{Name}' mod call were of an invalid length! {args.Length} arguments were inputted, {expectedInputCount} were expected.");

            // Verify that the arguments are of the correct type.
            for (int i = 0; i < args.Length; i++)
            {
                // i + 1 is used because the 0th argument (aka the mod call command) isn't included in this method.
                Type expectedType = expectedInputTypes.ElementAt(i);
                if (args[i].GetType() != expectedType)
                    throw new ArgumentException($"Argument {i + 1} was invalid for the '{Name}' mod call! It was of type '{args[i].GetType()}', but '{expectedType}' was expected.");
            }

            return Process(args);
        }

        // Feel free to assume that the argument types are valid when setting up mod calls.
        // Any cases where they wouldn't be should be neatly handled via ProcessInternal's error handling.
        protected abstract object Process(params object[] args);

        internal virtual void Load()
        {
        }
    }

    public abstract class ModCallProvider<ReturnType> : ModCallProvider
    {
        protected sealed override object Process(params object[] args) => ProcessGeneric(args);

        protected abstract ReturnType ProcessGeneric(params object[] args);
    }
}
