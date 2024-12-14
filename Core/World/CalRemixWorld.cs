using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System.IO;
using CalamityMod.World;
using System;
using CalRemix.Content.Tiles;
using CalamityMod.Walls;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.Cryogen;
using CalRemix.UI;
using CalRemix.Content.Tiles.PlaguedJungle;
using CalRemix.Content.Items.Weapons;
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
using Terraria.GameContent;
using Terraria.DataStructures;
using CalamityMod.NPCs.AquaticScourge;
using Terraria.GameContent.Generation;
using SubworldLibrary;
using CalRemix.Core.Subworlds;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Tiles.FurnitureStratus;
using CalamityMod.Tiles.SunkenSea;
using CalRemix.Content.NPCs.Bosses.Oxygen;
using CalRemix.Content.NPCs;
using CalRemix.UI.Anomaly109;
using CalRemix.Content.Items.Lore;
using CalamityMod.Tiles;
using CalRemix.Content.NPCs.Bosses.Wulfwyrm;
using Terraria.Audio;
using CalRemix.Core.Scenes;
using CalRemix.Content.Tiles.Plates;
using CalamityMod.Tiles.Plates;

namespace CalRemix.Core.World
{
    public class CalRemixWorld : ModSystem
    {
        public const int ROACHDURATIONSECONDS = 22;
        public const int maxStoryTime = 660;

        public static bool worldFullyStarted = false;
        public static int worldLoadCounter = 0;

        public static bool ogslime = false;

        public static int asbestosTiles;
        public static int lifeTiles;
        public static int vernalTiles;
        public static int PlagueTiles;
        public static int PlagueDesertTiles;
        public static int baronTiles;
        public static int MeldTiles;
        public static int strongholdTiles;
        public static int elumplateTiles;
        public static int aeroplateTiles;
        public static int havocplateTiles;
        public static int cinderplateTiles;
        public static int navyplateTiles;
        public static int plagueplateTiles;
        public static int bloodplateTiles;
        public static int onyxplateTiles;

        public static int ShrineTimer = -20; 
        public static int RoachCountdown = 0;
        public static int roachDuration = 0;
        public static bool loadedRecipeInjections = false;

        public static bool guideHasExisted = false;
        public static bool deusDeadInSnow = false;
        public static bool generatedCosmiliteSlag = false;
        public static bool generatedPlague = false;
        public static bool generatedStrain = false;
        public static bool generatedHydrogen = false;
        public static bool canGenerateBaron = false;
        public static bool generatedGrime = false;
        public static int trueStory = 0;

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
        public static bool hydrogenBomb = true;
        public static bool baronStrait = true;
        public static bool dyeStats = true;
        public static bool champions = true;
        public static bool astralBlight = true;
        public static bool mullet = true;
        public static bool deliciousMeat = true;
        public static bool profanedDesert = true;

        public static int ionQuestLevel = -1;
        public static bool wizardDisabled = false;

        public static int oxydayTime = 0;

        public static Vector2 hydrogenLocation = new Vector2(0, 0);

        public static bool stratusDungeonDisabled = false;
        public static NPC butterflyEffect = null;

        public static List<int> DungeonWalls = new List<int>
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

        public static List<int> SunkenSeaTiles = new List<int>
        {
            TileType<SeaAnemone>(),
            TileType<TubeCoral>(),
            TileType<BrainCoral>(),
            TileType<SmallBrainCoral>(),
            TileType<TableCoral>(),
            TileType<SmallCorals>(),
            TileType<MediumCoral>(),
            TileType<MediumCoral2>(),
            TileType<CoralPileLarge>(),
            TileType<SmallWideCoral>(),
            TileType<SmallWideCoral2>(),
            TileType<SunkenStalactite1>(),
            TileType<SunkenStalactite2>(),
            TileType<SunkenStalactite3>(),
            TileType<SunkenStalactitesSmall>(),
            TileType<SunkenStalagmite1>(),
            TileType<SunkenStalagmite2>(),
            TileType<SunkenStalagmite3>(),
            TileType<SunkenStalagmitesSmall>(),
            TileType<Navystone>(),
            TileType<EutrophicSand>(),
            TileType<HardenedEutrophicSand>(),
            TileType<SeaPrism>(),
            TileType<SeaPrismCrystals>(),
            TileType<FanCoral>(),
            TileType<SmallTubeCoral>(),
            TileID.MinecartTrack,
            TileID.Obsidian,
            TileID.WaterDrip

        };

        public static void UpdateWorldBool()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }

