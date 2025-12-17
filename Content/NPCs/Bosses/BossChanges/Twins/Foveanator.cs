using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.DataStructures;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social.Base;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.Twins
{
    public class Foveanator : ModNPC
    {
        public static int phase1IconIndex;
        public static int phase2IconIndex;

        public static Asset<Texture2D> GlowTexture;
        public static Asset<Texture2D> GlowTextureCTP;
        public static Asset<Texture2D> TextureCTP;

        public static bool CTPActive()
        {
            if (Main.netMode == NetmodeID.Server)
                return false;
            AssetSourceController aSC = Main.AssetSourceController;
            IEnumerable<Terraria.IO.ResourcePack> list = aSC.ActiveResourcePackList.EnabledPacks;
            foreach (Terraria.IO.ResourcePack item in list)
            {
                if (item.Name.Equals("Calamity Texture Pack"))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Load()
        {
            string phase1IconPath = "CalRemix/Content/NPCs/Bosses/BossChanges/Twins/Foveanator_Head_Boss";
            string phase2IconPath = "CalRemix/Content/NPCs/Bosses/BossChanges/Twins/Foveanator_Phase2_Head_Boss";
            phase1IconIndex = Mod.AddBossHeadTexture(phase1IconPath, -1);
            phase2IconIndex = Mod.AddBossHeadTexture(phase2IconPath, -1);
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);

            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossChanges/Twins/FoveanatorGlow", AssetRequestMode.AsyncLoad);
                GlowTextureCTP = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossChanges/Twins/FoveanatorCTPGlow", AssetRequestMode.AsyncLoad);
                TextureCTP = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossChanges/Twins/FoveanatorCTP", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 62;
            NPC.DR_NERD(0.2f);

            NPC.width = 100;
            NPC.height = 100;
            if (Main.tenthAnniversaryWorld)
                NPC.scale *= 0.5f;
            if (CalamityWorld.LegendaryMode)
                NPC.scale *= 0.8f;

            NPC.defense = 10;
            NPC.lifeMax = 24000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.SpawnWithHigherTime(30);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            AnimationType = NPCID.Retinazer;
            Music = MusicID.Boss2;

            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
            for (int i = 0; i < 4; i++)
                writer.Write(NPC.Calamity().newAI[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
            for (int i = 0; i < 4; i++)
                NPC.Calamity().newAI[i] = reader.ReadSingle();
        }

        public override void BossHeadSlot(ref int index) => index = NPC.ai[0] >= 3f ? phase2IconIndex : phase1IconIndex;

        public override void AI()
        {
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            bool bossRush = BossRushEvent.BossRushActive;

            // Get a target
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                CalamityUtils.CalamityTargeting(NPC, default);

            float enrageScale = bossRush ? 0.5f : 0.3f;
            if (Main.IsItDay() || bossRush)
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 1f;
            }

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Check for Retinazer
            bool retAlive = false;
            if (CalamityGlobalNPC.laserEye != -1)
                retAlive = Main.npc[CalamityGlobalNPC.laserEye].active;

            // Check for Spazmatism or Retinazer
            bool spazAlive = false;
            if (CalamityGlobalNPC.fireEye != -1)
                spazAlive = Main.npc[CalamityGlobalNPC.fireEye].active;

            // Explode if ret and spaz are dead
            if (!retAlive && !spazAlive)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            Vector2 lookDirection = NPC.Center - Main.player[NPC.target].Center;
            int direction = (NPC.Center.X < Main.player[NPC.target].position.X + Main.player[NPC.target].width) ? -1 : 1;

            float hoverRotation = lookDirection.ToRotation() + MathHelper.PiOver2;
            if (hoverRotation < 0f)
                hoverRotation += MathHelper.TwoPi;
            else if (hoverRotation > MathHelper.TwoPi)
                hoverRotation -= MathHelper.TwoPi;

            // Rotate faster during energy bomb and laser barrage
            float rotationRate = NPC.ai[1] == 3f ? 0.25f : 0.15f;
            NPC.rotation = NPC.rotation.AngleTowards(hoverRotation, rotationRate);

            if (Main.rand.NextBool(5))
            {
                int foveaDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + NPC.height * 0.25f), NPC.width, (int)(NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f, 0, default, 1f);
                Dust dust = Main.dust[foveaDust];
                dust.velocity.X *= 0.5f;
                dust.velocity.Y *= 0.1f;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && !Main.player[NPC.target].dead && NPC.timeLeft < 10)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (i != NPC.whoAmI && Main.npc[i].active && (Main.npc[i].type == NPCID.Retinazer || Main.npc[i].type == NPCID.Spazmatism || Main.npc[i].type == NPC.type) && Main.npc[i].timeLeft - 1 > NPC.timeLeft)
                        NPC.timeLeft = Main.npc[i].timeLeft - 1;
                }
            }

            // Phase HP ratios
            float phase2LifeRatio = 0.85f;
            float finalPhaseLifeRatio = 0.4f;

            // Movement variables
            float phase1MaxSpeedIncrease = 2f;
            float phase1MaxAccelerationIncrease = 0.025f;
            float phase1MaxChargeSpeedIncrease = 1f;

            // Phase duration variables
            float phase1MaxLaserPhaseDurationDecrease = 120f;

            // Go to phase 2 early if Spaz and Ret health total goes below 50%
            // Go to final phase early if Spaz and Ret health total goes below 20%
            float retAndSpazHPRatio = 0f;
            if (CalamityGlobalNPC.fireEye != -1)
            {
                if (Main.npc[CalamityGlobalNPC.fireEye].active)
                    retAndSpazHPRatio += Main.npc[CalamityGlobalNPC.fireEye].life / (float)Main.npc[CalamityGlobalNPC.fireEye].lifeMax;
            }
            if (CalamityGlobalNPC.laserEye != -1)
            {
                if (Main.npc[CalamityGlobalNPC.laserEye].active)
                    retAndSpazHPRatio += Main.npc[CalamityGlobalNPC.laserEye].life / (float)Main.npc[CalamityGlobalNPC.laserEye].lifeMax;
            }

            // Phase checks
            bool phase2 = lifeRatio < phase2LifeRatio || retAndSpazHPRatio < 0.5f;
            bool finalPhase = lifeRatio < finalPhaseLifeRatio || retAndSpazHPRatio < 0.2f;

            Vector2 mechQueenSpacing = Vector2.Zero;
            if (NPC.IsMechQueenUp)
            {
                NPC NPC = Main.npc[NPC.mechQueen];
                Vector2 mechQueenCenter = NPC.GetMechQueenCenter();
                Vector2 eyePosition = new Vector2(0f, -300f);
                eyePosition *= 0.75f;
                float mechdusaRotation = NPC.velocity.X * 0.025f;
                mechQueenSpacing = mechQueenCenter + eyePosition;
                mechQueenSpacing = mechQueenSpacing.RotatedBy(mechdusaRotation, mechQueenCenter);
            }

            NPC.reflectsProjectiles = false;

            // Despawn
            if (Main.player[NPC.target].dead)
            {
                NPC.velocity.Y -= 0.04f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                    return;
                }
            }

            else if (NPC.ai[0] == 0f)
            {
                if (NPC.ai[1] == 0f)
                {
                    // Avoid cheap bullshit
                    NPC.damage = 0;

                    float maxVelocity = 8.5f;
                    float acceleration = 0.12f;
                    maxVelocity += 4.5f * enrageScale;
                    acceleration += 0.06f * enrageScale;

                    maxVelocity += phase1MaxSpeedIncrease * ((1f - lifeRatio) / (1f - phase2LifeRatio));
                    acceleration += phase1MaxAccelerationIncrease * ((1f - lifeRatio) / (1f - phase2LifeRatio));

                    if (CalamityWorld.LegendaryMode)
                    {
                        maxVelocity *= 1.15f;
                        acceleration *= 1.15f;
                    }

                    // Foveanator stays further away from the player than Retinazer (Retinazer distance is 300, Foveanator distance is 450)
                    float distanceFromTarget = 450f;
                    Vector2 destination = Main.player[NPC.target].Center + Vector2.UnitX * distanceFromTarget * direction - Vector2.UnitY * distanceFromTarget;
                    float distanceFromDestination = (destination - NPC.Center).Length();
                    Vector2 idealVelocity = (destination - NPC.Center).SafeNormalize(Vector2.UnitX * direction);

                    if (NPC.IsMechQueenUp)
                    {
                        maxVelocity = 14f;

                        destination = mechQueenSpacing;
                        distanceFromDestination = (destination - NPC.Center).Length();
                        idealVelocity = (destination - NPC.Center).SafeNormalize(Vector2.UnitY);

                        if (distanceFromDestination > maxVelocity)
                            idealVelocity *= maxVelocity / distanceFromDestination;

                        float inertia = 60f;
                        NPC.velocity = (NPC.velocity * (inertia - 1f) + idealVelocity) / inertia;
                    }
                    else
                        NPC.SimpleFlyMovement(idealVelocity * maxVelocity, acceleration);

                    // Foveanator stays in this phase longer than Retinazer (Foveanator time is 450, Retinazer time is 300)
                    // Foveanator has longer cooldown between firing projectiles than Retinazer (Foveanator cooldown is 60, Retinazer cooldown is 30)
                    float phaseGateValue = 450f - (phase1MaxLaserPhaseDurationDecrease * ((1f - lifeRatio) / (1f - phase2LifeRatio)));
                    float laserGateValue = 60f;
                    if (NPC.IsMechQueenUp)
                    {
                        phaseGateValue = 1350f;
                        laserGateValue = ((!NPC.npcsFoundForCheckActive[NPCID.TheDestroyerBody]) ? 120f : 180f);
                    }

                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] >= phaseGateValue)
                    {
                        NPC.ai[1] = 1f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;

                        CalamityUtils.CalamityTargeting(NPC, default);

                        NPC.netUpdate = true;
                    }
                    else if (distanceFromDestination < 960f)
                    {
                        if (!Main.player[NPC.target].dead)
                        {
                            NPC.ai[3] += 1f;
                            if (CalamityWorld.LegendaryMode)
                                NPC.ai[3] += 0.5f;
                        }

                        if (NPC.ai[3] >= laserGateValue)
                        {
                            NPC.ai[3] = 0f;

                            SoundEngine.PlaySound(AstrumDeusHead.LaserSound, NPC.Center);

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                float laserSpeed = 6f + 1.5f * enrageScale;
                                int type = ModContent.ProjectileType<FoveanatorLaser>();
                                int damage = CalRemixHelper.ProjectileDamage(50, 108);

                                // Reduce mech boss projectile damage depending on the new ore progression changes
                                if (CalamityServerConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive)
                                {
                                    double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                                    double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                                    if (!NPC.downedMechBossAny)
                                        damage = (int)(damage * firstMechMultiplier);
                                    else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                                        damage = (int)(damage * secondMechMultiplier);
                                }

                                Vector2 laserVelocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * laserSpeed;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + laserVelocity.SafeNormalize(Vector2.UnitY) * 60f, laserVelocity, type, damage, 0f, Main.myPlayer);
                            }
                        }
                    }
                }
                else if (NPC.ai[1] == 1f)
                {
                    // Set damage
                    NPC.damage = NPC.defDamage;

                    NPC.rotation = hoverRotation;

                    float chargeSpeed = 4f;
                    chargeSpeed += 1f * enrageScale;
                    if (CalamityWorld.LegendaryMode)
                        chargeSpeed += phase1MaxChargeSpeedIncrease * ((1f - lifeRatio) / (1f - phase2LifeRatio));
                    if (CalamityWorld.LegendaryMode)
                        chargeSpeed += 1f;

                    NPC.velocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * chargeSpeed;

                    NPC.ai[1] = 2f;
                }
                else if (NPC.ai[1] == 2f)
                {
                    // Set damage
                    NPC.damage = NPC.defDamage;

                    NPC.ai[2] += 1f;
                    float decelerateGateValue = 72f + (12f * ((1f - lifeRatio) / (1f - phase2LifeRatio)));
                    if (NPC.ai[2] >= decelerateGateValue)
                    {
                        // Avoid cheap bullshit
                        NPC.damage = 0;

                        float decelerationMultiplier = 0.8f - (0.2f * ((1f - lifeRatio) / (1f - phase2LifeRatio)));
                        NPC.velocity *= decelerationMultiplier;

                        if (Math.Abs(NPC.velocity.X) < 0.1)
                            NPC.velocity.X = 0f;
                        if (Math.Abs(NPC.velocity.Y) < 0.1)
                            NPC.velocity.Y = 0f;
                    }
                    else
                    {
                        // Accelerative charge
                        float maxVelocity = 24f;
                        maxVelocity += 6f * enrageScale;

                        if (CalamityWorld.LegendaryMode)
                            maxVelocity += (phase1MaxChargeSpeedIncrease * 6f) * ((1f - lifeRatio) / (1f - phase2LifeRatio));

                        if (NPC.velocity.Length() < maxVelocity)
                        {
                            NPC.velocity *= 1.04f;
                            if (NPC.velocity.Length() > maxVelocity)
                            {
                                NPC.velocity.Normalize();
                                NPC.velocity *= maxVelocity;
                            }
                        }

                        NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                    }

                    float delayBeforeChargingAgain = 96f - (6f * ((1f - lifeRatio) / (1f - phase2LifeRatio)));
                    if (NPC.ai[2] >= delayBeforeChargingAgain)
                    {
                        NPC.ai[3] += 1f;
                        NPC.ai[2] = 0f;

                        NPC.rotation = hoverRotation;

                        float totalCharges = 3f;
                        if (NPC.ai[3] >= totalCharges)
                        {
                            NPC.ai[1] = 0f;
                            NPC.ai[3] = 0f;

                            CalamityUtils.CalamityTargeting(NPC, default);
                        }
                        else
                            NPC.ai[1] = 1f;
                    }
                }

                // Enter phase 2
                if (phase2)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;

                    CalamityUtils.CalamityTargeting(NPC, default);

                    NPC.netUpdate = true;
                }
            }

            else if (NPC.ai[0] == 1f || NPC.ai[0] == 2f)
            {
                // Avoid cheap bullshit
                NPC.damage = 0;

                if (NPC.IsMechQueenUp)
                    NPC.reflectsProjectiles = true;

                if (NPC.ai[0] == 1f)
                {
                    NPC.ai[2] += 0.005f;
                    if (NPC.ai[2] > 0.5f)
                        NPC.ai[2] = 0.5f;
                }
                else
                {
                    NPC.ai[2] -= 0.005f;
                    if (NPC.ai[2] < 0f)
                        NPC.ai[2] = 0f;
                }

                NPC.rotation += NPC.ai[2];

                NPC.ai[1] += 1f;
                if (NPC.ai[2] >= 0.2f)
                {
                    if (NPC.ai[1] % 20f == 0f)
                    {
                        SoundEngine.PlaySound(AstrumDeusHead.LaserSound, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int type = ModContent.ProjectileType<FoveanatorEnergyBomb>();
                            int damage = CalRemixHelper.ProjectileDamage(70, 120);

                            // Reduce mech boss projectile damage depending on the new ore progression changes
                            if (CalamityServerConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive)
                            {
                                double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                                double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                                if (!NPC.downedMechBossAny)
                                    damage = (int)(damage * firstMechMultiplier);
                                else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                                    damage = (int)(damage * secondMechMultiplier);
                            }

                            float projectileSpeed = 16f;
                            Vector2 projectileVelocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * projectileSpeed;
                            int spread = 30;
                            float rotation = MathHelper.ToRadians(spread);
                            for (int i = 0; i < 2; i++)
                            {
                                Vector2 perturbedSpeed = projectileVelocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + perturbedSpeed.SafeNormalize(Vector2.UnitY) * 60f, perturbedSpeed, type, damage, 0f, Main.myPlayer, 1f);
                            }
                        }
                    }
                }

                if (NPC.ai[1] == 100f)
                {
                    NPC.ai[0] += 1f;
                    NPC.ai[1] = 0f;
                    if (NPC.ai[0] == 3f)
                    {
                        NPC.ai[2] = 0f;
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit1, NPC.Center);

                        if (!Main.dedServ)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Foveanator1").Type, NPC.scale);
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7, NPC.scale);
                                Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 6, NPC.scale);
                            }
                        }

                        for (int j = 0; j < 20; j++)
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);

                        SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
                    }
                }

                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);

                NPC.velocity *= 0.98f;
                if (Math.Abs(NPC.velocity.X) < 0.1)
                    NPC.velocity.X = 0f;
                if (Math.Abs(NPC.velocity.Y) < 0.1)
                    NPC.velocity.Y = 0f;
            }
            else
            {
                // If in phase 2 but Spaz or Ret aren't
                bool spazOrRetInPhase1 = false;
                if (CalamityGlobalNPC.fireEye != -1)
                {
                    if (Main.npc[CalamityGlobalNPC.fireEye].active)
                        spazOrRetInPhase1 = Main.npc[CalamityGlobalNPC.fireEye].ai[0] == 1f || Main.npc[CalamityGlobalNPC.fireEye].ai[0] == 2f || Main.npc[CalamityGlobalNPC.fireEye].ai[0] == 0f;
                }
                if (CalamityGlobalNPC.laserEye != -1)
                {
                    if (Main.npc[CalamityGlobalNPC.laserEye].active)
                        spazOrRetInPhase1 = Main.npc[CalamityGlobalNPC.laserEye].ai[0] == 1f || Main.npc[CalamityGlobalNPC.laserEye].ai[0] == 2f || Main.npc[CalamityGlobalNPC.laserEye].ai[0] == 0f;
                }

                NPC.chaseable = !spazOrRetInPhase1;

                int setDamage = (int)Math.Round(NPC.defDamage * 1.5);
                NPC.defense = NPC.defDefense + 14;
                calamityGlobalNPC.DR = spazOrRetInPhase1 ? 0.9999f : 0.2f;
                calamityGlobalNPC.unbreakableDR = spazOrRetInPhase1;
                calamityGlobalNPC.CurrentlyIncreasingDefenseOrDR = spazOrRetInPhase1;

                NPC.HitSound = SoundID.NPCHit4;

                if (NPC.ai[1] == 0f)
                {
                    // Avoid cheap bullshit
                    NPC.damage = 0;

                    float maxVelocity = 4f + (1.2f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                    float acceleration = 0.06f + (0.02f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                    maxVelocity += 2f * enrageScale;
                    acceleration += 0.04f * enrageScale;

                    float distanceFromTarget = 360f;
                    Vector2 destination = Main.player[NPC.target].Center + Vector2.UnitX * distanceFromTarget * direction;
                    float distanceFromDestination = (destination - NPC.Center).Length();
                    Vector2 idealVelocity = (destination - NPC.Center).SafeNormalize(Vector2.UnitX * direction);

                    if (!NPC.IsMechQueenUp)
                    {
                        // Boost speed if too far from target
                        if (distanceFromDestination > distanceFromTarget)
                            maxVelocity += MathHelper.Lerp(0f, 12f, MathHelper.Clamp((distanceFromDestination - distanceFromTarget) / 1000f, 0f, 1f));

                        if (CalamityWorld.LegendaryMode)
                        {
                            maxVelocity *= 1.15f;
                            acceleration *= 1.15f;
                        }

                        NPC.SimpleFlyMovement(idealVelocity * maxVelocity, acceleration);
                    }

                    // Fire flamethrower for x seconds
                    NPC.ai[2] += (retAlive && spazAlive) ? 1f : 2f;
                    float phaseGateValue = NPC.IsMechQueenUp ? 1350f : 360f - (90f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                    if (NPC.ai[2] >= phaseGateValue)
                    {
                        // Go to charge phase or fire large barrage of lasers and energy bombs if in final phase
                        NPC.ai[1] = finalPhase ? 3f : 1f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }

                    // Fire fireballs and flamethrower
                    else if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                    {
                        // Play flame sound on timer
                        NPC.localAI[2] += 1f;
                        if (NPC.localAI[2] > 22f)
                        {
                            NPC.localAI[2] = 0f;
                            SoundEngine.PlaySound(SoundID.Item34, NPC.Center);
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.localAI[1] += 1f;
                            if (NPC.localAI[1] > 4f)
                            {
                                NPC.localAI[1] = 0f;

                                float foveanatorFlamethrowerSpeed = 4f + 2f * enrageScale;
                                float timeForFlamethrowerToReachMaxVelocity = (retAlive && spazAlive) ? 120f : 60f;
                                float flamethrowerSpeedScalar = MathHelper.Clamp(NPC.ai[2] / timeForFlamethrowerToReachMaxVelocity, 0f, 1f);
                                foveanatorFlamethrowerSpeed = MathHelper.Lerp(0.1f, foveanatorFlamethrowerSpeed, flamethrowerSpeedScalar);

                                int type = ModContent.ProjectileType<FoveanatorFlamethrower>();
                                int damage = CalRemixHelper.ProjectileDamage(70, 120);

                                // Reduce mech boss projectile damage depending on the new ore progression changes
                                if (CalamityServerConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive)
                                {
                                    double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                                    double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                                    if (!NPC.downedMechBossAny)
                                        damage = (int)(damage * firstMechMultiplier);
                                    else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                                        damage = (int)(damage * secondMechMultiplier);
                                }

                                Vector2 flamethrowerVelocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * foveanatorFlamethrowerSpeed + NPC.velocity * 0.5f;

                                if (NPC.IsMechQueenUp)
                                    flamethrowerVelocity = (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * foveanatorFlamethrowerSpeed + NPC.velocity * 0.5f;

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + flamethrowerVelocity.SafeNormalize(Vector2.UnitY) * 100f, flamethrowerVelocity, type, damage, 0f, Main.myPlayer);
                            }
                        }
                    }

                    if (NPC.IsMechQueenUp)
                    {
                        maxVelocity = 14f;

                        destination = mechQueenSpacing;
                        distanceFromDestination = (destination - NPC.Center).Length();
                        idealVelocity = (destination - NPC.Center).SafeNormalize(Vector2.UnitY);

                        if (distanceFromDestination > maxVelocity)
                            idealVelocity *= maxVelocity / distanceFromDestination;

                        float inertia = 60f;
                        NPC.velocity = (NPC.velocity * (inertia - 1f) + idealVelocity) / inertia;
                    }
                }

                // Charge
                else if (NPC.ai[1] == 1f)
                {
                    // Set damage
                    NPC.damage = setDamage;

                    // Play charge sound
                    // TODO - Make this a jet turbine sound or something
                    //SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);

                    // Set rotation and velocity
                    NPC.rotation = hoverRotation;

                    float chargeSpeed = 4.5f + (1.25f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                    chargeSpeed += 4f * enrageScale;
                    if (CalamityWorld.LegendaryMode)
                        chargeSpeed *= 1.2f;

                    NPC.velocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * chargeSpeed;

                    NPC.ai[1] = 2f;
                }

                // Maintain charge velocity until deceleration
                else if (NPC.ai[1] == 2f)
                {
                    // Set damage
                    NPC.damage = setDamage;

                    NPC.ai[2] += 1f;

                    float chargeTime = 60f - (20f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));

                    // Slow down
                    if (NPC.ai[2] >= chargeTime)
                    {
                        // Avoid cheap bullshit
                        NPC.damage = 0;

                        float deceleration = 0.8f - (0.15f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                        NPC.velocity *= deceleration;

                        if (Math.Abs(NPC.velocity.X) < 0.1)
                            NPC.velocity.X = 0f;
                        if (Math.Abs(NPC.velocity.Y) < 0.1)
                            NPC.velocity.Y = 0f;
                    }
                    else
                    {
                        // Accelerative charge
                        float maxVelocity = 27f;
                        maxVelocity += 6f * enrageScale;
                        if (CalamityWorld.LegendaryMode)
                            maxVelocity += 7.5f * ((1f - lifeRatio) / (1f - phase2LifeRatio));

                        // Spawn Energy Bombs along the way
                        float energyBombFireRate = 8f;
                        if (NPC.ai[2] % energyBombFireRate == 0f)
                        {
                            SoundEngine.PlaySound(AstrumDeusHead.LaserSound, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int type = ModContent.ProjectileType<FoveanatorEnergyBomb>();
                                int damage = CalRemixHelper.ProjectileDamage(70, 120);

                                // Reduce mech boss projectile damage depending on the new ore progression changes
                                if (CalamityServerConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive)
                                {
                                    double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                                    double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                                    if (!NPC.downedMechBossAny)
                                        damage = (int)(damage * firstMechMultiplier);
                                    else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                                        damage = (int)(damage * secondMechMultiplier);
                                }

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity.SafeNormalize(Vector2.UnitY) * 60f, Vector2.Zero, type, damage, 0f, Main.myPlayer, 1f);
                            }
                        }

                        if (NPC.velocity.Length() < maxVelocity)
                        {
                            NPC.velocity *= 1.06f;
                            if (NPC.velocity.Length() > maxVelocity)
                            {
                                NPC.velocity.Normalize();
                                NPC.velocity *= maxVelocity;
                            }
                        }

                        NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                    }

                    // Charge 2 times
                    if (NPC.ai[2] >= chargeTime * 1.3f)
                    {
                        NPC.ai[3] += 1f;
                        NPC.ai[2] = 0f;

                        NPC.rotation = hoverRotation;

                        float maxCharges = 2f;
                        if (NPC.ai[3] >= maxCharges)
                        {
                            NPC.ai[1] = 0f;
                            NPC.ai[3] = 0f;
                        }
                        else
                            NPC.ai[1] = 1f;
                    }
                }

                // Laser and energy bomb barrage
                else if (NPC.ai[1] == 3f)
                {
                    float maxVelocity = 12f + (2.4f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                    float acceleration = 0.16f + (0.32f * ((phase2LifeRatio - lifeRatio) / phase2LifeRatio));
                    maxVelocity += 6f * enrageScale;
                    acceleration += 0.08f * enrageScale;

                    if (CalamityWorld.LegendaryMode)
                    {
                        maxVelocity *= 1.15f;
                        acceleration *= 1.15f;
                    }

                    float distanceFromTarget = 640f;
                    Vector2 destination = Main.player[NPC.target].Center + Vector2.UnitX * distanceFromTarget * direction;
                    float distanceFromDestination = (destination - NPC.Center).Length();
                    Vector2 idealVelocity = (destination - NPC.Center).SafeNormalize(Vector2.UnitX * direction);

                    bool gettingIntoPosition = NPC.ai[2] == 0f;
                    if (NPC.Distance(Main.player[NPC.target].Center) < distanceFromTarget && gettingIntoPosition)
                    {
                        NPC.SimpleFlyMovement(idealVelocity * maxVelocity, acceleration);
                    }
                    else
                    {
                        NPC.SimpleFlyMovement(idealVelocity * maxVelocity * 0.5f, acceleration * 0.5f);

                        // Fire 3 spreads of energy bombs and lasers
                        int totalSpreads = 6;

                        // Fire rates
                        float laserBarrageFireRate = 45f;
                        float energyBombFireRate = 90f;

                        NPC.ai[2] += 1f;

                        // Energy bombs
                        if (NPC.ai[2] % energyBombFireRate == 0f)
                        {
                            SoundEngine.PlaySound(AstrumDeusHead.LaserSound, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int type = ModContent.ProjectileType<FoveanatorEnergyBomb>();
                                int damage = CalRemixHelper.ProjectileDamage(70, 120);

                                // Reduce mech boss projectile damage depending on the new ore progression changes
                                if (CalamityServerConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive)
                                {
                                    double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                                    double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                                    if (!NPC.downedMechBossAny)
                                        damage = (int)(damage * firstMechMultiplier);
                                    else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                                        damage = (int)(damage * secondMechMultiplier);
                                }

                                Vector2 projectileVelocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * 16f;
                                int numProj = 3;
                                int spread = 30;
                                float rotation = MathHelper.ToRadians(spread);
                                for (int i = 0; i < numProj; i++)
                                {
                                    Vector2 perturbedSpeed = projectileVelocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (float)(numProj - 1)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + perturbedSpeed.SafeNormalize(Vector2.UnitY) * 60f, perturbedSpeed, type, damage, 0f, Main.myPlayer, 1f);
                                }
                            }

                            NPC.ai[3] += 1f;
                        }

                        else if (NPC.ai[2] % laserBarrageFireRate == 0f)
                        {
                            SoundEngine.PlaySound(AstrumDeusHead.LaserSound, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int type = ModContent.ProjectileType<FoveanatorLaser>();
                                int damage = CalRemixHelper.ProjectileDamage(50, 108);

                                // Reduce mech boss projectile damage depending on the new ore progression changes
                                if (CalamityServerConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive)
                                {
                                    double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                                    double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                                    if (!NPC.downedMechBossAny)
                                        damage = (int)(damage * firstMechMultiplier);
                                    else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                                        damage = (int)(damage * secondMechMultiplier);
                                }

                                float projectileSpeed = 8f + 2f * enrageScale;
                                Vector2 projectileVelocity = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitY) * projectileSpeed;
                                int numProj = 4;
                                int spread = 40;
                                float rotation = MathHelper.ToRadians(spread);
                                for (int i = 0; i < numProj; i++)
                                {
                                    Vector2 perturbedSpeed = projectileVelocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (float)(numProj - 1)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + perturbedSpeed.SafeNormalize(Vector2.UnitY) * 60f, perturbedSpeed, type, damage, 0f, Main.myPlayer);
                                }
                            }

                            NPC.ai[3] += 1f;
                        }

                        // Charge
                        if (NPC.ai[3] >= totalSpreads)
                        {
                            NPC.ai[1] = 1f;
                            NPC.ai[2] = 0f;
                            NPC.ai[3] = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance * bossAdjustment);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(CTPActive() ? TextureCTP.Value : TextureAssets.Npc[Type].Value, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0f);

            Texture2D glowTexture = CTPActive() ? GlowTextureCTP.Value : GlowTexture.Value;
            spriteBatch.Draw(glowTexture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 100D; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f);

                return;
            }

            for (int i = 0; i < 150; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2 * hit.HitDirection, -2f);

            if (!Main.dedServ)
            {
                for (int i = 0; i < 2; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 2, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 9, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Foveanator2").Type, NPC.scale);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust].velocity *= 1.4f;
            }

            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 5f;
                dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust].velocity *= 3f;
            }

            if (!Main.dedServ)
            {
                int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, default, Main.rand.Next(61, 64));
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.X += 1f;
                Main.gore[gore].velocity.Y += 1f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, default, Main.rand.Next(61, 64));
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.X -= 1f;
                Main.gore[gore].velocity.Y += 1f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, default, Main.rand.Next(61, 64));
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.X += 1f;
                Main.gore[gore].velocity.Y -= 1f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, default, Main.rand.Next(61, 64));
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.X -= 1f;
                Main.gore[gore].velocity.Y -= 1f;
            }
        }
    }
}