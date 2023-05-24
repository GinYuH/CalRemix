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
using CalRemix.Projectiles;

namespace CalRemix.NPCs.Bosses
{
    internal class SignalDrone : ModNPC
    {
        public bool initialized = false;

        public NPC father;

        public int offx;
        public int offy;
        public float lvf = 1; //laser velocity factor

        Vector2 destiny;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Signal Drone");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.LifeMaxNERB(20000, 22000);
            NPC.damage = 50;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 40;
            NPC.height = 40;
            NPC.dontTakeDamage = false;
            NPC.defense = 22;
        }

        public override void AI()
        {
            if (Main.npc[(int)NPC.ai[0]].active && Main.npc[(int)NPC.ai[0]].type == ModContent.NPCType<DerellectBoss>())
            {
                father = Main.npc[(int)NPC.ai[0]];
            }
            else
            {
                NPC.StrikeInstantKill();
            }
            float AIState = father.ai[0];
            Player target = Main.player[father.target];
            switch (AIState)
            {
                case 0:
                    {
                        float pushVelocity = 0.2f;
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].active)
                            {
                                if (i != NPC.whoAmI && Main.npc[i].type == NPC.type)
                                {
                                    if (Vector2.Distance(NPC.Center, Main.npc[i].Center) < 80f)
                                    {
                                        if (NPC.position.X < Main.npc[i].position.X)
                                            NPC.velocity.X -= pushVelocity * Main.rand.Next(-1, 2);
                                        else
                                            NPC.velocity.X += pushVelocity * Main.rand.Next(-1, 2);

                                        if (NPC.position.Y < Main.npc[i].position.Y)
                                            NPC.velocity.Y -= pushVelocity * Main.rand.Next(-1, 2);
                                        else
                                            NPC.velocity.Y += pushVelocity * Main.rand.Next(-1, 2);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if (NPC.ai[2] == 0)
                        {
                            int maxdist = 600;
                            offx = (int)(target.Center.X - Main.rand.Next(-maxdist, maxdist + 1));
                            offy = (int)(target.Center.Y - Main.rand.Next(-maxdist, maxdist + 1));
                        }
                        NPC.ai[2]++;

                        if (NPC.ai[2] > 60)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        CalamityUtils.SmoothMovement(NPC, 100, new Vector2(offx, offy) - target.Center, 16f, 1.01f, true);
                    }
                    break;
                case 2:
                    {
                       /* 
                        * FUCK
                        * if (NPC.ai[2] == 0)
                        {
                            for (int i = 0; i < Main.maxNPCs; i++)
                            {
                                NPC neuron = Main.npc[i];
                                if (neuron.life >= Main.npc[(int)NPC.ai[3]].life && neuron.type == Type)
                                {
                                    NPC.ai[3] = neuron.whoAmI;
                                }
                            }
                            NPC thestron = Main.npc[(int)NPC.ai[3]];
                            Main.NewText("The strongest is a " + thestron.FullName);
                            offx = thestron.Center.X - target.Center.X > 0 ? -600 : 600;
                            offy = Main.rand.Next(-20, 21);                            
                        }
                        NPC.ai[2]++;
                        NPC strongest = Main.npc[(int)NPC.ai[3]];
                        if (strongest.type != Type)
                        {
                            father.ai[1] = 300;
                        }
                        if (strongest == NPC)
                        {
                            CalamityUtils.SmoothMovement(NPC, 100, new Vector2(offx, 0) - target.Center, 16f, 1.01f, true);
                            if (NPC.ai[2] == 60 || (NPC.ai[2] == 180 && CalamityWorld.revenge))
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(offx / 100, 0), ProjectileID.DeathLaser, 62, 0, Main.myPlayer);
                            }
                        }
                        else
                        {
                            CalamityUtils.SmoothMovement(NPC, 100, new Vector2(-offx, offy) - target.Center, 16f, 1.01f, true);
                        }*/
                    }
                    break;
            }
        }


        public override void OnKill()
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SignalDrone1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SignalDrone2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SignalDrone3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SignalDrone4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SignalDrone5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SignalDrone5").Type, 1f);
                }
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
        }
    }
}