        public static void ResetFlags()
        {
            // Misc ints
            meldCountdown = 72000;
            trueStory = 0;
            RoachCountdown = 0;
            roachDuration = 0;

            // Misc bools
            guideHasExisted = false;
            deusDeadInSnow = false;
            ogslime = false;
            loadedRecipeInjections = false;

            // Worldgen
            generatedCosmiliteSlag = false;
            generatedPlague = false;
            generatedStrain = false;
            canGenerateBaron = false;
            generatedHydrogen = false;
            generatedGrime = false;

            // Gen boss stuff
            ionQuestLevel = -1;
            wizardDisabled = false;
            oxydayTime = 0;

            // Fanny

            // A109
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
            hydrogenBomb = true;
            baronStrait = true;
            dyeStats = true;
            champions = true;
            astralBlight = true;
            mullet = true;
            deliciousMeat = true;
            profanedDesert = true;
        }

        public override void OnWorldLoad()
        {
            ResetFlags();
            FindCentralGeode();
        }

        public override void OnWorldUnload()
        {
            ResetFlags();
            worldFullyStarted = false;
            worldLoadCounter = 0;
            hydrogenLocation = Vector2.Zero;
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
            tag["genHydrogen"] = generatedHydrogen;
            tag["grime"] = generatedGrime;
            tag["meld"] = meldCountdown;
            tag["trueStory"] = trueStory;
            tag["roachDuration"] = roachDuration;

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
            tag["109hydrogen"] = hydrogenBomb;
            tag["109baron"] = baronStrait;
            tag["109dye"] = dyeStats;
            tag["109champ"] = champions;
            tag["109blight"] = astralBlight;
            tag["109mullet"] = mullet;
            tag["109deliciousMeat"] = deliciousMeat;
            tag["109profaned"] = profanedDesert;

            tag["ionQuest"] = ionQuestLevel;
            tag["wizardToggle"] = wizardDisabled;
            tag["hydrolocationX"] = hydrogenLocation.X;
            tag["hydrolocationY"] = hydrogenLocation.Y;
            tag["oxytime"] = oxydayTime;

            tag["109fanny"] = ScreenHelperManager.screenHelpersEnabled;
            tag["109fannyfreeze"] = ScreenHelperManager.fannyTimesFrozen;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            GetData(ref ogslime, "ogslime", tag);

            GetData(ref guideHasExisted, "guideHasExisted", tag);
            GetData(ref deusDeadInSnow, "deusDeadInSnow", tag);
            GetData(ref generatedCosmiliteSlag, "genSlag", tag);
            GetData(ref generatedStrain, "astrain", tag);
            GetData(ref canGenerateBaron, "canBaron", tag);
            GetData(ref generatedHydrogen, "genHydrogen", tag);
            GetData(ref generatedGrime, "grime", tag);

            GetData(ref alloyBars, "109alloybar", tag);
            GetData(ref essenceBars, "109essencebar", tag);
            GetData(ref yharimBars, "109yharimbar", tag);
            GetData(ref shimmerEssences, "109essenceshimmer", tag);
            GetData(ref meldGunk, "109meldgunk", tag);
            GetData(ref cosmislag, "109cosmilite", tag);
            GetData(ref reargar, "109reargar", tag);
            GetData(ref sidegar, "109sidegar", tag);
            GetData(ref frontgar, "109frontgar", tag);
            GetData(ref crocodile, "109crocodile", tag);
            GetData(ref permanenthealth, "109permanenthealth", tag);
            GetData(ref starbuster, "109starbuster", tag);
            GetData(ref plaguetoggle, "109plague", tag);
            GetData(ref shrinetoggle, "109shrine", tag);
            GetData(ref lifeoretoggle, "109lifeore", tag);
            GetData(ref itemChanges, "109itemchanges", tag);
            GetData(ref npcChanges, "109npcchanges", tag);
            GetData(ref bossdialogue, "109dialogue", tag);
            GetData(ref grimesandToggle, "109grime", tag);
            GetData(ref clowns, "109clowns", tag);
            GetData(ref aspids, "109aspids", tag);
            GetData(ref clamitas, "109clamitas", tag);
            GetData(ref wolfvenom, "109coyotevenom", tag);
            GetData(ref fearmonger, "109fearmonger", tag);
            GetData(ref seafood, "109seafood", tag);
            GetData(ref laruga, "109laruga", tag);
            GetData(ref acidsighter, "109acidsighter", tag);
            GetData(ref greenDemon, "109greenDemon", tag);
            GetData(ref remixJump, "109remixJump", tag);
            GetData(ref hydrogenBomb, "109hydrogen", tag);
            GetData(ref baronStrait, "109baron", tag);
            GetData(ref dyeStats, "109dye", tag);
            GetData(ref champions, "109champ", tag);
            GetData(ref astralBlight, "109blight", tag);
            GetData(ref mullet, "109mullet", tag);
            GetData(ref deliciousMeat, "109deliciousmeat", tag);
            GetData(ref profanedDesert, "109profaned", tag);
            GetData(ref ScreenHelperManager.screenHelpersEnabled, "109fanny", tag);

            meldCountdown = tag.Get<int>("meld");
            trueStory = tag.Get<int>("trueStory");
            roachDuration = tag.Get<int>("roachDuration");

            ionQuestLevel = tag.Get<int>("ionQuest");
            wizardDisabled = tag.Get<bool>("wizardToggle");
            hydrogenLocation.X = tag.Get<float>("hydrolocationX");
            hydrogenLocation.Y = tag.Get<float>("hydrolocationY");
            oxydayTime = tag.Get<int>("oxytime");

            ScreenHelperManager.fannyTimesFrozen = tag.Get<int>("109fannyfreeze");
        }

