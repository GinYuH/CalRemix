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
using CalRemix.Core.World;
using CalRemix.Content.Items.SummonItems;
using CalRemix.Content.Items.Lore;

namespace CalRemix
{
    public class CalRemixAddon : ModSystem
    {
        public static Mod CalVal;
        public static Mod Catalyst;
        public static Mod Infernum;
        public static Mod Wrath;

        public static Mod BossChecklist;
        public static Mod MusicDisplay;
        public static Mod Wikithis;
        public static Mod Census;

        public static Type calvalFanny = null;
        public static Type calvalFannyBox = null;

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
            "UnCalamityModMusic"
        };
        public static List<ModItem> Items = new List<ModItem>();
        public override void Load()
        {
            ModLoader.TryGetMod("CalValEX", out CalVal);
            ModLoader.TryGetMod("CatalystMod", out Catalyst);
            ModLoader.TryGetMod("InfernumMode", out Infernum);
            ModLoader.TryGetMod("NoxusBoss", out Wrath);

            ModLoader.TryGetMod("BossChecklist", out BossChecklist);
            ModLoader.TryGetMod("Census", out Census);
            ModLoader.TryGetMod("MusicDisplay", out MusicDisplay);
            ModLoader.TryGetMod("Wikithis", out Wikithis);

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
        }
        public override void Unload()
        {
            CalVal = null;
            Catalyst = null;
            Infernum = null;
            Wrath = null;

            BossChecklist = null;
            Census = null;
            MusicDisplay = null;
            Wikithis = null;
        }
        public override void PostSetupContent()
        {
            AddBossChecklistEntries();
            AddMusicDisplayEntries();
            if (Wikithis != null && !Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                Wikithis.Call("AddModURL", Mod, "https://terrariamods.wiki.gg/wiki/Calamity_Community_Remix/{}");
                Wikithis.Call("AddWikiTexture", Mod, Request<Texture2D>("CalRemix/icon_small"));
            }
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
            bc.Call("LogBoss", Mod, "WulfrumExcavator", 0.22f, () => RemixDowned.downedExcavator, NPCType<WulfwyrmHead>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<EnergyCore>(),
                ["customPortrait"] = wfportrait
            });
            bc.Call("LogBoss", Mod, "Origen", 0.3f, () => RemixDowned.downedOrigen, NPCType<OrigenCore>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<OrigenDoor>(),
            });
            bc.Call("LogBoss", Mod, "Acidsighter", 2.1f, () => RemixDowned.downedAcidsighter, NPCType<Acideye>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<PoisonedSclera>(),
            });
            bc.Call("LogBoss", Mod, "Carcinogen", 5.241f, () => RemixDowned.downedCarcinogen, NPCType<Carcinogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemID.WoodWall,
            });
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
            bc.Call("LogBoss", Mod, "Phytogen", 14.25f, () => RemixDowned.downedPhytogen, NPCType<Phytogen>(), new Dictionary<string, object>());
            bc.Call("LogBoss", Mod, "Hydrogen", 14.15f, () => RemixDowned.downedHydrogen, NPCType<Hydrogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemID.Grenade,
            });
            bc.Call("LogBoss", Mod, "Oxygen", 11.75f, () => RemixDowned.downedOxygen, NPCType<Oxygen>(), new Dictionary<string, object>()
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
            bc.Call("LogBoss", Mod, "XP-00 Hypnos", 22.5f, () => RemixDowned.downedHypnos, NPCType<Hypnos>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>()
            });
            /*
                $"Jam a [i:{ItemType<CalamityMod.Items.Pets.BloodyVein>()}] into the codebreaker",
                "An imperfection after allÅ, what a shame.",
             */
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
            bc.Call("LogEvent", Mod, "PandemicPanic", 16.71f, () => RemixDowned.downedPathogen, new List<int> { NPCType<Malignant>(), NPCType<Ecolium>(), NPCType<Basilius>(), NPCType<Tobasaia>(), NPCType<MaserPhage>(), NPCType<WhiteBloodCell>(), NPCType<Platelet>(), NPCType<RedBloodCell>(), NPCType<Eosinine>(), NPCType<Dendritiator>() }, new Dictionary<string, object>());
        }
        internal void AddCensusEntries()
        {
            if (Census is null)
                return;
            Census.Call("TownNPCCondition", NPCType<ZER0>(), "Have [i:CalRemix/Ogscule] in your inventory during Godseeker mode");
            Census.Call("TownNPCCondition", NPCType<YEENA>(), "The current month is December, January, or February or Astrum Deus has been defeated in a Snow biome");

            Census.Call("TownNPCCondition", NPCType<Ogslime>(), "Kill a Wandering Eye while wearing Titan Heart armor");

            Census.Call("TownNPCCondition", NPCType<WALTER>(), "Defeat Pathogen");
            Census.Call("TownNPCCondition", NPCType<IRON>(), "Defeat Ionogen");
            Census.Call("TownNPCCondition", NPCType<SIIVA>(), "Defeat Phytogen");
            Census.Call("TownNPCCondition", NPCType<UNCANNY>(), "Defeat Carcinogen");
            Census.Call("TownNPCCondition", NPCType<KABLOOEY>(), "Defeat Hydrogen");
            Census.Call("TownNPCCondition", NPCType<BALLER>(), "Defeat Oxygen");
        }
        internal void AddMusicDisplayEntries()
        {
            if (MusicDisplay is null)
                return;
            AddMusic(CalRemixMusic.Exosphere, "CanAnybodyHearMe", "Jteoh");

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
            AddMusic(CalRemixMusic.Origen, "Antarctic Reinforcement (Structured Mix)", "Jteoh");
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

            AddMusic(CalRemixMusic.Pyrogen, "Infernal Seal", "ThePurified");
            AddMusic(CalRemixMusic.DevourerofGods, "Last Battle (Ballos Mix)", "DM DOKURO");
            AddMusic(CalRemixMusic.Hypnos, "Cerebral Augmentations", "The Exiled Fellow");

            AddMusic(CalRemixMusic.ExoMechs, "Omniscius Calculus", "Jteoh");
            AddMusic(CalRemixMusic.ExoTwins, "Omicron Venata", "Jteoh");
            AddMusic(CalRemixMusic.Ares, "Delta Duella", "Jteoh");
            AddMusic(CalRemixMusic.Thanatos, "Epsilon Termina", "Jteoh");
            AddMusic(CalRemixMusic.ExoTwinsThanatos, "Omicron Termina", "Jteoh");
            AddMusic(CalRemixMusic.ExoTwinsAres, "Delta Venata", "Jteoh");
            AddMusic(CalRemixMusic.ThanatosAres, "Epsilon Duella", "Jteoh");

            AddMusic(CalRemixMusic.Menu, "This is Where the Magic Happens", "Jteoh");
        }
        internal void AddMusic(int music, string name, string author)
        {
            MusicDisplay.Call("AddMusic", (short)music, name, author, Mod.DisplayName);
        }
    }
}