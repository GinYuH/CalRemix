using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedLuckBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Lucky;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Speedrunner Luck");
            Description.SetDefault("Pearl drop rates increased");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Lucky))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedLuck = true;
        }
    }
}