        public static void GetData(ref bool baseVar, string path, TagCompound tag)
        {
            if (tag.ContainsKey(path))
            {                
                baseVar = tag.Get<bool>(path);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(ogslime);
            writer.Write(guideHasExisted);
            writer.Write(deusDeadInSnow);
            writer.Write(generatedCosmiliteSlag);
            writer.Write(generatedPlague);
            writer.Write(generatedStrain);
            writer.Write(canGenerateBaron);
            writer.Write(generatedHydrogen);
            writer.Write(generatedGrime);
            writer.Write(meldCountdown);
            writer.Write(trueStory);
            writer.Write(RoachCountdown);
            writer.Write(roachDuration);

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
            writer.Write(wolfvenom); ;
            writer.Write(fearmonger);
            writer.Write(seafood);
            writer.Write(laruga);
            writer.Write(acidsighter);
            writer.Write(greenDemon);
            writer.Write(remixJump);
            writer.Write(hydrogenBomb);
            writer.Write(baronStrait);
            writer.Write(dyeStats);
            writer.Write(champions);
            writer.Write(astralBlight);
            writer.Write(mullet);
            writer.Write(deliciousMeat);
            writer.Write(profanedDesert);

            writer.Write(ionQuestLevel);
            writer.Write(wizardDisabled);
            writer.Write(hydrogenLocation.X);
            writer.Write(hydrogenLocation.Y);
            writer.Write(oxydayTime);

            writer.Write(ScreenHelperManager.screenHelpersEnabled);
            writer.Write(ScreenHelperManager.fannyTimesFrozen);
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
            generatedHydrogen = reader.ReadBoolean();
            generatedGrime = reader.ReadBoolean();
            meldCountdown = reader.ReadInt32();
            trueStory = reader.ReadInt32();
            RoachCountdown = reader.ReadInt32();
            roachDuration = reader.ReadInt32();

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
            hydrogenBomb = reader.ReadBoolean();
            baronStrait = reader.ReadBoolean();
            dyeStats = reader.ReadBoolean();
            champions = reader.ReadBoolean();
            astralBlight = reader.ReadBoolean();
            mullet = reader.ReadBoolean();
            deliciousMeat = reader.ReadBoolean();
            profanedDesert = reader.ReadBoolean();

            ionQuestLevel = reader.ReadInt32();
            wizardDisabled = reader.ReadBoolean();
            hydrogenLocation.X = reader.ReadSingle();
            hydrogenLocation.Y = reader.ReadSingle();
            oxydayTime = reader.ReadInt32();

            ScreenHelperManager.screenHelpersEnabled = reader.ReadBoolean();
            ScreenHelperManager.fannyTimesFrozen = reader.ReadInt32();
            Anomaly109Manager.helpUnlocked = reader.ReadBoolean();
        }

        public override void PreUpdateWorld()
        {
            /*if (Main.LocalPlayer.selectedItem == 1 && Main.LocalPlayer.controlUseItem)
            {
                IonAltar.GenerateIonAltar();
            }*/
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
            if (butterflyEffect != null && !NPC.AnyNPCs(NPCType<FunnyButterfly>()))
                butterflyEffect = null;
            if (Main.eclipse)
            {
                if (TextureAssets.Sun3 == CalRemixAsset.sunOG)
                {
                    if (DownedBossSystem.downedDoG)
                    {
                        TextureAssets.Sun3 = CalRemixAsset.sunCreepy;
                    }
                    else
                    {
                        TextureAssets.Sun3 = CalRemixAsset.sunOG;
                    }
                }
            }
            if (oxydayTime > 0 && !NPC.AnyNPCs(NPCType<Oxygen>()))
            {
                if (RemixDowned.downedOxygen)
                {
                    TextureAssets.Sun = CalRemixAsset.sunOxy2;
                }
                else
                {
                    TextureAssets.Sun = CalRemixAsset.sunOxy;
                }
            }
            else
            {
                TextureAssets.Sun = CalRemixAsset.sunReal;
            }
            if (Main._shouldUseWindyDayMusic)
            {
                if (NPC.downedBoss3 && Main.time == 1 && oxydayTime <= 0 && Main.rand.NextBool(4))
                {
                    int oxTime = Main.rand.Next(CalamityUtils.SecondsToFrames(60 * 12), CalamityUtils.SecondsToFrames(60 * 16));
                    if (Main.netMode != NetmodeID.Server)
                    {
                        oxydayTime = oxTime;
                    }
                    else
                    {
                        ModPacket packet = CalRemix.instance.GetPacket();
                        packet.Write((byte)RemixMessageType.OxydayTime);
                        packet.Write(oxTime);
                        packet.Send();
                    }
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.GaleforceBegin", Color.SkyBlue);
                }
            }
            if (oxydayTime > 0)
            {
                if (Main.LocalPlayer.position.Y < Main.worldSurface * 16)
                    Main.LocalPlayer.Calamity().monolithDevourerBShader = 66;
                Main.windSpeedTarget = 2f;
                oxydayTime--;
                // roughly once per 6 minutes
                if (Main.rand.NextBool(22222))
                {
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.BiomassMigration", Color.DeepSkyBlue);
                    int amt = 22;
                    for (int i = 0; i < amt; i++)
                    {
                        float posX = Main.LocalPlayer.position.X + Main.rand.Next(1000, 2000) * Main.LocalPlayer.direction;
                        float posY = Main.LocalPlayer.position.Y - Main.rand.Next(-200, 600);
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)posX, (int)posY, NPCType<FloatingBiomass>());
                    }
                }
                if (Main.time == 1 && !Main.dayTime && Main.rand.NextBool(3))
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        oxydayTime = 0;
                    }
                    else
                    {
                        ModPacket packet = CalRemix.instance.GetPacket();
                        packet.Write((byte)RemixMessageType.OxydayTime);
                        packet.Write(0);
                        packet.Send();
                    }
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.GaleforceEnd", Color.LightBlue);
                }
            }
            NPC.savedWizard = false;
            if (trueStory < maxStoryTime)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    trueStory++;
                }
                else
                {
                    int td = trueStory + 1;
                    ModPacket packet = CalRemix.instance.GetPacket();
                    packet.Write((byte)RemixMessageType.TrueStory);
                    packet.Write(td);
                    packet.Send();
                }
                if (trueStory >= maxStoryTime)
                {
                    UpdateWorldBool();
                }
            }
            ExcavatorSummon();
        }

        public static void ExcavatorSummon()
        {
            foreach (Player p in Main.ActivePlayers)
            {
                CalRemixPlayer crp = p.GetModPlayer<CalRemixPlayer>();
                if (p.chest == -1 && crp.RecentChest >= 0 && Main.chest[crp.RecentChest] != null)
                {
                    int i = Main.chest[crp.RecentChest].x;
                    int j = Main.chest[crp.RecentChest].y;
                    int chestenum = Chest.FindChest(i, j);
                    if (chestenum >= 0)
                    {
                        Chest cheste = Main.chest[crp.RecentChest];
                        if (Main.tile[cheste.x, cheste.y].TileType == TileID.Containers && (Main.tile[i, j].TileFrameX == 432 || Main.tile[i, j].TileFrameX == 450))
                        {
                            for (int slot = 0; slot < Chest.maxItems; slot++)
                            {
                                if (!NPC.AnyNPCs(NPCType<WulfwyrmHead>()) && cheste.item[slot].type == ItemType<CalamityMod.Items.Materials.EnergyCore>() && cheste.item[slot].stack == 1)
                                {
                                    // is the rest of the chest empty
                                    int ok = 0;
                                    for (int q = 0; q < Chest.maxItems; q++) ok += cheste.item[q].stack;
                                    if (ok == 1)
                                    {
                                        cheste.item[slot] = new Item();

                                        if (Main.netMode == NetmodeID.MultiplayerClient)
                                            NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, chestenum, slot, cheste.item[slot].stack, cheste.item[slot].prefix, cheste.item[slot].type);

                                        SoundEngine.PlaySound(SoundID.Roar, new Vector2(i * 16, j * 16));

                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                        {
                                            int npc = NPC.NewNPC(p.GetSource_FromThis(), i * 16, j * 16 + 1200, NPCType<WulfwyrmHead>());
                                            Main.npc[npc].timeLeft *= 20;
                                            CalamityUtils.BossAwakenMessage(npc);
                                            Main.npc[npc].velocity.Y = -20;
                                            if (Main.netMode == NetmodeID.Server)
                                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                crp.RecentChest = p.chest;
            }
        }

        public override void PostUpdateWorld()
        {
            if (meldCountdown > 0)
            {
                meldCountdown--;
            }
            if (aspids)
            {
                if (CalRemixNPC.aspidCount >= 20 && !DownedBossSystem.downedCryogen)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.SpawnOnPlayer(Main.myPlayer, NPCType<Cryogen>());
                    }
                    CalRemixNPC.aspidCount = 0;
                }
            }
            if (CalamityWorld.spawnedCirrus)
            {
                CalamityWorld.spawnedCirrus = false;
            }
            if (!guideHasExisted)
            if (NPC.AnyNPCs(NPCID.Guide)) guideHasExisted = true;
            if (shrinetoggle)
            {
                if (ShrineTimer == 0)
                {
                    ThreadPool.QueueUserWorkItem(_ => HallowShrine.GenerateHallowShrine(), this);
                    ThreadPool.QueueUserWorkItem(_ => AstralShrine.GenerateAstralShrine(), this);

                    Color messageColor = Color.Magenta;
                    CalamityUtils.DisplayLocalizedText("Shrines appear within the newly spread infections!", messageColor);
                    if (CalRemixAddon.CalVal != null && astralBlight)
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
            if (meldGunk)
            {
                if (!generatedStrain && Main.hardMode)
                {
                    MeldStrain.GenerateCavernStrain();
                    generatedStrain = true;
                    UpdateWorldBool();
                }
            }
            if (!generatedHydrogen)
            {
                Rectangle sus = FindCentralGeode();
                int hydrogenRadius = 10;
                int borderAmt = 3;
                Vector2 center = new Vector2(sus.Center.X, sus.Y + sus.Height / 3);
                for (int i = -hydrogenRadius; i < hydrogenRadius; i++)
                {
                    for (int j = -hydrogenRadius; j < hydrogenRadius; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval((int)center.X + i, (int)center.Y + j);
                        Vector2 pos = new Vector2(center.X + i, center.Y + j);
                        if (pos.Distance(center) < hydrogenRadius - borderAmt)
                        {
                            WorldGen.KillTile((int)center.X + i, (int)center.Y + j, noItem: true);
                        }
                        else if (pos.Distance(center) < hydrogenRadius)
                        {
                            if (t.HasTile && !SunkenSeaTiles.Contains(t.TileType))
                                continue;
                            WorldGen.KillTile((int)center.X + i, (int)center.Y + j, noItem: true);
                            WorldGen.PlaceTile((int)center.X + i, (int)center.Y + j, TileType<RustedPipes>(), true);
                        }
                    }
                }
                hydrogenLocation = center * 16;
                generatedHydrogen = true;
                hydrogenBomb = true;
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
            if (CalRemixAddon.CalVal != null && (DateTime.Now.Day == 1 && DateTime.Now.Month == 4 || DateTime.Now.Month == 4 && DateTime.Now.Day <= 7 && Main.zenithWorld))
            {
                ScreenHelperManager.screenHelpersEnabled = false;
            }
            if (SubworldSystem.IsActive<FannySubworld>())
            {
                if (Main.LocalPlayer.position.X > 16 * (Main.maxTilesX - 130))
                {
                    Main.LocalPlayer.position.X = (Main.spawnTileX + 60) * 16;
                }
            }
            // Roach Mayhem!!!
            // If the date is Black Friday (well for 2024 at least), start incrementing the timer if it isn't at -1
            if (DateTime.Now.Month == 11 && DateTime.Now.Day == 29 && RoachCountdown >= 0 && roachDuration > -2)
            {
                RoachCountdown++;
            }
            // After 5 minutes, set the timer to -1 and start Roach Mayhem
            if (RoachCountdown > CalamityUtils.SecondsToFrames(300))
            {
                UnleashRoaches();
            }
            // Spawn three explosions
            int expDelay = 40;
            int startFire = ROACHDURATIONSECONDS - 6;
            bool firstExp = roachDuration == CalamityUtils.SecondsToFrames(startFire);
            bool secondExp = roachDuration == CalamityUtils.SecondsToFrames(startFire) - expDelay;
            bool thirdExp = roachDuration == CalamityUtils.SecondsToFrames(startFire) - expDelay * 2;
            if (firstExp || secondExp || thirdExp)
            {
                Vector2 pos = firstExp ? new Vector2(Main.screenWidth * 0.05f, Main.screenHeight) : secondExp ? new Vector2(Main.screenWidth * 0.35f, Main.screenHeight) : new Vector2(Main.screenWidth * 0.7f, Main.screenHeight);
                RoachScene.explosions.Add(new RealisticExplosion(pos));
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/EpicExplosion"));
            }
            // Regularly play alarms while decrementing the event duration
            // Do not decrement if the countdown hasn't finished (marked by it being -1)
            if (roachDuration > -2 && RoachCountdown <= 0)
            {
                if (roachDuration > 0 && Main.LocalPlayer.miscCounter % 90 == 0)
                {
                    SoundEngine.PlaySound(CalamityMod.NPCs.ExoMechs.Ares.AresBody.EnragedSound);
                }
                roachDuration--;
            }
            // When the event is 1 frame from being over, clear the explosion list
            if (roachDuration == -1)
            {
                if (RoachScene.explosions.Count > 0)
                {
                    RoachScene.explosions.Clear();
                }
                UpdateWorldBool();
            }
        }

        public static void UnleashRoaches()
        {
            RoachCountdown = -1;
            SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/BlackFriday"));
            roachDuration = CalamityUtils.SecondsToFrames(ROACHDURATIONSECONDS);
            UpdateWorldBool();
        }

        public static void AddLootDynamically(int npcType, bool npc = false)
        {
            if (npc)
            {
                if (npcType == NPCType<ReaperShark>())
                {
                    var postPolter = new LeadingConditionRule(DropHelper.PostPolter());
                    postPolter.Add(ItemType<ReaperTooth>(), 1, 3, 4);
                    Main.ItemDropsDB.RegisterToNPC(npcType, postPolter);
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
                    Main.ItemDropsDB.RegisterToItem(npcType, postPolter);
                }
                if (npcType == ItemID.JungleFishingCrate)
                {
                    var postDuke = new LeadingConditionRule(DropHelper.PostProv());
                    postDuke.Add(ItemType<CalamityMod.Items.Placeables.Ores.UelibloomOre>(), 5, 16, 28);
                    postDuke.Add(ItemType<CalamityMod.Items.Materials.UelibloomBar>(), new Fraction(15, 100), 4, 7);
                    Main.ItemDropsDB.RegisterToItem(npcType, postDuke);
                    Main.ItemDropsDB.RegisterToItem(ItemID.JungleFishingCrateHard, postDuke);
                }
            }
        }
        public static void RemoveLoot(int bagType, int itemToRemove, bool npc = false)
        {
            List<IItemDropRule> JungleCrateDrops = npc ? Main.ItemDropsDB.GetRulesForNPCID(bagType) : Main.ItemDropsDB.GetRulesForItemID(bagType);
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
            vernalTiles = 0;
            PlagueTiles = 0;
            PlagueDesertTiles = 0;
            MeldTiles = 0;
            baronTiles = 0;
            strongholdTiles = 0;
            elumplateTiles = 0;
            onyxplateTiles = 0;
            havocplateTiles = 0;
            cinderplateTiles = 0;
            plagueplateTiles = 0;
            aeroplateTiles = 0;
            navyplateTiles = 0;
            bloodplateTiles = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            // Life Ore tiles
            lifeTiles = tileCounts[TileType<LifeOreTile>()];
            asbestosTiles = tileCounts[TileType<AsbestosPlaced>()];
            vernalTiles = tileCounts[TileType<CalamityMod.Tiles.VernalSoil>()];
            PlagueTiles = tileCounts[TileType<PlaguedGrass>()] +
            tileCounts[TileType<PlaguedMud>()] +
            tileCounts[TileType<PlaguedStone>()] +
            tileCounts[TileType<PlaguedClay>()] +
            tileCounts[TileType<OvergrownPlaguedStone>()] +
            tileCounts[TileType<PlaguedSilt>()] +
            tileCounts[TileType<Sporezol>()];
            PlagueDesertTiles = tileCounts[TileType<PlaguedSand>()];
            MeldTiles = tileCounts[TileType<MeldGunkPlaced>()];
            strongholdTiles = tileCounts[TileType<FrostflakeBrickPlaced>()] + tileCounts[TileType<CryonicBrick>()];
            baronTiles = tileCounts[TileType<BrinerackPlaced>()] + tileCounts[TileType<TanzaniteGlassPlaced>()] + tileCounts[TileType<BaronBrinePlaced>()] + tileCounts[TileType<BaronsandPlaced>()] + tileCounts[TileType<BaronBrinePlaced>()];
            Main.SceneMetrics.JungleTileCount += PlagueTiles;
            Main.SceneMetrics.SandTileCount += PlagueDesertTiles + tileCounts[TileType<TorrefiedTephraPlaced>()];
            CalamityMod.Systems.BiomeTileCounterSystem.SunkenSeaTiles += tileCounts[TileType<NavystoneSafe>()] + tileCounts[TileType<SeaPrismSafe>()] + tileCounts[TileType<EutrophicSandSafe>()] + tileCounts[TileType<HardenedEutrophicSandSafe>()];
            aeroplateTiles = tileCounts[TileType<AeroplateTile>()];
            bloodplateTiles = tileCounts[TileType<BloodplateTile>()];
            onyxplateTiles = tileCounts[TileType<Onyxplate>()];
            havocplateTiles = tileCounts[TileType<Havocplate>()];
            cinderplateTiles = tileCounts[TileType<Cinderplate>()];
            elumplateTiles = tileCounts[TileType<Elumplate>()];
            navyplateTiles = tileCounts[TileType<Navyplate>()];
            plagueplateTiles = tileCounts[TileType<PlagueContainmentCells>()];
            Main.SceneMetrics.HolyTileCount += tileCounts[TileType<TorrefiedTephraPlaced>()];
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int GraniteIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite"));
            int SnowIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Generate Ice Biome")); 
            tasks.Insert(SnowIndex + 1, new PassLegacy("Frozen Stronghold", (progress, config) => {
                progress.Message = "Building a Wintery Castle";
                FrozenStronghold.GenerateFrozenStronghold();
            }));
            tasks.Insert(GraniteIndex, new PassLegacy("Asbestos", (progress, passConfig) =>
            {
                progress.Message = "Spreading the True Plague";
                int biomeAmount = 10;
                double xRange = (Main.maxTilesX - 200) / (double)biomeAmount;
                List<Point> placementPoints = new List<Point>(biomeAmount);
                int tries = 0;
                int currentBiomeAmount = 0;
                while (currentBiomeAmount < biomeAmount)
                {
                    double biomeRaito = currentBiomeAmount / (double)biomeAmount;
                    progress.Set(biomeRaito);
                    Point placementPoint = WorldGen.RandomRectanglePoint((int)(biomeRaito * (Main.maxTilesX - 200)) + 100, (int)GenVars.rockLayer + 20, (int)xRange, Main.maxTilesY - ((int)GenVars.rockLayer + 40) - 200);
                    if (WorldGen.remixWorldGen)
                    {
                        placementPoint = WorldGen.RandomRectanglePoint((int)(biomeRaito * (Main.maxTilesX - 200)) + 100, (int)GenVars.worldSurface + 100, (int)xRange, (int)GenVars.rockLayer - (int)GenVars.worldSurface - 100);
                    }
                    while (placementPoint.X > Main.maxTilesX * 0.45 && placementPoint.X < Main.maxTilesX * 0.55)
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
            int FinalIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Roxcalibur"));
            if (FinalIndex != -1)
            {
                if (!stratusDungeonDisabled)
                {
                    tasks.Insert(FinalIndex, new PassLegacy("Dungeon Retheme", (progress, config) =>
                    {
                        progress.Message = "Remodeling the Dungeon";
                        StratusDungeon.ReplaceDungeon();
                        StratusDungeon.AddOriginalDungeonHoles();
                        CalamityUtils.SpawnOre(TileType<ArsenicOrePlaced>(), 15E-01, 0.4f, 1f, 3, 8, new int[3] { TileID.BlueDungeonBrick, TileID.PinkDungeonBrick, TileID.GreenDungeonBrick });
                    }));
                }
                tasks.Insert(FinalIndex, new PassLegacy("Arsenic", (progress, config) =>
                {
                    progress.Message = "Arsenic Ore";
                    CalamityUtils.SpawnOre(TileType<ArsenicOrePlaced>(), 15E-01, 0.4f, 1f, 3, 8, new int[3] { TileID.BlueDungeonBrick, TileID.PinkDungeonBrick, TileID.GreenDungeonBrick });
                }));
                tasks.Insert(FinalIndex, new PassLegacy("Ion Altar", (progress, config) => { IonAltar.GenerateIonAltar(); }));
                tasks.Insert(FinalIndex, new PassLegacy("Engineering a bronze device", (progress, config) => { AncientConsoleGen.GenerateConsole(); }));
                tasks.Insert(FinalIndex, new PassLegacy("Origen Workshop", (progress, config) => { OrigenWorkshop.GenerateOrigenWorkshop(); }));
                tasks.Insert(FinalIndex, new PassLegacy("Building a Bomb", (progress, config) =>
                {
                    Rectangle sus = FindCentralGeode();
                    int hydrogenRadius = 10;
                    int borderAmt = 3;
                    Vector2 center = new Vector2(sus.Center.X, sus.Y + sus.Height / 3);
                    for (int i = -hydrogenRadius; i < hydrogenRadius; i++)
                    {
                        for (int j = -hydrogenRadius; j < hydrogenRadius; j++)
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval((int)center.X + i, (int)center.Y + j);
                            Vector2 pos = new Vector2(center.X + i, center.Y + j);
                            if (pos.Distance(center) < hydrogenRadius - borderAmt)
                            {
                                WorldGen.KillTile((int)center.X + i, (int)center.Y + j, noItem: true);
                            }
                            else if (pos.Distance(center) < hydrogenRadius)
                            {
                                if (t.HasTile && !SunkenSeaTiles.Contains(t.TileType))
                                    continue;
                                WorldGen.KillTile((int)center.X + i, (int)center.Y + j, noItem: true);
                                WorldGen.PlaceTile((int)center.X + i, (int)center.Y + j, TileType<RustedPipes>(), true);
                            }
                        }
                    }
                    hydrogenLocation = center * 16;
                    generatedHydrogen = true;
                }));
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
                    if (Main.tile[chest.x, chest.y].TileType == TileType<StratusChest>())
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

        public static void DestroyTheSunkenSea(Vector2 center, int rad)
        {
            for (int i = (int)MathHelper.Clamp((int)center.X - rad, 100, Main.maxTilesX - 100); i < (int)MathHelper.Clamp((int)center.X + rad, 100, Main.maxTilesX - 100); i++)
            {
                for (int j = (int)MathHelper.Clamp((int)center.Y - rad, 100, Main.maxTilesY - 100); j < (int)MathHelper.Clamp((int)center.Y + rad, 100, Main.maxTilesY - 100); j++)
                {
                    if (new Vector2(i, j).Distance(center) > rad)
                        continue;
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.LiquidAmount > 0)
                    {
                        t.LiquidAmount = 0;
                    }
                    if (t.WallType == WallType<NavystoneWall>() || t.WallType == WallType<EutrophicSandWall>())
                    {
                        t.WallType = 0;
                    }
                    if (!t.HasTile)
                    {
                        continue;
                    }
                    if (SunkenSeaTiles.Contains(t.TileType))
                    {
                        t.Get<TileWallWireStateData>().HasTile = false;
                    }
                }
            }
        }

        public static Rectangle FindCentralGeode()
        {
            int sunkenX = 0;
            int sunkenY = 0;
            int sunkenLastX = 0;
            int sunkenLastY = 0;
            for (int i = 100; i < Main.maxTilesX - 100; i++)
            {
                for (int j = 100; j < Main.maxTilesY - 100; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == TileType<Navystone>())
                    {
                        if (sunkenX == 0)
                            sunkenX = i;
                        sunkenLastX = i;
                        break;
                    }
                }
            }
            for (int i = 100; i < Main.maxTilesY - 100; i++)
            {
                for (int j = 100; j < Main.maxTilesX - 100; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(j, i);
                    if (t.TileType == TileType<Navystone>())
                    {
                        if (sunkenY == 0)
                            sunkenY = i;
                        sunkenLastY = i;
                        break;
                    }
                }
            }
            int sunkenWidth = sunkenLastX - sunkenX;
            int sunkenHeight = sunkenLastY - sunkenY;
            hydrogenLocation = new Vector2(sunkenX + sunkenWidth / 2, sunkenY + sunkenHeight / 3) * 16;
            return new Rectangle(sunkenX, sunkenY, sunkenWidth, sunkenHeight);
        }
    }
}