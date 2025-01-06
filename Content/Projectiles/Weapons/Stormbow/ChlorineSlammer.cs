using CalRemix.Content.DamageClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class ChlorineSlammer : ModProjectile
    {
        public ref float Mode => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

            Projectile.width = 18;
            Projectile.height = 52;

            Projectile.arrow = true;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Projectile.timeLeft = 220;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() * MathHelper.PiOver2;
            Projectile.velocity.Y *= 1.2f;
        }
        public override bool? CanDamage()
        {
            if (Mode == 1)
                return false;

            return null;
        }
    }
}
