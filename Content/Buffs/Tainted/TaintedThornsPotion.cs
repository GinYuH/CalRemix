using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedThornsBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Thorns;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shielded Enemies");
            // Description.SetDefault("Maybe one will hit!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Thorns))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedThorns = true;
        }
    }
}
