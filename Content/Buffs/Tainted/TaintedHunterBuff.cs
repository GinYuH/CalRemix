using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedHunterBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Hunter;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infamy");
            // Description.SetDefault("Everyone wants to be your enemy");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Hunter))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedHunt = true;
        }
    }
}
