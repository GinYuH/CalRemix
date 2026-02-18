using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class NamelessVortex : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.scale = 1.4f;
            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 30)
            {
                Projectile.Opacity = MathHelper.Lerp(1, 0, Utils.GetLerpValue(30, 0, Projectile.timeLeft, true));
            }
            else
            {
                if (Projectile.Opacity < 0.6f)
                    Projectile.Opacity += 0.1f;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 40)
            {
                Projectile.velocity *= 0.98f;
            }

            Projectile.rotation += Projectile.velocity.X.DirectionalSign() * 0.25f;

            Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Projectile.Hitbox);
            GeneralParticleHandler.SpawnParticle(new SquareParticle(spawnPos, spawnPos.DirectionTo(Projectile.Center) * 4, false, 60, Main.rand.NextFloat(1f, 2f), Color.Violet));
            if (Projectile.ai[0] == 1)
            {
                if (Main.rand.NextBool(5))
                { 
                    Vector2 spawnPose = Main.rand.NextVector2FromRectangle(Projectile.Hitbox);
                    for (int i = 0; i < 3; i++)
                    {
                        Particle spark2 = new GlowSparkParticle(spawnPose, new Vector2(0.1f, 0.1f).RotatedByRandom(100), false, 12, Main.rand.NextFloat(0.05f, 0.09f), (Main.rand.NextBool() ? Color.Violet : Main.rand.NextBool() ? Color.Red : Color.PaleGoldenrod) * 0.7f, new Vector2(2, 0.5f), true);
                        GeneralParticleHandler.SpawnParticle(spark2);
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion();

            // Prepare the flame trail shader with its map texture.
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
            float tentacleCount = 6;
            float pointCount = 51;
            float length = 300;
            Color col = Projectile.ai[2] == 0 ? Color.Violet : Projectile.ai[2] == 1 ? Color.Red : Color.PaleGoldenrod;
            for (int i = 0; i < tentacleCount; i++)
            {
                List<Vector2> points = new();
                for (int j = 0; j < pointCount; j++)
                {
                    Vector2 start = Projectile.Center;
                    Vector2 end = Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / tentacleCount)).RotatedBy(-Projectile.rotation.DirectionalSign() * MathHelper.Lerp(0, MathHelper.PiOver2, j / pointCount)).RotatedBy(Projectile.rotation) * length;
                    points.Add(Vector2.Lerp(start, end, j / pointCount));
                }
                PrimitiveRenderer.RenderTrail(points.ToArray(), new PrimitiveSettings((float f, Vector2 v) => 10 * MathHelper.Lerp(10, 20, CalamityUtils.SineOutEasing(1- f, 1)), (float f, Vector2 v) => Color.Lerp(col, col * 0.1f, f), shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]));
            }
            Main.spriteBatch.ExitShaderRegion();
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, bloom.Size() / 2, 0.2f * Projectile.scale, 0);
            Main.spriteBatch.ExitShaderRegion();
            //CalamityUtils.DrawAfterimagesCentered(Projectile, 1, Color.White * 0.4f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= 0.7f;
            if (Projectile.ai[0] == 1)
            {
                Vector2 spawnPos = Main.rand.NextVector2FromRectangle(Projectile.Hitbox);
                for (int i = 0; i < 3; i++)
                {
                    Particle spark2 = new GlowSparkParticle(spawnPos, new Vector2(0.1f, 0.1f).RotatedByRandom(100), false, 12, Main.rand.NextFloat(0.05f, 0.09f), (Main.rand.NextBool() ? Color.Violet : Main.rand.NextBool() ? Color.Red : Color.PaleGoldenrod) * 0.7f, new Vector2(2, 0.5f), true);
                    GeneralParticleHandler.SpawnParticle(spark2);
                }
            }
        }
    }
}