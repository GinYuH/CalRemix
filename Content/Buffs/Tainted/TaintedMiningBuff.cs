using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedMiningBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Mining;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mining Strength");
            // Description.SetDefault("Your pickaxe feels heavier...");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Mining))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedMining = true;
        }
    }
}
