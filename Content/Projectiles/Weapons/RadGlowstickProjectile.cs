using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class RadGlowstickProjectile : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/RadGlowstick";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Glowstick);
            AIType = ProjectileID.Glowstick;
            Projectile.damage = 23;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
    }
}
