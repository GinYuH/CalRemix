using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Tiles.Ores;
using CalRemix.Core.Subworlds;
using System.Collections.Generic;
using CalamityMod.InverseKinematics;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Typeless;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Sounds;
using System.IO;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class SkeletronOmega : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public ref float ExtraVar => ref NPC.ai[2];

        public ref float ExtraVar2 => ref NPC.ai[3];

        public int FistIndex => NPC.FindFirstNPC(ModContent.NPCType<OmegaFist>());

        public int GunIndex => NPC.FindFirstNPC(ModContent.NPCType<OmegaSoothingSunlightBlaster>());


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

        public bool IsGroovin
        {
            get => NPC.Calamity().newAI[0] == 1;
            set => NPC.Calamity().newAI[0] = value.ToInt();
        }

        public static Vector2 TentCenter => SealedSubworldData.TentCenter;

        public static float TentRight => SealedSubworldData.TentRight;

        public static float TentLeft => SealedSubworldData.TentLeft;

        public static float TentTop => SealedSubworldData.TentTop;

        public static float TentBottom => SealedSubworldData.TentBottom;

        public static float SlamTravelTime => 30f;
        public static float SlamDuration => 120f;
        public static float FireballTravelTime => 120f;
        public static float FireballDuration => 120f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
        }

        public enum PhaseType
        {
            SpawnAnim = 0,
            GunShots = 1,
            Fireballs = 2,
            SlamSlamSlam = 3,
            Judgement = 4,
            Desperation = 5
        }

        public AnimType CurrentAnimation
        {
            get => (AnimType)NPC.Calamity().newAI[3];
            set => NPC.Calamity().newAI[3] = (int)value;
        }

        public enum AnimType
        {
            None = 0,
            Biting = 1,
            Spinning = 3
        }

        public bool spawnedArms = false;

        public static List<LimbCollection> limbs = new List<LimbCollection>() { new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.27f, MathHelper.PiOver2), [120f, 120f]), new LimbCollection(new CyclicCoordinateDescentUpdateRule(0.27f, MathHelper.PiOver2), [120f, 120f]) };

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.LifeMaxNERB(50000, 100000, 150000);
            NPC.damage = 200;
            NPC.defense = 43;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.HitSound = AuricOre.MineSound;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = true;
            NPC.boss = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SealedFieldsBiome>().Type };
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.DR_NERD(0.3f);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = new[] { ModContent.GetInstance<SealedUndergroundBiome>().Type };
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
            writer.Write(NPC.Calamity().newAI[2]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
        }
        public override void AI()
        {
            if (!spawnedArms)
            {
                spawnedArms = true;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 240, (int)NPC.Center.Y, ModContent.NPCType<OmegaSoothingSunlightBlaster>(), ai0: NPC.whoAmI + 1);
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 240, (int)NPC.Center.Y, ModContent.NPCType<OmegaFist>(), ai0: NPC.whoAmI + 1);
            }
            if (NPC.life < NPC.lifeMax * 0.3f && State != (int)PhaseType.Desperation)
            {
                ChangePhase(PhaseType.Desperation);
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.type == ModContent.NPCType<OmegaFist>() || n.type == ModContent.NPCType<OmegaSoothingSunlightBlaster>())
                    {
                        n.active = false;
                        n.HitEffect();
                    }
                }
            }
            NPC.TargetClosest();
            if (!NPC.HasPlayerTarget || Target.dead)
            {
                NPC.active = false;
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.type == ModContent.NPCType<DreadonFriendly>())
                    {
                        n.active = false;
                    }
                }
            }
            switch ((PhaseType)State)
            {
                case PhaseType.SpawnAnim:
                    {
                        Timer++;
                        if (ExtraVar == 0)
                        {
                            int grace = 10;
                            if (NPC.velocity.Y == 0)
                            {
                                NPC.velocity.Y = 3;
                            }
                            NPC.velocity.Y = MathHelper.Min(NPC.velocity.Y * 1.5f, 22);
                            if (Timer > grace && Collision.SolidTiles(NPC.position, NPC.width, NPC.height))
                            {
                                ExtraVar = 1;
                                Timer = 0;
                                NPC.velocity = Vector2.Zero;
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 20;
                                foreach (Player p in Main.ActivePlayers)
                                {
                                    SoundEngine.PlaySound(AresTeslaCannon.TeslaOrbShootSound with { Pitch = -0.4f }, NPC.Center);
                                    SoundEngine.PlaySound(AresGaussNuke.NukeExplosionSound, NPC.Center);
                                    Point pos = p.Bottom.ToTileCoordinates();
                                    Tile t = CalamityUtils.ParanoidTileRetrieval(pos.X, pos.Y);
                                    if (t.IsTileSolidGround() && p.velocity == Vector2.Zero)
                                    {
                                        p.velocity.Y = -8;
                                    }
                                }
                            }
                        }
                        else if (ExtraVar == 1)
                        {
                            int spinAnimTime = 3;
                            int stillTime = 120;
                            int riseTime = 50 + stillTime;
                            int roarTime = riseTime + 20;
                            int end = roarTime + 40;
                            if (Timer <= spinAnimTime)
                            {
                                NPC.rotation = Utils.AngleLerp(0, MathHelper.ToRadians(75), CalamityUtils.SineInEasing(Timer / (float)spinAnimTime, 1));
                            }
                            else if (Timer == stillTime)
                            {
                                NPC.velocity.Y = -1;
                                SavePosition = NPC.Center - Vector2.UnitY * 200;
                                OldPosition = NPC.Center;
                            }
                            else if (Timer > stillTime && Timer < riseTime)
                            {
                                float completion = Utils.GetLerpValue(stillTime + 1, riseTime - 1, Timer, true);
                                NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(completion, 1));
                                NPC.rotation = Utils.AngleLerp(MathHelper.ToRadians(75), 0, CalamityUtils.SineInEasing(completion, 1));
                            }
                            else if (Timer == roarTime)
                            {
                                NPC.velocity.Y = 0;
                                SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
                                CurrentAnimation = AnimType.Spinning;
                            }
                            else if (Timer == end)
                            {
                                ChangePhase(PhaseType.GunShots);
                                NPC.dontTakeDamage = false;
                                CurrentAnimation = AnimType.Biting;
                            }
                        }
                        break;
                    }
                case PhaseType.GunShots:
                    {
                        CalamityUtils.SmoothMovement(NPC, 10, TentCenter - NPC.Center - NPC.DirectionTo(Target.Center) * 40, 10, 0.6f, true);

                        float singleDuration = 160;
                        float cooldown = 0;
                        float localTime = Timer % (singleDuration + cooldown);
                        int spawnRetic = 30;
                        int fireShot = (int)singleDuration - 20;

                        if (localTime == spawnRetic)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center, Vector2.Zero, ModContent.ProjectileType<OmegaReticle>(), 0, 0, ai0: Target.whoAmI, ai2: Main.rand.Next(0, 2222));
                            }
                        }
                        if (localTime == fireShot)
                        {
                            int reticle = CalamityUtils.FindFirstProjectile(ModContent.ProjectileType<OmegaReticle>());
                            if (reticle > -1)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 pos = Main.npc[GunIndex].Center;
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, pos.DirectionTo(Main.projectile[reticle].Center) * 20, ProjectileID.BulletHighVelocity, CalRemixHelper.ProjectileDamage(380, 480), 1);
                                    Main.projectile[p].friendly = false; 
                                    Main.projectile[p].hostile = true;
                                    Main.projectile[p].DamageType = DamageClass.Default;
                                    Main.projectile[reticle].Kill();
                                }
                                Main.npc[GunIndex].Center = Main.npc[GunIndex].Center - Main.npc[GunIndex].Center.DirectionTo(Target.Center) * 10;
                            }
                        }

                        if (Timer > (singleDuration + cooldown) * 4)
                        {
                            ChangePhase(PhaseType.SlamSlamSlam);
                            NPC.velocity = Vector2.Zero;
                        }
                        Timer++;
                        break;
                    }
                case PhaseType.Fireballs:
                    {
                        float singleDuration = FireballDuration;
                        float travelTime = FireballTravelTime;
                        float cycleDuration = travelTime + singleDuration;
                        float localTimer = Timer % cycleDuration;
                        if (localTimer == 0)
                        {
                            if (ExtraVar == 0)
                            {
                                bool playerLeft = Target.Center.X < SealedSubworldData.tentPos.X;
                                if (playerLeft)
                                {
                                    ExtraVar = Main.rand.Next(3, 5);
                                }
                                else
                                {
                                    ExtraVar = Main.rand.Next(1, 3);
                                }
                            }
                            else
                            {
                                ExtraVar++;
                                if (ExtraVar > 4)
                                    ExtraVar = 1;
                            }
                        }
                        else
                        {
                            Vector2 gotov = ExtraVar switch
                            {
                                1 => new Vector2(TentRight, TentTop),
                                2 => new Vector2(TentRight, TentBottom),
                                3 => new Vector2(TentLeft, TentBottom),
                                4 => new Vector2(TentLeft, TentTop),
                                _ => NPC.Center
                            };
                            CalamityUtils.SmoothMovement(NPC, 10, gotov - NPC.Center, 30, 1.8f, true);
                        }

                        if (Timer > cycleDuration * 4)
                        {
                            ChangePhase(PhaseType.GunShots);
                        }

                        Timer++;
                        break;
                    }
                case PhaseType.SlamSlamSlam:
                    {
                        float singleDuration = SlamDuration;
                        float travelTime = SlamTravelTime;
                        float localTime = Timer % (singleDuration + travelTime);
                        float totalDur = singleDuration + travelTime;
                        int rounds = 4;
                        if (localTime == 1)
                        {
                            float curDist = 300;
                            Vector2 targetPos = Vector2.Zero;
                            for (int i = 0; i < 30; i++)
                            {
                                Vector2 newPos = new Vector2(Main.rand.NextFloat(TentLeft, TentRight), SealedSubworldData.tentPos.Y);
                                float newDist = newPos.Distance(NPC.Center);
                                if (newDist >= curDist && newDist < 1000)
                                {
                                    targetPos = newPos;
                                    break;
                                }
                                targetPos = newPos;
                            }

                            SavePosition = targetPos;
                            OldPosition = NPC.Center;
                        }
                        else if (SavePosition != Vector2.Zero && localTime <= travelTime && localTime > 0)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(0, travelTime, localTime, true), 1));
                        }

                        if (localTime > travelTime)
                        {
                            CurrentAnimation = AnimType.Spinning;
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.DirectionTo(Main.npc[FistIndex].Center).ToRotation() * 0.8f, 0.1f);
                            ExtraVar = 1;
                        }
                        else
                        {
                            CurrentAnimation = AnimType.Biting;
                            NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.1f);
                        }
                        if (localTime == (travelTime + singleDuration - 1))
                        {
                            ExtraVar = 0;
                        }
                        NPC.velocity.Y = 0;
                        Timer++;
                        if (Timer > rounds * totalDur)
                        {
                            ChangePhase(PhaseType.Fireballs);
                        }
                        break;
                    }
                case PhaseType.Judgement:
                    {
                        break;
                    }
                case PhaseType.Desperation:
                    {
                        Timer++;
                        int windUp = 60;
                        Vector2 pos = NPC.Center;
                        if (Timer == windUp)
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 20;
                        }
                        if ((pos.X > TentRight || pos.X < TentLeft) && ExtraVar <= 0)
                        {
                            NPC.velocity.X *= -1;
                            ExtraVar = 20;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), 1000, 0, Target.whoAmI, NPC.whoAmI);
                            }
                        }
                        if ((pos.Y > TentBottom || pos.Y < TentTop) && ExtraVar2 <= 0)
                        {
                            NPC.velocity.Y *= -1;
                            ExtraVar2 = 20;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), 1000, 0, Target.whoAmI, NPC.whoAmI);
                            }

                        }
                        if (Timer > windUp + 30 && Timer % 5 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -NPC.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(6, 10), ModContent.ProjectileType<OmegaPlumLaser>(), CalRemixHelper.ProjectileDamage(200, 300), 1); 
                            }
                        }
                        NPC.rotation += 1f;
                        CurrentAnimation = AnimType.Spinning;
                        ExtraVar--;
                        ExtraVar2--;
                        break;
                    }
            }

            if (FistIndex > -1)
            {
                limbs[1].Update(NPC.Center + new Vector2(50, 40), Main.npc[FistIndex].Center);
            }
            if (GunIndex > -1)
            {
                limbs[0].Update(NPC.Center + new Vector2(-50, 40), Main.npc[GunIndex].Center);
            }

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.Center.X < TentLeft || p.Center.X > TentRight || p.Center.Y > TentBottom || p.Center.Y < TentTop)
                {
                    p.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 2222, 1);
                }
            }
        }

        public void ChangePhase(PhaseType newPhase)
        {
            ExtraVar = 0;
            ExtraVar2 = 0;
            Timer = 0;
            SavePosition = Vector2.Zero;
            OldPosition = Vector2.Zero;
            State = (int)newPhase;
            NPC.netUpdate = true;
        }

        public override void FindFrame(int frameHeight)
        {
            if (CurrentAnimation == AnimType.Spinning)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (CurrentAnimation == AnimType.Biting)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
            else if (CurrentAnimation == AnimType.None)
            {
                NPC.frame.Y = 0;
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 6), NPC.scale, SpriteEffects.None, 0f);

            Texture2D arm = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/OmegaArm").Value;

            if (State != (int)PhaseType.Desperation)
            {
                foreach (LimbCollection l in limbs)
                {
                    for (int i = 0; i < l.Limbs.Length; i++)
                    {
                        Limb lim = l.Limbs[i];
                        float rot = lim.ConnectPoint.DirectionTo(lim.EndPoint).ToRotation();
                        Point limp = ((lim.EndPoint - lim.ConnectPoint) * 0.5f + lim.ConnectPoint).ToTileCoordinates();
                        spriteBatch.Draw(arm, lim.ConnectPoint - screenPos, null, Lighting.GetColor(limp), rot + MathHelper.PiOver2, new Vector2(arm.Width / 2, arm.Height), NPC.scale, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }

        public override bool CheckActive()
        {
            bool alive = false;
            foreach (Player p in Main.ActivePlayers)
            {
                if (!p.dead)
                {
                    alive = true;
                }
            }
            return !alive;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return State == (int)PhaseType.Desperation;
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}