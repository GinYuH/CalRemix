using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class Reef : ModProjectile
	{
        public int finalDamage;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grand Reef");
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 74;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 1f)
            {
                finalDamage = Projectile.damage * 7;
            }

            if (Projectile.ai[1] == 5f)
            {
                Projectile.tileCollide = true;
            }

            Vector2 vector = Main.player[Projectile.owner].Center - Projectile.Center;
            Projectile.rotation = vector.ToRotation() - 1.57f;
            if (Main.player[Projectile.owner].dead)
            {
                Projectile.Kill();
                return;
            }

            Main.player[Projectile.owner].itemAnimation = 10;
            Main.player[Projectile.owner].itemTime = 10;
            if (vector.X < 0f)
            {
                Main.player[Projectile.owner].ChangeDir(1);
                Projectile.direction = 1;
            }
            else
            {
                Main.player[Projectile.owner].ChangeDir(-1);
                Projectile.direction = -1;
            }

            Main.player[Projectile.owner].itemRotation = (vector * -1f * Projectile.direction).ToRotation();
            Projectile.spriteDirection = ((!(vector.X > 0f)) ? 1 : (-1));
            if (Projectile.ai[1] >= 45f && (Projectile.ai[0] != 1f || Projectile.ai[0] != 2f))
            {
                Projectile.velocity.Y += 1f;
                Projectile.velocity.X *= 0.995f;
                Projectile.damage = finalDamage;
            }

            if (Projectile.ai[0] == 0f && vector.Length() > 1000f)
            {
                Projectile.ai[0] = 1f;
            }

            if (Projectile.ai[0] == 1f || Projectile.ai[0] == 2f)
            {
                float num5 = vector.Length();
                if (num5 > 1500f)
                {
                    Projectile.Kill();
                    return;
                }

                if (num5 > 600f)
                {
                    Projectile.ai[0] = 2f;
                }

                Projectile.tileCollide = false;
                float num6 = 20f;
                if (Projectile.ai[0] == 2f)
                {
                    num6 = 40f;
                }

                Projectile.velocity = Vector2.Normalize(vector) * num6;
                if (vector.Length() < num6)
                {
                    Projectile.Kill();
                    return;
                }
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 5f)
            {
                Projectile.alpha = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] >= 45f && Projectile.ai[0] != 1f)
            {
                int num = Main.rand.Next(30, 47);
                for (int i = 0; i < num; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<CoralShard>(), Projectile.damage / 100, Projectile.knockBack, Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Item92, Projectile.position);
            }
            else if ( Projectile.ai[1] < 45f && Projectile.ai[0] != 1f)
            {
                int num = Main.rand.Next(58, 73);
                for (int i = 0; i < num; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<CoralShard>(), Projectile.damage / 700, Projectile.knockBack, Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }
            if (Projectile.ai[1] >= 5f)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 vector = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    int num = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width / 2, Projectile.height / 2, DustID.Water, vector.X, vector.Y, 0, new Color(0, 142, 255), 1.5f);
                    Main.dust[num].velocity *= 2f;
                }
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Texture2D value = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/ReefChain").Value;
            Vector2 center = Projectile.Center;
            Rectangle? sourceRectangle = null;
            Vector2 origin = new((float)value.Width * 0.5f, (float)value.Height * 0.5f);
            float num = value.Height;
            Vector2 vector = mountedCenter - center;
            float rotation = (float)Math.Atan2(vector.Y, vector.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(center.X) && float.IsNaN(center.Y))
            {
                flag = false;
            }

            if (float.IsNaN(vector.X) && float.IsNaN(vector.Y))
            {
                flag = false;
            }

            while (flag)
            {
                if (vector.Length() < num + 1f)
                {
                    flag = false;
                    continue;
                }

                Vector2 vector2 = vector;
                vector2.Normalize();
                center += vector2 * num;
                vector = mountedCenter - center;
                Color color = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
                Main.spriteBatch.Draw(value, center - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
            }

            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] < 45f && Projectile.ai[0] != 1f)
            {
                int num = Main.rand.Next(30, 47);
                for (int i = 0; i < num; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<CoralShard>(), Projectile.damage / 100, Projectile.knockBack, Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }
            else if (Projectile.ai[1] >= 45 && Projectile.ai[0] != 1f)
            {
                int num = Main.rand.Next(58, 73);
                for (int i = 0; i < num; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ModContent.ProjectileType<CoralShard>(), Projectile.damage / 700, Projectile.knockBack, Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Item92, Projectile.position);
            }
            Projectile.ai[0] = 1f;
            Projectile.netUpdate = true;
        }
    }
}