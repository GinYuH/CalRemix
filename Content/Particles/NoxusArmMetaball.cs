using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.NPCs.Bosses.Noxus;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.DataStructures;
using Terraria.GameContent;
using System;
using Terraria.Graphics;

namespace CalRemix.Core.Graphics
{
    /// <summary>
    /// This isn't for usage as an actual particle, it's used for Noxus' arms
    /// </summary>
    public class NoxusArmMetaball : Metaball
    {
        public class GasParticle
        {
            public float Size;

            public Vector2 Velocity;

            public Vector2 Center;
        }

        public static readonly List<GasParticle> GasParticles = new();

        public override MetaballDrawLayerType DrawContext => MetaballDrawLayerType.BeforeNPCs;

        public override Color EdgeColor => Color.MediumPurple;

        public override List<Asset<Texture2D>> Layers => new()
        {
            ModContent.Request<Texture2D>("CalRemix/Core/Graphics/Metaballs/NoxusGasLayer1")
        };

        public static void CreateParticle(Vector2 spawnPosition, Vector2 velocity, float size)
        {
            GasParticles.Add(new()
            {
                Center = spawnPosition,
                Velocity = velocity,
                Size = size
            });
        }

        public override void Update()
        {
            foreach (GasParticle particle in GasParticles)
            {
                particle.Velocity *= 0.99f;
                particle.Size *= 0.93f;
                particle.Center += particle.Velocity;
            }
            GasParticles.RemoveAll(p => p.Size <= 2f);
        }

        public VertexStrip cordStrip;

        public override void DrawInstances()
        {
            int noggus = NPC.FindFirstNPC(ModContent.NPCType<EntropicGod>());
            if (noggus != -1)
            {
                NPC noxNPC = Main.npc[noggus];
                EntropicGod noxus = Main.npc[noggus].ModNPC<EntropicGod>();

                // Determines when the fog is up
                int fogCoverTime = 145;

                if (noxus.BrainFogChargeCounter >= 1)
                {
                    fogCoverTime -= 55;
                }

                bool fogIsntUp = noxus.AttackTimer <= EntropicGod.DefaultTeleportDelay * 2f + fogCoverTime;

                // When Noxus is doing either portal attack, his hands hang out below the screen, which makes the visual weird so the arms are disabled for these attacks
                // Also disable while fog is up
                if (noxNPC.ai[0] == (int)EntropicGod.EntropicGodAttackType.PortalChainCharges2 || noxNPC.ai[0] == (int)EntropicGod.EntropicGodAttackType.PortalChainCharges || (noxNPC.ai[0] == (int)EntropicGod.EntropicGodAttackType.BrainFogAndThreeDimensionalCharges && !fogIsntUp))
                    return;

                // While teleporting don't draw the arms
                if (noxNPC.Opacity < 0.8f)
                    return;

                // Draw arm connectors
                for (int j = 0; j < 2; j++)
                {
                    List<VerletSimulatedSegment> toDraw = j == 0 ? EntropicGod.rArm : EntropicGod.lArm;

                    List<Vector2> positions = new List<Vector2>();
                    List<float> rotations = new List<float>();
                    if (noxus.CurrentAttack != EntropicGod.EntropicGodAttackType.MigraineAttack && noxus.CurrentAttack != EntropicGod.EntropicGodAttackType.Phase2Transition)
                    {
                        for (int i = 0; i < toDraw.Count; i++)
                        {
                            VerletSimulatedSegment seg = toDraw[i];
                            positions.Add(seg.position - Main.screenPosition);
                            if (i > 0)
                            {
                                rotations.Add(seg.position.DirectionTo(toDraw[i - 1].position).ToRotation());
                            }
                            else
                                rotations.Add(0);
                        }
                    }
                    else
                    {
                        float halfHeight = (toDraw[^1].position.Y - toDraw[0].position.Y) * 0.5f;

                        BezierCurve curve = new BezierCurve(toDraw[0].position, new Vector2(toDraw[0].position.X + 200 * (j == 0 ? -1 : 1), toDraw[0].position.Y - halfHeight), toDraw[^1].position);
                        List<Vector2> points = curve.GetPoints(22);

                        foreach (Vector2 point in points)
                        {
                            positions.Add(point - Main.screenPosition);
                        }

                        rotations = new List<float>();
                        for (int i = 0; i < positions.Count; i++)
                        {
                            Vector2 pos = positions[i];
                            if (i > 0)
                                rotations.Add(positions[i].DirectionTo(positions[i - 1]).ToRotation());
                            else
                                rotations.Add(0);
                        }
                    }

                    // Organ donor
                    Effect effect = ModContent.Request<Effect>("CalRemix/Assets/Effects/GoozmaCordMap").Value;
                    effect.Parameters["uTransformMatrix"].SetValue(Main.GameViewMatrix.EffectMatrix * Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1));
                    effect.Parameters["uTime"].SetValue((Main.GlobalTimeWrappedHourly) % 1f);
                    effect.Parameters["uTexture"].SetValue(ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/LiquidTrail").Value);
                    effect.Parameters["uMap"].SetValue(ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/ColorMap_0").Value);
                    effect.CurrentTechnique.Passes[0].Apply();

                    cordStrip ??= new VertexStrip();

                    Color ColorFunc(float progress) => Color.White;
                    float WidthFunc(float progress) => MathF.Pow(Utils.GetLerpValue(1.1f, 0.3f, progress, true), 0.6f) * Utils.GetLerpValue(-0.1f, 0.1f, progress, true) * 28f * noxNPC.scale;

                    cordStrip.PrepareStrip(positions.ToArray(), rotations.ToArray(), ColorFunc, WidthFunc, Vector2.Zero, positions.Count, true);
                    cordStrip.DrawTrail();
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                }

            }
        }
    }
}
