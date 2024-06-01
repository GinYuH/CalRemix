using CalamityMod;
using CalamityMod.Tiles.DraedonStructures;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        //Loads fanny messages that have a chance to happen anywhere
        public static readonly SoundStyle HappySFX = new($"{nameof(CalRemix)}/Sounds/Happy");
        public static void LoadPassiveMessages()
        {
            fannyMessages.Add(new FannyMessage("GonerFanny", "",
                "Goner", (FannySceneMetrics scene) => Main.rand.NextBool(60000000)).SpokenByAnotherFanny(FannyUIState.GonerFanny));

            fannyMessages.Add(new FannyMessage("Register", "Did you know you can see where a register is initialized in its current scope by clicking on it with the middle mouse button? All instances of the register in the current scope will highlight in bright yellow. The mustard yellow one is where it is initialized in the current scope.",
                "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(1500000)).SetHoverTextOverride("I see!"));

            fannyMessages.Add(new FannyMessage("Sleeping", "Do you ever dream of me?",
                "Idle", (FannySceneMetrics scene) => Main.rand.NextBool(1500000) && Main.LocalPlayer.sleeping.isSleeping).SetHoverTextOverride("..."));

            fannyMessages.Add(new FannyMessage("FungusGarden", "Careful when exploring the Shroom Garden. I hear some rather large crustaceans make their home there. Wouldn't want to be turned into Delicious Meat!",
                "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && !DownedBossSystem.downedCrabulon, cooldown: 120));

            fannyMessages.Add(new FannyMessage("FakeGen", "I don't mean to alarm you my friend, but it seems like something huge might have generated in your world! You might want to go investigate whatever caused that terrible racket.",
                "Idle", (FannySceneMetrics scene) => Main.rand.NextBool(1500000)).AddStartEvent(FakeGen));

            fannyMessages.Add(new FannyMessage("FalseRef", "WHOA! Is that a reference to another of my favorite games?????",
                "Awooga", (FannySceneMetrics scene) => Main.rand.NextBool(1500000)));

            fannyMessages.Add(new FannyMessage("ProbablyYakuza", "One time, I saw someone being dragged into a car by three men. The men took around 10 minutes and 23 seconds to subdue their victim, and 2 more minutes to drive away. I did nothing to stop it.",
                "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(1500000)));

            fannyMessages.Add(new FannyMessage("Blink", "Hey! Uhh, I noticed you haven't blinked in a while. Maybe you should...",
               "Sob", (FannySceneMetrics scene) => Main.rand.NextBool(4000000)));

            fannyMessages.Add(new FannyMessage("Fuckyou", "You are now manually breathing.",
               "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(4000000)));

            fannyMessages.Add(new FannyMessage("Mount", "Do a barrel roll on that thing you're riding!",
               "Awooga", (FannySceneMetrics scene) => Main.rand.NextBool(1000) && Main.LocalPlayer.mount.Type != MountID.None));

            fannyMessages.Add(new FannyMessage("LookingForPlating", "Are you trying to find some Dubious Plating? I'm afraid that the stocks for them have plummeted and all existing plating was turned into scrap metal to be dumped in the Dungeon, so try looking there!",
               "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.chest != -1 && (Main.tile[Main.chest[Main.LocalPlayer.chest].x, Main.chest[Main.LocalPlayer.chest].y].TileType == ModContent.TileType<SecurityChestTile>() || Main.tile[Main.chest[Main.LocalPlayer.chest].x, Main.chest[Main.LocalPlayer.chest].y].TileType == ModContent.TileType<AgedSecurityChestTile>())));

            fannyMessages.Add(new FannyMessage("Creepy", Main.rand.Next(1000000) + " remaining...",
                "Cryptid", (FannySceneMetrics scene) => Main.rand.NextBool(100000000), duration: 60, needsToBeClickedOff: false));

            fannyMessages.Add(new FannyMessage("Mhage", "Be careful when using magic weapons. Drinking too many mana potions can drain your health, and leave you vulnerable to enemy attacks.",
               "Nuhuh", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == DamageClass.Magic, cooldown: 300, onlyPlayOnce: false));

            fannyMessages.Add(new FannyMessage("Thrust", "Did you know you can parry enemy attacks with your sword? Just right click the moment something is about to hit you, and you'll block it with ease!",
               "Idle", (FannySceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>(), cooldown: 300, onlyPlayOnce: false));

            fannyMessages.Add(new FannyMessage("Frozen1", "I'm back! It was quite chilly in there, but luckily, I was able to thaw myself out! Hopefully it doesn't happen again!",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 1).SetHoverTextOverride("..."));

            fannyMessages.Add(new FannyMessage("Frozen2", "I-cee you're having some trouble. Don't worry! I broke out of the ice cube I was stuck in again!",
               "Nuhuh", (FannySceneMetrics scene) => fannyTimesFrozen == 2));

            fannyMessages.Add(new FannyMessage("Frozen3", "Wouldja believe it? I somehow managed to get trapped in another ice cube! Whoever keeps doing that is sure getting on thin ice.",
               "Nuhuh", (FannySceneMetrics scene) => fannyTimesFrozen == 3));

            fannyMessages.Add(new FannyMessage("Frozen4", "This is a bit embarassing, but I got myself caught in yet another ice cube! This shtick is getting cold at this point, or should I say warm?",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 4));

            fannyMessages.Add(new FannyMessage("Frozen5", "At this point me and ice have gotten to know each other quite well, a true dance of the elements. I won't weigh you down anymore with updates on my frigid situation, have fun!",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 5));

            fannyMessages.Add(new FannyMessage("Frozen6", "Oh wait wait wait, this time I found a small crumb inside the ice. It was disgusting!",
               "Idle", (FannySceneMetrics scene) => fannyTimesFrozen == 6));

            try
            {
                if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\"))
                {
                    List<string> games = Directory.GetDirectories("C:\\Program Files (x86)\\Steam\\steamapps\\common\\").ToList<string>();
                    if (games.Count > 0)
                    {
                        List<string> noTerraria = new List<string>();
                        if (games != null && games.Count > 0)
                        {
                            for (int i = 0; i < games.Count; i++)
                            {
                                string b = games[i].Remove(0, 46);
                                if (!(b.Contains("Terraria") || b.Contains("tModLoader") || b.Contains("Steamworks")))
                                {
                                    noTerraria.Add(b);
                                }
                            }
                        }
                        if (noTerraria.Count > 0)
                        {
                            fannyMessages.Add(new FannyMessage("StraightUpEvil", "By the way, $0, I see everything. Like how you have played " + noTerraria[Main.rand.Next(0, noTerraria.Count - 1)],
                           "Cryptid", (FannySceneMetrics m) => NPC.downedDeerclops).AddDynamicText(SteamFriends.GetPersonaName).SetHoverTextOverride("Oh golly gee Fanny!"));
                        }
                    }
                }
            }
            catch
            {

            }

            discord1 = new FannyMessage("DiscordianHash", "Oh you're on Discord? What are they talking about in $0? I wanna see!",
            "Idle", (FannySceneMetrics m) => DiscordChat != "" && DiscordChat.Contains('#') && NPC.downedBoss1 && !discord2.alreadySeen).AddDynamicText(() => DiscordChat).SetHoverTextOverride("Nothing much Fanny!");

            fannyMessages.Add(discord1);

            discord2 = new FannyMessage("DiscordianAt", "Oh you're on Discord? What are you and $0 talking about? I wanna see!",
            "Idle", (FannySceneMetrics m) => DiscordChat != "" && !DiscordChat.Contains('#') && NPC.downedBoss1 && !discord1.alreadySeen).AddDynamicText(() => DiscordChat).SetHoverTextOverride("Nothing much Fanny!");

            fannyMessages.Add(discord2);
        }

        public static FannyMessage discord1 = null;
        public static FannyMessage discord2 = null;
        public static string DiscordChat = "";

        public static string GetDiscord()
        {
            Process[] processes = new Process[1];
            try
            {
                processes = Process.GetProcesses();
                string discord = "";
                foreach (Process p in processes)
                {
                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        if (p.MainWindowTitle.Contains("Discord"))
                        {
                            discord = p.MainWindowTitle;
                            break;
                        }
                    }
                }
                if (discord != "")
                {
                    string finalDiscord = "";
                    bool foundHash = false;
                    for (int i = 0; i < discord.Length; i++)
                    {
                        if (!foundHash)
                        {
                            if (discord[i] == '#' || discord[i] == '@')
                            {
                                foundHash = true;
                                if (discord[i] == '@')
                                    continue;
                            }
                        }
                        if (foundHash)
                        {
                            if (discord[i] != ' ')
                            {
                                finalDiscord += discord[i];
                            }
                            else
                            {
                                DiscordChat = finalDiscord;
                                return finalDiscord;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return "";
        }

        private static void FakeGen()
        {
            for (int i = 0; i < 10; i++) 
            {
                SoundEngine.PlaySound(SoundID.Tink, Main.LocalPlayer.Center + Vector2.UnitX * (Main.rand.NextBool() ? 100 : -100));
            }
            Thread.Sleep(10000);
        }
    }
}