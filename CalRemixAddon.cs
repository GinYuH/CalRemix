using static Terraria.ModLoader.ModContent;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.NPCs.Bosses.Wulfwyrm;
using CalRemix.Content.NPCs.Bosses.Poly;
using CalRemix.Content.NPCs.Bosses.BossScule;
using CalRemix.Content.NPCs.Bosses.Acideye;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.NPCs.TownNPCs;
using CalRemix.Content.NPCs.Bosses.Phytogen;
using CalRemix.Content.NPCs.Bosses.Hydrogen;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.NPCs.Bosses.Ionogen;
using CalRemix.Content.NPCs.Bosses.Oxygen;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using CalRemix.Content.NPCs.Bosses.Pathogen;
using CalRemix.Content.NPCs.Bosses.Origen;
using CalRemix.Content.NPCs.Bosses.Aurelionium;
using CalRemix.Core.World;
using CalRemix.Content.Items.SummonItems;
using CalRemix.Content.Items.Lore;
using Terraria.Audio;
using CalamityMod.Sounds;
using CalamityMod.NPCs.Perforator;
using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Bosses.Noxus;
using System.Reflection;
using CalRemix.Content.NPCs.Eclipse;
using Terraria.ModLoader.Core;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;
using CalamityMod.NPCs.Cryogen;

namespace CalRemix
{
    public class CalRemixAddon : ModSystem
    {
        internal static Mod CalVal;
        internal static Mod Catalyst;
        internal static Mod Infernum;
        internal static Mod Wrath;
        internal static Mod Fables;

        internal static Mod BossChecklist;
        internal static Mod MusicDisplay;
        internal static Mod Wikithis;
        internal static Mod Census;
        internal static Mod ExampleMod;

        internal static Mod Remnants;
        internal static Mod Spirit;
        internal static Mod Thorium;

        internal static Type calvalFanny = null;
        internal static Type calvalFannyBox = null;
        internal static Type WrathAvatarWorldSystem = null;
        internal static Type WrathAvatarOfEmptiness = null;
        internal static Type WrathNamelessDeity = null;
        internal static Type FargoSoulsWorldSaveSystem = null;

        internal static PropertyInfo WrathInAvatarWorld = null;
        internal static PropertyInfo FargoSoulsAngryMutant = null;

        public static readonly List<string> Names = new List<string>()
        {
            "ApothTestMod",
            "Bloopsitems",
            "CalamityHunt",
            "CalamityLootSwap",
            "CalamityMod",
            "CalamityModMusic",
            "CalRemix",
            "CalValEX",
            "CJMOD",
            "Clamity",
            "CatalystMod",
            "InfernumMode",
            "NoxusBoss",
            "UnCalamityModMusic",
            "CalamityFables"
        };
        public static List<ModItem> Items = new List<ModItem>();
        public override void Load()
        {
            ModLoader.TryGetMod("CalValEX", out CalVal);
            ModLoader.TryGetMod("CatalystMod", out Catalyst);
            ModLoader.TryGetMod("InfernumMode", out Infernum);
            ModLoader.TryGetMod("NoxusBoss", out Wrath);
            ModLoader.TryGetMod("CalamityFables", out Fables);

            ModLoader.TryGetMod("BossChecklist", out BossChecklist);
            ModLoader.TryGetMod("Census", out Census);
            ModLoader.TryGetMod("MusicDisplay", out MusicDisplay);
            ModLoader.TryGetMod("Wikithis", out Wikithis);
            ModLoader.TryGetMod("ExampleMod", out ExampleMod);

            ModLoader.TryGetMod("Remnants", out Remnants);
            ModLoader.TryGetMod("SpiritMod", out Spirit);
            ModLoader.TryGetMod("ThoriumMod", out Thorium);
        }
        public override void Unload()
        {
            CalVal = null;
            Catalyst = null;
            Infernum = null;
            Wrath = null;
            Fables = null;

            BossChecklist = null;
            Census = null;
            MusicDisplay = null;
            Wikithis = null;
            ExampleMod = null;

            Remnants = null;
            Spirit = null;
            Thorium = null;
        }
        public override void PostSetupContent()
        {
            AddBossChecklistEntries();
            // AddMusicDisplayEntries();
            if (Wikithis != null && !Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                Wikithis.Call("AddModURL", Mod, "https://terrariamods.wiki.gg/wiki/Calamity_Community_Remix/{}");
                Wikithis.Call("AddWikiTexture", Mod, Request<Texture2D>("CalRemix/icon_small"));
            }
            AddInfernumCards();
            ColoredDamageTypesSupport();
            LoadModTypes();
        }

