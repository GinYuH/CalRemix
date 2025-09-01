using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class GrakitBolt : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/LaserProj";
        public ref float BeamLength => ref Projectile.localAI[0];

        public Vector2 originalVelocity
        {
            get => new Vector2(Projectile.ai[0], Projectile.ai[1]);
            set
            {
                Projectile.ai[0] = value.X;
                Projectile.ai[1] = value.Y;
            }
        }

        public ref float state => ref Projectile.ai[2];

        public Vector2 orbitCenter
        {
            get => new Vector2(Projectile.localAI[2], Projectile.localAI[1]);
            set
            {
                Projectile.localAI[2] = value.X;
                Projectile.localAI[1] = value.Y;
            }
        }

        public float orbitAngle;

        public int timer = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.AmethystBolt);
            Projectile.aiStyle = -1;
            Projectile.MaxUpdates = 6;
            Projectile.timeLeft = 30 * Projectile.MaxUpdates;
        }

        public override void AI()
        {
            float orbitRadius = 200;
            if (Projectile.ai[2] == 0)
            {
                if (originalVelocity == Vector2.Zero)
                    originalVelocity = Projectile.velocity;

                if (Projectile.Distance(Main.MouseWorld) < 300)
                {
                    state = 1;
                    orbitCenter = Main.MouseWorld;
                    orbitAngle = (Projectile.Center - orbitCenter).ToRotation();
                }
            }
            else if (Projectile.ai[2] == 1)
            {
                Projectile.penetrate = -1;
                orbitAngle += 0.05f * (Projectile.whoAmI % 2 == 0).ToDirectionInt();
                Projectile.velocity = (orbitCenter + orbitRadius * orbitAngle.ToRotationVector2()) - Projectile.Center;
                Vector2 direction = (Projectile.Center - orbitCenter).SafeNormalize(Vector2.Zero);
                Vector2 ogDirection = originalVelocity.SafeNormalize(Vector2.Zero);

                if (timer > 65)
                {
                    Projectile.penetrate = 1;
                    state = 2;
                    Projectile.velocity = originalVelocity;
                }
                timer++;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.alpha = Utils.Clamp(Projectile.alpha - 25, 0, 255);
            BeamLength = MathHelper.Clamp(BeamLength + 2f, 0f, 100f);
            Lighting.AddLight(Projectile.Center, 1f, 0f, 0.7f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, 600);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Confused, 600);
        }
        public override Color? GetAlpha(Color lightColor) => new Color(250, 50, 200, 0);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(100f, 2f, lightColor, curve: true);

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[2] == 1 && timer > 20)
            {
                modifiers.SourceDamage *= 5;
            }
        }
    }
}