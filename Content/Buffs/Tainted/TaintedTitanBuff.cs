using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedTitanBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Titan;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Forceless");
            // Description.SetDefault("Your weapons have no impact");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Titan))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedTitan = true;
        }
    }
}