        internal void AddBossChecklistEntries()
        {
            if (BossChecklist is null)
                return;
            Mod bc = BossChecklist;
            // Bosses
            Action<SpriteBatch, Rectangle, Color> calamityPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/BossScule/TheCalamity_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            };
            bc.Call("LogBoss", Mod, "TheCalamity", 0.0000000000022f, () => RemixDowned.downedCalamity, NPCType<TheCalamity>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<Slumbering>(),
                ["customPortrait"] = calamityPortrait
            });
            Action<SpriteBatch, Rectangle, Color> wfportrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Wulfwyrm/WulfwyrmBossChecklist").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            };
            bc.Call("LogBoss", Mod, "WulfwyrmHead", 0.22f, () => RemixDowned.downedExcavator, NPCType<WulfwyrmHead>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<EnergyCore>(),
                ["customPortrait"] = wfportrait
            });
            bc.Call("LogBoss", Mod, "Origen", 0.3f, () => RemixDowned.downedOrigen, NPCType<OrigenCore>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<OrigenDoor>(),
            });
            bc.Call("LogBoss", Mod, "AcidEye", 2.1f, () => RemixDowned.downedAcidsighter, NPCType<AcidEye>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<PoisonedSclera>(),
            });
            bc.Call("LogBoss", Mod, "Carcinogen", 5.241f, () => RemixDowned.downedCarcinogen, NPCType<Carcinogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemID.WoodWall,
            });
            // fake boss to trick people into thinking that the gilded isle is a boss arena
            bc.Call("LogBoss", Mod, "Aurelionium", 7.333f, () => RemixDowned.downedAurelionium, NPCType<Aurelionium>(), new Dictionary<string, object>());
            Action<SpriteBatch, Rectangle, Color> plportrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Poly/Polyphemalus").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            /*
            bc.Call("LogBoss", Mod, "Derellect", 12.5f, () => RemixDowned.downedDerellect, NPCType<DerellectBoss>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>(),
            });
            */
            List<int> poly = new() { NPCType<Astigmageddon>(), NPCType<Exotrexia>(), NPCType<Conjunctivirus>(), NPCType<Cataractacomb>() };
            bc.Call("LogBoss", Mod, "Polyphemalus", 12.7f, () => RemixDowned.downedPolyphemalus, poly, new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<FusedEye>(),
                ["customPortrait"] = plportrait
            });
            bc.Call("LogBoss", Mod, "Phytogen", 13.25f, () => RemixDowned.downedPhytogen, NPCType<Phytogen>(), new Dictionary<string, object>());
            bc.Call("LogBoss", Mod, "Hydrogen", 14.15f, () => RemixDowned.downedHydrogen, NPCType<Hydrogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemID.Grenade,
            });
            bc.Call("LogBoss", Mod, "Oxygen", 12.805f, () => RemixDowned.downedOxygen, NPCType<Oxygen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemID.GolfBall,
            });
            bc.Call("LogBoss", Mod, "Ionogen", 11.6f, () => RemixDowned.downedIonogen, NPCType<Ionogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<IonCube>(),
            });
            bc.Call("LogBoss", Mod, "Pathogen", 16.75f, () => RemixDowned.downedPathogen, NPCType<Pathogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>(),
            });
            List<int> draedonBosses = new List<int>() { NPCType<SkeletronOmega>(), NPCType<Godraycaster>(), NPCType<ObliteratorHead>() };
            Action<SpriteBatch, Rectangle, Color> draePortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/Dreadon_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 1f, 0, 0);
            };
            bc.Call("LogBoss", Mod, "Dreadon", 17.6, () => RemixDowned.downedDraedon, draedonBosses, new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<TurnipSprout>(),
                ["customPortrait"] = draePortrait,
                ["displayName"] = CalRemixHelper.LocalText("NPCs.DreadonFriendly.BossChecklistIntegration.EntryName"),
                ["spawnInfo"] = CalRemixHelper.LocalText("NPCs.DreadonFriendly.BossChecklistIntegration.SpawnInfo"),
                ["despawnMessage"] = CalRemixHelper.LocalText("NPCs.DreadonFriendly.BossChecklistIntegration.DespawnMessage"),
            });
            Action<SpriteBatch, Rectangle, Color> voidPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/VoidBoss_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 1f, 0, 0);
            };
            bc.Call("LogBoss", Mod, "VoidBoss", 17.7, () => RemixDowned.downedVoid, NPCType<VoidBoss>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<NullBeacon>(),
                ["customPortrait"] = voidPortrait
            });
            Action<SpriteBatch, Rectangle, Color> gasPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/MonorianGastropodAscended_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 1f, 0, 0);
            };
            bc.Call("LogBoss", Mod, "Disilphia", 17.8, () => RemixDowned.downedDisil, NPCType<Disilphia>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<DoctorsRemote>()
            });
            bc.Call("LogBoss", Mod, "AscendedGastropod", 17.9, () => RemixDowned.downedGastropod, NPCType<MonorianGastropodAscended>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<Gastrosequence>(),
                ["customPortrait"] = gasPortrait
            });
            bc.Call("LogBoss", Mod, "MonorianWarrior", 17.91, () => RemixDowned.downedOneguy, NPCType<MonorianWarrior>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<GastropodEye>()
            });
            Action<SpriteBatch, Rectangle, Color> lvPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/Livyatan_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2) * 0.3f, rect.Center.Y - (texture.Height / 2) * 0.3f);
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 0.3f, 0, 0);
            };
            bc.Call("LogBoss", Mod, "Livyatan", 19.1f, () => RemixDowned.downedLivyatan, NPCType<Livyatan>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<SubworldDoor>(),
                ["customPortrait"] = lvPortrait
            });
            bc.Call("LogBoss", Mod, "Pyrogen", 19.6f, () => RemixDowned.downedPyrogen, NPCType<Pyrogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<MoltenMatter>(),
            });
            bc.Call("LogBoss", Mod, "Hypnos", 22.5f, () => RemixDowned.downedHypnos, NPCType<Hypnos>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>()
            });
            Action<SpriteBatch, Rectangle, Color> noxPort = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/NoxusBossChecklist").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2 / 2), rect.Center.Y - (texture.Height / 2 / 2));
                sb.Draw(texture, centered, null, Color.White, 0, Vector2.Zero, 0.5f, 0, 0);
            };
            bc.Call("LogBoss", Mod, "EntropicGodNoxus", 25f, () => RemixDowned.downedNoxus, NPCType<EntropicGod>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<Genesis>(),
                ["customPortrait"] = noxPort
            });
            // Minibosses
            Action<SpriteBatch, Rectangle, Color> clPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/Clamitas_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            bc.Call("LogMiniBoss", Mod, "Clamitas", 6.8f, () => RemixDowned.downedClamitas, NPCType<Clamitas>(), new Dictionary<string, object>()
            {
                ["customPortrait"] = clPortrait
            });
            Action<SpriteBatch, Rectangle, Color> cdPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/CyberDraedon").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, new Color(0, 255, 255, 125));
            };
            bc.Call("LogMiniBoss", Mod, "CyberDraedon", 3.99999f, () => RemixDowned.downedCyberDraedon, NPCType<CyberDraedon>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>(),
                ["customPortrait"] = cdPortrait
            });
            bc.Call("LogMiniBoss", Mod, "KingMinnowsPrime", 18.1f, () => RemixDowned.downedKingMinnowsPrime, NPCType<KingMinnowsPrime>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "LifeSlime", 16.7f, () => RemixDowned.downedLifeSlime, NPCType<LifeSlime>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "OnyxKinsman", 7.5f, () => RemixDowned.downedOnyxKinsman, NPCType<OnyxKinsman>(), new Dictionary<string, object>());
            Action<SpriteBatch, Rectangle, Color> pePortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/PlagueEmperor").Value;
                Texture2D texture2 = Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/PlagueEmperorEyes").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
                sb.Draw(texture2, centered, color);
            };
            bc.Call("LogMiniBoss", Mod, "PlagueEmperor", 21.5f, () => RemixDowned.downedPlagueEmperor, NPCType<PlagueEmperor>(), new Dictionary<string, object>()
            {
                ["customPortrait"] = pePortrait
            });
            bc.Call("LogMiniBoss", Mod, "YggdrasilEnt", 18.2f, () => RemixDowned.downedYggdrasilEnt, NPCType<YggdrasilEnt>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "Dendritiator", 16.73f, () => RemixDowned.downedDend, NPCType<Dendritiator>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "MaserPhage", 16.74f, () => RemixDowned.downedMaser, NPCType<MaserPhage>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "CrimsonKaiju", 20.5f, () => RemixDowned.downedRed, NPCType<CrimsonKaiju>(), new Dictionary<string, object>());
            // Events
            bc.Call("LogEvent", Mod, "PandemicPanic", 16.71f, () => RemixDowned.downedPathogen, new List<int> { NPCType<Malignant>(), NPCType<Ecolium>(), NPCType<Basilius>(), NPCType<Tobasaia>(), NPCType<MaserPhage>(), NPCType<WhiteBloodCell>(), NPCType<Platelet>(), NPCType<RedBloodCell>(), NPCType<Eosinine>(), NPCType<Dendritiator>() }, new Dictionary<string, object>());
            bc.Call("LogEvent", Mod, "GaleforceDay", 11.749f, () => RemixDowned.downedGale, new List<int> { NPCID.Dandelion, NPCType<FloatingBiomass>() }, new Dictionary<string, object>());
        }
        internal static void AddCensusEntries()
        {
            if (Census is null)
                return;
            Census.Call("TownNPCCondition", NPCType<ZER0>(), "Moves in during Godseeker mode");
            Census.Call("TownNPCCondition", NPCType<YEENA>(), "The current month is December, January, or February or Astrum Deus has been defeated in a Snow biome");

            Census.Call("TownNPCCondition", NPCType<Ogslime>(), "Kill a Wandering Eye while wearing Titan Heart armor");

            Census.Call("TownNPCCondition", NPCType<WALTER>(), "Defeat Pathogen");
            Census.Call("TownNPCCondition", NPCType<IRON>(), "Defeat Ionogen");
            Census.Call("TownNPCCondition", NPCType<SIIVA>(), "Defeat Phytogen");
            Census.Call("TownNPCCondition", NPCType<UNCANNY>(), "Defeat Carcinogen");
            Census.Call("TownNPCCondition", NPCType<KABLOOEY>(), "Defeat Hydrogen");
            Census.Call("TownNPCCondition", NPCType<BALLER>(), "Defeat Oxygen");
        }
        internal void AddInfernumCards()
        {
            if (Infernum is null)
                return;
            MakeCard(NPCType<Polyphemalus>(), (horz, anim) => Color.Lerp(Color.White, Color.Pink, horz), "Polyphemalus", SoundID.Roar, SoundID.ForceRoar);
            MakeCard(NPCType<Origen>(), (horz, anim) => Color.Cyan, "Origen", BetterSoundID.ItemIceBreak, BetterSoundID.ItemMissileFireSqueak);
            MakeCard(NPCType<Carcinogen>(), (horz, anim) => Color.Lerp(Color.White, Color.Brown, anim), "Carcinogen", BetterSoundID.ItemFireballImpact, Carcinogen.DeathSound);
            MakeCard(NPCType<Ionogen>(), (horz, anim) => Color.Lerp(Color.Blue, Color.Yellow, anim), "Ionogen", SoundID.DD2_LightningAuraZap, SoundID.Thunder);
            MakeCard(NPCType<Phytogen>(), (horz, anim) => Color.Lerp(Color.Lime, Color.Yellow, anim), "Phytogen", SoundID.Grass, SoundID.NPCDeath1);
            MakeCard(NPCType<Oxygen>(), (horz, anim) => Color.Lerp(Color.LightBlue, Color.SeaGreen, anim), "Oxygen", Oxygen.HitSound, SoundID.Shatter);
            MakeCard(NPCType<Pathogen>(), (horz, anim) => Color.Lerp(Color.Magenta, Color.Red, anim), "Pathogen", PerforatorHeadLarge.HitSound, PerforatorHeadSmall.HitSound);
            MakeCard(NPCType<SkeletronOmega>(), (horz, anim) => Color.Lerp(Color.Gold, Color.SkyBlue, anim), "SkeletronOmega", CalamityMod.Tiles.Ores.AuricOre.MineSound, AresTeslaCannon.TeslaOrbShootSound);
            MakeCard(NPCType<Godraycaster>(), (horz, anim) => Color.Lerp(Color.Gold, Color.SkyBlue, anim), "Godraycaster", CalamityMod.Tiles.Ores.AuricOre.MineSound, AresTeslaCannon.TeslaOrbShootSound);
            MakeCard(NPCType<ObliteratorHead>(), (horz, anim) => Color.Lerp(Color.Gold, Color.SkyBlue, anim), "Obliterator", CalamityMod.Tiles.Ores.AuricOre.MineSound, AresTeslaCannon.TeslaOrbShootSound);
            MakeCard(NPCType<VoidBoss>(), (horz, anim) => Color.Lerp(Color.Black, Color.Magenta, anim), "Void", BetterSoundID.ItemCast, BetterSoundID.ItemCast);
            MakeCard(NPCType<Disilphia>(), (horz, anim) => Color.Lerp(Color.White, Color.White, anim), "Disilphia", CommonCalamitySounds.ExoHitSound, CommonCalamitySounds.ELRFireSound);
            MakeCard(NPCType<MonorianGastropodAscended>(), (horz, anim) => Color.Lerp(Color.Pink, Color.Goldenrod, anim), "MonorianGastropodAscended", SoundID.NPCHit1, SoundID.NPCDeath1);
            MakeCard(NPCType<MonorianWarrior>(), (horz, anim) => Color.Lerp(Color.Red, Color.Red, anim), "MonorianWarrior", BetterSoundID.ItemAerialBane, SoundID.NPCDeath1);
            MakeCard(NPCType<MonorianSoul>(), (horz, anim) => Color.Lerp(Color.Cyan, Color.Red, anim), "MonorianSoul", BetterSoundID.ItemAerialBane, SoundID.NPCDeath1);
            MakeCard(NPCType<Pyrogen>(), (horz, anim) => Color.Lerp(Color.Magenta, Color.Red, anim), "Pyrogen", Cryogen.HitSound, Cryogen.DeathSound);
            MakeCard(() => NPC.FindFirstNPC(NPCType<Hydrogen>()) != -1 && Main.npc[NPC.FindFirstNPC(NPCType<Hydrogen>())].Calamity().newAI[2] > 0 && Main.npc[NPC.FindFirstNPC(NPCType<Hydrogen>())].Calamity().newAI[2] < 300, (horz, anim) => Color.Lerp(Color.Blue, Color.Yellow, anim), "Hydrogen", SoundID.Item14, CalamityMod.NPCs.ExoMechs.Ares.AresGaussNuke.NukeExplosionSound);
            MakeCard(NPCType<WulfwyrmHead>(), (horz, anim) => Color.Lerp(Color.LightGreen, Color.LightBlue, horz), "WulfrumExcavator", SoundID.NPCHit4, BetterSoundID.ItemThisStupidFuckingLaser);
            MakeCard(NPCType<Livyatan>(), (horz, anim) => Color.Lerp(Color.Turquoise, Color.SeaGreen, horz), "Livyatan", Livyatan.HitSound, Livyatan.RoarSound);
            MakeCard(NPCType<AcidEye>(), (horz, anim) => Color.Lerp(Color.LimeGreen, Color.Lime, anim), "Acidsighter", SoundID.Roar, SoundID.NPCDeath1);
            MakeCard(NPCType<TheCalamity>(), (horz, anim) => Color.Red, "Calamity", BetterSoundID.ItemThisStupidFuckingLaser, BetterSoundID.ItemThisStupidFuckingLaser, 360);
            MakeCard(NPCType<Hypnos>(), (horz, anim) => Main.DiscoColor, "Hypnos", CommonCalamitySounds.ExoHitSound, CommonCalamitySounds.ELRFireSound);
            MakeCard(NPCType<NoxusEgg>(), (horz, anim) => Color.Lerp(Color.Black, Color.Violet, anim), "Noxus", NoxusEgg.HitSound, NoxusEgg.GlitchSound);
        }
        internal void MakeCard(int type, Func<float, float, Color> color, string title, SoundStyle tickSound, SoundStyle endSound, int time = 300, float size = 1f)
        {
            MakeCard(()=> NPC.AnyNPCs(type), color, title, tickSound, endSound, time, size);
        }
        internal void MakeCard(Func<bool> condition, Func<float, float, Color> color, string title, SoundStyle tickSound, SoundStyle endSound, int time = 300, float size = 1f)
        {
            // Initialize the base instance for the intro card. Alternative effects may be added separately.
            Func<float, float, Color> textColorSelectionDelegate = color;
            object instance = Infernum.Call("InitializeIntroScreen", Mod.GetLocalization("InfernumIntegration." + title), time, true, condition, textColorSelectionDelegate);
            Infernum.Call("IntroScreenSetupLetterDisplayCompletionRatio", instance, new Func<int, float>(animationTimer => MathHelper.Clamp(animationTimer / (float)time * 1.36f, 0f, 1f)));

            // dnc but needed or else it errors
            Action onCompletionDelegate = () => { };
            Infernum.Call("IntroScreenSetupCompletionEffects", instance, onCompletionDelegate);

            // Letter addition sound.
            Func<SoundStyle> chooseLetterSoundDelegate = () => tickSound;
            Infernum.Call("IntroScreenSetupLetterAdditionSound", instance, chooseLetterSoundDelegate);
            
            // Main sound.
            Func<SoundStyle> chooseMainSoundDelegate = () => endSound;
            Func<int, int, float, float, bool> why = (_, _2, _3, _4) => true;
            Infernum.Call("IntroScreenSetupMainSound", instance, why, chooseMainSoundDelegate);

            // Text scale.
            Infernum.Call("IntroScreenSetupTextScale", instance, size);

            // Register the intro card.
            Infernum.Call("RegisterIntroScreen", instance);
        }
        /*
        internal void AddMusicDisplayEntries()
        {
            if (MusicDisplay is null)
                return;
            AddMusic(CalRemixMusic.Exosphere, "CanAnybodyHearMe", "Jteoh");

            AddMusic(CalRemixMusic.Generator, "Artistic Reinforcement", "Jteoh");

            AddMusic(CalRemixMusic.AsbestosCaves, "Fibrous Whisper", "Jteoh");
            AddMusic(CalRemixMusic.PlaguedJungle, "Everlasting Dark", "HeartPlusUp");
            AddMusic(CalRemixMusic.BaronStrait, "The End Zone", "Jteoh");
            AddMusic(CalRemixMusic.VernalPass, "Vajrabhairava's Rest", "Jteoh");

            AddMusic(CalRemixMusic.AcidRainTier2, "Tropic of Cancer", "Jteoh");
            AddMusic(CalRemixMusic.PandemicPanic, "Pandemic Panic", "Jteoh");

            AddMusic(CalRemixMusic.CryoSlime, "Antarctic Reinsertion", "Jteoh");
            AddMusic(CalRemixMusic.LaRuga, "La Ruga's Ambience", "Sploopo");

            AddMusic(CalRemixMusic.TheCalamity, "Stained, Smeared Calamity", "Jteoh");
            AddMusic(CalRemixMusic.WulfrumExcavator, "Scourge of the Scrapyard", "Sploopo");
            AddMusic(CalRemixMusic.Origen, "Antarctic Reinforcement (Structured Mix)", "DM Dokuro");
            AddMusic(CalRemixMusic.Acidsighter, "Opticatalysis", "DEMON GIRLFRIEND");
            AddMusic(CalRemixMusic.Carcinogen, "Oncologic Reinforcement", "Jteoh");

            AddMusic(CalRemixMusic.Derellect, "Signal Interruption", "Sploopo");
            AddMusic(CalRemixMusic.Polyphemalus, "Eyes of Flame (Arranged)", "DM DOKURO");
            AddMusic(CalRemixMusic.PolyphemalusAlt, "The Eyes are Anger", "GummyLeeches");
            AddMusic(CalRemixMusic.Phytogen, "Botanic Reinforcement", "Jteoh");
            AddMusic(CalRemixMusic.Hydrogen, "Atomic Reinforcement", "Jteoh");
            AddMusic(CalRemixMusic.Oxygen, "Aerobic Reinforcement", "Jteoh");
            AddMusic(CalRemixMusic.Ionogen, "Ionic Reinforcement", "Jteoh");
            AddMusic(CalRemixMusic.Pathogen, "Microbic Reinforcement", "Jteoh");
            AddMusic(CalRemixMusic.EmpressofLightDay, "Gegenschein", "Jteoh");

            AddMusic(CalRemixMusic.Pyrogen, "Volcanic Reinforcement", "The Purified");
            AddMusic(CalRemixMusic.Generator, "The Generator", "Jteoh");
            AddMusic(CalRemixMusic.DevourerofGods, "Last Battle (Ballos Mix)", "DM DOKURO");
            AddMusic(CalRemixMusic.DevourerofGodsFinalForm, "Univesal Collapse?", "SiivaGunner");
            AddMusic(CalRemixMusic.Hypnos, "Cerebral Augmentations", "The Exiled Fellow");

            AddMusic(CalRemixMusic.ExoMechs, "Omniscius Calculus", "Jteoh");
            AddMusic(CalRemixMusic.ExoTwins, "Omicron Venata", "Jteoh");
            AddMusic(CalRemixMusic.Ares, "Delta Duella", "Jteoh");
            AddMusic(CalRemixMusic.Thanatos, "Epsilon Termina", "Jteoh");
            AddMusic(CalRemixMusic.ExoTwinsThanatos, "Omicron Termina", "Jteoh");
            AddMusic(CalRemixMusic.ExoTwinsAres, "Delta Venata", "Jteoh");
            AddMusic(CalRemixMusic.ThanatosAres, "Epsilon Duella", "Jteoh");

            AddMusic(CalRemixMusic.Menu, "This Is Where the Magic Happens", "Jteoh");
        }
        internal void AddMusic(int music, string name, string author)
        {
            MusicDisplay.Call("AddMusic", music, name, author, Mod.DisplayName);
        }
         */
        public static void ColoredDamageTypesSupport()
        {
            // i dont care about this
            //Mod coloredDamageTypes = GetInstance<CalamityMod>().coloredDamageTypes;
            //if (coloredDamageTypes is null)
            //    return;

            //coloredDamageTypes.Call("AddDamageType", StormbowDamageClass.Instance, RogueTooltipColor, RogueDamageColor, RogueCritColor);
        }
        public static void LoadModTypes()
        {
            if (CalVal != null)
            {
                Type[] r = CalVal.Code.GetTypes();
                foreach (Type mn in r)
                {
                    if (mn.Name == "Fanny")
                    {
                        calvalFanny = mn;
                    }
                    if (mn.Name == "FannyTextbox")
                    {
                        calvalFannyBox = mn;
                    }
                    if (calvalFannyBox != null && calvalFanny != null)
                    {
                        break;
                    }
                }
            }
            if (Wrath != null)
            {
                Type[] r = AssemblyManager.GetLoadableTypes(Wrath.Code);
                foreach (Type mn in r)
                {
                    if (mn.Name == "AvatarUniverseExplorationSystem")
                    {
                        WrathAvatarWorldSystem = mn;
                    }
                    if (mn.Name == "AvatarOfEmptiness")
                    {
                        WrathAvatarOfEmptiness = mn;
                    }
                    if (mn.Name == "NamelessDeityBoss")
                    {
                        WrathNamelessDeity = mn;
                    }
                    if (WrathAvatarWorldSystem != null && WrathAvatarOfEmptiness != null && WrathNamelessDeity != null)
                    {
                        break;
                    }
                }
                if (WrathAvatarWorldSystem != null && WrathInAvatarWorld == null)
                    WrathInAvatarWorld = WrathAvatarWorldSystem.GetProperty("InAvatarUniverse", BindingFlags.Public | BindingFlags.Static);
            }
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod f))
            {
                Type[] r = f.Code.GetTypes();
                foreach (Type mn in r)
                {
                    if (mn.Name == "WorldSavingSystem")
                    {
                        FargoSoulsWorldSaveSystem = mn;
                    }
                    if (FargoSoulsWorldSaveSystem != null)
                    {
                        break;
                    }
                }
                if (FargoSoulsWorldSaveSystem != null && FargoSoulsAngryMutant == null)
                    FargoSoulsAngryMutant = FargoSoulsWorldSaveSystem.GetProperty("AngryMutant", BindingFlags.Public | BindingFlags.Static);
            }
        }
    }
}