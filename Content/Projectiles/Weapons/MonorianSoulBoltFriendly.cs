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
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Graphics.Effects.Filters;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MonorianSoulBoltFriendly : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 26;
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
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 30 && Projectile.ai[1] < 90)
            {
                CalamityUtils.HomeInOnNPC(Projectile, true, 2000, 26, 10);
            }
            if (Projectile.timeLeft <= 30)
            {
                Projectile.damage = 0;
                Projectile.Opacity = Utils.GetLerpValue(0, 30, Projectile.timeLeft);
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.NewText("Hi");
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            Vector2 trailOffset = Projectile.Size * 0.5f;
            Main.spriteBatch.EnterShaderRegion();

            // Prepare the flame trail shader with its map texture.
            GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new((float f) => 100, FlameTrailColorFunction, (_) => trailOffset, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), 61);

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
                vertices[0] = new(topLeft, Color.Red * 0.6f * Projectile.Opacity, new Vector2(0, 0));
                vertices[1] = new(topRight, Color.Teal * 0.6f * Projectile.Opacity, new Vector2(1, 0));
                vertices[2] = new(botLeft, Color.DarkRed * 0.6f * Projectile.Opacity, new Vector2(0, 1));
                vertices[3] = new(botRight, Color.Purple * 0.6f * Projectile.Opacity, new Vector2(1, 1));

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
        public float FlameTrailWidthFunction(float completionRatio) => MathHelper.SmoothStep(40 * Projectile.scale, 28f * Projectile.scale, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio)
        {
            Color main = CalamityUtils.MulticolorLerp(completionRatio, Color.Red, Color.Cyan, Color.Purple, Color.Teal, Color.DarkRed);
            return Color.Lerp(main * 2, default, completionRatio) * Projectile.Opacity;
        }
    }
}