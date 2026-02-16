using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedHeartBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Heartreach;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heart Attack");
            // Description.SetDefault("Hearts now attack");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Heartreach))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedHeart = true;
        }
    }
}
