using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedLoveBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Lifeforce;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Idol");
            // Description.SetDefault("Everyone is tearing each other apart to get to you");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Lovestruck))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedLove = true;
        }
    }
}
