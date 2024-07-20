using Terraria.ModLoader;

namespace CalRemix
{
    public class CalRemixKeybinds : ModSystem
    {
        public static ModKeybind BaroClawHotKey { get; private set; }
        public static ModKeybind PolyDashKeybind { get; private set; }
        public static ModKeybind StealthPotKeybind { get; private set; }
        public static ModKeybind IonoLightningKeybind { get; private set; }
        public override void Load()
        {
            // Register keybinds
            BaroClawHotKey = KeybindLoader.RegisterKeybind(Mod, "Baroclaw", ";");
            PolyDashKeybind = KeybindLoader.RegisterKeybind(Mod, "Polypebral Dash", "Mouse4");
            StealthPotKeybind = KeybindLoader.RegisterKeybind(Mod, "Quick Stealth", "L");
            IonoLightningKeybind = KeybindLoader.RegisterKeybind(Mod, "Ionic Lightning", "]");
        }
        public override void Unload()
        {
            BaroClawHotKey = null;
            PolyDashKeybind = null;
            StealthPotKeybind = null;
            IonoLightningKeybind = null;
        }
    }
}
