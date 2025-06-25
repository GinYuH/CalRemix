using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;

namespace CalRemix.Core.Graphics
{
    public class ScreenShatterSystem : ModSystem
    {
        public class ScreenTriangleShard
        {
            public float RotationX;

            public float RotationY;

            public float RotationZ;

            public Vector3 RotationalAxis;

            public Vector2 ScreenCoord1;

            public Vector2 ScreenCoord2;

            public Vector2 ScreenCoord3;

            public Vector2 DrawPosition;

            public Vector2 Velocity;

            public void Update()
            {
                float angularSlowdownInterpolant = GetLerpValue(0.112f, 1f, Velocity.Length(), true);
                RotationX += angularSlowdownInterpolant * RotationalAxis.X;
                RotationY += angularSlowdownInterpolant * RotationalAxis.Y;
                RotationZ += angularSlowdownInterpolant * RotationalAxis.Z;
                Velocity *= 0.91f;
                DrawPosition += GetLerpValue(0.97f, 0.55f, ShardOpacity, true) * Velocity;
            }

            public ScreenTriangleShard(Vector2 a, Vector2 b, Vector2 c, Vector2 drawPosition)
            {
                ScreenCoord1 = a;
                ScreenCoord2 = b;
                ScreenCoord3 = c;
                DrawPosition = drawPosition;
                Velocity = (DrawPosition - ShatterFocalPoint).RotatedByRandom(0.23f) * Main.rand.NextFloat(0.04f, 0.06f) + Main.rand.NextVector2CircularEdge(1.2f, 1.2f);
                RotationalAxis = new(Main.rand.NextFloatDirection() * 0.06f, Main.rand.NextFloatDirection() * 0.06f, Main.rand.NextFloatDirection() * 0.03f);
            }
        }

        public static bool ShouldCreateSnapshot
        {
            get;
            private set;
        }

        public static float ShardOpacity
        {
            get;
            private set;
        }

        public static Vector2 ShatterFocalPoint
        {
            get;
            private set;
        }

        public static BasicEffect DrawShader
        {
            get;
            private set;
        }

        public static RenderTarget2D ContentsBeforeShattering
        {
            get;
            private set;
        }

        public static readonly List<ScreenTriangleShard> screenTriangles = new();

        public static readonly SoundStyle ShatterSound = new("CalRemix/Assets/Sounds/Noxus/ScreenShatter");

        public override void Load()
        {
            Main.OnPostDraw += DrawShatterEffect;
            Main.QueueMainThreadAction(() =>
            {
                if (Main.netMode == NetmodeID.Server)
                    return;

                DrawShader = new(Main.instance.GraphicsDevice)
                {
                    TextureEnabled = true,
                    VertexColorEnabled = true
                };
            });
        }

        public override void Unload()
        {
            Main.OnPostDraw -= DrawShatterEffect;
            Main.QueueMainThreadAction(() =>
            {
                if (Main.netMode == NetmodeID.Server)
                    return;

                DrawShader?.Dispose();
                ContentsBeforeShattering?.Dispose();
            });
        }

        private void DrawShatterEffect(GameTime obj)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            // Draw and update all shards.
            Vector3 screenArea = new(Main.screenWidth, Main.screenHeight, 1f);
            List<VertexPositionColorTexture> shardVertices = new();
            Color shardColor = Color.White * Pow(ShardOpacity, 0.56f);
            foreach (ScreenTriangleShard shard in screenTriangles)
            {
                Vector3 a = new(shard.ScreenCoord1, 0f);
                Vector3 b = new(shard.ScreenCoord2, 0f);
                Vector3 c = new(shard.ScreenCoord3, 0f);

                // Calculate the rotation matrix for the shard.
                Matrix shardTransformation = Matrix.CreateRotationX(shard.RotationX) * Matrix.CreateRotationY(shard.RotationY) * Matrix.CreateRotationZ(shard.RotationZ);

                // Rotate shards in accordance with the rotation matrix.
                a = Vector3.Transform(a, shardTransformation);
                b = Vector3.Transform(b, shardTransformation);
                c = Vector3.Transform(c, shardTransformation);
                a.Z = 0f;
                b.Z = 0f;
                c.Z = 0f;

                Vector3 center = (a + b + c) / 3f;
                Vector3 drawPositionA = (a - center) * screenArea + new Vector3(shard.DrawPosition, 0f);
                Vector3 drawPositionB = (b - center) * screenArea + new Vector3(shard.DrawPosition, 0f);
                Vector3 drawPositionC = (c - center) * screenArea + new Vector3(shard.DrawPosition, 0f);
                shardVertices.Add(new(drawPositionA, shardColor, shard.ScreenCoord1));
                shardVertices.Add(new(drawPositionB, shardColor, shard.ScreenCoord2));
                shardVertices.Add(new(drawPositionC, shardColor, shard.ScreenCoord3));

                shard.Update();
            }

