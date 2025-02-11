using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedEnduranceBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Endurance;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shattered Enemies");
            Description.SetDefault("Your foes grow weaker");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Endurance))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedEndurance = true;
        }
    }
}
