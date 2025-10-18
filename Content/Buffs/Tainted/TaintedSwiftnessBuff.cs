using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedSwiftnessBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Swiftness;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sneaking");
            // Description.SetDefault("Be careful");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Swiftness))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedSwift = true;
        }
    }
}
