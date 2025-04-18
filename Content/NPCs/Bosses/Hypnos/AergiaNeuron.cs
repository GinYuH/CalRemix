using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.World;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Graphics.Primitives;
using Terraria.Graphics.Effects;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Skies;

namespace CalRemix.Content.NPCs.Bosses.Hypnos
{
    public class AergiaNeuron : ModNPC
    {
        public bool initialized = false;
        public bool afterimages = false;
        public bool hypnosafter = false;
        public bool p2 = false;
        public bool enraged = false;

        public float corite1 = 0;
        public float corite2 = 0;
        public float corite3 = 0;
        public float corite4 = 0;
        public float rottimer = 0;

        public int offx;
        public int offy;
        public float lvf = 1; //laser velocity factor

        Vector2 destiny;

        NPC hypnos;
        NPC plug;

        public Particle ring;

        public ThanatosSmokeParticleSet SmokeDrawer = new ThanatosSmokeParticleSet(-4, 3, 0f, 16f, 1.5f);
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("XP-00 Hypnos Aergia Neuron");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 200;
            NPC.damage = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 1;
            NPC.dontTakeDamage = true;
            NPC.defense = 22;
        }

        public override void AI()
        {
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            int laserproj = p2 ? ModContent.ProjectileType<RedExoPulseLaser>() : ModContent.ProjectileType<BlueExoPulseLaser>();
            int laserdamage = 160;
            if (Main.expertMode)
            {
                laserdamage = p2 ? 120 : 130;

            }
            else
            {
                laserdamage = p2 ? 205 : 160;
            }

			

            if (!initialized)
            {
				hypnos = Main.npc[(int)NPC.ai[0]];
				plug = Main.npc[(int)NPC.ai[3]];
				initialized = true;
                NPC.netUpdate = true;
                return;
            }

			//hypnos = Main.npc[CalRemixNPC.hypnos];
			//I'm keeping NPC.ai[0] consistent with Hypnos to reduce confusion

			//NPC.ai[0] = hypnos.ai[0];
            enraged = hypnos.ModNPC<Hypnos>().enraged;
            Player target = Main.player[hypnos.target];
            if (!plug.active && plug.type == ModContent.NPCType<HypnosPlug>())
            {
                p2 = true;
            }
            if (!hypnos.active)
            {
                NPC.active = false;
            }
            if (p2)
            {
                lvf = 1.3f;
            }
            else if (CalamityWorld.death)
            {
                lvf = 1.2f;
            }
            else if (CalamityWorld.revenge)
            {
                lvf = 1.1f;
            }
            
            if (Main.netMode != NetmodeID.Server)
            {
				SmokeDrawer.ParticleSpawnRate = 9999999;
				if (ring != null)
				{
					ring.Scale *= 1.1f;
					ring.Time += 1;
				}
				if (hypnos.ModNPC<Hypnos>().ragetimer > 0 && hypnos.ai[0] != 8)
				{
					SmokeDrawer.ParticleSpawnRate = 12;
					SmokeDrawer.BaseMoveRotation = NPC.rotation + MathHelper.PiOver2;
					SmokeDrawer.SpawnAreaCompactness = 1f;
				}
				SmokeDrawer.Update();
			}
            
            if (NPC.localAI[0] < 1f)
            {
                NPC.localAI[0] += 0.01f;
            }
            
            switch (hypnos.ai[0])
            {
                case 0: //Basic idle
                case 1:
                    {
                        Vector2 idealpos = NPC.Center;

                        double deg = 30 * NPC.ai[1];
                        double rad = deg * (Math.PI / 180);
                        double dist = 300;
                        idealpos.X = target.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                        idealpos.Y = target.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                        Vector2 distanceFromDestination = idealpos - NPC.Center;
                        CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 60, 1, true);
                    }
                    break;
                case 2: //Fan attack
                    {
                        NPC.damage = 0;
                        NPC.velocity = Vector2.Zero;
                        int stop = 60;
                        Vector2 velocity = target.Center - NPC.Center;
                        velocity.Normalize();
                        velocity *= 9f;

                        double deg = 15 * NPC.ai[1] + 8;
                        double rad = deg * (Math.PI / 180);
                        double dist = 200;
                        float hypx = hypnos.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                        float hypy = hypnos.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                        float idealx = MathHelper.Lerp(NPC.position.X, hypx, 0.4f);
                        float idealy = MathHelper.Lerp(NPC.position.Y, hypy, 0.4f);
                        NPC.position = new Vector2((int)idealx, (int)idealy);
                        if (hypnos.ai[1] >= stop + 30 && hypnos.ai[1] < stop + 90)
                        {
                            NPC.ai[2]++;
                        }
                        int lasertimer = 20;
                        if (CalamityWorld.death)
                        {
                            lasertimer = 10;
                        }
                        else if (CalamityWorld.revenge)
                        {
                            lasertimer = 12;
                        }
                        else if (Main.expertMode)
                        {
                            lasertimer = 15;
                        }
                        else
                        {
                            lasertimer = 20;
                        }
                        if (NPC.ai[2] >= lasertimer)
                        {
                            Vector2 position = NPC.Center;
                            Vector2 targetPosition = hypnos.Center;
                            Vector2 direction = targetPosition - position;
                            direction.Normalize();
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * -15 * lvf, laserproj, laserdamage, 0);
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Volume - 0.1f }, NPC.Center);
                            NPC.ai[2] = 0;
                        }
                    }
                    break;
                case 3: //Back & forth dashes
                    {
                        int heightoffset = -100;
                        int widthoffset = -349;
                        int spacing = 60;
                        Vector2 hypos = new Vector2(hypnos.Center.X + (NPC.ai[1] * spacing) + widthoffset, hypnos.Center.Y + heightoffset);
                        float idealx = MathHelper.Lerp(NPC.position.X, hypos.X, 0.4f);
                        float idealy = MathHelper.Lerp(NPC.position.Y, hypos.Y, 0.4f);
                        NPC.position = new Vector2(idealx, idealy);
                        NPC.ai[2]++;
                        int lasertimer = 60;
                        if (Main.expertMode)
                        {
                            lasertimer -= 5;
                        }
                        if (CalamityWorld.revenge)
                        {
                            lasertimer -= 5;
                        }
                        if (CalamityWorld.death)
                        {
                            lasertimer -= 5;
                        }
                        if (NPC.ai[2] >= lasertimer + Main.rand.Next(-5, 5))
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -11 * lvf), laserproj, laserdamage, 0);
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Volume - 0.1f }, NPC.Center);
                            NPC.ai[2] = 0;
                        }
                    }
                    break;
                case 4: //Spinspinspin
                    {
                        NPC.damage = 200;
                        NPC.Calamity().canBreakPlayerDefense = true;
                        rottimer += 12f;
                        double deg = 30 * NPC.ai[1] + rottimer;
                        double rad = deg * (Math.PI / 180);
                        double dist = 200;
                        if (hypnos.ai[1] < 60)
                        {
                            float hyposx = hypnos.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            float hyposy = hypnos.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                            float idealx = MathHelper.Lerp(NPC.position.X, hyposx, 0.8f);
                            float idealy = MathHelper.Lerp(NPC.position.Y, hyposy, 0.8f);
                            NPC.position = new Vector2(idealx, idealy);
                        }
                        else
                        {
                            NPC.position.X = hypnos.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            NPC.position.Y = hypnos.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                        }
                        Vector2 playerspeed = target.velocity / 2;

                        NPC.ai[2]++;
                        int lasertimer = 60;
                        if (Main.expertMode)
                        {
                            lasertimer -= 10;
                        }
                        if (CalamityWorld.revenge)
                        {
                            lasertimer -= 5;
                        }
                        if (CalamityWorld.death)
                        {
                            lasertimer -= 5;
                        }
                        if (NPC.ai[2] >= lasertimer)
                        {
                            Vector2 position = NPC.Center;
                            Vector2 targetPosition = hypnos.Center;
                            Vector2 direction = targetPosition - position;
                            direction.Normalize();

                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Volume - 0.1f }, NPC.Center);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * -15 * lvf + playerspeed, laserproj, laserdamage, 0);
                            NPC.ai[2] = 0;
                        }
                    }
                    break;
                case 5: //Neuron charges, 480 ticks
                    {
                        Vector2 idealpos = NPC.Center;

                        double deg = 30 * NPC.ai[1];
                        double rad = deg * (Math.PI / 180);
                        double dist = 400;
                        bool bottom = NPC.ai[1] <= 10 && NPC.ai[1] >= 8;
                        int chargespeed = 20;
                        if (CalamityWorld.death)
                        {
                            chargespeed += 5;
                        }
                        if (CalamityWorld.revenge)
                        {
                            chargespeed += 3;
                        }

                        if (!bottom)
                        {
                            NPC.ai[2]++;
                        }
                        if (NPC.ai[2] < (60 * NPC.ai[1]) + 80)
                        {
                            idealpos.X = target.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            idealpos.Y = target.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                            Vector2 distanceFromDestination = idealpos - NPC.Center;
                            CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 60, 1, true);
                        }
                        else if (NPC.ai[2] == (60 * NPC.ai[1]) + 80)
                        {
                            idealpos.X = target.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            idealpos.Y = target.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                            NPC.position = idealpos;
                        }
                        else if (NPC.ai[2] == (60 * NPC.ai[1]) + 81)
                        {
                            Vector2 direction = target.Center - NPC.Center;
                            direction.Normalize();
                            NPC.velocity = direction * chargespeed;
                        }
                        else if (NPC.ai[2] >= (60 * NPC.ai[1]) + 101)
                        {
                            NPC.damage = 0;
                            NPC.Calamity().canBreakPlayerDefense = false;
                            NPC.ai[2] = 0;
                        }
                        else
                        {
                            NPC.velocity *= 1.01f;
                            NPC.damage = 200;
                            NPC.Calamity().canBreakPlayerDefense = true;
                        }
                    }
                    break;
                //Phase 2
                case 6: //Neuron lightning gates
                    {
                        int barrierx = 1000;
                        int barriery = 500;
                        destiny = target.position;
                        if (hypnos.ai[2] == 0)
                        {
                            if (NPC.ai[1] > 3)
                            {
                                if (enraged)
                                {
                                    offx = Main.rand.Next(-1500, 1501);
                                    offy = Main.rand.Next(-1000, 1001);
                                }
                                else
                                {
                                    offx = Main.rand.Next(-1200, 1201);
                                    offy = Main.rand.Next(-700, 701);
                                }
                            }
                            else
                            {
                                switch (NPC.ai[1])
                                {
                                    case 0:
                                        offx = barrierx;
                                        offy = barriery;
                                        break;
                                    case 1:
                                        offx = -barrierx;
                                        offy = barriery;
                                        break;
                                    case 2:
                                        offx = -barrierx;
                                        offy = -barriery;
                                        break;
                                    case 3:
                                        offx = barrierx;
                                        offy = -barriery;
                                        break;
                                }
                            }
                        }
                        destiny.X = destiny.X + offx;
                        destiny.Y = destiny.Y + offy;
                        //Neurons briefly follow the player 
                        if (hypnos.ai[2] < 30)
                        {
                            float idealx = MathHelper.Lerp(NPC.position.X, destiny.X, 0.1f);
                            float idealy = MathHelper.Lerp(NPC.position.Y, destiny.Y, 0.1f);
                            NPC.position = new Vector2(idealx, idealy);
                        }
                        //Normal neurons
                        if (hypnos.ai[2] >= 120 && NPC.ai[1] >= 4)
                        {
                            NPC.velocity = Vector2.Zero;
                            float nothing = 0;
                            for (int i = 0; i < Main.maxNPCs; i++)
                            {
                                NPC nextneuron = Main.npc[i];
                                if (nextneuron.type == ModContent.NPCType<AergiaNeuron>() && nextneuron.ai[1] == NPC.ai[1] + 1 && nextneuron.active)
                                {
                                    if (Collision.CheckAABBvLineCollision(target.getRect().TopLeft(), target.Size, NPC.Center, nextneuron.Center, 3f, ref nothing))
                                    {
                                        target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(target.name + " felt 10 thousand volts."), 200, 0);
                                    }
                                }
                            }
                        }
                        //Barrier neurons
                        if (hypnos.ai[2] >= 60 && NPC.ai[1] < 4)
                        {
                            NPC.velocity = Vector2.Zero;
                            float nothing = 0;
                            for (int i = 0; i < Main.maxNPCs; i++)
                            {
                                NPC nextneuron = Main.npc[i];
                                if (nextneuron.type == ModContent.NPCType<AergiaNeuron>() && nextneuron.ai[1] == NPC.ai[1] + 1 && NPC.ai[1] < 3 && nextneuron.active)
                                {
                                    if (Collision.CheckAABBvLineCollision(target.getRect().TopLeft(), target.Size, NPC.Center, nextneuron.Center, 3f, ref nothing))
                                    {
                                        target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(target.name + " felt 10 thousand volts."), 600, 0);
                                    }
                                }
                                if (nextneuron.type == ModContent.NPCType<AergiaNeuron>() && NPC.ai[1] == 3 && nextneuron.ai[1] == 0 && nextneuron.active)
                                {
                                    if (Collision.CheckAABBvLineCollision(target.getRect().TopLeft(), target.Size, NPC.Center, nextneuron.Center, 3f, ref nothing))
                                    {
                                        target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(target.name + " felt 10 thousand volts."), 600, 0);
                                    }
                                }
                            }

                        }
                        if (hypnos.ai[2] == 121)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.1f }, NPC.Center);
                        }
                    }
                    break;
                case 7: //Rings
                    {
                        if (CalamityWorld.revenge)
                        {
                            //0 1 2  [3 4 5] 6 7 8 [9 10 11]
                            if (NPC.ai[1] < 3 || (NPC.ai[1] > 5 && NPC.ai[1] < 9))
                            {
                                rottimer += 2f;
                            }
                            else
                            {
                                rottimer -= 2f;
                            }

                        }
                        else
                        {
                            if (NPC.ai[1] < 6)
                            {
                                rottimer += 2f;
                            }
                            else
                            {
                                rottimer -= 2f;
                            }
                        }
                        int spacing = CalamityWorld.revenge ? 120 : 60;
                        double deg = spacing * NPC.ai[1] + rottimer;
                        double rad = deg * (Math.PI / 180);
                        int lasertimer = 60;
                        if (CalamityWorld.revenge)
                        {
                            switch (NPC.ai[1])
                            {
                                case 0:
                                case 1:
                                case 2:
                                    NPC.ai[3] = 200;
                                    //lasertimer = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? 70 : 110;
                                    break;
                                case 3:
                                case 4:
                                case 5:
                                    NPC.ai[3] = 300;
                                    //lasertimer = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? 55 : 70;
                                    break;
                                case 6:
                                case 7:
                                case 8:
                                    NPC.ai[3] = 400;
                                    lasertimer = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? 40 : 50;
                                    break;
                                case 9:
                                case 10:
                                case 11:
                                    NPC.ai[3] = 500;
                                    lasertimer = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? 25 : 30;
                                    break;
                            }
                        }
                        else
                        {
                            NPC.ai[3] = NPC.ai[1] < 6 ? 200 : 400;
                            lasertimer = NPC.ai[1] < 6 ? 99999 : 40;
                            if (hypnos.ModNPC<Hypnos>().ragetimer > 0)
                            {
                                lasertimer = NPC.ai[1] < 6 ? 99999 : 30;
                            }
                        }
                        if (hypnos.ai[1] < 60)
                        {
                            float hyposx = target.Center.X - (int)(Math.Cos(rad) * NPC.ai[3]) - NPC.width / 2;
                            float hyposy = target.Center.Y - (int)(Math.Sin(rad) * NPC.ai[3]) - NPC.height / 2;
                            float idealx = MathHelper.Lerp(NPC.position.X, hyposx, 0.8f);
                            float idealy = MathHelper.Lerp(NPC.position.Y, hyposy, 0.8f);
                            NPC.position = new Vector2(idealx, idealy);
                        }
                        else
                        {
                            NPC.position.X = target.Center.X - (int)(Math.Cos(rad) * NPC.ai[3]) - NPC.width / 2;
                            NPC.position.Y = target.Center.Y - (int)(Math.Sin(rad) * NPC.ai[3]) - NPC.height / 2;
                        }

                        NPC.ai[2]++;

                        if (Main.expertMode)
                        {
                            lasertimer -= 10;
                        }
                        if (CalamityWorld.revenge)
                        {
                            lasertimer -= 5;
                        }
                        if (CalamityWorld.death)
                        {
                            lasertimer -= 5;
                        }
                        if (enraged)
                        {
                            lasertimer -= 5;
                        }    
                        if (NPC.ai[2] >= lasertimer)
                        {
                            Vector2 position = NPC.Center;
                            Vector2 targetPosition = target.Center;
                            Vector2 direction = targetPosition - position;
                            direction.Normalize();

                            Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Volume - 0.1f }, NPC.Center);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 15 * lvf, laserproj, laserdamage, 0);
                            NPC.ai[2] = 0;
                        }
                    }
                    break;
                case 8: //Predictive charge
                    {
                        NPC.velocity = Vector2.Zero;
                        rottimer += hypnos.ModNPC<Hypnos>().ragetimer > 0 ? 15f : 12f;
                        double deg = 30 * NPC.ai[1] + rottimer;
                        double rad = deg * (Math.PI / 180);
                        double dist = 200;
                        if (CalamityWorld.revenge)
                        {
                            dist -= 30;
                        }
                        if (CalamityWorld.death)
                        {
                            dist -= 30;
                        }
                        if (hypnos.ai[1] < 60)
                        {
                            float hyposx = hypnos.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            float hyposy = hypnos.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                            float idealx = MathHelper.Lerp(NPC.position.X, hyposx, 0.8f);
                            float idealy = MathHelper.Lerp(NPC.position.Y, hyposy, 0.8f);
                            NPC.position = new Vector2(idealx, idealy);
                        }
                        else
                        {
                            NPC.damage = 200;
                            NPC.Calamity().canBreakPlayerDefense = true;
                            NPC.position.X = hypnos.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            NPC.position.Y = hypnos.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                        }
                    }
                    break;
                case 9: //SWR Yukari attack
                    {
                        rottimer = 0;
                        int heightoffset = -400;
                        int spacing = 100;
                        int maxtimes = 3;
                        int maxspeed = CalamityWorld.death ? 20 : 35;
                        if (CalamityWorld.death)
                        {
                            maxtimes++;
                            if (NPC.ai[2] == 0)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    NPC.Calamity().newAI[0] = 800;
                                    NPC.velocity = Vector2.Zero;
                                }
                                else
                                {
                                    NPC.Calamity().newAI[0] = -800;
                                    NPC.velocity = Vector2.Zero;
                                }
                            }
                        }
                        else
                        {
                            NPC.Calamity().newAI[0] = -800;
                        }
                        if (Main.expertMode)
                        {
                            maxtimes++;
                        }
                        maxtimes = 999;
                        Vector2 hypos = new Vector2(target.Center.X + NPC.Calamity().newAI[0], target.Center.Y + (NPC.ai[1] * spacing) + heightoffset);
                        float idealx = MathHelper.Lerp(NPC.position.X, hypos.X, 0.4f);
                        float idealy = MathHelper.Lerp(NPC.position.Y, hypos.Y, 0.4f);
                        NPC.ai[2]++;

                        if (NPC.ai[3] > maxtimes)
                        {
                            NPC.velocity.X = 0;
                        }
                        if (NPC.ai[2] >= 30 + NPC.ai[1] * 10 && NPC.ai[3] <= maxtimes)
                        {
                            NPC.damage = 200;
                            NPC.Calamity().canBreakPlayerDefense = true;
                            afterimages = true;
                            if (NPC.velocity.X > -maxspeed && NPC.Calamity().newAI[0] == 800)
                            {
                                NPC.velocity.X -= CalamityWorld.death ? 0.3f : 0.4f;
                            }
                            if (NPC.velocity.X < maxspeed && NPC.Calamity().newAI[0] == -800)
                            {
                                NPC.velocity.X += 0.3f;
                            }
                            if ((NPC.position.X > target.position.X + target.width + 600 && NPC.Calamity().newAI[0] == -800) || (NPC.position.X < target.position.X + target.width - 600 && NPC.Calamity().newAI[0] == 800))
                            {
                                NPC.damage = 0;
                                NPC.Calamity().canBreakPlayerDefense = false;
                                Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.LaserCannonSound.Volume - 0.2f, Pitch = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Pitch + 0.2f }, NPC.Center);
                                NPC.ai[3]++;
                                NPC.ai[2] = 0;
                                if (Main.netMode != NetmodeID.Server)
                                {
									Color ringcolor = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red * 1.2f : Color.Pink * 0.6f;
									ring = new BloomRing(NPC.Center, Vector2.Zero, ringcolor, NPC.scale * 0.4f, 30);
									GeneralParticleHandler.SpawnParticle(ring);
								}
                                
                                NPC.position = new Vector2(idealx, idealy);
                            }
                        }
                        else
                        {
                            afterimages = false;
                            NPC.position = new Vector2(idealx, idealy);
                        }
                    }
                    break;
                case 10: //Lightning wall
                    {
                        if (!p2)
                        {
                            Vector2 idealpos = NPC.Center;

                            double deg = 30 * NPC.ai[1];
                            double rad = deg * (Math.PI / 180);
                            double dist = 300;
                            bool bottom = NPC.ai[1] <= 10 && NPC.ai[1] >= 8;
                            int laseramt = 7;
                            int freq = 65;
                            if (CalamityMod.World.CalamityWorld.death)
                            {
                                laseramt++;
                                freq -= 5;
                            }
                            if (CalamityWorld.revenge)
                            {
                                freq -= 5;
                            }
                            if (Main.expertMode)
                            {
                                laseramt += 2;
                            }

                            if (!bottom && NPC.ai[2] > 0)
                            {
                                NPC.ai[2]++;
                            }
                            if (NPC.ai[2] >= 60)
                            {
                                float variance = MathHelper.TwoPi / laseramt;
                                for (int i = 0; i < laseramt; i++)
                                {
                                    Vector2 velocity = new Vector2(0f, 8f * lvf);
                                    velocity = velocity.RotatedBy(variance * i);
                                    Terraria.Audio.SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound with { Volume = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Volume - 0.2f, Pitch = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Pitch + 0.2f }, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, laserproj, laserdamage, 0);
                                }
                                NPC.ai[2] = 0;
                            }
                            idealpos.X = target.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                            idealpos.Y = target.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
                            Vector2 distanceFromDestination = idealpos - NPC.Center;
                            CalamityUtils.SmoothMovement(NPC, 100, distanceFromDestination, 60, 1, true);
                        }
                    }
                    break;
                case 11: //Vanish
                    {
                        NPC.damage = 200;
                        NPC.Calamity().canBreakPlayerDefense = true;
                        int maxdist = 900;
                        int spinfactor = enraged ? 9 : 6;
                        rottimer++;
                        if (enraged)
                        {
                            rottimer++;
                        }
                        double deg = 30 * NPC.ai[1] + rottimer;
                        double rad = deg * (Math.PI / 180);
                        NPC.ai[3] = maxdist - NPC.ai[2];
                        if (hypnos.ai[2] > 60)
                        {
                            NPC.ai[2] += spinfactor + (NPC.ai[2] / 200);
                        }
                        float hyposx = destiny.X - (int)(Math.Cos(rad) * NPC.ai[3]) - NPC.width / 2;
                        float hyposy = destiny.Y - (int)(Math.Sin(rad) * NPC.ai[3]) - NPC.height / 2;
                        float idealx = MathHelper.Lerp(NPC.position.X, hyposx, 0.8f);
                        float idealy = MathHelper.Lerp(NPC.position.Y, hyposy, 0.8f);
                        NPC.position = new Vector2(idealx, idealy);
                        if (hypnos.ai[2] <= 60)
                        {
                            destiny.X = target.Center.X - NPC.width / 2;
                            destiny.Y = target.Center.Y - NPC.height / 2;
                        }
                        if (NPC.ai[2] >= maxdist)
                        {                            
                            if (NPC.ai[1] == 0)
                            {
                                ExoMechsSky.CreateLightningBolt(22, true);
                                Terraria.Audio.SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Ares.AresGaussNuke.NukeExplosionSound with { Volume = CalamityMod.NPCs.ExoMechs.Ares.AresGaussNuke.NukeExplosionSound.Volume, Pitch = CalamityMod.Sounds.CommonCalamitySounds.ExoLaserShootSound.Pitch - 0.2f });
                                for (int i = 0; i < 3; i++)
                                {
                                    Projectile explosion = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AresGaussNukeProjectileBoom>(), laserdamage * 2, 0)];
                                    if (explosion.whoAmI.WithinBounds(Main.maxProjectiles))
                                    {
                                        explosion.ai[1] = 560f + i * 90f;
                                        explosion.localAI[1] = 0.25f;
                                        explosion.Opacity = MathHelper.Lerp(0.03f, 0.6f, i / 3f) + Main.rand.NextFloat(-0.08f, 0.08f);
                                        explosion.netUpdate = true;
                                    }
                                }
                            }
                            NPC.ai[2] = 0;
                            if (NPC.ai[1] == 0)
                            {
                                hypnos.ai[3]++;
                            }
                            destiny.X = target.Center.X - NPC.width / 2;
                            destiny.Y = target.Center.Y - NPC.height / 2;
                        }
                    }
                    break;
            }
            //This is copypasted Corite AI
            if (hypnos.ai[0] == 10 && p2)
            {
                NPC.damage = 240;
                NPC.Calamity().canBreakPlayerDefense = true;
                NPC.TargetClosest(faceTarget: false);
                float num1058 = 0.3f;
                float num1059 = 8f;
                float scaleFactor3 = 300f;
                float num1060 = 800f;
                float num1061 = 60f;
                float num1062 = 2f;
                float num1063 = 1.8f;
                int num1064 = 0;
                float scaleFactor4 = 30f;
                float num1065 = 30f;
                float num1066 = 150f;
                float num1067 = 60f;
                float num1068 = 0.333333343f;
                float num1069 = 8f;
                bool flag61 = false;
                num1068 *= num1067;
                if (Main.expertMode)
                {
                    num1058 *= Main.GameModeInfo.KnockbackToEnemiesMultiplier;
                }
                float num248;
                if (corite1 == 0f)
                {
                    NPC.knockBackResist = num1058;
                    float scaleFactor5 = num1059;
                    Vector2 center6 = NPC.Center;
                    Vector2 center7 = Main.player[NPC.target].Center;
                    Vector2 value6 = center7 - center6;
                    Vector2 vector126 = value6 - Vector2.UnitY * scaleFactor3;
                    float num1075 = value6.Length();
                    value6 = Vector2.Normalize(value6) * scaleFactor5;
                    vector126 = Vector2.Normalize(vector126) * scaleFactor5;
                    bool flag62 = Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1);
                    if (corite4 >= 120f)
                    {
                        flag62 = true;
                    }
                    float num1076 = 100f;
                    flag62 = (flag62 && value6.ToRotation() > (float)Math.PI / num1076 && value6.ToRotation() < (float)Math.PI - (float)Math.PI / num1076);
                    if (num1075 > num1060 || !flag62)
                    {
                        NPC.velocity.X = (NPC.velocity.X * (num1061 - 1f) + vector126.X) / num1061;
                        NPC.velocity.Y = (NPC.velocity.Y * (num1061 - 1f) + vector126.Y) / num1061;
                        if (!flag62)
                        {
                            num248 = corite4;
                            corite4 = num248 + 1f;
                            if (corite4 == 120f)
                            {
                                NPC.netUpdate = true;
                            }
                        }
                        else
                        {
                            corite4 = 0f;
                        }
                    }
                    else
                    {
                        corite1 = 1f;
                        corite3 = value6.X;
                        corite4 = value6.Y;
                        NPC.netUpdate = true;
                    }
                }
                else if (corite1 == 1f)
                {
                    NPC.knockBackResist = 0f;
                    NPC.velocity *= num1063;
                    num248 = corite2;
                    corite2 = num248 + 1f;
                    if (corite2 >= num1062)
                    {
                        corite1 = 2f;
                        corite2 = 0f;
                        NPC.netUpdate = true;
                        Vector2 vector127 = new Vector2(corite3, corite4) + new Vector2(Main.rand.Next(-num1064, num1064 + 1), Main.rand.Next(-num1064, num1064 + 1)) * 0.04f;
                        vector127.Normalize();
                        vector127 = (NPC.velocity = vector127 * scaleFactor4);
                    }
                }
                else if (corite1 == 2f)
                {
                    NPC.knockBackResist = 0f;
                    float num1078 = num1065;
                    num248 = corite2;
                    corite2 = num248 + 1f;
                    bool flag63 = Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > num1066 && NPC.Center.Y > Main.player[NPC.target].Center.Y;
                    if ((corite2 >= num1078 && flag63) || NPC.velocity.Length() < num1069)
                    {
                        corite1 = 4f;
                        corite2 = 45f;
                        corite3 = 0f;
                        corite4 = 0f;
                        NPC.velocity /= 2f;
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        Vector2 center8 = NPC.Center;
                        Vector2 center9 = Main.player[NPC.target].Center;
                        Vector2 vec2 = center9 - center8;
                        vec2.Normalize();
                        if (vec2.HasNaNs())
                        {
                            vec2 = new Vector2(NPC.direction, 0f);
                        }
                        NPC.velocity = (NPC.velocity * (num1067 - 1f) + vec2 * (NPC.velocity.Length() + num1068)) / num1067;
                    }
                    if (flag61 && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                    {
                        corite1 = 3f;
                        corite2 = 0f;
                        corite3 = 0f;
                        corite4 = 0f;
                        NPC.netUpdate = true;
                    }
                }
                else if (corite1 == 4f)
                {
                    corite2 -= 9f;
                    if (corite2 <= 0f)
                    {
                        corite1 = 0f;
                        corite2 = 0f;
                        NPC.netUpdate = true;
                    }
                    NPC.velocity *= 0.95f;
                }
                if (flag61 && corite1 != 3f && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) < 64f)
                {
                    corite1 = 3f;
                    corite2 = 0f;
                    corite3 = 0f;
                    corite4 = 0f;
                    NPC.netUpdate = true;
                }
                if (corite1 != 3f)
                {
                    return;
                }
                NPC.position = NPC.Center;
                NPC.width = (NPC.height = 192);
                NPC.position.X -= NPC.width / 2;
                NPC.position.Y -= NPC.height / 2;
                NPC.velocity = Vector2.Zero;
                NPC.damage = NPC.GetAttackDamage_ScaledByStrength(80f);
                NPC.alpha = 255;
                num248 = corite2;
                corite2 = num248 + 1f;
                if (corite2 >= 3f)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                    NPC.life = 0;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }

        }

        //public void ChangePhase(int phasenum, bool reset1 = true, bool reset2 = true, bool reset4 = true)
        //{
        //    hypnos.ai[0] = phasenum;
        //    if (reset2)
        //    {
        //        NPC.ai[2] = 0;
        //    }
        //    if (reset4)
        //    {
        //        NPC.Calamity().newAI[4] = 0;
        //    }
        //    afterimages = false;
        //}

        public override void FindFrame(int frameHeight)
        {
        }

        internal float WidthFunction(float completionRatio) 
        {
            float ratio = 0.9f;
            if ((hypnos.ai[2] < 120 && hypnos.ai[0] == 6 && NPC.ai[1] >= 4) || (hypnos.ai[2] < 60 && hypnos.ai[0] == 6 && NPC.ai[1] < 4))
            {
                ratio = 0.2f;
            }
            else if (hypnos.ai[2] >= 60 && hypnos.ai[0] == 6 && NPC.ai[1] < 4)
            {
                ratio = 1.1f;
            }
            else if (hypnos.ai[0] == 10 && NPC.ai[1] > 0)
            {
                ratio = 1.8f;
            }
            else
            {
                ratio = 0.9f;
            }
            return ratio;
        }

        internal float BackgroundWidthFunction(float completionRatio) => WidthFunction(completionRatio) * 4f;

        public Color BackgroundColorFunction(float completionRatio) => hypnos.ModNPC<Hypnos>().ragetimer > 0 ? Color.IndianRed * 0.4f : Color.CornflowerBlue * 0.4f;

        internal Color ColorFunction(float completionRatio)
        {
            Color baseColor1 = Color.Cyan;
            Color baseColor2 = Color.Cyan;

            if (hypnos.ModNPC<Hypnos>().ragetimer > 0)
            {
                baseColor1 = baseColor2 = Color.Red;
            }
            else if ((hypnos.ai[2] < 120 && hypnos.ai[0] == 6 && NPC.ai[1] >= 4) || (hypnos.ai[2] < 60 && hypnos.ai[0] == 6 && NPC.ai[1] < 4))
            {
                baseColor1 = baseColor2 = Color.Orchid;
            }
            else if (hypnos.ai[2] >= 60 && hypnos.ai[0] == 6 && NPC.ai[1] < 4)
            {
                baseColor1 = baseColor2 = Color.Orange;
            }

                float fadeToWhite = MathHelper.Lerp(0f, 0.65f, (float)Math.Sin(MathHelper.TwoPi * completionRatio + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
            Color baseColor = Color.Lerp(baseColor1, Color.White, fadeToWhite);
            Color color = Color.Lerp(baseColor, baseColor2, ((float)Math.Sin(MathHelper.Pi * completionRatio + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f) * 0.8f) * 0.65f;
            color.A = 84;
            return color;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            SmokeDrawer.DrawSet(NPC.Center);
            if (!(hypnos.ai[0] == 11 && hypnos.ai[1] >= 31))
            {
                DrawHypnos(spriteBatch, screenPos, drawColor);
            }
            if (!p2)
            {
                drawchain(spriteBatch, screenPos, drawColor);
            }
            dolightning();
            return false;
        }

        public void dolightning()
        {            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nextneuron = Main.npc[i];
                if (nextneuron.active && nextneuron.type == ModContent.NPCType<AergiaNeuron>())
                {
                    if (hypnos.ai[0] == 6 && hypnos.ai[2] >= 40 + NPC.ai[1] * 6 && NPC.ai[1] < 11)
                    {
                        if (nextneuron.ai[1] == NPC.ai[1] + 1 && !(NPC.ai[1] == 3 && nextneuron.ai[1] == 4))
                        {
                            List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(NPC.Center, nextneuron.Center, 250290787);
                            PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                            PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);

                        }

                        if (NPC.ai[1] == 3 && nextneuron.ai[1] == 0)
                        {
                            List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(NPC.Center, nextneuron.Center, 250290787);
                            PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                            PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);

                        }
                    }
                    if (hypnos.ai[0] == 8)
                    {
                        if ((nextneuron.ai[1] == NPC.ai[1] + 1 && NPC.ai[1] < 11) || (NPC.ai[1] == 11 && nextneuron.ai[1] == 0))
                        {
                            List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(NPC.Center, nextneuron.Center, 250290787);
                            PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                            PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);
                        }
                        if (NPC.ai[1] == 11 && nextneuron.ai[1] == 0)
                        {
                            List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(NPC.Center, nextneuron.Center, 250290787);
                            PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                            PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);
                        }
                    }
                }
            }
            if ((NPC.ai[2] > (60 * NPC.ai[1]) + 10) && hypnos.ai[0] == 5 && !p2)
            {
                List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(plug.Center, NPC.Center, 250290787);
                PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);
            }
            if (hypnos.ai[0] == 10 && NPC.ai[2] > 0 && !(NPC.ai[1] <= 10 && NPC.ai[1] >= 8) && !p2)
            {
                List<Vector2> points = AresTeslaOrb.DetermineElectricArcPoints(plug.Center, plug.Center + (NPC.Center - plug.Center) * NPC.ai[2] / 60, 250290787);
                PrimitiveRenderer.RenderTrail(points, new(BackgroundWidthFunction, BackgroundColorFunction, smoothen: false), 90);
                PrimitiveRenderer.RenderTrail(points, new(WidthFunction, ColorFunction, smoothen: false), 90);
            }
        }

        public void doafterimages(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
             
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Color neurcolor = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red : drawColor;
            Color glowcolor = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red : Color.White;

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D glowmask = p2 ? ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/AergiaNeuron2_Glow").Value : ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/AergiaNeuron_Glow").Value;
            if (p2)
            {
                texture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/AergiaNeuron2").Value;
            }

            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterimageAmt = 7;


            if (CalamityConfig.Instance.Afterimages && afterimages)
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
            spriteBatch.Draw(texture, npcOffset, NPC.frame, neurcolor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            spriteBatch.Draw(glowmask, npcOffset, NPC.frame, glowcolor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
        }

        public void drawchain(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int heighoffset = 20;
            int heighoffsetin = 30;
            int innerdist = 70;
            int outerdist = 80;
            Vector2 leftleftplug = new Vector2(hypnos.Center.X - outerdist, hypnos.Center.Y + heighoffset);
            Vector2 leftplug = new Vector2(hypnos.Center.X - innerdist, hypnos.Center.Y + heighoffsetin);
            Vector2 rightplug = new Vector2(hypnos.Center.X + innerdist, hypnos.Center.Y + heighoffsetin);
            Vector2 rightrightplug = new Vector2(hypnos.Center.X + outerdist, hypnos.Center.Y + heighoffset);

            Vector2 pluglocation = hypnos.Center;

            switch (NPC.ai[1])
            {
                case 0:
                case 1:
                case 2:
                    pluglocation = leftleftplug;
                    break;
                case 3:
                case 4:
                case 5:
                    pluglocation = leftplug;
                    break;
                case 6:
                case 7:
                case 8:
                    pluglocation = rightplug;
                    break;
                case 9:
                case 10:
                case 11:
                    pluglocation = rightrightplug;
                    break;
            }

            if (hypnos.ai[0] == 10 && NPC.ai[2] > 0)
            {
                pluglocation = plug.Center + (NPC.Center - pluglocation) * NPC.ai[2] / 60;
            }

            Vector2 distToProj = NPC.Center;
            float projRotation = NPC.AngleTo(pluglocation) - 1.57f;
            bool doIDraw = true;
            Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/HypnosPlugCable").Value; //change this accordingly to your chain texture

            Color chaincolor = drawColor;
            if ((NPC.ai[2] > (60 * NPC.ai[1]) + 10) && hypnos.ai[0] == 5)
            {
                doIDraw = false;
            }
            while (doIDraw)
            {
                float distance = (pluglocation - distToProj).Length();
                if (distance > 10000f)
                {
                    break;
                }
                if (distance < (texture.Height + 1))
                {
                    doIDraw = false;
                }
                else if (!float.IsNaN(distance))
                {
                    distToProj += NPC.DirectionTo(pluglocation) * texture.Height;
                    Main.EntitySpriteDraw(texture, distToProj - Main.screenPosition,
                        new Rectangle(0, 0, texture.Width, texture.Height), chaincolor, projRotation,
                        Utils.Size(texture) / 2f, 1f, SpriteEffects.None, 0);
                }
            }
        }

        public void DrawHypnos(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
             
            if (NPC.ai[1] == 11)
            {
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (hypnos.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;

                Texture2D texture = TextureAssets.Npc[hypnos.type].Value;
                Texture2D glowmask = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/Hypnos_Glow").Value;
                Texture2D eyetexture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/Hypnos_Eye").Value;
                Texture2D pipestexture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/Hypnos_Pipes").Value;
                Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[hypnos.type] / 2));
                Color white = Color.White;
                float colorLerpAmt = 0.5f;
                int afterimageAmt = 7;

                Color eyecolor = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red : Lighting.GetColor((int)hypnos.position.X / 16, (int)hypnos.position.Y / 16);
                Color glowcolor = hypnos.ModNPC<Hypnos>().ragetimer > 0 ? Color.Red : Color.White;

                if (CalamityConfig.Instance.Afterimages && hypnos?.ModNPC<Hypnos>().afterimages == true)
                {
                    for (int i = 1; i < afterimageAmt; i += 2)
                    {
                        Color color1 = drawColor;
                        color1 = Color.Lerp(color1, white, colorLerpAmt);
                        color1 = hypnos.GetAlpha(color1);
                        color1 *= (float)(afterimageAmt - i) / 15f;
                        Vector2 offset = hypnos.oldPos[i] + new Vector2((float)hypnos.width, (float)hypnos.height) / 2f - screenPos;
                        offset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[hypnos.type])) * hypnos.scale / 2f;
                        offset += origin * hypnos.scale + new Vector2(0f, hypnos.gfxOffY);
                        spriteBatch.Draw(texture, offset, hypnos.frame, color1, hypnos.rotation, origin, hypnos.scale, spriteEffects, 0f);
                    }
                }

                Vector2 npcOffset = hypnos.Center - screenPos;
                npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[hypnos.type])) * hypnos.scale / 2f;
                npcOffset += origin * hypnos.scale + new Vector2(0f, hypnos.gfxOffY);
                spriteBatch.Draw(texture, npcOffset, hypnos.frame, Lighting.GetColor((int)hypnos.position.X / 16, (int)hypnos.position.Y / 16), hypnos.rotation, origin, hypnos.scale, spriteEffects, 0f);
                spriteBatch.Draw(glowmask, npcOffset, hypnos.frame, glowcolor, hypnos.rotation, origin, hypnos.scale, spriteEffects, 0f);
                spriteBatch.Draw(eyetexture, npcOffset, hypnos.frame, eyecolor, hypnos.rotation, origin, hypnos.scale, spriteEffects, 0f);

                if (!hypnos.ModNPC<Hypnos>().p2)
                {
                    spriteBatch.EnterShaderRegion();
                    var shader = Filters.Scene["CalRemix:HoloShieldShader"].GetShader().Shader;
                    shader.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly);
                    shader.Parameters["screenPosition"].SetValue(Main.screenPosition);
                    shader.Parameters["screenSize"].SetValue(Main.ScreenSize.ToVector2());
                    shader.Parameters["resolution"].SetValue(0.9f);
                    shader.Parameters["alpha"].SetValue(NPC.localAI[0]);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shader, Main.GameViewMatrix.TransformationMatrix);


                    Texture2D shield = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Hypnos/HypnosShield").Value;
                    spriteBatch.Draw(shield, npcOffset - new Vector2(0, 40 * hypnos.scale), null, Color.White, NPC.rotation, shield.Size() / 2, hypnos.scale, spriteEffects, 0f);

                    spriteBatch.ExitShaderRegion();
                    spriteBatch.Draw(pipestexture, npcOffset, hypnos.frame, Lighting.GetColor((int)hypnos.position.X / 16, (int)hypnos.position.Y / 16), hypnos.rotation, origin, hypnos.scale, spriteEffects, 0f);
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (initialized)
                doafterimages(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            //writer.Write(hypnos);

            writer.Write(hypnosafter);
            writer.Write(afterimages);
            //writer.Write(initialized);
            writer.Write(p2);
            writer.Write(enraged);

            writer.Write(corite1);
            writer.Write(corite2);
            writer.Write(corite3);
            writer.Write(corite4);
            writer.Write(rottimer);

            writer.Write(offx);
            writer.Write(offy);
            writer.Write(lvf);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //hypnos = reader.ReadByte(hypnos);

			hypnosafter = reader.ReadBoolean();
			afterimages = reader.ReadBoolean();
            
            //initialized = reader.ReadBoolean();
            p2 = reader.ReadBoolean();
            enraged = reader.ReadBoolean();

            corite1 = reader.ReadInt32();
            corite2 = reader.ReadInt32();
            corite3 = reader.ReadInt32();
            corite4 = reader.ReadInt32();
            rottimer = reader.ReadInt32();

            offx = reader.ReadInt32();
            offy = reader.ReadInt32();
            lvf = reader.ReadInt32();
        }
    }
}