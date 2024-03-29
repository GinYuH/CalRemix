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

namespace CalRemix.CrossCompatibility
{
    public class ModSupport : ModSystem
    {
        internal Mod BossChecklist;
        internal Mod MusicDisplay;
        internal Mod Wikithis;
        public override void Load()
        {
            ModLoader.TryGetMod("BossChecklist", out BossChecklist);
            ModLoader.TryGetMod("MusicDisplay", out MusicDisplay);
            ModLoader.TryGetMod("Wikithis", out Wikithis);
        }
        public override void Unload()
        {
            BossChecklist = null;
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
            bc.Call("LogBoss", Mod, "TheCalamity", 0.0000000000022f, () => CalRemixWorld.downedCalamity, NPCType<TheCalamity>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<LoreAwakening>(),
                ["customPortrait"] = calamityPortrait
            });
            Action<SpriteBatch, Rectangle, Color> wfportrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Bosses/Wulfwyrm/WulfwyrmBossChecklist").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, null, color, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            };
            bc.Call("LogBoss", Mod, "WulfrumExcavator", 0.22f, () => CalRemixWorld.downedExcavator, NPCType<WulfwyrmHead>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<EnergyCore>(),
                ["customPortrait"] = wfportrait
            });
            bc.Call("LogBoss", Mod, "Acidsighter", 2.1f, () => CalRemixWorld.downedAcidsighter, NPCType<Acideye>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<PoisonedSclera>(),
            });
            Action<SpriteBatch, Rectangle, Color> plportrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Bosses/Poly/Polyphemalus").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            /*
            bc.Call("LogBoss", Mod, "Derellect", 12.5f, () => CalRemixWorld.downedDerellect, NPCType<DerellectBoss>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>(),
            });
            */
            List<int> poly = new() { NPCType<Astigmageddon>(), NPCType<Exotrexia>(), NPCType<Conjunctivirus>(), NPCType<Cataractacomb>() };
            bc.Call("LogBoss", Mod, "Polyphemalus", 12.7f, () => CalRemixWorld.downedPolyphemalus, poly, new Dictionary<string, object>()
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
            bc.Call("LogMiniBoss", Mod, "Clamitas", 6.8f, () => CalRemixWorld.downedClamitas, NPCType<Clamitas>(), new Dictionary<string, object>()
            {
                ["customPortrait"] = clPortrait
            });
            Action<SpriteBatch, Rectangle, Color> cdPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Minibosses/CyberDraedon").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, new Color(0, 255, 255, 125));
            };
            bc.Call("LogMiniBoss", Mod, "CyberDraedon", 3.99999f, () => CalRemixWorld.downedCyberDraedon, NPCType<CyberDraedon>(), new Dictionary<string, object>()
            {
                ["spawnItems"] = ItemType<BloodyVein>(),
                ["customPortrait"] = cdPortrait
            });
            bc.Call("LogMiniBoss", Mod, "KingMinnowsPrime", 18.1f, () => CalRemixWorld.downedKingMinnowsPrime, NPCType<KingMinnowsPrime>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "LaRuga", 20.2f, () => CalRemixWorld.downedLaRuga, NPCType<LaRuga>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "LifeSlime", 16.7f, () => CalRemixWorld.downedLifeSlime, NPCType<LifeSlime>(), new Dictionary<string, object>());
            bc.Call("LogMiniBoss", Mod, "OnyxKinsman", 7.5f, () => CalRemixWorld.downedOnyxKinsman, NPCType<OnyxKinsman>(), new Dictionary<string, object>());
            Action<SpriteBatch, Rectangle, Color> pePortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = Request<Texture2D>("CalRemix/NPCs/Minibosses/PlagueEmperor").Value;
                Texture2D texture2 = Request<Texture2D>("CalRemix/NPCs/Minibosses/PlagueEmperorEyes").Value;
                Vector2 centered = new(rect.Center.X - (texture.Width / 2), rect.Center.Y - (texture.Height / 2));
                sb.Draw(texture, centered, color);
                sb.Draw(texture2, centered, color);
            };
            bc.Call("LogMiniBoss", Mod, "PlagueEmperor", 21.5f, () => CalRemixWorld.downedPlagueEmperor, NPCType<PlagueEmperor>(), new Dictionary<string, object>()
            {
                ["customPortrait"] = pePortrait
            });
            bc.Call("LogMiniBoss", Mod, "YggdrasilEnt", 18.2f, () => CalRemixWorld.downedYggdrasilEnt, NPCType<YggdrasilEnt>(), new Dictionary<string, object>());
        }
        internal void AddMusicDisplayEntries()
        {
            if (MusicDisplay is null)
                return;
            AddMusic("Opticatalysis", "Opticatalysis", "DEMON GIRLFRIEND");
            AddMusic("AntarcticReinsertion", "Antarctic Reinsertion", "Jteoh");
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