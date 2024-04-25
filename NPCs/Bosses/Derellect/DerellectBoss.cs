#region Derrick Code
/*
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using CalRemix.Items;
using CalRemix.Items.Placeables;
namespace CalRemix.NPCs.Bosses.Derellect
{
    [AutoloadBossHead]
    internal class DerellectBoss : ModNPC
    {
        public bool p2 = false;
        public bool p3 = false;

        public Particle aura;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Derellect");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.LifeMaxNERB(42000, 55000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 240;
            NPC.height = 200;
            NPC.boss = true;
            NPC.defense = 90;
            NPC.Calamity().canBreakPlayerDefense = true;
            Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/SignalInterruption");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                new FlavorTextBestiaryInfoElement("A mother computer designed as a simulacrum of a grotesque creature. Though scraped for parts, it still barely operates.")
            });
        }

        public override void AI()
        {
            if (Main.getGoodWorld)
            {
                NPC.scale = 1.75f;
            }
            //Boss zen
            Main.player[Main.myPlayer].Calamity().isNearbyBoss = true;
            Main.player[Main.myPlayer].AddBuff(BuffType<CalamityMod.Buffs.StatBuffs.BossEffects>(), 10, true);
            //Handle transitioning to phase 2
            if (NPC.CountNPCS(NPCType<SignalDrone>()) <= 0 && NPC.ai[0] != 0)
            {
                if (!p3)
                ChangePhase(5);
                p3 = true;
            }
            Player target = Main.player[NPC.target];

            // heal
            if (NPC.AnyNPCs(NPCType<SignalDrone>()))
            {
                NPC.localAI[0]++;
                float healfactor = 60;
                if (NPC.life < NPC.lifeMax * 0.5f)
                {
                    healfactor = 30;
                }
                if (NPC.life < NPC.lifeMax * 0.25f)
                {
                    healfactor = 15;
                }
                if (NPC.life < NPC.lifeMax * 0.1f)
                {
                    healfactor = 5;
                }
                if (NPC.localAI[0] % healfactor == 0 && NPC.life < NPC.lifeMax)
                {
                    NPC.HealEffect(NPC.lifeMax / 10);
                    NPC.life += Math.Clamp(NPC.lifeMax / 10, 0, NPC.lifeMax - NPC.life);
                }
            }

            switch (NPC.ai[0])
            {
                case 0: //spawn drones
                    {
                        NPC.ai[1]++;
                        if (NPC.ai[1] == 1)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<SignalDrone>(), 0, NPC.whoAmI, i);
                            }
                        }
                        if (NPC.ai[1] == 10)
                        {
                            ChangePhase(1);
                        }
                    }
                    break;
                case 1: // lightning cage
                    {
                        if (NPC.ai[1] < 210)
                        {
                            FlyYouFool();
                        }
                        NPC.ai[1]++;
                        if (NPC.ai[1] == 210)
                        {
                            Vector2 dist = target.Center - NPC.Center;
                            dist.Normalize();
                            NPC.velocity = dist * 26f;
                            SoundEngine.PlaySound(CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath.DashSound, NPC.Center);
                        }
                        if (NPC.ai[1] >= 270)
                        {
                            ChangePhase(3);
                        }
                    }
                    break;
                case 2: // pool
                    {
                        FlyYouFool(900);
                        NPC.ai[1]++;
                        if (NPC.ai[1] >= 300)
                        {
                            ChangePhase(3);
                        }
                    }
                    break;
                case 3: // bottom clusters
                    {
                        FlyYouFool(specificY: -400);
                        NPC.ai[1]++;
                        if (NPC.ai[1] >= 480)
                        {
                            ChangePhase(4);
                        }
                    }
                    break;
                case 4: // snake
                    {
                        FlyYouFool(-700);
                        NPC.ai[1]++;
                        if (NPC.ai[1] >= 480)
                        {
                            ChangePhase(1);
                        }
                    }
                    break;
                case 5: // sputter
                    {
                        NPC.ai[1]++;
                        if (NPC.ai[1] < 90)
                        {
                            FlyYouFool();
                        }
                        else if (NPC.ai[1] < 270)
                        {
                            NPC.velocity = Vector2.Zero;
                            NPC.ai[2]++;
                            if (NPC.ai[2] >= 5)
                            {
                                int xpseed = 26;
                                Vector2 velocity = new Vector2(Main.rand.Next(-xpseed, xpseed), Main.rand.Next(-20, 0));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ProjectileID.SaucerScrap, 40, 0f, Main.myPlayer);
                                NPC.ai[2] = 0;
                            }
                        }
                        else if (NPC.ai[1] >= 270)
                        {
                            ChangePhase(6);
                        }
                    }
                    break;
                case 6: // deathray
                    {
                        NPC.ai[1]++;
                        if (NPC.ai[1] < 10)
                        {
                            NPC.velocity.Y = -10;
                        }
                        if (NPC.ai[1] == 20)
                        {
                            NPC.velocity.Y = 0;
                            NPC.velocity.X = target.Center.X - NPC.Center.X > 0 ? 10 : -10;
                            SoundEngine.PlaySound(SoundID.Zombie104, NPC.Center);
                        }
                        else if (NPC.ai[1] >= 180)
                        {
                            ChangePhase(7);
                        }
                        else
                        {
                            NPC.velocity.X = target.Center.X - NPC.Center.X > 0 ? 10 : -10;
                        }
                    }
                    break;
                case 7: // wave
                    {
                        float phasetime = 180;
                        if (NPC.ai[1] < 100)
                        {
                            FlyYouFool(target.position.X - NPC.position.X > 0 ? -700 : 700);
                        }
                        if (NPC.Calamity().newAI[0] == 0)
                        {
                            NPC.Calamity().newAI[0] = Main.rand.Next(-2, 2);
                        }
                        NPC.ai[1]++;
                        Main.NewText(NPC.ai[1]);
                        if (NPC.ai[1] > 100)
                        {
                            if (NPC.ai[3] == 0)
                            {
                                NPC.ai[3] = target.position.X - NPC.position.X > 0 ? 1 : -1;
                            }
                            NPC.velocity = Vector2.Zero;
                            NPC.ai[2]++;
                            if (NPC.ai[2] % 5 == 0)
                            {
                                Vector2 velocity = new Vector2(10 * NPC.ai[3], ((NPC.ai[2] + NPC.Calamity().newAI[0]) / phasetime) * 50 - 5);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ProjectileID.DeathLaser, 62, 0, Main.myPlayer);
                            }
                        }
                        if (NPC.ai[1] >= phasetime)
                        {
                            ChangePhase(5);
                        }
                    }
                    break;
            }
        }

       public void ChangePhase(int phasenum, bool reset1 = true, bool reset2 = true, bool reset3 = true)
        {
            NPC.ai[0] = phasenum;
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
                NPC.ai[3] = 0;
            }
            NPC.Calamity().newAI[0] = 0;
            NPC.Calamity().newAI[1] = 0;
            NPC.Calamity().newAI[2] = 0;
            NPC.Calamity().newAI[3] = 0;
            NPC.damage = 0;
            NPC.Calamity().canBreakPlayerDefense = false;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC neuron = Main.npc[i];
                if (neuron.type == NPCType<SignalDrone>())
                {
                    neuron.ai[2] = 0;
                    neuron.ai[3] = 0;
                    neuron.Calamity().newAI[0] = 0;
                    neuron.Calamity().newAI[1] = 0;
                    neuron.Calamity().newAI[2] = 0;
                    neuron.Calamity().newAI[3] = 0;
                    neuron.ModNPC<SignalDrone>().offx = 0;
                    neuron.ModNPC<SignalDrone>().offy = 0;
                }
            }
            NPC.TargetClosest();
        }

        public void FlyYouFool(int specificX = 0, int specificY = 0)
        {
            if (specificX != 0 || specificY != 0)
            {
                Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X + specificX, Main.player[NPC.target].Center.Y + specificY);
                Vector2 distanceFromDestination = playerpos - NPC.Center;
                CalamityUtils.SmoothMovement(NPC, 100f, distanceFromDestination, 20, 1, true);

            }
            else
            {
                NPC.damage = 124;
                NPC.Calamity().canBreakPlayerDefense = true;
                float speed = p3 ? 12f : 10f;
                Vector2 playerpos = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y);
                Vector2 distanceFromDestination = playerpos - NPC.Center;
                CalamityMod.CalamityUtils.SmoothMovement(NPC, 100f, distanceFromDestination, speed, 1.01f, false);
            }
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
                Texture2D glowmask = Request<Texture2D>("CalRemix/NPCs/Bosses/Derellect/DerellectGlow").Value;
                Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
                Color white = Color.White;
                float colorLerpAmt = 0.5f;
                int afterimageAmt = 7;

                Vector2 npcOffset = NPC.Center - screenPos;
                npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(texture, npcOffset, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(glowmask, npcOffset, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            }
            return true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int a, float bossLifeScale, float e)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalOnly = new(new Conditions.NotExpert());
            npcLoot.Add(normalOnly);

            // Trophies
            npcLoot.Add(ItemType<DerellectTrophyInv>(), 10);
            npcLoot.Add(ItemType<DerellectMask>(), 7);

            // Lore item
            npcLoot.Add(ItemDropRule.ByCondition(DropHelper.If(() => !RemixDowned.downedDerellect), ItemType<KnowledgeDerellect>()));
        }

        public override void OnKill()
        {
            RemixDowned.downedDerellect = true;
            CalRemixWorld.UpdateWorldBool();
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect1").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect1").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect2").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect3").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect4").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect5").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect6").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect6").Type);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(p2);
            writer.Write(p3);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            p2 = reader.ReadBoolean();
            p3 = reader.ReadBoolean();
        }
    }
}
*/
#endregion