using Terraria.ModLoader;

namespace CalRemix
{
	public class CalRemix : Mod
	{
        public override void PostSetupContent()
        {
            ModLoader.GetMod("CalamityMod").Call("RegisterModCooldowns", this);
        }
    }
}