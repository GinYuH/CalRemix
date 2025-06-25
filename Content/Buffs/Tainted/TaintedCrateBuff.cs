using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedCrateBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Crate;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gacha Sim");
            // Description.SetDefault("Double or nothing");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.Crate))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedCrate = true;
        }
    }
}
