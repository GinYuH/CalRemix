using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class DaggerofLibertyProj : ModProjectile
	{
        public override string Texture => "CalRemix/Content/Items/Weapons/DaggerofLiberty";
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.timeLeft = 300;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.aiStyle = 2;
            AIType = 48;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft % 2 == 0)
            {
                if (Projectile.Calamity().stealthStrike)
                {
                    Projectile.tileCollide = false;
                    for (int i = 0; i < 2; i++)
                    {
                        GeneralParticleHandler.SpawnParticle(new CritSpark(Projectile.Center + Main.rand.NextVector2Circular(10f, 10f), -Projectile.velocity, Color.PaleGoldenrod, Color.Goldenrod, Main.rand.NextFloat(0.65f, 0.95f), 8, 0f, 2.5f));
                    }
                }
                else
                {
                    if (Main.rand.NextBool() && Projectile.numHits == 0)
                    {
                        GeneralParticleHandler.SpawnParticle(new SparkParticle(Projectile.Center - Projectile.velocity * 0.5f, Projectile.velocity * 0.01f, affectedByGravity: false, 7, 0.7f, Color.PaleGoldenrod * 0.3f));
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Firework_Yellow, Projectile.oldVelocity.X * Main.rand.NextFloat(1.1f, 1.3f), Projectile.oldVelocity.Y * Main.rand.NextFloat(1.1f, 1.3f));
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SpearofDestinyProjectile.Hitsound, Projectile.position);
            if (!Projectile.Calamity().stealthStrike)
            {
                float num = 360f / 5;
                for (int i = 0; (float)i < 5; i++)
                {
                    float angle = MathHelper.ToRadians((float)i * num) + Main.rand.NextFloat(MathHelper.TwoPi);
                    Vector2 vector = new Vector2(6f, 0f).RotatedBy(angle);
                    Vector2 vector2 = new Vector2(3f, 0f).RotatedBy(angle);
                    GeneralParticleHandler.SpawnParticle(new SparkParticle(Projectile.Center + vector, new Vector2(vector2.X, vector2.Y) * Main.rand.NextFloat(0.8f, 1.3f), affectedByGravity: false, Main.rand.Next(23, 28), 1.9f, Main.rand.NextBool(5) ? Color.Gold : Color.PaleGoldenrod));
                }
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0f, ModContent.ProjectileType<LibertyExplosion>(), Projectile.damage / 2, Projectile.knockBack * 2f, Projectile.owner);
            }
            else
            {
                for (int j = 0; j <= 11; j++)
                {
                    Dust obj = Main.dust[Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Firework_Red, 0f, 0f, 0, default(Color), 1.5f)];
                    obj.velocity.Y -= Main.rand.NextFloat(2.5f, 10.5f);
                    obj.velocity.X += Main.rand.NextFloat(-3f, 3f);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, value.Size() / 2f, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}