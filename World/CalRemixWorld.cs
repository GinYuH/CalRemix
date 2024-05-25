using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System.IO;
using CalamityMod.World;
using System;
using CalRemix.Tiles;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Tiles.FurnitureMonolith;
using CalRemix.Items.Placeables;
using CalamityMod.Walls;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.Cryogen;
using CalRemix.UI;
using CalRemix.Tiles.PlaguedJungle;
using CalRemix.Items.Weapons;
using System.Threading;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.NPCs.Abyss;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Fishing.SulphurCatches;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Tiles.DraedonStructures;
using Terraria.WorldBuilding;
using CalamityMod.Tiles.FurnitureVoid;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.Bumblebirb;
using CalRemix.Retheme;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.DataStructures;
using CalamityMod.NPCs.AquaticScourge;
using Terraria.GameContent.Bestiary;
using CalamityMod.NPCs.HiveMind;
using Terraria.GameContent.Generation;
using SubworldLibrary;
using CalRemix.Subworlds;
using CalamityMod.Items.DraedonMisc;
using CalRemix.Items;
using Terraria.IO;
using CalRemix.World;
using CalamityMod.Items.Tools;
using Terraria.GameContent.Biomes;
using CalRemix.Walls;
using CalamityMod.Tiles.FurnitureStratus;

namespace CalRemix
{
    public class CalRemixWorld : ModSystem
    {
        public static bool worldFullyStarted = false;
        public static int worldLoadCounter = 0;

        public static bool ogslime = false;

        public static int lifeTiles;
        public static int asbestosTiles;
        public static int PlagueTiles;
        public static int PlagueDesertTiles;
        public static int MeldTiles;
        public static int ShrineTimer = -20;
        public static bool loadedRecipeInjections = false;

        public static bool guideHasExisted = false;
        public static bool deusDeadInSnow = false;
        public static bool generatedCosmiliteSlag = false;
        public static bool generatedPlague = false;
        public static bool generatedStrain = false;
        public static bool canGenerateBaron = false;
        public static bool grime = false;

        public static List<(int, int)> plagueBiomeArray = new List<(int, int)>();
        public static int meldCountdown = 72000;

        public static bool alloyBars = true;
        public static bool essenceBars = true;
        public static bool yharimBars = true;
        public static bool shimmerEssences = true;
        public static bool meldGunk = true;
        public static bool cosmislag = true;
        public static bool reargar = true;
        public static bool sidegar = true;
        public static bool frontgar = true;
        public static bool crocodile = true;
        public static bool permanenthealth = true;
        public static bool starbuster = true;
        public static bool plaguetoggle = true;
        public static bool shrinetoggle = true;
        public static bool lifeoretoggle = true;
        public static bool itemChanges = true;
        public static bool npcChanges = true;
        public static bool bossdialogue = true;
        public static bool grimesandToggle = true;
        public static bool clowns = true;
        public static bool aspids = true;
        public static bool clamitas = true;
        public static bool wolfvenom = true;
        public static bool fearmonger = true;
        public static bool seafood = true;
        public static bool laruga = true;
        public static bool acidsighter = true;
        public static bool greenDemon = true;
        public static bool remixJump = true;

        public static int ionQuestLevel = -1;
        public static bool wizardDisabled = false;

        public List<int> DungeonWalls = new List<int>
        {
            WallID.BlueDungeonUnsafe,
            WallID.BlueDungeonSlabUnsafe,
            WallID.BlueDungeonTileUnsafe,
            WallID.GreenDungeonUnsafe,
            WallID.GreenDungeonSlabUnsafe,
            WallID.GreenDungeonTileUnsafe,
            WallID.PinkDungeonUnsafe,
            WallID.PinkDungeonSlabUnsafe,
            WallID.PinkDungeonTileUnsafe
        };

