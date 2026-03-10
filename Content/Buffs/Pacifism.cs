using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Pacifism : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}