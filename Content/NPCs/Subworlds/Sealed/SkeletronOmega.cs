using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Tiles.Ores;
using CalamityMod.CalPlayer;
using CalRemix.Core.Subworlds;
using System.Collections.Generic;
using CalamityMod.InverseKinematics;
using CalRemix.Content.Projectiles.Weapons;
using Steamworks;
using Terraria.DataStructures;

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

        public static Vector2 TentCenter => new(SealedSubworldData.tentPos.X, SealedSubworldData.tentPos.Y - 300);

        public static float TentRight => SealedSubworldData.tentPos.X + 50 * 16;

        public static float TentLeft => SealedSubworldData.tentPos.X - 74 * 16;

        public static float TentTop => SealedSubworldData.tentPos.Y - 44 * 16;

        public static float TentBottom => SealedSubworldData.tentPos.Y + 4 * 16;

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
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToCold = true;
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
                        Timer++;
                        CalamityUtils.SmoothMovement(NPC, 10, TentCenter - NPC.Center - NPC.DirectionTo(Target.Center) * 40, 10, 0.6f, true);
                        if (Timer > 200)
                        {
                            ChangePhase(PhaseType.SlamSlamSlam);
                        }
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
                        if (localTime == 0)
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
                        else if (SavePosition != Vector2.Zero && localTime <= travelTime)
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
                        }
                        if ((pos.Y > TentBottom || pos.Y < TentTop) && ExtraVar2 <= 0)
                        {
                            NPC.velocity.Y *= -1;
                            ExtraVar2 = 20;
                        }
                        if (Timer > windUp + 30 && Timer % 5 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -NPC.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(6, 10), ProjectileID.BrainScramblerBolt, CalRemixHelper.ProjectileDamage(200, 300), 1); 
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 6), NPC.scale, SpriteEffects.None, 0f);

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
                        spriteBatch.Draw(arm, lim.ConnectPoint - Main.screenPosition, null, Lighting.GetColor(limp), rot + MathHelper.PiOver2, new Vector2(arm.Width / 2, arm.Height), NPC.scale, SpriteEffects.None, 0f);
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
    }
}