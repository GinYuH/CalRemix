using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class Warath : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warath");
            Description.SetDefault("Your minions are angry");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<SummonDamageClass>() += 0.1f;
        }
    }
}
