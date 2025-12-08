using CalRemix.Core.Biomes;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using System;
using Terraria.Audio;
using CalamityMod.NPCs.Abyss;

namespace CalRemix.Content.NPCs.Subworlds.SingularPoint
{
    // [AutoloadBossHead]
    public class AnomalyOne : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[0] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public Vector2 OldPosition = new();

        public NPC MainHead
        {
            get => Main.npc[(int)NPC.localAI[0]];
            set => NPC.localAI[0] = value.whoAmI;
        }

        public NPC OrbHead
        {
            get => Main.npc[(int)NPC.localAI[1]];
            set => NPC.localAI[1] = value.whoAmI;
        }

        public bool PhaseTwo
        {
            get => NPC.localAI[2] == 1;
            set => NPC.localAI[2] = value.ToInt();
        }

        public enum PhaseType
        {
            SpawnAnim = 0,
            SineGas = 1,
            Flamethrower = 2,
            Knockout = 3
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }
        public ref float JawRotation => ref NPC.Calamity().newAI[2];

        public List<Vector2> ctrlPoints = new();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 150;
            NPC.height = 150;
            NPC.lifeMax = 300000;
            NPC.damage = 340;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            NPC.behindTiles = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (target == null || !target.active || target.dead)
            {
                NPC.active = false;
                return;
            }
            if (!MainHead.active || MainHead.type != ModContent.NPCType<AnomalyTwo>())
            {
                int dHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyTwo>());
                if (dHead != -1)
                {
                    MainHead = Main.npc[dHead];
                    NPC.netUpdate = true;
                }
            }
            if (!OrbHead.active || OrbHead.type != ModContent.NPCType<AnomalyTwo>())
            {
                int oHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyTwo>());
                if (oHead != -1)
                {
                    OrbHead = Main.npc[oHead];
                    NPC.netUpdate = true;
                }
            }
            float phaseGate = 0.7f;
            if (!PhaseTwo && NPC.life < (NPC.lifeMax * phaseGate))
            {
                ChangePhase(PhaseType.Knockout);
            }
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnim:
                    {
                        float startShake = 120;
                        float startRise = startShake + 60;
                        float endRise = startRise + 90;
                        float roarDuration = endRise + 120;
                        float stopLooking = roarDuration + 70;
                        float travelTime = stopLooking + 30;
                        float arenaCreationTime = travelTime + 60;
                        float lookDown = arenaCreationTime + 60;
                        float finish = lookDown + 50;

                        if (Timer == 0)
                        {
                            NPC.Center = target.Center + new Vector2(-400, 500);
                        }
                        if (Timer < startRise)
                        {
                            if (Timer >= startShake)
                            {
                                NPC.rotation = -MathHelper.PiOver2;
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = MathHelper.Lerp(1, 10, Utils.GetLerpValue(startShake, startRise, Timer, true));
                            }
                            EditPoints(new() { new(1, 0), new(0, 250), new(0, 550), new(0, 900) });
                        }
                        else if (Timer < endRise)
                        {
                            NPC.Opacity = 1;
                            if (Timer == startRise)
                            {
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 30;
                                SavePosition = NPC.Center - Vector2.UnitY * 900;
                                OldPosition = NPC.Center;
                            }
                            else
                            {
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.ExpInEasing(Utils.GetLerpValue(startRise, endRise, Timer, true), 1));
                            }
                        }
                        else if (Timer < roarDuration)
                        {
                            if (Timer == endRise)
                            {
                                SoundEngine.PlaySound(ReaperShark.EnragedRoarSound with { Pitch = 1 });
                            }
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, -MathHelper.PiOver4 + MathF.Sin(Timer * 0.4f + 1) * MathHelper.ToRadians(10), 0.1f);
                            JawRotation = MathHelper.ToRadians(40) + MathF.Sin(Timer * 1f) * MathHelper.ToRadians(4);
                            EditPoints(new() { new(), new(-200, 250), new(400, 550), new(0, 900) });
                        }
                        else if (Timer < stopLooking)
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, MathHelper.PiOver4, 0.2f);
                            JawRotation = 0;
                            EditPoints(new() { new(), new(-560, -20), new(260, 550), new(0, 900) });
                            OldPosition = NPC.Center;
                            SavePosition = NPC.Center + new Vector2(-2000, -500);
                        }
                        else if (Timer < travelTime)
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, -MathHelper.PiOver4, 0.2f);
                            JawRotation = Utils.AngleLerp(0, MathHelper.ToRadians(40), 0.2f);
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.ExpInEasing(Utils.GetLerpValue(stopLooking, travelTime, Timer, true), 1));
                            EditPoints(new() { new(), new(0, 250), new(0, 550), new(0, 900) });
                        }
                        else if (Timer < arenaCreationTime)
                        {
                            JawRotation = MathHelper.ToRadians(40) + MathF.Sin(Timer * 1f) * MathHelper.ToRadians(5);
                            if (Timer == travelTime)
                            {
                                SavePosition = new Vector2(OldPosition.X + 2000, OldPosition.Y - 500);
                                OldPosition = NPC.Center;
                            }
                            else
                            {
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(travelTime, arenaCreationTime, Timer, true), 1));
                            }
                        }
                        else if (Timer < lookDown)
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, MathHelper.PiOver4, 0.2f);
                            JawRotation = 0;
                            EditPoints(new() { new(), new(-560, -20), new(260, 550), new(0, 900) });
                            if (Timer == arenaCreationTime)
                            {
                                SavePosition = target.Center + new Vector2(-300, -200);
                                OldPosition = NPC.Center;
                            }
                            else
                            {
                                NPC.rotation = NPC.DirectionTo(target.Center).ToRotation();
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInEasing(Utils.GetLerpValue(arenaCreationTime, lookDown, Timer, true), 1));
                            }
                        }
                        else if (Timer >= finish)
                        {
                            //ChangePhase(Main.rand.NextBool() ? PhaseType.SineGas : PhaseType.Flamethrower);
                        }

                        if (Timer < stopLooking)
                        {
                            Main.blockInput = true;
                        }
                        else
                        {
                            Main.blockInput = false;
                        }
                    }
                    break;
                case PhaseType.SineGas:
                    {
                    }
                    break;
                case PhaseType.Flamethrower:
                    {
                    }
                    break;
                case PhaseType.Knockout:
                    {
                        NPC.dontTakeDamage = true;
                        NPC.velocity = Vector2.Zero;
                    }
                    break;
            }
            Timer++;
        }

        public void ChangePhase(PhaseType phase)
        {
            CurrentPhase = phase;
            ExtraOne = 0;
            ExtraTwo = 0;
            Timer = 0;
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
            writer.Write(NPC.Calamity().newAI[2]);
            writer.WriteVector2(OldPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
            OldPosition = reader.ReadVector2();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color outlineColor = Color.SeaGreen * NPC.Opacity;
            Vector3 outlineHSL = Main.rgbToHsl(outlineColor);
            float outlineThickness = 2;
            CalamityUtils.EnterShaderRegion(spriteBatch);
            GameShaders.Misc["CalamityMod:BasicTint"].UseOpacity(1f);
            GameShaders.Misc["CalamityMod:BasicTint"].UseColor(Main.hslToRgb(1 - outlineHSL.X, outlineHSL.Y, outlineHSL.Z));
            GameShaders.Misc["CalamityMod:BasicTint"].Apply();
            for (float i = 0; i < 1; i += 0.125f)
            {
                DrawGuy(spriteBatch, screenPos, drawColor, (i * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * outlineThickness, Color.SeaGreen);
            }
            CalamityUtils.ExitShaderRegion(spriteBatch);
            DrawGuy(spriteBatch, screenPos, drawColor);
            return false;
        }

        public void DrawGuy(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, Vector2 offset = default, Color overrideColor = default)
        {
            Color norm = overrideColor == default ? new Color(0, 7, 0) : overrideColor;

            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "_Jaw").Value;
            Texture2D chain = ModContent.Request<Texture2D>(Texture + "_Segment").Value;

            if (ctrlPoints.Count <= 0)
            {
                int pt = 4;
                for (int i = 0; i < pt; i++)
                {
                    float x = -tex.Width / 2 * -NPC.spriteDirection;
                    Vector2 newp = Vector2.Lerp(new Vector2(x, 0), new Vector2(x, 1000), i / (float)(pt - 1));
                    ctrlPoints.Add(newp);
                }
            }

            BezierCurve curve = new(ctrlPoints.ToArray());
            List<Vector2> points = curve.GetPoints(40);
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 lastPoint = i == 0 ? NPC.Center : points[i - 1];
                spriteBatch.Draw(chain, NPC.Center + offset + points[i] - screenPos, null, norm, points[i].DirectionTo(lastPoint).ToRotation(), chain.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            }

            Vector2 jawOrigin = new Vector2(NPC.spriteDirection == 1 ? jaw.Width - 106 : 106, 65);
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, norm, NPC.rotation, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(jaw, NPC.Center - screenPos + offset - (jaw.Size() / 2).RotatedBy(NPC.rotation) + jawOrigin.RotatedBy(NPC.rotation), null, norm, NPC.rotation + JawRotation, jawOrigin, NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(eye, NPC.Center - screenPos + offset, null, Color.SeaGreen, NPC.rotation, eye.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
        }

        public void EditPoints(List<Vector2> points)
        {
            if (Main.dedServ)
                return;
            if (ctrlPoints.Count <= 0)
                return;
            if (points[0] == Vector2.Zero)
                points[0] = new Vector2(-TextureAssets.Npc[Type].Value.Width / 2 * -NPC.spriteDirection, 0).RotatedBy(NPC.rotation);
            points[^1] = new Vector2(points[^1].X, (int)(Main.maxTilesY - 120) * 16);
            for (int i = 0; i < points.Count; i++)
            {
                ctrlPoints[i] = Vector2.Lerp(ctrlPoints[i], points[i], 0.4f);
            }
        }
    }
}