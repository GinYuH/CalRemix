﻿using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Hostile;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class EvilAnimatronic : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Evil Animatronic");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 100;
            NPC.width = 34;
            NPC.height = 52;
            NPC.defense = 40;
            NPC.lifeMax = 1600;
            NPC.knockBackResist = 0.2f;
            NPC.value = Item.buyPrice(gold: 5, silver: 20);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.Remix().GreenAI[0] = 0;
            NPC.Remix().GreenAI[1] = 0;
            NPC.Remix().GreenAI[2] = 0;
        }

        public override void AI()
        {
            NPC.Remix().GreenAI[0]++;
            {
                CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedHerplingAI(NPC, Mod);
                if (NPC.HasPlayerTarget)
                {
                    if (Collision.CanHitLine(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                    {
                        NPC.Remix().GreenAI[2] = 0;
                        if (NPC.Remix().GreenAI[0] % (90 + Main.rand.Next(0, 21)) == 0)
                        {
                            Vector2 dist = Main.player[NPC.target].position - NPC.position;
                            dist.Normalize();
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dist * 14, ModContent.ProjectileType<PizzaWheelHostile>(), NPC.damage, 0, Main.myPlayer, NPC.whoAmI);
                            SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, NPC.Center);
                        }
                        if (NPC.Remix().GreenAI[0] % 120 == 0)
                        {
                            bool left = Main.rand.NextBool();
                            float variance = Main.rand.NextFloat(-20, 20);
                            if (Main.rand.NextBool())
                            {
                                for (int i = -10; i < 10; i++)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(Main.player[NPC.target].Center.X + 1200 * (left ? 1 : -1), Main.player[NPC.target].Center.Y + i * 120 + variance), new Vector2(-16 * (left ? 1 : -1), 0), ModContent.ProjectileType<PizzaWheelHostile>(), NPC.damage, 0, Main.myPlayer, NPC.whoAmI, 2);
                                    Main.projectile[p].timeLeft = 180;
                                }
                            }
                            else
                            {
                                for (int i = -10; i < 10; i++)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(Main.player[NPC.target].Center.X + i * 120 + variance, Main.player[NPC.target].Center.Y + 800 * (left ? 1 : -1)), new Vector2(0, -8 * (left ? 1 : -1)), ModContent.ProjectileType<PizzaWheelHostile>(), NPC.damage, 0, Main.myPlayer, NPC.whoAmI, 2);
                                    Main.projectile[p].timeLeft = 270;
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC.Remix().GreenAI[2]++;
                        if (NPC.Remix().GreenAI[2] >= 240 + Main.rand.Next(0, 31))
                        {
                            Main.player[NPC.target].AddBuff(BuffID.Blackout, 600);
                            SoundEngine.PlaySound(SoundID.ScaryScream with { Pitch = SoundID.ScaryScream.Pitch + 1}, Main.player[NPC.target].Center);
                            NPC.Remix().GreenAI[2] = 0;
                        }
                    }
                }
            }
            NPC.spriteDirection = -NPC.direction;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y != 0)
            {
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 7)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight * 4;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG)
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.5f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), new Fraction(3, 4), 1, 2);
        }
    }
}
