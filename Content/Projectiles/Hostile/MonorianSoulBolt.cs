using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Particles;
using CalamityMod.Sounds;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Graphics.Effects.Filters;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MonorianSoulBolt : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 11;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                int gem = NPC.FindFirstNPC(ModContent.NPCType<MonorianGemBoss>());
                if (gem != -1)
                {
                    Projectile.Center = Vector2.Lerp(Projectile.Center, Main.npc[gem].Center, MathHelper.Lerp(0.05f, 0.5f, Utils.GetLerpValue(10, 30, Projectile.ai[1], true)));
                    Projectile.ai[1]++;
                    if (Projectile.Distance(Main.npc[gem].Center) < 40)
                    {
                        int boltAmount = 15;

                        for (int i = 0; i < boltAmount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedBy(MathHelper.PiOver2 + (Main.rand.NextBool() ? -MathHelper.Pi : 0)).RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(5, 12), Type, Projectile.damage, 1, ai0: 2, ai2: Projectile.ai[2]);
                            }
                        }
                        for (int i = 0; i < 40; i++)
                            GeneralParticleHandler.SpawnParticle(new SparkleParticle(Main.rand.NextVector2FromRectangle(Projectile.Hitbox), Main.rand.NextVector2Circular(8, 8), Color.LightCyan, Color.Cyan, 1f, 10, Main.rand.NextFloat(-0.1f, 0.1f)));
                        SoundEngine.PlaySound(CommonCalamitySounds.ExoPlasmaExplosionSound, Projectile.Center);
                        Projectile.Kill();
                    }
                }
            }
            else if (Projectile.ai[0] == 2)
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] > 30 && Projectile.ai[1] < 90)
                {
                    float scaleFactor = 18;
                    float inertia = 10f;
                    Vector2 speed = Projectile.DirectionTo(Main.player[(int)Projectile.ai[2]].Center).SafeNormalize(Vector2.UnitY) * scaleFactor;
                    Projectile.velocity = (Projectile.velocity * inertia + speed) / (inertia + 1f);
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= scaleFactor;
                }
                if (Projectile.timeLeft <= 30)
                {
                    Projectile.damage = 0;
                    Projectile.Opacity = Utils.GetLerpValue(0, 30, Projectile.timeLeft);
                }
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            Vector2 trailOffset = Projectile.Size * 0.5f;
            //trailOffset += (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2();
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(FlameTrailWidthFunction, FlameTrailColorFunction, (_, _) => trailOffset), 61);

            Main.spriteBatch.ExitShaderRegion();
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Main.EntitySpriteDraw(texture, centered, null, Color.LightCyan * 0.5f * Projectile.Opacity, Projectile.rotation, texture.Size() / 2, Projectile.scale * 0.1f, SpriteEffects.None, 0);


            Main.spriteBatch.ExitShaderRegion();
            float animSpeed = 4 + Projectile.whoAmI % 3;
            float size = 5;
            Matrix rotMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * animSpeed));
            Matrix rotMatrix2 = Matrix.CreateRotationZ(22 * MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * animSpeed));
            Matrix rotMatrix3 = Matrix.CreateRotationX(22 * MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * animSpeed));
            List<Vector3> rotatedVertices = new();
            for (int i = 0; i < MonorianGemBoss.cubeVertices.Count; i++)
            {
                Vector3 transformed = Vector3.Transform(MonorianGemBoss.cubeVertices[i], rotMatrix);
                transformed = Vector3.Transform(transformed, rotMatrix2);
                transformed = Vector3.Transform(transformed, rotMatrix3);
                rotatedVertices.Add(transformed);
            }

            List<Vector2> twodvertices = new();
            for (int i = 0; i < rotatedVertices.Count; i++)
            {
                Vector2 newpoint = new Vector2(rotatedVertices[i].X, rotatedVertices[i].Y) * size;
                twodvertices.Add(newpoint);
            }

            Texture2D testSprite = TextureAssets.MagicPixel.Value;
            Rectangle testFrame = new Rectangle(0, 0, (int)size, (int)size);

            Matrix rotation = Matrix.CreateRotationX(0);
            Matrix translation = Matrix.CreateTranslation(new Vector3(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y, 0));
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -220, 220);
            Matrix renderMatrix = rotation * translation * view * projection;
            Effect effect = Scene["CalRemix:NormalDraw"].GetShader().Shader;

            for (int i = 0; i < MonorianGemBoss.faces.Count; i++)
            {
                short[] indices = [0, 1, 2, 1, 3, 2];
                Vector4 positions = MonorianGemBoss.faces[i];

                Vector3 topLeft = new Vector3(rotatedVertices[(int)positions.X].X, rotatedVertices[(int)positions.X].Y, 0) * size;
                Vector3 topRight = new Vector3(rotatedVertices[(int)positions.Y].X, rotatedVertices[(int)positions.Y].Y, 0) * size;
                Vector3 botLeft = new Vector3(rotatedVertices[(int)positions.Z].X, rotatedVertices[(int)positions.Z].Y, 0) * size;
                Vector3 botRight = new Vector3(rotatedVertices[(int)positions.W].X, rotatedVertices[(int)positions.W].Y, 0) * size;

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
                vertices[0] = new(topLeft, Color.Cyan * 0.6f * Projectile.Opacity, new Vector2(0, 0));
                vertices[1] = new(topRight, Color.White * 0.6f * Projectile.Opacity, new Vector2(1, 0));
                vertices[2] = new(botLeft, Color.Cyan * 0.6f * Projectile.Opacity, new Vector2(0, 1));
                vertices[3] = new(botRight, Color.LightCyan * 0.6f * Projectile.Opacity, new Vector2(1, 1));

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    effect.Parameters["textureResolution"].SetValue(testFrame.Size());
                    effect.Parameters["sampleTexture"].SetValue(testSprite);
                    effect.Parameters["frame"].SetValue(new Vector4(testFrame.X, testFrame.Y, testFrame.Width, testFrame.Height));
                    effect.Parameters["uWorldViewProjection"].SetValue(renderMatrix);
                    effect.Parameters["opacity"].SetValue(1);
                    pass.Apply();

                    Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
                }
            }


            return false;
        }
        public float FlameTrailWidthFunction(float completionRatio, Vector2 v) => MathHelper.SmoothStep(6f * Projectile.scale, 2f * Projectile.scale, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio, Vector2 v)
        {
            return Color.Lerp(Color.Cyan * 0.5f, default, completionRatio) * Projectile.Opacity;
        }
    }
}