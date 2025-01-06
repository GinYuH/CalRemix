using CalamityMod;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class StratusStar : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityMod/Projectiles/Ranged/StarmageddonStar2";
        private const float MaxVelocity = 32f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stratus Star");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 3600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            CalamityUtils.HomeInOnNPC(Projectile, false, 800f, MaxVelocity, 30f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;
            if (Projectile.ai[2] > 7f)
            {
                Dust startrail = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y), 8, 8, DustID.IceTorch, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, Projectile.alpha, default, 1.25f)];
                startrail.velocity *= -0.25f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int height = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = height * Projectile.frame;
            Vector2 origin = Projectile.Size / 2;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, height)), Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            SpriteEffects spriteEffects = Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.ExpandHitboxBy(200);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 50, default(Color), 1.5f);
                Main.dust[dust].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
            }

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, default(Color), 3.7f);
                Main.dust[dust].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Main.dust[dust].noGravity = true;
                Dust dust2 = Main.dust[dust];
                dust2.velocity *= 4f;
                dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 50, default(Color), 1.5f);
                Main.dust[dust].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                dust2 = Main.dust[dust];
                dust2.velocity *= 2f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 3f;
            }

            for (int i = 0; i < 2; i++)
            {
                int gore = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2((float)(Projectile.width * Main.rand.Next(50)) / 50f, (float)(Projectile.height * Main.rand.Next(50)) / 50f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(30, 32));
                Main.gore[gore].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Gore gore2 = Main.gore[gore];
                gore2.velocity *= 0.3f;
                Main.gore[gore].velocity.X += (float)Main.rand.Next(-10, 11) * 0.05f;
                Main.gore[gore].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.05f;
            }
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 0, default(Color), 2.7f);
                Main.dust[dust].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Main.dust[dust].noGravity = true;
                Dust dust2 = Main.dust[dust];
                dust2.velocity *= 3f;
            }

        }
    }    
}