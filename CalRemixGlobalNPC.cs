using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Items.Materials;
using CalRemix.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.SulphurousSea;
using CalRemix.NPCs;
using CalRemix.NPCs.Minibosses;
using CalRemix.NPCs.Bosses.Wulfwyrm;
using CalRemix.Items;
using CalRemix.Items.Accessories;
using CalamityMod.Items.Materials;
using System.Collections.Generic;
using CalRemix.Projectiles.Accessories;
using CalRemix.Projectiles.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader.IO;
using CalamityMod.NPCs.Providence;
using CalamityMod.Events;
using System;
using System.IO;
using Terraria.Chat;
using Terraria.Localization;
using CalamityMod.NPCs.Crabulon;
using CalRemix.Items.Weapons;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Yharon;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using System.Reflection;
using CalamityMod.Sounds;
using CalRemix.Items.Potions;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.Items.Accessories;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Tiles.Ores;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using Terraria.UI;
using CalamityMod.NPCs.Leviathan;
using CalRemix.Retheme;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.NPCs.Other;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Armor.Fearmonger;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using CalamityMod.NPCs.Perforator;
using log4net.Repository.Hierarchy;
using MonoMod.Utils;
using Terraria.DataStructures;
using CalRemix.UI;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.GreatSandShark;
using CalamityMod.NPCs.AstrumDeus;

namespace CalRemix
{
    public class CalRemixGlobalNPC : GlobalNPC
    {
        bool SlimeBoost = false;
        public bool vBurn = false;
        public bool grappled = false;
        public int clawed = 0;
        private int crabSay, slimeSay, guardSay, yharSay, jaredSay = 0;
        public Vector2 clawPosition = Vector2.Zero;
        public float shadowHit = 1;
        public static int wulfyrm = -1;
        public static int aspidCount = 0;
        private bool guardRage, guardOver, yharRage = false;
        public float[] storedAI = { 0f, 0f, 0f, 0f };
        public float[] storedCalAI = { 0f, 0f, 0f, 0f };
        public float[] storedLocalAI = { 0f, 0f, 0f, 0f };
        public static FannyMessage CystMessage;
        public override bool InstancePerEntity => true;

        public List<int> BossSlimes = new List<int>
        {
            NPCID.KingSlime,
            NPCID.QueenSlimeBoss,
            ModContent.NPCType<AstrumAureus>(),
            ModContent.NPCType<EbonianPaladin>(),
            ModContent.NPCType<CrimulanPaladin>(),
            ModContent.NPCType<SplitEbonianPaladin>(),
            ModContent.NPCType<SplitCrimulanPaladin>(),
            ModContent.NPCType<SlimeGodCore>(),
            ModContent.NPCType<LifeSlime>(),
            ModContent.NPCType<CragmawMire>()
        };

        public List<int> Slimes = new List<int>
        {
            1, 16, 59, 71, 81, 138, 121, 122, 141, 147, 183, 184, 204, 225, 244, 302, 333, 335, 334, 336, 537,
            NPCID.SlimeSpiked,
            NPCID.QueenSlimeMinionBlue,
            NPCID.QueenSlimeMinionPink,
            NPCID.QueenSlimeMinionPurple,
            ModContent.NPCType<AeroSlime>(),
            ModContent.NPCType<CalamityMod.NPCs.Astral.AstralSlime>(),
            ModContent.NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>(),
            ModContent.NPCType<BloomSlime>(),
            ModContent.NPCType<CalamityMod.NPCs.Crags.InfernalCongealment>(),
            ModContent.NPCType<PerennialSlime>(),
            ModContent.NPCType<CryoSlime>(),
            ModContent.NPCType<GammaSlime>(),
            ModContent.NPCType<IrradiatedSlime>(),
            ModContent.NPCType<AuricSlime>(),
            ModContent.NPCType<CorruptSlimeSpawn>(),
            ModContent.NPCType<CorruptSlimeSpawn2>(),
            ModContent.NPCType<CrimsonSlimeSpawn>(),
            ModContent.NPCType<CrimsonSlimeSpawn2>()
        };

