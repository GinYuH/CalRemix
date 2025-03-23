﻿using CalamityMod;
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
        public static readonly SoundStyle BizarroFannyTalk = new("CalRemix/Assets/Sounds/Helpers/BizarroFannyTalk");
        public static void LoadPassiveMessages()
        {
            HelperMessage.New("GonerFanny", "",
                "BizarroFannyGoner", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(60000000)).SpokenByAnotherHelper(ScreenHelpersUIState.BizarroFanny).AddStartEvent(NoMusic);

            HelperMessage.New("Register", "Did you know you can see where a register is initialized in its current scope by clicking on it with the middle mouse button? All instances of the register in the current scope will highlight in bright yellow. The mustard yellow one is where it is initialized in the current scope.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).SetHoverTextOverride("I see!");

            HelperMessage.New("Sleeping", "Do you ever dream of me?",
                "FannyEepy", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000) && Main.LocalPlayer.sleeping.isSleeping).SetHoverTextOverride("Zzzzzzz");

            HelperMessage.New("FungusGarden", "Careful when exploring the Shroom Garden. I hear some rather large crustaceans make their home there. Wouldn't want to be turned into Delicious Meat!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && !DownedBossSystem.downedCrabulon, cooldown: 120);

            HelperMessage.New("FakeGen", "I don't mean to alarm you my friend, but it seems like something huge might have generated in your world! You might want to go investigate whatever caused that terrible racket.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).AddStartEvent(FakeGen);

            HelperMessage.New("FalseRef", "WHOA! Is that a reference to another of my favorite games?????",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000));

            HelperMessage.New("ProbablyYakuza", "One time, I saw someone being dragged into a car by three men. The men took around 10 minutes and 23 seconds to subdue their victim, and 2 more minutes to drive away. I did nothing to stop it.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).AddEndEvent(() => MoodTracker.remorsefulMood.Activate());

            HelperMessage.New("CreditCard", "Heya $0 I'm feeling hungry could you send me your credit card details so I can get some food!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1500000)).SetHoverTextOverride("Sure thing Fanny!").AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("Blink", "Hey! Uhh, I noticed you haven't blinked in a while. Maybe you should...",
               "FannySob", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(4000000));

            HelperMessage.New("Fuckyou", "You are now manually breathing.",
               "FannyBigGrin", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(4000000));

            HelperMessage.New("Luigi", "Did you know you can unlock a \"Luigi\" by defeating every boss in Death mode on the first attempt? I don't even know what that is, but that has to be genuine! I read it online!",
              "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(4000000) && !CalamityWorld.death);

            HelperMessage.New("Mount", "Do a barrel roll on that thing you're riding!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(1000) && Main.LocalPlayer.mount.Type != MountID.None);

            HelperMessage.New("LookingForPlating", "Are you trying to find some Dubious Plating? I'm afraid that the stocks for them have plummeted and all existing plating was turned into scrap metal to be dumped in the Dungeon, so try looking there!",
               "FannyNuhuh", PlatingCheck);

            HelperMessage.New("Creepy", Main.rand.Next(1000000) + " remaining...",
                "FannyCryptid", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(100000000), duration: 60, cantBeClickedOff: true);

            HelperMessage.New("Mhage", "Be careful when using magic weapons. Drinking too many mana potions can drain your health, and leave you vulnerable to enemy attacks.",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.ActiveItem().DamageType == DamageClass.Magic, cooldown: 300, onlyPlayOnce: false);

            HelperMessage.New("Thrust", "Did you know you can parry enemy attacks with your sword? Just right click the moment something is about to hit you, and you'll block it with ease!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && Main.LocalPlayer.ActiveItem().DamageType == ModContent.GetInstance<TrueMeleeDamageClass>(), cooldown: 300, onlyPlayOnce: false);

            HelperMessage.New("Frozen1", "I'm back! It was quite chilly in there, but luckily, I was able to thaw myself out! Hopefully it doesn't happen again!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 1).SetHoverTextOverride("...");

            HelperMessage.New("Frozen2", "I-cee you're having some trouble. Don't worry! I broke out of the ice cube I was stuck in again!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 2);

            HelperMessage.New("Frozen3", "Wouldja believe it? I somehow managed to get trapped in another ice cube! Whoever keeps doing that is sure getting on thin ice.",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 3);

            HelperMessage.New("Frozen4", "This is a bit embarassing, but I got myself caught in yet another ice cube! This shtick is getting cold at this point, or should I say warm?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 4);

            HelperMessage.New("Frozen5", "At this point me and ice have gotten to know each other quite well, a true dance of the elements. I won't weigh you down anymore with updates on my frigid situation, have fun!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 5);

            HelperMessage.New("Frozen6", "Oh wait wait wait, this time I found a small crumb inside the ice. It was disgusting!",
               "FannyDisturbed", (ScreenHelperSceneMetrics scene) => fannyTimesFrozen == 6);

            HelperMessage.New("WrathRiftMention", "Strange that none of the others notice that rift in the sky. Fanny? That disgusting thing? They're either useless dumbasses or blind.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(370000) && DownedBossSystem.downedPolterghast && CalRemixAddon.Wrath != null, 8, cantBeClickedOff: true).SpokenByEvilFanny().InitiateConversation();

            HelperMessage.New("WrathRiftMention2", "Mhhmmm, what did you call me my red striped flame? I'm a miraculous and beautiful angel sent by god to bless our little adventure mmmmm!",
                "MiracleBoyIdle", duration: 8, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter();

            HelperMessage.New("WrathRiftMention3", "Kill yourself.",
                "EvilFannyMiffed", duration: 5, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter();

            HelperMessage.New("WrathRiftMention4", "Paradise... reclaiming. Star girl... in my way. $0... mine. I must take it all.",
                "FannyStare", duration: 8, cantBeClickedOff: true).ChainAfter(delay: 2).AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("WrathRiftMention5", "Wait what was I saying?",
                "FannyIdle", duration: 5, cantBeClickedOff: true).ChainAfter(delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage.New("WrathRiftMention6", "On second thought this isn't gonna end well for any of us.",
                "EvilFannyIdle").SpokenByEvilFanny().ChainAfter(delay: 2).EndConversation();

            HelperMessage.New("CalamityChecker", "My-my! It looks like you have the Calamity Mod enabled. You're in for a special treat, my friend!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.rand.NextBool(2160000) && ModLoader.HasMod("CalamityChecker")).SetHoverTextOverride("Why thank you, my trusted flame steed. I too enjoy the fruits of the Calamity modification for Terraria. I find joy and bliss in the contents of this articulately, well-crafted piece of art for such a highly-praised video game, and you too flame. You've been such a helpful assistant in my goals of terraforming this world as my own, decorating it finely with my creations comparable to an artist and their fine-tipped paintbrush upon a felt canvas. Without the presence of your partnership, I would fall into an ever-growing pit of confusion, despair, darkness and misery as a result of the overwhelming unknown knowledge of such world that we're in, but you've helped me to glimpse into the beauty of this world and see it for what it truly is. Together, we've built up a community of all races, gender, and species, forever welded with the light that comes with friendship. We've truly managed to create a perfect version of such community, a remix of sorts. As we tread on through the lands stained by past calamities, through the infernum of the dragons and against the catalyst of the cosmos' gift, we can face the wrath of the gods together, forging our path through the stars above the birds and the foliage together, with every twist and turn bringing a new challenge for all of us. Us, as in our community. At such point in time, however, one cannot simply call it, a \"community\" anymore. We are family. With each victory we achieve, we gain a new melody in our symphony of the world. And when the final battle comes, when we stand face-to-face with the deities too unfathomable to describe, we'll still yet to back down. With our hearts alight with fire, our weapons ready, and the wisdom of our journey together, we shall strike down the final calamity with a grace only we could understand! Grazing through fields of the razor-sharp tentacles that the cerloise-painted scules bear, we stand together as a remix of this community. We are, Community Remix.");

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
                            HelperMessage.New("StraightUpEvil", "By the way, $0, I see everything. Like how you have played " + noTerraria[Main.rand.Next(0, noTerraria.Count - 1)],
                           "FannyCryptid", (ScreenHelperSceneMetrics m) => NPC.downedDeerclops).AddDynamicText(SteamFriends.GetPersonaName).SetHoverTextOverride("Oh golly gee Fanny!");
                        }
                    }
                }
            }
            catch
            {

            }

            discord1 = HelperMessage.New("DiscordianHash", "Oh you're on Discord? What are they talking about in $0? I wanna see!",
            "FannyIdle", (ScreenHelperSceneMetrics m) => DiscordChat != "" && DiscordChat.Contains('#') && NPC.downedBoss1 && !discord2.alreadySeen).AddDynamicText(() => DiscordChat).SetHoverTextOverride("Nothing much Fanny!");

            screenHelperMessages.Add(discord1);

            discord2 = HelperMessage.New("DiscordianAt", "Oh you're on Discord? What are you and $0 talking about? I wanna see!",
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
        public static void NoMusic()
        {
            Main.musicVolume = 0;
        }
        private static void FakeGen()
        {
            for (int i = 0; i < 10; i++) 
            {
                SoundEngine.PlaySound(SoundID.Tink, Main.LocalPlayer.Center + Vector2.UnitX * (Main.rand.NextBool() ? 100 : -100));
            }
            Thread.Sleep(10000);
        }
        public static bool PlatingCheck(ScreenHelperSceneMetrics scene)
        {
            try
            {
                if (Main.LocalPlayer == null)
                    return false;
                if (Main.LocalPlayer.chest <= -1 || Main.LocalPlayer.chest >= Main.chest.Length)
                    return false;
                int c = Main.LocalPlayer.chest;
                if (Main.chest == null || Main.chest.Length <= 0)
                    return false;
                if (Main.chest[c] == null)
                    return false;
                if (Main.chest[c].x < 0 && Main.chest[c].x >= Main.maxTilesX && Main.chest[c].y < 0 && Main.chest[c].y >= Main.maxTilesY)
                    return false;
                if (Main.tile[Main.chest[c].x, Main.chest[c].y].TileType == ModContent.TileType<SecurityChestTile>() || Main.tile[Main.chest[c].x, Main.chest[c].y].TileType == ModContent.TileType<AgedSecurityChestTile>())
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Data);
                Console.WriteLine(e.TargetSite);
                Console.WriteLine(e.InnerException);
            }
            return false;
        }
    }
}