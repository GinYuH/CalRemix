using CalamityMod;
using CalamityMod.Tiles.DraedonStructures;
using CalamityMod.World;
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
    public partial class ScreenHelperManager : ModSystem
    {
        //Loads fanny messages that have a chance to happen anywhere
        public static readonly SoundStyle HappySFX = new($"{nameof(CalRemix)}/Sounds/Happy");
        public static void LoadPassiveMessages()
        {
            screenHelperMessages.Add(new HelperMessage("GonerFanny", "",
                "FannyGoner", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(60000000)).SpokenByAnotherHelper(ScreenHelpersUIState.GonerFanny));

            screenHelperMessages.Add(new HelperMessage("Register", "Did you know you can see where a register is initialized in its current scope by clicking on it with the middle mouse button? All instances of the register in the current scope will highlight in bright yellow. The mustard yellow one is where it is initialized in the current scope.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).SetHoverTextOverride("I see!"));

            screenHelperMessages.Add(new HelperMessage("Sleeping", "Do you ever dream of me?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000) && Main.LocalPlayer.sleeping.isSleeping).SetHoverTextOverride("..."));

            screenHelperMessages.Add(new HelperMessage("FungusGarden", "Careful when exploring the Shroom Garden. I hear some rather large crustaceans make their home there. Wouldn't want to be turned into Delicious Meat!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && !DownedBossSystem.downedCrabulon, cooldown: 120));

            screenHelperMessages.Add(new HelperMessage("FakeGen", "I don't mean to alarm you my friend, but it seems like something huge might have generated in your world! You might want to go investigate whatever caused that terrible racket.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).AddStartEvent(FakeGen));

            screenHelperMessages.Add(new HelperMessage("FalseRef", "WHOA! Is that a reference to another of my favorite games?????",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)));

            screenHelperMessages.Add(new HelperMessage("ProbablyYakuza", "One time, I saw someone being dragged into a car by three men. The men took around 10 minutes and 23 seconds to subdue their victim, and 2 more minutes to drive away. I did nothing to stop it.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)));

            screenHelperMessages.Add(new HelperMessage("CreditCard", "Heya $0 I'm feeling hungry could you send me your credit card details so I can get some food!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).SetHoverTextOverride("Sure thing Fanny!").AddDynamicText(HelperMessage.GetPlayerName));

            screenHelperMessages.Add(new HelperMessage("Blink", "Hey! Uhh, I noticed you haven't blinked in a while. Maybe you should...",
               "FannySob", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(4000000)));

            screenHelperMessages.Add(new HelperMessage("Fuckyou", "You are now manually breathing.",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(4000000)));

            screenHelperMessages.Add(new HelperMessage("Luigi", "Did you know you can unlock a \"Luigi\" by defeating every boss in Death mode on the first attempt? I don't even know what that is, but that has to be genuine! I read it online!",
              "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(4000000) && !CalamityWorld.death));

            screenHelperMessages.Add(new HelperMessage("Mount", "Do a barrel roll on that thing you're riding!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1000) && Main.LocalPlayer.mount.Type != MountID.None));

            screenHelperMessages.Add(new HelperMessage("LookingForPlating", "Are you trying to find some Dubious Plating? I'm afraid that the stocks for them have plummeted and all existing plating was turned into scrap metal to be dumped in the Dungeon, so try looking there!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.chest != -1 && (Main.tile[Main.chest[Main.LocalPlayer.chest].x, Main.chest[Main.LocalPlayer.chest].y].TileType == ModContent.TileType<SecurityChestTile>() || Main.tile[Main.chest[Main.LocalPlayer.chest].x, Main.chest[Main.LocalPlayer.chest].y].TileType == ModContent.TileType<AgedSecurityChestTile>())));

            screenHelperMessages.Add(new HelperMessage("Creepy", Main.rand.Next(1000000) + " remaining...",
                "FannyCryptid", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(100000000), duration: 60, needsToBeClickedOff: false));

            screenHelperMessages.Add(new HelperMessage("Mhage", "Be careful when using magic weapons. Drinking too many mana potions can drain your health, and leave you vulnerable to enemy attacks.",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == DamageClass.Magic, cooldown: 300, onlyPlayOnce: false));

            screenHelperMessages.Add(new HelperMessage("Thrust", "Did you know you can parry enemy attacks with your sword? Just right click the moment something is about to hit you, and you'll block it with ease!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>(), cooldown: 300, onlyPlayOnce: false));

            screenHelperMessages.Add(new HelperMessage("Frozen1", "I'm back! It was quite chilly in there, but luckily, I was able to thaw myself out! Hopefully it doesn't happen again!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 1).SetHoverTextOverride("..."));

            screenHelperMessages.Add(new HelperMessage("Frozen2", "I-cee you're having some trouble. Don't worry! I broke out of the ice cube I was stuck in again!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 2));

            screenHelperMessages.Add(new HelperMessage("Frozen3", "Wouldja believe it? I somehow managed to get trapped in another ice cube! Whoever keeps doing that is sure getting on thin ice.",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 3));

            screenHelperMessages.Add(new HelperMessage("Frozen4", "This is a bit embarassing, but I got myself caught in yet another ice cube! This shtick is getting cold at this point, or should I say warm?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 4));

            screenHelperMessages.Add(new HelperMessage("Frozen5", "At this point me and ice have gotten to know each other quite well, a true dance of the elements. I won't weigh you down anymore with updates on my frigid situation, have fun!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 5));

            screenHelperMessages.Add(new HelperMessage("Frozen6", "Oh wait wait wait, this time I found a small crumb inside the ice. It was disgusting!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 6));

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
                            screenHelperMessages.Add(new HelperMessage("StraightUpEvil", "By the way, $0, I see everything. Like how you have played " + noTerraria[Main.rand.Next(0, noTerraria.Count - 1)],
                           "FannyCryptid", (ScreenHelperSceneMetrics m) => NPC.downedDeerclops).AddDynamicText(SteamFriends.GetPersonaName).SetHoverTextOverride("Oh golly gee Fanny!"));
                        }
                    }
                }
            }
            catch
            {

            }

            discord1 = new HelperMessage("DiscordianHash", "Oh you're on Discord? What are they talking about in $0? I wanna see!",
            "FannyIdle", (ScreenHelperSceneMetrics m) => DiscordChat != "" && DiscordChat.Contains('#') && NPC.downedBoss1 && !discord2.alreadySeen).AddDynamicText(() => DiscordChat).SetHoverTextOverride("Nothing much Fanny!");

            screenHelperMessages.Add(discord1);

            discord2 = new HelperMessage("DiscordianAt", "Oh you're on Discord? What are you and $0 talking about? I wanna see!",
            "FannyIdle", (ScreenHelperSceneMetrics m) => DiscordChat != "" && !DiscordChat.Contains('#') && NPC.downedBoss1 && !discord1.alreadySeen).AddDynamicText(() => DiscordChat).SetHoverTextOverride("Nothing much Fanny!");

            screenHelperMessages.Add(discord2);
        }

        public static HelperMessage discord1 = null;
        public static HelperMessage discord2 = null;
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