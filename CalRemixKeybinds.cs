using Terraria.ModLoader;

namespace CalRemix
{
    public class CalRemixKeybinds : ModSystem
    {
        public static ModKeybind BaroClawHotKey { get; private set; }

        public override void Load()
        {
            // Register keybinds
            BaroClawHotKey = KeybindLoader.RegisterKeybind(Mod, "Baroclaw", ";");
        }

        public override void Unload()
        {
            BaroClawHotKey = null;
        }
    }
}
