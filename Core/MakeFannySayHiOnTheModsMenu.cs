using Terraria.ModLoader;

namespace CalRemix.Core;

internal sealed class MakeFannySayHiOnTheModsMenu : ILoadable
{
    void ILoadable.Load(Mod mod)
    {
        FannyModListEdit.Load(mod);
    }

    void ILoadable.Unload() { }
}