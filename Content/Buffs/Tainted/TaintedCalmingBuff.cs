using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedCalmingBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Calm;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Weak Hordes");
            // Description.SetDefault("More seek to challenge you, but fall in fear");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Calm))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedCalm = true;
        }
    }
}
