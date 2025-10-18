using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedBuilderBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Builder;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Resourceful");
            // Description.SetDefault("How does this physically work???");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Builder))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedBuilder = true;
        }
    }
}
