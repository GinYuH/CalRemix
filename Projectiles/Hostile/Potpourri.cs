using CalamityMod;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class Potpourri : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Potpourri");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void AI()
        {
            Entity target = Projectile.ai[0] == 0 ? Main.npc[(int)Projectile.ai[1]] : Main.player[(int)Projectile.ai[1]];
            bool active = target != null && target.active;
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] < 90)
            { 
                Projectile.ai[2] += 0.1f;
                float timeThing = (float)Math.Sin(Projectile.ai[2]);
                float speedX = 16f;
                float speedY = 4;
                Projectile.velocity = new Vector2(timeThing * speedX, speedY);
                Projectile.rotation = timeThing;
            }
            else if (Projectile.localAI[1] == 90 && active)
            {
                int launchSpeed = 16;
                Projectile.velocity = Projectile.DirectionTo(target.Center) * launchSpeed;
                SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.SwiftSliceSound with { Pitch = 0.4f, Volume = 0.6f }, Projectile.Center);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
            int padding = 0;
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(Projectile.position - new Vector2(padding, padding), Projectile.width + padding, Projectile.height + padding, DustID.Grass, Scale: Main.rand.NextFloat(2, 4));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.localAI[0] == 0 ? TextureAssets.Projectile[Type].Value : Projectile.localAI[0] == 1 ? ModContent.Request<Texture2D>(Texture + "2").Value : ModContent.Request<Texture2D>(Texture + "3").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            if (Projectile.localAI[1] >= 90)
            {
                if (Projectile.oldPos[^1] == Projectile.position)
                    return false;
                Main.spriteBatch.EnterShaderRegion();

                // Prepare the flame trail shader with its map texture.
                GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
                Vector2 trailOffset = Projectile.Size * 0.5f;
                trailOffset += (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2();
                PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(FlameTrailWidthFunction, FlameTrailColorFunction, (_) => trailOffset, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), 61);

                Main.spriteBatch.ExitShaderRegion();
                Main.EntitySpriteDraw(texture, centered, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }
            else
            {
                CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.Violet * 0.4f, Color.Violet, 4, texture);
            }
            return false;
        }
        public float FlameTrailWidthFunction(float completionRatio) => MathHelper.SmoothStep(12f * Projectile.scale, 8f * Projectile.scale, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0.8f, 0.27f, completionRatio, true) * Utils.GetLerpValue(0f, 0.067f, completionRatio, true);
            return Color.Violet * trailOpacity;
        }
    }
}