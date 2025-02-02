using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedInvisibilityBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Invisibility;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abandoned");
            Description.SetDefault("Where did everyone go?");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Invisibility))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedInvis = true;
        }
    }
}
