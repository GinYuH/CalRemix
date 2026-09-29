using CalamityMod;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalRemix.Core.Graphics
{
    public class TextureMesh
    {
        public VertexPositionColorTexture[] vertices;

        public short[] indicies;

        public int pter = -1;

        /// <summary>
        /// Creates a rectangular mesh.
        /// </summary>
        /// <param name="center">The center of the mesh.</param>
        /// <param name="segmentsX">The number of horizontal segments.</param>
        /// <param name="segmentsY">The number of vertical segments</param>
        /// <param name="segmentSize">How many pixels wide/tall is each segment</param>
        /// <param name="color">Color</param>
        /// <returns></returns>
        public static TextureMesh CreateRectangularMesh(Vector3 center, int segmentsX, int segmentsY, int segmentSize, Color color)
        {
            return CreateRectangularMesh(center, segmentsX, segmentsY, segmentSize, segmentSize, color);
        }

        /// <summary>
        /// Creates a rectangular mesh.
        /// </summary>
        /// <param name="center">The center of the mesh.</param>
        /// <param name="segmentsX">The number of horizontal segments.</param>
        /// <param name="segmentsY">The number of vertical segments.</param>
        /// <param name="segmentSizeX">The width of each segment.</param>
        /// <param name="segmentSizeY">The height of each segment.</param>
        /// <param name="color">Color</param>
        /// <returns></returns>
        public static TextureMesh CreateRectangularMesh(Vector3 center, int segmentsX, int segmentsY, int segmentSizeX, int segmentSizeY, Color color)
        {
            int vertexCountX = segmentsX + 1;
            int vertexCountY = segmentsY + 1;
            int vertexCount = vertexCountX * vertexCountY;
            int indexCount = 6 * segmentsX * segmentsY;

            Vector3 topLeft = new Vector3(center.X - (segmentsX * segmentSizeX) / 2f, center.Y - (segmentsY * segmentSizeY) / 2f, 0f);

            TextureMesh mesh = new();
            mesh.vertices = new VertexPositionColorTexture[vertexCount];
            mesh.indicies = new short[indexCount];

            int iter = 0;
            for (int j = 0; j < vertexCountY; j++)
            {
                for (int i = 0; i < vertexCountX; i++)
                {
                    float x = topLeft.X + i * segmentSizeX;
                    float y = topLeft.Y + j * segmentSizeY;

                    float xComp = (float)i / segmentsX;
                    float yComp = (float)j / segmentsY;

                    mesh.vertices[iter++] = new VertexPositionColorTexture(new Vector3(x, y, 0f), color, new Vector2(xComp, yComp));
                }
            }

            iter = 0;

            for (int j = 0; j < segmentsY; j++)
            {
                for (int i = 0; i < segmentsX; i++)
                {
                    int topLeftIdx = j * vertexCountX + i;
                    int topRightIdx = topLeftIdx + 1;
                    int bottomLeftIdx = topLeftIdx + vertexCountX;
                    int bottomRightIdx = bottomLeftIdx + 1;
                    mesh.indicies[iter++] = (short)topLeftIdx;
                    mesh.indicies[iter++] = (short)bottomLeftIdx;
                    mesh.indicies[iter++] = (short)topRightIdx;
                    mesh.indicies[iter++] = (short)topRightIdx;
                    mesh.indicies[iter++] = (short)bottomLeftIdx;
                    mesh.indicies[iter++] = (short)bottomRightIdx;
                }
            }
            return mesh;
        }

        /// <summary>
        /// Creates a circular mesh.
        /// </summary>
        /// <param name="center">The center of the mesh.</param>
        /// <param name="radius">The radius of the mesh.</param>
        /// <param name="count">The number of segments aka sides, with 8 being an octagon.</param>
        /// <param name="color">Color</param>
        /// <returns></returns>
        public TextureMesh CreateCircularMesh(Vector3 center, float radius, int count, Color color)
        {
            int vertexCount = count + 1;
            int indexCount = count * 3;
            TextureMesh mesh = new TextureMesh();
            mesh.vertices = new VertexPositionColorTexture[vertexCount];
            mesh.indicies = new short[indexCount];

            mesh.vertices[0] = new VertexPositionColorTexture(center, color, new Vector2(0.5f, 0.5f));

            for (int i = 0; i < count; i++)
            {
                float angle = i * MathHelper.TwoPi / count;
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);
                Vector3 position = new Vector3(center.X + cos * radius, center.Y + sin * radius, 0f);
                Vector2 coords = new Vector2((cos + 1f) / 2f, (sin + 1f) / 2f);
                mesh.vertices[i + 1] = new VertexPositionColorTexture(position, color, coords);
            }

            int idx = 0;
            for (int i = 1; i <= count; i++)
            {
                mesh.indicies[idx] = 0;
                mesh.indicies[idx + 1] = (short)i;
                mesh.indicies[idx + 2] = (short)((i == count) ? 1 : i + 1);

                idx += 3;
            }
            return mesh;
        }

        /// <summary>
        /// Draws the points of the mesh as rectangles with lines connecting paired points.
        /// </summary>
        /// <param name="anchorPos">The center of the mesh's screen position</param>
        /// <param name="spriteBatch"></param>
        public void DrawDebugGrid(Vector2 anchorPos, SpriteBatch spriteBatch = default)
        {
            if (vertices == null || indicies == null)
                return;
            if (spriteBatch == default)
                spriteBatch = Main.spriteBatch;
            for (int i = 0; i < indicies.Length; i += 3)
            {
                int i1 = indicies[i];
                int i2 = indicies[i + 1];
                int i3 = indicies[i + 2];

                Vector2 v1 = anchorPos + new Vector2(
                    vertices[i1].Position.X,
                    vertices[i1].Position.Y);

                Vector2 v2 = anchorPos + new Vector2(
                    vertices[i2].Position.X,
                    vertices[i2].Position.Y);

                Vector2 v3 = anchorPos + new Vector2(
                    vertices[i3].Position.X,
                    vertices[i3].Position.Y);

                Utils.DrawLine(spriteBatch, v1, v2, Color.Orange * 0.4f, Color.Orange * 0.4f, 1);
                Utils.DrawLine(spriteBatch, v2, v3, Color.Orange * 0.4f, Color.Orange * 0.4f, 1);
                Utils.DrawLine(spriteBatch, v3, v1, Color.Orange * 0.4f, Color.Orange * 0.4f, 1);
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 v = anchorPos + new Vector2(vertices[i].Position.X, vertices[i].Position.Y);
                int rectSize = 4;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, v - Vector2.One * rectSize / 2, new Rectangle(0, 0, rectSize, rectSize), Main.DiscoColor);
            }
        }

        /// <summary>
        /// Allows control of the mesh by clicking its verticies and dragging them around.
        /// </summary>
        /// <param name="anchorPos">The center of the mesh's screen position</param>
        /// <param name="maxRange">The maximum distance at which vertices can be selected</param>
        public void ControlGrid(Vector2 anchorPos, float maxRange = 888888)
        {
            if (vertices == null || indicies == null)
                return;

            if (pter != -1)
            {
                if (Main.mouseLeft)
                {
                    Vector2 newPos = Main.MouseWorld - anchorPos;
                    vertices[pter].Position = new Vector3(newPos.X, newPos.Y, 0);

                }
                else
                {
                    pter = -1;
                }
            }
            else
            {
                float currentDist = maxRange;
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector2 point = new Vector2(vertices[i].Position.X, vertices[i].Position.Y) + anchorPos;
                    Rectangle maus = Utils.CenteredRectangle(Main.MouseWorld, 5 * Vector2.One);
                    Rectangle pointRect = Utils.CenteredRectangle(point, 2 * Vector2.One);
                    float dist = maus.Distance(pointRect.Center.ToVector2());
                    if (dist < currentDist)
                    {
                        if (Main.mouseLeft)
                        {
                            currentDist = dist;
                            pter = i;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws the mesh.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="anchorPos">The center of the mesh's screen position</param>
        /// <param name="texture"></param>
        /// <param name="frame"></param>
        /// <param name="opacity"></param>
        public void DrawMesh(SpriteBatch spriteBatch, Vector2 anchorPos, Asset<Texture2D> texture, Rectangle frame = default, float opacity = 1f)
        {
            if (indicies == null || vertices == null)
                return;
            spriteBatch.EnterShaderRegion();
            if (frame == default)
                frame = texture.Frame(1, 1, 0, 0);

            Matrix translation = Matrix.CreateTranslation(new Vector3(anchorPos.X, anchorPos.Y, 0));
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -220, 220);
            Matrix renderMatrix = translation * view * projection;
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["CalRemix:NormalDraw"].GetShader().Shader;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Parameters["textureResolution"].SetValue(texture.Value.Size());
                effect.Parameters["sampleTexture"].SetValue(texture.Value);
                effect.Parameters["frame"].SetValue(new Vector4(frame.X, frame.Y, frame.Width, frame.Height));
                effect.Parameters["uWorldViewProjection"].SetValue(renderMatrix);
                effect.Parameters["opacity"].SetValue(opacity);
                pass.Apply();

                Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indicies.ToArray(), 0, indicies.Length / 3);
            }
            spriteBatch.ExitShaderRegion();
        }
    }
}