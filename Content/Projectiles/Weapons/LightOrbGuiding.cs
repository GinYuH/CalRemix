using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class LightOrbGuiding : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 4, 4, 0);
            Vector2 partPos = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 20;
            GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(partPos, partPos.DirectionTo(Projectile.Center) * 0.2f, Main.rand.NextFloat(0.8f, 1f), Color.Yellow, 16, 0.8f));

            Main.player[Projectile.owner].buffImmune[BuffID.Darkness] = true;
            Main.player[Projectile.owner].buffImmune[BuffID.Obstructed] = true;
            Main.player[Projectile.owner].Calamity().externalAbyssLight += 50;
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