using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Potions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using System;
using CalamityMod.Graphics.Primitives;
using CalRemix.UI;
using Terraria.UI;
using static Terraria.Graphics.Effects.Filters;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using CalamityMod.NPCs.Cryogen;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class MonorianGemBoss : ModNPC
    {
        public Player Target => Main.player[NPC.target];

        public NPC Soul => Main.npc[(int)NPC.ai[1]];

        public float Timer => Soul.ai[1];
        public float State => Soul.ai[0];

        public ref float ExtraVar => ref NPC.ai[2];

        public ref float ExtraVar2 => ref NPC.ai[3];
        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }
        public Vector2 OldPosition
        {
            get => new Vector2(NPC.localAI[2], NPC.localAI[1]);
            set
            {
                NPC.localAI[2] = value.X;
                NPC.localAI[1] = value.Y;
            }
        }
        public MonorianSoul.PhaseType CurrentPhase => (MonorianSoul.PhaseType)State;

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        #region Cube stuff
        public static List<Vector3> cubeVertices = new()
        {
            new ( 1, 1, 1 ), // Front bottom right
            new ( -1, 1, 1 ), // Front bottom left
            new ( 1, -1, 1 ), // Front top right
            new (-1, -1, 1 ), // Front top left
            new ( 1, 1, -1 ), // Back bottom right
            new ( -1, 1, -1 ), // Back bottom left
            new ( 1, -1, -1 ), // Back top right
            new (-1, -1, -1 ), // Back top left
        };

        public static List<Vector2> cubeEdges = new()
        {
            new (0, 1),
            new (0, 2),
            new (0, 4),
            new (3, 1),
            new (3, 2),
            new (3, 7),
            new (6, 2),
            new (6, 7),
            new (6, 4),
            new (5, 4),
            new (5, 1),
            new (5, 7),
        };

        public static List<Vector4> faces = new()
        {
                new Vector4(6, 2, 4, 0),
                new Vector4(7, 6, 3, 2),
                new Vector4(3, 2, 1, 0),
                new Vector4(7, 3, 5, 1),
                new Vector4(7, 6, 5, 4),
                new Vector4(5, 4, 1, 0),
        };
        #endregion

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lifeMax = 100000;
            NPC.damage = 270;
            NPC.defense = 40;
            NPC.noGravity = true;
            NPC.HitSound = Cryogen.HitSound with { Pitch = -1 };
            NPC.DeathSound = Cryogen.DeathSound with { Pitch = 1 };
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }
        public override void AI()
        {
            if (!Soul.active || Soul.type != NPCType<MonorianSoul>() || Soul.life <= 0)
            {
                NPC.StrikeInstantKill();
                return;
            }
            NPC.TargetClosest(false);
            switch (CurrentPhase)
            {
                case MonorianSoul.PhaseType.SpawnAnimation:
                    {

                    }
                    break;
                case MonorianSoul.PhaseType.Goozma:
                    {

                    }
                    break;
                case MonorianSoul.PhaseType.Shotgun:
                    {

                    }
                    break;
                case MonorianSoul.PhaseType.Bounce:
                    {

                    }
                    break;
                case MonorianSoul.PhaseType.Laser:
                    {

                    }
                    break;
                case MonorianSoul.PhaseType.Metagross:
                    {

                    }
                    break;
                case MonorianSoul.PhaseType.Block:
                    {

                    }
                    break;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Microsoft.Xna.Framework.Color drawColor)
        {
            spriteBatch.ExitShaderRegion();
            float animSpeed = 7;
            float size = 40;
            Matrix rotMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * animSpeed));
            Matrix rotMatrix2 = Matrix.CreateRotationZ(22 * MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * animSpeed));
            Matrix rotMatrix3 = Matrix.CreateRotationX(22 * MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * animSpeed));
            List<Vector3> rotatedVertices = new();
            for (int i = 0; i < cubeVertices.Count; i++)
            {
                Vector3 transformed = Vector3.Transform(cubeVertices[i], rotMatrix);
                transformed = Vector3.Transform(transformed, rotMatrix2);
                transformed = Vector3.Transform(transformed, rotMatrix3);
                rotatedVertices.Add(transformed);
            }

            List<Vector2> twodvertices = new();
            for (int i = 0; i <  rotatedVertices.Count; i++)
            {
                Vector2 newpoint = new Vector2(rotatedVertices[i].X, rotatedVertices[i].Y) * size;
                twodvertices.Add(newpoint);
            }

            Texture2D testSprite = TextureAssets.MagicPixel.Value;
            Rectangle testFrame = new Rectangle(0, 0, (int)size, (int)size);

            Matrix rotation = Matrix.CreateRotationX(0);
            Matrix translation = Matrix.CreateTranslation(new Vector3(NPC.Center.X - screenPos.X, NPC.Center.Y - screenPos.Y, 0));
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -220, 220);
            Matrix renderMatrix = rotation * translation * view * projection;
            Effect effect = Scene["CalRemix:NormalDraw"].GetShader().Shader;

            for (int i = 0; i < faces.Count; i++)
            {
                short[] indices = [0, 1, 2, 1, 3, 2];
                Vector4 positions = faces[i];

                Vector3 topLeft = new Vector3(rotatedVertices[(int)positions.X].X, rotatedVertices[(int)positions.X].Y, 0) * size;
                Vector3 topRight = new Vector3(rotatedVertices[(int)positions.Y].X, rotatedVertices[(int)positions.Y].Y, 0) * size;
                Vector3 botLeft = new Vector3(rotatedVertices[(int)positions.Z].X, rotatedVertices[(int)positions.Z].Y, 0) * size;
                Vector3 botRight = new Vector3(rotatedVertices[(int)positions.W].X, rotatedVertices[(int)positions.W].Y, 0) * size;

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
                vertices[0] = new(topLeft, Color.Red, new Vector2(0, 0));
                vertices[1] = new(topRight, Color.IndianRed, new Vector2(1, 0));
                vertices[2] = new(botLeft, Color.Red, new Vector2(0, 1));
                vertices[3] = new(botRight, Color.OrangeRed, new Vector2(1, 1));

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
    }
}
