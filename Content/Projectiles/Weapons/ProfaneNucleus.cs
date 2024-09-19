using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Magic;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class ProfaneNucleus : ModProjectile
	{
        public override string Texture => "CalRemix/Content/Items/Weapons/ProfanedNucleus";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Profaned Nucleus");
		}
        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01f && Projectile.velocity.X < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.05f;

            if (Projectile.Calamity().stealthStrike)
            {
                Projectile.ai[1]++;
                for (int i = 0; i < 1; i++)
                {
                    if (Projectile.ai[1] % 1 == 0)
                    {
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RancorFog>(), Projectile.damage, 0);
                        if (p.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[p].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int p = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), Projectile.damage, 0);
            if (p.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[p].DamageType = ModContent.GetInstance<RogueDamageClass>();
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                float num461 = 25f;
                num461 *= 0.7f;
                Projectile.ai[0] += 4f;
                int num462 = 0;
                while ((float)num462 < num461)
                {
                    float num463 = (float)Main.rand.Next(-30, 31);
                    float num464 = (float)Main.rand.Next(-30, 31);
                    float num465 = (float)Main.rand.Next(9, 27);
                    float num466 = (float)Math.Sqrt((double)(num463 * num463 + num464 * num464));
                    num466 = num465 / num466;
                    num463 *= num466;
                    num464 *= num466;
                    int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num467];
                    dust.noGravity = true;
                    dust.position.X = Projectile.Center.X;
                    dust.position.Y = Projectile.Center.Y;
                    dust.position.X += (float)Main.rand.Next(-10, 11);
                    dust.position.Y += (float)Main.rand.Next(-10, 11);
                    dust.velocity.X = num463;
                    dust.velocity.Y = num464;
                    num462++;
                }
                int Amount = 8;
                float Extra = Main.rand.Next(-20, 20) / MathHelper.Pi;
                int ExtraSpeed = Main.rand.Next(-2, 4);
                float variance = MathHelper.TwoPi / Amount;
                for (int i = 0; i < Amount; i++)
                {
                    Vector2 velocity = new Vector2(0f, 10 + ExtraSpeed);
                    velocity = velocity.RotatedBy(variance * i + Extra);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<NucleusCrystal>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0);
                    Vector2 velocity2 = new Vector2(0f, 3 + ExtraSpeed);
                    velocity2 = velocity2.RotatedBy(variance * i + Extra);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity2, ModContent.ProjectileType<NucleusCrystal>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0);
                }
            }
        }
    }
}