using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedRageBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Rage;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Focused Power");
            // Description.SetDefault("No more luck, only violence");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Rage))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedRage = true;
        }
    }
}
