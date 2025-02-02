using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedRegenerationBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Regeneration;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vampiric");
            Description.SetDefault("Drain them");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Regeneration))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedRegen = true;
        }
    }
}
