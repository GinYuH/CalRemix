using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedStinkBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Stinky;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disgusting");
            Description.SetDefault("Even noseless enemies think you smell awful");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Stinky))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedStink = true;
        }
    }
}
