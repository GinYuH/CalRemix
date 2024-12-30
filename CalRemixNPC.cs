using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Fearmonger;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.Crags;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.GreatSandShark;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.PlagueEnemies;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.SulphurousSea;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Sounds;
using CalamityMod.Tiles.Ores;
using CalamityMod.World;
using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Items.Pets;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Placeables.MusicBoxes;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.Items.Potions.Recovery;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Weapons.Stormbow;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Bosses.Poly;
using CalRemix.Content.NPCs.Bosses.Wulfwyrm;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.NPCs.TownNPCs;
using CalRemix.Content.Projectiles.Accessories;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Tiles;
using CalRemix.Core.Biomes;
using CalRemix.Core.World;
using CalRemix.UI;
using CalRemix.UI.Anomaly109;
using CalRemix.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using static Terraria.ModLoader.ModContent;

namespace CalRemix
{
    public class CalRemixNPC : GlobalNPC
    {
        bool SlimeBoost = false;
        public bool vBurn = false;
        public bool grappled = false;
        public bool witherDebuff = false;
        public int wither = 0;
        public int shreadedLungs = 0;
        public int clawed = 0;
        private int crabSay, slimeSay, guardSay, yharSay, jaredSay = 0;
        public Vector2 clawPosition = Vector2.Zero;
        public float shadowHit = 1;
        public static int wulfyrm = -1;
        public static int pyrogen = -1;
        public static int hypnos = -1;
        public static int aspidCount = 0;
        private bool guardRage, guardOver, yharRage = false;
        public float[] storedAI = { 0f, 0f, 0f, 0f };
        public float[] storedCalAI = { 0f, 0f, 0f, 0f };
        public float[] storedLocalAI = { 0f, 0f, 0f, 0f };
        public float[] storedGreenAI = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        public static HelperMessage CystMessage;
        public float[] GreenAI = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0];
        public override bool InstancePerEntity => true;

        public List<int> BossSlimes = new List<int>
        {
            NPCID.KingSlime,
            NPCID.QueenSlimeBoss,
            NPCType<AstrumAureus>(),
            NPCType<EbonianPaladin>(),
            NPCType<CrimulanPaladin>(),
            NPCType<SplitEbonianPaladin>(),
            NPCType<SplitCrimulanPaladin>(),
            NPCType<SlimeGodCore>(),
            NPCType<LifeSlime>(),
            NPCType<CragmawMire>()
        };

