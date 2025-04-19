using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedWarmthBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Warmth;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cooling");
            // Description.SetDefault("Reduced damage from warm sources");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Warmth))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedWarmth = true;
        }
    }
}