            if (shardVertices.Any())
            {
                CalamityUtils.CalculatePerspectiveMatricies(out Matrix effectView, out Matrix effectProjection);

                // Calculate a universal scale factor for all shards. This is done to make them appear to be "approaching" the camera as time goes on.
                Vector2 scaleFactor = (MathHelper.SmoothStep(0f, 1f, 1f - ShardOpacity) * 3f + 1f) * Vector2.One / Main.GameViewMatrix.Zoom;
                Matrix scale = Matrix.CreateScale(scaleFactor.X, scaleFactor.Y, 1f);

                // Set shader data.
                DrawShader.View = effectView;
                DrawShader.Projection = effectProjection * scale;
                DrawShader.Texture = ContentsBeforeShattering;
                DrawShader.CurrentTechnique.Passes[0].Apply();

                // Draw shard vertices.
                Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, shardVertices.ToArray(), 0, screenTriangles.Count);
            }

            // Make the shards become more transparent as time goes on, until eventually they disappear.
            ShardOpacity *= 0.95f;
            if (ShardOpacity <= 0.0045f)
                screenTriangles.Clear();

            Main.spriteBatch.End();
        }

        public static void CreateShatterEffect(Vector2 shatterScreenPosition)
        {
            if (/*!CalRemixConfig.instance.ScreenShatterEffects*/ false)
            {
                Main.LocalPlayer.Calamity().GeneralScreenShakePower += 11f;
                return;
            }

            ShatterFocalPoint = shatterScreenPosition;
            ShouldCreateSnapshot = true;
            ShardOpacity = 1f;
        }

        public static void CreateSnapshotIfNecessary(RenderTarget2D screenContents)
        {
            if (!ShouldCreateSnapshot)
                return;

            ShouldCreateSnapshot = false;

            // Reset the render target if it is invalid or of the incorrect size.
            if (ContentsBeforeShattering is null || ContentsBeforeShattering.IsDisposed || ContentsBeforeShattering.Width != screenContents.Width || ContentsBeforeShattering.Height != screenContents.Height)
            {
                ContentsBeforeShattering?.Dispose();
                ContentsBeforeShattering = new(Main.instance.GraphicsDevice, screenContents.Width, screenContents.Height, true, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            }

            // Take the contents of the screen for usage by the shatter pieces.
            ContentsBeforeShattering.CopyContentsFrom(screenContents);

            // Reset old shards.
            screenTriangles.Clear();

            // Generate radial slice angles. These are randomly offset slightly, and form the basis of the shatter effect.
            // The slice is relative to the focal point.
            int radialSliceCount = Main.rand.Next(8, 12);
            List<float> radialSliceAngles = new();
            Rectangle screenRectangle = new(0, 0, ContentsBeforeShattering.Width, ContentsBeforeShattering.Height);
            for (int i = 0; i < radialSliceCount; i++)
            {
                float sliceAngle = TwoPi * i / radialSliceCount + Main.rand.NextFloatDirection() * 0.00146f;
                radialSliceAngles.Add(sliceAngle);
            }
            for (int i = 0; i < radialSliceCount; i++)
            {
                Vector2 a = ShatterFocalPoint / screenRectangle.Size();
                Vector2 b = a + radialSliceAngles[i].ToRotationVector2() * 0.7f;
                Vector2 c = a + radialSliceAngles[(i + 1) % radialSliceCount].ToRotationVector2() * 0.7f;

                screenTriangles.Add(new(a, b, c, (a + b + c) / 3f * screenRectangle.Size()));
            }

            // Subdivide the triangles until a lot of triangles exist.
            while (screenTriangles.Count <= 350)
                SubdivideRadialTriangle(Main.rand.Next(screenTriangles), Main.rand.NextFloat(0.15f, 0.44f), Main.rand.NextFloat(0.15f, 0.44f), screenTriangles);

            ScreenEffectSystem.SetFlashEffect(ShatterFocalPoint + Main.screenPosition, 2f, 32);
            SoundEngine.PlaySound(ShatterSound);
        }

        public static void SubdivideRadialTriangle(ScreenTriangleShard shard, float line1BreakInterpolant, float line2BreakInterpolant, List<ScreenTriangleShard> shards)
        {
            // Remove the original shard from the list, since its subdivisions will be placed instead.
            shards.Remove(shard);

            Vector2 center = shard.ScreenCoord1;
            Vector2 left = shard.ScreenCoord2;
            Vector2 right = shard.ScreenCoord3;

            // Calculate the shatter point relative to the lines of the triangle.
            Vector2 lineBreakLeft = Vector2.Lerp(center, left, line1BreakInterpolant);
            Vector2 lineBreakRight = Vector2.Lerp(center, right, line2BreakInterpolant);

            // Subdivision one - Inner triangle.
            shards.Add(new(center, lineBreakLeft, lineBreakRight, new Vector2(Main.screenWidth, Main.screenHeight) * (center + lineBreakLeft + lineBreakRight) / 3f));

            // Subdivision two - First part of outer triangle.
            shards.Add(new(right, left, lineBreakLeft, new Vector2(Main.screenWidth, Main.screenHeight) * (left + right + lineBreakLeft) / 3f));

            // Subdivision three - Second part of outer triangle.
            shards.Add(new(right, lineBreakLeft, lineBreakRight, new Vector2(Main.screenWidth, Main.screenHeight) * (right + lineBreakLeft + lineBreakRight) / 3f));
        }
    }
}
