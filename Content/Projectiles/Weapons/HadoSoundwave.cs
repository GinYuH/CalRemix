using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class HadoSoundwave : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Hadopelagic Soundwave");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<EidolicWailSoundwave>());
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            if (Projectile.localAI[0] < 1f)
            {
                Projectile.localAI[0] += 0.05f;
                Projectile.scale += 0.05f;
                Projectile.width = (int)(36f * Projectile.scale);
                Projectile.height = (int)(36f * Projectile.scale);
            }
            Projectile.localAI[1]--;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= (int)Projectile.localAI[0];
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);
            target.AddBuff(BuffID.Electrified, 240);
            Projectile.velocity *= 0.85f;
            if (Projectile.localAI[1] <= 0)
            {
                float startDist = Main.rand.NextFloat(260f, 270f);
                Vector2 startDir = Main.rand.NextVector2Unit();
                Vector2 startPoint = target.Center + (startDir * startDist);

                float echoSpeed = Main.rand.NextFloat(15f, 18f);
                Vector2 velocity = startDir * (-echoSpeed);
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromAI(), startPoint, velocity, ModContent.ProjectileType<EidolicWailSoundwave>(), Projectile.damage / 3, Projectile.knockBack / 2, Projectile.owner)];
                    proj.tileCollide = false;
                    proj.timeLeft = 60;
                    proj.scale = 1f;
                    proj.localAI[0] = 1f;
                }
                Projectile.localAI[1] = 60;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte color = (byte)(Projectile.timeLeft * 3);
                byte alpha = (byte)(100f * (color / 255f));
                return new Color(color, color, color, alpha);
            }
            return new Color(255, 255, 255, 100);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}