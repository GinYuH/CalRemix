using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class ClockArrowForNightsEdgeBow : ModProjectile
    {
        public override string Texture => "CalamityMod/Items/Ammo/SproutingArrow";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 150;

        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 1.1f;
            }
        }
        public override Color? GetAlpha(Color lightColor) => new Color(255, 0, 255, 127);
    }
}
