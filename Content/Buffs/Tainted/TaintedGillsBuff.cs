using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedGillsBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Gills;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrophobia");
            Description.SetDefault("Drowning makes you panic");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Gills))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedGills = true;
        }
    }
}
