using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod;
namespace CalRemix.Projectiles.Weapons
{
    public class RockBullet : ModProjectile
    {
        public bool initialized = false;
        private int laserdirection = 1;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
        }

        public override void AI()
        {
            Projectile.velocity = new Vector2(0f, (float)Math.Sin((double)(MathHelper.TwoPi * Projectile.ai[0] / 300f)) * 0.5f);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 300f)
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 7200f)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= 10f)
            {
                Projectile.localAI[0] = 0f;
                int projCount = 0;
                int index = 0;
                float findOldest = 0f;
                int projType = Projectile.type;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == Projectile.owner && proj.type == projType && proj.ai[1] < 3600f)
                    {
                        projCount++;
                        if (proj.ai[1] > findOldest)
                        {
                            index = i;
                            findOldest = proj.ai[1];
                        }
                    }
                }
                if (projCount > 1)
                {
                    Main.projectile[index].netUpdate = true;
                    Main.projectile[index].ai[1] = 36000f;
                    return;
                }
            }
            if (!initialized)
            {
                SoundEngine.PlaySound(SoundID.Item119, Projectile.Center);
                Projectile.ExpandHitboxBy(20);
                initialized = true;
            }
            if (Projectile.timeLeft % 30 == 1 && Projectile.alpha <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);
                Projectile.ExpandHitboxBy(20);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 48; i++)
                    {
                        Vector2 velocity = ((MathHelper.TwoPi * i / 16f) - (MathHelper.Pi / 16f)).ToRotationVector2() * 15f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<RockFire>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            NPC target = CalamityUtils.MinionHoming(Projectile.Center, 5000, Main.player[Projectile.owner]);
            if (Projectile.timeLeft % 300 == 1 && target != null && target.active && Projectile.timeLeft > 60)
            {
                SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, Projectile.Center);

                float screenShakePower = 22 * Utils.GetLerpValue(722f, 0f, Projectile.Distance(Main.LocalPlayer.Center), true);
                if (Main.LocalPlayer.Calamity().GeneralScreenShakePower < screenShakePower)
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = screenShakePower;

                Vector2 direction = Projectile.DirectionTo(target.Center);
                direction.Normalize();
                direction = direction.RotatedBy(MathHelper.ToRadians(30 * -laserdirection));
                float angularChange = (MathHelper.Pi / 180f) * 1.01f * laserdirection;
                if (Main.myPlayer == Projectile.owner)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ModContent.ProjectileType<RockRay>(), (int)(Projectile.damage * 2f), 0f, Projectile.owner, angularChange, (float)Projectile.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = Projectile.originalDamage;
                }
                laserdirection *= -1;

                float starSpeed = 25f;

                for (int i = 0; i < 40; i++)
                {
                    Vector2 shootVelocity = (MathHelper.TwoPi * i / 40f).ToRotationVector2() * starSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<RockOrb>(), (int)(Projectile.damage * 2), 0f, Projectile.owner);
                }

                // now where could this have come from
                int pointsOnStar = 6;
                for (int k = 0; k < 2; k++)
                {
                    for (int i = 0; i < pointsOnStar; i++)
                    {
                        float angle = MathHelper.Pi * 1.5f - i * MathHelper.TwoPi / pointsOnStar;
                        float nextAngle = MathHelper.Pi * 1.5f - (i + 3) % pointsOnStar * MathHelper.TwoPi / pointsOnStar;
                        if (k == 1)
                            nextAngle = MathHelper.Pi * 1.5f - (i + 2) * MathHelper.TwoPi / pointsOnStar;
                        Vector2 start = angle.ToRotationVector2();
                        Vector2 end = nextAngle.ToRotationVector2();
                        int pointsOnStarSegment = 18;
                        for (int j = 0; j < pointsOnStarSegment; j++)
                        {
                            Vector2 shootVelocity = Vector2.Lerp(start, end, j / (float)pointsOnStarSegment) * starSpeed;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<RockOrb>(), (int)(Projectile.damage * 2), 0f, Projectile.owner);
                        }
                    }
                }

                float num461 = 3f;
                num461 *= 0.7f;
                Projectile.ai[0] += 4f;
                int num462 = 0;
                while ((float)num462 < num461)
                {
                    float num463 = (float)Main.rand.Next(-50, 51);
                    float num464 = (float)Main.rand.Next(-50, 51);
                    float num465 = (float)Main.rand.Next(9, 27);
                    float num466 = (float)Math.Sqrt((double)(num463 * num463 + num464 * num464));
                    num466 = num465 / num466;
                    num463 *= num466;
                    num464 *= num466;
                    int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.Brimstone, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num467];
                    dust.noGravity = true;
                    dust.position.X = Projectile.Center.X;
                    dust.position.Y = Projectile.Center.Y;
                    dust.position.X += (float)Main.rand.Next(-20, 21);
                    dust.position.Y += (float)Main.rand.Next(-20, 21);
                    dust.velocity.X = num463;
                    dust.velocity.Y = num464;
                    num462++;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            int height = texture.Height / Main.projFrames[Projectile.type];
            int frameHeight = height * Projectile.frame;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, frameHeight, texture.Width, height)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, height / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool? CanDamage() => false;

        public override void OnKill(int timeLeft)
        {

        }
    }
}
