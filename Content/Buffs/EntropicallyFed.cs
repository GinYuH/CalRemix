using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class EntropicallyFed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.5f;
        }
    }
}