        public List<int> Slimes = new List<int>
        {
            1, 16, 59, 71, 81, 138, 121, 122, 141, 147, 183, 184, 204, 225, 244, 302, 333, 335, 334, 336, 537,
            NPCID.SlimeSpiked,
            NPCID.QueenSlimeMinionBlue,
            NPCID.QueenSlimeMinionPink,
            NPCID.QueenSlimeMinionPurple,
            NPCType<AeroSlime>(),
            NPCType<CalamityMod.NPCs.Astral.AstralSlime>(),
            NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>(),
            NPCType<BloomSlime>(),
            NPCType<CalamityMod.NPCs.Crags.InfernalCongealment>(),
            NPCType<PerennialSlime>(),
            NPCType<CryoSlime>(),
            NPCType<GammaSlime>(),
            NPCType<IrradiatedSlime>(),
            NPCType<AuricSlime>(),
            NPCType<CorruptSlimeSpawn>(),
            NPCType<CorruptSlimeSpawn2>(),
            NPCType<CrimsonSlimeSpawn>(),
            NPCType<CrimsonSlimeSpawn2>()
        };
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                CystMessage = HelperMessage.New("CystDeath", "See!", "").NeedsActivation();
            }
        }

        public static void AddModBiomeToBestiary(int curNPC, int npcID, ModBiome biome, BestiaryEntry entry)
        {
            if (curNPC == npcID)
                entry.Info.Add(LoaderManager.Get<BiomeLoader>().Get(biome.Type).ModBiomeBestiaryInfoElement);
        }
        public static void AddModBiomeToBestiary(int curNPC, int npcID, int biomeID, BestiaryEntry entry)
        {
            if (curNPC == npcID)
                entry.Info.Add(LoaderManager.Get<BiomeLoader>().Get(biomeID).ModBiomeBestiaryInfoElement);
        }

        public static void RemoveVanillaBiomeFromBestiary(int curNPC, int npcID, SpawnConditionBestiaryInfoElement biome, BestiaryEntry entry)
        {
            if (curNPC == npcID)
                if (entry.Info.Contains(biome))
                    entry.Info.Remove(biome);
        }

        public static void ConvertPlagueEnemy(int curNPC, int npcID, BestiaryEntry entry)
        {
            RemoveVanillaBiomeFromBestiary(curNPC, npcID, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle, entry);
            RemoveVanillaBiomeFromBestiary(curNPC, npcID, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle, entry);
            AddModBiomeToBestiary(curNPC, npcID, GetInstance<PlagueBiome>().Type, entry);
        }


        public override bool PreAI(NPC npc)
        {
            if (CalamityUtils.CountProjectiles(ProjectileType<Claw>()) <= 0)
            {
                clawed = 0;
            }
            clawed--;
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();

            if (player.pathogenSoul)
            {
                npc.canGhostHeal = true;
            }

            bool assortgel = player.assortegel;
            bool amalgam = player.amalgel;
            bool godfather = player.godfather;
            bool tvo = player.tvo;

            bool bossrush = CalamityMod.Events.BossRushEvent.BossRushActive;
            bool normalSlime = Slimes.Contains(npc.type);
            bool bossSlime = BossSlimes.Contains(npc.type);

            // Kill passive slimes if none of the accessories are on
            if (npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost && !assortgel && !amalgam && !godfather)
            {
                npc.active = false;
                return false;
            }
            // Godfather causes slimes to try to assimilate into goozma
            if (godfather && !bossrush)
            {
                if (normalSlime || bossSlime)
                {
                    if (!npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost)
                    {
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.friendly = true;
                        npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.ownedProjectileCounts[ProjectileType<CriticalSlimeCore>()] == 1)
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile target = Main.projectile[i];
                            if (target.type == ProjectileType<CriticalSlimeCore>())
                            {
                                Vector2 direction = target.Center - npc.Center;
                                direction.Normalize();
                                npc.velocity = direction * 20;
                                npc.noTileCollide = true;
                            }
                        }
                    }
                }
            }
            // Behavior if you DONT have godfather
            else
            {
                // If other passive slime accessories, and the slime isn't a boss, target the player's target
                if (normalSlime && (assortgel || amalgam))
                {
                    if (!npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost)
                    {
                        if (amalgam)
                        {
                            npc.lifeMax = (int)(npc.lifeMax * 22f);
                            npc.damage = (int)(npc.damage * 12f);
                        }
                        else
                        {
                            npc.lifeMax = (int)(npc.lifeMax * 6f);
                            npc.damage = (int)(npc.damage * 12f);
                        }
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.friendly = true;
                        npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.MinionAttackTargetNPC > 0 && !Slimes.Contains(Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].type))
                    {
                        Vector2 direction = Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity = direction * 20;
                        npc.noTileCollide = true;
                    }
                    else
                    {
                        npc.noTileCollide = false;
                    }
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC target = Main.npc[i];
                        Rectangle thisrect = npc.getRect();
                        Rectangle theirrect = target.getRect();
                        if (target.immune[npc.whoAmI] == 0 && thisrect.Intersects(theirrect) && target.whoAmI != npc.whoAmI && npc.active && target.active && !target.dontTakeDamage && !normalSlime)
                        {
                            if (bossSlime && amalgam)
                            {

                            }
                            else
                            {
                                target.StrikeNPC(npc.CalculateHitInfo(npc.damage, 0));
                                target.immune[npc.whoAmI] = 10;
                                if (target.damage > 0)
                                    npc.StrikeNPC(npc.CalculateHitInfo(target.damage, 0));
                            }
                        }

                    }
                }
                // If it's a boss and you have gemalgamation, attack enemies
                if (bossSlime && amalgam && !assortgel && !bossrush)
                {
                    if (!npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost)
                    {
                        npc.boss = false;
                        npc.friendly = true;
                        npc.lifeMax = (int)(npc.lifeMax * 22f);
                        npc.damage = (int)(npc.damage * 12f);
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.MinionAttackTargetNPC > 0 && !Slimes.Contains(Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].type) && !BossSlimes.Contains(Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].type))
                    {
                        Vector2 direction = Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity = direction * 20;
                        npc.noTileCollide = true;
                    }
                    else
                    {
                        npc.velocity.X *= 0.98f;
                        if (npc.velocity.Y < 10)
                            npc.velocity.Y += 1;
                        npc.noTileCollide = false;
                    }
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC target = Main.npc[i];
                        Rectangle thisrect = npc.getRect();
                        Rectangle theirrect = target.getRect();
                        if (target.immune[npc.whoAmI] == 0 && thisrect.Intersects(theirrect) && target.whoAmI != npc.whoAmI && npc.active && target.active && !target.dontTakeDamage && !Slimes.Contains(target.type) && !bossSlime)
                        {
                            target.StrikeNPC(target.CalculateHitInfo(npc.damage, 0));
                            target.immune[npc.whoAmI] = 10;
                            if (target.damage > 0)
                                npc.StrikeNPC(npc.CalculateHitInfo(target.damage, 0));
                        }

                    }
                    return false;
                }
                // if it's a boss and you have assortagelatin, do nothing and become passive
                else if (bossSlime && assortgel && !amalgam && !bossrush)
                {
                    npc.damage = 0;
                    // slime god is specifically excluded because hes stupid and i hate him
                    if (npc.type == NPCType<SlimeGodCore>())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            #region Quotes
            if (npc.type == NPCType<Crabulon>())
            {
                if (crabSay == 0)
                {
                    if (DateTime.Today.ToString("dd/MM").Equals("01/04") && Main.rand.NextBool(100))
                        Talk("Buy Delicious Meat! So Very Delicious! 20% Off! Buy Today!", Color.LightSkyBlue);
                    else
                        Talk("Hello, are you here to place a delivery for my world-famous Delicious Meat, made with Frosted Pigron and Blue Truffles (now 70% bluer)?", Color.LightSkyBlue);
                    crabSay = 1;
                }
                else if (crabSay == 2 && npc.life < (npc.lifeMax * 3 / 4))
                {
                    Talk("You must be kidding. You're just another one of those desperate Delicious Meat fans that don't care to pay up for our hard work that was put into making these. For shame.", Color.LightSkyBlue);
                    crabSay = 3;
                }
                else if (crabSay == 3 && npc.life < (npc.lifeMax / 3))
                {
                    Talk("You remind me of that giant mushroom pig flying fish thing. If it could, it would easily butcher you whole, while you're blinded by your depression or whatever.", Color.LightSkyBlue);
                    crabSay = 4;
                }
            }
            else if (npc.type == NPCType<SlimeGodCore>())
            {
                bool noPals = !NPC.AnyNPCs(NPCType<CrimulanPaladin>()) && !NPC.AnyNPCs(NPCType<EbonianPaladin>());
                bool none = noPals && !NPC.AnyNPCs(NPCType<SplitCrimulanPaladin>()) && !NPC.AnyNPCs(NPCType<SplitEbonianPaladin>());
                bool lastCrim = noPals && NPC.CountNPCS(NPCType<SplitCrimulanPaladin>()) < 2 && !NPC.AnyNPCs(NPCType<SplitEbonianPaladin>());
                bool lastEbon = noPals && NPC.CountNPCS(NPCType<SplitEbonianPaladin>()) < 2 && !NPC.AnyNPCs(NPCType<SplitCrimulanPaladin>());
                if (slimeSay == 0)
                {
                    Talk("Hello we have suspected you committing blasphemy against sloomes", Color.Olive);
                    slimeSay = 1;
                }
                else if (slimeSay == 1 && (lastCrim || lastEbon))
                {
                    Talk("Absurd! I can't allow you to butcher the last bean bag", Color.Olive);
                    slimeSay = 2;
                }
                else if (slimeSay == 2 && none)
                {
                    Talk("You will not be forgiven for your sins. I'll be back, and batter than ever...", Color.Olive);
                    slimeSay = 3;
                }
            }
            else if (npc.type == NPCType<ProfanedGuardianCommander>())
            {
                if (guardSay == 0)
                {
                    Talk("Guardian Commander: YOU WILL BURN BY THE WILL OF THE PROFLAMED FLAMES!", Color.Yellow);
                    Talk("Guardian Defender: Prepare to meet your end, fool.", Color.Gold);
                    Talk("Guardian Healer: Be careful... we're some tough guardians!", Color.LavenderBlush);
                    guardSay = 1;
                }
                if (npc.Calamity().CurrentlyEnraged && !guardOver && guardSay > 0)
                {
                    Talk("Guardian Commander: That is bad. We are Angry", Color.Yellow);
                    if (NPC.AnyNPCs(NPCType<ProfanedGuardianDefender>()))
                        Talk("Guardian Defender: That is bad. We are Angry", Color.Gold);
                    if (NPC.AnyNPCs(NPCType<ProfanedGuardianHealer>()))
                        Talk("Guardian Healer: That is bad. We are Angry", Color.LavenderBlush);
                    guardOver = true;
                }
                if (NPC.AnyNPCs(NPCType<DILF>()) && !guardRage && guardSay > 0)
                {
                    foreach (NPC frosty in Main.ActiveNPCs)
                    {
                        if (frosty.type == NPCType<DILF>() && npc.Distance(frosty.Center) < 2400)
                        {
                            Talk("Guardian Commander: BURN THE DELICIOUS MEAT! ALL OF IT!", Color.Yellow);
                            if (NPC.AnyNPCs(NPCType<ProfanedGuardianDefender>()))
                                Talk("Guardian Defender: You... you will not get away with the prize money this time.", Color.Gold);
                            if (NPC.AnyNPCs(NPCType<ProfanedGuardianHealer>()))
                                Talk("Guardian Healer: Guardians unite! We have a more worthy enemy to destroy.", Color.LavenderBlush);
                            guardRage = true;
                        }
                    }
                }
            }
            else if (npc.type == NPCType<Yharon>())
            {

                float hp = (float)npc.life / (float)npc.lifeMax;
                bool flag = Main.expertMode || BossRushEvent.BossRushActive;
                bool flag2 = CalamityWorld.revenge || BossRushEvent.BossRushActive;
                bool flag3 = CalamityWorld.death || BossRushEvent.BossRushActive;

                bool p2 = hp <= (flag2 ? 0.9f : (flag ? 0.85f : 0.75f));
                bool p3 = hp <= (flag3 ? 0.8f : (flag2 ? 0.75f : (flag ? 0.7f : 0.625f)));
                bool p5 = hp <= (flag2 ? 0.44f : (flag ? 0.385f : 0.275f));
                bool p6 = hp <= (flag3 ? 0.358f : (flag2 ? 0.275f : (flag ? 0.22f : 0.138f)));

                Yharon yhar = npc.ModNPC as Yharon;
                int y = (int)yhar.GetType().GetField("invincibilityCounter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(yhar);
                if (yharSay == 0)
                {
                    Talk("Hello! Are you here for a duel? I'll go easy on you since you're so small and weak.", Color.OrangeRed);
                    yharSay = 1;
                }
                else if (yharSay == 1 && p2)
                {
                    Talk("Wow, you are quite strong! I underestimated you!", Color.OrangeRed);
                    yharSay = 2;
                }
                else if (yharSay == 2 && p3)
                {
                    Talk("Well done! You can withstand my attacks while launching powerful attacks of your own!", Color.OrangeRed);
                    yharSay = 3;
                }
                else if (yharSay == 3 && hp <= 0.55f)
                {
                    Talk("No more messing around. Impressive, but it's not enough to stop me!", Color.OrangeRed);
                    yharSay = 4;
                }
                else if (yharSay == 4 && y >= 300)
                {
                    Talk("You're very tough, I hope I can win this...", Color.OrangeRed);
                    yharSay = 5;
                }
                else if (yharSay == 5 && p5)
                {
                    Talk("I won't hold back! You may be hard to beat, but I am harder!", Color.OrangeRed);
                    yharSay = 6;
                }
                else if (yharSay == 6 && p6)
                {
                    Talk("STOP! STOP! NO!", Color.OrangeRed);
                    yharSay = 7;
                }
            }
            else if (npc.type == NPCType<PrimordialWyrmHead>())
            {
                if (jaredSay == 0)
                {
                    Talk("You are foolish to think you can invade our lands, mortal.", Color.Aqua);
                    jaredSay = 1;
                }
                else if (jaredSay == 1 && (float)npc.life / (float)npc.lifeMax < 0.8f)
                {
                    Talk("Oh? So you are stronger than I thought... this will be fun. Have you come to take that little artifact we have guarded for all of eternity?", Color.Aqua);
                    jaredSay = 2;
                }
                else if (jaredSay == 2 && (float)npc.life / (float)npc.lifeMax < 0.6f)
                {
                    Talk("Soon, you will cease to exist. How naive, to think that you possess even a fraction of my power.", Color.Aqua);
                    jaredSay = 3;
                }
                else if (jaredSay == 3 && (float)npc.life / (float)npc.lifeMax < 0.4f)
                {
                    Talk("For millions of years, I have ruled this sea, undefeated. Your pointless existence will not change that.", Color.Aqua);
                    jaredSay = 4;
                }
                else if (jaredSay == 4 && (float)npc.life / (float)npc.lifeMax < 0.2f)
                {
                    Talk("Do you think you can POSSIBLY defeat me? I am Jared, the primordial being, the abyssal god, and you think you could ever stand a chance?!", Color.Aqua);
                    jaredSay = 5;
                }
                else if (jaredSay == 5 && (float)npc.life / (float)npc.lifeMax < 0.05f)
                {
                    Talk("Your actions are useless! You came here to slaughter us and take our treasures, but we will not let that happen. You will never truly defeat us. Even if you were to kill me, dozens more of the sea's young wyrms will take my place. Do you want this? Do you want this world to erupt into chaos?", Color.Aqua);
                    jaredSay = 6;
                }
                else if (jaredSay == 6 && (float)npc.life / (float)npc.lifeMax < 0.01f)
                {
                    Talk("This is just the beginning of the calamity. Your enemies are ascending beyond your control... or was that all your intention?", Color.Aqua);
                    jaredSay = 7;
                }
            }
            #endregion
            if (npc.type == NPCType<ProfanedGuardianCommander>() || npc.type == NPCType<ProfanedGuardianDefender>() || npc.type == NPCType<ProfanedGuardianHealer>())
            {
                if (NPC.AnyNPCs(NPCType<DILF>()) && guardRage)
                {
                    foreach (NPC frosty in Main.ActiveNPCs)
                    {
                        if (frosty.type == NPCType<DILF>() && npc.Distance(frosty.Center) < 2400)
                        {
                            npc.velocity = npc.DirectionTo(frosty.Center) * 14f;
                            if (frosty.Hitbox.Intersects(npc.Hitbox))
                            {
                                frosty.StrikeInstantKill();
                                break;
                            }
                            return false;
                        }
                    }
                }
            }
            if (npc.type == NPCID.DukeFishron)
            {
                if (!CalRemixWorld.canGenerateBaron)
                {
                    if (npc.position.X < 300 || npc.position.X > (Main.maxTilesX * 16) - 300)
                    {
                        bool left = npc.position.X < 300;
                        ThreadPool.QueueUserWorkItem(_ => BaronStrait.GenerateBaronStrait(left));
                        CalRemixWorld.canGenerateBaron = true;
                        CalRemixWorld.UpdateWorldBool();
                    }
                }
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                if (npc.life < (int)(npc.lifeMax * 0.5f) && !Main.hardMode && !BossRushEvent.BossRushActive && CalRemixWorld.mullet)
                {
                    npc.active = false;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, NPCType<Fleshmullet>());
                    }
                }
            }
            return true;
        }
        public override void AI(NPC npc)
        {
            CalRemixPlayer modPlayer = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (npc.type == NPCType<MicrobialCluster>() && npc.catchItem == 0)
            {
                npc.catchItem = ItemType<DisgustingSeawater>();
            }
            if (npc.type == NPCType<FAP>()) // MURDER the drunk princess
            {
                npc.active = false;
            }
            if (npc.type == NPCType<SupremeCalamitas>()) // MURDER the drunk princess
            {
                SupremeCalamitas cirrus = npc.ModNPC as SupremeCalamitas;
                if (cirrus.cirrus)
                {
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.ByeSAC", Color.Red);
                    npc.active = false;
                }
            }
            if (npc.type == NPCType<AureusSpawn>() && (modPlayer.nuclegel || modPlayer.assortegel) && !CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                npc.active = false;
            }
            if (npc.type == NPCType<WulfrumAmplifier>())
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC exc = Main.npc[i];
                    if (npc.Distance(exc.Center) <= npc.ai[1] && exc.ModNPC is WulfwyrmHead)
                    {
                        exc.ModNPC<WulfwyrmHead>().PylonCharged = true;
                    }
                }
            }
            if (CalRemixWorld.plaguetoggle)
            {
                if (npc.type == NPCType<PlaguebringerGoliath>() && !Main.player[npc.target].GetModPlayer<CalRemixPlayer>().ZonePlague)
                {
                    if (npc.ai[2] == 1)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 dist = npc.Center - Main.player[npc.target].Center;
                            dist.Normalize();
                            for (int i = 0; i < Main.rand.Next(5, 9); i++)
                            {
                                Vector2 velocity = dist * 16;
                                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)(3 - 1)));
                                perturbedSpeed += new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
                                int type = Main.rand.NextBool(3) ? ProjectileType<HiveBombGoliath>() : ProjectileType<PlagueStingerGoliathV2>();
                                int p = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, perturbedSpeed, type, (int)(npc.damage * 0.5f), 0f);
                            }
                        }
                    }
                    //npc.ai[1]++;
                }
            }
            if (!CalamityPlayer.areThereAnyDamnBosses)
            {
                if (!NPCID.Sets.BossBestiaryPriority.Contains(npc.type) || npc.Calamity().CanHaveBossHealthBar)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval((int)npc.Bottom.X / 16, (int)npc.Bottom.Y / 16);
                    if (t.TileType == TileType<GrimesandPlaced>())
                    {
                        npc.StrikeInstantKill();
                        for (int i = 0; i < 30; i++)
                        {
                            Dust d = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.CorruptionThorns, Scale: Main.rand.NextFloat(2f, 4f))];
                            d.noGravity = true;
                            Vector2 vel = d.position - npc.Center;
                            vel.SafeNormalize(Vector2.Zero);
                            d.velocity = vel * Main.rand.NextFloat(0.1f, 0.3f);
                        }
                        if (CalRemixWorld.fearmonger)
                        {
                            if (npc.type != NPCType<SuperDummyNPC>() && !npc.SpawnedFromStatue && npc.damage > 0 && !npc.friendly)
                            {
                                if (Main.rand.NextBool(10))
                                {
                                    Item.NewItem(npc.GetSource_Death(), npc.getRect(), ItemType<FearmongerGreathelm>());
                                }
                                if (Main.rand.NextBool(10))
                                {
                                    Item.NewItem(npc.GetSource_Death(), npc.getRect(), ItemType<FearmongerGreaves>());
                                }
                                if (Main.rand.NextBool(10))
                                {
                                    Item.NewItem(npc.GetSource_Death(), npc.getRect(), ItemType<FearmongerPlateMail>());
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (CalRemixWorld.starbuster)
            {
                if (npc.type == NPCID.Unicorn)
                {
                    if (NPC.AnyNPCs(NPCType<StellarCulex>()))
                    {
                        foreach (NPC n in Main.npc)
                        {
                            if (n.type == NPCType<StellarCulex>() && n.active)
                            {
                                if (n.getRect().Intersects(npc.getRect()))
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Item.NewItem(npc.GetSource_Death(), npc.Center, ItemType<StarbusterCore>());
                                    }
                                    int craterRadius = 4;
                                    for (int i = -craterRadius; i < craterRadius; i++)
                                    {
                                        for (int j = -craterRadius; j < craterRadius; j++)
                                        {
                                            int dist = ((int)(npc.Bottom.X / 16) - ((int)(npc.Bottom.X / 16) + i)) * ((int)(npc.Bottom.X / 16) - ((int)(npc.Bottom.X / 16) + i)) + ((int)(npc.Bottom.Y / 16) - ((int)(npc.Bottom.Y / 16) + j)) * ((int)(npc.Bottom.Y / 16) - ((int)(npc.Bottom.Y / 16) + j));
                                            if (dist > craterRadius * craterRadius)
                                                continue;

                                            Tile t = Main.tile[(int)(npc.Bottom.X / 16) + i, (int)(npc.Bottom.Y / 16) + j];
                                            if (TileID.Sets.Grass[t.TileType] || TileID.Sets.Stone[t.TileType] || t.TileType == TileID.SnowBlock || t.TileType == TileID.Dirt || TileID.Sets.Conversion.Sand[t.TileType] || TileID.Sets.Conversion.Sandstone[t.TileType] || TileID.Sets.Conversion.HardenedSand[t.TileType] || TileID.Sets.Conversion.Ice[t.TileType])
                                            {
                                                t.TileType = (ushort)TileType<AstralOre>();
                                                WorldGen.SquareTileFrame((int)(npc.Bottom.X / 16) + i, (int)(npc.Bottom.Y / 16) + j, true);
                                                NetMessage.SendTileSquare(-1, (int)(npc.Bottom.X / 16) + i, (int)(npc.Bottom.Y / 16) + j, 1);
                                            }
                                        }
                                    }
                                    n.StrikeInstantKill();
                                    npc.StrikeInstantKill();
                                }
                            }
                        }
                    }
                }
            }
            if (!CalamityMod.CalPlayer.CalamityPlayer.areThereAnyDamnBosses && !CalamityLists.enemyImmunityList.Contains(npc.type))
            {
                if (npc.GetGlobalNPC<CalamityMod.NPCs.CalamityGlobalNPC>().pearlAura > 0)
                    npc.AddBuff(BuffType<CalamityMod.Buffs.StatDebuffs.GlacialState>(), 60);
            }
            if (npc.GetGlobalNPC<CalRemixNPC>().clawed > 0)
            {
                npc.position.X = MathHelper.Lerp(npc.position.X, clawPosition.X - npc.width / 2, 0.1f);
                npc.position.Y = MathHelper.Lerp(npc.position.Y, clawPosition.Y - npc.height / 2, 0.1f);
                npc.velocity = Vector2.Zero;
                npc.position += new Vector2(Main.rand.NextFloat(-1f, 2f), Main.rand.NextFloat(-1f, 2f));
                npc.frameCounter += 2;
            }
            if (shreadedLungs > 0)
                shreadedLungs--;
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(new NPCShop.Entry(ItemType<LesserStealthPotion>()));
            }
            if (shop.NpcType == NPCType<THIEF>())
            {
                shop.Add(new NPCShop.Entry(ItemType<StealthPotion>()));
            }
            if (shop.NpcType == NPCID.Steampunker)
            {
                shop.Add(new NPCShop.Entry(ItemType<PlaguedSolution>()));
                shop.Add(new NPCShop.Entry(ItemID.CellPhone));
            }
            if (shop.NpcType == NPCType<DILF>())
            {
                shop.Add(new NPCShop.Entry(ItemType<ColdheartIcicle>()));
                shop.Add(new NPCShop.Entry(ItemType<TheGenerator>(), new Condition("Conditions.DownedGens", () => RemixDowned.DownedGens)));
            }
            if (shop.NpcType == NPCID.ArmsDealer)
            {
                shop.Add(new NPCShop.Entry(ItemType<ElectricEel>()));
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            EnemyLoot(npc, npcLoot);
            MiniBossLoot(npc, npcLoot);
            BossLoot(npc, npcLoot);
        }
        private static void EnemyLoot(NPC npc, NPCLoot npcLoot)
        {
            #region Pre-Hardmode
            if (NPCID.Sets.DemonEyes[npc.type])
            {
                npcLoot.AddIf(() => Main.LocalPlayer.armor[0].type == ItemID.WoodHelmet && Main.LocalPlayer.armor[1].type == ItemID.WoodBreastplate && Main.LocalPlayer.armor[2].type == ItemID.WoodGreaves, ItemType<Ogscule>());
            }
            if (npc.type == NPCID.Vulture)
            {
                LeadingConditionRule mainRule = new(new Conditions.IsHardmode());
                mainRule.Add(ItemType<DesertFeather>(), 1, 3, 5);
                npcLoot.Add(mainRule);
            }
            if (npc.type == NPCID.LarvaeAntlion)
            {
                npcLoot.Add(ItemType<AntlionOre>(), 6, 7, 13);
            }
            if (npc.type == NPCType<Stormlion>() || npc.type == NPCID.Antlion || npc.type == NPCID.WalkingAntlion || npc.type == NPCID.GiantWalkingAntlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.GiantFlyingAntlion)
            {
                npcLoot.Add(ItemType<AntlionOre>(), 1, 13, 23);
            }
            if (npc.type == NPCType<Cnidrion>())
            {
                npcLoot.AddIf(() => RemixDowned.downedExcavator, ItemType<DesertMedallion>());
            }
            if (npc.type == NPCID.GoblinThief)
            {
                npcLoot.AddNormalOnly(ItemType<Warglaive>(), 40, 25, 68);
                npcLoot.AddIf(() => Main.expertMode, ItemType<Warglaive>(), 20, 37, 120);
            }
            if (npc.type == NPCID.GoblinPeon)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<Warbell>(), 40, 20));
            }
            if (npc.type == NPCID.GoblinSorcerer)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<Warstaff>(), 20, 10));
            }
            if (npc.type == NPCID.GoblinArcher)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<Warbow>(), 40, 20));
                npcLoot.AddNormalOnly(ItemType<WarArrow>(), 40, 25, 68);
                npcLoot.AddIf(() => Main.expertMode, ItemType<WarArrow>(), 20, 37, 120);
            }
            if (npc.type == NPCID.GoblinWarrior)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<WaraxeReloaded>(), 20, 10));
            }
            if (npc.type == NPCType<HiveTumor>())
            {
                npcLoot.AddIf(() => CalRemixWorld.grimesandToggle, ItemID.DemoniteOre, 1, 10, 26, ui: !CalRemixWorld.grimesandToggle);
            }
            if (npc.type == NPCType<PerforatorCyst>())
            {
                npcLoot.AddIf(() => CalRemixWorld.grimesandToggle, ItemID.CrimtaneOre, 1, 10, 26, ui: !CalRemixWorld.grimesandToggle);
            }
            if (npc.type == NPCType<MirageJelly>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<MirageJellyItem>(), 7, 5));
            }
            if (npc.type == NPCID.GraniteFlyer)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<CosmicStone>(), 20, 10));
            }
            if (npc.type == NPCID.BloodJelly)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<IrateJelly>(), 7, 5));
            }
            if (npc.type == NPCType<BoxJellyfish>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<ElasticJelly>(), 7, 5));
            }
            if (npc.type == NPCType<CannonballJellyfish>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<InvigoratingJelly>(), 7, 5));
            }
            #endregion
            #region Hardmode
            if (npc.type == NPCID.ManEater || CalamityLists.hornetList.Contains(npc.type) || npc.type == NPCID.SpikedJungleSlime || npc.type == NPCID.JungleSlime)
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ItemType<EssenceofBabil>(), 4, hideLootReport: !Main.hardMode);
                npcLoot.Add(hm);
            }
            if (npc.type == NPCID.AngryTrapper || CalamityLists.mossHornetList.Contains(npc.type) || npc.type == NPCID.Derpling)
            {
                npcLoot.Add(ItemType<EssenceofBabil>(), 3);
            }
            if (npc.type == NPCID.Wolf)
            {
                LeadingConditionRule postPolter = new LeadingConditionRule(new Conditions.IsExpert());
                postPolter.Add(ItemType<CoyoteVenom>(), 3, hideLootReport: !Main.expertMode);
                postPolter.AddFail(ItemType<CoyoteVenom>(), 4, hideLootReport: Main.expertMode);
                npcLoot.Add(postPolter);
            }
            if (npc.DeathSound == CommonCalamitySounds.AstralNPCDeathSound || npc.type == NPCType<AstralSlime>())
            {
                npcLoot.Add(ItemType<TitanFinger>(), 50);
            }
            if (npc.type == NPCType<Nova>())
            {
                LeadingConditionRule leadingConditionRule = npcLoot.DefineConditionalDropSet((DropAttemptInfo info) => CalRemixWorld.permanenthealth && info.npc.ai[3] <= -10000f && CheckAstralOreBlocks(info.npc));
                leadingConditionRule.Add(ItemType<CometShard>());
            }
            if (npc.type == NPCType<Atlas>())
            {
                npcLoot.Add(ItemType<TitanFinger>(), 6);
            }
            if (npc.type == NPCType<EutrophicRay>() || npc.type == NPCType<PrismBack>() || npc.type == NPCType<BlindedAngler>() || npc.type == NPCType<GhostBell>() || npc.type == NPCType<GhostBell>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ItemType<ClamChowder>(), 50);
            }
            if (npc.type == NPCType<Clam>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ItemType<ClamChowder>(), 10);
                npcLoot.Add(hm);
            }
            if (npc.type == NPCType<SeaSerpent1>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ItemType<ClamChowder>(), 20);
                npcLoot.Add(hm);
            }
            if (npc.type == NPCType<StellarCulex>())
            {
                npcLoot.RemoveWhere((rule) => rule is ItemDropWithConditionRule rouxls && rouxls.itemId == ItemType<StarbusterCore>());
                npcLoot.AddIf(() => !CalRemixWorld.starbuster, ItemType<StarbusterCore>(), 7, ui: !CalRemixWorld.starbuster);
            }
            switch (npc.type)
            {
                case NPCID.SolarSpearman: // Drakanian
                case NPCID.SolarSolenian: // Selenian
                case NPCID.SolarCorite:
                case NPCID.SolarSroller:
                case NPCID.SolarDrakomireRider:
                case NPCID.SolarDrakomire:
                case NPCID.SolarCrawltipedeHead:
                case NPCID.VortexSoldier:     // Vortexian
                case NPCID.VortexLarva:       // Alien Larva
                case NPCID.VortexHornet:      // Alien Hornet
                case NPCID.VortexHornetQueen: // Alien Queen
                case NPCID.VortexRifleman:    // Storm Diver
                case NPCID.NebulaBrain:    // Nebula Floater
                case NPCID.NebulaSoldier:  // Predictor
                case NPCID.NebulaHeadcrab: // Brain Suckler
                case NPCID.NebulaBeast:    // Evolution Beast
                case NPCID.StardustSoldier:      // Stargazer
                case NPCID.StardustSpiderBig:    // Twinkle Popper
                case NPCID.StardustJellyfishBig: // Flow Invader
                case NPCID.StardustCellBig:      // Star Cell
                case NPCID.StardustWormHead:     // Milkyway Weaver
                    npcLoot.Add(ItemType<MeldChipIceCream>(), 33);
                    break;
            }
            if (npc.type == NPCType<SightseerSpitter>())
            {
                npcLoot.Add(ItemType<AstralPearl>(), new Fraction(1, 15));
            }
            if (npc.type == NPCType<SightseerCollider>())
            {
                npcLoot.Add(ItemType<AstralPearl>(), 20);
            }
            if (npc.type == NPCID.DD2OgreT2)
            {
                npcLoot.Add(ItemType<BoringStormbow>(), 10);
            }
            if (npc.type == NPCID.DD2OgreT3)
            {
                npcLoot.Add(ItemType<BoringStormbow>(), 5);
            }
            #endregion
            #region Godseeker Mode
            if (npc.type == NPCID.Clinger)
            {
                LeadingConditionRule postPolter = new LeadingConditionRule(new Conditions.IsExpert());
                postPolter.Add(ItemType<CursedSpear>(), new Fraction(2, 30), hideLootReport: !Main.expertMode);
                postPolter.AddFail(ItemType<CursedSpear>(), 25, hideLootReport: Main.expertMode);
                npcLoot.Add(postPolter);
            }
            if (npc.type == NPCID.IchorSticker)
            {
                LeadingConditionRule postPolter = new LeadingConditionRule(new Conditions.IsExpert());
                postPolter.Add(ItemType<IchorDagger>(), new Fraction(2, 30), hideLootReport: !Main.expertMode);
                postPolter.AddFail(ItemType<IchorDagger>(), 25, hideLootReport: Main.expertMode);
                npcLoot.Add(postPolter);
            }
            if (npc.type == NPCType<Bohldohr>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedCalamitas && DownedBossSystem.downedExoMechs, ItemType<NO>(), 10, 1, 1, ui: false);
            }
            if (npc.type == NPCType<Sulflounder>())
            {
                LeadingConditionRule flound = new LeadingConditionRule(new Conditions.IsExpert());
                flound.Add(ItemType<FlounderMortar>(), 10, hideLootReport: !Main.expertMode);
                flound.AddFail(ItemType<FlounderMortar>(), new Fraction(2, 30), hideLootReport: Main.expertMode);
                npcLoot.Add(flound);
            }
            if (npc.type == NPCType<FlakCrab>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<CadaverousCarrion>(), 20);
            }
            if (npc.type == NPCType<Orthocera>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<InsidiousImpaler>(), 20);
            }
            if (npc.type == NPCType<AcidEel>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<VitriolicViper>(), 20);
            }
            if (npc.type == NPCType<Trilobite>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<ToxicantTwister>(), 20);
            }
            if (npc.type == NPCID.PigronCorruption || npc.type == NPCID.PigronCrimson || npc.type == NPCID.PigronHallow)
            {
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<MutatedTruffle>(), 20);
            }
            if (npc.type == NPCType<CragmawMire>())
            {
                LeadingConditionRule postPolter = npcLoot.DefineConditionalDropSet(() => DownedBossSystem.downedPolterghast);
                postPolter.Add(ItemType<NucleateGello>(), 10);
                postPolter.AddFail(ItemType<NucleateGello>());
                npcLoot.Add(postPolter);
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<SepticSkewer>(), 4);
            }
            if (npc.type == NPCID.BigMimicCorruption || npc.type == NPCID.BigMimicCrimson || npc.type == NPCID.BigMimicHallow)
            {
                npcLoot.Add(ItemType<GreaterStealthPotion>(), 1, 5, 10);
                npcLoot.Add(ItemType<GreaterFlightPotion>(), 1, 5, 10);
                npcLoot.AddIf(() => CalamityWorld.revenge || CalamityWorld.death, ItemType<GreaterAdrenalinePotion>(), 1, 5, 10);
                npcLoot.AddIf(() => CalamityWorld.revenge || CalamityWorld.death, ItemType<GreaterEnragePotion>(), 1, 5, 10);
            }
            #endregion
        }
        private static void MiniBossLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCType<GiantClam>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ItemType<ClamChowder>(), 2);
                npcLoot.Add(hm);
            }
            if (npc.type == NPCType<Horse>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<RockStone>(), 5, 3));
            }
            if (npc.type == NPCType<GreatSandShark>())
            {
                LeadingConditionRule toothRule = new LeadingConditionRule(new Conditions.IsExpert());
                toothRule.Add(ItemType<SandSharkToothNecklace>(), 1, hideLootReport: !Main.expertMode);
                toothRule.AddFail(ItemType<SandSharkToothNecklace>(), 2, hideLootReport: Main.expertMode);
                npcLoot.Add(toothRule);

                LeadingConditionRule mainRule = new LeadingConditionRule(new Conditions.IsExpert());
                LeadingConditionRule normal = npcLoot.DefineNormalOnlyDropSet();
                int[] itemIDs = new int[6]
                {
                    ItemType<Tumbleweed>(),
                    ItemType<SandstormGun>(),
                    ItemType<ShiftingSands>(),
                    ItemType<SandSharknadoStaff>(),
                    ItemType<Sandslasher>(),
                    ItemType<DuststormInABottle>()
                };
                mainRule.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, itemIDs), hideLootReport: !Main.expertMode);
                mainRule.AddFail(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, itemIDs), hideLootReport: Main.expertMode);
                npcLoot.Add(mainRule);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemType<GrandScale>());
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.LightShard);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.DarkShard);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.DungeonDesertKey);
            }
            if (npc.type == NPCType<Mauler>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<FetidEmesis>(), 4);
            }
            if (npc.type == NPCType<NuclearTerror>())
            {
                npcLoot.Add(ItemType<Microxodonta>(), 3);
                npcLoot.AddIf(() => DownedBossSystem.downedPolterghast, ItemType<OldDukeScales>(), 4);
            }
        }
        private static void BossLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCType<DesertScourgeHead>())
            {
                npcLoot.AddNormalOnly(ItemType<ParchedScale>(), 1, 25, 30);
            }
            if (npc.type == NPCType<Crabulon>())
            {
                npcLoot.AddNormalOnly(ItemType<DeliciousMeat>(), 1, 4, 7);
                npcLoot.AddNormalOnly(ItemType<CrabLeaves>(), 1, 4, 7);
                npcLoot.AddNormalOnly(ItemType<OddMushroom>(), 5);
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.CorruptionKey);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.CrimsonKey);
            }
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.HallowedKey);
            }
            if (npc.type == NPCID.Plantera)
            {
                npcLoot.AddNormalOnly(ItemType<EssenceofBabil>(), 1, 4, 8);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.JungleKey);
            }
            if (npc.type == NPCType<Leviathan>() || npc.type == NPCType<Anahita>())
            {
                LeadingConditionRule mainRule = npcLoot.DefineConditionalDropSet(Leviathan.LastAnLStanding);
                mainRule.Add(ItemType<CrocodileScale>(), 1, 20, 30);
                npcLoot.AddNormalOnly(mainRule);
            }
            if (npc.type == NPCType<AstrumAureus>())
            {
                npcLoot.AddNormalOnly(ItemType<SoulofBright>(), 1, 4, 6);
            }
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.AddNormalOnly(ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            if (npc.type == NPCType<Providence>())
            {
                npcLoot.AddNormalOnly(ItemType<ProfanedNucleus>(), 4);
                npcLoot.AddNormalOnly(ItemType<TorrefiedTephra>(), 1, 200, 222);
            }
            if (npc.type == NPCType<DevourerofGodsHead>())
            {
                npcLoot.AddNormalOnly(ItemType<Lean>(), 1, 3, 4);
            }
            if (npc.type == NPCType<Yharon>())
            {
                npcLoot.AddNormalOnly(ItemType<MovieSign>(), 100);
            }
            if (npc.type == NPCType<SupremeCalamitas>())
            {
                npcLoot.AddNormalOnly(ItemType<YharimBar>(), 1, 6, 8);
            }
            if (npc.type == NPCType<PrimordialWyrmHead>())
            {
                npcLoot.Add(ItemType<SubnauticalPlate>(), 1, 22, 34);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemType<HalibutCannon>());
            }
            if (CalRemixAddon.Wrath != null)
            {
                if (CalRemixAddon.Wrath.TryFind("EntropicGod", out ModNPC noxus) && npc.type == noxus.Type)
                {
                    LeadingConditionRule bar = new LeadingConditionRule(new Conditions.IsExpert());
                    bar.Add(ItemType<EntropicBar>(), 10);
                    bar.AddFail(ItemType<EntropicBar>(), 20);
                    npcLoot.Add(bar);

                    LeadingConditionRule frond = new LeadingConditionRule(new Conditions.IsExpert());
                    frond.Add(ItemType<EntropicFrond>(), 1, 25, 35);
                    frond.AddFail(ItemType<EntropicFrond>(), 1, 35, 45);
                    npcLoot.Add(frond);
                }
            }
            if (npc.type == NPCID.Deerclops)
            {
                npcLoot.AddNormalOnly(ItemType<DeerdalusStormclops>(), 20);
            }

        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            GeneralHitStuff(npc, hit, damageDone, player);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            Player p = Main.player[projectile.owner];
            GeneralHitStuff(npc, hit, damageDone, p);
        }
        public static void GeneralHitStuff(NPC npc, NPC.HitInfo hit, int damageDone, Player p = null)
        {
            if (p != null)
            {
                if (p.Calamity().titanHeartSet)
                {
                    if (npc.type == NPCID.WanderingEye)
                    {
                        if (NPC.CountNPCS(NPCType<Ogslime>()) <= 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            NPC.NewNPC(npc.GetSource_OnHit(npc), (int)npc.position.X, (int)npc.position.Y, NPCType<Ogslime>());
                            if (!CalRemixWorld.ogslime)
                            {
                                CalRemixWorld.ogslime = true;
                                CalRemixWorld.UpdateWorldBool();
                            }
                        }
                    }
                }
                if (npc.type == NPCID.Wizard && npc.life <= 0 && CalRemixWorld.ionQuestLevel == IonCubeTE.dialogue.Count - 2)
                {
                    CalRemixWorld.wizardDisabled = true;
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.ByeWizard", Color.DarkBlue);
                    CalRemixWorld.UpdateWorldBool();
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().tvo)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<LumChunk>(), 0, 0, Main.myPlayer);
                }
            }
            else if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().cart)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<CalHeart>(), 0, 0, Main.myPlayer);
                }
            }
            if (npc.type == NPCType<Horse>())
                RemixDowned.downedEarthElemental = true;
            if (!Main.dedServ)
            {
                if (!CystMessage.alreadySeen && !CalRemixWorld.grimesandToggle)
                {
                    if (npc.type == NPCType<PerforatorCyst>() || npc.type == NPCType<HiveTumor>())
                    {
                        CystMessage.ActivateMessage();
                    }
                }
            }
            if (npc.type == ModContent.NPCType<CalamitasClone>())
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<EDGY>()))
                {
                    NPC.NewNPC(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Center.Y, NPCType<EDGY>());
                }
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                if (!NPC.AnyNPCs(NPCID.Clothier))
                {
                    NPC.NewNPC(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.Clothier);
                }
            }
            if (npc.type == ModContent.NPCType<Piggy>())
            {
                NPC.NewNPC(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.DukeFishron);
            }
        }
        public override bool CheckDead(NPC npc)
        {
            if (npc.lifeMax > 1000 && npc.value > 0f && npc.HasPlayerTarget && NPC.downedMoonlord && Main.player[npc.target].ZoneDungeon && npc.type != NPCType<GildedSpirit>() && npc.type != NPCType<GlisteningSpirit>())
            {
                if (Main.rand.NextBool(10))
                {
                    int index = NPC.NewNPC(NPC.GetSource_None(), (int)npc.Center.X, (int)npc.Center.Y, NPCType<GlisteningSpirit>());
                    if (Main.netMode == NetmodeID.MultiplayerClient && index < Main.maxNPCs)
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
                else
                {
                    int index = NPC.NewNPC(NPC.GetSource_None(), (int)npc.Center.X, (int)npc.Center.Y, NPCType<GildedSpirit>());
                    if (Main.netMode == NetmodeID.MultiplayerClient && index < Main.maxNPCs)
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
            }

            return true;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (npc.type == NPCType<Crabulon>())
                binaryWriter.Write(crabSay);
            else if (npc.type == NPCType<SlimeGodCore>())
                binaryWriter.Write(slimeSay);
            else if (npc.type == NPCType<ProfanedGuardianCommander>())
                binaryWriter.Write(guardSay);
            else if (npc.type == NPCType<Yharon>())
                binaryWriter.Write(yharSay);
            else if (npc.type == NPCType<PrimordialWyrmHead>())
                binaryWriter.Write(jaredSay);

            if (npc.type == NPCType<ProfanedGuardianCommander>())
            {
                binaryWriter.Write(guardRage);
                binaryWriter.Write(guardOver);
            }
            else if (npc.type == NPCType<Yharon>())
                binaryWriter.Write(yharRage);
            if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
                binaryWriter.Write(npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == NPCType<Crabulon>())
                crabSay = binaryReader.ReadInt32();
            else if (npc.type == NPCType<SlimeGodCore>())
                slimeSay = binaryReader.ReadInt32();
            else if (npc.type == NPCType<ProfanedGuardianCommander>())
                guardSay = binaryReader.ReadInt32();
            else if (npc.type == NPCType<Yharon>())
                yharSay = binaryReader.ReadInt32();
            else if (npc.type == NPCType<PrimordialWyrmHead>())
                jaredSay = binaryReader.ReadInt32();

            if (npc.type == NPCType<ProfanedGuardianCommander>())
            {
                guardRage = binaryReader.ReadBoolean();
                guardOver = binaryReader.ReadBoolean();
            }
            else if (npc.type == NPCType<Yharon>())
                yharRage = binaryReader.ReadBoolean();
            if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
                if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
                    npc.GetGlobalNPC<CalRemixNPC>().SlimeBoost = binaryReader.ReadBoolean();
        }
        public override bool PreKill(NPC npc)
        {
            if (CalRemixWorld.lifeoretoggle)
            { 
                if (!DownedBossSystem.downedRavager && npc.type == NPCType<RavagerBody>())
                {
                    CalamityUtils.SpawnOre(TileType<LifeOreTile>(), 0.25E-05, 0.45f, 0.65f, 30, 40);

                    Color messageColor = Color.Lime;
                    CalamityUtils.DisplayLocalizedText("Vitality sprawls throughout the underground.", messageColor);
                }
            }
            if (CalRemixWorld.shrinetoggle)
            {
                if (npc.type == NPCID.WallofFlesh && !Main.hardMode)
                {
                    if (!ScreenHelperManager.screenHelpersEnabled)
                    {
                        // he's defrosting!
                        ScreenHelperManager.screenHelpersEnabled = true;
                        ScreenHelperManager.fannyTimesFrozen++;
                        Anomaly109UI.fannyFreezeTime = 0;
                    }
                    CalRemixWorld.ShrineTimer = 3000;
                }
            }
            if (!CalRemixWorld.deusDeadInSnow)
            {
                if (Main.LocalPlayer.ZoneSnow && npc.type == NPCType<AstrumDeusHead>())
                {
                    CalRemixWorld.deusDeadInSnow = true;
                    CalRemixWorld.UpdateWorldBool();
                }
            }
            return true;
        }
        public override void ResetEffects(NPC npc)
        {
            vBurn = false;
            if (!npc.HasBuff<Wither>())
                witherDebuff = false;
            if (!witherDebuff)
                wither = 0;

            if (npc.Hitbox.Intersects(ScreenHelperManager.screenRect))
                ScreenHelperManager.sceneMetrics.onscreenNPCs.Add(npc);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (witherDebuff)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                if (wither > 0)
                {
                    npc.lifeRegen -= 2 * (120 + (int)Math.Log(wither) * 80);
                    if (damage < 120)
                        damage = 120 + (int)Math.Log(wither) * 80;
                }
                else
                {
                    npc.lifeRegen -= 240;
                    if (damage < 120)
                        damage = 120;
                }
            }
            if (shreadedLungs > 0)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 12;
                if (damage < 12)
                {
                    damage = 12;
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            #region CrabTalk
            if (npc.type == NPCType<Crabulon>() && crabSay <= 1)
            {
                Talk("No? Please do be careful with that weapon, though, it looks kinda dangerous. Honestly, you seem quite... crabby. Get it?!", Color.LightSkyBlue);
                crabSay = 2;
            }
            else if (npc.type == NPCType<Crabulon>() && npc.life <= 0 && crabSay == 4)
            {
                Talk("AAAAAAAAAh", Color.LightSkyBlue);
                crabSay = 5;
            }
            else if (npc.life <= 0 && npc.type == NPCType<ProfanedGuardianCommander>())
            {
                Talk("Guardian Commander: MY MENTAL FORTITUDE IS FADING...", Color.Yellow);
            }
            else if (npc.life <= 0 && npc.type == NPCType<ProfanedGuardianDefender>())
            {
                Talk("Guardian Defender: Nothing... can beat my eldest sibling...", Color.Gold);
                if (NPC.AnyNPCs(NPCType<ProfanedGuardianCommander>()))
                    Talk("Guardian Commander: VERY SOON, YOU WILL FEEL MY PROFANED RAGE... HA-HA-HA...", Color.Yellow);
            }
            else if (npc.life <= 0 && npc.type == NPCType<ProfanedGuardianHealer>())
            {
                Talk("Guardian Healer: Ouch!", Color.LavenderBlush);
                if (NPC.AnyNPCs(NPCType<ProfanedGuardianDefender>()))
                    Talk("Guardian Defender: How? How could you!?", Color.Gold);
                if (NPC.AnyNPCs(NPCType<ProfanedGuardianCommander>()))
                    Talk("Guardian Commander: ENOUGH! YOU MAY HAVE DEFEATED ONE OF US, BUT US TWO ARE MUCH TOUGHER!", Color.Yellow);
            }
            else if (npc.life <= 0 && npc.type == NPCType<Yharon>())
            {
                Talk("I can't believe it, you are even stronger than me. Nice job!", Color.OrangeRed);
            }
            #endregion
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (vBurn)
            {
                modifiers.SourceDamage *= 1.05f;
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<CalRemixPlayer>().dungeon2)
            {
                spawnRate = NPC.downedMoonlord ? (int)(spawnRate * 0.2f) : NPC.downedPlantBoss ? (int)(spawnRate * 0.4f) : (int)(spawnRate * 0.6f);
                maxSpawns *= NPC.downedMoonlord ? 16 : NPC.downedBoss3 ? 12 : 8;
            }
            if (CalRemixWorld.oxydayTime > 0)
            {
                spawnRate = (int)(spawnRate * 0.2f);
                maxSpawns *= 8;
            }
            if (player.InModBiome<FrozenStrongholdBiome>())
            {
                spawnRate = (int)(spawnRate * 0.3f);
                maxSpawns *= 12;
            }
            if (CalRemixWorld.roachDuration > 0)
            {
                spawnRate = 3;
                maxSpawns *= 15;
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (CalRemixWorld.roachDuration > 0)
            {
                pool.Clear();
                pool.Add(NPCType<LabRoach>(), 22f);
                return;
            }
            if (ProfanedDesert.scorchedWorld)
            {
                pool.Clear();
                pool.Add(NPCType<ScornEater>(), 1);
                if (!NPC.AnyNPCs(NPCType<ProfanedEnergyBody>()))
                    pool.Add(NPCType<ProfanedEnergyBody>(), 1);
                pool.Add(NPCType<ImpiousImmolator>(), 1);
                pool.Add(NPCType<YggdrasilEnt>(), 0.05f);
                if (CalRemixAddon.CalVal != null)
                {
                    pool.Add(CalRemixAddon.CalVal.Find<ModNPC>("ProvFly").Type, 1);
                    pool.Add(CalRemixAddon.CalVal.Find<ModNPC>("CrystalFly").Type, 1);
                }

                return;
            }
            //Wizard can't respawn
            if (CalRemixWorld.wizardDisabled)
            {
                if (pool.ContainsKey(NPCID.BoundWizard))
                {
                    pool.Remove(NPCID.BoundWizard);
                }
            }
            if (CalRemixWorld.oxydayTime > 0)
            {
                pool.Add(NPCID.Dandelion, 100);
            }
            if (spawnInfo.Player.InModBiome<FrozenStrongholdBiome>())
            {
                foreach (var n in pool)
                {
                    if (n.Key != ModContent.NPCType<FrozenMummy>() && n.Key != ModContent.NPCType<EnchantedSkull>() && n.Key != ModContent.NPCType<EnchantedTome>())
                    {
                        pool.Remove(n);
                    }
                }
            }
            if (spawnInfo.Player.GetModPlayer<CalRemixPlayer>().dungeon2)
            {
                pool.Clear();
                //if (NPC.downedBoss3)
                {
                    if (!NPC.savedMech)
                    {
                        pool.Add(NPCID.BoundMechanic, 0.1f);
                    }
                    pool.Add(NPCID.AngryBones, 1);
                    pool.Add(NPCID.AngryBonesBig, 1);
                    pool.Add(NPCID.AngryBonesBigHelmet, 1);
                    pool.Add(NPCID.AngryBonesBigMuscle, 1);
                    pool.Add(NPCID.DarkCaster, 0.5f);
                    pool.Add(NPCID.CursedSkull, 0.5f);
                    pool.Add(NPCID.DungeonSlime, 0.05f);
                    pool.Add(NPCID.SpikeBall, 0.05f);
                    pool.Add(NPCID.BlazingWheel, 0.05f);
                    if (Main.hardMode)
                    {
                        pool.Add(NPCType<RenegadeWarlock>(), 0.05f);
                    }
                    if (NPC.downedPlantBoss)
                    {
                        pool.Add(NPCID.BlueArmoredBones, 1);
                        pool.Add(NPCID.BlueArmoredBonesMace, 1);
                        pool.Add(NPCID.BlueArmoredBonesNoPants, 1);
                        pool.Add(NPCID.BlueArmoredBonesSword, 1);
                        pool.Add(NPCID.HellArmoredBones, 1);
                        pool.Add(NPCID.HellArmoredBonesMace, 1);
                        pool.Add(NPCID.HellArmoredBonesSpikeShield, 1);
                        pool.Add(NPCID.HellArmoredBonesSword, 1);
                        pool.Add(NPCID.RustyArmoredBonesAxe, 1);
                        pool.Add(NPCID.RustyArmoredBonesFlail, 1);
                        pool.Add(NPCID.RustyArmoredBonesSword, 1);
                        pool.Add(NPCID.RustyArmoredBonesSwordNoArmor, 1);
                        pool.Add(NPCID.Necromancer, 0.2f);
                        pool.Add(NPCID.DiabolistRed, 0.1f);
                        pool.Add(NPCID.DiabolistWhite, 0.1f);
                        pool.Add(NPCID.RaggedCaster, 0.2f);
                        pool.Add(NPCID.Paladin, 0.05f);
                        pool.Add(NPCID.TacticalSkeleton, 0.2f);
                        pool.Add(NPCID.SkeletonSniper, 0.2f);
                        pool.Add(NPCID.SkeletonCommando, 0.2f);
                        pool.Add(NPCID.GiantCursedSkull, 0.2f);
                        pool.Add(NPCID.BoneLee, 0.2f);
                    }
                    if (spawnInfo.Water && DownedBossSystem.downedPolterghast)
                    {
                        pool.Add(NPCType<MinnowsPrime>(), 1f);
                    }
                }
            }
        }
        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (Main.player[Main.myPlayer].HasBuff(BuffType<Acid>()))
                return drawColor.MultiplyRGB(Color.YellowGreen);
            return null;
        }

        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int desertID = GetInstance<ProfanedDesertBiome>().Type;

            AddModBiomeToBestiary(npc.type, NPCType<ScornEater>(), desertID, bestiaryEntry);
            AddModBiomeToBestiary(npc.type, NPCType<ImpiousImmolator>(), desertID, bestiaryEntry);
            AddModBiomeToBestiary(npc.type, NPCType<ProfanedEnergyBody>(), desertID, bestiaryEntry);
            AddModBiomeToBestiary(npc.type, NPCType<ProfanedGuardianCommander>(), desertID, bestiaryEntry);
            AddModBiomeToBestiary(npc.type, NPCType<ProfanedGuardianDefender>(), desertID, bestiaryEntry);
            AddModBiomeToBestiary(npc.type, NPCType<ProfanedGuardianHealer>(), desertID, bestiaryEntry);
            AddModBiomeToBestiary(npc.type, NPCType<Providence>(), desertID, bestiaryEntry);

            AddModBiomeToBestiary(npc.type, NPCID.Dandelion, GetInstance<GaleforceDayBiome>(), bestiaryEntry);

            ConvertPlagueEnemy(npc.type, NPCType<PlaguebringerGoliath>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<PlagueCharger>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<PlagueChargerLarge>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<PlaguebringerMiniboss>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<Viruling>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<Plagueshell>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<PestilentSlime>(), bestiaryEntry);
            ConvertPlagueEnemy(npc.type, NPCType<Melter>(), bestiaryEntry);
        }

        private static void Talk(string text, Color color)
        {
            if (!CalRemixWorld.bossdialogue)
                return;
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            else if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, color);
        }
        public static void WormAI(NPC npc, float speed, float acceleration, Entity target, Vector2 prowlPoint, int segmentType = 0, bool canFlyByDefault = false, bool makeBurrowSound = false, bool despawnOnSurface = false, float despawnSpeed = 0.2f)
        {
            {
                if (npc.ai[3] > 0f)
                {
                    npc.realLife = (int)npc.ai[3];
                }
                if (target == null || !target.active || (despawnOnSurface && (double)target.position.Y < Main.worldSurface * 16.0))
                {
                    npc.EncourageDespawn(300);
                    if (despawnOnSurface)
                    {
                        npc.velocity.Y += despawnSpeed;
                    }
                }
                {

                    if (segmentType != 0)
                    {
                        if (!Main.npc[(int)npc.ai[1]].active)
                            {
                                npc.life = 0;
                                npc.HitEffect();
                                npc.checkDead();
                                npc.active = false;
                                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
                                return;
                            }
                    }
                    if (segmentType == 0)
                    { 
                        if (!Main.npc[(int)npc.ai[0]].active)
                        {
                            npc.life = 0;
                            npc.HitEffect();
                            npc.checkDead();
                            npc.active = false;
                            NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
                            return;
                        }
                    }
                    if (!npc.active && Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
                    }
                }
                int tileLeft = (int)(npc.position.X / 16f) - 1;
                int tileRight = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
                int tileAbove = (int)(npc.position.Y / 16f) - 1;
                int tileBelow = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
                if (tileLeft < 0)
                {
                    tileLeft = 0;
                }
                if (tileRight > Main.maxTilesX)
                {
                    tileRight = Main.maxTilesX;
                }
                if (tileAbove < 0)
                {
                    tileAbove = 0;
                }
                if (tileBelow > Main.maxTilesY)
                {
                    tileBelow = Main.maxTilesY;
                }
                bool canFly = canFlyByDefault;
                if (!canFly)
                {
                    Vector2 vector2 = default(Vector2);
                    for (int num43 = tileLeft; num43 < tileRight; num43++)
                    {
                        for (int num44 = tileAbove; num44 < tileBelow; num44++)
                        {
                            if (Main.tile[num43, num44] == null || ((Main.tile[num43, num44].HasTile || (!Main.tileSolid[Main.tile[num43, num44].TileType] && (!Main.tileSolidTop[Main.tile[num43, num44].TileType] || Main.tile[num43, num44].TileFrameY != 0))) && Main.tile[num43, num44].LiquidAmount <= 64))
                            {
                                continue;
                            }
                            vector2.X = num43 * 16;
                            vector2.Y = num44 * 16;
                            if (npc.position.X + (float)npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + (float)npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                            {
                                canFly = true;
                                if (Main.rand.NextBool(100) && npc.type != NPCID.LeechHead && !Main.tile[num43, num44].HasTile && Main.tileSolid[Main.tile[num43, num44].TileType])
                                {
                                    WorldGen.KillTile(num43, num44, fail: true, effectOnly: true);
                                }
                            }
                        }
                    }
                }
                if (!canFly && (!canFlyByDefault))
                {
                    Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                    int num46 = 1000;
                    bool flag3 = true;
                    Rectangle rectangle2 = new Rectangle((int)target.position.X - num46, (int)target.position.Y - num46, num46 * 2, num46 * 2);
                    if (rectangle.Intersects(rectangle2))
                    {
                        flag3 = false;
                    }
                    if (flag3)
                    {
                        canFly = true;
                    }
                }
                if (npc.type == NPCType<Basilius>())
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.spriteDirection = 1;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.spriteDirection = -1;
                    }
                }
                Vector2 vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num50 = prowlPoint.X;
                float num51 = prowlPoint.Y;
                if (target != null)
                {
                    num50 = target.position.X + (float)(target.width / 2);
                    num51 = target.position.Y + (float)(target.height / 2);
                }
                bool passive = false;
                if (npc.type == NPCType<Basilius>() && !(target != null && target.active && !(target is NPC ne && ne.life <= 0)))
                {
                    passive = true;
                }
                if (passive && prowlPoint != Vector2.Zero)
                {
                    speed = 10f;
                    acceleration = 0.3f;
                    int num52 = -1;
                    int num53 = (int)(prowlPoint.X / 16f);
                    int num54 = (int)(prowlPoint.Y / 16f);
                    for (int num56 = num53 - 2; num56 <= num53 + 2; num56++)
                    {
                        for (int num57 = num54; num57 <= num54 + 15; num57++)
                        {
                            if (WorldGen.SolidTile2(num56, num57))
                            {
                                num52 = num57;
                                break;
                            }
                        }
                        if (num52 > 0)
                        {
                            break;
                        }
                    }
                    if (num52 > 0)
                    {
                        num52 *= 16;
                        float num58 = num52 - 800;
                        if (prowlPoint.Y > num58)
                        {
                            num51 = num58;
                            if (Math.Abs(npc.Center.X - prowlPoint.X) < 500f)
                            {
                                num50 = ((!(npc.velocity.X > 0f)) ? (prowlPoint.X - 600f) : (prowlPoint.X + 600f));
                            }
                        }
                    }
                    else
                    {
                        speed = 14f;
                        acceleration = 0.5f;
                    }
                    float num59 = speed * 1.3f;
                    float num60 = speed * 0.7f;
                    float num61 = npc.velocity.Length();
                    if (num61 > 0f)
                    {
                        if (num61 > num59)
                        {
                            npc.velocity.Normalize();
                            npc.velocity *= num59;
                        }
                        else if (num61 < num60)
                        {
                            npc.velocity.Normalize();
                            npc.velocity *= num60;
                        }
                    }
                    if (num52 > 0)
                    {
                        for (int num62 = 0; num62 < 200; num62++)
                        {
                            if (Main.npc[num62].active && Main.npc[num62].type == npc.type && num62 != npc.whoAmI)
                            {
                                Vector2 vector6 = Main.npc[num62].Center - npc.Center;
                                if (vector6.Length() < 400f)
                                {
                                    vector6.Normalize();
                                    vector6 *= 1000f;
                                    num50 -= vector6.X;
                                    num51 -= vector6.Y;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int num63 = 0; num63 < 200; num63++)
                        {
                            if (Main.npc[num63].active && Main.npc[num63].type == npc.type && num63 != npc.whoAmI)
                            {
                                Vector2 vector7 = Main.npc[num63].Center - npc.Center;
                                if (vector7.Length() < 60f)
                                {
                                    vector7.Normalize();
                                    vector7 *= 200f;
                                    num50 -= vector7.X;
                                    num51 -= vector7.Y;
                                }
                            }
                        }
                    }
                }
                num50 = (int)(num50 / 16f) * 16;
                num51 = (int)(num51 / 16f) * 16;
                vector5.X = (int)(vector5.X / 16f) * 16;
                vector5.Y = (int)(vector5.Y / 16f) * 16;
                num50 -= vector5.X;
                num51 -= vector5.Y;
                float num64 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
                if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length)
                {
                    try
                    {
                        vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        num50 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector5.X;
                        num51 = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector5.Y;
                    }
                    catch
                    {
                    }
                    npc.rotation = (float)Math.Atan2(num51, num50) + MathHelper.Pi;
                    num64 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
                    int num65 = npc.width;
                    num64 = (num64 - (float)num65) / num64;
                    num50 *= num64;
                    num51 *= num64;
                    npc.velocity = Vector2.Zero;
                    npc.position.X += num50;
                    npc.position.Y += num51;
                    if (npc.type == NPCType<Basilius>())
                    {
                        if (num50 < 0f)
                        {
                            npc.spriteDirection = 1;
                        }
                        else if (num50 > 0f)
                        {
                            npc.spriteDirection = -1;
                        }
                    }
                }
                else
                {
                    if (!canFly)
                    {
                        npc.TargetClosest();
                        npc.velocity.Y += 0.11f;
                        if (npc.velocity.Y > speed)
                        {
                            npc.velocity.Y = speed;
                        }
                        if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.4)
                        {
                            if (npc.velocity.X < 0f)
                            {
                                npc.velocity.X -= acceleration * 1.1f;
                            }
                            else
                            {
                                npc.velocity.X += acceleration * 1.1f;
                            }
                        }
                        else if (npc.velocity.Y == speed)
                        {
                            if (npc.velocity.X < num50)
                            {
                                npc.velocity.X += acceleration;
                            }
                            else if (npc.velocity.X > num50)
                            {
                                npc.velocity.X -= acceleration;
                            }
                        }
                        else if (npc.velocity.Y > 4f)
                        {
                            if (npc.velocity.X < 0f)
                            {
                                npc.velocity.X += acceleration * 0.9f;
                            }
                            else
                            {
                                npc.velocity.X -= acceleration * 0.9f;
                            }
                        }
                    }
                    else
                    {
                        if (makeBurrowSound && npc.soundDelay == 0)
                        {
                            float num67 = num64 / 40f;
                            if (num67 < 10f)
                            {
                                num67 = 10f;
                            }
                            if (num67 > 20f)
                            {
                                num67 = 20f;
                            }
                            npc.soundDelay = (int)num67;
                            SoundEngine.PlaySound(SoundID.WormDig, npc.position);
                        }
                        num64 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
                        float num68 = Math.Abs(num50);
                        float num69 = Math.Abs(num51);
                        float num70 = speed / num64;
                        num50 *= num70;
                        num51 *= num70;
                        bool flag6 = false;
                        if (canFly)
                        {
                            if (((npc.velocity.X > 0f && num50 < 0f) || (npc.velocity.X < 0f && num50 > 0f) || (npc.velocity.Y > 0f && num51 < 0f) || (npc.velocity.Y < 0f && num51 > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > acceleration / 2f && num64 < 300f)
                            {
                                flag6 = true;
                                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed)
                                {
                                    npc.velocity *= 1.1f;
                                }
                            }
                            Vector2 pos = target != null ? target.position : prowlPoint;
                            if (false)
                            {
                                flag6 = true;
                                if (Math.Abs(npc.velocity.X) < speed / 2f)
                                {
                                    if (npc.velocity.X == 0f)
                                    {
                                        npc.velocity.X -= npc.direction;
                                    }
                                    npc.velocity.X *= 1.1f;
                                }
                                else if (npc.velocity.Y > 0f - speed)
                                {
                                    npc.velocity.Y -= acceleration;
                                }
                            }
                        }
                        if (!flag6)
                        {
                            if ((npc.velocity.X > 0f && num50 > 0f) || (npc.velocity.X < 0f && num50 < 0f) || (npc.velocity.Y > 0f && num51 > 0f) || (npc.velocity.Y < 0f && num51 < 0f))
                            {
                                if (npc.velocity.X < num50)
                                {
                                    npc.velocity.X += acceleration;
                                }
                                else if (npc.velocity.X > num50)
                                {
                                    npc.velocity.X -= acceleration;
                                }
                                if (npc.velocity.Y < num51)
                                {
                                    npc.velocity.Y += acceleration;
                                }
                                else if (npc.velocity.Y > num51)
                                {
                                    npc.velocity.Y -= acceleration;
                                }
                                if ((double)Math.Abs(num51) < (double)speed * 0.2 && ((npc.velocity.X > 0f && num50 < 0f) || (npc.velocity.X < 0f && num50 > 0f)))
                                {
                                    if (npc.velocity.Y > 0f)
                                    {
                                        npc.velocity.Y += acceleration * 2f;
                                    }
                                    else
                                    {
                                        npc.velocity.Y -= acceleration * 2f;
                                    }
                                }
                                if ((double)Math.Abs(num50) < (double)speed * 0.2 && ((npc.velocity.Y > 0f && num51 < 0f) || (npc.velocity.Y < 0f && num51 > 0f)))
                                {
                                    if (npc.velocity.X > 0f)
                                    {
                                        npc.velocity.X += acceleration * 2f;
                                    }
                                    else
                                    {
                                        npc.velocity.X -= acceleration * 2f;
                                    }
                                }
                            }
                            else if (num68 > num69)
                            {
                                if (npc.velocity.X < num50)
                                {
                                    npc.velocity.X += acceleration * 1.1f;
                                }
                                else if (npc.velocity.X > num50)
                                {
                                    npc.velocity.X -= acceleration * 1.1f;
                                }
                                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.5)
                                {
                                    if (npc.velocity.Y > 0f)
                                    {
                                        npc.velocity.Y += acceleration;
                                    }
                                    else
                                    {
                                        npc.velocity.Y -= acceleration;
                                    }
                                }
                            }
                            else
                            {
                                if (npc.velocity.Y < num51)
                                {
                                    npc.velocity.Y += acceleration * 1.1f;
                                }
                                else if (npc.velocity.Y > num51)
                                {
                                    npc.velocity.Y -= acceleration * 1.1f;
                                }
                                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.5)
                                {
                                    if (npc.velocity.X > 0f)
                                    {
                                        npc.velocity.X += acceleration;
                                    }
                                    else
                                    {
                                        npc.velocity.X -= acceleration;
                                    }
                                }
                            }
                        }
                    }
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + MathHelper.Pi;
                    if (!canFlyByDefault)
                    {
                        if (canFly)
                        {
                            if (npc.localAI[0] != 1f)
                            {
                                npc.netUpdate = true;
                            }
                            npc.localAI[0] = 1f;
                        }
                        else
                        {
                            if (npc.localAI[0] != 0f)
                            {
                                npc.netUpdate = true;
                            }
                            npc.localAI[0] = 0f;
                        }
                        if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                        {
                            npc.netUpdate = true;
                        }
                    }
                }
                if (npc.alpha > 0 && npc.life > 0)
                {
                    for (int num77 = 0; num77 < 2; num77++)
                    {
                        int num78 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Demonite, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num78].noGravity = true;
                        Main.dust[num78].noLight = true;
                    }
                }
                if ((npc.position - npc.oldPosition).Length() > 2f)
                {
                    npc.alpha -= 42;
                    if (npc.alpha < 0)
                    {
                        npc.alpha = 0;
                    }
                }
            }
        }
        public static bool CheckAstralOreBlocks(NPC npc)
        {
            for (int i = -5; i < npc.width + 5; i++)
            {
                for (int j = -5; j < npc.height + 5; j++)
                {
                    int x = (npc.position + Vector2.UnitX * i).ToSafeTileCoordinates().X;
                    int y = (npc.position + Vector2.UnitY * i).ToSafeTileCoordinates().Y;
                    if (Main.tile[x, y].TileType == TileType<AstralOre>())
                        return true;
                }
            }
            return false;
        }
    }
}
