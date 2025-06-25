﻿using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Items.Materials;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.Items.Critters;

namespace CalRemix.Content.NPCs
{
    public class MinnowsPrime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.CountsAsCritter[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.1f;
            NPC.noGravity = true;
            NPC.damage = 0;
            NPC.width = 36;
            NPC.height = 22;
            NPC.defense = 0;
            NPC.lifeMax = 500;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.chaseable = false;
            NPC.catchItem = (short)ModContent.ItemType<MinnowsPrimeItem>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement (CalRemixHelper.LocalText($"Bestiary.{Name}").Value),
            });
        }

        public override void AI()
        {
            CalamityMod.NPCs.CalamityAIs.CalamityRegularEnemyAIs.CalamityRegularEnemyAI.PassiveSwimmingAI(NPC, Mod, 3, 150f, 0.25f, 0.15f, 6f, 6f, 0.05f);
            NPC.noGravity = true;
            bool flag14 = false;
            if (NPC.direction == 0)
            {
                NPC.TargetClosest(true);
            }
            if (NPC.wet)
            {
                NPC.TargetClosest(false);
                if (Main.player[NPC.target].wet && !Main.player[NPC.target].dead &&
                    (Main.player[NPC.target].Center - NPC.Center).Length() < 150f)
                {
                    flag14 = true;
                }
                if ((!Main.player[NPC.target].wet || Main.player[NPC.target].dead) && flag14)
                {
                    flag14 = false;
                }
                if (!flag14)
                {
                    if (NPC.collideX || NPC.velocity.X == 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X * -3f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                    if (NPC.collideY)
                    {
                        NPC.netUpdate = true;
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -3f;
                            NPC.directionY = -1;
                            NPC.ai[0] = -1f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                            NPC.directionY = 1;
                            NPC.ai[0] = 1f;
                        }
                    }
                }
                if (flag14)
                {
                    NPC.TargetClosest(true);
                    NPC.velocity.X = NPC.velocity.X - (float)NPC.direction * 0.25f;
                    NPC.velocity.Y = NPC.velocity.Y - (float)NPC.directionY * 0.15f;
                    if (NPC.velocity.X > 6f)
                    {
                        NPC.velocity.X = 6f;
                    }
                    if (NPC.velocity.X < -6f)
                    {
                        NPC.velocity.X = -6f;
                    }
                    if (NPC.velocity.Y > 6f)
                    {
                        NPC.velocity.Y = 6f;
                    }
                    if (NPC.velocity.Y < -6f)
                    {
                        NPC.velocity.Y = -6f;
                    }
                    NPC.direction *= -1;
                }
                else
                {
                    NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.1f;
                    if (NPC.velocity.X < -2.5f || NPC.velocity.X > 2.5f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.95f;
                    }
                    if (NPC.ai[0] == -1f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                        if ((double)NPC.velocity.Y < -0.3)
                        {
                            NPC.ai[0] = 1f;
                        }
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                        if ((double)NPC.velocity.Y > 0.3)
                        {
                            NPC.ai[0] = -1f;
                        }
                    }
                }
                int num258 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                int num259 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                if (Main.tile[num258, num259 - 1].LiquidAmount > 128)
                {
                    if (Main.tile[num258, num259 + 1].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                    else if (Main.tile[num258, num259 + 2].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                if ((double)NPC.velocity.Y > 0.4 || (double)NPC.velocity.Y < -0.4)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.95f;
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.94f;
                    if ((double)NPC.velocity.X > -0.2 && (double)NPC.velocity.X < 0.2)
                    {
                        NPC.velocity.X = 0f;
                    }
                }
                NPC.velocity.Y = NPC.velocity.Y + 0.3f;
                if (NPC.velocity.Y > 10f)
                {
                    NPC.velocity.Y = 10f;
                }
                NPC.ai[0] = 1f;
            }
            NPC.rotation = NPC.velocity.X * 0.05f;
            if ((double)NPC.rotation < -0.1)
            {
                NPC.rotation = -0.1f;
            }
            if ((double)NPC.rotation > 0.1)
            {
                NPC.rotation = 0.1f;
                return;
            }
            NPC.spriteDirection = NPC.direction = (int)Math.Sign(NPC.velocity.X);
        }

        public override void FindFrame(int frameHeight)
        {
            if (!NPC.wet && !NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDungeon && spawnInfo.Water && NPC.downedMoonlord)
            {
                return SpawnCondition.CaveJellyfish.Chance * 2f;
            }
            return 0f;
        }
        public override void OnKill()
        {
            if (Main.rand.NextBool(10))
            {
                CalRemixHelper.SpawnNewNPC(Entity.GetSource_Death(), NPC.Center, ModContent.NPCType<KingMinnowsPrime>());
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ItemID.Ectoplasm, 1, 36, 57));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Necroplasm>(), 1, 7, 15));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityMod.Dusts.CalamityDusts.Necroplasm, hit.HitDirection, -1f, 0, default, 1f);
            }
        }
    }
}
