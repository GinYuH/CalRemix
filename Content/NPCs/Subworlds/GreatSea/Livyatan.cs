using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis.Host.Mef;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Hostile;
using System.Collections.Generic;
using CalamityMod.InverseKinematics;
using CalamityMod.Projectiles.Boss;
using System.Reflection.Metadata.Ecma335;
using CalamityMod.Items.Accessories;
using Humanizer;
using CalRemix.Content.Items.Placeables.Subworlds.GreatSea;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalRemix.Core.World;
using CalamityMod.Items.Potions;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Bags;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    [AutoloadBossHead]
    public class Livyatan : ModNPC
    {
        public static SoundStyle HitSound = new SoundStyle("CalRemix/Assets/Sounds/LivyatanHit");
        public static SoundStyle DeathSound = new SoundStyle("CalRemix/Assets/Sounds/LivyatanDeath");
        public static SoundStyle PassiveSound = new SoundStyle("CalRemix/Assets/Sounds/LivyatanIdle") with { MaxInstances = 0 };
        public static SoundStyle RoarSound = new SoundStyle("CalRemix/Assets/Sounds/LivyatanRoar") with { MaxInstances = 0 };
        public ref float Timer => ref NPC.ai[0];

        public ref float CurrentPhase => ref NPC.ai[1];

        public ref float JawTimer => ref NPC.localAI[0];

        public ref float JawRotation => ref NPC.localAI[1];

        public ref Player Target => ref Main.player[NPC.target];

        public int NPCIndex
        {
            get => (int) NPC.ai[2] - 1;
            set => NPC.ai[2] = value + 1;
        }

        public NPC BoundNPC => Main.npc[NPCIndex];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[1] = value.Y;
                NPC.Calamity().newAI[0] = value.X;
            }

        }

        public Vector2 HeadPosition
        {
            get
            {
                Vector2 projection = NPC.Center + (NPC.rotation + (NPC.direction == -1 ? MathHelper.ToRadians(190) : 0)).ToRotationVector2() * (NPC.direction == -1 ? 240 : 190);
                Rectangle rect = new Rectangle((int)projection.X, (int)projection.Y, NPC.direction == -1 ? 100 : 80, 60);
                return rect.Center.ToVector2();
            }
        }

        // Unused: ai[3], local[2], local[3], new[2], new[3], green[all]

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 180;
            NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 56;
            NPC.lifeMax = 2000000;
            NPC.value = 1000000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            NPC.boss = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            int jawAnimLength = 80;
            bool flip = ((Target.oldPosition.X < NPC.Center.X && Target.position.X > NPC.Center.X) || (Target.oldPosition.X > NPC.Center.X && Target.position.X < NPC.Center.X));
            float flipRot = flip ? MathHelper.Pi : 0;
            float baseJawRotation = MathHelper.ToRadians(-16);
            bool lockTail = true;
            Vector2 tailDestination = NPC.Center + (Vector2.UnitX * -NPC.spriteDirection * 1000).RotatedBy(NPC.rotation + MathF.Sin(Timer * 0.05f) * 0.5f);
            NPC.TargetClosest(false);
            bool deathanim = false;
            if (CurrentPhase != 0)
            {
                if (Target.dead)
                {
                    deathanim = true;
                    NPC.velocity.Y++;
                    NPC.ai[3]++;
                    if (NPC.ai[3] > 120)
                    {
                        NPC.active = false;
                    }
                }
            }
            if (!deathanim)
            {
                if (CurrentPhase == 0)
                {
                    if (Main.rand.NextBool(600))
                    {
                        SoundEngine.PlaySound(PassiveSound, NPC.Center);
                    }
                    JawRotation = MathHelper.ToRadians(-16);
                    if (Timer % 210 == 0 || NPC.collideX || NPC.collideY)
                    {
                        if (NPC.velocity.Length() < 1)
                        {
                            NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1, 3f);
                        }
                        else
                        {
                            NPC.velocity = NPC.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.5f);
                        }
                    }
                    Timer++;
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi), 0.1f) + MathF.Sin(Timer * 0.05f + 2) * 0.02f;
                    NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();

                    if (NPC.justHit)
                    {
                        Timer = 0;
                        CurrentPhase = 1;
                    }
                }
                else if (CurrentPhase == 1)
                {
                    NPC.noTileCollide = true;
                    Timer++;
                    NPC.velocity *= 0.97f;
                    NPC.spriteDirection = NPC.direction = NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi) + flipRot, 0.1f);

                    if (Timer == 30)
                    {
                        JawTimer = 1;
                    }
                    if (JawTimer == 0 && Timer > 30)
                    {
                        CurrentPhase = 2;
                        Timer = 0;
                    }
                }
                else if (CurrentPhase == 2)
                {
                    if (Main.rand.NextBool(200))
                    {
                        SoundEngine.PlaySound(PassiveSound, NPC.Center);
                    }
                    NPC.spriteDirection = NPC.direction = 1;
                    Timer++;
                    NPC.rotation = NPC.velocity.ToRotation();
                    if (Timer == 1)
                    {
                        SavePosition = NPC.Center;
                    }
                    else
                    {
                        NPC.velocity = Vector2.Lerp(NPC.Center, (SavePosition + Vector2.UnitY.RotatedBy(Timer * MathHelper.Lerp(0, 0.12f, Utils.GetLerpValue(0, 60, Timer, true))) * 300), 0.1f) - NPC.Center;
                    }
                    if (Timer > 60 && Timer % 5 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Splash);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, SavePosition.DirectionTo(NPC.Center) * 10, ModContent.ProjectileType<LivyatanWave>(), (int)(NPC.damage * 0.25f), 1);
                        }
                    }
                    if (Timer > 240 || Target.Distance(NPC.Center) > 2000)
                    {
                        Timer = 0;
                        CurrentPhase = 3;
                    }
                }
                else if (CurrentPhase == 3)
                {
                    if (Timer == 0)
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                    if (Timer == 30)
                    {
                        JawTimer = 1;
                    }
                    if (Timer == 60 + jawAnimLength)
                    {
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 60;
                    }
                    else if (Timer > 70 + jawAnimLength)
                    {
                        NPC.velocity *= 0.96f;
                    }
                    if (Timer < 60 + jawAnimLength || Timer > 75 + jawAnimLength)
                    {
                        NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi) + flipRot, 0.05f);
                    }

                    if (Timer > 35 && Timer < 35 + jawAnimLength)
                    {
                        if (Timer % 10 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.NPCDeath13 with { Pitch = -0.3f }, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    if (Main.rand.NextBool(3))
                                        continue;
                                    Vector2 velocity = NPC.DirectionTo(Target.Center).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / 9f)).RotatedByRandom(MathHelper.PiOver4 * 0.5f) * 20;
                                    if (Main.rand.NextBool(20) && NPC.CountNPCS(ModContent.NPCType<Remora>()) < 6)
                                    {
                                        Point rt = HeadPosition.ToPoint();
                                        int type = Main.rand.NextBool(5) ? ModContent.NPCType<Zoaoa>() : ModContent.NPCType<Remora>();
                                        int n = NPC.NewNPC(NPC.GetSource_FromThis(), rt.X, rt.Y, type);
                                        Main.npc[n].velocity = velocity;
                                        Main.npc[n].dontTakeDamage = false;
                                        Main.npc[n].noTileCollide = true;
                                    }
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), HeadPosition, velocity, ModContent.ProjectileType<LivyatanBile>(), (int)(NPC.damage * 0.1f), 1);
                                }
                            }
                        }
                    }

                    Timer++;
                    NPC.spriteDirection = NPC.direction = NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
                    if (Timer > 150 + jawAnimLength)
                    {
                        CurrentPhase = 4;
                        Timer = 0;
                    }
                }
                else if (CurrentPhase == 4)
                {
                    NPC.spriteDirection = NPC.direction = NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi) + flipRot, 0.1f);
                    if (Timer < 40)
                    {
                        CalamityUtils.SmoothMovement(NPC, 200, Target.Center - NPC.Center, 20, 0.3f, true);
                    }
                    else if (Timer >= 40 && Timer < 80)
                    {
                        JawRotation = MathHelper.Lerp(baseJawRotation, MathHelper.ToRadians(16), CalamityUtils.ExpInEasing(Utils.GetLerpValue(40, 80, Timer, true), 1));
                    }
                    else if (Timer == 80)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemGrenadeChuck with { Pitch = -0.6f }, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)HeadPosition.X, (int)HeadPosition.Y, ModContent.NPCType<XiphactinusHead>(), ai0: NPC.whoAmI + 1);
                            Main.npc[n].direction = NPC.direction;
                            Main.npc[n].spriteDirection = NPC.spriteDirection;
                            Main.npc[n].rotation = NPC.rotation;
                            Main.npc[n].velocity = (NPC.rotation + MathHelper.ToRadians(NPC.spriteDirection == -1 ? 60 : 22) + (NPC.spriteDirection == -1 ? MathHelper.PiOver2 : 0)).ToRotationVector2() * 36;
                            NPCIndex = n;
                        }
                    }
                    if (Timer >= 40)
                    {
                        NPC.velocity *= 0.95f;
                    }
                    Timer++;
                }
                else if (CurrentPhase == 5)
                {
                    Timer++;
                    int swingTime = 50;
                    int smashTime = 10;
                    int maxtim = swingTime + smashTime + 40;
                    if (Timer == 1)
                    {
                        SavePosition = NPC.Center;
                        SoundEngine.PlaySound(BetterSoundID.ItemToxicFlaskThrow with { Pitch = -0.6f }, NPC.Center);
                    }

                    if (Timer > 1)
                    {
                        if (Timer < swingTime)
                        {
                            Vector2 dirToSave = SavePosition - BoundNPC.Center;
                            Vector2 newPos = BoundNPC.Center + dirToSave.RotatedBy(MathHelper.Lerp(0, -MathHelper.Pi, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, swingTime, Timer, true), 1)));
                            NPC.velocity = newPos - NPC.Center;
                        }
                        else if (Timer == swingTime)
                        {
                            SavePosition = NPC.Center;
                        }
                        else if (Timer >= swingTime && Timer < swingTime + smashTime)
                        {
                            Vector2 newPos = Vector2.Lerp(SavePosition, BoundNPC.Center, Utils.GetLerpValue(swingTime, smashTime + swingTime, Timer, true));
                            NPC.velocity = newPos - NPC.Center;
                        }
                        if (Timer >= swingTime + smashTime)
                        {
                            NPC.velocity *= 0.9f;
                            JawRotation = MathHelper.Lerp(MathHelper.ToRadians(16), baseJawRotation, CalamityUtils.ExpOutEasing(Utils.GetLerpValue(swingTime + smashTime, maxtim - 20, Timer, true), 1));
                            if (NPCIndex > -1)
                            {
                                SoundEngine.PlaySound(WulfrumAcrobaticsPack.GrabSound, NPC.Center);
                                BoundNPC.active = false;
                                NPCIndex = -1;
                            }
                            if (Timer > maxtim)
                            {
                                Timer = 0;
                                CurrentPhase = 6;
                            }
                        }

                        if (NPCIndex > -1)
                        {
                            NPC.spriteDirection = BoundNPC.Center.X < NPC.Center.X ? -1 : 1;
                            NPC.rotation = NPC.DirectionTo(BoundNPC.Center).ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
                        }
                    }
                }
                if (CurrentPhase == 6)
                {
                    NPC.noTileCollide = true;
                    Timer++;

                    int diveTime = 50;
                    int dashTime = 60;
                    int wait = 80;
                    bool look = false;
                    if (Timer < diveTime)
                    {
                        if (NPC.velocity.Y <= 0)
                        {
                            NPC.velocity.Y = 5;
                        }
                        if (NPC.velocity.Y < 30)
                            NPC.velocity.Y *= 4f;
                        lockTail = false;
                        tailDest = NPC.Center - Vector2.UnitY * 1000;
                    }
                    else if (Timer == diveTime)
                    {
                        NPC.Center = Target.Center + Vector2.UnitY * 2000;
                    }
                    else if (Timer == diveTime + 1)
                    {
                        JawTimer = 1;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 60;
                    }
                    else if (Timer > diveTime + dashTime)
                    {
                        CalamityUtils.SmoothMovement(NPC, 40, (Target.Center + new Vector2(600, -300)) - NPC.Center, 30, 1.3f, false);
                        look = true;
                    }

                    if (Timer > diveTime + 1 && Timer < diveTime + dashTime)
                    {
                        lockTail = false;
                        tailDest = NPC.Center + Vector2.UnitY * 1000;
                        if (NPC.ai[3] == 0 && Collision.SolidCollision(Utils.CenteredRectangle(HeadPosition, Vector2.One * 200).TopLeft(), 200, 200) && NPC.Bottom.Y < Target.Top.Y)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemMeteorImpact, NPC.Center);
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 4;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), HeadPosition, -NPC.velocity.SafeNormalize(Vector2.UnitY).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4 * 1.5f, MathHelper.PiOver4 * 1.5f, i / 9f)).RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(16, 25), ModContent.ProjectileType<ThrowableChunk>(), (int)(NPC.damage * 0.5f), 1, ai1: Main.rand.Next(0, 2));
                                }
                            }
                            NPC.ai[3] = 1;
                        }
                    }

                    if (look)
                    {
                        NPC.spriteDirection = NPC.direction = NPC.DirectionTo(Main.player[NPC.target].Center).X.DirectionalSign();
                        NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi) + flipRot, 0.1f);
                    }
                    else
                    {
                        NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi), 0.1f) + MathF.Sin(Timer * 0.05f + 2) * 0.02f;
                        NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
                    }

                    if (Timer > diveTime + dashTime + wait)
                    {
                        NPC.ai[3] = 0;
                        Timer = 0;
                        CurrentPhase = (NPC.life < NPC.lifeMax * 0.5f) ? 7 : 2;
                    }
                }
                if (CurrentPhase == 7)
                {
                    Timer++;
                    NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.1f);
                    NPC.velocity *= 0.96f;
                    if (Timer == 30)
                    {
                        JawTimer = 1;
                        int mtCount = 16;
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
                        for (int i = 0; i < mtCount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.Next(-2000, 2000), Main.rand.Next(600, 1200)), Vector2.UnitY * Main.rand.Next(-6, -1), ModContent.ProjectileType<ThrowableChunk>(), NPC.damage, 1, ai1: 2);
                            }
                        }
                    }
                    if (Timer > jawAnimLength + 60)
                    {
                        Timer = 0;
                        CurrentPhase = 2;
                    }
                    if (Timer > 30 && Timer < 45)
                    {
                        if (Timer % 3 == 0)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemMeteorStaffLunarFlare, NPC.Center);
                        }
                    }
                }

                if (NPC.velocity.Length() > 3)
                {
                    foreach (Projectile p in Main.ActiveProjectiles)
                    {
                        if (p.type != ModContent.ProjectileType<ThrowableChunk>())
                            continue;
                        if (p.ai[1] != 2)
                            continue;
                        if (NPC.getRect().Intersects(p.getRect()))
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemMeteorStaffLunarFlare, NPC.Center);
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 3;
                            p.Kill();
                            int ct = 10;
                            for (int i = 0; i < ct; i++)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int pe = Projectile.NewProjectile(NPC.GetSource_FromThis(), p.Center, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)ct)).RotatedByRandom(MathHelper.PiOver4) * 15, ModContent.ProjectileType<ThrowableChunk>(), (int)(p.damage * 0.25f), 1, ai1: Main.rand.Next(0, 2));
                                    Main.projectile[pe].height = Main.projectile[pe].width = 22;
                                }
                            }
                        }
                    }
                    if (Collision.SolidTiles(NPC.position, NPC.width, NPC.height) && (CurrentPhase == 3 || CurrentPhase == 5 || CurrentPhase == 6))
                    {
                        for (int i = 0; i < (int)(NPC.width); i++)
                        {
                            for (int j = 0; j < (int)(NPC.height); j++)
                            {
                                Point start = NPC.position.ToTileCoordinates();
                                Tile t = CalamityUtils.ParanoidTileRetrieval(start.X + i, start.Y + j);
                                if (t.TileType == ModContent.TileType<SyringodiumPlaced>())
                                {
                                    WorldGen.KillTile(start.X + i, start.Y + j, noItem: true);
                                }
                            }
                        }
                    }
                }
            }


            if (JawTimer >= 1)
            {
                BasicOpenMouth();
            }

            if (flip)
            {
                handIK.Limbs[0].Rotation += MathHelper.Pi;
                tailIK.Limbs[0].Rotation += MathHelper.Pi;
            }
            handIK.Limbs[0].Rotation = GetIKRotationClamp((float)handIK.Limbs[0].Rotation, MathHelper.Pi + NPC.rotation, MathHelper.PiOver2 + NPC.rotation);
            if (lockTail)
                tailIK.Limbs[0].Rotation = GetIKRotationClamp((float)tailIK.Limbs[0].Rotation, MathHelper.ToRadians(270) + NPC.rotation, MathHelper.ToRadians(120) + NPC.rotation);
            handIK.Update(NPC.Center + new Vector2(NPC.spriteDirection * 160, 40).RotatedBy(NPC.rotation), NPC.Center + (Vector2.UnitX * -NPC.spriteDirection * 1000).RotatedBy(NPC.rotation + MathF.Cos(Timer * 0.05f) * 0.5f));
            tailIK.Update(NPC.Center + new Vector2(NPC.spriteDirection * -170, 0).RotatedBy(NPC.rotation), tailDestination);
        }

        public void BasicOpenMouth()
        {
            int jawOpenEnd = 15;
            int jawAnim = jawOpenEnd + 50;
            int jawFinish = jawAnim + 15;

            float baseRotation = MathHelper.ToRadians(-16);
            float openRotation = MathHelper.ToRadians(16);

            if (JawTimer == jawOpenEnd)
            {
                SoundEngine.PlaySound(RoarSound, NPC.Center);
            }

            if (JawTimer >= 0 && JawTimer <= jawOpenEnd)
            {
                JawRotation = MathHelper.Lerp(baseRotation, openRotation, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, jawOpenEnd, JawTimer, true), 1));
            }
            else if (JawTimer >= jawAnim && JawTimer <= jawFinish)
            {
                JawRotation = MathHelper.Lerp(openRotation, baseRotation, CalamityUtils.SineInEasing(Utils.GetLerpValue(jawAnim, jawFinish, JawTimer, true), 1));
            }
            if (JawTimer > jawFinish)
            {
                JawTimer = 0;
            }
            else
                JawTimer++;
        }


        public float GetIKRotationClamp(float baseRotation, float min, float max)
        {
            float flipRot = NPC.spriteDirection == -1 ? MathHelper.Pi : 0;
            if (NPC.spriteDirection == -1)
            {
                return MathHelper.Clamp(baseRotation, NPC.spriteDirection * min + flipRot, NPC.spriteDirection * max + flipRot);
            }
            else
            {
                return MathHelper.Clamp(baseRotation, NPC.spriteDirection * max + flipRot, NPC.spriteDirection * min + flipRot);
            }
            return MathHelper.Clamp(baseRotation, NPC.spriteDirection * min + flipRot, NPC.spriteDirection * max + flipRot);
        }

        public LimbCollection handIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 96f, 142f);
        public Vector2 handDest = new Vector2();

        public LimbCollection tailIK = new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.ToRadians(60)), 202f, 248f);
        public Vector2 tailDest = new Vector2();

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Head").Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "Mouth").Value;
            Texture2D arm = ModContent.Request<Texture2D>(Texture + "Arm").Value;
            Texture2D hand = ModContent.Request<Texture2D>(Texture + "Hand").Value;
            Texture2D pupil = ModContent.Request<Texture2D>(Texture + "Pupil").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "Eye").Value;
            Texture2D bodyLower = ModContent.Request<Texture2D>(Texture + "BodyLower").Value;
            Texture2D tail = ModContent.Request<Texture2D>(Texture + "Tail").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 headPos = new Vector2(180, -40);
            Vector2 adjustedHeadPos = new Vector2(180 * NPC.spriteDirection, -40);
            float headRotation = 0;// (NPC.Center + adjustedHeadPos).DirectionTo(Main.MouseWorld).ToRotation();
            /*headRotation *= NPC.spriteDirection;
            if (NPC.spriteDirection == -1)
            {
                headRotation += MathHelper.Pi;
            }*/
            adjustedHeadPos = adjustedHeadPos.RotatedBy(headRotation);

            if (CurrentPhase == 5 || CurrentPhase == 4)
            {
                if (JawRotation >= 0)
                {
                    Texture2D xyph = TextureAssets.Npc[ModContent.NPCType<Xiphactinus>()].Value;
                    spriteBatch.Draw(xyph, GetPiecePosition(screenPos, new Vector2(260, 60), rotationAnchor: headRotation), xyph.Frame(1, 3, 0, 0), NPC.GetAlpha(drawColor), NPC.rotation + NPC.spriteDirection * (headRotation), new Vector2(xyph.Width / 2, xyph.Height / 6), NPC.scale, fx, 0);

                    if (NPCIndex > -1)
                    {
                        //if (BoundNPC.Distance(NPC.Center) < 3000)
                        {
                            Vector2 bottom = GetPiecePosition(screenPos, new Vector2(300, 60), rotationAnchor: headRotation) + screenPos;
                            Vector2 distToProj = BoundNPC.Center;
                            float projRotation = NPC.AngleTo(bottom) - 1.57f;
                            bool doIDraw = true;
                            Texture2D chain = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/XiphactinusChain").Value;

                            while (doIDraw)
                            {
                                float distance = (bottom - distToProj).Length();
                                if (distance < (chain.Height + 1))
                                {
                                    doIDraw = false;
                                }
                                else if (!float.IsNaN(distance))
                                {
                                    Color drawColore = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                                    distToProj += BoundNPC.Center.DirectionTo(bottom) * chain.Height;
                                    Main.EntitySpriteDraw(chain, distToProj - Main.screenPosition,
                                        new Rectangle(0, 0, chain.Width, chain.Height), drawColore, projRotation,
                                        Utils.Size(chain) / 2f, 1f, SpriteEffects.None, 0);
                                }
                            }
                        }
                    }
                }
            }
            DrawPiece(spriteBatch, jaw, screenPos, new Vector2(180, 50), new Vector2(334, 23), drawColor, rotationAnchor: JawRotation + headRotation);
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, fx, 0);
            DrawPiece(spriteBatch, head, screenPos, headPos, new Vector2(397, 54), drawColor, rotationOverride: headRotation);
            DrawPiece(spriteBatch, eye, screenPos, new Vector2(321, -1), new Vector2(10, 3), drawColor, rotationAnchor: headRotation);
            DrawPiece(spriteBatch, pupil, screenPos, new Vector2(321, -1), new Vector2(3, 3), drawColor, rotationAnchor: headRotation);

            for (int i = 0; i < handIK.Limbs.Length; i++)
            {
                Texture2D touse = (i == 0) ? arm : hand;
                Vector2 origin = (i == 0) ? new Vector2(30, 24) : new Vector2(35, 19);
                float offset = (i == 0) ? -MathHelper.PiOver4 * 3 : MathHelper.ToRadians(200);
                if (NPC.spriteDirection == -1)
                {
                    offset = -offset + MathHelper.Pi;
                }
                Limb limb = handIK.Limbs[i];
                spriteBatch.Draw(touse, limb.ConnectPoint - screenPos, null, NPC.GetAlpha(drawColor), (float)limb.Rotation + offset, GetPieceOrigin(touse.Width, origin), NPC.scale, fx, 0);
            }
            for (int i = 0; i < tailIK.Limbs.Length; i++)
            {
                Texture2D touse = (i == 0) ? bodyLower : tail;
                Vector2 origin = (i == 0) ? new Vector2(31, 52) : new Vector2(15, 43);
                float offset = (i == 0) ? MathHelper.ToRadians(180) : MathHelper.ToRadians(180);
                if (NPC.spriteDirection == -1)
                {
                    offset = -offset + MathHelper.Pi;
                }
                Limb limb = tailIK.Limbs[i];
                spriteBatch.Draw(touse, limb.ConnectPoint - screenPos, null, NPC.GetAlpha(drawColor), (float)limb.Rotation + offset, GetPieceOrigin(touse.Width, origin), NPC.scale, fx, 0);
            }
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == ModContent.NPCType<Remora>() && n.ai[2] == NPC.whoAmI + 1)
                {
                    Texture2D rem = TextureAssets.Npc[ModContent.NPCType<Remora>()].Value;
                    spriteBatch.Draw(rem, n.Center - screenPos, n.frame, n.GetAlpha(drawColor), n.rotation, new Vector2(rem.Width / 2, rem.Height / 4), n.scale, fx, 0);
                }
            }
            return false;
        }

        public void DrawPiece(SpriteBatch sb, Texture2D tex, Vector2 screenPos, Vector2 offset, Vector2 originOffset, Color drawColor, float rotationOverride = 0, float rotationAnchor = 0)
        {
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            sb.Draw(tex, GetPiecePosition(screenPos, offset, rotationOverride, rotationAnchor), null, NPC.GetAlpha(drawColor), NPC.rotation + NPC.spriteDirection * (rotationAnchor + rotationOverride), GetPieceOrigin(tex.Width, originOffset), NPC.scale, fx, 0);
        }

        public Vector2 GetPiecePosition(Vector2 screenPos, Vector2 offset, float rotationOverride = 0, float rotationAnchor = 0)
        {
            return NPC.Center - screenPos + NPC.scale * new Vector2(offset.X * NPC.spriteDirection, offset.Y).RotatedBy(NPC.rotation);// + rotationAnchor);
        }

        public Vector2 GetPieceOrigin(float baseWidth, Vector2 originalOffset)
        {
            return new Vector2(NPC.spriteDirection == 1 ? baseWidth - originalOffset.X : originalOffset.X, originalOffset.Y);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<SupremeHealingPotion>();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();
            int[] itemIDs =
            {
                ModContent.ItemType<LivyatanadoStaff>(),
                ModContent.ItemType<RemoraDart>(),
                ModContent.ItemType<Laevateinn>(),
                ModContent.ItemType<XiphactinusGun>(),
                ModContent.ItemType<FrilledShark>()
            };
            npcLoot.Add(ModContent.ItemType<LivyatanTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<LivyatanBag>());
        }

        public override void OnKill()
        {
            RemixDowned.downedLivyatan = true;
            RemixDowned.subDownedLivyatan = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
