using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Items.Placeables;
using CalamityMod.Events;
using CalRemix.UI;
using System.Linq;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.NPCs.TownNPCs;
using CalRemix.Core.World;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Lore;
using System.Collections.Generic;
using System;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalamityMod.Projectiles.Boss;
using System.Collections;

namespace CalRemix.Content.NPCs.Bosses.SealedOne
{
    [AutoloadBossHead]
    public class SealedOne : ModNPC
    {
        public ref float AttackType => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];
        public ref float Unsure => ref NPC.ai[2];
        public ref float AttackTotal => ref NPC.ai[3];
        public ref float HasDoneSpawnAnim => ref NPC.localAI[0];
        public ref float LengthOfMovement => ref NPC.localAI[1];

        public ref Player Target => ref Main.player[NPC.target];

        public Vector2 PreviousNPCLocation = Vector2.Zero;
        public Vector2 PreviousPlayerLocation = Vector2.Zero;

        public enum AttackTypes
        {
            Spawn = -1,
            None = 0,
            Move = 1,
            IceMist = 2,
            Fireball = 3,
            LightningOrb = 4,
            Ritual = 5,
            Something = 6,
            StardustOrb = 7,
            AncientDoom = 8
        }

        public static readonly SoundStyle HitSound = new("CalRemix/Assets/Sounds/GenBosses/CarcinogenHit", 3);
        public static readonly SoundStyle DeathSound = new("CalRemix/Assets/Sounds/GenBosses/CarcinogenDeath");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sealed One");
            if (Main.dedServ)
                return;
            HelperMessage.New("Sealed One",
                "Oh jeez, is that the Sealed One? Who the FUCK is that",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)).SetHoverTextOverride("Indeed Fanny, Indeed.");
        }

        public override void SetDefaults()
        {
            NPC.width = 121;
            NPC.height = 177;
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
            
        }

        public override void AI()
        {
            Main.NewText("Position - " + NPC.position);
            Main.NewText("Velocity - " + NPC.velocity);
            Main.NewText("AI 0 - " + AttackType);
            Main.NewText("AI 1 - " + Timer);
            Main.NewText("AI 2 - " + NPC.ai[2]);
            Main.NewText("AI 3 - " + AttackTotal);
            Main.NewText("Local AI 0 - " + NPC.localAI[0]);
            Main.NewText("Local AI 1 - " + NPC.localAI[1]);
            Main.NewText("Local AI 2 - " + NPC.localAI[2]);
            Main.NewText("Local AI 3 - " + NPC.localAI[3]);

            List<SoundStyle> lunaticCultistSounds = [SoundID.Zombie88, SoundID.Zombie89, SoundID.Zombie90, SoundID.Zombie91];

            if (AttackType != (float)AttackTypes.Spawn && Main.rand.NextBool(1000))
            {
                SoundEngine.PlaySound(lunaticCultistSounds[Main.rand.Next(0, lunaticCultistSounds.Count + 1)], NPC.position);
            }

            bool expertMode = Main.expertMode;
            bool isPhase2 = NPC.life <= NPC.lifeMax * 0.75;
            bool isPhase3 = NPC.life <= NPC.lifeMax * 0.50;
            bool isPhase4 = NPC.life <= NPC.lifeMax * 0.30;
            bool isPhase5 = NPC.life <= NPC.lifeMax * 0.10;

            // low fire rate = faster fire
            int iceMistDamage = NPC.GetAttackDamage_ForProjectiles(35f, 25f);
            int iceMistFireRate = 6;
            int fireballDamage = NPC.GetAttackDamage_ForProjectiles(30f, 20f);
            int fireballFireRate = 6;
            int fireballAmount = 5;
            int lightningOrbDamage = NPC.GetAttackDamage_ForProjectiles(45f, 30f);
            int stardustOrbFireRate = 6;
            int stardustOrbAmount = 5;
            int ancientDoomFireRate = 6;
            int ancientDoomAmount = 6;

            int phase1SpeedReductionMultiplier = 2;
            int phase1SpeedReductionFlat = 2;
            float movementLengthMultiplier = 3;

            int totalProjectiles = 8;

            if (expertMode)
            {
                iceMistFireRate = 4;
                fireballFireRate = 4;
                fireballAmount = 12;
                stardustOrbFireRate = 12;
                stardustOrbAmount = 4;
                ancientDoomFireRate = 12;
                ancientDoomAmount = 5;
            }

            bool amISealedOne = NPC.type == ModContent.NPCType<SealedOne>();
            bool shouldTakeDamage = false;
            bool shouldNotBeChased = false;

            if (isPhase2)
            {
                // this is gonna be kept. dont remove this
                NPC.defense = (int)((float)NPC.defDefense * 0.65f);

                phase1SpeedReductionMultiplier = 1;
                phase1SpeedReductionFlat = 0;

                movementLengthMultiplier = 2.75f;
            }

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
            }
            #endregion

            if (NPC.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Zombie89, NPC.position);
                NPC.localAI[0] = 1f;
                NPC.alpha = 255;
                NPC.rotation = 0f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    AttackType = (float)AttackTypes.Spawn;
                    NPC.netUpdate = true;
                }
            }

            #region Attacks
            if (AttackType == (float)AttackTypes.Spawn)
            {
                NPC.alpha -= 5;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                Timer += 1f;
                if (Timer >= 420f)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    NPC.netUpdate = true;
                }
                else if (Timer > 360f)
                {
                    NPC.velocity *= 0.95f;
                    if (NPC.localAI[2] != 13f)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie105, NPC.position);
                    }
                    NPC.localAI[2] = 13f;
                }
                else if (Timer > 300f)
                {
                    NPC.velocity = -Vector2.UnitY;
                    NPC.localAI[2] = 10f;
                }
                else if (Timer > 120f)
                {
                    NPC.localAI[2] = 1f;
                }
                else
                {
                    NPC.localAI[2] = 0f;
                }
                shouldTakeDamage = true;
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
                    NPC.direction = (NPC.spriteDirection = leftOrRight);
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
                                attackToUse = (int)AttackTypes.Fireball;
                                break;
                            case 2:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 3:
                                attackToUse = (int)AttackTypes.StardustOrb;
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
                                attackToUse = (int)AttackTypes.StardustOrb;
                                break;
                            case 8:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 9:
                                attackToUse = (int)AttackTypes.IceMist;
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
                                attackToUse = (int)AttackTypes.Fireball;
                                break;
                            case 2:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 3:
                                attackToUse = (int)AttackTypes.IceMist;
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
                                attackToUse = (int)AttackTypes.Fireball;
                                break;
                            case 8:
                                attackToUse = (int)AttackTypes.None;
                                break;
                            case 9:
                                attackToUse = (int)AttackTypes.IceMist;
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
                    if (expertMode && isPhase2 && Main.rand.NextBool(attackReplacementChance) && attackToUse != (int)AttackTypes.None && attackToUse != (int)AttackTypes.Ritual && attackToUse != (int)AttackTypes.StardustOrb && NPC.CountNPCS(523) < 10)
                    {
                        attackToUse = (int)AttackTypes.AncientDoom;
                    }
                    #endregion

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
                        Timer = LengthOfMovement * movementLengthMultiplier * phase1SpeedReductionMultiplier;
                        AttackType = (float)AttackTypes.Move;
                    }

                    // final stuff: convert attacktouse to an attack to be used
                    if (attackToUse != (int)AttackTypes.Spawn && attackToUse != (int)AttackTypes.None  && attackToUse != (int)AttackTypes.Move && attackToUse != (int)AttackTypes.Something)
                    {
                        AttackType = attackToUse;
                        Timer = 0f;
                    }
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.Move)
            {
                shouldTakeDamage = true;
                NPC.localAI[2] = 10f;

                NPC.position.X = MathHelper.Lerp(PreviousPlayerLocation.X, PreviousNPCLocation.X, Timer / (NPC.localAI[1] * movementLengthMultiplier * phase1SpeedReductionMultiplier));
                NPC.position.Y = MathHelper.Lerp(PreviousPlayerLocation.Y, PreviousNPCLocation.Y, Timer / (NPC.localAI[1] * movementLengthMultiplier * phase1SpeedReductionMultiplier));

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
            else if (AttackType == (float)AttackTypes.IceMist)
            {
                NPC.localAI[2] = 11f;

                // spawn ring of evil mines
                if (Timer == 3f)
                {
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    float radians = MathHelper.TwoPi / totalProjectiles;
                    float projectileVelocity = 5;
                    float projectileDistance = 400;
                    int mineDamage = 120;
                    for (int i = 0; i < totalProjectiles; i++)
                    {
                        Vector2 spawnVector = player.Center + Vector2.Normalize(new Vector2(0f, -projectileVelocity).RotatedBy(radians * i)) * projectileDistance;
                        Vector2 projVelocity = Vector2.Normalize(player.Center - spawnVector) * projectileVelocity * Main.rand.NextFloat(-1, 1);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnVector, projVelocity, ModContent.ProjectileType<SirenSong>(), mineDamage, 0f, Main.myPlayer);
                    }
                }

                Timer += 1f;
                if (Timer >= (4 + iceMistFireRate) * phase1SpeedReductionMultiplier)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.Fireball)
            {
                float endAttackTime = 4 + fireballFireRate * fireballAmount;


                NPC.localAI[2] = 11f;
                Vector2 vec2 = Vector2.Normalize(player.Center - npcCenter);
                if (vec2.HasNaNs())
                {
                    vec2 = new Vector2(NPC.direction, 0f);
                }
                if (Timer >= 4f && amISealedOne && (int)(Timer - 4f) % fireballFireRate == 0 && Timer < endAttackTime)
                {
                    int num14 = Math.Sign(player.Center.X - npcCenter.X);
                    if (num14 != 0)
                    {
                        NPC.direction = (NPC.spriteDirection = num14);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        vec2 = Vector2.Normalize(player.Center - npcCenter + player.velocity * 20f);
                        if (vec2.HasNaNs())
                        {
                            vec2 = new Vector2(NPC.direction, 0f);
                        }
                        Vector2 fireballSpawn = NPC.Center + new Vector2(NPC.direction * 30, 12f);
                        for (int num15 = 0; num15 < 1; num15++)
                        {
                            Vector2 fireballVelocity = vec2 * (6f + (float)Main.rand.NextDouble() * 4f);
                            fireballVelocity = fireballVelocity.RotatedByRandom(0.52359879016876221);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), fireballSpawn.X, fireballSpawn.Y, fireballVelocity.X, fireballVelocity.Y, ProjectileID.CultistBossFireBall, fireballDamage, 0f, Main.myPlayer);
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
                if (amISealedOne)
                {
                    NPC.localAI[2] = 12f;
                }
                else
                {
                    NPC.localAI[2] = 11f;
                }
                if (Timer == 20f && amISealedOne && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if ((int)(Timer - 20f) % iceMistFireRate == 0)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y - 100f, 0f, 0f, ProjectileID.CultistBossLightningOrb, lightningOrbDamage, 0f, Main.myPlayer);
                    }
                }
                Timer += 1f;
                if (Timer >= (20 + iceMistFireRate))
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
                if (Vector2.Normalize(player.Center - npcCenter).HasNaNs())
                {
                    new Vector2(NPC.direction, 0f);
                }
                if (Timer >= 0f && Timer < 30f)
                {
                    shouldTakeDamage = true;
                    shouldNotBeChased = true;
                    float num19 = (Timer - 0f) / 30f;
                    NPC.alpha = (int)(num19 * 255f);
                }
                else if (Timer >= 30f && Timer < 90f)
                {
                    if (Timer == 30f && Main.netMode != NetmodeID.MultiplayerClient && amISealedOne)
                    {
                        // this is where the ritual attack was
                    }
                    shouldTakeDamage = true;
                    shouldNotBeChased = true;
                    NPC.alpha = 255;
                    if (amISealedOne)
                    {
                        Vector2 vector2 = Main.projectile[(int)NPC.ai[2]].Center;
                        vector2 -= NPC.Center;
                        if (vector2 == Vector2.Zero)
                        {
                            vector2 = -Vector2.UnitY;
                        }
                        vector2.Normalize();
                        if (Math.Abs(vector2.Y) < 0.77f)
                        {
                            NPC.localAI[2] = 11f;
                        }
                        else if (vector2.Y < 0f)
                        {
                            NPC.localAI[2] = 12f;
                        }
                        else
                        {
                            NPC.localAI[2] = 10f;
                        }
                        int num31 = Math.Sign(vector2.X);
                        if (num31 != 0)
                        {
                            NPC.direction = (NPC.spriteDirection = num31);
                        }
                    }
                    else
                    {
                        Vector2 vector3 = Main.projectile[(int)Main.npc[(int)AttackTotal].ai[2]].Center;
                        vector3 -= NPC.Center;
                        if (vector3 == Vector2.Zero)
                        {
                            vector3 = -Vector2.UnitY;
                        }
                        vector3.Normalize();
                        if (Math.Abs(vector3.Y) < 0.77f)
                        {
                            NPC.localAI[2] = 11f;
                        }
                        else if (vector3.Y < 0f)
                        {
                            NPC.localAI[2] = 12f;
                        }
                        else
                        {
                            NPC.localAI[2] = 10f;
                        }
                        int num32 = Math.Sign(vector3.X);
                        if (num32 != 0)
                        {
                            NPC.direction = (NPC.spriteDirection = num32);
                        }
                    }
                }
                else if (Timer >= 90f && Timer < 120f)
                {
                    shouldTakeDamage = true;
                    shouldNotBeChased = true;
                    float num33 = (Timer - 90f) / 30f;
                    NPC.alpha = 255 - (int)(num33 * 255f);
                }
                else if (Timer >= 120f && Timer < 420f)
                {
                    shouldNotBeChased = true;
                    NPC.alpha = 0;
                    if (amISealedOne)
                    {
                        Vector2 vector4 = Main.projectile[(int)NPC.ai[2]].Center;
                        vector4 -= NPC.Center;
                        if (vector4 == Vector2.Zero)
                        {
                            vector4 = -Vector2.UnitY;
                        }
                        vector4.Normalize();
                        if (Math.Abs(vector4.Y) < 0.77f)
                        {
                            NPC.localAI[2] = 11f;
                        }
                        else if (vector4.Y < 0f)
                        {
                            NPC.localAI[2] = 12f;
                        }
                        else
                        {
                            NPC.localAI[2] = 10f;
                        }
                        int num35 = Math.Sign(vector4.X);
                        if (num35 != 0)
                        {
                            NPC.direction = (NPC.spriteDirection = num35);
                        }
                    }
                    else
                    {
                        Vector2 vector5 = Main.projectile[(int)Main.npc[(int)AttackTotal].ai[2]].Center;
                        vector5 -= NPC.Center;
                        if (vector5 == Vector2.Zero)
                        {
                            vector5 = -Vector2.UnitY;
                        }
                        vector5.Normalize();
                        if (Math.Abs(vector5.Y) < 0.77f)
                        {
                            NPC.localAI[2] = 11f;
                        }
                        else if (vector5.Y < 0f)
                        {
                            NPC.localAI[2] = 12f;
                        }
                        else
                        {
                            NPC.localAI[2] = 10f;
                        }
                        int num36 = Math.Sign(vector5.X);
                        if (num36 != 0)
                        {
                            NPC.direction = (NPC.spriteDirection = num36);
                        }
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
            else if (AttackType == (float)AttackTypes.Something)
            {
                NPC.localAI[2] = 13f;
                Timer += 1f;
                if (Timer >= 120f)
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.StardustOrb)
            {
                NPC.localAI[2] = 11f;
                Vector2 vec3 = Vector2.Normalize(player.Center - npcCenter);
                if (vec3.HasNaNs())
                {
                    vec3 = new Vector2(NPC.direction, 0f);
                }
                if (Timer >= 4f && amISealedOne && (int)(Timer - 4f) % stardustOrbFireRate == 0)
                {
                    int num40 = Math.Sign(player.Center.X - npcCenter.X);
                    if (num40 != 0)
                    {
                        NPC.direction = (NPC.spriteDirection = num40);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        vec3 = Vector2.Normalize(player.Center - npcCenter + player.velocity * 20f);
                        if (vec3.HasNaNs())
                        {
                            vec3 = new Vector2(NPC.direction, 0f);
                        }
                        Vector2 ancientLightSpawn = NPC.Center + new Vector2(NPC.direction * 30, 12f);
                        float num41 = 8f;
                        float num42 = (float)Math.PI * 2f / 25f;
                        for (int num43 = 0; (float)num43 < 5f; num43++)
                        {
                            Vector2 ancientLightVelocity = vec3 * num41;
                            ancientLightVelocity = ancientLightVelocity.RotatedBy(num42 * (float)num43 - ((float)Math.PI * 2f / 5f - num42) / 2f);
                            float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * ((float)Math.PI * 2f) / 60f;
                            int ancientLight = NPC.NewNPC(NPC.GetSource_FromThis(), (int)ancientLightSpawn.X, (int)ancientLightSpawn.Y + 7, NPCID.AncientLight, 0, 0f, ai, ancientLightVelocity.X, ancientLightVelocity.Y);
                            Main.npc[ancientLight].velocity = ancientLightVelocity;
                            Main.npc[ancientLight].netUpdate = true;
                        }
                    }
                }
                Timer += 1f;
                if (Timer >= (float)(4 + stardustOrbFireRate * stardustOrbAmount))
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            else if (AttackType == (float)AttackTypes.AncientDoom)
            {
                NPC.localAI[2] = 13f;
                if (Timer >= 4f && amISealedOne && (int)(Timer - 4f) % ancientDoomFireRate == 0)
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
                            Point point = NPC.Center.ToTileCoordinates();
                            Point point2 = Main.player[NPC.target].Center.ToTileCoordinates();
                            Vector2 vector8 = Main.player[NPC.target].Center - NPC.Center;
                            int num50 = 20;
                            int num51 = 3;
                            int num52 = 7;
                            int num53 = 2;
                            int num54 = 0;
                            bool flag6 = false;
                            if (vector8.Length() > 2000f)
                            {
                                flag6 = true;
                            }
                            while (!flag6 && num54 < 100)
                            {
                                num54++;
                                int ancientDoomX = Main.rand.Next(point2.X - num50, point2.X + num50 + 1);
                                int ancientDoomY = Main.rand.Next(point2.Y - num50, point2.Y + num50 + 1);
                                if ((ancientDoomY < point2.Y - num52 || ancientDoomY > point2.Y + num52 || ancientDoomX < point2.X - num52 || ancientDoomX > point2.X + num52) && (ancientDoomY < point.Y - num51 || ancientDoomY > point.Y + num51 || ancientDoomX < point.X - num51 || ancientDoomX > point.X + num51) && !Main.tile[ancientDoomX, ancientDoomY].HasUnactuatedTile)
                                {
                                    bool flag7 = true;
                                    if (flag7 && Collision.SolidTiles(ancientDoomX - num53, ancientDoomX + num53, ancientDoomY - num53, ancientDoomY + num53))
                                    {
                                        flag7 = false;
                                    }
                                    if (flag7)
                                    {
                                        NPC.NewNPC(NPC.GetSource_FromThis(), ancientDoomX * 16 + 8, ancientDoomY * 16 + 8, NPCID.AncientLight, 0, NPC.whoAmI);
                                        flag6 = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                Timer += 1f;
                if (Timer >= (float)(4 + ancientDoomFireRate * ancientDoomAmount))
                {
                    AttackType = (float)AttackTypes.None;
                    Timer = 0f;
                    AttackTotal += 1f;
                    NPC.velocity = Vector2.Zero;
                    NPC.netUpdate = true;
                }
            }
            #endregion

            NPC.dontTakeDamage = shouldTakeDamage;
            NPC.chaseable = !shouldNotBeChased;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            
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
