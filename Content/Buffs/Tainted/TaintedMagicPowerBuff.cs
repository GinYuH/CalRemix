using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedMagicPowerBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.MagicPower;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Telekinesis");
            // Description.SetDefault("Toss enemies around with magic");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.MagicPower))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedMagic = true;
        }
    }
}
