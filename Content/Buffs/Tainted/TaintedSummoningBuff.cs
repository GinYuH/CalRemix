using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedSummoningBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Summoning;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mayfly Minions");
            // Description.SetDefault("They grow up so fast...");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Summoning))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedSummoning = true;
        }
    }
}
