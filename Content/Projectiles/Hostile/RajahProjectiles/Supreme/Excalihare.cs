using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.NPCs.Bosses.RajahBoss;


namespace CalRemix.Content.Projectiles.Hostile.RajahProjectiles.Supreme
{
    public class Excalihare : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Excalihare");
		}
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 8f)
            {
                Projectile.position += Projectile.velocity;
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
            }
            Vector2 spinningpoint = new Vector2(0f, -3f - Projectile.ai[0]).RotatedByRandom(3.1415927410125732);
            float num13 = 10f + Projectile.ai[0] * 4f;
            Vector2 value6 = new Vector2(1.05f, 1f);
            for (float num14 = 0f; num14 < num13; num14 += 1f)
            {
                int num15 = Dust.NewDust(Projectile.Center, 0, 0, 66, 0f, 0f, 0, Color.Transparent, 1f);
                Main.dust[num15].position = Projectile.Center;
                Main.dust[num15].velocity = spinningpoint.RotatedBy(6.28318548f * num14 / num13) * value6 * (0.8f + Main.rand.NextFloat() * 0.4f);
                Main.dust[num15].color = Main.hslToRgb(num14 / num13, 1f, 0.5f);
                Main.dust[num15].noGravity = true;
                Main.dust[num15].scale = 1f + Projectile.ai[0] / 3f;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                int width = Projectile.width;
                int height = Projectile.height;
                int num16 = Projectile.penetrate;
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 40 + 8 * (int)Projectile.ai[0];
                Projectile.Center = Projectile.position;
                Projectile.penetrate = -1;
                Projectile.Damage();
                Projectile.penetrate = num16;
                Projectile.position = Projectile.Center;
                Projectile.width = width;
                Projectile.height = height;
                Projectile.Center = Projectile.position;
            }
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExcalihareBoom>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[p].Center = Projectile.Center;
            Main.projectile[p].netUpdate = true;
            return false;
        }

        public override void AI()
        {
            float num99 = Main.DiscoR / 255f;
            float num100 = Main.DiscoG / 255f;
            float num101 = Main.DiscoB / 255f;
            num99 = (0.5f + num99) / 2f;
            num100 = (0.5f + num100) / 2f;
            num101 = (0.5f + num101) / 2f;
            Lighting.AddLight(Projectile.Center, num99, num100, num101);
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;
            if (Projectile.velocity.X != 0f)
            {
                Projectile.spriteDirection = Projectile.direction = -Math.Sign(Projectile.velocity.X);
            }
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
                return;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            BaseDrawing.DrawAfterimage(Main.spriteBatch, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, .5f, 1f, 10, false, 0f, 0f, Main.DiscoColor);
            BaseDrawing.DrawTexture(Main.spriteBatch, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, lightColor, false);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Buffs.InfinityOverload>(), 120);
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExcalihareBoom>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[p].Center = Projectile.Center;
            Main.projectile[p].netUpdate = true;
        }

        public override void OnKill(int i)
        {
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExcalihareBoom>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[p].Center = Projectile.Center;
            Main.projectile[p].netUpdate = true;
        }
    }
}