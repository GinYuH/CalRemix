using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class Acid : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.03f;
            player.statDefense += 4;
        }
    }
}
