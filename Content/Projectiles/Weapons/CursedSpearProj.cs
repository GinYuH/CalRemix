using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class CursedSpearProj : ModProjectile
	{
        public override string Texture => "CalRemix/Content/Items/Weapons/CursedSpear";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cursed Spear");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.scale = 1.2f;
            if (Projectile.ai[0] > 90)
            {
                Projectile.velocity.X *= 0.998f;
                Projectile.velocity.Y += 0.3f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 4f;
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.Center + Projectile.velocity, 1, 1, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (Projectile.Calamity().stealthStrike && Projectile.timeLeft % 5 == 0 && Projectile.ai[0] < 180)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(22.5f)) * 0.5f).ClampMagnitude(20, 20), ProjectileID.CursedFlameFriendly, Projectile.damage / 5, Projectile.knockBack / 2, Projectile.owner, ai0: 4);
                proj.DamageType = ModContent.GetInstance<RogueDamageClass>();
                proj.penetrate = 10;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(IchorSpearProj.Hitsound, Projectile.position);
            target.AddBuff(BuffID.CursedInferno, 120);
            for (int i = 0; i < Main.rand.Next(4, 6); i++)
            {
                Vector2 velocity = Projectile.velocity.RotatedByRandom(0.20000000298023224) * Main.rand.NextFloat(0.6f, 1.1f);
                int lifetime = Main.rand.Next(23, 25);
                float scale = Main.rand.NextFloat(0.8f, 1f) * 0.955f;
                Color value = Color.Lerp(Color.Lime, Color.LimeGreen, Main.rand.NextFloat(0.7f));
                value = Color.Lerp(value, Color.Lime, Main.rand.NextFloat());
                GeneralParticleHandler.SpawnParticle(new SparkParticle(Projectile.Center, velocity, affectedByGravity: true, lifetime, scale, value));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, value.Size() / 2f, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}