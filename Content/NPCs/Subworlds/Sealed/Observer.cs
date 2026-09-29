using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Potions;
using CalRemix.Core.Biomes;
using CalRemix.UI;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Observer : ModNPC
    {
        public int pter = -1;

        public VertexPositionColorTexture[] vertexes;
        public short[] indexes;
        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
            NPC.width = 60;
            NPC.height = 134;
            NPC.lifeMax = 1000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.noGravity = false;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
            NPC.scale = 1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<TurnipBiome>().Type, ModContent.GetInstance<SealedDimensionBiome>().Type };
        }
        public override void AI()
        {
            NPC.TargetClosest();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<ObserverMask>(), 5);
            npcLoot.Add(ModContent.ItemType<ObserverEye>(), 1, 3, 6);
            npcLoot.Add(ItemID.CopperPickaxe, 10);
        }

        public void ControlMesh()
        {
            if (vertexes == null)
                return;

            if (pter != -1)
            {
                if (Main.mouseLeft)
                {
                    Vector2 newPos = Main.MouseWorld - NPC.Center;
                    vertexes[pter].Position = new Vector3(newPos.X, newPos.Y, 0);

                }
                else
                {
                    pter = -1;
                }
            }
            else
            {
                float ptClosest = 888888;
                for (int i = 0; i < vertexes.Length; i++)
                {
                    Vector2 point = new Vector2(vertexes[i].Position.X, vertexes[i].Position.Y) + NPC.Center;
                    Rectangle maus = Utils.CenteredRectangle(Main.MouseWorld, 5 * Vector2.One);
                    Rectangle pt = Utils.CenteredRectangle(point, 2 * Vector2.One);
                    if (maus.Distance(pt.Center.ToVector2()) < ptClosest)
                    {
                        if (Main.mouseLeft)
                        {
                            ptClosest = maus.Distance(pt.Center.ToVector2());
                            pter = i;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
            Asset<Texture2D> tex = TextureAssets.Npc[NPC.type];
            int primCount = 24;
            if (vertexes == null)
            {
                CreateRectangularMesh(Vector3.Zero, 30, 30, 4, Color.White);
                //CreateCircularMesh(new Vector3(0, 0, 0), 100, primCount, Color.White);
                return false;
            }
            NPC.noGravity = true;
            NPC.velocity.Y = 0;

            Main.spriteBatch.EnterShaderRegion();
            Rectangle testFrame = tex.Frame(1, 1, 0, 0);

            Matrix translation = Matrix.CreateTranslation(new Vector3(NPC.Center.X - screenPos.X, NPC.Center.Y - screenPos.Y, 0));
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -220, 220);
            Matrix renderMatrix = translation * view * projection;
            Effect effect = Terraria.Graphics.Effects.Filters.Scene["CalRemix:NormalDraw"].GetShader().Shader;

            VertexPositionColorTexture[] newish = new VertexPositionColorTexture[vertexes.Length];
            for (int i = 0; i < newish.Length; i++)
            {
                newish[i] = vertexes[i];
                float mag = Main.LocalPlayer.selectedItem;
                newish[i].Position.X += Main.rand.NextFloat(-mag, mag);
                newish[i].Position.Y += Main.rand.NextFloat(-mag, mag);

            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.Parameters["textureResolution"].SetValue(tex.Value.Size());
                effect.Parameters["sampleTexture"].SetValue(tex.Value);
                effect.Parameters["frame"].SetValue(new Vector4(testFrame.X, testFrame.Y, testFrame.Width, testFrame.Height));
                effect.Parameters["uWorldViewProjection"].SetValue(renderMatrix);
                effect.Parameters["opacity"].SetValue(1);
                pass.Apply();

                Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, newish, 0, vertexes.Length, indexes.ToArray(), 0, indexes.Length / 3);
            }


            Main.spriteBatch.ExitShaderRegion();
            return false;
            if (Main.LocalPlayer.selectedItem < 5)
            {
                for (int i = 0; i < indexes.Length; i += 3)
                {
                    int i1 = indexes[i];
                    int i2 = indexes[i + 1];
                    int i3 = indexes[i + 2];

                    Vector2 v1 = NPC.Center + new Vector2(
                        vertexes[i1].Position.X,
                        vertexes[i1].Position.Y);

                    Vector2 v2 = NPC.Center + new Vector2(
                        vertexes[i2].Position.X,
                        vertexes[i2].Position.Y);

                    Vector2 v3 = NPC.Center + new Vector2(
                        vertexes[i3].Position.X,
                        vertexes[i3].Position.Y);

                    Utils.DrawLine(spriteBatch, v1, v2, Color.Orange * 0.4f, Color.Orange * 0.4f, 1);
                    Utils.DrawLine(spriteBatch, v2, v3, Color.Orange * 0.4f, Color.Orange * 0.4f, 1);
                    Utils.DrawLine(spriteBatch, v3, v1, Color.Orange * 0.4f, Color.Orange * 0.4f, 1);
                }
                for (int i = 0; i < vertexes.Length; i++)
                {
                    Vector2 v = NPC.Center + new Vector2(vertexes[i].Position.X, vertexes[i].Position.Y) - screenPos;
                    int rectSize = 4;
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, v - Vector2.One * rectSize / 2, new Rectangle(0, 0, rectSize, rectSize), Main.DiscoColor);
                }
            }
            return false;
        }

        public void CreateCircularMesh(Vector3 center, float radius, int count, Color color)
        {
            int vertexCount = count + 1;
            int indexCount = count * 3;
            vertexes = new VertexPositionColorTexture[vertexCount];
            indexes = new short[indexCount];

            vertexes[0] = new VertexPositionColorTexture(center, color, new Vector2(0.5f, 0.5f));

            for (int i = 0; i < count; i++)
            {
                float angle = i * MathHelper.TwoPi / count;
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);
                Vector3 position = new Vector3(center.X + cos * radius, center.Y + sin * radius, 0f);
                Vector2 coords = new Vector2((cos + 1f) / 2f, (sin + 1f) / 2f);
                vertexes[i + 1] = new VertexPositionColorTexture(position, color, coords);
            }

            int idx = 0;
            for (int i = 1; i <= count; i++)
            {
                indexes[idx] = 0;
                indexes[idx + 1] = (short)i;
                indexes[idx + 2] = (short)((i == count) ? 1 : i + 1);

                idx += 3;
            }
        }

        public void CreateRectangularMesh(Vector3 center, int segmentsX, int segmentsY, int segmentSize, Color color)
        {
            int vertexCountX = segmentsX + 1;
            int vertexCountY = segmentsY + 1;
            int vertexCount = vertexCountX * vertexCountY;
            int indexCount = 6 * segmentsX * segmentsY;

            Vector3 topLeft = new Vector3(center.X - (segmentsX * segmentSize) / 2f, center.Y - (segmentsY * segmentSize) / 2f, 0f);

            vertexes = new VertexPositionColorTexture[vertexCount];
            indexes = new short[indexCount];

            int iter = 0;
            for (int j = 0; j < vertexCountY; j++)
            {
                for (int i = 0; i < vertexCountX; i++)
                {
                    float x = topLeft.X + i * segmentSize;
                    float y = topLeft.Y + j * segmentSize;

                    float xComp = (float)i / segmentsX;
                    float yComp = (float)j / segmentsY;

                    vertexes[iter++] = new VertexPositionColorTexture(new Vector3(x, y, 0f), color, new Vector2(xComp, yComp));
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
                    indexes[iter++] = (short)topLeftIdx;
                    indexes[iter++] = (short)bottomLeftIdx;
                    indexes[iter++] = (short)topRightIdx;
                    indexes[iter++] = (short)topRightIdx;
                    indexes[iter++] = (short)bottomLeftIdx;
                    indexes[iter++] = (short)bottomRightIdx;
                }
            }
        }
    }
}
