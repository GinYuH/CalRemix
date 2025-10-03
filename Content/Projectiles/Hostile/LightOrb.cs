using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class LightOrb : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X.DirectionalSign() * 0.5f;
            Player p = Main.player[(int)Projectile.ai[0]];
            if (p == null || !p.active || p.dead)
                return;
            Projectile.ai[1]++;
            int timeBeforeHome = 30;
            if (Projectile.ai[1] == timeBeforeHome)
            {
                Projectile.velocity = Projectile.DirectionTo(p.Center).RotatedByRandom(MathHelper.PiOver2) * 3;
            }
            if (Projectile.ai[1] > timeBeforeHome)
            {
                if (Projectile.ai[1] > 120)
                    Projectile.tileCollide = true;
                float scaleFactor = 10;
                float inertia = 40f;
                Vector2 speed = Projectile.DirectionTo(p.Center).SafeNormalize(Vector2.UnitY) * scaleFactor;
                Projectile.velocity = (Projectile.velocity * inertia + speed) / (inertia + 1f);
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor;
            }
            for (int i = 0; i < 3; i++)
            {
                Vector2 partPos = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 20;
                GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(partPos, partPos.DirectionTo(Projectile.Center), Main.rand.NextFloat(0.8f, 1f), Color.Yellow, 2, 0.8f));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            /*Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            Texture2D wing = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeSpark").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.spriteBatch.Draw(texture, centered, null, Color.Yellow with { A = 0 }, Projectile.rotation, texture.Size() / 2, Projectile.scale * 0.2f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(wing, centered, null, Color.Yellow with { A = 0 }, Projectile.rotation, wing.Size() / 2, new Vector2(0.5f, 1f) * Projectile.scale * 0.2f, SpriteEffects.None, 0);*/
            return false;
        }
    }
}