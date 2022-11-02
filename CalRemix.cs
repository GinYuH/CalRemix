using CalRemix.NPCs;
using Terraria.ModLoader;

namespace CalRemix
{
	public class CalRemix : Mod
	{
        public override void PostSetupContent()
        {
            ModLoader.GetMod("CalamityMod").Call("RegisterModCooldowns", this);
            ModLoader.GetMod("CalamityMod").Call("DeclareMiniboss", ModContent.NPCType<LifeSlime>());
        }
    }
}