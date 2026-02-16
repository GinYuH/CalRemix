using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Utils;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class NoxusExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 720;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.MaxUpdates = 3;
            Projectile.scale = 0.35f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.5f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 8 == 7)
                Projectile.frame++;

            if (Projectile.frame >= 18)
                Projectile.Kill();
            Projectile.scale *= 1.0115f;
            Projectile.Opacity = GetLerpValue(5f, 36f, Projectile.timeLeft, true) * GetLerpValue(0f, 4f, Projectile.frameCounter, true);

            if (Projectile.timeLeft == 80)
            {
                SoundEngine.PlaySound(EntropicGod.ExplosionSound, Projectile.Center);

                // Release a little bit of bright dust.
                for (int i = 0; i < 16; i++)
                {
                    Vector2 dustSpawnPosition = Projectile.Center + Main.rand.NextVector2Circular(8f, 8f);
                    Vector2 dustVelocity = Main.rand.NextVector2Circular(20f, 20f);
                    Dust dust = Dust.NewDustPerfect(dustSpawnPosition, DustID.PortalBoltTrail, dustVelocity);
                    dust.color = Color.Lerp(Color.SkyBlue, Color.Fuchsia, Main.rand.NextFloat(0.35f, 0.65f));
                    dust.scale = 2.2f;
                    dust.noLight = true;
                    dust.noGravity = true;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<NoxusFumes>(), EntropicGod.DebuffDuration_RegularAttack);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D lightTexture = ModContent.Request<Texture2D>("CalamityMod/Skies/XerocLight").Value;
            Rectangle frame = texture.Frame(3, 6, Projectile.frame / 6, Projectile.frame % 6);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = frame.Size() * 0.5f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Color lightBurstColor = Color.Lerp(Color.Lerp(Color.Blue, Color.BlueViolet, Projectile.timeLeft / 303f), Color.Purple, 0.64f) * 0.9f;
            lightBurstColor = Color.Lerp(lightBurstColor, Color.White, -0.1f) * Projectile.Opacity;
            Main.EntitySpriteDraw(lightTexture, drawPosition, null, lightBurstColor, 0f, lightTexture.Size() * 0.5f, Projectile.scale * 1.27f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, drawPosition, frame, Color.Black, 0f, origin, 1.5f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(texture, drawPosition, frame, Color.White, 0f, origin, 1.88f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool? CanDamage() => Projectile.timeLeft < 84;
    }
}
