using System;
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
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Items.Potions;
using System.Runtime.Serialization;
using Terraria.GameContent.ObjectInteractions;
using CalRemix.Core.Retheme;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Weapons;
using CalRemix.Core.World;
using CalamityMod.Items.Materials;
using CalRemix.Content.NPCs.TownNPCs;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.NPCs.TownNPCs;

namespace CalRemix.Content.NPCs.Bosses.Pyrogen
{
    [AutoloadBossHead]
    public class Pyrogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];
        private int currentPhase = 1;
        public ref Player Target => ref Main.player[NPC.target];


        public Rectangle teleportPos = new Rectangle();
        public Rectangle playerRadius = new Rectangle();

        public ref float AttackTimer => ref NPC.ai[2];

        float rotationMult = 3f;
        float rotationAmt = 0.03f;
        public bool predictiveCharge = false; //decides whether dash will predict player movement

        public int attackSubTotal = 0;
        public int attackTotal = 0; //counts how many attacks have been used- for hellstorm, since it's position in the cycle is set

        public const float BlackholeSafeTime = 40;

        public enum PyroPhaseType
        {
            Idle = 0,
            Charge = 1,
            Rain = 2,
            PullintoShield = 3,
            ThrowShieldBits = 4,
            P21 = 5,
            P22 = 6,
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
        public int healthPercent = 0;

        public static readonly SoundStyle HitSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenHit", 3);
        public static readonly SoundStyle TransitionSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenTransition");
        public static readonly SoundStyle DeathSound = new("CalRemix/Assets/Sounds/GenBosses/PyrogenDeath");
        public static readonly SoundStyle FlareSound = new("CalamityMod/Sounds/Custom/Yharon/YharonFireball", 3);

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

        public static int cryoIconIndex;
        public static int pyroIconIndex;
        public float lifeRatio => NPC.life / (float)NPC.lifeMax;
        public static float BaseDifficultyScale => MathHelper.Clamp((CalamityMod.Events.BossRushEvent.BossRushActive.ToInt() * 3 + CalamityWorld.revenge.ToInt() + Main.expertMode.ToInt()) * 0.5f, 0f, 1f);

        public static bool phase2 = false;

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
            if (!Main.dedServ)
                Music = CalRemixMusic.Pyrogen;
        }


        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 15; i++)
            {
                int bitSprite = i % 6;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PyrogenShield>(), ai0: NPC.whoAmI, ai1: i, ai2: bitSprite);  
            }
            phase2 = false; //weird bug fix
        }

        public override bool CheckDead()
        {
            //if (NPC.life < 1) {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {

                NPC.netUpdate = true;
                if (phase2)
                {
                    NPC.life = 0;
                    NPC.HitEffect();
                    NPC.NPCLoot();
                    NPC.active = false;
                    NPC.netUpdate = true;
                    NPC.justHit = true;
                    SoundStyle sound = DeathSound;
                    SoundEngine.PlaySound(sound, NPC.Center);
                }
                else
                {
                    HandlePhaseTransition();
                }
            }
            return false;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            Player player = Main.player[NPC.target];
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

            bool phase2 = lifeRatio < 0.5f;
            bool phase3 = lifeRatio < 0.3f;

            //  if ((int)NPC.ai[4] + 1 > currentPhase)
            // HandlePhaseTransition((int)NPC.ai[4] + 1);

            Lighting.AddLight(NPC.Center, TorchID.Red);


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
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Torch);
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
                            int tpDistX = 1500;
                            int tpDistY = 1000;
                            int chargeDelaySub = death ? 30 : rev ? 35 : 40; //pause before dash length
                            float chargeLimit = death ? 6 : rev ? 5 : 4;
                            float chargeVelocity = 35; //stays the same

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
                                    SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                                    charges++;
                                }
                                else
                                {
                                    NPC.velocity = NPC.DirectionTo(Target.Center) * chargeVelocity;
                                    NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
                                    SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                                    charges++;
                                    break;
                                }                         
                            }

                            if (AttackTimer == 120) //finished dash! check what to do next...
                            {
                                NPC.velocity.X *= 0.7f; //slow down just in case onscreen
                                NPC.velocity.X *= 0.7f;
                                if (charges >= chargeLimit) //dashed enough times! switch attacks...
                                {
                                    int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.FlameBurst);
                                    Main.dust[d].noGravity = true;
                                    DustExplosion();
                                    NPC.velocity.X = 0f;
                                    NPC.velocity.Y = 0f;
                                    NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
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
                            int flareRate = 15;
                            bool canShootFlares = false;
                            int endPhase = 170;

                            if (AttackTimer == 1)
                            {
                                SafeTeleport();
                            }
                            if (revenge)
                            {
                                stop -= 5;
                                flareRate = 12;
                            }
                            if (death)
                            {
                                stop -= 5;
                                flareRate = 10;
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
                                if (NPC.ai[1] % flareRate == 0)
                                {
                                    SoundEngine.PlaySound(FlareSound, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<PyrogenFlare>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
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
                            int tpDistX = 500;
                            int tpDistY = 500;
                            float distanceRequired = 2000f;
                            float pullForce = 16f;

                            if (AttackTimer == 1)
                            {
                                NPC.damage = 0;
                                for (int i = 0; i < 1000; i++)
                                {
                                    int safeRadius = i > 666 ? 5 : i > 333 ? 10 : i > 100 ? 20 : 30;
                                    teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
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
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Torch);
                                Main.dust[d].noGravity = true;
                                DustExplosion();
                                NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                                DustExplosion();

                                CalamityUtils.KillAllHostileProjectiles();
                            }

                            if (AttackTimer < 20 && AttackTimer > 1) //YANK player in with boss
                            {
                                foreach (Player victim in Main.ActivePlayers)
                                {
                                    victim.velocity = Vector2.Zero;
                                    victim.position = Vector2.Lerp(victim.position, NPC.Center, AttackTimer / BlackholeSafeTime / 2);
                                }
                            }

                            if (AttackTimer == BlackholeSafeTime)
                            {
                            }

                            float randomVariance = NPC.ai[1];
                            int dmg = 100;

                            bool firing = AttackTimer % 60 < 50 && AttackTimer % 90 > 50;
                            if (NPC.Calamity().newAI[0] == 0)
                            {
                                if (AttackTimer > BlackholeSafeTime * 2 && AttackTimer % 5 == 0 && firing)
                                {
                                    int firePoints = 8;
                                    int fireProjSpeed = master ? 4 : 4; // Ice blast speed
                                    float variance = MathHelper.TwoPi / firePoints;
                                    SoundEngine.PlaySound(BetterSoundID.ItemFireball, NPC.Center);
                                    for (int i = 0; i < firePoints; i++)
                                    {
                                        Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                        velocity = velocity.RotatedBy(variance * i + randomVariance);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<PyrogenWaveFlare>(), dmg, 0);
                                    }
                                }
                            }
                            else if (NPC.Calamity().newAI[0] == 1)
                            {
                                if (AttackTimer > BlackholeSafeTime * 2 && AttackTimer % 10 == 0 && firing)
                                {
                                    SoundEngine.PlaySound(BetterSoundID.ItemFireball, NPC.Center);
                                    int dir = Main.rand.NextBool().ToInt();
                                    int totalObjects = master ? 16 : 10;
                                    for (int i = 0; i < totalObjects; i++)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<PyrogenOrbitalFlare>(), dmg, 0, Main.myPlayer, i + 1, totalObjects, Main.rand.NextFloat(0, 4f));
                                    }
                                }
                            }
                            else
                            {
                                if (AttackTimer > BlackholeSafeTime * 2 && AttackTimer % 5 == 0 && firing)
                                {
                                    int firePoints = 8;
                                    int fireProjSpeed = master ? 4 : 4; // Ice blast speed
                                    float variance = MathHelper.TwoPi / firePoints;
                                    SoundEngine.PlaySound(BetterSoundID.ItemFireball, NPC.Center);
                                    for (int i = 0; i < firePoints; i++)
                                    {
                                        Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                        velocity = velocity.RotatedBy(variance * i + randomVariance);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<PyrogenZigZagFlare>(), dmg, 0);
                                    }
                                }
                            }

                            // Controls variance for bullet patterns
                            if (!firing)
                            {
                                NPC.ai[1] = Main.rand.NextFloat(-22.22f, 22.22f);
                                NPC.Calamity().newAI[0] = Main.rand.Next(0, 3);
                            }

                            if (AttackTimer == 700) //reject player from shield since attack has concluded
                            {
                                foreach (Player victim in Main.ActivePlayers)
                                {
                                    distanceRequired = 900f;
                                    float distance = Vector2.Distance(victim.Center, NPC.Center);
                                    if (distance < distanceRequired)
                                    {
                                        victim.velocity = NPC.DirectionTo(victim.Center) * 22;
                                    }
                                }
                            }

                            if (AttackTimer >= 730) //all done!
                            {
                                SelectNextAttack();
                            }

                            foreach (Player victim in Main.ActivePlayers)
                            {
                                if (victim.Calamity() != null)
                                {
                                    victim.Calamity().infiniteFlight = true;
                                }
                            }
                        }
                    break;
                }
                case (int)PyroPhaseType.ThrowShieldBits: //throws shield bits, then pulls them back in at new position 
                {
                    break;
                }
                case (int)PyroPhaseType.P21: //phase 2 first attack, ???
                {
                    break;
                }
                case (int)PyroPhaseType.P22: //phase 2 second attack, ???
                {
                    break;
                }
                case (int)PyroPhaseType.Hellstorm: //spins really fast, pulling projectiles and the player in, then fires a shrapnel bomb at the end of the attack duration; 50 dr
                {
                    break;
                }
                case (int)PyroPhaseType.HellstormFatal: //above, far more intense, boss gradually loses health until dead; 100% dr
                {
                    break;
                }
                case (int)PyroPhaseType.Transitioning: //switching to phase 2; do absolutely nothing until finished
                    {
                        deathTimer++;
                        NPC.velocity.X *= 0.5f;
                        NPC.velocity.Y *= 0.5f;
                        if (deathTimer > 60){ //wait one second before refilling all health
                            healthPercent++;
                            if (NPC.life <= NPC.lifeMax * 0.33f) {
                                NPC.life = 1 + (int)((float)Math.Pow(Utils.GetLerpValue(300, 530, healthPercent, true), 3) * (NPC.lifeMax - 1));
                            }
                        }
                        if (deathTimer >= 160)
                        { //ensure health is 100% filled just in case
                            if (NPC.life < NPC.lifeMax)
                            {
                                NPC.life = NPC.lifeMax;

                            }
                            NPC.dontTakeDamage = false;
                            AIState = PyroPhaseType.Idle;
                        }
                        break;
                    }
            }

            base.AI();
        }


        public void SelectNextAttack()
        {
            // cycle through attacks based on current HP. chosen attack depends 

            switch (AIState)
            {
                case PyroPhaseType.Idle:
                    AIState = phase2 ? PyroPhaseType.Idle : PyroPhaseType.Charge;
                    break;

                case PyroPhaseType.Charge:
                    AIState = phase2 ? PyroPhaseType.Idle : PyroPhaseType.Rain;
                    break;

                case PyroPhaseType.Rain:
                    AIState = phase2 ? PyroPhaseType.PullintoShield : PyroPhaseType.PullintoShield;
                    break;

                case PyroPhaseType.PullintoShield:
                    AIState = phase2 ? PyroPhaseType.Idle : PyroPhaseType.Idle;
                    break;
            }

            if (attackTotal == 5) //fifth attack in each cycle will always be hellstorm
            {
                AIState = PyroPhaseType.Idle; attackTotal = 0; //replace with storm when added
            }

            NPC.ai[1] = 0;
            AttackTimer = 0f;
            charges = 0;
            NPC.damage = 200;
            NPC.netUpdate = true;
        }

        public void SafeTeleport() //instantly teleport somewhere, but never get too close to the player when doing so; bias towards being offscreen
        {
            int tpDistX = 2000;
            int tpDistY = 2000;
            for (int i = 0; i < 120; i++) //tries 120 times to find an eligible position; loop breaks if it somehow fails for that long consecutively
            {
                teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                playerRadius = new Rectangle((int)Target.Center.X, (int)Target.Center.Y, 1500, 1500);
                if (!teleportPos.Intersects(playerRadius))
                {
                    break;
                }
                else continue;
            }

            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.FlameBurst);
                Main.dust[d].noGravity = true;
                DustExplosion();
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
                NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                DustExplosion();
                NPC.ai[1] = 0;
            }
        }



        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FlameBurst, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }
        private void HandlePhaseTransition() //has died for the first time
        {
            NPC.ai[1] = 0;
            AttackTimer = 0;
            CalamityUtils.KillAllHostileProjectiles();
            NPC.life = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;
            SoundStyle sound = TransitionSound;
            SoundEngine.PlaySound(sound, NPC.Center);
            phase2 = true;
            AIState = PyroPhaseType.Transitioning;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }
            Texture2D texture = phase2 ? Phase2Texture.Value : TextureAssets.Npc[Type].Value;
            Texture2D bloomTx = phase2 ? BloomTexture2.Value : BloomTexture.Value;
            Texture2D additiveTx = phase2 ? AdditiveTexture2.Value : AdditiveTexture.Value;
            Texture2D gm = phase2 ? Glowmask2.Value : Glowmask.Value;

            Vector2 pos = NPC.Center - screenPos;
            Vector2 ringOrigin = RingTexture.Value.Size() / 2;
            float ringRotation = -Main.GlobalTimeWrappedHourly;
            Main.EntitySpriteDraw(RingTexture.Value, pos, null, Color.White, ringRotation, ringOrigin, NPC.scale, SpriteEffects.None);
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Main.EntitySpriteDraw(RingBloomTexture.Value, pos, null, Color.White, ringRotation, ringOrigin, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(bloomTx, pos, null, Color.White, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.spriteBatch.ExitShaderRegion();
            Main.EntitySpriteDraw(BloomExtra.Value, pos, null, Color.White * 0.05f, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(texture, pos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(gm, pos, null, Color.White, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(additiveTx, pos, null, Color.White with { A = 0 }, NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
        new FlavorTextBestiaryInfoElement("Having absorbed the energy of the fallen goddess, this elemental construct's seal is supreme amongst its kin. Fate is often cruel to the kind, and mistakes repeated are the most bitter form of punishment.")
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
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FlameBurst, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FlameBurst, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
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
