using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class Acid : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("You're not on acid, everyone else is");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.03f;
            player.statDefense += 4;
        }
    }
}
