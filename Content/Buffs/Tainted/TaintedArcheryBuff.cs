using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedArcheryBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Archery;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reckless Archer");
            Description.SetDefault("Arrows everywhere!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.archery)
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedArchery = true;
        }
    }
}
