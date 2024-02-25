using CalamityMod.Particles;
using CalRemix.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class EntropicallyFed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropically Fed");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.5f;
        }
    }
}
