using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalamityMod.Items.LoreItems;
using CalRemix.Items;
using CalRemix.NPCs.Minibosses;
using CalRemix.NPCs.Bosses.Wulfwyrm;
using CalRemix.NPCs.Bosses.Poly;
using CalRemix.NPCs.Bosses.BossScule;
using CalRemix.NPCs.Bosses.Acideye;
using static Terraria.ModLoader.ModContent;
using CalRemix.NPCs.Bosses.Carcinogen;
using CalRemix.NPCs.TownNPCs;

namespace CalRemix.CrossCompatibility
{
    public class ModSupport : ModSystem
    {
        internal Mod BossChecklist;
        internal Mod MusicDisplay;
        internal Mod Wikithis;
        internal Mod Census;
        public override void Load()
        {
            ModLoader.TryGetMod("BossChecklist", out BossChecklist);
            ModLoader.TryGetMod("Census", out Mod Census);
            ModLoader.TryGetMod("MusicDisplay", out MusicDisplay);
            ModLoader.TryGetMod("Wikithis", out Wikithis);
        }
        public override void Unload()
        {
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
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Bosses/BossScule/TheCalamity_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            };
            bc.Call("LogBoss", Mod, "TheCalamity", 0.0000000000022f, () => RemixDowned.downedCalamity, NPCType<TheCalamity>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<Slumbering>(),
                ["customPortrait"] = calamityPortrait
            });
            Action<SpriteBatch, Rectangle, Color> wfportrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Bosses/Wulfwyrm/WulfwyrmBossChecklist").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            };
            bc.Call("LogBoss", Mod, "WulfrumExcavator", 0.22f, () => RemixDowned.downedExcavator, NPCType<WulfwyrmHead>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<EnergyCore>(),
                ["customPortrait"] = wfportrait
            });
            bc.Call("LogBoss", Mod, "Acidsighter", 2.1f, () => RemixDowned.downedAcidsighter, NPCType<Acideye>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<PoisonedSclera>(),
            });
            /*bc.Call("LogBoss", Mod, "Carcinogen", 9.22f, () => RemixDowned.downedCarcinogen, NPCType<Carcinogen>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemID.WoodWall,
            });*/
            Action<SpriteBatch, Rectangle, Color> plportrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Bosses/Poly/Polyphemalus").Value;
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

            // Minibosses
            Action<SpriteBatch, Rectangle, Color> clPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Minibosses/Clamitas_BC").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            bc.Call("LogMiniBoss", Mod, "Clamitas", 6.8f, () => RemixDowned.downedClamitas, NPCType<Clamitas>(), new Dictionary<string, object>()
            {
                ["customPortrait"] = clPortrait
            });
            Action<SpriteBatch, Rectangle, Color> cdPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Minibosses/CyberDraedon").Value;
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
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Minibosses/PlagueEmperor").Value;
                Texture2D texture2 = Request<Texture2D>("CalRemix/NPCs/Minibosses/PlagueEmperorEyes").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
                sb.Draw(texture2, centered, color);
            };
            bc.Call("LogMiniBoss", Mod, "PlagueEmperor", 21.5f, () => RemixDowned.downedPlagueEmperor, NPCType<PlagueEmperor>(), new Dictionary<string, object>()
            {
                ["customPortrait"] = pePortrait
            });
            bc.Call("LogMiniBoss", Mod, "YggdrasilEnt", 18.2f, () => RemixDowned.downedYggdrasilEnt, NPCType<YggdrasilEnt>(), new Dictionary<string, object>());
        }
        internal void AddCensusEntries()
        {
            if (Census is null)
                return;
            Census.Call("TownNPCCondition", ModContent.NPCType<ZER0>(), "Have [i:CalRemix/Ogscule] in your inventory during Godseeker mode");
            Census.Call("TownNPCCondition", ModContent.NPCType<YEENA>(), "The current month is December, January, or February or Astrum Deus has been defeated in a Snow biome");

            Census.Call("TownNPCCondition", ModContent.NPCType<Ogslime>(), "Kill a Wandering Eye while wearing Titan Heart armor");
        }
        internal void AddMusicDisplayEntries()
        {
            if (MusicDisplay is null)
                return;
            AddMusic("Opticatalysis", "Opticatalysis", "DEMON GIRLFRIEND");
            AddMusic("AntarcticReinsertion", "Antarctic Reinsertion", "Jteoh");
            AddMusic("Gegenschein", "Gegenschein", "Jteoh");
            AddMusic("TropicofCancer", "Tropic of Cancer", "Jteoh");
            AddMusic("SignalInterruption", "Signal Interruption", "Sploopo");
            AddMusic("ScourgeoftheScrapyard", "Scourge of the Scrapyard", "Sploopo");
            AddMusic("LaRuga", "La Ruga's Ambience", "Sploopo");
            AddMusic("PlaguedJungle", "Everlasting Dark", "HeartPlusUp");
            AddMusic("EyesofFlame", "Eyes of Flame (Remix)", "DM DOKURO");
            AddMusic("TheEyesareAnger", "The Eyes are Anger", "GummyLeeches");

        }
        internal void AddMusic(string realName, string name, string author)
        {
            MusicDisplay.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, $"Sounds/Music/{realName}"), name, author, Mod.DisplayName);
        }
    }
}