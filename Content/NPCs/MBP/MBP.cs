using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.DataStructures;
using CalamityMod.Items.Pets;
using CalamityMod.NPCs.Other;
using CalamityMod.NPCs.Perforator;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRemix.Content.NPCs.MBP
{
    public class MBP : ModNPC
    {
        public const string MBPSoundPath = "CalRemix/Assets/Sounds/MBP/";

        public static int QuestionGate => CalamityUtils.SecondsToFrames(3);

        public static int RawrGate => CalamityUtils.SecondsToFrames(5);

        public static int RequestGate => CalamityUtils.SecondsToFrames(7);

        public static int DeathRevGate => CalamityUtils.SecondsToFrames(11);

        public static int MoreBloodGate => CalamityUtils.SecondsToFrames(16);

        public static int SnapGate => CalamityUtils.SecondsToFrames(21);

        public static int DieGate => CalamityUtils.SecondsToFrames(24);

        public ref float Timer => ref NPC.ai[2];

        public List<VerletSimulatedSegment> PerfSegments;
        int perfSegmentCount = 6;

        public List<VerletSimulatedSegment> armSegmentsL;

        public List<VerletSimulatedSegment> armSegmentsR;

        public List<VerletSimulatedSegment> armSegmentsS;
        int armSegmentCount = 20;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Some Hive Mind Thing");
            this.HideFromBestiary();
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 68;
            NPC.height = 180;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                NPC.TargetClosest();
                if (NPC.HasPlayerTarget)
                {
                    if (Main.player[NPC.target].Distance(NPC.Center) < (20 * 16))
                    {
                        NPC.ai[0] = 1;
                    }
                }
            }
            else if (NPC.ai[0] == 1)
            {
                Timer++;
                if (Timer == CalamityUtils.SecondsToFrames(1))
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "1HmmHmm"));
                }
                if (Timer == QuestionGate)
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "2WhatbIsDis"));
                }
                if (Timer == RawrGate)
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "3Rawr"));
                }
                if (Timer == RequestGate)
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "4IWantBlood"));
                }
                if (Timer == DeathRevGate)
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "5DeathAndRev"));
                }
                if (Timer == MoreBloodGate)
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "6MoreBlood"));
                }
                if (Timer == SnapGate)
                {
                    SoundEngine.PlaySound(new SoundStyle(MBPSoundPath + "7SarcSnap"));
                }
                if (Timer == DieGate)
                {
                    SoundEngine.PlaySound(THELORDE.DeathSound);
                }
                if (Timer > DieGate + 420)
                    NPC.alpha += 5;
                if (NPC.alpha > 255)
                {
                    NPC.active = false;
                }

                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (!n.friendly && !NPCID.Sets.ActsLikeTownNPC[n.type] && !NPCID.Sets.CountsAsCritter[n.type] && !n.CountsAsACritter)
                    {
                        n.active = false;
                    }
                }
            }

            if (Timer == DieGate + 16)
            {
                CalRemixWorld.seenMBP = true;
                CalRemixWorld.UpdateWorldBool();

                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 222;
                SoundEngine.PlaySound(PerforatorHive.DeathSound);

                int perfPos = 240;

                for (int i = 0; i < 10; i++)
                {
                    int goreType = Main.rand.Next(1, 5);
                    string goreSuffix = goreType.ToString();
                    if (goreType == 1)
                        goreSuffix = "";
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + Vector2.UnitX * perfPos, new Vector2(-22, 0).RotatedByRandom(MathHelper.PiOver4), CalRemix.CalMod.Find<ModGore>("Hive" + goreSuffix).Type);
                }

                for (int i = 0; i < 40; i++)
                {
                    Dust.NewDust(NPC.Center + Vector2.UnitX * perfPos - new Vector2(60, 60), 100, 100, DustID.Blood, Scale: Main.rand.NextFloat(1.8f, 5.4f));
                }

                Item.NewItem(NPC.GetSource_FromThis(), new Rectangle((int)NPC.Center.X + perfPos, (int)NPC.Center.Y, 1, 1), ModContent.ItemType<BloodyVein>());
            }

            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frame.Y += 1;
                NPC.frameCounter = 0;
                if (NPC.frame.Y >= 9)
                    NPC.frame.Y = 0;
            }

            NPC.netUpdate = true;
            NPC.netSpam = 0;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.ZoneCorrupt || CalRemixWorld.seenMBP || !NPC.downedBoss2 || CalamityPlayer.areThereAnyDamnBosses || CalamityPlayer.areThereAnyDamnEvents)
            {
                return 0f;
            }
            return SpawnCondition.Corruption.Chance * 0.04f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            string path = "CalRemix/Content/NPCs/MBP/";
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            Texture2D hiveEye = ModContent.Request<Texture2D>(path + "MBPEye").Value;
            Texture2D hiveEyelid = ModContent.Request<Texture2D>(path + "MBPEyelid").Value;
            Texture2D hiveEyeBase = ModContent.Request<Texture2D>(path + "MBPEyeBase").Value;

            Vector2 hiveEyeOffset = NPC.Center + new Vector2(0, 8) - screenPos;
            Vector2 eyelidOffset = hiveEyeOffset - new Vector2(4, 4);

            hiveEyeOffset += DetermineHiveEyeOffset();

            spriteBatch.Draw(hiveEyeBase, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), 0, tex.Size() / 2, 1f, 0, 0);
            spriteBatch.Draw(hiveEye, hiveEyeOffset, null, NPC.GetAlpha(drawColor), 0, hiveEye.Size() / 2, 1f, 0, 0);
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), 0, tex.Size() / 2, 1f, 0, 0);

            spriteBatch.Draw(hiveEyelid, eyelidOffset, null, NPC.GetAlpha(drawColor), 0, hiveEye.Size() / 2, 1f, 0, 0);

            DrawArms(spriteBatch, screenPos);

            DrawStand(spriteBatch, screenPos, NPC.GetAlpha(drawColor));

            if (Timer >= RawrGate && Timer < DieGate + 16)
                DrawPerforator(spriteBatch, screenPos, NPC.GetAlpha(drawColor));

            if (Timer >= DieGate && Timer <= DieGate + 30)
                DrawLorde(spriteBatch, screenPos, NPC.GetAlpha(drawColor));

            return false;
        }

        public void DrawArms(SpriteBatch sb, Vector2 screenPos)
        {
            Vector2 armLBase = new Vector2(32, 10);
            Vector2 armRBase = new Vector2(-28, 10);

            Vector2 handL = DetermineSideHandPosition(armLBase, 1);
            Vector2 handR = DetermineSideHandPosition(armRBase, -1);
            Vector2 handS = DetermineSideHandPosition(armLBase, 0);

            DrawSingularArm(sb, screenPos, armLBase, handL, ref armSegmentsL);
            DrawSingularArm(sb, screenPos, armRBase, handR, ref armSegmentsR);

            if (Timer >= SnapGate)
                DrawSingularArm(sb, screenPos, armLBase, handS, ref armSegmentsS, true);
        }

        public Vector2 DetermineSideHandPosition(Vector2 startPos, int dir)
        {
            Vector2 ret = Vector2.Zero;

            if (dir != 0)
            {
                int appear = DeathRevGate + 60 + (dir == 1 ? 10 : 0);
                int stop = appear + 30 + (dir == 1 ? 10 : 0);
                Vector2 startHand = startPos + Vector2.UnitY * 60;
                Vector2 endHand = startPos + new Vector2(-20 * -dir, 10);

                ret = DetermineObjectPosition(appear, stop, startHand, endHand, false);
            }
            else
            {
                int move = SnapGate;
                int stopUp = SnapGate + 20;
                int startAgain = stopUp + 5;
                int stopLeft = startAgain + 5;
                int startSnap = stopLeft + 10;
                int endSnap = startSnap + 10;

                Vector2 initPos = startPos + Vector2.UnitY * 60;
                Vector2 upPos = startPos + new Vector2(10, 16);
                Vector2 sidePos = startPos + new Vector2(6, 14);
                Vector2 snapPos = startPos + new Vector2(30, 8);

                ret += DetermineObjectPosition(0, move, initPos);
                ret += DetermineObjectPosition(move, stopUp, initPos, upPos);
                ret += DetermineObjectPosition(stopUp, startAgain, upPos);
                ret += DetermineObjectPosition(startAgain, stopLeft, upPos, sidePos);
                ret += DetermineObjectPosition(stopLeft, startSnap, sidePos);
                ret += DetermineObjectPosition(startSnap, endSnap, sidePos, snapPos);
                ret += DetermineObjectPosition(endSnap, 9999999, snapPos);
            }

            return ret;
        }

        public void DrawSingularArm(SpriteBatch sb, Vector2 screenPos, Vector2 startPos, Vector2 endPos, ref List<VerletSimulatedSegment> segments, bool snap = false)
        {
            string path = "CalRemix/Content/NPCs/MBP/";
            Texture2D death = ModContent.Request<Texture2D>(path + "Death").Value;
            Texture2D rev = ModContent.Request<Texture2D>(path + "Revengeance").Value;
            Texture2D snapT = ModContent.Request<Texture2D>(path + "MBPSnap").Value;
            if (segments == null || segments.Count <= 0)
            {
                segments = new List<VerletSimulatedSegment>(armSegmentCount);
                for (int i = 0; i < armSegmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(startPos + Vector2.UnitY * i * 5);
                    segments.Add(segment);
                }

                segments[0].locked = true;
                segments[^1].locked = true;
            }

            segments[0].oldPosition = segments[0].position;
            segments[0].position = startPos;

            segments[^1].oldPosition = segments[^1].position;
            segments[^1].position = endPos;

            segments = VerletSimulatedSegment.SimpleSimulation(segments, 2, 20, 0.3f);

            for (int i = 0; i < segments.Count; i++)
            {
                VerletSimulatedSegment seg = segments[i];
                float rot = 0f;

                if (i > 0)
                    rot = seg.position.DirectionTo(segments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                else
                    rot = NPC.rotation;

                int size = 6;
                int length = i == 0 ? size : (int)seg.position.Distance(segments[i - 1].position);

                sb.Draw(TextureAssets.MagicPixel.Value, NPC.Center + seg.position - screenPos, new Rectangle(0, 0, size, length + 2), Color.Black * NPC.Opacity, rot, new Vector2(4, 4), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

                if (i == segments.Count - 1)
                {
                    Texture2D item = seg.position.X > 0 ? rev : death;

                    if (snap)
                    {
                        item = snapT;
                        //if (Timer < SnapGate + 50)
                           // return;
                    }

                    sb.Draw(item, NPC.Center + seg.position - screenPos, null, Color.White * NPC.Opacity, snap ? rot - MathHelper.Pi : 0, new Vector2(item.Width / 2, item.Height / 2), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                }
            }
        }

        public void DrawLorde(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[ModContent.NPCType<THELORDE>()].Value;
            Vector2 pos = NPC.Center - screenPos + Vector2.Lerp(new Vector2(1000, -1000), new Vector2(-600, 1000), Utils.GetLerpValue(DieGate, DieGate + 30, Timer, true));

            sb.Draw(tex, pos, tex.Frame(2, 7, 0, 1), drawColor, -MathHelper.PiOver4, new Vector2(tex.Width / 4, tex.Height / 14), 1f, 0, 0);
        }

        #region Perforator

        public void DrawPerforator(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            string path = "CalRemix/Content/NPCs/MBP/";
            Texture2D perf = ModContent.Request<Texture2D>(path + "MBPPerf").Value;
            Texture2D perfEye = ModContent.Request<Texture2D>(path + "MBPPerfEye").Value;
            Texture2D perfEyeBase = ModContent.Request<Texture2D>(path + "MBPPerfEyeBase").Value;
            Texture2D perfLid = ModContent.Request<Texture2D>(path + "MBPPerfEyelid").Value;

            float perfRotation = NPC.localAI[3];
            Vector2 perfBodyOffset = NPC.Center - screenPos;

            perfBodyOffset += DeterminePerforatorOffset();

            Vector2 lidOffset = perfBodyOffset + new Vector2(2, -20);

            Vector2 perfEyeOffset = perfBodyOffset;

            perfEyeOffset += DeterminePerforatorEyeOffset();

            perfEyeOffset += new Vector2(2, -16).RotatedBy(perfRotation);

            DrawPerfTail(sb, screenPos, drawColor, perfBodyOffset);
            sb.Draw(perfEyeBase, perfBodyOffset, perf.Frame(1, 10, 0, NPC.frame.Y), drawColor, perfRotation, new Vector2(perf.Width / 2, perf.Height / 20), 1f, 0, 0);
            sb.Draw(perfEye, perfEyeOffset, null, drawColor, perfRotation, perfEye.Size() / 2, 1f, 0, 0);
            sb.Draw(perf, perfBodyOffset, perf.Frame(1, 10, 0, NPC.frame.Y), drawColor, perfRotation, new Vector2(perf.Width / 2, perf.Height / 20), 1f, 0, 0);
            if (Timer >= (MoreBloodGate + 10) && Timer <= (SnapGate - 20))
                sb.Draw(perfLid, lidOffset, null, drawColor, perfRotation, perfLid.Size() / 2, 1f, 0, 0);
        }

        public Vector2 DeterminePerforatorOffset()
        {
            Vector2 ret = Vector2.Zero;

            int startDist = 600;
            int endDist = 260;
            Vector2 startPos = new Vector2(startDist, 0);
            Vector2 endPos = new Vector2(endDist, 0);

            int beginMove = RawrGate + 10;
            int endMove = RawrGate + 40;
            int startShake1 = RequestGate + 10;
            int endShake1 = startShake1 + 180;

            int startShake2 = MoreBloodGate + 10;
            int endShake2 = startShake2 + 180;

            int startShake3 = endShake2 + 30;
            int endShake3 = startShake3 + 60;

            ret += DetermineObjectPosition(0, beginMove, startPos);
            ret += DetermineObjectPosition(beginMove, endMove, startPos, endPos);
            ret += DetermineObjectPosition(endMove, startShake1, endPos);
            int shakeStrength = Timer > (startShake1 + 60) ? 8 : 4;            
            ret += Shake(startShake1, endShake1, endPos, shakeStrength);
            ret += DetermineObjectPosition(endShake1, startShake2, endPos);
            ret += Shake(startShake2, endShake2, endPos, 4);
            ret += DetermineObjectPosition(endShake2, startShake3, endPos);
            ret += Shake(startShake3, endShake3, endPos, 22);
            ret += DetermineObjectPosition(endShake3, 999999, endPos);

            // rotate as he does his initial slide
            if (Timer > beginMove && Timer < endMove)
            {
                NPC.localAI[3] = MathHelper.Lerp(0, -MathHelper.PiOver4, Utils.GetLerpValue(beginMove, endMove, Timer, true));
            }
            else if (Timer >= endMove)
            {
                NPC.localAI[3] = MathHelper.Lerp(-MathHelper.PiOver4, 0, Utils.GetLerpValue(endMove, endMove + 6, Timer, true));
            }
            return ret;
        }

        public Vector2 Shake(int start, int end, Vector2 endPos, int shakeStrength)
        {
            if (Timer > start && Timer <= end)
            {
                return endPos + Main.rand.NextVector2Square(-shakeStrength, shakeStrength);
            }
            return Vector2.Zero;
        }
        public Vector2 DeterminePerforatorEyeOffset()
        {
            Vector2 ret = Vector2.Zero;

            int moveEye = RequestGate + 200;
            int endMoveEye = moveEye + 30;

            int poutStart = MoreBloodGate + 10;
            int poutStopMove = poutStart + 20;
            int poutEnd = poutStart + 280;
            int endMoveBack = SnapGate + 10;

            Vector2 lookLeft = new Vector2(-4, 0);
            Vector2 poutPos = new Vector2(4, -2);

            ret += DetermineObjectPosition(0, moveEye, Vector2.Zero);
            ret += DetermineObjectPosition(moveEye, endMoveEye, Vector2.Zero, lookLeft);
            ret += DetermineObjectPosition(endMoveEye, poutStart, lookLeft);
            ret += DetermineObjectPosition(poutStart, poutStopMove, lookLeft, poutPos);
            ret += DetermineObjectPosition(poutStopMove, poutEnd, poutPos);
            ret += DetermineObjectPosition(poutEnd, endMoveBack, poutPos, lookLeft);
            ret += DetermineObjectPosition(endMoveBack, 99999, lookLeft);

            return ret;
        }

        public void DrawPerfTail(SpriteBatch sb, Vector2 screenPos, Color drawColor, Vector2 bodyOffset)
        {
            string path = "CalRemix/Content/NPCs/MBP/";

            Texture2D perfTail = ModContent.Request<Texture2D>(path + "MBPPerfTail").Value;
            Vector2 perfTailOffset = bodyOffset + new Vector2(0, 24).RotatedBy(NPC.localAI[3]);

            if (PerfSegments == null || PerfSegments.Count <= 0)
            {
                PerfSegments = new List<VerletSimulatedSegment>(perfSegmentCount);
                for (int i = 0; i < perfSegmentCount; i++)
                {
                    VerletSimulatedSegment segment = new VerletSimulatedSegment(perfTailOffset + screenPos + Vector2.UnitY * 5 * i);
                    PerfSegments.Add(segment);
                }

                PerfSegments[0].locked = true;
            }

            PerfSegments[0].oldPosition = PerfSegments[0].position;
            PerfSegments[0].position = perfTailOffset + screenPos;

            PerfSegments = VerletSimulatedSegment.SimpleSimulation(PerfSegments, 10, 20, 2f);

            for (int i = 0; i < PerfSegments.Count; i++)
            {
                VerletSimulatedSegment seg = PerfSegments[i];
                float rot = 0f;
                Rectangle frame = i switch
                {
                    0 => new Rectangle(0, 0, 32, 10),
                    5 => new Rectangle(0, 34, 32, 40),
                    _ => new Rectangle(0, 18, 32, 10)
                };

                if (i > 0)
                    rot = seg.position.DirectionTo(PerfSegments[i - 1].position).ToRotation() + MathHelper.PiOver2;
                else
                    rot = NPC.rotation;

                sb.Draw(perfTail, seg.position - screenPos, frame, NPC.GetAlpha(Lighting.GetColor(new Point((int)seg.position.X / 16, (int)seg.position.Y / 16))), rot, new Vector2(frame.Width / 2, frame.Y), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0); ;
            }
        }
        #endregion

        public void DrawStand(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Main.instance.LoadTiles(TileID.WorkBenches);
            Main.instance.LoadTiles(TileID.Tables);
            Texture2D table = TextureAssets.Tile[TileID.Tables].Value;
            Texture2D bench = TextureAssets.Tile[TileID.WorkBenches].Value;

            int blockSize = 16;
            int blockSize18 = 18;

            int graniteBenchCoord = 1044;
            Vector2 benchScale = new Vector2(6f, 4f);
            Vector2 benchPos = NPC.Center - screenPos + new Vector2(-96, 40);

            int graniteTableCoord = 1782;
            Vector2 tableScale = new Vector2(4f, 5f);
            Vector2 tablePos = NPC.Center - screenPos + new Vector2(-96, -110);

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    sb.Draw(table, tablePos + new Vector2(i, j) * blockSize * tableScale , new Rectangle(graniteTableCoord + i * blockSize18, j * blockSize18, blockSize, blockSize), drawColor, 0, Vector2.Zero, tableScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

            for (int i = 0; i < 2; i++)
                sb.Draw(bench, benchPos + Vector2.UnitX * blockSize * i * benchScale.X, new Rectangle(graniteBenchCoord + i * blockSize18, 0, blockSize, blockSize), drawColor, 0, Vector2.Zero, benchScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

        }

        public Vector2 DetermineHiveEyeOffset()
        {
            Vector2 ret = Vector2.Zero;

            // "Hmm? What is this?"
            int startHmm = QuestionGate + 10;
            int endHmm = QuestionGate + 50;
            int offsetX = 4;

            // Death Rev
            int startDR = DeathRevGate + 10;
            int endLookDown = startDR + 40;
            int startLookLeft = endLookDown + 20;
            int endLookLeft = startLookLeft + 20;
            int endDR = endLookLeft + 30;
            int offsetDown = 4;
            int offsetLeft = -4;
            int offsetRight = 4;

            Vector2 startPos = new Vector2(0, offsetDown);
            Vector2 lookUp = new Vector2(offsetX, 0);
            Vector2 lookLeft = new Vector2(offsetLeft, 0);
            Vector2 lookRight = new Vector2(offsetRight, 0);

            ret += DetermineObjectPosition(0, startHmm, startPos);
            ret += DetermineObjectPosition(startHmm, endHmm, startPos, lookUp);
            ret += DetermineObjectPosition(endHmm, startDR, lookUp);
            ret += DetermineObjectPosition(startDR, endLookDown, lookUp, startPos);
            ret += DetermineObjectPosition(endLookDown, startLookLeft, startPos);
            ret += DetermineObjectPosition(startLookLeft, endLookLeft, startPos, lookLeft);
            ret += DetermineObjectPosition(endLookLeft, endDR, lookLeft, lookRight);
            ret += DetermineObjectPosition(endDR, 99999999, lookRight);

            return ret;
        }

        #region functions
        public Vector2 DetermineObjectPosition(int start, int end, Vector2 startPos, Vector2 endPos, bool fallBack = true)
        {
            if ((Timer > start && Timer <= end) || !fallBack)
                return Vector2.Lerp(startPos, endPos, EaseInSine(Utils.GetLerpValue(start, end, Timer, true)));
            else
                return Vector2.Zero;
        }

        public Vector2 DetermineObjectPosition(int start, int end, Vector2 pos)
        {
            if (Timer > start && Timer <= end)
                return pos;
            else
                return Vector2.Zero;
        }

        public float EaseInSine(float num)
        {
            return 1 - MathF.Cos((num * MathF.PI) / 2);
        }
        #endregion
    }
}
