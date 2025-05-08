using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs;
using Terraria.Audio;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System.Threading;
using Terraria.DataStructures;
using CalRemix.Content.Items.Placeables.Banners;
using System.Collections.Generic;
using Terraria.GameContent;
using CalRemix.Content.Items.Pets;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Ammo;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class WaterloggedEffigy : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 200;
            NPC.width = 28;
            NPC.height = 42;
            NPC.defense = 5;
            NPC.lifeMax = 10000;
            NPC.value = 1000;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = true;
            Banner = Type;
            BannerItem = ModContent.ItemType<WaterloggedEffigyBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void AI()
        {
            if (Main.rand.NextBool(8))
            {
                Gore bubble = Gore.NewGorePerfect(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.2f + Main.rand.NextVector2Circular(1f, 1f), 411);
                bubble.timeLeft = 8 + Main.rand.Next(6);
                bubble.scale = Main.rand.NextFloat(0.6f, 1f);
                bubble.type = Main.rand.NextBool(3) ? 412 : 411;
            }
            switch (NPC.ai[0])
            {
                case 0:
                    {
                        NPC.ai[1]++;
                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                        NPC.velocity.X *= 0.9f;
                        NPC.noGravity = false;
                        NPC.TargetClosest();
                        if (Target.Distance(NPC.Center) < 4000 && Target.active && NPC.ai[1] > 120)
                        {
                            NPC.ai[0] = 1;
                            NPC.ai[1] = 0;
                        }
                    }
                    break;
                case 1:
                    {
                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                        if (NPC.ai[2] == 0)
                        {
                            ThreadPool.QueueUserWorkItem(_ => FindWater(NPC, 500));
                            NPC.ai[2] = 1;
                        }
                        bool water = false;
                        if (NPC.Calamity().newAI[0] != 0)
                        {
                            water = true;
                        }
                        NPC.TargetClosest();
                        if (!Target.active)
                        {
                            NPC.ai[0] = 0;
                        }
                        else if (Target.Distance(NPC.Center) < 300 && water && !Target.wet)
                        {
                            NPC.ai[0] = 2;
                        }
                        else
                        {
                            if (NPC.velocity.Y == 0)
                            {
                                NPC.velocity.Y = -10;
                            }
                            NPC.velocity.X = NPC.DirectionTo(Target.Center).X.DirectionalSign();
                        }
                    }
                    break;
                case 2:
                    {
                        NPC.noTileCollide = true;
                        NPC.noGravity = true;
                        NPC.damage = 0;
                        NPC.TargetClosest();
                        if (!Target.active)
                        {
                            NPC.ai[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.damage = NPC.defDamage;
                        }
                        else
                        {
                            if (NPC.ai[1] == 0)
                            {
                                Vector2 dest = Target.Center - Vector2.UnitY * 200;
                                NPC.velocity = NPC.DirectionTo(dest) * 22;
                                if (NPC.Distance(dest) < 40)
                                {
                                    NPC.ai[1] = 1;
                                }
                            }
                            else
                            {
                                NPC.velocity = NPC.DirectionTo(Target.Center) * 22;
                                if (NPC.Distance(Target.Center) < 40)
                                {
                                    NPC.ai[1] = 0;
                                    NPC.ai[0] = 3;
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        NPC.damage = 0;
                        if (!Target.active)
                        {
                            NPC.ai[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.damage = NPC.defDamage;
                        }
                        else
                        {
                            Vector2 dest = new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
                            NPC.ai[1]++;
                            NPC.Center = Vector2.Lerp(NPC.Center, dest, Utils.GetLerpValue(0, 120, NPC.ai[1], true));
                            Target.position = NPC.Center;
                            if (Target.wet)
                            {
                                Target.breath -= 4;
                                if (Target.breath < 0)
                                    Target.breath = 0;
                            }
                        }
                    }
                    break;
            }
        }

        public static bool FindWater(NPC n, int dist)
        {
            Point pos = n.Center.ToTileCoordinates();
            List<Point> validTiles = new List<Point>();
            for (int i = pos.X - dist; i <= pos.X + dist; i++)
            {
                for (int j = pos.Y - dist; j <= pos.Y + dist; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.LiquidAmount == 255)
                    {
                        Tile t2 = CalamityUtils.ParanoidTileRetrieval(i, j + 1);
                        if (t2.LiquidAmount == 255)
                        {
                            validTiles.Add(new Point(i, j));
                        }
                    }
                }
            }
            Vector2 closest = Vector2.Zero;
            for (int i = 0; i < validTiles.Count; i++)
            {
                if (validTiles[i].ToWorldCoordinates().Distance(n.Center) < closest.Distance(n.Center) || closest == Vector2.Zero)
                {
                    closest = validTiles[i].ToWorldCoordinates();
                }
            }
            if (closest != Vector2.Zero)
            {

                n.Calamity().newAI[0] = closest.X;
                n.Calamity().newAI[1] = closest.Y;
            }
            return closest != Vector2.Zero;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y > 0)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (NPC.velocity.Y < 0)
            {
                NPC.frame.Y = frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG || NPC.AnyNPCs(Type))
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), 1, 4, 6);
            npcLoot.Add(ModContent.ItemType<Glockarina>(), 20);
            npcLoot.Add(ModContent.ItemType<CursedCartridge>(), 20);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Water, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Water, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
