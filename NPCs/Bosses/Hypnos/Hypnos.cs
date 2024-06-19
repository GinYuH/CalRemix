using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Bestiary;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using static CalamityMod.World.CalamityWorld;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables.Furniture.BossRelics;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Potions;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalRemix.Items;
using CalRemix.Items.Placeables;
using CalRemix.Projectiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using CalRemix.Projectiles.Hostile;

namespace CalRemix.NPCs.Bosses.Hypnos
{
    [AutoloadBossHead]
    internal class Hypnos : ModNPC
    {
        public bool initializedLocal = false;
        public bool afterimages = false;
        public bool p2 = false;
        public bool beserk = false;
        public bool enraged = false;

        public Particle ring;
        public Particle ring2;
        public Particle aura;

        public int ragetimer = 0;
        public int hostdamage = 400;
        public int beserktimer = 0;

        public ThanatosSmokeParticleSet SmokeDrawer = new ThanatosSmokeParticleSet(-1, 3, 0f, 16f, 1.5f);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("XP-00 Hypnos");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.LifeMaxNERB(1320000, 1980000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.DeathSound = SoundID.Item14;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 208;
            NPC.height = 138;
            NPC.boss = true;
            NPC.dontTakeDamage = true;
            NPC.damage = 1;
            NPC.defense = 90;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CerebralAugmentations");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A cerebral dreadnaught, and quite possibly one of Draedon’s finest creations. While the usage of gray matter is questionable, the feat of getting a brain to interface with cybernetics is impressive.")
            });
        }

        public override void AI()
        {
            // Lol
            //Main.player[Main.myPlayer].ZoneTowerVortex = true;
            if (Main.getGoodWorld)
            {
                NPC.scale = 1.75f;
            }
            //Boss zen
            Main.player[Main.myPlayer].Calamity().isNearbyBoss = true;
            Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatBuffs.BossEffects>(), 10, true);
            //Handle transitioning to phase 2
            if (NPC.CountNPCS(ModContent.NPCType<HypnosPlug>()) <= 0 && NPC.ai[0] > 1)
            {
                NPC.dontTakeDamage = false;
                if (!p2 && Main.netMode != NetmodeID.Server)
                {
                    ring2 = new BloomRing(NPC.Center, Vector2.Zero, Color.Purple * 1.2f, NPC.scale * 1.5f, 40);
                    GeneralParticleHandler.SpawnParticle(ring2);
                }
                p2 = true;
                NPC.netUpdate = true;
            }
            else if (NPC.ai[0] != 11)
            {
                NPC.dontTakeDamage = true;
            }

            SmokeDrawer.ParticleSpawnRate = 9999999;
            if (ragetimer > 0)
            {
                SmokeDrawer.ParticleSpawnRate = 3;
                SmokeDrawer.BaseMoveRotation = NPC.rotation + MathHelper.PiOver2;
                SmokeDrawer.SpawnAreaCompactness = 200f;
            }
            SmokeDrawer.Update();
            //Pulse fx
            if (Main.netMode != NetmodeID.Server)
            {
				if (NPC.ai[0] == 1 && !initializedLocal)
                {
                    initializedLocal = true;
				}
				if (ring != null)
                {
                    ring.Scale *= 1.1f;
                    ring.Time += 1;
                }
				if (aura != null)
				{
					aura.Position = NPC.Center;
					aura.Velocity = NPC.velocity;
					aura.Time = 0;
				}
				if (ring2 != null)
				{
					ring2.Position = NPC.Center;
					ring2.Velocity = NPC.velocity;
					ring2.Scale *= 1.1f;
					ring2.Time += 1;
				}
			}
            
            hostdamage = Main.expertMode ? 400 : 600;
            CalRemixGlobalNPC.hypnos = NPC.whoAmI;
            if ((NPC.life <= NPC.lifeMax * 0.25f && !beserk && revenge) || (NPC.life <= NPC.lifeMax * 0.75f && !beserk && death))
            {
                ChangePhase(11);
                ragetimer = 0;
                beserk = true;
            }

            if (NPC.life <= NPC.lifeMax * 0.25f && revenge)
            {
                if (!enraged && NPC.ai[1] == 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar with { Pitch = SoundID.Roar.Pitch + 0.2f}, NPC.Center);
                    enraged = true;
                }
            }

            //The sweet juicy AI
            switch (NPC.ai[0])
            {
                case 0: //Spawn animation
                    {
                        NPC.ai[1]++;
                        NPC.damage = 0;
                        if (NPC.ai[1] < 120)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                if (!Main.dedServ)
                                {
									int num5 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, 0f, 200, default, 1.5f);
									Main.dust[num5].noGravity = true;
									Main.dust[num5].velocity *= 0.75f;
									Main.dust[num5].fadeIn = 1.3f;
									Vector2 vector = new Vector2((float)Main.rand.Next(-400, 401), (float)Main.rand.Next(-400, 401));
									vector.Normalize();
									vector *= (float)Main.rand.Next(100, 200) * 0.04f;
									Main.dust[num5].velocity = vector;
									vector.Normalize();
									vector *= 34f;
									Main.dust[num5].position = NPC.Center - vector;
								}
                               
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                //if (Main.netMode != NetmodeID.MultiplayerClient)
                                NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HypnosPlug>(), 0, NPC.whoAmI, i);
                            }
                            for (int l = 0; l < 48; l++)
                            {
                                Vector2 vector3 = Vector2.UnitX * (float)-(float)NPC.width / 2f;
                                vector3 += -Vector2.UnitY.RotatedBy((double)((float)l * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
                                int num9 = Dust.NewDust(NPC.Center, 0, 0, DustID.FireworkFountain_Blue, 0f, 0f, 160, default, 1f);
                                Main.dust[num9].scale = 1.1f;
                                Main.dust[num9].noGravity = true;
                                Main.dust[num9].position = NPC.Center + vector3;
                                Main.dust[num9].velocity = NPC.velocity * 0.1f;
                                Main.dust[num9].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num9].position) * 1.25f;
                            }
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.FlareSound, NPC.Center);
                            ChangePhase(1);
                        }
                    }
                    break;
                case 1: //Move downward
                    {
                        NPC.ai[1]++;
                        afterimages = true;
                        Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y + 200);
                        Vector2 distanceFromDestination = playerpos - NPC.Center;
                        CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 30, 1, true);
                        if (NPC.ai[1] == 1)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ELRFireSound, NPC.Center);
                            int tesladamage = Main.expertMode ? 175 : 250;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LargeTeslaSphere>(), tesladamage, 0, 255, NPC.target, NPC.whoAmI);
                        }
                        if (NPC.ai[1] >= 120)
                        {
                            ChangePhase(2);
                        }
                    }
                    break;
                case 2: //Fan attack
                    {
                        int stop = 60;
                        if (revenge)
                        {
                            stop -= 5;
                        }
                        if (death)
                        {
                            stop -= 5;
                        }
                        NPC.ai[1]++;
                        if (NPC.ai[1] >= stop && NPC.ai[1] < stop + 120)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        else
                        {
                            Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y + 300);
                            Vector2 distanceFromDestination = playerpos - NPC.Center;
                            CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, Main.getGoodWorld ? 35 : 30, 1, true);
                        }
                        if (NPC.ai[1] >= stop + 140)
                        {
                            ChangePhase(3);
                        }
                    }
                    break;
                case 3: //Back & forth dashes
                    {
                        float movespeed = Main.getGoodWorld ? 0.06f : 0.04f;
                        NPC.ai[1]++;
                        int start = 90;
                        if (revenge)
                        {
                            start -= 5;
                        }
                        if (death)
                        {
                            start -= 5;
                        }
                        if (NPC.ai[1] >= start)
                        {
                            NPC.ai[2] += movespeed + (NPC.ai[1] * 0.000075f);
                        }
                        else
                        {
                            NPC.ai[2] = 5;
                        }
                        afterimages = true;
                        Vector2 hypos = new Vector2(Main.player[NPC.target].Center.X + ((float)Math.Sin(NPC.ai[2]) * 800), Main.player[NPC.target].Center.Y + 300);
                        float idealx = MathHelper.Lerp(NPC.position.X, hypos.X, 0.4f);
                        float idealy = MathHelper.Lerp(NPC.position.Y, hypos.Y, 0.4f);
                        NPC.position = new Vector2(idealx, idealy);
                        if (NPC.ai[1] >= 420)
                        {
                            ChangePhase(4);
                        }
                    }
                    break;
                case 4: //Spinspinspin
                    {
                        afterimages = true;
                        Player target = Main.player[NPC.target];
                        int chargetime = 60;
                        int chargestart = 100;
                        int chargespeed = 15;
                        Vector2 position = NPC.Center;
                        Vector2 targetPosition = target.Center;
                        int predictamt = revenge ? 2 : 3;
                        NPC.ai[1]++;
                        NPC.damage = hostdamage;
                        NPC.Calamity().canBreakPlayerDefense = true;
                        if (death)
                        {
                            chargestart -= 10;
                        }
                        if (revenge)
                        {
                            chargestart -= 10;
                        }
                        if (NPC.ai[1] % chargetime == 0 && NPC.ai[1] >= chargestart && NPC.ai[1] < 340)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Artemis.Artemis.ChargeSound, NPC.Center);
                            if (NPC.ai[1] % (chargetime * predictamt) == 0)
                            {
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, position);
                                Vector2 pos = targetPosition + target.velocity * 20f - position;
                                NPC.velocity = Vector2.Normalize(pos) * chargespeed;
                                if (Main.netMode != NetmodeID.Server)
                                {
									ring2 = new BloomRing(NPC.Center, Vector2.Zero, Color.Purple * 1.2f, NPC.scale * 1.5f, 40);
									GeneralParticleHandler.SpawnParticle(ring2);
								}
                                
                            } 
                            else
                            {
                                Vector2 direction = targetPosition - position;
                                direction.Normalize();
                                NPC.velocity = direction * chargespeed;
                            }
                        }
                        if (NPC.ai[1] < chargestart)
                        {
                            Vector2 playerpos = new Vector2(target.Center.X, target.Center.Y + 300);
                            Vector2 distanceFromDestination = playerpos - position;
                            CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 30, 1, true);

                        }
                        else
                        {
                            NPC.velocity *= 1.01f;
                        }
                        if (NPC.ai[1] >= 361)
                        {
                            ChangePhase(5);
                        }
                    }
                    break;
                case 5: //Neuron charges
                    {
                        NPC.ai[1]++;
                        afterimages = false;
                        Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y + 400);
                        Vector2 distanceFromDestination = playerpos - NPC.Center;
                        CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, Main.getGoodWorld ? 35 : 30, 1, true);
                        if (NPC.ai[1] >= 480)
                        {
                            if (Main.expertMode)
                            {
                                ChangePhase(10);
                            }
                            else
                            {
                                ChangePhase(2);
                            }
                        }
                    }
                    break;
                //Phase 2
                case 6: //Neuron Lightning gates
                    {
                        Player target = Main.player[NPC.target];
                        if (target.chaosState)
                        {
                            ragetimer = 2;
                        }
                        float chargespeed = enraged ? 22 : 20;
                        float walkspeed = Main.getGoodWorld ? 4 : 2;
                        int chargetime = 60;
                        int chargegate = 60;
                        int setuptime = 180;
                        int attackamt = 2;
                        if (death)
                        {
                            chargegate = 120;
                            chargetime = 25;
                            attackamt = 4;
                        }
                        else if (revenge)
                        {
                            chargegate = 120;
                            chargetime = 40;
                            attackamt = 3;
                        }
                        else if (Main.expertMode)
                        {
                            chargetime = 20;
                        }
                        if (enraged)
                        {
                            attackamt--;
                        }
                        Vector2 position = NPC.Center;
                        Vector2 targetPosition = target.Center;
                        NPC.ai[1]++;
                        NPC.ai[2]++;
                        Vector2 direction = targetPosition - position;
                        direction.Normalize();
                        if (NPC.ai[2] <= setuptime)
                        {
                            NPC.velocity = direction * 2 + direction * walkspeed;
                        }
                        else if (NPC.ai[2] % chargetime == 0 && NPC.ai[2] < setuptime + chargegate)
                        {
                            if (Main.netMode != NetmodeID.Server)
                            {
                                ring?.Kill();
                            }
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Artemis.Artemis.ChargeSound, NPC.Center);
                            NPC.velocity = direction * chargespeed;
                            NPC.damage = hostdamage;
                            NPC.Calamity().canBreakPlayerDefense = true;
                            afterimages = true;
                        }
                        if (NPC.ai[2] > setuptime + chargegate + chargetime)
                        {
                            NPC.damage = 0;
                            NPC.Calamity().canBreakPlayerDefense = false;
                            afterimages = false;
                            NPC.ai[2] = 0;
                        }
                        if (NPC.ai[1] > attackamt * (setuptime + chargegate + chargetime) + 1)
                        {
                            ChangePhase(7);
                        }

                        if (Main.netMode != NetmodeID.Server)
                        {
                            if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                            {
                                if (Main.player[Main.myPlayer].wingTime < Main.player[Main.myPlayer].wingTimeMax)
                                {
                                    Main.player[Main.myPlayer].wingTime = Main.player[Main.myPlayer].wingTimeMax;
                                }
                            }
                        }
                    }
                    break;
                case 7: //Rings
                    {
                        Player target = Main.player[NPC.target];
                        int phasetime = 0;
                        if (enraged)
                        {
                            phasetime = death ? 360 : 240;
                        }
                        else
                        {
                            phasetime = death ? 480 : 360;
                        }
                        NPC.ai[1]++;
                        NPC.ai[2]++;

                        if (NPC.ai[1] == 1)
                        {
                            NPC.ai[3] = Main.rand.Next(1, 361);
                            NPC.netUpdate = true;
                        }
                        float rotspeed = NPC.ai[2] * 0.01f;
                        double deg = NPC.ai[3] + NPC.ai[2] * MathHelper.Clamp(rotspeed, NPC.ai[2] * 0.01f, 4);
                        double rad = deg * (Math.PI / 180);
                        double dist = MathHelper.Clamp(200 + NPC.ai[2], 200, 1200);
                        float hyposx = target.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                        float hyposy = target.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;                        
                        float idealx = MathHelper.Lerp(NPC.position.X, hyposx, 0.4f);
                        float idealy = MathHelper.Lerp(NPC.position.Y, hyposy, 0.4f);

                        if (NPC.ai[1] == 0)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Artemis.Artemis.ChargeSound, NPC.Center);
                        }
                        if (NPC.ai[1] < phasetime - 90)
                        {
                            afterimages = true;
                            NPC.position = new Vector2(idealx, idealy);
                        }
                        else if (NPC.ai[1] < phasetime - 60)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        else if (NPC.ai[1] == phasetime - 60)
                        {
                            Vector2 position = NPC.Center;
                            Vector2 targetPosition = target.Center;
                            Vector2 direction = targetPosition - position;
                            direction.Normalize();
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Artemis.Artemis.ChargeSound, NPC.Center);
                            NPC.velocity = direction * 20;
                            NPC.damage = hostdamage;
                            NPC.Calamity().canBreakPlayerDefense = true;
                        }
                        if (NPC.ai[1] > phasetime)
                        {
                            ragetimer--;
                            ChangePhase(8);
                        }
                    }
                    break;
                case 8: //Predictive charge
                    {
                        int attacktime = 180;
                        if (death)
                        {
                            attacktime += 60;
                        }
                        if (Main.expertMode)
                        {
                            attacktime += 60;
                        }
                        if (enraged)
                        {
                            attacktime /= 2;
                        }
                        afterimages = true;
                        Player target = Main.player[NPC.target];
                        int chargetime = 59;
                        int chargespeed = enraged ? 20 : 15;
                        Vector2 position = NPC.Center;
                        Vector2 targetPosition = target.Center;
                        NPC.ai[1]++;
                        NPC.damage = hostdamage;
                        NPC.Calamity().canBreakPlayerDefense = true;
                        if (NPC.ai[1] % chargetime == 0)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Artemis.Artemis.ChargeSound, NPC.Center);
                            Vector2 pos = targetPosition + target.velocity * 20f - position;
                            NPC.velocity = Vector2.Normalize(pos) * chargespeed;
                            if (Main.netMode != NetmodeID.Server)
                            {
                                Color ringcolor = ragetimer > 0 ? Color.Red * 1.2f : Color.CornflowerBlue * 0.6f;
                                ring2 = new BloomRing(NPC.Center, Vector2.Zero, ringcolor, NPC.scale * 1.5f, 40);
                                GeneralParticleHandler.SpawnParticle(ring2);
                            }
                            afterimages = true;
                        }
                        if (NPC.ai[1] < (chargetime - 1))
                        {
                            Vector2 playerpos = new Vector2(target.Center.X, target.Center.Y + 300);
                            Vector2 distanceFromDestination = playerpos - position;
                            CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 30, 1, true);

                        }
                        else
                        {
                            NPC.velocity *= 1.01f;
                        }
                        if (NPC.ai[1] >= attacktime)
                        {
                            NPC.velocity = Vector2.Zero;
                            ragetimer--;
                            ChangePhase(9);
                        }
                    }
                    break;
                case 9: //SWR Yukari attack
                    {
                        int phasetime = enraged ? 600 : 840;
                        NPC.ai[1]++;
                        Player target = Main.player[NPC.target];
                        Vector2 distance = target.Center - NPC.Center;
                        distance *= 6;
                        NPC.velocity = (NPC.velocity * 24f + distance) / 25f;
                        NPC.velocity.Normalize();
                        NPC.velocity *= 6;
                        if (NPC.ai[1] > phasetime)
                        {
                            if (death && beserk)
                            {
                                ChangePhase(11);
                            }
                            else
                            {
                                ChangePhase(6);
                            }
                        }

                        if (Main.netMode != NetmodeID.Server)
                        {
                            if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                            {
                                if (Main.player[Main.myPlayer].wingTime < Main.player[Main.myPlayer].wingTimeMax)
                                {
                                    Main.player[Main.myPlayer].wingTime = Main.player[Main.myPlayer].wingTimeMax;
                                }
                            }
                        }
                    }
                    break;
                case 10: //Lightning wall
                    {
                        NPC.ai[1]++;
                        afterimages = false;
                        Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y + 300);
                        Vector2 distanceFromDestination = playerpos - NPC.Center;
                        CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, Main.getGoodWorld ? 35 : 30, 1, true);
                        int neuroncharge = -1;
                        if (NPC.ai[1] % 40 == 0)
                        {
                            neuroncharge = Main.rand.Next(0, 12);
                        }
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC neuron = Main.npc[i];
                            if (neuron.type == ModContent.NPCType<AergiaNeuron>() && neuron.active && neuroncharge != 0 && neuron.ai[1] == neuroncharge && neuron.ai[2] <= 0)
                            {
                                neuron.ai[2] = 1;
                                neuroncharge = 0;
                            }
                        }
                        if (NPC.ai[1] >= 480)
                        {
                            ChangePhase(2);
                        }
                    }
                    break;
                case 11: //Vanish
                    {
                        int attackamt = enraged ? 2 : 4;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > 30)
                        {
                            NPC.ai[2]++;
                        }
                        if (NPC.ai[1] == 31)
                        {
                            if (Main.netMode != NetmodeID.Server)
                            {
                                ring2 = new BloomRing(NPC.Center, Vector2.Zero, Color.Purple * 1.2f, NPC.scale * 1.5f, 40);
                                GeneralParticleHandler.SpawnParticle(ring2);
                            }
                        }
                        if (NPC.ai[3] == attackamt)
                        {
                            NPC.dontTakeDamage = false;
                            ChangePhase(6);
                        }
                        if (NPC.ai[1] >= 31)
                        {
                            NPC.dontTakeDamage = true;
                        }
                        Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y + 400);
                        Vector2 distanceFromDestination = playerpos - NPC.Center;
                        CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 30, 1.1f, true);
                    }
                    break;
            }
            //Change to phase 2 
            if ((NPC.ai[0] < 6 || NPC.ai[0] == 10) && p2)
            {
                ChangePhase(6);
            }
        }

        public void ChangePhase(int phasenum, bool reset1 = true, bool reset2 = true, bool reset3 = true)
        {
            if (beserk && NPC.ai[0] != 10)
            {
                beserktimer++;
            }
            NPC.ai[0] = beserktimer == 5 ? 11 : phasenum;
            if (reset1)
            {
                NPC.ai[1] = 0;
            }
            if (reset2)
            {
                NPC.ai[2] = 0;
            }
            if (reset3)
            {
                NPC.Calamity().newAI[3] = 0;
            }
            NPC.ai[3] = 0;
            NPC.damage = 0;
            NPC.Calamity().canBreakPlayerDefense = false;
            afterimages = false;
            //Reset Aergia Neuron variables
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC neur = Main.npc[i];
                if (neur.type == NPCType<AergiaNeuron>())
                {
                    neur.velocity = Vector2.Zero;
                    neur.ai[2] = 0;
                    neur.Calamity().newAI[0] = 0;
                    neur.Calamity().newAI[1] = 0;
                    neur.Calamity().newAI[2] = 0;
                    neur.damage = 0;
                    neur.Calamity().canBreakPlayerDefense = false;
                    neur.ModNPC<AergiaNeuron>().afterimages = false;
                    if (p2)
                    {
                        neur.ai[3] = 0;
                    }
                    neur.ModNPC<AergiaNeuron>().rottimer = 0;
                }
            }
            NPC.TargetClosest();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (NPC.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;

                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
                Texture2D glowmask = Request<Texture2D>("CalRemix/NPCs/Bosses/Hypnos/Hypnos_Glow").Value;
                Texture2D eyetexture = Request<Texture2D>("CalRemix/NPCs/Bosses/Hypnos/Hypnos_Eye").Value;
                Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
                Color white = Color.White;
                float colorLerpAmt = 0.5f;
                int afterimageAmt = 7;

                Color eyecolor = NPC.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red : Lighting.GetColor((int)NPC.position.X / 16, (int)NPC.position.Y / 16);
                Color glowcolor = NPC.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red : Color.White;

                if (CalamityConfig.Instance.Afterimages && NPC?.ModNPC<Hypnos>().afterimages == true)
                {
                    for (int i = 1; i < afterimageAmt; i += 2)
                    {
                        Color color1 = drawColor;
                        color1 = Color.Lerp(color1, white, colorLerpAmt);
                        color1 = NPC.GetAlpha(color1);
                        color1 *= (float)(afterimageAmt - i) / 15f;
                        Vector2 offset = NPC.oldPos[i] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
                        offset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                        offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                        spriteBatch.Draw(texture, offset, NPC.frame, color1, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                    }
                }

                Vector2 npcOffset = NPC.Center - screenPos;
                npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(texture, npcOffset, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(glowmask, npcOffset, NPC.frame, glowcolor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(eyetexture, npcOffset, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            }

            SmokeDrawer.DrawSet(NPC.Center);
            return false;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
            npcLoot.Add(normalOnly);

            // Trophies
            npcLoot.Add(ModContent.ItemType<HypnosTrophyInv>());
            npcLoot.Add(ModContent.ItemType<HypnosMask>(), 3);

            // Relic
            npcLoot.Add(ItemDropRule.ByCondition(DropHelper.If(() => Main.masterMode || revenge), ModContent.ItemType<DraedonRelic>()));

            // Lore item
            npcLoot.Add(ItemDropRule.ByCondition(DropHelper.If(() => !RemixDowned.downedHypnos), ModContent.ItemType<LoreExoMechs>()));

            // Treasure bag
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<DraedonBag>()));

            // All other drops are contained in the bag, so they only drop directly on Normal
            if (!Main.expertMode)
            {
                // Materials
                normalOnly.Add(ModContent.ItemType<ExoPrism>(), 1, 24, 32);

                // Equipment
                normalOnly.Add(ModContent.ItemType<ExoThrone>());
                normalOnly.Add(ModContent.ItemType<DraedonsHeart>());

                // Vanity
                // Higher chance due to how the drops work
                normalOnly.Add(ModContent.ItemType<DraedonMask>(), 3);
            }
            RemixDowned.downedHypnos = true;
        }

        public override void OnKill()
        {
            RemixDowned.downedHypnos = true;
            if (Main.netMode != NetmodeID.Server)
            {
                if (aura != null)
                    aura.Kill();
                if (ring != null)
                    ring.Kill();
            }

            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hypnos1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hypnos2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hypnos3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hypnos4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hypnos5").Type, 1f);
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 5;
                SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoHitSound, NPC.Center);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(afterimages);
            //writer.Write(initializedLocal);
            writer.Write(p2);
            writer.Write(enraged);
            writer.Write(beserk);
            writer.Write(hostdamage);
            writer.Write(ragetimer);
            writer.Write(beserktimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            afterimages = reader.ReadBoolean();
			//initializedLocal = reader.ReadBoolean();
			p2 = reader.ReadBoolean();

			enraged = reader.ReadBoolean();
			beserk = reader.ReadBoolean();

			hostdamage = reader.ReadInt32();
			ragetimer = reader.ReadInt32();
            
            beserktimer = reader.ReadInt32();
        }

        public static void SummonDraedon(Player player)
        { // don't call it on multiplayer client
			NPC.NewNPC(new Terraria.DataStructures.EntitySource_BossSpawn(player), (int)player.Center.X, (int)(player.Center.Y - 1200), NPCType<RemixDraedon>(), 0, 0, 0, 0, player.whoAmI, player.whoAmI);

		}
	}
}