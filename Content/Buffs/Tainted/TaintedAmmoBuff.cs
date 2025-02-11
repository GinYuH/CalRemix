using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs.Tainted
{
    public class TaintedAmmoBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.AmmoReservation;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ammo Unleashed");
            Description.SetDefault("You are leaking ammo!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ammoPotion)
            {
                player.ClearBuff(Type);
                return;
            }
            player.Remix().taintedAmmo = true;
        }
    }
}
