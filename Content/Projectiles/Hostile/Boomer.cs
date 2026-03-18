using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class Boomer : ModProjectile
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/BallisticPoisonBomb";

        public static int TickTime => CalamityUtils.SecondsToFrames(2.5f);
        public static int Telegraph => TickTime + 60;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.getRect().Intersects(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight)))
            {
                Projectile.ai[1]++;
            }
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] > TickTime)
            {
                Projectile.velocity *= 0.98f;
            }
            if (Projectile.ai[0] > Telegraph)
            {
                Projectile.Kill();
            }

            float center = Main.maxTilesY * 16 * 0.5f;
            int wiggleRoom = 1000;
            if (Projectile.ai[2] <= 0)
            {
                if (Projectile.Center.Y > center + wiggleRoom || Projectile.Center.Y < center - wiggleRoom)
                {
                    Projectile.velocity.Y *= -1;
                    Projectile.ai[2] = 22;
                }
            }

            Projectile.ai[2]--;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile p = Projectile;
            Main.LocalPlayer.Calamity().GeneralScreenShakePower += 1;
            p.position = p.Center;
            p.width = p.height = 200;
            p.position.X = p.position.X - (float)(p.width / 2);
            p.position.Y = p.position.Y - (float)(p.height / 2);
            p.maxPenetrate = -1;
            p.penetrate = -1;
            p.Damage();
            SoundEngine.PlaySound(MercuryRocket.RockyExplosion with { PitchVariance = 0.3f }, p.Center);
            for (int i = 0; i < 10; i++)
                GeneralParticleHandler.SpawnParticle(new CustomPulse(p.Center, Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(10f, 20f), Color.Orange, "CalamityMod/Particles/Light", Vector2.One, 0, Main.rand.NextFloat(1f, 3f), 0, 20));
            for (int i = 0; i < 20; i++)
                GeneralParticleHandler.SpawnParticle(new TimedSmokeParticle(p.Center, Main.rand.NextVector2Circular(20, 20), Color.DarkGray, new Color(20, 20, 20), Main.rand.NextFloat(0.8f, 1f), 0.6f, 30, 0.02f));
            GeneralParticleHandler.SpawnParticle(new StrongBloom(p.Center, Vector2.Zero, Color.Orange, 2f, 3));
            GeneralParticleHandler.SpawnParticle(new PulseRing(p.Center, Vector2.Zero, Color.Red, 0.4f, 2f, 10));
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            float alpha = 0;
            if (Projectile.ai[0] > TickTime)
            {
                float comp = Utils.GetLerpValue(TickTime, Telegraph, Projectile.ai[0], true);
                float rotAmt = MathHelper.Lerp(0, 5, comp);
                centered += Main.rand.NextVector2Circular(rotAmt, rotAmt);
                Projectile.DrawBackglow(Color.Red * 0.2f * comp, 4);
                alpha = MathHelper.Lerp(0, 0.6f, comp);
            }
            Main.spriteBatch.Draw(texture, centered, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            if (alpha > 0)
            {
                CalamityUtils.EnterShaderRegion(Main.spriteBatch);
                Color outlineColor = Color.Orange;
                Vector3 outlineHSL = Main.rgbToHsl(outlineColor);
                GameShaders.Misc["CalamityMod:BasicTint"].UseOpacity(alpha);
                GameShaders.Misc["CalamityMod:BasicTint"].UseColor(Main.hslToRgb(1 - outlineHSL.X, outlineHSL.Y, outlineHSL.Z));
                GameShaders.Misc["CalamityMod:BasicTint"].Apply();
                Main.spriteBatch.Draw(texture, centered, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                CalamityUtils.ExitShaderRegion(Main.spriteBatch);
            }
            return false;
        }
    }
}