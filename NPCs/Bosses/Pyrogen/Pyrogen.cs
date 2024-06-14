using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.Audio;
using ReLogic.Content;
using CalamityMod;
using CalRemix.Items;
using System.Linq;
using CalRemix.UI;
using System.Collections;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using CalRemix.Retheme;
using CalRemix.UI.ElementalSystem;
using CalamityMod.World;
using Terraria.DataStructures;
using CalamityMod.Events;
using static CalamityMod.World.CalamityWorld;
using CalRemix.Projectiles.Hostile;

namespace CalRemix.NPCs.Bosses.Pyrogen
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
            HellstormFatal = 8
        }

        public PyroPhaseType AIState
        {
            get => (PyroPhaseType)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }

        public int chargeLimit = 4;
        public int charges = 0;

        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/CryogenHit", 3);
        public static readonly SoundStyle TransitionSound = new("CalamityMod/Sounds/NPCHit/CryogenPhaseTransitionCrack");
        public static readonly SoundStyle DeathSound = new("CalamityMod/Sounds/NPCKilled/CryogenDeath");
        public static readonly SoundStyle FlareSound = new("CalamityMod/Sounds/Custom/Yharon/YharonFireball", 3);

        public override string Texture => "CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase1";

        public static Asset<Texture2D> Phase2Texture;
        public static Asset<Texture2D> Phase3Texture;
        public static Asset<Texture2D> GlowTexture;

        public static int cryoIconIndex;
        public static int pyroIconIndex;
        public float lifeRatio => NPC.life / (float)NPC.lifeMax;
        public static float BaseDifficultyScale => MathHelper.Clamp((CalamityMod.Events.BossRushEvent.BossRushActive.ToInt() * 3 + CalamityWorld.revenge.ToInt() + Main.expertMode.ToInt()) * 0.5f, 0f, 1f);

        public static float Phase2LifeRatio => MathHelper.Lerp(0.6f, 0.5f, BaseDifficultyScale);

        internal static void LoadHeadIcons()
        {
            string pyroIconPath = "CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase1_Head_Boss";
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
                Phase2Texture = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase2", AssetRequestMode.AsyncLoad);
                Phase3Texture = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Pyrogen/Pyrogen_Phase3", AssetRequestMode.AsyncLoad);
                GlowTexture = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.AsyncLoad);
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
            NPC.LifeMaxNERB(322600, 375700, 1350000); ;
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
            Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/InfernalSeal");
        }


        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 15; i++)
            {
                int bitSprite = 0;
                switch (i)
                {
                    case 0:
                    case 4:
                    case 8:
                    case 12:
                        bitSprite = 0;
                        break;
                    case 1:
                    case 5:
                    case 9:
                    case 13:
                        bitSprite = 1;
                        break;
                    case 2:
                    case 6:
                    case 10:
                    case 14:
                        bitSprite = 2;
                        break;
                    case 3:
                    case 7:
                    case 11:
                    case 15:
                        bitSprite = 3;
                        break;
                }
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PyrogenShield>(), ai0: NPC.whoAmI, ai1: i, ai2: bitSprite);  
            }
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
                                if (charges == chargeLimit) //dashed enough times! switch attacks...
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
                            float pullForce = 8f;

                            if (AttackTimer == 1)
                            {
                                NPC.damage = 0;
                                teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Torch);
                                Main.dust[d].noGravity = true;
                                DustExplosion();
                                NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                                DustExplosion();
                            }

                            if (AttackTimer < 20) //YANK player in with boss
                            {
                                foreach (Player victim in Main.ActivePlayers)
                                {
                                    float distance = Vector2.Distance(victim.Center, NPC.Center);
                                    if (distance < distanceRequired)
                                    {
                                        float distanceRatio = distance / distanceRequired;
                                        float multiplier = 1f - distanceRatio;
                                        victim.velocity.X += pullForce * multiplier;
                                        victim.velocity.Y += pullForce * multiplier;
                                    }
                                }
                            }

                            if (AttackTimer == 40)
                            {
                                NPC.damage = 200;
                            }

                            if (AttackTimer == 700) //reject player from shield since attack has concluded
                            {
                                foreach (Player victim in Main.ActivePlayers)
                                {
                                    distanceRequired = 900f;
                                    float distance = Vector2.Distance(victim.Center, NPC.Center);
                                    if (distance < distanceRequired)
                                    {
                                        float distanceRatio = distance / distanceRequired;
                                        float multiplier = 1f - distanceRatio;
                                        victim.velocity.X -= pullForce * multiplier;
                                        victim.velocity.Y -= pullForce * multiplier;
                                    }
                                }
                            }

                            if (AttackTimer == 730) //all done!
                            {
                                SelectNextAttack();
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
            }

            base.AI();
        }


        public void SelectNextAttack()
        {

            // cycle through attacks based on current HP. chosen attack depends 

            switch (AIState)
            {
                case PyroPhaseType.Idle:
                    AIState = lifeRatio < Phase2LifeRatio ? PyroPhaseType.Idle : PyroPhaseType.Charge;
                    break;

                case PyroPhaseType.Charge:
                    AIState = lifeRatio < Phase2LifeRatio ? PyroPhaseType.Idle : PyroPhaseType.Rain;
                    break;

                case PyroPhaseType.Rain:
                    AIState = lifeRatio < Phase2LifeRatio ? PyroPhaseType.PullintoShield : PyroPhaseType.PullintoShield;
                    break;

                case PyroPhaseType.PullintoShield:
                    AIState = lifeRatio < Phase2LifeRatio ? PyroPhaseType.Idle : PyroPhaseType.Idle;
                    break;
            }

            if (attackTotal == 5) //fifth attack in each cycle will always be hellstorm
            {
                AIState = PyroPhaseType.Idle; attackTotal = 0; //replace with storm when added
            }

            NPC.ai[1] = 0;
            AttackTimer = 0f;
            charges = 0;
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
        private void HandlePhaseTransition(int newPhase)
        {
            SoundStyle sound = TransitionSound;
            SoundEngine.PlaySound(sound, NPC.Center);

            currentPhase = newPhase;
            switch (currentPhase)
            {
                case 0: break;

                case 1: break;

                case 2: break;

                case 3: break;

                case 4: break;

                case 5: break;

            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY + 4),
                NPC.frame, Color.Orange, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, effects, 0);
            }

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            switch (currentPhase)
            {
                case 2:
                    texture = Phase2Texture.Value;
                    break;
                case 3:
                    texture = Phase3Texture.Value;
                    break;
                default:
                    texture = TextureAssets.Npc[NPC.type].Value;
                    break;
            }
            return true;
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
    }
}