        public static void UpdateWorldBool()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void OnWorldLoad()
        {
            ogslime = false;

            guideHasExisted = false;
            deusDeadInSnow = false;
            generatedCosmiliteSlag = false;
            generatedPlague = false;
            generatedStrain = false;
            canGenerateBaron = false;
            grime = false;
            meldCountdown = 72000;

            alloyBars = true;
            essenceBars = true;
            yharimBars = true;
            shimmerEssences = true;
            meldGunk = true;
            cosmislag = true;
            reargar = true;
            sidegar = true;
            frontgar = true;
            crocodile = true;
            permanenthealth = true;
            starbuster = true;
            plaguetoggle = true;
            shrinetoggle = true;
            lifeoretoggle = true;
            itemChanges = true;
            if (itemChanges)
            {
                foreach (KeyValuePair<int, string> p in RethemeList.Items)
                {
                    TextureAssets.Item[p.Key] = Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                }
                foreach (KeyValuePair<int, string> p in RethemeList.Projs)
                {
                    TextureAssets.Projectile[p.Key] = Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                }
                Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(6, 16));
            }
            else
            {
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Items)
                {
                    TextureAssets.Item[p.Key] = p.Value;
                }
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Projs)
                {
                    TextureAssets.Projectile[p.Key] = p.Value;
                }
                Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(1, 1));
            }
            npcChanges = true;
            if (npcChanges)
            {
                foreach (KeyValuePair<int, string> p in RethemeList.NPCs)
                {
                    TextureAssets.Npc[p.Key] = Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.NPCs)
                {
                    TextureAssets.Npc[p.Key] = p.Value;
                }
            }
            bossdialogue = true;
            grimesandToggle = true;
            clowns = true;
            aspids = true;
            clamitas = true;
            wolfvenom = true;
            fearmonger = true;
            seafood = true;
            laruga = true;
            acidsighter = true;
            greenDemon = true;
            remixJump = true;

            loadedRecipeInjections = false;

            ionQuestLevel = -1;
            wizardDisabled = false;
        }
        public override void OnWorldUnload()
        {
            worldFullyStarted = false;
            worldLoadCounter = 0;

            ogslime = false;

            guideHasExisted = false;
            deusDeadInSnow = false;
            generatedCosmiliteSlag = false;
            generatedPlague = false;
            generatedStrain = false;
            canGenerateBaron = false;

            alloyBars = true;
            essenceBars = true;
            yharimBars = true;
            meldCountdown = 72000;

            alloyBars = true;
            essenceBars = true;
            yharimBars = true;
            shimmerEssences = true;
            meldGunk = true;
            cosmislag = true;
            reargar = true;
            sidegar = true;
            frontgar = true;
            crocodile = true;
            permanenthealth = true;
            starbuster = true;
            plaguetoggle = true;
            shrinetoggle = true;
            lifeoretoggle = true;
            itemChanges = true;
            npcChanges = true;
            bossdialogue = true;
            grimesandToggle = true;
            clowns = true;
            aspids = true;
            clamitas = true;
            wolfvenom = true;
            fearmonger = true;
            seafood = true;
            laruga = true;
            acidsighter = true;
            greenDemon = true;
            remixJump = true;

            loadedRecipeInjections = false;

            ionQuestLevel = -1;
            wizardDisabled = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["ogslime"] = ogslime;

            tag["guideHasExisted"] = guideHasExisted;
            tag["deusDeadInSnow"] = deusDeadInSnow;
            tag["genSlag"] = generatedCosmiliteSlag;
            tag["plague"] = generatedPlague;
            tag["astrain"] = generatedStrain;
            tag["canBaron"] = canGenerateBaron;
            tag["grime"] = grime;
            tag["meld"] = meldCountdown;

            tag["109alloybar"] = alloyBars;
            tag["109essencebar"] = essenceBars;
            tag["109yharimbar"] = yharimBars;
            tag["109essenceshimmer"] = shimmerEssences;
            tag["109meldgunk"] = meldGunk;
            tag["109cosmilite"] = cosmislag;
            tag["109reargar"] = reargar;
            tag["109sidegar"] = sidegar;
            tag["109frontgar"] = frontgar;
            tag["109crocodile"] = crocodile;
            tag["109permanenthealth"] = permanenthealth;
            tag["109starbuster"] = starbuster;
            tag["109plague"] = plaguetoggle;
            tag["109shrine"] = shrinetoggle;
            tag["109lifeore"] = lifeoretoggle;
            tag["109itemchanges"] = itemChanges;
            tag["109npcchanges"] = npcChanges;
            tag["109dialogue"] = bossdialogue;
            tag["109grime"] = grimesandToggle;
            tag["109clowns"] = clowns;
            tag["109aspids"] = aspids;
            tag["109clamitas"] = clamitas;
            tag["109coyotevenom"] = wolfvenom;
            tag["109fearmonger"] = fearmonger;
            tag["109seafood"] = seafood;
            tag["109laruga"] = laruga;
            tag["109acidsighter"] = acidsighter;
            tag["109greenDemon"] = greenDemon;
            tag["109remixJump"] = remixJump;
            tag["ionQuest"] = ionQuestLevel;
            tag["wizardToggle"] = wizardDisabled;

            tag["109fanny"] = FannyManager.fannyEnabled;
            tag["109fannyfreeze"] = FannyManager.fannyTimesFrozen;

        }

        public override void LoadWorldData(TagCompound tag)
        {
            ogslime = tag.Get<bool>("ogslime");


            guideHasExisted = tag.Get<bool>("guideHasExisted");
            deusDeadInSnow = tag.Get<bool>("deusDeadInSnow");
            generatedCosmiliteSlag = tag.Get<bool>("genSlag");
            generatedPlague = tag.Get<bool>("plague");
            generatedStrain = tag.Get<bool>("astrain");
            canGenerateBaron = tag.Get<bool>("canBaron");
            grime = tag.Get<bool>("grime");
            meldCountdown = tag.Get<int>("meld");

            alloyBars = tag.Get<bool>("109alloybar");
            essenceBars = tag.Get<bool>("109essencebar");
            yharimBars = tag.Get<bool>("109yharimbar");
            shimmerEssences = tag.Get<bool>("109essenceshimmer");
            meldGunk = tag.Get<bool>("109meldgunk");
            cosmislag = tag.Get<bool>("109cosmilite");
            reargar = tag.Get<bool>("109reargar");
            sidegar = tag.Get<bool>("109sidegar");
            frontgar = tag.Get<bool>("109frontgar");
            crocodile = tag.Get<bool>("109crocodile");
            permanenthealth= tag.Get<bool>("109permanenthealth");
            starbuster= tag.Get<bool>("109starbuster");
            plaguetoggle= tag.Get<bool>("109plague");
            shrinetoggle= tag.Get<bool>("109shrine");
            lifeoretoggle= tag.Get<bool>("109lifeore");
            itemChanges= tag.Get<bool>("109itemchanges");
            npcChanges = tag.Get<bool>("109npcchanges");
            bossdialogue = tag.Get<bool>("109dialogue");
            grimesandToggle= tag.Get<bool>("109grime");
            clowns= tag.Get<bool>("109clowns");
            aspids= tag.Get<bool>("109aspids");
            clamitas= tag.Get<bool>("109clamitas");
            wolfvenom= tag.Get<bool>("109coyotevenom");
            fearmonger= tag.Get<bool>("109fearmonger");
            seafood= tag.Get<bool>("109seafood");
            laruga= tag.Get<bool>("109laruga");
            acidsighter = tag.Get<bool>("109acidsighter");
            greenDemon = tag.Get<bool>("109greenDemon");
            remixJump = tag.Get<bool>("109remixJump");
            ionQuestLevel = tag.Get<int>("ionQuest");
            wizardDisabled = tag.Get<bool>("wizardToggle");

            FannyManager.fannyEnabled = tag.Get<bool>("109fanny");
            FannyManager.fannyTimesFrozen = tag.Get<int>("109fannyfreeze");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(guideHasExisted);
            writer.Write(ogslime);
            writer.Write(deusDeadInSnow);
            writer.Write(generatedCosmiliteSlag);
            writer.Write(generatedPlague);
            writer.Write(generatedStrain);
            writer.Write(canGenerateBaron);
            writer.Write(grime);
            writer.Write(meldCountdown);

            writer.Write(alloyBars);
            writer.Write(essenceBars);
            writer.Write(yharimBars);
            writer.Write(shimmerEssences);
            writer.Write(meldGunk);
            writer.Write(cosmislag);
            writer.Write(reargar);
            writer.Write(sidegar);
            writer.Write(frontgar);
            writer.Write(crocodile);
            writer.Write(permanenthealth);
            writer.Write(starbuster);
            writer.Write(plaguetoggle);
            writer.Write(shrinetoggle);
            writer.Write(lifeoretoggle);
            writer.Write(itemChanges);
            writer.Write(npcChanges);
            writer.Write(bossdialogue);
            writer.Write(grimesandToggle);
            writer.Write(clowns);
            writer.Write(aspids);
            writer.Write(clamitas);
            writer.Write(wolfvenom);;
            writer.Write(fearmonger);
            writer.Write(seafood);
            writer.Write(laruga);
            writer.Write(acidsighter);
            writer.Write(greenDemon);
            writer.Write(remixJump);
            writer.Write(ionQuestLevel);
            writer.Write(wizardDisabled);

            writer.Write(FannyManager.fannyEnabled);
            writer.Write(FannyManager.fannyTimesFrozen);
            writer.Write(Anomaly109Manager.helpUnlocked);
        }

        public override void NetReceive(BinaryReader reader)
        {
            ogslime = reader.ReadBoolean();
            guideHasExisted = reader.ReadBoolean();
            deusDeadInSnow = reader.ReadBoolean();
            generatedCosmiliteSlag = reader.ReadBoolean();
            generatedPlague = reader.ReadBoolean();
            generatedStrain = reader.ReadBoolean();
            canGenerateBaron = reader.ReadBoolean();
            grime = reader.ReadBoolean();
            meldCountdown = reader.ReadInt32();

            alloyBars = reader.ReadBoolean();
            essenceBars = reader.ReadBoolean();
            yharimBars = reader.ReadBoolean();
            shimmerEssences = reader.ReadBoolean();//.Get<bool>("109essenceshimmer");// = shimmerEssences;
            meldGunk = reader.ReadBoolean();//.Get<bool>("109meldgunk");// = meldGunk;
            cosmislag = reader.ReadBoolean();//.Get<bool>("109cosmilite");// = cosmislag;
            reargar = reader.ReadBoolean();//.Get<bool>("109reargar");// = reargar;
            sidegar = reader.ReadBoolean();//.Get<bool>("109sidegar");// = sidegar;
            frontgar = reader.ReadBoolean();//.Get<bool>("109frontgar");// = frontgar;
            crocodile = reader.ReadBoolean();//.Get<bool>("109crocodile");// = crocodile;
            permanenthealth = reader.ReadBoolean();//.Get<bool>("109permanenthealth");// = permanenthealth;
            starbuster = reader.ReadBoolean();//.Get<bool>("109starbuster");// = starbuster;
            plaguetoggle = reader.ReadBoolean();//.Get<bool>("109plague");// = plaguetoggle;
            shrinetoggle = reader.ReadBoolean();//.Get<bool>("109shrine");// = shrinetoggle;
            lifeoretoggle = reader.ReadBoolean();//.Get<bool>("109lifeore");// = lifeoretoggle;
            itemChanges = reader.ReadBoolean();//.Get<bool>("109resprites");// = resprites;
            npcChanges = reader.ReadBoolean();//.Get<bool>("109renames");// = renames;
            bossdialogue = reader.ReadBoolean();//.Get<bool>("109dialogue");// = bossdialogue;
            grimesandToggle = reader.ReadBoolean();//.Get<bool>("109grime");// = grimesand;
            clowns = reader.ReadBoolean();//.Get<bool>("109clowns");// = clowns;
            aspids = reader.ReadBoolean();//.Get<bool>("109aspids");// = aspids;
            clamitas = reader.ReadBoolean();//.Get<bool>("109clamitas");// = clamitas;
            wolfvenom = reader.ReadBoolean();//.Get<bool>("109coyotevenom");// = wolfvenom;
            fearmonger = reader.ReadBoolean();//.Get<bool>("109fearmonger");// = fearmonger;
            seafood = reader.ReadBoolean();//.Get<bool>("109seafood");// = seafood;
            laruga = reader.ReadBoolean();//.Get<bool>("109laruga");// = laruga;
            acidsighter = reader.ReadBoolean();
            greenDemon = reader.ReadBoolean();
            remixJump = reader.ReadBoolean();
            ionQuestLevel = reader.ReadInt32();
            wizardDisabled = reader.ReadBoolean();

            FannyManager.fannyEnabled = reader.ReadBoolean();
            FannyManager.fannyTimesFrozen = reader.ReadInt32();
            Anomaly109Manager.helpUnlocked = reader.ReadBoolean();
        }

        public static List<int> hallowlist = new List<int>
        {
            TileID.Pearlstone,
            TileID.HallowedIce,
            TileID.HallowHardenedSand,
            TileID.HallowSandstone
        };
        public static List<int> astrallist = new List<int>
        {
            TileType<AstralStone>(),
            TileType<AstralSandstone>(),
            TileType<HardenedAstralSand>(),
            TileType<CelestialRemains>(),
            TileType<NovaeSlag>(),
            TileType<AstralDirt>(),
            TileType<AstralIce>(),
            TileType<AstralSnow>(),
            TileType<AstralGrass>(),
            TileType<AstralClay>(),
            TileType<AstralSand>(),
            TileType<AstralMonolith>(),
        };
        public override void PreUpdateWorld()
        {
            if (worldLoadCounter < 180)
                worldLoadCounter++;
            else
                worldFullyStarted = true;
            if (!loadedRecipeInjections)
            {
                if (reargar)
                {
                    RemoveLoot(ItemID.JungleFishingCrate, ItemType<CalamityMod.Items.Placeables.Ores.UelibloomOre>());
                    RemoveLoot(ItemID.JungleFishingCrate, ItemType<CalamityMod.Items.Materials.UelibloomBar>());
                    RemoveLoot(ItemID.JungleFishingCrateHard, ItemType<CalamityMod.Items.Placeables.Ores.UelibloomOre>());
                    RemoveLoot(ItemID.JungleFishingCrateHard, ItemType<CalamityMod.Items.Materials.UelibloomBar>());
                }
                if (frontgar)
                {
                    RemoveLoot(ItemType<SulphurousCrate>(), ItemType<ReaperTooth>());
                    RemoveLoot(NPCType<ReaperShark>(), ItemType<ReaperTooth>(), true);
                }
                if (cosmislag)
                {
                    RemoveLoot(NPCType<DevourerofGodsHead>(), ItemType<CosmiliteBar>(), true);
                }
                RemoveLoot(NPCType<DesertScourgeHead>(), ItemType<PearlShard>(), true);
                RemoveLoot(NPCType<Bumblefuck>(), ItemType<EffulgentFeather>(), true);
                Recipes.MassModifyIngredient(!yharimBars, Recipes.yharimBarCrafts);
                Recipes.MassModifyIngredient(!alloyBars, Recipes.alloyBarCrafts);
                Recipes.MassModifyIngredient(!essenceBars, Recipes.essenceBarCrafts);
                Recipes.MassModifyIngredient(!crocodile, Recipes.crocodileCrafts);
                Recipes.MassModifyIngredient(!wolfvenom, Recipes.venomCrafts);
                loadedRecipeInjections = true;
                //RemoveLoot(NPCType<DevourerofGodsHead>(), ItemType<PearlShard>(), true);
            }
            if (Main.eclipse)
            {
                if (TextureAssets.Sun3 == CalRemix.sunOG)
                {
                    if (DownedBossSystem.downedDoG)
                    {
                        TextureAssets.Sun3 = CalRemix.sunCreepy;
                    }
                    else
                    {
                        TextureAssets.Sun3 = CalRemix.sunOG;
                    }
                }
            }
            NPC.savedWizard = false;
        }
        public static void RefreshBestiary(BestiaryEntry entry, NPC npc, string text)
        {
            entry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption);
            entry.Info.Add(new FlavorTextBestiaryInfoElement(text));
            npc.ModNPC?.SetBestiary(Main.BestiaryDB, entry);
        }
        public override void PostUpdateWorld()
        {
            NPC heart = ContentSamples.NpcsByNetId[NPCType<DarkHeart>()];
            if (!BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(heart.type).Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption))
            {
                NPC creep = ContentSamples.NpcsByNetId[NPCType<DankCreeper>()];
                NPC blob = ContentSamples.NpcsByNetId[NPCType<HiveBlob>()];
                RefreshBestiary(BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(creep.type), creep, "When threatened by outside forces, chunks of the Hive Mind knocked loose in combat will animate in attempt to subdue their attacker. Each Creeper knocked loose shrinks the brain ever so slightly- though this is an inherently selfdestructive self defense mechanism, any survivors will rejoin with the main body should the threat pass.");
                RefreshBestiary(BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(heart.type), heart, "Flying sacs filled with large amounts of caustic liquid. The Hive Mind possesses a seemingly large amount of these hearts, adding to its strange biology.");
                RefreshBestiary(BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(blob.type), blob, "Clustering globs ejected from the Hive Mind. The very nature of these balls of matter act as a common example of the convergent properties that the Corruption's microorganisms possess.");
            }
            if (meldCountdown > 0)
            {
                meldCountdown--;
            }
            if (aspids)
            {
                if (CalRemixGlobalNPC.aspidCount >= 20 && !DownedBossSystem.downedCryogen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.SpawnOnPlayer(Main.myPlayer, NPCType<Cryogen>());
                    }
                    CalRemixGlobalNPC.aspidCount = 0;
                }
            }
            if (CalamityMod.World.CalamityWorld.spawnedCirrus)
            {
                CalamityMod.World.CalamityWorld.spawnedCirrus = false;
            }
            if (NPC.AnyNPCs(NPCID.Guide)) guideHasExisted = true;
            if (shrinetoggle)
            {
                if (ShrineTimer == 0)
                {
                    HMChest(TileID.CrystalBlock, TileID.CrystalBlock, WallID.Crystal, ItemType<HallowEffigy>(), hallowlist, 21);
                    HMChest(TileType<AstralMonolith>(), TileType<AstralMonolith>(), WallType<AstralMonolithWall>(), ItemType<AstralEffigy>(), astrallist, 46);

                    Color messageColor = Color.Magenta;
                    CalamityUtils.DisplayLocalizedText("Shrines appear within the newly spread infections!", messageColor);
                    if (DateTime.Now.Month == 4 && DateTime.Now.Day <= 7 && CalRemix.CalVal != null)
                    {
                        ThreadPool.QueueUserWorkItem(_ => AstralBlightBiome.GenerateBlight(), this);
                    }
                }
            }
            if (ShrineTimer > -20)
            {
                ShrineTimer--;
            }
            if (cosmislag)
            {
                if (!generatedCosmiliteSlag)
                {
                    if (NPC.downedMoonlord)
                    {
                        if (CalamityWorld.HasGeneratedLuminitePlanetoids)
                        {
                            //ThreadPool.QueueUserWorkItem(_ => GenerateCosmiliteSlag());
                            PlanetoidGeneration.GenerateCosmiliteSlag();
                        }
                    }
                }
            }
            if (plaguetoggle)
            {
                if (!generatedPlague && NPC.downedGolemBoss)
                {
                    ThreadPool.QueueUserWorkItem(_ => PlagueGeneration.GeneratePlague(), this);
                }
            }
            //if (Main.LocalPlayer.HeldItem.type == ItemID.CopperAxe && Main.LocalPlayer.controlUseItem)
            if (meldGunk)
            {
                if (!generatedStrain && Main.hardMode)
                {
                    MeldStrain.GenerateCavernStrain();
                    generatedStrain = true;
                    UpdateWorldBool();
                }
            }
            if (!NPC.AnyNPCs(NPCType<AquaticScourgeHead>()))
            {
                if (CalamityUtils.CountProjectiles(ProjectileID.ChumBucket) > 21 && Main.LocalPlayer.Calamity().ZoneSulphur)
                {
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p != null && p.active)
                        {
                            if (p.type == ProjectileID.ChumBucket)
                            {
                                p.Kill();
                            }
                        }
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.SpawnOnPlayer(Main.myPlayer, NPCType<AquaticScourgeHead>());
                    }
                }
            }
            if (CalRemix.CalVal != null && ((DateTime.Now.Day == 1 && DateTime.Now.Month == 4) || (DateTime.Now.Month == 4 && DateTime.Now.Day <= 7 && Main.zenithWorld)))
            {
                FannyManager.fannyEnabled = false;
            }
            if (SubworldSystem.IsActive<FannySubworld>())
            {
                if (Main.LocalPlayer.position.X > 16 * (Main.maxTilesX - 130))
                {
                    Main.LocalPlayer.position.X = (Main.spawnTileX + 60) * 16;
                }
            }
        }

        public static void AddLootDynamically(int npcType, bool npc = false)
        {
            if (npc)
            {
                if (npcType == NPCType<ReaperShark>())
                {
                    var postPolter = new LeadingConditionRule(DropHelper.PostPolter());
                    postPolter.Add(ItemType<ReaperTooth>(), 1, 3, 4);
                    Terraria.Main.ItemDropsDB.RegisterToNPC(npcType, postPolter);
                }
            }
            else
            {
                if (npcType == ItemType<SulphurousCrate>())
                {
                    // dude i fucking FUCKING LOATHE drop code
                    var postDuke = new LeadingConditionRule(DropHelper.PostOD());
                    postDuke.Add(ItemType<ReaperTooth>(), 10, 1, 5);
                    var postPolter = new LeadingConditionRule(DropHelper.PostPolter()).OnSuccess(postDuke);
                    Terraria.Main.ItemDropsDB.RegisterToItem(npcType, postPolter);
                }
                if (npcType == ItemID.JungleFishingCrate)
                {
                    var postDuke = new LeadingConditionRule(DropHelper.PostProv());
                    postDuke.Add(ItemType<CalamityMod.Items.Placeables.Ores.UelibloomOre>(), 5, 16, 28);
                    postDuke.Add(ItemType<CalamityMod.Items.Materials.UelibloomBar>(), new Fraction(15, 100), 4, 7);
                    Terraria.Main.ItemDropsDB.RegisterToItem(npcType, postDuke);
                    Terraria.Main.ItemDropsDB.RegisterToItem(ItemID.JungleFishingCrateHard, postDuke);
                }
            }
        }
        public static void RemoveLoot(int bagType, int itemToRemove, bool npc = false)
        {
            List<IItemDropRule> JungleCrateDrops = npc ? Terraria.Main.ItemDropsDB.GetRulesForNPCID(bagType) : Terraria.Main.ItemDropsDB.GetRulesForItemID(bagType);
            for (int i = 0; i < JungleCrateDrops.Count; i++)
            {
                if (JungleCrateDrops[i] is LeadingConditionRule lead)
                {
                    for (int j = 0; j < lead.ChainedRules.Count; j++)
                    {
                        if (lead.ChainedRules[j] is Chains.TryIfSucceeded c)
                        {
                            if (c.RuleToChain is CommonDrop fuck)
                            {
                                if (fuck.itemId == itemToRemove)
                                {
                                    lead.ChainedRules.RemoveAt(j);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void ResetNearbyTileEffects()
        {
            asbestosTiles = 0;
            lifeTiles = 0;
            PlagueTiles = 0;
            PlagueDesertTiles = 0;
            MeldTiles = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            // Life Ore tiles
            lifeTiles = tileCounts[TileType<LifeOreTile>()];
            asbestosTiles = tileCounts[TileType<AsbestosPlaced>()];
            PlagueTiles = tileCounts[TileType<PlaguedGrass>()] +
            tileCounts[TileType<PlaguedMud>()] +
            tileCounts[TileType<PlaguedStone>()] +
            tileCounts[TileType<PlaguedClay>()] +
            tileCounts[TileType<OvergrownPlaguedStone>()] +
            tileCounts[TileType<PlaguedSilt>()] +
            tileCounts[TileType<Sporezol>()];
            PlagueDesertTiles = tileCounts[TileType<PlaguedSand>()];
            MeldTiles = tileCounts[TileType<MeldGunkPlaced>()];
            Main.SceneMetrics.JungleTileCount += PlagueTiles;
            Main.SceneMetrics.SandTileCount += PlagueDesertTiles;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int FinalIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Roxcalibur"));
            int GraniteIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite"));
            tasks.Insert(GraniteIndex, new PassLegacy("Asbestos", (progress, passConfig) =>
            {
                progress.Message = "Spreading the True Plague";
                int biomeAmount = 10;
                double xRange = (double)(Main.maxTilesX - 200) / (double)biomeAmount;
                List<Point> placementPoints = new List<Point>(biomeAmount);
                int tries = 0;
                int currentBiomeAmount = 0;
                while (currentBiomeAmount < biomeAmount)
                {
                    double biomeRaito = (double)currentBiomeAmount / (double)biomeAmount;
                    progress.Set(biomeRaito);
                    Point placementPoint = WorldGen.RandomRectanglePoint((int)(biomeRaito * (double)(Main.maxTilesX - 200)) + 100, (int)GenVars.rockLayer + 20, (int)xRange, Main.maxTilesY - ((int)GenVars.rockLayer + 40) - 200);
                    if (WorldGen.remixWorldGen)
                    {
                        placementPoint = WorldGen.RandomRectanglePoint((int)(biomeRaito * (double)(Main.maxTilesX - 200)) + 100, (int)GenVars.worldSurface + 100, (int)xRange, (int)GenVars.rockLayer - (int)GenVars.worldSurface - 100);
                    }
                    while ((double)placementPoint.X > (double)Main.maxTilesX * 0.45 && (double)placementPoint.X < (double)Main.maxTilesX * 0.55)
                    {
                        placementPoint.X = WorldGen.genRand.Next(WorldGen.beachDistance, Main.maxTilesX - WorldGen.beachDistance);
                    }
                    tries++;
                    if (AsbestosBiome.CanPlace(placementPoint, GenVars.structures))
                    {
                        placementPoints.Add(placementPoint);
                        currentBiomeAmount++;
                    }
                    else if (tries > Main.maxTilesX * 10)
                    {
                        biomeAmount = currentBiomeAmount;
                        currentBiomeAmount++;
                        tries = 0;
                    }
                }
                AsbestosBiome asBiome = GenVars.configuration.CreateBiome<AsbestosBiome>();
                for (int i = 0; i < biomeAmount; i++)
                {
                    asBiome.Place(placementPoints[i], GenVars.structures);
                }
                AsbestosBiome.GenerateAllHouses();
            }));
            if (FinalIndex != -1)
            {
                tasks.Insert(FinalIndex, new PassLegacy("Dungeon Retheme", (progress, config) =>
                {
                progress.Message = "Remodeling the Dungeon";
                StratusDungeon.ReplaceDungeon();
                StratusDungeon.AddOriginalDungeonHoles();
                CalamityUtils.SpawnOre(TileType<ArsenicOrePlaced>(), 15E-01, 0.4f, 1f, 3, 8, new int[3] { TileID.BlueDungeonBrick, TileID.PinkDungeonBrick, TileID.GreenDungeonBrick });
                }));
                tasks.Insert(FinalIndex, new PassLegacy("Ion Altar", (progress, config) => { IonAltar.GenerateIonAltar(); }));
            }
            // Secret Banished Baron seed
            if (WorldGen.currentWorldSeed.ToLower() == "banishedbaron")
            {
                // Clear all worldgen tasks, they won't be needed at all and only serve to clutter precious time
                tasks.Clear();
                tasks.Add(new PassLegacy("Banishing The Baron", (progress, config) =>
                {
                    progress.Message = "Creating a Baron Wasteland";
                    BaronStrait.GenerateBaronStrait(null); 
                }));
            }
        }

        public override void PostWorldGen()
        {
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null)
                {
                    if (Main.tile[chest.x, chest.y].TileType == ModContent.TileType<StratusChest>())
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                if (Main.rand.NextBool(4))
                                {
                                    chest.item[inventoryIndex].SetDefaults(ItemType<BundleBones>());
                                    chest.item[inventoryIndex].stack = Main.rand.Next(10, 26);
                                }
                                break;
                            }
                        }
                    }
                    if (Main.tile[chest.x, chest.y].TileType == TileType<VoidChest>())
                    {
                        if (chest.item[0].type == ItemType<Terminus>())
                        {
                            chest.item[0].SetDefaults(ItemType<FannyLogAbyss>());
                        }
                    }
                    if (Main.tile[chest.x, chest.y].TileType == TileType<SecurityChestTile>() || Main.tile[chest.x, chest.y].TileType == TileType<AgedSecurityChestTile>())
                    {
                        bool getShifty = false;
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (inventoryIndex == 38)
                                break;
                            if (chest.item[inventoryIndex].type == ItemType<DubiousPlating>())
                            {
                                if (chest.item[inventoryIndex + 1] != null)
                                {
                                    chest.item[inventoryIndex] = chest.item[inventoryIndex + 1];
                                    getShifty = true;
                                }
                            }
                            if (getShifty)
                            {
                                if (chest.item[inventoryIndex + 1] != null)
                                {
                                    chest.item[inventoryIndex] = chest.item[inventoryIndex + 1];
                                }
                                else
                                {
                                    chest.item[inventoryIndex] = null;
                                    break;
                                }
                            }
                        }
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemType<DraedonsLogHell>())
                            {
                                chest.item[inventoryIndex].SetDefaults(ItemType<FannyLogHell>());
                                break;
                            }
                            if (chest.item[inventoryIndex].type == ItemType<DraedonsLogJungle>())
                            {
                                chest.item[inventoryIndex].SetDefaults(ItemType<FannyLogJungle>());
                                break;
                            }
                            if (chest.item[inventoryIndex].type == ItemType<DraedonsLogSnowBiome>())
                            {
                                chest.item[inventoryIndex].SetDefaults(ItemType<FannyLogIce>());
                                break;
                            }
                            if (chest.item[inventoryIndex].type == ItemType<DraedonsLogSunkenSea>())
                            {
                                chest.item[inventoryIndex].SetDefaults(ItemType<FannyLogSunkenSea>());
                                break;
                            }
                            if (chest.item[inventoryIndex].type == ItemType<DraedonsLogPlanetoid>())
                            {
                                chest.item[inventoryIndex].SetDefaults(ItemType<FannyLogSpace>());
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void HMChest(int block1, int block2, int wall, int loot, List<int> anchor, int chest)
        {
            int x = Main.maxTilesX;
            int y = Main.maxTilesY;
            for (int k = 0; k < (int)(x * y * 100E-05); k++)
            {
                int tilesX = WorldGen.genRand.Next(0, x);
                int tilesY = WorldGen.genRand.Next((int)(y * 0.35f), (int)(y * 0.8f));

                if (anchor.Contains(Main.tile[tilesX, tilesY].TileType))
                {
                    UndergroundShrines.SpecialHut((ushort)block1, (ushort)Main.tile[tilesX, tilesY].TileType, (ushort)wall, UndergroundShrines.UndergroundShrineType.Surface, tilesX, tilesY);
                    for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                    {
                        Chest cheste = Main.chest[chestIndex];
                        if (cheste != null && Main.tile[cheste.x, cheste.y].TileType == TileID.Containers && Main.tile[cheste.x, cheste.y + 2].TileType == block1)
                        {
                            if (block1 == TileID.CrystalBlock)
                            {
                                Main.tile[cheste.x, cheste.y].TileType = TileID.Containers2;
                                Main.tile[cheste.x + 1, cheste.y].TileType = TileID.Containers2;
                                Main.tile[cheste.x, cheste.y + 1].TileType = TileID.Containers2;
                                Main.tile[cheste.x + 1, cheste.y + 1].TileType = TileID.Containers2;
                            }
                            if (block1 == TileType<AstralMonolith>())
                            {
                                Main.tile[cheste.x, cheste.y].TileType = (ushort)TileType<MonolithChest>();
                                Main.tile[cheste.x + 1, cheste.y].TileType = (ushort)TileType<MonolithChest>();
                                Main.tile[cheste.x, cheste.y + 1].TileType = (ushort)TileType<MonolithChest>();
                                Main.tile[cheste.x + 1, cheste.y + 1].TileType = (ushort)TileType<MonolithChest>();
                            }
                            cheste.item[0].SetDefaults(loot);
                        }
                    }
                    break;
                }
            }
        }
    }
}