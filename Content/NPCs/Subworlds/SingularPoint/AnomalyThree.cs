using CalamityMod;
using CalamityMod.Particles;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Content.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Biomes;
using CalRemix.Core.Graphics;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalRemix.Content.NPCs.Subworlds.SingularPoint
{
    // [AutoloadBossHead]
    public class AnomalyThree : ModNPC
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

        public ref float EyeRotation => ref NPC.Calamity().newAI[2];

        public NPC MainHead
        {
            get => Main.npc[(int)NPC.localAI[0]];
            set => NPC.localAI[0] = value.whoAmI;
        }

        public NPC DragonHead
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
            BouncyBalls = 1,
            Orbitals = 2,
            Knockout = 3
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }

        public bool FinishedAttack = false;

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
            Vector2 arenaCenter = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8f;
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
            if (!DragonHead.active || DragonHead.type != ModContent.NPCType<AnomalyOne>())
            {
                int oHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyOne>());
                if (oHead != -1)
                {
                    DragonHead = Main.npc[oHead];
                    NPC.netUpdate = true;
                }
            }
            float phaseGate = 0.7f;
            if (!PhaseTwo && NPC.life < (NPC.lifeMax * phaseGate) && CurrentPhase != PhaseType.Knockout)
            {
                ChangePhase(PhaseType.Knockout);
                NPC.dontTakeDamage = true;
            }
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnim:
                    {

                        float startShake = 120;
                        float startRise = startShake + 60;
                        float endRise = startRise + 90;
                        float roarDuration = endRise + 170;
                        float stopLooking = roarDuration + 70;
                        float finish = stopLooking + 50;

                        bool skip = false;
                        if (skip && Timer < startRise)
                        {
                            Timer = startRise;
                        }

                        Vector2 arenaPoint = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8 + Vector2.UnitY * 200;
                        Vector2 screenShake = Main.rand.NextVector2Circular(Main.LocalPlayer.Calamity().GeneralScreenShakePower * CalamityClientConfig.Instance.ScreenshakePower, Main.LocalPlayer.Calamity().GeneralScreenShakePower * CalamityClientConfig.Instance.ScreenshakePower);
                        float arenaCreationWidth = 1000;

                        #region camera stuff
                        if (Timer < roarDuration)
                        {
                            if (Timer < startRise)
                            {
                                CameraPanSystem.CameraPanInterpolant = CalamityUtils.SineInEasing(Utils.GetLerpValue(0, startRise, Timer, true), 1);
                            }
                            else
                            {
                                CameraPanSystem.CameraPanInterpolant = 1;
                            }
                            CameraPanSystem.CameraFocusPoint = arenaPoint + screenShake;
                        }
                        else if (Timer > stopLooking)
                        {
                            if (Timer < finish)
                            {
                                CameraPanSystem.CameraPanInterpolant = CalamityUtils.SineInEasing(Utils.GetLerpValue(finish, stopLooking, Timer, true), 1);
                            }
                            else
                            {
                                CameraPanSystem.CameraPanInterpolant = 0;
                            }
                            CameraPanSystem.CameraFocusPoint = arenaPoint + screenShake;
                        }
                        else if (Timer <= stopLooking && Timer >= roarDuration)
                        {
                            CameraPanSystem.CameraFocusPoint = arenaPoint + screenShake;
                            CameraPanSystem.CameraPanInterpolant = 1;
                        }
                        #endregion

                        if (Timer < startRise)
                        {
                            if (Timer >= startShake)
                            {
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = MathHelper.Lerp(1, 10, Utils.GetLerpValue(startShake, startRise, Timer, true));
                            }
                        }
                        else if (Timer < endRise)
                        {
                            NPC.Opacity = 1;
                            if (Timer == startRise)
                            {
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 30;
                                SavePosition = NPC.Center - Vector2.UnitY * 900;
                                OldPosition = NPC.Center;
                                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyThreeRise"));
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
                                SoundEngine.PlaySound(AnomalyOne.RoarSound with { Pitch = -0.5f });
                            }
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 4;
                            NPC.rotation = MathF.Sin(Timer * 3f) * 0.05f;
                            if (Timer % 7 == 0)
                            {
                                GeneralParticleHandler.SpawnParticle(new HKShockwave(NPC.Center, Vector2.Zero, Color.SeaGreen * 0.9f, 0.1f, 22f, 20));
                            }
                            EyeRotation += 0.2f;
                        }
                        else if (Timer < stopLooking)
                        {
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);
                            OldPosition = NPC.Center;
                            SavePosition = NPC.Center + new Vector2(-arenaCreationWidth, -500);
                            EyeRotation += MathHelper.Lerp(0.2f, 0, Utils.GetLerpValue(roarDuration, stopLooking, Timer, true));
                        }
                        else if (Timer >= finish)
                        {
                            CameraPanSystem.CameraPanInterpolant = 0;
                            NPC.dontTakeDamage = false;
                            ChangePhase(Main.rand.NextBool() ? PhaseType.Orbitals : PhaseType.BouncyBalls);
                        }
                    }
                    break;
                case PhaseType.BouncyBalls:
                    {
                        int orbRate = 90;
                        int reposition = 40;
                        int spawnOrbs = reposition + 30;
                        int stopOrbs = spawnOrbs + (orbRate * 3);
                        int waitDuration = PhaseTwo ? 180 : 360;
                        int wait = stopOrbs + waitDuration;

                        if (Timer < reposition)
                        {
                            if (Timer <= 1)
                            {
                                SavePosition = arenaCenter - Vector2.UnitY * 100;
                                OldPosition = NPC.Center;
                            }
                            else
                            {
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, reposition, Timer, true), 1));
                            }
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(0, 40, Utils.GetLerpValue(0, reposition, Timer, true)) / 60f);
                        }
                        else if (Timer < spawnOrbs)
                        {
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(40, 180, Utils.GetLerpValue(reposition, spawnOrbs, Timer, true)) / 60f);
                        }
                        else if (Timer < stopOrbs)
                        {
                            float localTimer = Timer % orbRate;
                            if (localTimer % orbRate == 0)
                            {
                                SoundEngine.PlaySound(AnomalyDisciple3.BurstSound, NPC.Center);
                                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyThreeRadial"));
                                int projAmt = 6;
                                float randomOff = Main.rand.NextFloat(MathHelper.PiOver4);
                                for (int i = 0; i < projAmt; i++)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY.RotatedBy(randomOff + MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)projAmt)) * 10, ModContent.ProjectileType<AnomalyOrbuleBoss>(), CalRemixHelper.ProjectileDamage(300, 510), 1f, ai0: 1, ai2: NPC.whoAmI);
                                        if (PhaseTwo)
                                        {
                                            Main.projectile[p].timeLeft /= 2;
                                        }
                                    }
                                }
                                Vector2 dest = arenaCenter + Main.rand.NextVector2Circular(200, 200);
                                NPC.velocity = (dest - NPC.Center).ClampMagnitude(0, 20);
                            }
                            else
                            {
                                NPC.velocity *= 0.92f;
                            }
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(180, 620, Utils.GetLerpValue(spawnOrbs, spawnOrbs + 60, Timer, true)) / 60f);
                        }
                        else if (Timer < wait)
                        {
                            NPC.velocity *= 0.95f;
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(620, 0, Utils.GetLerpValue(stopOrbs, wait, Timer, true)) / 60f);
                        }
                        else if (Timer >= wait)
                        {
                            PhaseCheck(PhaseType.Orbitals);
                        }
                    }
                    break;
                case PhaseType.Orbitals:
                    {
                        int reposition = 30;
                        int spawnOrbs = reposition + 40;
                        int startMoving = spawnOrbs + 60;
                        int moveTime = startMoving + 300;
                        int wait = moveTime + (PhaseTwo ? 30 : 90);
                        if (Timer < reposition)
                        {
                            if (Timer <= 1)
                            {
                                NPC.direction = Main.rand.NextBool().ToDirectionInt();
                                OldPosition = NPC.Center;
                                SavePosition = arenaCenter + Vector2.UnitX * 1000 * NPC.direction;
                            }
                            else
                            {
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, reposition, Timer, true), 1));
                            }
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(0, 40, Utils.GetLerpValue(0, reposition, Timer, true)) / 60f);
                        }
                        else if (Timer < spawnOrbs)
                        {
                            if (Timer == reposition)
                            {
                                SoundEngine.PlaySound(AnomalyDisciple3.BurstSound, NPC.Center);
                                int orbCountMain = 8;
                                for (int i = 0; i < orbCountMain; i++)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AnomalyOrbuleBoss>(), CalRemixHelper.ProjectileDamage(280, 390), 1f, ai2: NPC.whoAmI);
                                    Main.projectile[p].localAI[1] = MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)orbCountMain);
                                    Main.projectile[p].localAI[2] = 600;
                                    Main.projectile[p].direction = 1;
                                    Main.projectile[p].netUpdate = true;
                                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
                                }
                                int orbCountSmall = 6;
                                for (int i = 0; i < orbCountSmall; i++)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AnomalyOrbuleBoss>(), CalRemixHelper.ProjectileDamage(280, 390), 1f, ai2: NPC.whoAmI);
                                    Main.projectile[p].localAI[1] = MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)orbCountSmall);
                                    Main.projectile[p].localAI[2] = 300;
                                    Main.projectile[p].direction = -1;
                                    Main.projectile[p].netUpdate = true;
                                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
                                }
                            }
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(40, 120, Utils.GetLerpValue(reposition, spawnOrbs, Timer, true)) / 60f);
                        }
                        else if (Timer < startMoving)
                        {
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(120, 40, Utils.GetLerpValue(spawnOrbs, startMoving, Timer, true)) / 60f);
                        }
                        else if (Timer < moveTime)
                        {
                            if (Timer == startMoving)
                            {
                                OldPosition = NPC.Center;
                                SavePosition = arenaCenter + Vector2.UnitX * 1000 * NPC.direction;
                            }
                            else
                            {
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(startMoving, moveTime, Timer, true), 1));
                            }
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(40, 180, Utils.GetLerpValue(startMoving, startMoving + 60, Timer, true)) / 60f);
                        }
                        else if (Timer < wait)
                        {
                            EyeRotation += MathHelper.ToRadians(MathHelper.Lerp(120, 0, Utils.GetLerpValue(moveTime, wait, Timer, true)) / 60f);
                        }
                        else
                        {
                            PhaseCheck(PhaseType.BouncyBalls);
                        }
                    }
                    break;
                case PhaseType.Knockout:
                    {
                        NPC.dontTakeDamage = true;
                        NPC.velocity = Vector2.Zero;

                        int pauseBeforeFall = 60;
                        int fallDuration = pauseBeforeFall + 60;
                        int finishAnim = fallDuration + 90;

                        PhaseTwo = true;

                        if (Timer < pauseBeforeFall)
                        {
                            if (Timer <= 1)
                            {
                                SoundEngine.PlaySound(AnomalyOne.RoarSound with { Pitch = 0.2f });
                                SavePosition = new Vector2(NPC.Center.X, arenaCenter.Y + 600);
                                OldPosition = NPC.Center;
                                int orbType = ModContent.ProjectileType<AnomalyOrbuleBoss>();
                                foreach (Projectile p in Main.ActiveProjectiles)
                                {
                                    if (p.type == orbType)
                                    {
                                        p.Kill();
                                    }
                                }
                            }
                            else
                            {
                                NPC.Center = OldPosition + Main.rand.NextVector2Circular(8, 8);
                            }
                        }
                        else if (Timer < fallDuration)
                        {
                            if (Timer == pauseBeforeFall)
                            {
                                SoundEngine.PlaySound(AnomalyTwo.FallSound);
                            }
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInEasing(Utils.GetLerpValue(pauseBeforeFall, fallDuration, Timer, true), 1));
                        }
                        else if (Timer > finishAnim)
                        {
                            NPC.Opacity = 0;
                            if (MainHead.type == ModContent.NPCType<AnomalyTwo>())
                            {
                                if (MainHead.ModNPC<AnomalyTwo>().CurrentPhase == AnomalyTwo.PhaseType.PhaseTwo)
                                    MainHead.ModNPC<AnomalyTwo>().ChangePhase(AnomalyTwo.PhaseType.Rise);
                            }
                            else
                            {
                                Mod.Logger.Error("Main head not found!");
                            }
                        }
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
            FinishedAttack = false;
        }
        public void PhaseCheck(PhaseType change)
        {
            FinishedAttack = true;
            bool canChange = true;
            if (MainHead.ModNPC != null)
            {
                if (MainHead.ModNPC is AnomalyTwo atwo)
                {
                    if (atwo.CurrentPhase >= AnomalyTwo.PhaseType.IdleBehaviour)
                        canChange = false;
                }
            }
            if (canChange)
            {
                ChangePhase(change);
            }
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
            writer.Write(FinishedAttack);
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
            FinishedAttack = reader.ReadBoolean();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D chain = ModContent.Request<Texture2D>(Texture + "_Segment").Value;
            if (MainHead.active)
            {
                if (MainHead.type == ModContent.NPCType<AnomalyTwo>())
                {
                    int chainAmt = 18;

                    for (int i = 0; i < chainAmt; i++)
                    {
                        Vector2 pos = Vector2.Lerp(NPC.Center, MainHead.Bottom, i / (float)chainAmt);
                        Main.EntitySpriteDraw(chain, pos - screenPos, null, Color.White, 0, chain.Size() / 2, NPC.scale, 0);
                    }
                }
            }
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
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;
            Color norm = overrideColor == default ? NPC.GetAlpha(drawColor) : overrideColor;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, norm, NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = NPC.Center - Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 3f)).RotatedBy(EyeRotation) * 50;
                spriteBatch.Draw(eye, pos - screenPos, null, Color.White, NPC.rotation, eye.Size() / 2, NPC.scale, 0, 0);
            }
        }
        public override bool CheckActive()
        {
            return !NPC.HasValidTarget;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}