using Terraria.ModLoader;
using Terraria;
using CalRemix.NPCs;

namespace CalRemix;
public class CalRemixKeybindSystem : ModSystem
{
    public static ModKeybind PolyDashKeybind { get; private set; }
    public override void Load()
    {
        CalRemixKeybindSystem.PolyDashKeybind = KeybindLoader.RegisterKeybind(base.Mod, "Polypebral Dash", "Mouse4");
    }

    public override void Unload()
    {
        CalRemixKeybindSystem.PolyDashKeybind = null;
    }

    
}

public static class npcFinder
{
    public static T ModNPC<T>(this NPC npc) where T : ModNPC
    {
        return npc.ModNPC as T;
    }
}

