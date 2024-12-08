using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using ReLogic.Content;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.World;
using Terraria.DataStructures;
using CalamityMod.Events;
using static CalamityMod.World.CalamityWorld;
using static CalamityMod.NPCs.CalamityGlobalNPC;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Items.Potions;
using System.Runtime.Serialization;
using Terraria.GameContent.ObjectInteractions;
using CalRemix.Core.Retheme;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Buffs;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Accessories;
using CalRemix.Core.World;
using CalRemix.Content.Items.Lore;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Sounds;
using CalamityMod.Particles;
using CalRemix.Content.NPCs.TownNPCs;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Items.Weapons.Ranged;
using rail;
using CalRemix.UI.ElementalSystem;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.NPCs.Cryogen;

namespace CalRemix.Content.NPCs.Bosses.Pyrogen
{
    [AutoloadBossHead]
    public class Pyrogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];
        private int currentPhase = 1;
        private int biomeEnrageTimer = CalamityMod.NPCs.CalamityGlobalNPC.biomeEnrageTimerMax;
        private double rotation;
        private double rotationIncrement;
        private int rotationDirection;
        public FireParticleSet FireDrawer = null;

        public ref Player Target => ref Main.player[NPC.target];


        public Rectangle teleportPos = new Rectangle();
        public Rectangle playerRadius = new Rectangle();

        public ref float AttackTimer => ref NPC.ai[2];

        float rotationMult = 3f;
        float rotationAmt = 0.03f;
        public bool predictiveCharge = false; //decides whether dash will predict player movement
        public bool biomeEnraged = false;
        public bool ultraEnraged = false; //stops him from enraging again if summoned by cryo key

        public int attackSubTotal = 0;
        public int attackTotal = 0; //counts how many attacks have been used- for hellstorm, since it's position in the cycle is set
        public int enrageCounter = 0;
        public const float BlackholeSafeTime = 40;

        public enum PyroPhaseType
        {
            Idle = 0,
            Charge = 1,
            Rain = 2,
            PullintoShield = 3,
            FireWall = 4,
            PonceSpin = 5,
            FireChain = 6,
            Hellstorm = 7,
            HellstormFatal = 8,
            Transitioning = 9
        }

        public PyroPhaseType AIState
        {
            get => (PyroPhaseType)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }

        public int chargeLimit = 4;
        public int charges = 0;
        public int deathTimer = 0;
        public ref float Time => ref NPC.ai[0];

        public static readonly SoundStyle HitSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenHit", 3);
        public static readonly SoundStyle TransitionSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenTransition");
        public static readonly SoundStyle DeathSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenDeath");
        public static readonly SoundStyle FlareSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenAttack", 4);
        public static readonly SoundStyle ChargeSound = new("CalamityMod/Sounds/Custom/Yharon/YharonFireball", 3);
        public static readonly SoundStyle EnrageSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenEnrage");
        public override string Texture => "CalRemix/Content/NPCs/Bosses/Pyrogen/Pyrogen_Phase1";

        public static Asset<Texture2D> Phase2Texture;
        public static Asset<Texture2D> AdditiveTexture;
        public static Asset<Texture2D> AdditiveTexture2;
        public static Asset<Texture2D> BloomTexture;
        public static Asset<Texture2D> BloomTexture2;
        public static Asset<Texture2D> RingTexture;
        public static Asset<Texture2D> RingBloomTexture;
        public static Asset<Texture2D> Glowmask;
        public static Asset<Texture2D> Glowmask2;
        public static Asset<Texture2D> BloomExtra;

        public static Asset<Texture2D> Phase1TextureC;
        public static Asset<Texture2D> Phase2TextureC;
        public static Asset<Texture2D> AdditiveTextureC;
        public static Asset<Texture2D> AdditiveTexture2C;
        public static Asset<Texture2D> BloomTextureC;
        public static Asset<Texture2D> BloomTexture2C;
        public static Asset<Texture2D> RingTextureC;
        public static Asset<Texture2D> RingBloomTextureC;
        public static Asset<Texture2D> GlowmaskC;
        public static Asset<Texture2D> Glowmask2C;
        public static Asset<Texture2D> BloomExtraC;

        public static int cryoIconIndex;
        public static int pyroIconIndex;
        public float lifeRatio => NPC.life / (float)NPC.lifeMax;
        public static float BaseDifficultyScale => MathHelper.Clamp((CalamityMod.Events.BossRushEvent.BossRushActive.ToInt() * 3 + CalamityWorld.revenge.ToInt() + Main.expertMode.ToInt()) * 0.5f, 0f, 1f);

        public bool phase2 = false;
        public bool phase3 = false;
        public bool fullyHealed = false;

        internal static void LoadHeadIcons()
        {
            string pyroIconPath = "CalRemix/Content/NPCs/Bosses/Pyrogen/Pyrogen_Phase1_Head_Boss";
            string cryoIconPath = "CalamityMod/NPCs/Cryogen/Cryogen_Phase1_Head_Boss";

            CalRemix.instance.AddBossHeadTexture(pyroIconPath, -1);
            pyroIconIndex = ModContent.GetModBossHeadSlot(pyroIconPath);

            CalRemix.instance.AddBossHeadTexture(cryoIconPath, -1);
            cryoIconIndex = ModContent.GetModBossHeadSlot(cryoIconPath);

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyrogen");
            if (!Main.dedServ)
            {
                Phase2Texture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Pyrogen_Phase2", AssetRequestMode.AsyncLoad);
                AdditiveTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenAdditive", AssetRequestMode.AsyncLoad);
                AdditiveTexture2 = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenAdditive2", AssetRequestMode.AsyncLoad);
                BloomTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenBloom1", AssetRequestMode.AsyncLoad);
                BloomTexture2 = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenBloom2", AssetRequestMode.AsyncLoad);
                RingTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenRing", AssetRequestMode.AsyncLoad);
                RingBloomTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenRingAdditive", AssetRequestMode.AsyncLoad);
                Glowmask = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Pyrogen_Phase1_Glow", AssetRequestMode.AsyncLoad);
                Glowmask2 = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Pyrogen_Phase2_Glow", AssetRequestMode.AsyncLoad);
                BloomExtra = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/PyrogenBloomExtra", AssetRequestMode.AsyncLoad);

                Phase1TextureC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/Pyrogen_Phase1", AssetRequestMode.AsyncLoad);
                Phase2TextureC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/Pyrogen_Phase2", AssetRequestMode.AsyncLoad);
                AdditiveTextureC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenAdditive", AssetRequestMode.AsyncLoad);
                AdditiveTexture2C = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenAdditive2", AssetRequestMode.AsyncLoad);
                BloomTextureC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenBloom1", AssetRequestMode.AsyncLoad);
                BloomTexture2C = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenBloom2", AssetRequestMode.AsyncLoad);
                RingTextureC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenRing", AssetRequestMode.AsyncLoad);
                RingBloomTextureC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenRingAdditive", AssetRequestMode.AsyncLoad);
                GlowmaskC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/Pyrogen_Phase1_Glow", AssetRequestMode.AsyncLoad);
                Glowmask2C = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/Pyrogen_Phase2_Glow", AssetRequestMode.AsyncLoad);
                BloomExtraC = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/PyrogenBloomExtra", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 200;
            NPC.width = 86;
            NPC.height = 88;
            NPC.defense = 60;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(161300, 187850, 675000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(3, 5, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToCold = true;
            rotationIncrement = 0.0246399424 * 0.3 * 15;
            if (!Main.dedServ)
                if (!Main.zenithWorld) Music = CalRemixMusic.Pyrogen;
                else Music = CalRemixMusic.Cryogen;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(rotationDirection);
            writer.Write(rotation);
            writer.Write(phase2);
            writer.Write(phase3);
            writer.Write(enrageCounter);
            writer.Write(biomeEnrageTimer);
            writer.Write(enrageCounter);

        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            rotation = reader.ReadDouble();
            rotationDirection = reader.ReadInt32();
            phase2 = reader.ReadBoolean();
            phase3 = reader.ReadBoolean();
            enrageCounter = reader.ReadInt32();
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 15; i++)
            {
                int bitSprite = i % 6;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PyrogenShield>(), ai0: NPC.whoAmI, ai1: i, ai2: bitSprite);  
            }
            phase2 = false; //weird bug fix
            phase3 = false; // ???
        }

        public override bool CheckDead()
        {
            //if (NPC.life < 1) {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {

                NPC.netUpdate = true;
                if (phase3)
                {
                }
                else if (phase2)
                {
                    HandlePhaseTransition(true);
                    NPC.life = 10;
                }
                else
                {
                    HandlePhaseTransition(false);
                    NPC.life = 10;
                }
            }
            return false;
        }

        public override void BossHeadSlot(ref int index)
        {
            if (Main.zenithWorld)
                index = Cryogen.cryoIconIndex;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            Player player = Main.player[NPC.target];
            if (FireDrawer != null)
                FireDrawer.Update();
            if (Target == null || Target.dead)
            {
                if (!player.active || player.dead || Vector2.Distance(player.Center, NPC.Center) > 5600f)
                {
                    NPC.rotation = (NPC.rotation * rotationMult + NPC.velocity.X * rotationAmt) / 10f;

                    NPC.velocity.Y += 1;

                    if (NPC.timeLeft > 60)
                        NPC.timeLeft = 60;

                    if (NPC.ai[0] != 0f)
                    {
                        NPC.ai[0] = 0f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;

                        NPC.netUpdate = true;

                        if (NPC.netSpam >= 10)
                            NPC.netSpam = 9;
                    }

                    return;
                }
                return;
            }

            int torch = Main.zenithWorld ? TorchID.Ice : TorchID.Red;   
            Lighting.AddLight(NPC.Center, torch);

            if (!player.ZoneUnderworldHeight && !ultraEnraged)
            {
                if (biomeEnrageTimer > 0)
                    biomeEnrageTimer--;
            }
            else
                biomeEnrageTimer = CalamityMod.NPCs.CalamityGlobalNPC.biomeEnrageTimerMax;

            if (biomeEnrageTimer <= 0 && !biomeEnraged) {
                biomeEnraged = true;
                SoundEngine.PlaySound(EnrageSound, NPC.Center);
            }


            if (biomeEnraged && (!player.ZoneUnderworldHeight))
                enrageCounter = 4;

            if (enrageCounter == 0 && biomeEnraged)
            {
                biomeEnraged = false;
                biomeEnrageTimer = CalamityMod.NPCs.CalamityGlobalNPC.biomeEnrageTimerMax;
            }

            switch (Phase)
            {
                case (int)PyroPhaseType.Idle: //freshly spawned and right after transitioning to phase 2; attempt to move into the player with some speed, but not an unavoidable amount
                    {
                        int tpDistX = 1000;
                        int tpDistY = 500;
                        int IdleTimer = 300;
                        NPC.ai[1]++;
                        AttackTimer++;

                        player = Main.player[NPC.target];
                        Vector2 pyrogenCenter = new Vector2(NPC.Center.X, NPC.Center.Y);
                        float playerXDist = player.Center.X - pyrogenCenter.X;
                        float playerYDist = player.Center.Y - pyrogenCenter.Y;
                        float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                        float pyrogenSpeed = CalamityWorld.revenge ? 10f : 7f;
                        pyrogenSpeed += 4f;

                        playerDistance = pyrogenSpeed / playerDistance;
                        playerXDist *= playerDistance;
                        playerYDist *= playerDistance;

                        float inertia = 25f;

                        NPC.velocity.X = (NPC.velocity.X * inertia + playerXDist) / (inertia + 1f);
                        NPC.velocity.Y = (NPC.velocity.Y * inertia + playerYDist) / (inertia + 1f);
                        NPC.rotation = NPC.velocity.X * 0.1f;

                        if (NPC.ai[1] == 180)
                        {
                            teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                        }
                        if (NPC.ai[1] > 180)
                        {
                            int dustType = Main.zenithWorld ? DustID.SnowflakeIce : DustID.FlameBurst;
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, dustType);
                                Main.dust[d].noGravity = true;
                            }
                        }
                        if (NPC.ai[1] > 240)
                        {
                            DustExplosion();
                            NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                            DustExplosion();
                            NPC.ai[1] = 0;
                        }
                        if (AttackTimer > IdleTimer)
                        {
                            SelectNextAttack();
                            AttackTimer = 0;
                        }
                        break;
                    }
               case (int)PyroPhaseType.Charge: //charges; alternates between predictive and not randomly, predictive chance goes up with difficulty, ALMOST certain on death mode
                    {
                        {
                            AttackTimer++;
                            int chargeDelaySub = death ? 30 : rev ? 35 : 40; //pause before dash length
                            float chargeLimit = death ? 6 : rev ? 5 : 4;
                            float chargeVelocity = 35; //stays the same

                            if (enrageCounter > 0)
                            {
                                chargeDelaySub = (int)(chargeDelaySub * 0.8f);
                            }

                            player = Main.player[NPC.target];
                            Vector2 pyrogenCenter = new Vector2(NPC.Center.X, NPC.Center.Y);
                            float playerXDist = player.Center.X - pyrogenCenter.X;
                            float playerYDist = player.Center.Y - pyrogenCenter.Y;
                            float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                            float pyrogenSpeed = 1;

                            playerDistance = pyrogenSpeed / playerDistance;
                            playerXDist *= playerDistance;
                            playerYDist *= playerDistance;

                            float inertia = 25f;

                            Vector2 predictiveVector = player.Center + player.velocity * 20f - NPC.Center;

                            if (AttackTimer == 2) //reset position immediately to prep charge
                            {
                                NPC.velocity.X *= 0.5f;
                                NPC.velocity.Y *= 0.5f;
                            }
                            if (AttackTimer == 3)
                            {
                                SafeTeleport();
                            }

                            if (AttackTimer % 6 == 0 && AttackTimer < chargeDelaySub)
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemInfernoFork with { Pitch = 1f }, Target.Center);
                            }
                            //decide what kind of dash to use
                            if (AttackTimer >= 3 && AttackTimer < chargeDelaySub)
                            {
                                int chargeType = Main.rand.Next(10); // > 5 = non predictive
                                if (CalamityWorld.revenge) chargeType = Main.rand.Next(8);
                                if (CalamityWorld.death) chargeType = Main.rand.Next(6); //JUST often enough to screw you occasionally
                                switch (chargeType)
                                {
                                    case < 5:
                                        predictiveCharge = true;
                                        NPC.rotation += 0.5f;
                                        NPC.rotation *= 1.4f;
                                        break;
                                    case > 5:
                                        predictiveCharge = false;
                                        break;
                                    default:
                                        predictiveCharge = true;
                                        NPC.rotation += 0.5f;
                                        NPC.rotation *= 1.4f;
                                        break;
                                }
                            }

                            //then, wait approx. a second...
                            if (AttackTimer == chargeDelaySub) //... and dash
                            {
                                if (predictiveCharge) {
                                    NPC.velocity = Vector2.Normalize(predictiveVector) * chargeVelocity;
                                    NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
                                    SoundEngine.PlaySound(ChargeSound with { Pitch = 0.4f }, Target.Center);
                                    charges++;
                                }
                                else
                                {
                                    NPC.velocity = NPC.DirectionTo(Target.Center) * chargeVelocity;
                                    NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
                                    SoundEngine.PlaySound(ChargeSound with { Pitch = 0.2f }, Target.Center);
                                    charges++;
                                    break;
                                }                         
                            }

                            if (AttackTimer == 120) //finished dash! check what to do next...
                            {
                                NPC.velocity.X *= 0.7f; //slow down just in case onscreen
                                NPC.velocity.Y *= 0.7f;
                                int dustType = Main.zenithWorld ? DustID.IceTorch : DustID.FlameBurst;
                                if (charges >= chargeLimit) //dashed enough times! switch attacks...
                                {
                                    int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, dustType);
                                    Main.dust[d].noGravity = true;
                                    DustExplosion();
                                    NPC.velocity.X = 0f;
                                    NPC.velocity.Y = 0f;
                                    Vector2 tpos = Main.rand.NextVector2Circular(222, 222);
                                    NPC.position = Target.Center + tpos.SafeNormalize(Vector2.Zero) * 600;
                                    DustExplosion();
                                    NPC.ai[1] = 0;
                                    SelectNextAttack();
                                    NPC.netUpdate = true;
                                }
                                else //still has dashes left, reset!
                                {
                                    AttackTimer = 0;
                                    NPC.netUpdate = true;
                                }
                            }
                                NPC.rotation = NPC.velocity.X * 0.1f;

                            if (AttackTimer < chargeDelaySub && AttackTimer >= 3)
                                for (int i = 0; i < 10; i++)
                                {
                                    Vector2 predictiveVectore = predictiveCharge ? predictiveVector + NPC.Center : Target.Center;
                                    Particle spark = new SquareParticle(NPC.Center + Main.rand.NextVector2Square(-200, 200), NPC.DirectionTo(predictiveVectore) * NPC.Distance(predictiveVectore) / 10f, false, 30, Main.rand.NextFloat(3, 4), (predictiveCharge ? Color.Red : Color.Orange) * 0.9f);
                                    if (Main.zenithWorld)
                                    {
                                        spark = new SnowflakeSparkle(NPC.Center + Main.rand.NextVector2Square(-200, 200), NPC.DirectionTo(predictiveVectore) * NPC.Distance(predictiveVectore) / 10f, (predictiveCharge ? Color.Blue : Color.LightBlue), Color.White, Main.rand.NextFloat(3, 4), 30);
                                    }
                                    GeneralParticleHandler.SpawnParticle(spark);
                                }
                        }
                        break;
                    }
                case (int)PyroPhaseType.Rain: //moves above the player, rains projectiles down in an even spread; twice in phase 2
                {
                        {
                            AttackTimer++;
                            int stop = 90;
                            int stop2 = 170;
                            int dash = 91;
                            int flareRate = 10;
                            bool canShootFlares = false;
                            int endPhase = 170;

                            if (AttackTimer == 1)
                            {
                                SafeTeleport();
                            }
                            if (revenge)
                            {
                                stop -= 5;
                                flareRate = 7;
                            }
                            if (death)
                            {
                                stop -= 5;
                                flareRate = 5;
                            }

                            if (enrageCounter > 0)
                            {
                                flareRate = (int)(flareRate * 0.5f);
                            }

                            if (AttackTimer < stop)
                            {
                                Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X - 600, Main.player[NPC.target].Center.Y - 400); //head for top left
                                Vector2 distanceFromDestination = playerpos - NPC.Center;
                                CalamityUtils.SmoothMovement(NPC, 60, distanceFromDestination, Main.getGoodWorld ? 35 : 30, 1, true);
                            }

                            if (AttackTimer == stop || AttackTimer == stop2)
                            {
                                NPC.velocity *= 0.8f;
                            }

                            if (AttackTimer > dash)
                            {
                                NPC.ai[1]++;
                            }

                            if (AttackTimer > dash && AttackTimer < stop2)
                            {
                                Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X + 600, Main.player[NPC.target].Center.Y - 400); //and then top right
                                Vector2 distanceFromDestination = playerpos - NPC.Center;
                                CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, Main.getGoodWorld ? 35 : 30, 1, true);
                                int type = Main.zenithWorld ? ModContent.ProjectileType<IceBomb>() : ModContent.ProjectileType<PyrogenFlare>();
                                if (NPC.ai[1] % flareRate == 0)
                                {
                                    SoundEngine.PlaySound(FlareSound, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, type, (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                                }
                            }

                            if (NPC.ai[1] >= endPhase)
                            {
                                canShootFlares = false;
                                SelectNextAttack();
                            }
                        }
                        break;
                }
                case (int)PyroPhaseType.PullintoShield: //extends shield out, tugs player in with it, then fires some slow-moving projectiles in the limited space; repel player again at end
                {
                        {
                            AttackTimer++;
                            NPC.velocity = Vector2.Zero;
                            float distanceRequired = 2000f;
                            bool masterRage = enrageCounter > 0 || master;

                            if (AttackTimer == 1)
                            {
                                ClearingTeleport();
                                CalamityUtils.KillAllHostileProjectiles();
                            }

                            // shoot the chain at the player
                            if (AttackTimer == 1)
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemBeesKnees with { Pitch = 0.4f }, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    foreach (Player playere in Main.ActivePlayers)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Target.Center), ModContent.ProjectileType<PyrogenHarpoon>(), 0, 0, Main.myPlayer, NPC.whoAmI, playere.whoAmI, 1);
                                        Projectile proj = Main.projectile[p];
                                        proj.localAI[1] = 30;
                                    }
                                }
                            }

                            float randomVariance = NPC.ai[1];
                            int dmg = 100;

                            bool firing = AttackTimer % 60 < 50 && AttackTimer % 90 > 50;
                            if (NPC.Calamity().newAI[0] == 0)
                            {
                                if (AttackTimer > BlackholeSafeTime * 2 && AttackTimer % 5 == 0 && firing)
                                {
                                    int firePoints = masterRage ? 12 : 8;
                                    int fireProjSpeed = masterRage ? 6 : 4;
                                    float variance = MathHelper.TwoPi / firePoints;
                                    SoundEngine.PlaySound(BetterSoundID.ItemFireball, NPC.Center);
                                    int type = Main.zenithWorld ? ModContent.ProjectileType<IceBomb>() : ModContent.ProjectileType<PyrogenWaveFlare>();
                                    for (int i = 0; i < firePoints; i++)
                                    {
                                        Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                        velocity = velocity.RotatedBy(variance * i + randomVariance);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, type, dmg, 0);
                                    }
                                }
                            }
                            else if (NPC.Calamity().newAI[0] == 1)
                            {
                                if (AttackTimer > BlackholeSafeTime * 2 && AttackTimer % 10 == 0 && firing)
                                {
                                    SoundEngine.PlaySound(BetterSoundID.ItemFireball, NPC.Center);
                                    int dir = Main.rand.NextBool().ToInt();
                                    int totalObjects = masterRage ? 16 : 10;
                                    int type = Main.zenithWorld ? ModContent.ProjectileType<IceBomb>() : ModContent.ProjectileType<PyrogenOrbitalFlare>();
                                    for (int i = 0; i < totalObjects; i++)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, type, dmg, 0, Main.myPlayer, i + 1, totalObjects, Main.rand.NextFloat(0, 4f));
                                    }
                                }
                            }
                            else
                            {
                                if (AttackTimer > BlackholeSafeTime * 2 && AttackTimer % 5 == 0 && firing)
                                {
                                    int firePoints = masterRage ? 12 : 8;
                                    int fireProjSpeed = masterRage ? 6 : 4; 
                                    float variance = MathHelper.TwoPi / firePoints;
                                    SoundEngine.PlaySound(BetterSoundID.ItemFireball, NPC.Center);
                                    int type = Main.zenithWorld ? ModContent.ProjectileType<IceBomb>() : ModContent.ProjectileType<PyrogenZigZagFlare>();
                                    for (int i = 0; i < firePoints; i++)
                                    {
                                        Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                        velocity = velocity.RotatedBy(variance * i + randomVariance);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, type, dmg, 0);
                                    }
                                }
                            }

                            // Controls variance for bullet patterns
                            if (!firing)
                            {
                                NPC.ai[1] = Main.rand.NextFloat(-22.22f, 22.22f);
                                NPC.Calamity().newAI[0] = Main.rand.Next(0, 3);
                            }

                            if (AttackTimer >= 730) //all done!
                            {
                                SelectNextAttack();
                                CalamityUtils.KillAllHostileProjectiles();
                            }
                            InfiniteFlight();
                            if (AttackTimer > BlackholeSafeTime * 2)
                            foreach (Player victim in Main.ActivePlayers)
                            {
                                distanceRequired = 630f;
                                float distance = Vector2.Distance(victim.Center, NPC.Center);
                                if (distance > distanceRequired)
                                {
                                    Vector2 dir = NPC.DirectionTo(victim.Center);
                                    victim.position = NPC.Center + dir * 600;
                                }
                            }
                        }
                    break;
                }
                case (int)PyroPhaseType.FireWall: //stays stationary, throws out chain to drag player in while spawning fire walls
                {
                        int shootChain = 60; // when to shoot the chain
                        int waveTime = 300; // how long each sequence of fireballs lasts
                        int waitTime = 35; // how long it should wait before starting a sequence
                        int waveInterval = 70; // how often fireballs should be shot
                        int waveAmount = 2; // total amount of sequences
                        int attackTime = shootChain + (waveTime + waitTime) * waveAmount; // total attack time
                        int withdrawChain = attackTime - 120; // when to withdraw the chain
                        int projectileAmount = 40; // how many fireballs make up the walls
                        float projectileSpeed = 18; // speed of fireballs
                        float shotSpacing = 60; // amount of pixels between each fireball's position
                        float spawnDistX = 1200; // how far the fireballs spawn away from Pyrogen
                        int hookHitTime = 30; // how long it takes for the hook to hit the player
                        int safeableSpawnRange = 4; // the range centered on the middle at which one fireball will be removed for the player to move through
                        NPC.velocity = Vector2.Zero;
                        NPC.rotation = 0;
                        AttackTimer++;

                        if (enrageCounter > 0)
                        {
                            waveInterval = (int)(waveInterval * 0.5f);
                        }

                        if (AttackTimer == 1)
                        {
                            ClearingTeleport();
                        }

                        foreach (Player victim in Main.ActivePlayers) //kills platforms for the duration of the attack
                        {
                            if (victim.Calamity() != null)
                            {
                                victim.AddBuff(ModContent.BuffType<Deplatformed>(), 1);
                            }
                        }

                        // shoot the chain at the player
                        if (AttackTimer == shootChain)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemBeesKnees with { Pitch = 0.4f }, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                foreach (Player playere in Main.ActivePlayers)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Target.Center), ModContent.ProjectileType<PyrogenHarpoon>(), 0, 0, Main.myPlayer, NPC.whoAmI, playere.whoAmI, 0);
                                    Projectile proj = Main.projectile[p];
                                    proj.localAI[1] = withdrawChain;
                                }
                            }
                        }
                        InfiniteFlight();
                        // the fireball which will never be shot, never to hit the player, never to have any dreams, never t
                        int noFire = Main.rand.Next(-safeableSpawnRange / 2, safeableSpawnRange);
                        bool startAttacking = AttackTimer > (shootChain + hookHitTime);
                        bool notOnCooldown = (((AttackTimer - (shootChain + hookHitTime)) % (waveTime + waitTime)) < waveTime - waitTime);
                        // spawn fireball firewalls
                        if (AttackTimer % waveInterval == 0 && startAttacking && notOnCooldown)
                        {
                            SoundEngine.PlaySound(CommonCalamitySounds.SwiftSliceSound, Target.Center);
                            for (int i = -(projectileAmount / 2); i < (projectileAmount / 2); i++)
                            {
                                if (i != noFire)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        bool goRight = NPC.DirectionTo(Target.Center).X.DirectionalSign() == 1;
                                        Vector2 spawnPos = new Vector2(NPC.Center.X + (goRight ? -spawnDistX : spawnDistX), Target.Center.Y + i * shotSpacing);
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, Vector2.UnitX * NPC.DirectionTo(Target.Center).X.DirectionalSign() * projectileSpeed, ModContent.ProjectileType<PyrogenTool>(), 100, 0);
                                    }
                                }
                            }
                        }

                        if (AttackTimer >= attackTime)
                        {
                            SelectNextAttack();
                            SafeTeleport();
                            CalamityUtils.KillAllHostileProjectiles();
                            AttackTimer = 0;
                        }

                    break;
                }
                case (int)PyroPhaseType.PonceSpin: //spins around the player and leaves stationary fireballs to trap the player in, then relentlessly pursues them while leaving a trail that forces awkward movement
                {
                        int flareRate = 10;
                        int timeToCharge = 300;
                        int chargingTime = 0;
                        float chargeVelocity = 35; //stays the same
                        AttackTimer++;

                        if (enrageCounter > 0)
                        {
                            flareRate = (int)(flareRate * 0.8f);
                        }

                        if (AttackTimer == 1)
                        {
                            rotationDirection = rotationDirection == 1 ? -1 : 1;
                        }


                        if (AttackTimer == 10)
                        {
                            int dustType = Main.zenithWorld ? DustID.SnowflakeIce : DustID.FlameBurst;
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, dustType);
                                Main.dust[d].noGravity = true;
                                DustExplosion();
                                NPC.velocity.X = 0f;
                                NPC.velocity.Y = 0f;
                                NPC.position = new Vector2(Target.Center.X, Target.Center.Y - 400);
                                DustExplosion();
                                NPC.ai[1] = 0;
                            }
                        }
                        if (AttackTimer >= 15 && AttackTimer <= 70) //create a border of flares
                        {
                            int spintimer = 65;

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                 NPC.Center = player.Center + new Vector2(500, 0).RotatedBy(rotation);
                            }
                            foreach (Player victim in Main.ActivePlayers) //LITERALLY cannot be assed to figure this out so you just can't move during the ring setup
                            {
                                victim.velocity = Vector2.Zero;
                            }
                            NPC.Center = player.Center + Vector2.UnitY.RotatedBy(rotation) * 500;
                            int type = Main.zenithWorld ? ModContent.ProjectileType<IceBlast>() : ModContent.ProjectileType<PyrogenFlareStatic>();
                            rotation += rotationIncrement * rotationDirection;
                                if (AttackTimer % flareRate == 0)
                                SoundEngine.PlaySound(FlareSound, NPC.Center);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, type, (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                        }

                        if (AttackTimer > 70)
                        {
                            flareRate = 20;
                            player = Main.player[NPC.target];
                            Vector2 pyrogenCenter = new Vector2(NPC.Center.X, NPC.Center.Y);
                            float playerXDist = player.Center.X - pyrogenCenter.X;
                            float playerYDist = player.Center.Y - pyrogenCenter.Y;
                            float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                            float pyrogenSpeed = CalamityWorld.revenge ? 10f : 7f;
                            pyrogenSpeed += Main.getGoodWorld ? 8f : 6f;

                            playerDistance = pyrogenSpeed / playerDistance;
                            playerXDist *= playerDistance;
                            playerYDist *= playerDistance;

                            float inertia = 25f;

                            NPC.velocity.X = (NPC.velocity.X * inertia + playerXDist) / (inertia + 1f);
                            NPC.velocity.Y = (NPC.velocity.Y * inertia + playerYDist) / (inertia + 1f);
                            NPC.rotation = NPC.velocity.X * 0.1f;
                            if (AttackTimer % flareRate == 0)
                            {
                                int type = Main.zenithWorld ? ModContent.ProjectileType<IceBomb>() : ModContent.ProjectileType<PyrogenFlareStatic2>();
                                SoundEngine.PlaySound(FlareSound, NPC.Center);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, type, (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                            }
                        }
                        InfiniteFlight();
                        if (AttackTimer >= 700)
                        {
                            SafeTeleport();
                            CalamityUtils.KillAllHostileProjectiles();
                            SelectNextAttack();
                        }
                        break;
                }
                case (int)PyroPhaseType.FireChain: // Shoot a hostile chain that spawns fireballs
                {
                        int attackTime = 360; // how long the attack lasts
                        int shootHook = 120; // when to shoot the hook
                        float minDist = 600; // the minimum distance the hook can be shot out
                        float distMultiplier = 0.8f; // how far the hook shoots out relative to the player
                        float hookDist = MathHelper.Max(Target.Distance(NPC.Center) * distMultiplier, minDist); // the final distance the hook shoots
                        int moveSpeed = 4; // speed Pyrogen moves

                        if (enrageCounter > 0)
                        {
                            moveSpeed = 8;
                        }

                        AttackTimer++;

                        if (AttackTimer == 1)
                        {
                            ClearingTeleport();
                        }
                        // shoot the hook at the player
                        if (AttackTimer == shootHook)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemBeesKnees with { Pitch = 0.4f }, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                foreach (Player playere in Main.ActivePlayers)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Target.Center), ModContent.ProjectileType<PyrogenHarpoon>(), 0, 0, Main.myPlayer, NPC.whoAmI, playere.whoAmI, 2);
                                    Projectile proj = Main.projectile[p];
                                    proj.localAI[2] = hookDist; 
                                }
                            }
                        }

                        // move
                        NPC.velocity = NPC.SafeDirectionTo(Target.Center) * moveSpeed;


                        if (AttackTimer > attackTime)
                        {
                            CalamityUtils.KillAllHostileProjectiles();
                            SelectNextAttack();
                            SafeTeleport();
                        }
                        if (AttackTimer < shootHook)
                            for (int i = 0; i < 20; i++)
                            {
                                Vector2 predictiveVector = Vector2.Zero;
                                Vector2 predictiveVectore = predictiveCharge ? predictiveVector + NPC.Center : Target.Center;
                                SquareParticle spark = new SquareParticle(NPC.Center + Main.rand.NextVector2Square(-22, 22), NPC.DirectionTo(predictiveVectore) * NPC.Distance(predictiveVectore) / 10f, false, 30, Main.rand.NextFloat(3, 4), (predictiveCharge ? Color.Red : Color.Orange) * 0.9f);
                                GeneralParticleHandler.SpawnParticle(spark);
                            }

                        break;
                }
                case (int)PyroPhaseType.Hellstorm: //spins really fast, pulling projectiles and the player in, then fires a shrapnel bomb at the end of the attack duration; 50 dr
                {
                        InfiniteFlight();
                        Hellstorm();
                    break;
                }
                case (int)PyroPhaseType.HellstormFatal: //above, far more intense, boss gradually loses health until dead; 100% dr
                {
                        NPC.life -= (int)(NPC.lifeMax * 0.001f);
                        NPC.dontTakeDamage = true;
                        if (NPC.life <= 0)
                        {
                            CalamityUtils.KillAllHostileProjectiles();
                            DustExplosion2();
                            NPC.life = 0;
                            NPC.HitEffect();
                            NPC.NPCLoot();
                            NPC.active = false;
                            NPC.netUpdate = true;
                            NPC.justHit = true;
                            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pyrogen1").Type, 1f);
                            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pyrogen2").Type, 1f);
                            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pyrogen3").Type, 1f);
                            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pyrogen4").Type, 1f);
                            SoundStyle sound = DeathSound;
                            SoundEngine.PlaySound(sound, NPC.Center);
                        }
                        InfiniteFlight();
                        Hellstorm(true);
                    break;
                }
                case (int)PyroPhaseType.Transitioning: //switching to phase 2; do absolutely nothing until finished
                    {
                        deathTimer++;
                        NPC.velocity.X *= 0.5f;
                        NPC.velocity.Y *= 0.5f;
                        NPC.rotation = NPC.velocity.X * 0.5f;

                        if (deathTimer == 1 && !phase3)
                        {
                            if (Main.netMode != NetmodeID.Server)
                            {
                                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * 16, Mod.Find<ModGore>("PyrogenDoor").Type);
                            }
                        }

                        NPC.life = (int)MathHelper.Lerp(1, NPC.lifeMax, Utils.GetLerpValue(60, 200, deathTimer, true));

                        if (deathTimer >= 200)
                        { //ensure health is 100% filled just in case
                            if (NPC.life < NPC.lifeMax)
                            {
                                NPC.life = NPC.lifeMax;

                            }
                            NPC.dontTakeDamage = false;
                            deathTimer = 0;
                            if (phase3)
                            {
                                AIState = PyroPhaseType.HellstormFatal;
                            }
                            else if (phase2)
                            {
                                AIState = PyroPhaseType.Idle;
                            }

                        }
                        break;
                    }
            }

            if (enrageCounter > 0)
            {
                enrageCounter--;
            }
            base.AI();
        }

        public static void InfiniteFlight()
        {
            foreach (Player victim in Main.ActivePlayers)
            {
                if (victim.Calamity() != null)
                {
                    victim.Calamity().infiniteFlight = true;
                }
            }
        }

        public void Hellstorm(bool end = false)
        {
            int startAbsorbing = 60; // when projectiles should start appearing
            int stopAbsorbing = 360; // when projectiles should stop appearing
            int fireRate = 5; // how often projectiles are spawned
            int shootBombGate = stopAbsorbing + 60; // when does the bomb appear
            int endAttack = shootBombGate + 300; // when the attack ends
            int maxSpawnRad = 2000; // max dist projectiles can spawn
            int minSpawnRad = 1800; // min dist projectiles can spawn
            int projSpeed = 12; // speed of projectiles
            int bombProjAmount = 22; // how many projectiles the bomb explodes into

            // for desperation, spawn twice as many projectiles
            if (end || enrageCounter > 0)
            {
                fireRate /= 2;
            }

            NPC.Calamity().DR = 0.6f;
            NPC.velocity = Vector2.Zero;
            NPC.rotation = 0;

            foreach (Player victim in Main.ActivePlayers) //kills platforms for the duration of the attack
            {
                if (victim.Calamity() != null)
                {
                    victim.AddBuff(ModContent.BuffType<Deplatformed>(), 1);
                }
            }

            if (AttackTimer == 1)
            {
                ClearingTeleport();
            }

            // spawn projectiles around the player if he should start absorbing and before he should either stop absorbing or infinite if in desperation
            if (AttackTimer > startAbsorbing && (AttackTimer < stopAbsorbing || end))
            {
                if (AttackTimer % fireRate == 0)
                {
                    Vector2 rand = Main.rand.NextVector2Square(-22, 22);
                    Vector2 norm = rand.SafeNormalize(Vector2.Zero);
                    Vector2 pos = NPC.Center + norm * Main.rand.Next(minSpawnRad, maxSpawnRad);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, pos.DirectionTo(NPC.Center) * projSpeed, ModContent.ProjectileType<ObsidianFragment>(), 100, 0, Main.myPlayer, Main.rand.Next(0, 6));
                    }
                }
            }

            // only start ticking down to the bomb when no more fragments are left
            if ((!(AttackTimer == stopAbsorbing && CalamityUtils.AnyProjectiles(ModContent.ProjectileType<ObsidianFragment>()))) || end)
            {
                AttackTimer++;
            }

            // spawn the bomb
            if (!end && AttackTimer == shootBombGate)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemGrenadeExplosion with { Pitch = -0.2f, Volume = 2 }, NPC.Center);
                SoundEngine.PlaySound(BetterSoundID.ItemMeteorImpact with { Pitch = -0.2f, Volume = 2 }, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Target.Center) * 10, ModContent.ProjectileType<ObsidianBomb>(), 200, 0, Main.myPlayer, bombProjAmount);
                }
                NPC.position = NPC.position - NPC.DirectionTo(Target.Center) * 40;
            }

            // eat shards that come in contact
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == ModContent.ProjectileType<ObsidianFragment>() && p.ai[1] == 0)
                {
                    if (p.Hitbox.Intersects(NPC.Hitbox))
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemExplosion, NPC.Center);
                        for (int i = 0; i < 25; i++)
                        {
                            int d = Dust.NewDust(p.position, p.width, p.height, DustID.Obsidian, Scale: Main.rand.NextFloat(1f, 2f));
                            Main.dust[d].velocity = (Main.dust[d].position - p.Center).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(2, 4);
                            Main.dust[d].noGravity = true;
                        }
                        p.Kill();
                    }
                }
            }

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.active && !p.dead)
                {
                    p.velocity = Vector2.Lerp(p.velocity, p.velocity + p.SafeDirectionTo(NPC.Center) * 3, Utils.GetLerpValue(200, 8000, p.Distance(NPC.Center), true));
                }
            }

            // end the attack when it's not desperation
            if (AttackTimer > endAttack && !end)
            {
                SelectNextAttack();
                CalamityUtils.KillAllHostileProjectiles();
            }
        }



        public void SelectNextAttack()
        {
            if (AIState != PyroPhaseType.Idle)
                attackTotal++;

            // cycle through attacks in order, entirely random in phase 2

            if (!phase2)
            {
                switch (AIState)
                {
                    case PyroPhaseType.Idle:
                        AIState = PyroPhaseType.Charge;
                        break;

                    case PyroPhaseType.Charge:
                        AIState = PyroPhaseType.Rain;
                        break;

                    case PyroPhaseType.Rain:
                        AIState = PyroPhaseType.PullintoShield;
                        break;

                    case PyroPhaseType.PullintoShield:
                        AIState = PyroPhaseType.FireWall;
                        break;

                    case PyroPhaseType.FireWall:
                        AIState = PyroPhaseType.Idle;
                        break;

                    case PyroPhaseType.Hellstorm:
                        AIState = PyroPhaseType.Idle;
                        break;
                }
            }
            else
            {
                int choice = Main.rand.Next(6);
                switch (choice)
                {
                    case 0:
                        AIState = PyroPhaseType.Charge;
                        break;
                    case 1:
                        AIState = PyroPhaseType.Rain;
                        break;
                    case 2:
                        AIState = PyroPhaseType.PullintoShield;
                        break;
                    case 3:
                        rotation = MathHelper.ToRadians(Main.rand.Next(360));
                        if (Main.player[NPC.target].velocity.X > 0)
                            rotationDirection = 1;
                        else if (Main.player[NPC.target].velocity.X < 0)
                            rotationDirection = -1;
                        else
                            rotationDirection = Main.player[NPC.target].direction;
                        AIState = PyroPhaseType.PonceSpin;
                        break;
                    case 4:
                        AIState = PyroPhaseType.FireWall;
                        break;
                    case 5:
                        AIState = PyroPhaseType.FireChain;
                        break;
                    default:
                        AIState = PyroPhaseType.Idle;
                        break;
                }

            }

            if (attackTotal == 4)
            {
                AIState = PyroPhaseType.Hellstorm; 
                attackTotal = 0;
            }

            NPC.ai[1] = 0;
            AttackTimer = 0f;
            charges = 0;
            NPC.damage = 200;
            NPC.Calamity().DR = 0.3f;
            NPC.netUpdate = true;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == ModContent.NPCType<PyrogenShield>() && n.ai[0] == NPC.whoAmI)
                {
                    n.localAI[1] = 0;
                }
            }
        }

        public void SafeTeleport(int maxRad = 2000, int avoidRad = 1500) //instantly teleport somewhere, but never get too close to the player when doing so; bias towards being offscreen
        {
            int tpDistX = maxRad;
            int tpDistY = maxRad;
            for (int i = 0; i < 120; i++) //tries 120 times to find an eligible position; loop breaks if it somehow fails for that long consecutively
            {
                teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                playerRadius = new Rectangle((int)Target.Center.X, (int)Target.Center.Y, avoidRad, avoidRad);
                if (!teleportPos.Intersects(playerRadius))
                {
                    break;
                }
                else continue;
            }

            int dustType = Main.zenithWorld ? DustID.SnowflakeIce : DustID.FlameBurst;
            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, dustType);
                Main.dust[d].noGravity = true;
                DustExplosion();
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
                NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                DustExplosion();
                NPC.ai[1] = 0;
            }
        }

        public void ClearingTeleport()
        {
            NPC.damage = 0;
            for (int i = 0; i < 1000; i++)
            {
                int safeRadius = i > 666 ? 5 : i > 333 ? 10 : i > 100 ? 20 : 30;
                teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-1000, 1000)), (int)(Target.Center.Y + Main.rand.Next(-1000, 1000)), NPC.width, NPC.height);
                bool foundTile = false;
                for (int x = -safeRadius; x < safeRadius; x++)
                {
                    if (foundTile)
                        break;
                    for (int y = -safeRadius; y < safeRadius; y++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval((int)(teleportPos.X / 16f) + x, (int)(teleportPos.Y / 16f) + y);
                        if (t.HasUnactuatedTile)
                        {
                            foundTile = true;
                            break;
                        }
                    }
                }
                if (!foundTile)
                    break;
            }
            int dustType = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, dustType);
            Main.dust[d].noGravity = true;
            DustExplosion();
            NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
            DustExplosion();
        }



        public void DustExplosion()
        {
            int dust = Main.zenithWorld ? DustID.SnowflakeIce : DustID.FlameBurst;
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, dust, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public void DustExplosion2()
        {
            int dust = Main.zenithWorld ? DustID.SnowflakeIce : DustID.FlameBurst;
            int dust2 = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;

            for (int i = 0; i < 60; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, dust, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
            for (int i = 0; i < 80; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, dust2, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        private void HandlePhaseTransition(bool p3) //has died for the first time
        {
            NPC.ai[1] = 0;
            AttackTimer = 0;
            CalamityUtils.KillAllHostileProjectiles();
            NPC.life = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;
            SoundStyle sound = TransitionSound;
            SoundEngine.PlaySound(sound, NPC.Center);
            DustExplosion();
            DustExplosion();

            if (p3)
            {
                phase3 = true;
            }
            phase2 = true;
            AIState = PyroPhaseType.Transitioning;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }
            Texture2D texture = phase2 ? (Main.zenithWorld ? Phase2TextureC.Value : Phase2Texture.Value) : (Main.zenithWorld ? Phase1TextureC.Value : TextureAssets.Npc[Type].Value);
            Texture2D bloomTx = phase2 ? (Main.zenithWorld ? BloomTexture2C.Value : BloomTexture2.Value) : (Main.zenithWorld ? BloomTextureC.Value : BloomTexture.Value);
            Texture2D additiveTx = phase2 ? (Main.zenithWorld ? AdditiveTexture2C.Value : AdditiveTexture2.Value) : (Main.zenithWorld ? AdditiveTextureC.Value : AdditiveTexture.Value);
            Texture2D gm = phase2 ? (Main.zenithWorld ? Glowmask2C.Value : Glowmask2.Value) : (Main.zenithWorld ? GlowmaskC.Value : Glowmask.Value);
            Texture2D vortex = Main.zenithWorld ? ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Cryogen/Vortex").Value : ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Pyrogen/Vortex").Value;

            Color color = drawColor;
            Color white = Color.White;

            Vector2 pos = NPC.Center - screenPos;

            if (Phase == (int)PyroPhaseType.Hellstorm || Phase == (int)PyroPhaseType.HellstormFatal)
            {
                float comp = Utils.GetLerpValue(0, 60, AttackTimer, true);
                float vortexScale = MathHelper.Lerp(1.8f, 1f, comp);
                if (Phase == (int)PyroPhaseType.Hellstorm && AttackTimer > 600)
                {
                    comp = Utils.GetLerpValue(720, 600, AttackTimer, true);
                    vortexScale = MathHelper.Lerp(0, 1f, comp);
                }
                Color vortexColor = (Main.zenithWorld ? Color.Blue : Color.Orange) * comp * 0.8f;
                Main.EntitySpriteDraw(vortex, pos, null, vortexColor, Main.GlobalTimeWrappedHourly * 4, vortex.Size() / 2, vortexScale, SpriteEffects.None, 0);
            }


            Vector2 ringOrigin = RingTexture.Value.Size() / 2;
            float ringRotation = -Main.GlobalTimeWrappedHourly;
            Main.EntitySpriteDraw((Main.zenithWorld ? RingTextureC.Value : RingTexture.Value), pos, null, white, ringRotation, ringOrigin, NPC.scale, SpriteEffects.None);
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Main.EntitySpriteDraw((Main.zenithWorld ? RingBloomTextureC.Value : RingBloomTexture.Value), pos, null, white, ringRotation, ringOrigin, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(bloomTx, pos, null, white, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.spriteBatch.ExitShaderRegion();
            Main.EntitySpriteDraw((Main.zenithWorld ? BloomExtraC.Value : BloomExtra.Value), pos, null, white * 0.05f, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(texture, pos, null, color, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(gm, pos, null, white, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(additiveTx, pos, null, white with { A = 0 }, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            if (enrageCounter > 0) { 
                if (FireDrawer is null || FireDrawer.LocalTimer >= FireDrawer.SetLifetime)
                    FireDrawer = new FireParticleSet(int.MaxValue, 1, Main.zenithWorld ? Color.White : Color.Red * 1.25f, Main.zenithWorld ? Color.Cyan : Color.Red, 200, 3);
                else
                    FireDrawer.DrawSet(NPC.Bottom - Vector2.UnitY * (4f - NPC.gfxOffY));
            }
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
        new FlavorTextBestiaryInfoElement("A brimstone prison subject to a desperate possession by the soul of a goddess. Its chaotic, ill-fitting energies make it extremely erratic and unstable, as well as incredibly unpredictable in combat.")
            });
        }

        public override void ModifyTypeName(ref string typeName)
        {
            if (Main.zenithWorld)
            {
                typeName = CalamityUtils.GetTextValue("NPCs.Cryogen.DisplayName");
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int dust = Main.zenithWorld ? DustID.IceTorch : DustID.Torch;
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, dust, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, dust, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();
            int[] itemIDs =
            {
                ModContent.ItemType<PyroclasticFlow>(),
                ModContent.ItemType<PlumeflameBow>(),
                ModContent.ItemType<Magmasher>(),
                ModContent.ItemType<TheFirestorm>(),
                ModContent.ItemType<PhreaticChanneler>()
            };
            npcLoot.AddNormalOnly(ModContent.ItemType<EssenceofHavoc>(), 1, 8, 10);

            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<PyrogenBag>());
            npcLoot.Add(ModContent.ItemType<PyrogenTrophy>(), 10);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<PyrogenRelic>());
            npcLoot.AddIf(() => Main.zenithWorld, ModContent.ItemType<DeliciousMeat>(), 1, 2200, 6600);

            npcLoot.AddNormalOnly(ModContent.ItemType<PyrogenMask>(), 7);
            npcLoot.AddNormalOnly(ModContent.ItemType<SoulofPyrogen>());
            npcLoot.AddConditionalPerPlayer(() => !RemixDowned.downedPyrogen, ModContent.ItemType<KnowledgePyrogen>(), desc: DropHelper.FirstKillText);
        }

        public override void OnKill()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<WITCH>()))
            {
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WITCH>());
            }
            RemixDowned.downedPyrogen = true;
            CalRemixWorld.UpdateWorldBool();
        }
    }
}