        public override void Load()
        {
            On_NPC.NewNPC += KillHiveMind;
        }

        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                CystMessage = new FannyMessage("CystDeath", "See!", "",
                FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation();
                FannyManager.LoadFannyMessage(CystMessage);
            }
        }
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npc.damage = 80;
                npc.lifeMax = 58500;
                npc.defense = 20;
                npc.value = Item.buyPrice(gold: 10);
            }
            else if (npc.type == ModContent.NPCType<Bumblefuck2>())
            {
                npc.damage = 60;
                npc.lifeMax = 3375;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<Claw>()) <= 0)
            {
                clawed = 0;
            }
            clawed--;
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();

            bool assortgel = player.assortegel;
            bool amalgam = player.amalgel;
            bool godfather = player.godfather;
            bool tvo = player.tvo;

            bool bossrush = CalamityMod.Events.BossRushEvent.BossRushActive;
            bool normalSlime = Slimes.Contains(npc.type);
            bool bossSlime = BossSlimes.Contains(npc.type);

            // Kill passive slimes if none of the accessories are on
            if (npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost && !assortgel && !amalgam && !godfather)
            {
                npc.active = false;
                return false;
            }
            // Godfather causes slimes to try to assimilate into goozma
            if (godfather && !bossrush)
            {
                if (normalSlime || bossSlime)
                {
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
                    {
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.friendly = true;
                        npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<CriticalSlimeCore>()] == 1)
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile target = Main.projectile[i];
                            if (target.type == ModContent.ProjectileType<CriticalSlimeCore>())
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
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
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
                        npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = true;
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
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
                    {
                        npc.boss = false;
                        npc.friendly = true;
                        npc.lifeMax = (int)(npc.lifeMax * 22f);
                        npc.damage = (int)(npc.damage * 12f);
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = true;
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
                    if (npc.type == ModContent.NPCType<SlimeGodCore>())
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
            if (npc.type == ModContent.NPCType<Crabulon>())
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
            else if (npc.type == ModContent.NPCType<SlimeGodCore>())
            {
                bool noPals = !NPC.AnyNPCs(ModContent.NPCType<CrimulanPaladin>()) && !NPC.AnyNPCs(ModContent.NPCType<EbonianPaladin>());
                bool none = noPals && !NPC.AnyNPCs(ModContent.NPCType<SplitCrimulanPaladin>()) && !NPC.AnyNPCs(ModContent.NPCType<SplitEbonianPaladin>());
                bool lastCrim = noPals && NPC.CountNPCS(ModContent.NPCType<SplitCrimulanPaladin>()) < 2 && !NPC.AnyNPCs(ModContent.NPCType<SplitEbonianPaladin>());
                bool lastEbon = noPals && NPC.CountNPCS(ModContent.NPCType<SplitEbonianPaladin>()) < 2 && !NPC.AnyNPCs(ModContent.NPCType<SplitCrimulanPaladin>());
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
                    Talk("You will not be forgiven for your sins. I'm now going to change my gender soon...", Color.Olive);
                    slimeSay = 3;
                }
            }
            else if (npc.type == ModContent.NPCType<ProfanedGuardianCommander>())
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
                    if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianDefender>()))
                        Talk("Guardian Defender: That is bad. We are Angry", Color.Gold);
                    if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianHealer>()))
                        Talk("Guardian Healer: That is bad. We are Angry", Color.LavenderBlush);
                    guardOver = true;
                }
                if (NPC.AnyNPCs(ModContent.NPCType<DILF>()) && !guardRage && guardSay > 0)
                {
                    Talk("Guardian Commander: BURN THE DELICIOUS MEAT! ALL OF IT!", Color.Yellow);
                    if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianDefender>()))
                        Talk("Guardian Defender: You... you will not get away with the prize money this time.", Color.Gold);
                    if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianHealer>()))
                        Talk("Guardian Healer: Guardians unite! We have a more worthy enemy to destroy.", Color.LavenderBlush);
                    guardRage = true;
                }
            }
            else if (npc.type == ModContent.NPCType<Yharon>())
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
            else if (npc.type == ModContent.NPCType<PrimordialWyrmHead>())
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
            if (npc.type == ModContent.NPCType<ProfanedGuardianCommander>() || npc.type == ModContent.NPCType<ProfanedGuardianDefender>() || npc.type == ModContent.NPCType<ProfanedGuardianHealer>())
            {
                if (NPC.AnyNPCs(ModContent.NPCType<DILF>()) && guardRage)
                {
                    foreach (NPC frosty in Main.npc)
                    {
                        if (frosty.type == ModContent.NPCType<DILF>())
                        {
                            npc.velocity = npc.DirectionTo(frosty.Center) * 10f;
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public override void AI(NPC npc)
        {
            CalRemixPlayer modPlayer = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (npc.type == ModContent.NPCType<MicrobialCluster>() && npc.catchItem == 0)
            {
                npc.catchItem = ModContent.ItemType<DisgustingSeawater>();
            }
            if (npc.type == ModContent.NPCType<FAP>()) // MURDER the drunk princess
            {
                npc.active = false;
            }
            if (npc.type == ModContent.NPCType<AureusSpawn>() && (modPlayer.nuclegel || modPlayer.assortegel) && !CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                npc.active = false;
            }
            if (npc.type == ModContent.NPCType<WulfrumAmplifier>())
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
                if (npc.type == ModContent.NPCType<PlaguebringerGoliath>() && !Main.player[npc.target].GetModPlayer<CalRemixPlayer>().ZonePlague)
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
                                int type = Main.rand.NextBool(3) ? ModContent.ProjectileType<HiveBombGoliath>() : ModContent.ProjectileType<PlagueStingerGoliathV2>();
                                int p = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, perturbedSpeed, type, (int)(npc.damage * 0.5f), 0f);
                            }
                        }
                    }
                    //npc.ai[1]++;
                }
            }
            if (CalRemixWorld.starbuster)
            {
                if (npc.type == NPCID.Unicorn)
                {
                    if (NPC.AnyNPCs(ModContent.NPCType<StellarCulex>()))
                    {
                        foreach (NPC n in Main.npc)
                        {
                            if (n.type == ModContent.NPCType<StellarCulex>() && n.active)
                            {
                                if (n.getRect().Intersects(npc.getRect()))
                                {
                                    if (Main.netMode != NetmodeID.Server)
                                    {
                                        Item.NewItem(npc.GetSource_Death(), npc.Center, ModContent.ItemType<StarbusterCore>());
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
                                                t.TileType = (ushort)ModContent.TileType<AstralOre>();
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
            if (!CalamityPlayer.areThereAnyDamnBosses)
            {
                if (!NPCID.Sets.BossBestiaryPriority.Contains(npc.type) || npc.Calamity().CanHaveBossHealthBar)
                {
                    if (Main.tile[(int)npc.Bottom.X / 16, (int)npc.Bottom.Y / 16].TileType == ModContent.TileType<GrimesandPlaced>())
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
                            if (npc.type != ModContent.NPCType<SuperDummyNPC>() && !npc.SpawnedFromStatue && npc.damage > 0 && !npc.friendly)
                            {
                                if (Main.rand.NextBool(10))
                                {
                                    Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<FearmongerGreathelm>());
                                }
                                if (Main.rand.NextBool(10))
                                {
                                    Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<FearmongerGreaves>());
                                }
                                if (Main.rand.NextBool(10))
                                {
                                    Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<FearmongerPlateMail>());
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (!CalamityMod.CalPlayer.CalamityPlayer.areThereAnyDamnBosses && !CalamityLists.enemyImmunityList.Contains(npc.type))
            {
                if (npc.GetGlobalNPC<CalamityMod.NPCs.CalamityGlobalNPC>().pearlAura > 0)
                    npc.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatDebuffs.GlacialState>(), 60);
            }
            if (npc.GetGlobalNPC<CalRemixGlobalNPC>().clawed > 0)
            {
                npc.position.X = MathHelper.Lerp(npc.position.X, clawPosition.X - npc.width / 2, 0.1f);
                npc.position.Y = MathHelper.Lerp(npc.position.Y, clawPosition.Y - npc.height / 2, 0.1f);
                npc.velocity = Vector2.Zero;
                npc.position += new Vector2(Main.rand.NextFloat(-1f, 2f), Main.rand.NextFloat(-1f, 2f));
                npc.frameCounter += 2;
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Steampunker)
            {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<PlaguedSolution>()));
            }
            if (shop.NpcType == ModContent.NPCType<DILF>())
            {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<ColdheartIcicle>()));
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss)
            {
                npcLoot.AddIf(() => Main.BestiaryTracker.Kills.GetKillCount(npc) >= 25, ModContent.ItemType<ConquestFragment>(), 5);
            }
            if (!npc.SpawnedFromStatue && npc.value > 0)
            {
                npcLoot.AddIf(()=> Main.BestiaryTracker.Kills.GetKillCount(npc) >= 25, ModContent.ItemType<ConquestFragment>(), 10);
            }
            if (npc.type == ModContent.NPCType<GreatSandShark>())
            {
                LeadingConditionRule toothRule = new LeadingConditionRule(new Conditions.IsExpert());
                toothRule.Add(ModContent.ItemType<SandSharkToothNecklace>(), 4, hideLootReport: !Main.expertMode);
                toothRule.AddFail(ModContent.ItemType<SandSharkToothNecklace>(), 3, hideLootReport: Main.expertMode);

                LeadingConditionRule mainRule = new LeadingConditionRule(new Conditions.IsExpert());
                LeadingConditionRule normal = npcLoot.DefineNormalOnlyDropSet();
                int[] itemIDs = new int[6]
                {
                    ModContent.ItemType<Tumbleweed>(),
                    ModContent.ItemType<SandstormGun>(),
                    ModContent.ItemType<ShiftingSands>(),
                    ModContent.ItemType<SandSharknadoStaff>(),
                    ModContent.ItemType<Sandslasher>(),
                    ModContent.ItemType<DuststormInABottle>()
                };
                mainRule.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, itemIDs), hideLootReport: !Main.expertMode);
                mainRule.AddFail(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, itemIDs), hideLootReport: Main.expertMode);
                npcLoot.Add(mainRule);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ModContent.ItemType<GrandScale>());
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.LightShard);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ItemID.DarkShard);
            }
            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                LeadingConditionRule mainRule = new LeadingConditionRule(new Conditions.NotExpert());
                mainRule.Add(ModContent.ItemType<ParchedScale>(), 1, 25, 30);
                npcLoot.Add(mainRule);
            }
            if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                LeadingConditionRule mainRule = new LeadingConditionRule(new Conditions.NotExpert());
                mainRule.Add(ModContent.ItemType<DesertFeather>(), 1, 25, 30);
                npcLoot.Add(mainRule);
            }
            if (npc.type == ModContent.NPCType<PrimordialWyrmHead>())
            {
                npcLoot.Add(ModContent.ItemType<SubnauticalPlate>(), 1, 22, 34);
                npcLoot.RemoveWhere((rule) => rule is CommonDrop e && e.itemId == ModContent.ItemType<HalibutCannon>());
            }
            if (npc.type == ModContent.NPCType<MirageJelly>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<MirageJellyItem>(), 7, 5));
            }
            if (npc.type == ModContent.NPCType<CragmawMire>())
            {
                LeadingConditionRule postPolter = npcLoot.DefineConditionalDropSet(() => DownedBossSystem.downedPolterghast);
                postPolter.Add(ModContent.ItemType<NucleateGello>(), 10, hideLootReport: !DownedBossSystem.downedPolterghast);
                postPolter.AddFail(ModContent.ItemType<NucleateGello>(), hideLootReport: DownedBossSystem.downedPolterghast);
            }
            if (npc.type == ModContent.NPCType<NuclearTerror>())
            {
                npcLoot.Add(ModContent.ItemType<Microxodonta>(), 3);
            }
            if (npc.type == NPCID.GraniteFlyer)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CosmicStone>(), 20, 10));
            }
            if (npc.type == ModContent.NPCType<Horse>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<RockStone>(), 5, 3));
            }
            if (npc.type == ModContent.NPCType<Providence>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ModContent.ItemType<ProfanedNucleus>(), 4);
                npcLoot.Add(hm);
            }
            if (npc.type == ModContent.NPCType<Cnidrion>())
            {
                npcLoot.AddIf(() => CalRemixWorld.downedExcavator, ModContent.ItemType<DesertMedallion>());
            }
            if (npc.type == NPCID.ManEater || CalamityLists.hornetList.Contains(npc.type) || npc.type == NPCID.SpikedJungleSlime || npc.type == NPCID.JungleSlime)
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ModContent.ItemType<EssenceofBabil>(), 4, hideLootReport: !Main.hardMode);
                npcLoot.Add(hm);
            }
            if (npc.type == NPCID.AngryTrapper || CalamityLists.mossHornetList.Contains(npc.type) || npc.type == NPCID.Derpling)
            {
                npcLoot.Add(ModContent.ItemType<EssenceofBabil>(), 3);
            }
            if (npc.type == NPCID.Plantera)
            {
                LeadingConditionRule exp = new LeadingConditionRule(new Conditions.NotExpert());
                exp.Add(ModContent.ItemType<EssenceofBabil>(), 1, 4, 8, hideLootReport: Main.expertMode);
                npcLoot.Add(exp);
            }
            if (npc.type == NPCID.Wolf)
            {
                LeadingConditionRule postPolter = new LeadingConditionRule(new Conditions.IsExpert());
                postPolter.Add(ModContent.ItemType<CoyoteVenom>(), 3, hideLootReport: !Main.expertMode);
                postPolter.AddFail(ModContent.ItemType<CoyoteVenom>(), 4, hideLootReport: Main.expertMode);
                npcLoot.Add(postPolter);
            }
            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                npcLoot.Add(ModContent.ItemType<YharimBar>(), 1, 6, 8);
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                LeadingConditionRule mainRule = new LeadingConditionRule(new Conditions.NotExpert());
                mainRule.Add(ModContent.ItemType<DeliciousMeat>(), 1, 4, 7);
                mainRule.Add(ModContent.ItemType<CrabLeaves>(), 1, 4, 7);
                npcLoot.Add(mainRule);
            }
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ModContent.ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            if (npc.type == ModContent.NPCType<Leviathan>() || npc.type == ModContent.NPCType<Anahita>())
            {
                LeadingConditionRule mainRule = npcLoot.DefineConditionalDropSet(Leviathan.LastAnLStanding);
                LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
                mainRule.OnSuccess(leadingConditionRule.Add(ModContent.ItemType<CrocodileScale>(), 1, 20, 30));
                npcLoot.Add(mainRule);
            }
            if (NPCID.Sets.DemonEyes[npc.type])
            {
                npcLoot.AddIf(() => Main.LocalPlayer.armor[0].type == ItemID.WoodHelmet && Main.LocalPlayer.armor[1].type == ItemID.WoodBreastplate && Main.LocalPlayer.armor[2].type == ItemID.WoodGreaves, ModContent.ItemType<Ogscule>());
            }
            if (npc.DeathSound == CommonCalamitySounds.AstralNPCDeathSound || npc.type == ModContent.NPCType<AstralSlime>())
            {
                npcLoot.Add(ModContent.ItemType<TitanFinger>(), 50);
            }
            if (npc.type == ModContent.NPCType<Atlas>())
            {
                npcLoot.Add(ModContent.ItemType<TitanFinger>(), 6);
            }
            if (npc.type == ModContent.NPCType<EutrophicRay>() || npc.type == ModContent.NPCType<PrismBack>() || npc.type == ModContent.NPCType<BlindedAngler>() || npc.type == ModContent.NPCType<GhostBell>() || npc.type == ModContent.NPCType<GhostBell>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ModContent.ItemType<ClamChowder>(), 50);
            }
            if (npc.type == ModContent.NPCType<Clam>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ModContent.ItemType<ClamChowder>(), 10);
                npcLoot.Add(hm);
            }
            if (npc.type == ModContent.NPCType<SeaSerpent1>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ModContent.ItemType<ClamChowder>(), 20);
                npcLoot.Add(hm);
            }
            if (npc.type == ModContent.NPCType<GiantClam>())
            {
                LeadingConditionRule hm = new LeadingConditionRule(new Conditions.IsHardmode());
                hm.Add(ModContent.ItemType<ClamChowder>(), 2);
                npcLoot.Add(hm);
            }
            if (npc.type == ModContent.NPCType<StellarCulex>())
            {
                npcLoot.RemoveWhere((rule) => rule is ItemDropWithConditionRule rouxls && rouxls.itemId == ModContent.ItemType<StarbusterCore>());
                npcLoot.AddIf(() => !CalRemixWorld.starbuster, ModContent.ItemType<StarbusterCore>(), 7, ui: !CalRemixWorld.starbuster);
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
                    npcLoot.Add(ModContent.ItemType<MeldChipIceCream>(), 33);
                    break;
            }
            if (npc.type == ModContent.NPCType<SightseerSpitter>())
            {
                npcLoot.Add(ModContent.ItemType<AstralPearl>(), new Fraction(1, 15));
            }
            if (npc.type == ModContent.NPCType<SightseerCollider>())
            {
                npcLoot.Add(ModContent.ItemType<AstralPearl>(), 20);
            }
            if (npc.type == NPCID.Clinger)
            {
                LeadingConditionRule postPolter = new LeadingConditionRule(new Conditions.IsExpert());
                postPolter.Add(ModContent.ItemType<CursedSpear>(), new Fraction(2, 30) , hideLootReport: !Main.expertMode);
                postPolter.AddFail(ModContent.ItemType<CursedSpear>(), 25, hideLootReport: Main.expertMode);
                npcLoot.Add(postPolter);
            }
            if (npc.type == NPCID.IchorSticker)
            {
                LeadingConditionRule postPolter = new LeadingConditionRule(new Conditions.IsExpert());
                postPolter.Add(ModContent.ItemType<IchorDagger>(), new Fraction(2, 30), hideLootReport: !Main.expertMode);
                postPolter.AddFail(ModContent.ItemType<IchorDagger>(), 25, hideLootReport: Main.expertMode);
                npcLoot.Add(postPolter);
            }
            if (npc.type == ModContent.NPCType<Bohldohr>())
            {
                npcLoot.AddIf(() => DownedBossSystem.downedCalamitas && DownedBossSystem.downedExoMechs, ModContent.ItemType<NO>(), 10, 1, 1, ui: false);
            }
            if (npc.type == ModContent.NPCType<HiveTumor>())
            {
                npcLoot.AddIf(()=> CalRemixWorld.grimesandToggle, ItemID.DemoniteOre, 1, 10, 26, ui: !CalRemixWorld.grimesandToggle);
            }
            if (npc.type == ModContent.NPCType<PerforatorCyst>())
            {
                npcLoot.AddIf(() => CalRemixWorld.grimesandToggle, ItemID.CrimtaneOre, 1, 10, 26, ui: !CalRemixWorld.grimesandToggle);
            }
            if (npc.type == ModContent.NPCType<Sulflounder>())
            {
                LeadingConditionRule flound = new LeadingConditionRule(new Conditions.IsExpert());
                flound.Add(ModContent.ItemType<FlounderMortar>(), 10, hideLootReport: !Main.expertMode);
                flound.AddFail(ModContent.ItemType<FlounderMortar>(), new Fraction(2, 30), hideLootReport: Main.expertMode);
                npcLoot.Add(flound);
            }
        }
        public override void OnKill(NPC npc)
        {
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().tvo)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<LumChunk>(), 0, 0, Main.myPlayer);
                }
            }
            else if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().cart)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<CalHeart>(), 0, 0, Main.myPlayer);
                }
            }
            if (npc.type == ModContent.NPCType<Horse>())
                CalRemixWorld.downedEarth = true;
            if (!Main.dedServ)
            {
                if (!CystMessage.alreadySeen && !CalRemixWorld.grimesandToggle)
                {
                    if (npc.type == ModContent.NPCType<PerforatorCyst>() || npc.type == ModContent.NPCType<HiveTumor>())
                    {
                        CystMessage.ActivateMessage();
                    }
                }
            }
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (npc.type == ModContent.NPCType<Crabulon>())
                binaryWriter.Write(crabSay);
            else if (npc.type == ModContent.NPCType<SlimeGodCore>())
                binaryWriter.Write(slimeSay);
            else if (npc.type == ModContent.NPCType<ProfanedGuardianCommander>())
                binaryWriter.Write(guardSay);
            else if (npc.type == ModContent.NPCType<Yharon>())
                binaryWriter.Write(yharSay);
            else if (npc.type == ModContent.NPCType<PrimordialWyrmHead>())
                binaryWriter.Write(jaredSay);

            if (npc.type == ModContent.NPCType<ProfanedGuardianCommander>())
            {
                binaryWriter.Write(guardRage);
                binaryWriter.Write(guardOver);
            }
            else if (npc.type == ModContent.NPCType<Yharon>())
                binaryWriter.Write(yharRage);
            if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
                binaryWriter.Write(npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == ModContent.NPCType<Crabulon>())
                crabSay = binaryReader.ReadInt32();
            else if (npc.type == ModContent.NPCType<SlimeGodCore>())
                slimeSay = binaryReader.ReadInt32();
            else if (npc.type == ModContent.NPCType<ProfanedGuardianCommander>())
                guardSay = binaryReader.ReadInt32();
            else if (npc.type == ModContent.NPCType<Yharon>())
                yharSay = binaryReader.ReadInt32();
            else if (npc.type == ModContent.NPCType<PrimordialWyrmHead>())
                jaredSay = binaryReader.ReadInt32();

            if (npc.type == ModContent.NPCType<ProfanedGuardianCommander>())
            {
                guardRage = binaryReader.ReadBoolean();
                guardOver = binaryReader.ReadBoolean();
            }
            else if (npc.type == ModContent.NPCType<Yharon>())
                yharRage = binaryReader.ReadBoolean();
            if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
                if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
                    npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = binaryReader.ReadBoolean();
        }
        public override bool PreKill(NPC npc)
        {
            if (CalRemixWorld.lifeoretoggle)
            { 
                if (!DownedBossSystem.downedRavager && npc.type == ModContent.NPCType<RavagerBody>())
                {
                    CalamityUtils.SpawnOre(ModContent.TileType<LifeOreTile>(), 0.25E-05, 0.45f, 0.65f, 30, 40);

                    Color messageColor = Color.Lime;
                    CalamityUtils.DisplayLocalizedText("Vitality sprawls throughout the underground.", messageColor);
                }
            }
            if (CalRemixWorld.shrinetoggle)
            {
                if (npc.type == NPCID.WallofFlesh && !Main.hardMode)
                {
                    if (!FannyManager.fannyEnabled)
                    {
                        // he's defrosting!
                        FannyManager.fannyEnabled = true;
                        FannyManager.fannyTimesFrozen++;
                        Anomaly109UI.fannyFreezeTime = 0;
                    }
                    CalRemixWorld.ShrineTimer = 3000;
                }
            }
            if (!CalRemixWorld.deusDeadInSnow)
            {
                if (Main.LocalPlayer.ZoneSnow && npc.type == ModContent.NPCType<AstrumDeusHead>())
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
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (vBurn)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 200;
                if (damage < 40)
                {
                    damage = 40;
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            #region CrabTalk
            if (npc.type == ModContent.NPCType<Crabulon>() && crabSay <= 1)
            {
                Talk("No? Please do be careful with that weapon, though, it looks kinda dangerous. Honestly, you seem quite... crabby. Get it?!", Color.LightSkyBlue);
                crabSay = 2;
            }
            else if (npc.type == ModContent.NPCType<Crabulon>() && npc.life <= 0 && crabSay == 4)
            {
                Talk("AAAAAAAAAh", Color.LightSkyBlue);
                crabSay = 5;
            }
            else if (npc.life <= 0 && npc.type == ModContent.NPCType<ProfanedGuardianCommander>())
            {
                Talk("Guardian Commander: MY MENTAL FORTITUDE IS FADING...", Color.Yellow);
            }
            else if (npc.life <= 0 && npc.type == ModContent.NPCType<ProfanedGuardianDefender>())
            {
                Talk("Guardian Defender: Nothing... can beat my eldest sibling...", Color.Gold);
                if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianCommander>()))
                    Talk("Guardian Commander: VERY SOON, YOU WILL FEEL MY PROFANED RAGE... HA-HA-HA...", Color.Yellow);
            }
            else if (npc.life <= 0 && npc.type == ModContent.NPCType<ProfanedGuardianHealer>())
            {
                Talk("Guardian Healer: Ouch!", Color.LavenderBlush);
                if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianDefender>()))
                    Talk("Guardian Defender: How? How could you!?", Color.Gold);
                if (NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianCommander>()))
                    Talk("Guardian Commander: ENOUGH! YOU MAY HAVE DEFEATED ONE OF US, BUT US TWO ARE MUCH TOUGHER!", Color.Yellow);
            }
            else if (npc.life <= 0 && npc.type == ModContent.NPCType<Yharon>())
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
        private static void Talk(string text, Color color)
        {
            if (!CalRemixWorld.bossdialogue)
                return;
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            else if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, color);
        }
        private static int KillHiveMind(Terraria.On_NPC.orig_NewNPC orig, IEntitySource spawnSource, int x, int y, int type, int star, float ai0, float ai1, float ai2, float ai3, int targ)
        {
            if (!CalRemixWorld.grimesandToggle)
                return orig(spawnSource, x, y, type, star, ai0, ai1, ai2, ai3, targ);
            if (spawnSource is EntitySource_Death)
            {
                if (type == ModContent.NPCType<HiveMind>())
                {
                    if (!(Main.zenithWorld && NPC.AnyNPCs(ModContent.NPCType<HiveMind>())))
                    {
                        return 0;
                    }
                    else
                    {
                        return orig(spawnSource, x, y, type, star, ai0, ai1, ai2, ai3, targ);
                    }
                }
                else if (type == ModContent.NPCType<PerforatorHive>())
                {
                    return 0;
                }
                else
                {
                    return orig(spawnSource, x, y, type, star, ai0, ai1, ai2, ai3, targ);
                }
            }
            else
            {
                return orig(spawnSource, x, y, type, star, ai0, ai1, ai2, ai3, targ);
            }
        }
    }
}
