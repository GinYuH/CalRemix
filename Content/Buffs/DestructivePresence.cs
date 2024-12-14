using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class DestructivePresence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
