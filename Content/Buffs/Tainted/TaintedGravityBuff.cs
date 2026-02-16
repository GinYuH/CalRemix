using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedGravityBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Gravitation;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Very Distorted");
            // Description.SetDefault("Losing control!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Gravitation))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedGravity = true;
        }
    }
}
