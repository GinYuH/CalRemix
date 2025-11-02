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
using CalRemix.Content.NPCs.Bosses.Noxus;
using rail;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.Audio;
using CalamityMod.Sounds;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class MonorianGemBoss : ModNPC
    {
        public Player Target => Main.player[NPC.target];

        public NPC Soul => Main.npc[(int)NPC.ai[1]];

        public float Timer => Soul.ai[0];
        public float State => Soul.ai[1];

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
                        float repoTime = 30;
                        float waitTime = repoTime + 20;
                        float dashTime = waitTime + 30;
                        float localTimer = Timer % dashTime;

                        if (localTimer < waitTime)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        else if (localTimer == waitTime)
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 30;
                        }
                        else if (localTimer > dashTime - 5)
                        {
                            NPC.velocity *= 0.98f;
                        }
                    }
                    break;
                case MonorianSoul.PhaseType.Shotgun:
                    {
                        float waitTime = 70;
                        float countDownTime = waitTime + 240;
                        Vector2 targetPos = Soul.Center + (Soul.DirectionTo(Target.Center) * Soul.Distance(Target.Center) * 2);
                        if (Timer < waitTime)
                        {
                            NPC.Center = Vector2.Lerp(NPC.Center, targetPos, 0.1f);
                        }
                        else if (Timer <= countDownTime)
                        {
                            NPC.Center = targetPos;
                        }
                    }
                    break;
                case MonorianSoul.PhaseType.Bounce:
                    {
                        float repositionTime = 40;
                        float gemPositionTime = repositionTime + 40;
                        Vector2 soulPos = Soul.ModNPC<MonorianSoul>().SavePosition;

                        if (Timer <= repositionTime)
                        {
                            NPC.velocity *= 0.9f;
                            SavePosition = Soul.Center - Vector2.UnitY * 100;
                            OldPosition = NPC.Center;
                        }
                        else if (Timer < gemPositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, Utils.GetLerpValue(repositionTime, gemPositionTime - 20, Timer, true));
                        }
                        else if (Timer == gemPositionTime)
                        {
                            NPC.velocity = Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, Main.rand.Next(0, 4) / 4f)).RotatedBy(MathHelper.PiOver4) * 10;
                        }
                        else
                        {
                            float disX = Math.Abs(NPC.Center.X - soulPos.X);
                            float disY = Math.Abs(NPC.Center.Y - soulPos.Y);
                            bool spawnProj = false;
                            if (disX > 300 && ExtraVar <= 0)
                            {
                                Vector2 flippedVelocity = new Vector2(-NPC.velocity.X, NPC.velocity.Y);
                                NPC.velocity = flippedVelocity.RotatedByRandom(MathHelper.PiOver4);
                                ExtraVar = 10;
                                spawnProj = true;
                            }
                            if (disY > 300 && ExtraVar2 <= 0)
                            {
                                Vector2 flippedVelocity = new Vector2(NPC.velocity.X, -NPC.velocity.Y);
                                NPC.velocity = flippedVelocity.RotatedByRandom(MathHelper.PiOver4);
                                ExtraVar2 = 10;
                                spawnProj = true;
                            }

                            if (spawnProj)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.PlasmaBoltSound with { Pitch = -0.4f }, NPC.Center);
                                for (int i = 0; i < 4; i++)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 4f)) * 10, ProjectileType<MonorianSoulBolt>(), CalRemixHelper.ProjectileDamage(200, 320), 1);
                                    }
                                }
                            }

                            ExtraVar--;
                            ExtraVar2--;
                        }
                    }
                    break;
                case MonorianSoul.PhaseType.Laser:
                    {
                        float repositionTime = 40;
                        float waitTime = repositionTime + 20;

                        if (Timer == 1)
                        {
                            NPC.velocity *= 0.9f;
                            SavePosition = Target.Center -  new Vector2(-Target.DirectionTo(NPC.Center).X.DirectionalSign() * 800, 200);
                            OldPosition = NPC.Center;
                        }
                        else if (Timer < repositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, Utils.GetLerpValue(0, repositionTime, Timer, true));
                        }
                        else if (Timer < waitTime)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        else if (Timer == waitTime)
                        {
                            NPC.velocity.X = NPC.DirectionTo(Target.Center).X.DirectionalSign() * 5;
                        }
                    }
                    break;
                case MonorianSoul.PhaseType.Metagross:
                    {
                        float repositionTime = 30;
                        float waitTime = repositionTime + 30;
                        float dashTime = waitTime + 10;
                        float waitAgain = dashTime + 30;
                        float localTimer = Timer % waitAgain;
                        if (localTimer <= 1)
                        {
                            OldPosition = NPC.Center;
                            SavePosition = Vector2.UnitX * Target.DirectionTo(NPC.Center).X.DirectionalSign() * 800;
                        }
                        else if (localTimer <= repositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition + Target.Center, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(0, repositionTime, localTimer, true), 1));
                        }
                        else if (localTimer < waitTime)
                        {
                            if (localTimer < waitTime - 10)
                            {
                                NPC.Center = SavePosition + Target.Center;
                            }
                            else
                            {
                                NPC.velocity = Vector2.Zero;
                            }
                        }
                        else if (localTimer == waitTime)
                        {
                            OldPosition = NPC.Center;
                            SavePosition = NPC.Center + new Vector2(NPC.DirectionTo(Target.Center).X.DirectionalSign() * NPC.Distance(Target.Center) * 2, 0);
                        }
                        else if (localTimer < dashTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(waitTime, dashTime, localTimer, true), 1));
                        }
                        else if (localTimer < waitAgain)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                    }
                    break;
                case MonorianSoul.PhaseType.Block:
                    {
                        NPC.Center = Vector2.Lerp(NPC.Center, Target.Center + Vector2.UnitX * Soul.direction * 410, 0.1f);
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
