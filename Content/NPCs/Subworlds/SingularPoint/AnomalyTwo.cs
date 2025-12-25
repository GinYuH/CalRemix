using CalamityMod;
using CalamityMod.Items.Potions;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Summon;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Biomes;
using CalRemix.Core.Graphics;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stubble.Core.Tokens;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds.SingularPoint
{
    [AutoloadBossHead]
    public class AnomalyTwo : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public ref float JawRotation => ref NPC.localAI[2];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[0] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public Vector2 OldPosition;

        public NPC DragonHead
        {
            get => Main.npc[(int)NPC.localAI[0]];
            set => NPC.localAI[0] = value.whoAmI;
        }

        public NPC OrbHead
        {
            get => Main.npc[(int)NPC.localAI[1]];
            set => NPC.localAI[1] = value.whoAmI;
        }

        public enum PhaseType
        {
            PhaseOne = 0,
            PhaseTwo = 1,
            Rise = 2,
            IdleBehaviour = 3,
            Enrage = 4,
            Desperation = 5
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }

        public static SoundStyle FallSound = new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyFall");
        public static SoundStyle RoarSound = new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyTwoRoar");
        public static SoundStyle ShortRoarSound = new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyTwoRoarShort");

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 150;
            NPC.height = 150;
            NPC.lifeMax = 500000;
            NPC.damage = 340;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 60);
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            NPC.behindTiles = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void BossHeadSlot(ref int index)
        {
            if (NPC.Opacity >= 1)
            {
                index = ModContent.GetModBossHeadSlot(BossHeadTexture);
            }
            else
            {
                index = -1;
            }
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            Vector2 arenaCenter = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8f;
            if (target == null || !target.active || target.dead)
            {
                NPC.active = false;
                if (DragonHead.active && DragonHead.type == ModContent.NPCType<AnomalyOne>())
                {
                    DragonHead.active = false;
                }
                if (OrbHead.active && OrbHead.type == ModContent.NPCType<AnomalyThree>())
                {
                    OrbHead.active = false;
                }
                return;
            }
            if (!DragonHead.active || DragonHead.type != ModContent.NPCType<AnomalyOne>())
            {
                int dHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyOne>());
                if (dHead != -1)
                {
                    DragonHead = Main.npc[dHead];
                    NPC.netUpdate = true;
                }
            }
            if (!OrbHead.active || OrbHead.type != ModContent.NPCType<AnomalyThree>())
            {
                int oHead = NPC.FindFirstNPC(ModContent.NPCType<AnomalyThree>());
                if (oHead != -1)
                {
                    OrbHead = Main.npc[oHead];
                    NPC.netUpdate = true;
                }
            }
            switch (CurrentPhase)
            {
                case PhaseType.PhaseOne:
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<AnomalyOne>()))
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                DragonHead = NPC.QuickSpawnNPC(ModContent.NPCType<AnomalyOne>());
                            }
                        }
                        if (DragonHead.active && DragonHead.type == ModContent.NPCType<AnomalyOne>())
                        {
                            if (DragonHead.ModNPC<AnomalyOne>().CurrentPhase == AnomalyOne.PhaseType.Knockout)
                            {
                                ChangePhase(PhaseType.PhaseTwo);
                            }
                        }
                    }
                    break;
                case PhaseType.PhaseTwo:
                    {
                        if (Timer > 120)
                        {
                            if (!NPC.AnyNPCs(ModContent.NPCType<AnomalyThree>()))
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    OrbHead = NPC.QuickSpawnNPC(ModContent.NPCType<AnomalyThree>());
                                }
                            }
                        }
                        if (OrbHead.active && DragonHead.type == ModContent.NPCType<AnomalyThree>())
                        {
                            if (OrbHead.ModNPC<AnomalyThree>().CurrentPhase == AnomalyThree.PhaseType.Knockout)
                            {
                                ChangePhase(PhaseType.Rise);
                            }
                        }
                    }
                    break;
                case PhaseType.Rise:
                    {
                        float startShake = 120;
                        float startRise = startShake + 60;
                        float endRise = startRise + 40;
                        float endRoar = endRise + 260;
                        float beginFight = endRoar + 90;

                        float awakenGuys = endRise + 60;

                        Vector2 arenaPoint = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8 + Vector2.UnitY * 200;
                        Vector2 screenShake = Main.rand.NextVector2Circular(Main.LocalPlayer.Calamity().GeneralScreenShakePower * CalamityClientConfig.Instance.ScreenshakePower, Main.LocalPlayer.Calamity().GeneralScreenShakePower * CalamityClientConfig.Instance.ScreenshakePower);
                        
                        #region camera stuff
                        if (Timer < endRoar)
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
                        else if (Timer >= endRoar)
                        {
                            if (Timer < beginFight)
                            {
                                CameraPanSystem.CameraPanInterpolant = CalamityUtils.SineInEasing(Utils.GetLerpValue(beginFight, endRoar, Timer, true), 1);
                            }
                            CameraPanSystem.CameraFocusPoint = arenaPoint + screenShake;
                        }
                        #endregion

                        if (Timer < startShake)
                        {

                        }
                        else if (Timer < startRise)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = MathHelper.Lerp(1, 10, Utils.GetLerpValue(startShake, startRise, Timer, true));
                        }
                        else if (Timer < endRise)
                        {
                            if (Timer == startRise)
                            {
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 30;
                                SavePosition = arenaCenter + Vector2.UnitY * 360;
                                OldPosition = NPC.Center;
                                NPC.Opacity = 1;
                            }
                            else
                            {
                                float riseLerp = CalamityUtils.SineOutEasing(Utils.GetLerpValue(startRise, endRise, Timer, true), 1);
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, riseLerp);
                            }
                        }
                        else if (Timer < endRoar)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 8;
                            if (Timer == endRise)
                            {
                                SoundEngine.PlaySound(RoarSound);
                            }
                            JawRotation = Utils.AngleLerp(JawRotation, -MathHelper.ToRadians(50) + MathF.Sin(Timer * 0.6f) * MathHelper.ToRadians(35 / 2f), 0.3f);
                            if (Timer % 7 == 0)
                            {
                                GeneralParticleHandler.SpawnParticle(new HKShockwave(NPC.Center - Vector2.UnitY * 100, Vector2.Zero, Color.SeaGreen * 0.7f, 0.1f, 22f, 20));
                            }
                            NPC.frame.Y = 1;
                        }
                        else if (Timer < beginFight)
                        {
                            NPC.frame.Y = 0;
                            JawRotation = Utils.AngleLerp(JawRotation, 0, 0.1f);
                        }
                        else if (Timer >= beginFight)
                        {
                            ChangePhase(PhaseType.IdleBehaviour);
                            Timer = 270;
                            DragonHead.ModNPC<AnomalyOne>().ChangePhase(Main.rand.NextBool() ? AnomalyOne.PhaseType.SineGas : AnomalyOne.PhaseType.Flamethrower);
                            OrbHead.ModNPC<AnomalyThree>().ChangePhase(Main.rand.NextBool() ? AnomalyThree.PhaseType.BouncyBalls : AnomalyThree.PhaseType.Orbitals);
                        }

                        if (Timer == awakenGuys)
                        {
                            DragonHead.ModNPC<AnomalyOne>().ChangePhase(AnomalyOne.PhaseType.Respawn);
                            OrbHead.ModNPC<AnomalyThree>().ChangePhase(AnomalyThree.PhaseType.Respawn);
                        }
                    }
                    break;
                case PhaseType.IdleBehaviour:
                    {
                        #region Control other heads
                        bool orbFinished = false;
                        bool serpentFinished = false;
                        if (OrbHead.ModNPC != null)
                        {
                            if (OrbHead.ModNPC is AnomalyThree athree)
                            {
                                if (athree.FinishedAttack)
                                    orbFinished = true;
                            }
                        }
                        if (DragonHead.ModNPC != null)
                        {
                            if (DragonHead.ModNPC is AnomalyOne aone)
                            {
                                if (aone.FinishedAttack)
                                    serpentFinished = true;
                            }
                        }

                        if (orbFinished && serpentFinished)
                        {
                            DragonHead.ModNPC<AnomalyOne>().ChangePhase(Main.rand.NextBool() ? AnomalyOne.PhaseType.SineGas : AnomalyOne.PhaseType.Flamethrower);
                            OrbHead.ModNPC<AnomalyThree>().ChangePhase(Main.rand.NextBool() ? AnomalyThree.PhaseType.BouncyBalls : AnomalyThree.PhaseType.Orbitals);
                        }

                        if (!OrbHead.active && !DragonHead.active)
                        {
                            NPC.dontTakeDamage = false;
                            ChangePhase(PhaseType.Enrage);
                            int spit = ModContent.ProjectileType<VirisiteDrop>();
                            foreach (Projectile p in Main.ActiveProjectiles)
                            {
                                if (p.type == spit)
                                {
                                    p.Kill();
                                }
                            }
                        }
                        #endregion

                        int idleTime = 420;
                        int waking = idleTime + 180;
                        int stopFiring = waking + 90;
                        int close = stopFiring + 30;

                        float localTimer = Timer % close;

                        if (localTimer < idleTime)
                        {
                            JawRotation = Utils.AngleLerp(JawRotation, 0, 0.1f);
                        }
                        else if (localTimer < waking)
                        {
                            NPC.frame.Y = 1;
                        }
                        else if (localTimer < stopFiring)
                        {
                            if (localTimer == waking)
                            {
                                SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
                            }
                            JawRotation = Utils.AngleLerp(JawRotation, -MathHelper.ToRadians(30) + MathF.Sin(Timer * 0.6f) * MathHelper.ToRadians(20 / 2f), 0.3f);
                            NPC.frame.Y = 1;

                            if (localTimer % 2 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 shootVel = -Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(15)) * Main.rand.NextFloat(12f, 16f);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - Vector2.UnitY * 60, shootVel, ModContent.ProjectileType<VirisiteDrop>(), CalRemixHelper.ProjectileDamage(160, 260), 1f, Main.myPlayer);
                                }
                            }
                        }
                        else if (localTimer < close)
                        {
                            JawRotation = Utils.AngleLerp(JawRotation, 0, CalamityUtils.SineInEasing(Utils.GetLerpValue(stopFiring, close, localTimer, true), 1));
                            NPC.frame.Y = 0;
                        }
                    }
                    break;
                case PhaseType.Enrage:
                    {
                        float endRoar = 260;
                        float beginFight = endRoar + 90;
                        Vector2 arenaPoint = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8 + Vector2.UnitY * 200;
                        Vector2 screenShake = Main.rand.NextVector2Circular(Main.LocalPlayer.Calamity().GeneralScreenShakePower * CalamityClientConfig.Instance.ScreenshakePower, Main.LocalPlayer.Calamity().GeneralScreenShakePower * CalamityClientConfig.Instance.ScreenshakePower);


                        #region camera stuff
                        if (Timer < endRoar)
                        {
                            if (Timer < endRoar)
                            {
                                CameraPanSystem.CameraPanInterpolant = CalamityUtils.SineInEasing(Utils.GetLerpValue(0, endRoar, Timer, true), 1);
                            }
                            else
                            {
                                CameraPanSystem.CameraPanInterpolant = 1;
                            }
                            CameraPanSystem.CameraFocusPoint = arenaPoint + screenShake;
                        }
                        else if (Timer >= endRoar)
                        {
                            if (Timer < beginFight)
                            {
                                CameraPanSystem.CameraPanInterpolant = CalamityUtils.SineInEasing(Utils.GetLerpValue(beginFight, endRoar, Timer, true), 1);
                            }
                            CameraPanSystem.CameraFocusPoint = arenaPoint + screenShake;
                        }
                        #endregion

                        if (Timer <= 1)
                        {
                            SavePosition = NPC.Center - Vector2.UnitY * 80;
                            OldPosition = NPC.Center;
                        }
                        if (Timer < endRoar)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 8;
                            if (Timer <= 1)
                            {
                                SoundEngine.PlaySound(RoarSound);
                            }
                            JawRotation = Utils.AngleLerp(JawRotation, -MathHelper.ToRadians(40) + MathF.Sin(Timer * 0.6f) * MathHelper.ToRadians(35 / 2f), 0.3f);
                            if (Timer % 7 == 0)
                            {
                                GeneralParticleHandler.SpawnParticle(new HKShockwave(NPC.Center - Vector2.UnitY * 100, Vector2.Zero, Color.SeaGreen * 0.8f, 0.1f, 22f, 20));
                            }
                            NPC.frame.Y = 1;

                            if (Timer > 1)
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, Utils.GetLerpValue(0, 40, Timer, true));
                        }
                        else if (Timer < beginFight)
                        {
                            JawRotation = Utils.AngleLerp(JawRotation, -MathHelper.ToRadians(15), 0.1f);
                        }
                        else if (Timer >= beginFight)
                        {
                            ChangePhase(PhaseType.Desperation);
                        }
                    }
                    break;
                case PhaseType.Desperation:
                    {
                        int idleTime = 300;
                        int stopFiring = idleTime + 300;

                        float localTimer = Timer % stopFiring;

                        if (localTimer < idleTime)
                        {
                            JawRotation = Utils.AngleLerp(JawRotation, -MathHelper.ToRadians(20) + MathF.Sin(Timer * 0.1f) * MathHelper.ToRadians(10 / 2f), 0.3f);
                        }
                        else if (localTimer < stopFiring)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 6;
                            if (localTimer == idleTime)
                            {
                                SoundEngine.PlaySound(RoarSound);
                            }
                            JawRotation = Utils.AngleLerp(JawRotation, -MathHelper.ToRadians(60) + MathF.Sin(Timer * 0.6f) * MathHelper.ToRadians(20 / 2f), 0.3f);
                            NPC.frame.Y = 1;

                            if (localTimer % 2 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 shootVel = -Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(12f, 16f);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - Vector2.UnitY * 60, shootVel, ModContent.ProjectileType<VirisiteDrop>(), CalRemixHelper.ProjectileDamage(160, 260), 1f, Main.myPlayer);
                                }
                            }
                        }
                        if (localTimer == stopFiring - 1)
                        {
                            SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
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
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
            writer.WriteVector2(OldPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
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
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "_Jaw").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;
            Texture2D core = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Texture2D tongue = ModContent.Request<Texture2D>(Texture + "_Tongue").Value;

            Color norm = overrideColor == default ? NPC.GetAlpha(drawColor) : overrideColor;
            //Color lightColor = overrideColor == default ? Color.White : overrideColor;
            int xOrig = 54;
            int yOrig = 330;
            int xOff = 50;
            int yOff = -60;
            int eyeYOff = -40;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, norm, 0, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, 0, 0);
            spriteBatch.Draw(tongue, NPC.Center - screenPos - Vector2.UnitY * 60, null, Color.White * NPC.Opacity, 0, new Vector2(tongue.Width / 2, tongue.Height), NPC.scale, 0, 0);
            spriteBatch.Draw(jaw, NPC.Center - screenPos + new Vector2(-xOff, yOff) + offset, null, norm, JawRotation, new Vector2(xOrig, yOrig), NPC.scale, 0, 0);
            spriteBatch.Draw(jaw, NPC.Center - screenPos + new Vector2(xOff, yOff) + offset, null, norm, -JawRotation, new Vector2(jaw.Width - xOrig, yOrig), NPC.scale, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(eye, NPC.Center - screenPos + new Vector2(-xOff, yOff) + Vector2.UnitY.RotatedBy(JawRotation) * eyeYOff + offset, eye.Frame(1, 2, 0, NPC.frame.Y), Color.White * NPC.Opacity, JawRotation, new Vector2(eye.Width / 2, eye.Height / 4), NPC.scale, 0, 0);
            spriteBatch.Draw(eye, NPC.Center - screenPos + new Vector2(xOff, yOff) + Vector2.UnitY.RotatedBy(-JawRotation) * eyeYOff + offset, eye.Frame(1, 2, 0, NPC.frame.Y), Color.White * NPC.Opacity, -JawRotation, new Vector2(eye.Width / 2, eye.Height / 4), NPC.scale, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(core, NPC.Center - screenPos + offset, null, Color.White * NPC.Opacity, 0, tex.Size() / 2, NPC.scale, 0, 0);
        }

        public override bool CheckActive()
        {
            return !NPC.HasValidTarget;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void OnKill()
        {
            RemixDowned.downedAnomaly = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(DropHelper.RevAndMaster, ModContent.ItemType<AnomalyRelic>()));
            npcLoot.AddNormalOnly(ModContent.ItemType<VirisiteTear>(), 1, 20, 35);
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ModContent.ItemType<SupremeHealingPotion>();
        }
    }
}