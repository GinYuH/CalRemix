using Terraria.ModLoader;
using System;

namespace CalRemix
{
    public class CalRemixKeybindSystem : ModSystem
    {
        public static ModKeybind VerbotenGunHotkey { get; private set; }
     
        public override void Load()
        {
            VerbotenGunHotkey = KeybindLoader.RegisterKeybind(Mod, "VerbotenGunHotkey", "V");
        }
        public override void Unload()
        {
            VerbotenGunHotkey = null;
        }

    }
}
