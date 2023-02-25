using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class ShadowsDescentBall : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Shadow Ball");
		}
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 50;
            Projectile.timeLeft = 3000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.rotation += 0.2f * (float)Projectile.spriteDirection;
            if (Projectile.ai[0] >= 70f && Projectile.velocity != Vector2.Zero)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
                Projectile.velocity.X *= 0.96f;
                Projectile.velocity.Y *= 0.96f;
            }
            if (Projectile.ai[0] == 300f)
            {
                Projectile.frame = 0;
                Main.projFrames[Projectile.type] = 1;
                Projectile.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item74);
                Projectile.width = 256;
                Projectile.height = 256;
                Projectile.Center = new Vector2(Projectile.Center.X - Projectile.width/2 + 52/4, Projectile.Center.Y - Projectile.height/2 + 52/4);
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                }
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if (Projectile.ai[0] % 120 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 5f, ModContent.ProjectileType<PenumbraSoul>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        projectile.netUpdate = true;
                    }
                }
                if (Projectile.Calamity().stealthStrike && Projectile.ai[0] % 180 == 0)
                {
                    SoundEngine.PlaySound(CommonCalamitySounds.FlareSound);
                    Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowPulse>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    projectile.ai[1] = Main.rand.NextFloat(110f, 200f) + (float)20f;
                    projectile.localAI[1] = Main.rand.NextFloat(0.18f, 0.3f);
                    projectile.netUpdate = true;
                }
            }
            if (Main.player[Projectile.owner].Calamity().killSpikyBalls)
            {
               Projectile.active = false;
               Projectile.netUpdate = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= (int)target.GetGlobalNPC<CalRemixGlobalNPC>().shadowHit;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 600);
            if (target.GetGlobalNPC<CalRemixGlobalNPC>().shadowHit < 5)
            {
                target.GetGlobalNPC<CalRemixGlobalNPC>().shadowHit += 0.2f;
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 600);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            Projectile.spriteDirection = Projectile.direction;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("CalRemix/Projectiles/Weapons/ShadowsDescentBall");
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Color drawColor = Projectile.GetAlpha(new Color(255, 255, 255, 255));
            Vector2 origin = sourceRectangle.Size() / 2f;
            if (Projectile.velocity == Vector2.Zero)
            {
                Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Shadow");
                Rectangle sourceRectangle2 = new Rectangle(0, 0, texture2.Width, texture2.Height);
                Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition + new Vector2(-Projectile.width/4, -Projectile.height/4), sourceRectangle2, drawColor, 0, origin, Projectile.scale*3f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}