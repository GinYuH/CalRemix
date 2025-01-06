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
using CalamityMod.Particles;

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
            //Projectile.rotation = Projectile.velocity.ToRotation() * MathHelper.PiOver2;
            Projectile.velocity.Y *= 1.1f;
            Particle smoke = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(-0.2f, -0.6f), Color.DarkCyan * 0.65f, 22, Main.rand.NextFloat(0.4f, 0.55f), 0.3f, Main.rand.NextFloat(-0.2f, 0.2f), false, required: true);
            GeneralParticleHandler.SpawnParticle(smoke);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.position);

            Particle pulse = new DirectionalPulseRing(Projectile.Center + new Vector2(0, Projectile.height / 2), Vector2.Zero, Color.DarkCyan, new Vector2(4f, 2f), Main.rand.NextFloat(-0.2f, 0.2f), 0.01f, 1.5f, 22);
            GeneralParticleHandler.SpawnParticle(pulse);
        }
        public override bool? CanDamage()
        {
            if (Mode == 1)
                return false;

            return null;
        }
    }
}
