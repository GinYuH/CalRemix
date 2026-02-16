using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedSpelunkerBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Spelunker;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Magnet");
            // Description.SetDefault("Metal leaks out of the walls");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Spelunker))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedSpelunker = true;
        }
    }
}
