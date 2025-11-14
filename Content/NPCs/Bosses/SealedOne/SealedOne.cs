using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalRemix.Content.Items.Placeables;
using CalamityMod.Events;
using CalRemix.UI;
using System.Linq;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Core.World;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Lore;
using System.Collections.Generic;
using System;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace CalRemix.Content.NPCs.Bosses.SealedOne
{
    [AutoloadBossHead]
    public class SealedOne : ModNPC
    {
        //public override bool IsLoadingEnabled(Mod mod) => false;

        public ref float AttackType => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float Unsure => ref NPC.ai[2];
        public ref float AttackTotal => ref NPC.ai[3];
        public ref float HasDonePhaseTransition => ref NPC.localAI[0];
        public ref float LengthOfMovement => ref NPC.localAI[1];

        public ref Player Target => ref Main.player[NPC.target];

        public Vector2 PreviousNPCLocation = Vector2.Zero;
        public Vector2 PreviousPlayerLocation = Vector2.Zero;

        public int FrameX = 0;
        public int FrameY = 0;

        public enum AttackTypes
        {
            PhaseTransition = -1,
            None = 0,
            Move = 1,
            MineRing = 2,
            ProjectileVomit = 3,
            LightningOrb = 4,
            Ritual = 5,
            HereticSpears = 6,
            SpinningRingProjectiles = 7,
            AncientDoomEsqueMines = 8
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sealed One");
            if (Main.dedServ)
                return;
            HelperMessage.New("Sealed One",
                "Oh jeez, is that the Sealed One? Who the FUCK is that",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)).SetHoverTextOverride("Indeed Fanny, Indeed.");
        }

        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 164;
            NPC.aiStyle = -1;
            NPC.damage = 50;
            NPC.defense = 42;
            NPC.lifeMax = 32000;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.npcSlots = 10f;
            NPC.HitSound = SoundID.NPCHit55;
            NPC.DeathSound = SoundID.NPCDeath59;
            NPC.value = 100000f;
            NPC.boss = true;
            NPC.netAlways = true;
            NPC.SpawnWithHigherTime(30);
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Zombie89, NPC.position);
            NPC.rotation = 0f;
            if (!Main.dedServ)
            {
                NPC.netUpdate = true;
            }
        }

        public override void AI()
        {
            Main.NewText("Player prev pos " + PreviousPlayerLocation);
            Main.NewText("NPC prev pos - " + PreviousNPCLocation);
            Main.NewText("AI 0 - " + AttackType);
            Main.NewText("AI 1 - " + Timer);
            Main.NewText("AI 2 - " + NPC.ai[2]);
            Main.NewText("AI 3 - " + AttackTotal);
            Main.NewText("Local AI 0 - " + HasDonePhaseTransition);
            Main.NewText("Local AI 1 - " + NPC.localAI[1]);
            Main.NewText("Local AI 2 - " + NPC.localAI[2]);
            Main.NewText("Local AI 3 - " + NPC.localAI[3]);

            List<SoundStyle> lunaticCultistSounds = [SoundID.Zombie88, SoundID.Zombie89, SoundID.Zombie90, SoundID.Zombie91];

            if (AttackType != (float)AttackTypes.PhaseTransition && Main.rand.NextBool(1000))
            {
                SoundEngine.PlaySound(lunaticCultistSounds[Main.rand.Next(0, lunaticCultistSounds.Count)], NPC.position);
            }

            #region Values
            bool bossRush = BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || bossRush;
            bool revengeance = CalamityWorld.revenge || bossRush;
            bool death = CalamityWorld.death || bossRush;
            bool isPhase2 = NPC.life <= NPC.lifeMax * 0.75;
            bool isPhase3 = NPC.life <= NPC.lifeMax * 0.50;
            bool isPhase4 = NPC.life <= NPC.lifeMax * 0.30;
            bool isPhase5 = NPC.life <= NPC.lifeMax * 0.10;

            float intendedVelocity = 5f;
            float intendedAcceleration = 0.2f;

            // low fire rate = faster fire
            int mineRingDamage = NPC.GetAttackDamage_ForProjectiles(35f, 25f);
            int mineRingFireRate = 6;
            int mineRingTotalProjectiles = 8;

            int projVomitDamage = NPC.GetAttackDamage_ForProjectiles(30f, 20f);
            int projVomitFireRate = 6;
            int projVomitAmount = 8;

            int lightningOrbDamage = NPC.GetAttackDamage_ForProjectiles(45f, 30f);
            int lightningOrbFireRate = 6;
            float lightningOrbSpeed = 4;

            int spinningProjDamage = NPC.GetAttackDamage_ForProjectiles(45f, 30f);
            int spinningProjDuration = 280;
            int spinningProjSpawnInterval = 20;
            int spinningProjStopSpawningProjectilesInterval = spinningProjDuration - 140;
            int spinningProjTotalProjectiles = 3;

            int ritualTotalProjectiless = 12;

            int ancientDoomFireRate = 30;
            int ancientDoomTotalProjectiles = 6;

            int phase1SpeedReductionMultiplier = 2;

            bool shouldNotTakeDamage = false;
            bool shouldNotBeChased = false;
            #endregion

            #region Difficulty Changes
            if (expertMode)
            {
                mineRingFireRate -= 2;

                projVomitFireRate -= 2;
                projVomitAmount += 4;

                spinningProjTotalProjectiles += 2;
            }
            if (revengeance)
            {

            }
            if (death)
            {

            }
            #endregion

            #region Phase Changes
            if (isPhase2)
            {
                // this is gonna be kept. dont remove this
                NPC.defense = (int)((float)NPC.defDefense * 0.65f);

                phase1SpeedReductionMultiplier = 1;

                intendedVelocity += 15;
                intendedAcceleration += 0.15f;
            }
            if (isPhase3)
            {

            }
            if (isPhase4)
            {
                spinningProjTotalProjectiles += 2;
            }
            if (isPhase5)
            {

            }
            #endregion

            #region Target Player, Despawn
            Vector2 npcCenter = NPC.Center;
            Player player = Main.player[NPC.target];
            float distanceToDespawn = 5600f;
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active || Vector2.Distance(player.Center, npcCenter) > distanceToDespawn)
            {
                NPC.TargetClosest(faceTarget: false);
                player = Main.player[NPC.target];
                NPC.netUpdate = true;
            }

            if (player.dead || !player.active || Vector2.Distance(player.Center, npcCenter) > distanceToDespawn)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f);
                }
                return;
            }
            #endregion

            #region Music
            if (!Main.dedServ)
            {
                if (!isPhase2)
                {
                    Music = CalRemixMusic.SealedOnePhase1;
                }
                else
                {
                    // note: handling the music instantly starting is done in the phase transition attack
                    Music = CalRemixMusic.SealedOnePhase2;
                }
            }
            #endregion

            #region Phase Transition Handling
            if (HasDonePhaseTransition == 0 && isPhase2)
            {
                HasDonePhaseTransition = 1f;

                AttackType = (float)AttackTypes.PhaseTransition;
                Timer = 0f;
                NPC.velocity = Vector2.Zero;
                if (!Main.dedServ)
                {
                    NPC.netUpdate = true;
                }

            }
            #endregion

            #region Attacks
            if (AttackType == (float)AttackTypes.PhaseTransition)
            {
                if (Timer > 100 && Timer < 102)
                {
                    SoundEngine.PlaySound(SoundID.Zombie92, NPC.position);
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    NPC.netUpdate = true;
                }
                else if (Timer > 1f)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - (NPC.height / 2)), 0, 0, DustID.Blood, Main.rand.NextFloat(-0.2f, 0.2f), -10f, 0, default, 1f);
                    }
                }
                else
                {
                    for (int i = 0; i < 40; i++)
                    {
                        Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - (NPC.height / 2)), 0, 0, DustID.Blood, Main.rand.NextFloat(-2f, 2f), -10f, 0, default, 2f);
                    }
                    SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.position);

                    Music = CalRemixMusic.SealedOnePhase2;
                    Main.newMusic = CalRemixMusic.SealedOnePhase2;
                    try
                    {
                        Main.musicFade[Main.curMusic] = 0f;
                        Main.musicFade[Main.newMusic] = 1f;
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                }
                Timer += 1f;

                shouldNotTakeDamage = true;
                shouldNotBeChased = true;
            }
            if (AttackType == (float)AttackTypes.None)
            {
                if (Timer == 0f)
                {
                    NPC.TargetClosest(faceTarget: false);
                }

                NPC.localAI[2] = 10f;

                // face left or right towards player
                int leftOrRight = Math.Sign(player.Center.X - npcCenter.X);
                if (leftOrRight != 0)
                {
                    NPC.direction = NPC.spriteDirection = leftOrRight;
                }

                Timer += 1f;
                if (Timer >= 40f)
                {
                    int attackToUse = (int)AttackTypes.None;
                    #region Attack Patterns
                    if (isPhase2)
                    {
                        switch ((int)AttackTotal)
                        {
                            case 0:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 1:
                                attackToUse = (int)AttackTypes.ProjectileVomit;
                                break;
                            case 2:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 3:
                                attackToUse = (int)AttackTypes.SpinningRingProjectiles;
                                break;
                            case 4:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 5:
                                attackToUse = (int)AttackTypes.LightningOrb;
                                break;
                            case 6:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 7:
                                attackToUse = (int)AttackTypes.HereticSpears;
                                break;
                            case 8:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 9:
                                attackToUse = (int)AttackTypes.MineRing;
                                break;
                            case 10:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 11:
                                attackToUse = (int)AttackTypes.LightningOrb;
                                break;
                            case 12:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 13:
                                attackToUse = (int)AttackTypes.Ritual;
                                AttackTotal = -1f;
                                break;
                            default:
                                AttackTotal = -1f;
                                break;
                        }
                    }
                    else
                    {
                        switch (AttackTotal)
                        {
                            case 0:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 1:
                                attackToUse = (int)AttackTypes.ProjectileVomit;
                                break;
                            case 2:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 3:
                                attackToUse = (int)AttackTypes.MineRing;
                                break;
                            case 4:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 5:
                                attackToUse = (int)AttackTypes.LightningOrb;
                                break;
                            case 6:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 7:
                                attackToUse = (int)AttackTypes.ProjectileVomit;
                                break;
                            case 8:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 9:
                                attackToUse = (int)AttackTypes.MineRing;
                                break;
                            case 10:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 11:
                                attackToUse = (int)AttackTypes.Ritual;
                                AttackTotal = -1f;
                                break;
                            default:
                                AttackTotal = -1f;
                                break;
                        }
                    }
                    #endregion

                    /*
                    #region Ancient Doom Attack Replacement
                    // gradually increase chance of replacing an attack with ancient doom
                    int attackReplacementChance = 6;
                    if (NPC.life < NPC.lifeMax / 3)
                    {
                        attackReplacementChance = 4;
                    }
                    if (NPC.life < NPC.lifeMax / 4)
                    {
                        attackReplacementChance = 3;
                    }
                    // replace attack w ancient doom
                    if (expertMode && isPhase2 && Main.rand.NextBool(attackReplacementChance) && attackToUse != (int)AttackTypes.None && attackToUse != (int)AttackTypes.Ritual && attackToUse != (int)AttackTypes.SpinningRingProjectiles && NPC.CountNPCS(523) < 10)
                    {
                        //attackToUse = (int)AttackTypes.AncientDoomEsqueMines;
                    }
                    #endregion
                    */

                    // get locations to lerp from and to, and set timer
                    if (attackToUse == (int)AttackTypes.None)
                    {
                        LengthOfMovement = (float)Math.Ceiling((player.Center + new Vector2(0f, -100f) - npcCenter).Length() / 50f);
                        if (LengthOfMovement == 0f)
                        {
                            LengthOfMovement = 1f;
                        }

                        PreviousNPCLocation = NPC.position;
                        PreviousPlayerLocation = player.position;
                        Timer = 30;
                        AttackType = (float)AttackTypes.Move;
                    }

                    // final stuff: convert attacktouse to an attack to be used
                    if (attackToUse != (int)AttackTypes.PhaseTransition && attackToUse != (int)AttackTypes.None  && attackToUse != (int)AttackTypes.Move)
                    {
                        //attackToUse = (int)AttackTypes.SpinningRingProjectiles;
                        AttackType = attackToUse;
                        Timer = 0f;
                    }
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.Move)
            {
                shouldNotTakeDamage = false;
                shouldNotBeChased = false;
                NPC.localAI[2] = 10f;

                Vector2 distanceFromDestination = PreviousPlayerLocation - NPC.Center;
                int maxDistance = 40;

                // Movement
                if (NPC.Distance(PreviousPlayerLocation) > maxDistance)
                {
                    CalamityUtils.SmoothMovement(Main.npc[NPC.whoAmI], 0f, distanceFromDestination, intendedVelocity, intendedAcceleration, true);
                    
                    // reset timer if it gets too low 
                    if (Timer < 10)
                    {
                        Timer += 20;
                    }
                }
                else
                {
                    // Slow down
                    if (NPC.velocity.Length() > 0.5f)
                        NPC.velocity *= 0.8f;
                    else
                        NPC.velocity = Vector2.Zero;
                }

                // in p3 leave a trail of mines
                if (isPhase3 && (int)Timer % 4 == 0)
                {
                    int mineDamage = 120;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)), ModContent.ProjectileType<SirenSong>(), mineDamage, 0f, Main.myPlayer);
                }

                Timer -= 1f;
                if (Timer <= 0f)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.MineRing)
            {
                NPC.localAI[2] = 11f;

                // spawn ring of evil mines
                if (Timer == 3f)
                {
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    float radians = MathHelper.TwoPi / mineRingTotalProjectiles;
                    float projectileVelocity = 5;
                    float projectileDistance = 400;
                    int mineDamage = 120;
                    for (int i = 0; i < mineRingTotalProjectiles; i++)
                    {
                        Vector2 spawnVector = player.Center + Vector2.Normalize(new Vector2(0f, -projectileVelocity).RotatedBy(radians * i)) * projectileDistance;
                        Vector2 projVelocity = Vector2.Normalize(player.Center - spawnVector) * projectileVelocity * Main.rand.NextFloat(-1, 1);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnVector, projVelocity, ModContent.ProjectileType<SirenSong>(), mineDamage, 0f, Main.myPlayer);
                    }
                }

                Timer += 1f;
                if (Timer >= (4 + mineRingFireRate) * phase1SpeedReductionMultiplier)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.ProjectileVomit)
            {
                float endAttackTime = 4 + projVomitFireRate * projVomitAmount;

                if (Timer == 0f)
                {
                    PreviousPlayerLocation = player.Center;
                }

                NPC.localAI[2] = 11f;
                Vector2 distanceToPlayer = Vector2.Normalize(player.Center - npcCenter);
                if (distanceToPlayer.HasNaNs())
                {
                    distanceToPlayer = new Vector2(NPC.direction, 0f);
                }

                // fire projs at the location of starting attack
                if (Timer >= 4f && (int)(Timer - 4f) % projVomitFireRate == 0 && Timer < endAttackTime)
                {
                    // face left or right
                    int leftOrRight = Math.Sign(player.Center.X - npcCenter.X);
                    if (leftOrRight != 0)
                    {
                        NPC.direction = (NPC.spriteDirection = leftOrRight);
                    }

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        distanceToPlayer = Vector2.Normalize(PreviousPlayerLocation - npcCenter + player.velocity * 20f);
                        if (distanceToPlayer.HasNaNs())
                        {
                            distanceToPlayer = new Vector2(NPC.direction, 0f);
                        }
                        Vector2 fireballSpawn = NPC.Center + new Vector2(NPC.direction * 30, 12f);
                        for (int num15 = 0; num15 < 1; num15++)
                        {
                            Vector2 fireballVelocity = distanceToPlayer * (6f + (float)Main.rand.NextDouble() * 4f);
                            fireballVelocity = fireballVelocity.RotatedByRandom(0.52359879016876221);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), fireballSpawn.X, fireballSpawn.Y, fireballVelocity.X, fireballVelocity.Y, ProjectileID.CultistBossFireBall, projVomitDamage, 0f, Main.myPlayer);
                        }
                    }
                }

                Timer += 1f;
                if (Timer >= endAttackTime * phase1SpeedReductionMultiplier)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.LightningOrb)
            {
                NPC.localAI[2] = 12f;

                // shoot the cultist lighting balls
                if (Timer == 20f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if ((int)(Timer - 20f) % lightningOrbFireRate == 0)
                    {
                        
                        int lightningOrbHigh = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y - 100f, 0, 0, ProjectileID.CultistBossLightningOrb, lightningOrbDamage, 0f, Main.myPlayer);
                        Main.projectile[lightningOrbHigh].velocity = Main.projectile[lightningOrbHigh].DirectionTo(player.Center) * lightningOrbSpeed;
                        if (isPhase2)
                        {
                            int lightningOrbLow = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y + 100f, NPC.DirectionTo(player.Center).X * lightningOrbSpeed, NPC.DirectionTo(player.Center).Y * lightningOrbSpeed, ProjectileID.CultistBossLightningOrb, lightningOrbDamage, 0f, Main.myPlayer);
                            Main.projectile[lightningOrbLow].velocity = Main.projectile[lightningOrbLow].DirectionTo(player.Center) * lightningOrbSpeed;
                        }
                    }
                }
                Timer += 1f;
                if (Timer >= (20 + lightningOrbFireRate))
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.Ritual)
            {
                NPC.localAI[2] = 10f;

                if (Timer % 24 == 0) 
                {
                    int projNoise = Main.rand.Next(-1, 7);
                    float rotationNose = Main.rand.NextFloat(0, 1);
                    int totalProjsAdjusted = ritualTotalProjectiless + projNoise;
                    for (int i = 0; i < totalProjsAdjusted; i++)
                    {
                        float radians = MathHelper.TwoPi / totalProjsAdjusted;
                        int mineDamage = 120;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.RotatedBy((radians * i) + rotationNose), ProjectileID.MartianTurretBolt, mineDamage, 0f, Main.myPlayer);
                    }
                }

                if (Timer % 32 == 0)
                {
                    for (int i = 0; i < ritualTotalProjectiless - (int)(ritualTotalProjectiless / 2); i++)
                    {
                        float radians = MathHelper.TwoPi / (int)(ritualTotalProjectiless / 2);
                        int mineDamage = 120;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.RotatedBy(radians * i), ProjectileID.PhantasmalBolt, mineDamage, 0f, Main.myPlayer);
                    }
                }

                if (Timer % 50 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float radians = MathHelper.TwoPi / 3;
                        int mineDamage = 120;
                        Vector2 newSpawn = NPC.Center + Vector2.Normalize(NPC.Center.RotatedBy(radians * i).RotatedBy(radians * i)) * 80;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), newSpawn, NPC.Center.RotatedBy(radians * i), ProjectileID.StardustJellyfishSmall, mineDamage, 0f, Main.myPlayer);
                    }
                }

                Timer += 1f;
                if (Timer >= 420f)
                {
                    shouldNotBeChased = true;
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.HereticSpears)
            {
                float endAttackTime = 4 + projVomitFireRate * projVomitAmount;

                NPC.localAI[2] = 13f;

                // if starting or close to target, make up a new target
                // having the failsafe be triggered by vector2.zero means he cant go to the top left of the world
                // zzzzzzzzzzz
                int flip = Main.rand.Next(0, 2);
                if (flip == 0)
                    flip = -1;

                int boundingBoxHeight = 800;
                int boundingBoxVerticalOffset = -boundingBoxHeight / 2;
                int boundingBoxWidth = 400;
                int boundingBoxHorizontalOffset = 300 * flip;
                int boundingBoxHalfWidth = boundingBoxWidth / 2;
                // making a "bounding box" of areas he can choose to go to
                float lowerBound = Target.Center.Y - boundingBoxVerticalOffset;
                float upperBound = Target.Center.Y - boundingBoxHeight - boundingBoxVerticalOffset;
                float leftBound = Target.Center.X - boundingBoxHalfWidth + boundingBoxHorizontalOffset;
                float rightBound = Target.Center.X + boundingBoxHalfWidth + boundingBoxHorizontalOffset;

                /*
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound, rightBound), Main.rand.NextFloat(lowerBound, upperBound)), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound, rightBound), lowerBound), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound, rightBound), upperBound), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(leftBound, Main.rand.NextFloat(lowerBound, upperBound)), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(rightBound, Main.rand.NextFloat(lowerBound, upperBound)), DustID.BlueFairy, Vector2.Zero);
                }
                */

                // fire projs from sides of screen
                if (Timer % 12 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 spearSpawnSpot = new Vector2(Main.rand.NextFloat(leftBound, rightBound), Main.rand.NextFloat(lowerBound, upperBound));
                    // play a sound
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spearSpawnSpot.X, spearSpawnSpot.Y, 0, 0, ModContent.ProjectileType<HereticSpear>(), projVomitDamage, 0f, Main.myPlayer, 0, flip, 1);
                }

                // face left or right
                int leftOrRight = Math.Sign(player.Center.X - npcCenter.X);
                if (leftOrRight != 0)
                {
                    NPC.direction = (NPC.spriteDirection = leftOrRight);
                }

                Timer += 1f;

                if (Timer >= 300f)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.SpinningRingProjectiles)
            {
                NPC.localAI[2] = 11f;

                if (Timer % spinningProjSpawnInterval == 0 && Timer <= spinningProjStopSpawningProjectilesInterval)
                {
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingOrb>(), spinningProjDamage, 0f, Main.myPlayer);
                }

                Timer += 1f;
                if (Timer >= (float)spinningProjDuration)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.AncientDoomEsqueMines)
            {
                NPC.localAI[2] = 13f;
                if (Timer >= 4f && (int)(Timer - 4f) % ancientDoomFireRate == 0)
                {
                    int num48 = Math.Sign(player.Center.X - npcCenter.X);
                    if (num48 != 0)
                    {
                        NPC.direction = (NPC.spriteDirection = num48);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int num49 = 0; num49 < 5; num49++)
                        {
                            Point tileNPCCenter = NPC.Center.ToTileCoordinates();
                            Point tileTargetCenter = Main.player[NPC.target].Center.ToTileCoordinates();
                            Vector2 distanceBetweenPlayer = Main.player[NPC.target].Center - NPC.Center;
                            int num50 = 20;
                            int num51 = 3;
                            int num52 = 7;
                            int num53 = 2;
                            int num54 = 0;
                            bool flag6 = false;
                            if (distanceBetweenPlayer.Length() > 100000f)
                            {
                                flag6 = true;
                            }
                            while (!flag6 && num54 < 100)
                            {
                                num54++;
                                int ancientDoomX = Main.rand.Next(tileTargetCenter.X - num50, tileTargetCenter.X + num50 + 1);
                                int ancientDoomY = Main.rand.Next(tileTargetCenter.Y - num50, tileTargetCenter.Y + num50 + 1);
                                if ((ancientDoomY < tileTargetCenter.Y - num52 || ancientDoomY > tileTargetCenter.Y + num52 || ancientDoomX < tileTargetCenter.X - num52 || ancientDoomX > tileTargetCenter.X + num52) && (ancientDoomY < tileNPCCenter.Y - num51 || ancientDoomY > tileNPCCenter.Y + num51 || ancientDoomX < tileNPCCenter.X - num51 || ancientDoomX > tileNPCCenter.X + num51) && !Main.tile[ancientDoomX, ancientDoomY].HasUnactuatedTile)
                                {
                                    bool DoesNotHaveTiles = true;
                                    if (DoesNotHaveTiles && Collision.SolidTiles(ancientDoomX - num53, ancientDoomX + num53, ancientDoomY - num53, ancientDoomY + num53))
                                    {
                                        DoesNotHaveTiles = false;
                                    }
                                    if (DoesNotHaveTiles)
                                    {
                                        CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), ancientDoomX * 16 + 8, ancientDoomY * 16 + 8, NPCID.AncientLight, 0, NPC.whoAmI);
                                        flag6 = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                Timer += 1f;
                if (Timer >= (float)(4 + ancientDoomFireRate * ancientDoomTotalProjectiles))
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            
            // projectile anti-rain
            if (revengeance && isPhase4)
            {
                int boundingBoxHeight = 400;
                int boundingBoxVerticalOffset = (-boundingBoxHeight / 2) - 800;
                int boundingBoxWidth = 1800;
                int boundingBoxHorizontalOffset = 0;
                int boundingBoxHalfWidth = boundingBoxWidth / 2;
                // making a "bounding box" of areas he can choose to go to
                float lowerBound = Target.Center.Y - boundingBoxVerticalOffset;
                float upperBound = Target.Center.Y - boundingBoxHeight - boundingBoxVerticalOffset;
                float leftBound = Target.Center.X - boundingBoxHalfWidth + boundingBoxHorizontalOffset;
                float rightBound = Target.Center.X + boundingBoxHalfWidth + boundingBoxHorizontalOffset;

                /*
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound, rightBound), Main.rand.NextFloat(lowerBound, upperBound)), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound, rightBound), lowerBound), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(leftBound, rightBound), upperBound), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(leftBound, Main.rand.NextFloat(lowerBound, upperBound)), DustID.BlueFairy, Vector2.Zero);
                    Dust.NewDustPerfect(new Vector2(rightBound, Main.rand.NextFloat(lowerBound, upperBound)), DustID.BlueFairy, Vector2.Zero);
                }
                */

                if (Main.rand.NextBool(80))
                {
                    Vector2 flameSpawnSpot = new Vector2(Main.rand.NextFloat(leftBound, rightBound), Main.rand.NextFloat(lowerBound, upperBound));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), flameSpawnSpot.X, flameSpawnSpot.Y, 0, 0, ModContent.ProjectileType<ChaoticFlame>(), projVomitDamage, 0f, Main.myPlayer);
                }
            }
            #endregion

            #region Frame Stuff
            NPC.frame = new Rectangle(0, 0, TextureAssets.Npc[NPC.type].Value.Width / 4, TextureAssets.Npc[NPC.type].Value.Height / 4);

            // vertical stuff
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                FrameY++;
                NPC.frameCounter = 0;
                if (FrameY >= 4)
                {
                    FrameY = 0;
                }
            }

            // horizontal stuff
            if (isPhase2)
            {
                if (AttackType == (float)AttackTypes.Move || AttackType == (float)AttackTypes.None || AttackType == (float)AttackTypes.PhaseTransition)
                {
                    FrameX = 2;
                }
                else
                {
                    FrameX = 3;
                }
            }
            else
            {
                if (AttackType == (float)AttackTypes.Move || AttackType == (float)AttackTypes.None)
                {
                    FrameX = 0;
                }
                else
                {
                    FrameX = 1;
                }
            }

            #endregion

            NPC.dontTakeDamage = shouldNotTakeDamage;
            NPC.chaseable = !shouldNotBeChased;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D spritesheet = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/SealedOne/SealedOne").Value;
            Main.EntitySpriteDraw(spritesheet, NPC.Center - Main.screenPosition, spritesheet.Frame(4, 4, FrameX, FrameY), drawColor, 0, new Vector2(spritesheet.Size().X / 8, spritesheet.Size().Y / 8), 1, NPC.direction != -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.DungeonDesertKey, 3);
            npcLoot.Add(ModContent.ItemType<CarcinogenTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<CarcinogenBag>());
            npcLoot.AddNormalOnly(ModContent.ItemType<Asbestos>(), 1, 216, 224);
            npcLoot.AddNormalOnly(ModContent.ItemType<FiberBaby>());
            npcLoot.AddNormalOnly(ModContent.ItemType<Chainsmoker>());
            npcLoot.AddNormalOnly(ModContent.ItemType<SoulofCarcinogen>());
            npcLoot.AddNormalOnly(ModContent.ItemType<CarcinogenMask>(), 7);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<CarcinogenRelic>());
            npcLoot.AddConditionalPerPlayer(() => !RemixDowned.downedSealedOne, ModContent.ItemType<KnowledgeCarcinogen>(), desc: DropHelper.FirstKillText);
        }
        public override void OnKill()
        {
            RemixDowned.downedSealedOne = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool CheckActive()
        {
            return Target == null || Target.dead || !Target.active;
        }
    }
}
