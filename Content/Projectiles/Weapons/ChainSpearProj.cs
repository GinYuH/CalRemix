using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class ChainSpearProj : ModProjectile
    {
        private NPC victim;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chain Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            if (Owner.dead)
            {
                Projectile.Kill();
                return;
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 5f)
                Projectile.tileCollide = true;
            Vector2 vector = Owner.Center - Projectile.Center;
            Projectile.rotation = vector.ToRotation() - (float)Math.PI / 4f;

            if (victim != null)
                victim.Center = Projectile.Center;
            if (Projectile.ai[0] == 0f && vector.Length() > 600f)
                Projectile.ai[0] = 1f;
            if (Projectile.ai[0] >= 1f)
            {
                Projectile.tileCollide = false;
                float currentLength = vector.Length();
                if (currentLength > 400f)
                    Projectile.ai[0] = 2f;
                if (currentLength > 1500f)
                {
                    Projectile.Kill();
                    return;
                }
                float min = Projectile.ai[0] == 2f ? 40f : 20f;
                Projectile.velocity = Vector2.Normalize(vector) * min;
                if (vector.Length() < min)
                {
                    Projectile.Kill();
                    return;
                }
            }

            Projectile.ai[1] += 1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] >= 5f)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.spriteDirection = -1;
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/SpearChain").Value;
            Vector2 center = Projectile.Center;
            float xVel = Projectile.velocity.X;
            float yVel = Projectile.velocity.Y;
            float velDis = (float)Math.Sqrt(xVel * xVel + yVel * yVel);
            velDis = 4f / velDis;
            if (Projectile.ai[0] == 0f)
            {
                center.X -= Projectile.velocity.X * velDis;
                center.Y -= Projectile.velocity.Y * velDis;
            }
            else
            {
                center.X += Projectile.velocity.X * velDis;
                center.Y += Projectile.velocity.Y * velDis;
            }
            xVel = Owner.MountedCenter.X - center.X;
            yVel = Owner.MountedCenter.Y - center.Y;
            float rotation = (float)Math.Atan2(yVel, xVel) - 1.57f;
            if (Projectile.alpha == 0)
            {
                int num115 = -1;
                if (Projectile.position.X + (float)(Projectile.width / 2) < Owner.MountedCenter.X)
                {
                    num115 = 1;
                }
                if (Main.player[Projectile.owner].direction == 1)
                {
                    Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(yVel * (float)num115, xVel * (float)num115);
                }
                else
                {
                    Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(yVel * (float)num115, xVel * (float)num115);
                }
            }
            bool flag20 = true;
            while (flag20)
            {
                float num116 = (float)Math.Sqrt(xVel * xVel + yVel * yVel);
                if (num116 < 16f)
                {
                    flag20 = false;
                    continue;
                }
                if (float.IsNaN(num116))
                {
                    flag20 = false;
                    continue;
                }
                num116 = 8f / num116;
                xVel *= num116;
                yVel *= num116;
                center.X += xVel;
                center.Y += yVel;
                xVel = Owner.MountedCenter.X - center.X;
                yVel = Owner.MountedCenter.Y - center.Y;
                Color color = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, new Vector2(texture.Width, texture.Height), 1f, SpriteEffects.None, 0);
            }    
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (victim is null)
                return;
            victim.GetGlobalNPC<CalRemixNPC>().grappled = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = 1f;
            Projectile.netUpdate = true;
            if (target.Equals(victim))
                hit.Damage = 0;
            if (target.boss && victim != null)
                return;
            victim = target;
            victim.GetGlobalNPC<CalRemixNPC>().grappled = true;
        }
    }
}