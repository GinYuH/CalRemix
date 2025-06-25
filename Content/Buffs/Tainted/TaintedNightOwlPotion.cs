using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedNightOwlBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.NightOwl;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hazy");
            // Description.SetDefault("Darkness falls");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.NightOwl))
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedOwl = true;
        }
    }
}
