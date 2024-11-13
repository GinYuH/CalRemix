using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.Polterghast;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.World;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadTrapperBulbChan()
        {
            HelperMessage.New("TrapperBeginning1", "Hey, have you seen my precious pink flower that I've been growing for 15 years? I left her around here. She's been my best friend for years now, and I could never fathom what I'd do if I had lost h",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedPlantBoss, cantBeClickedOff: true);

            HelperMessage fan1 = HelperMessage.New("TrapperBeginning2", "Oh.",
               "FannySob", HelperMessage.AlwaysShow, cantBeClickedOff: true, duration: 2)
                .ChainAfter();

            HelperMessage trap1 = HelperMessage.New("TrapperBeginning3", "Trapper Bulb Chan here! HAIIII!!! ^v^",
                "TrapperHappy", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(fan1, delay: 3);

            HelperMessage fan2 = HelperMessage.New("TrapperBeginning4", "Woah there! Where did you come from?",
                "FannyAwooga", HelperMessage.AlwaysShow, 7, cantBeClickedOff: true).ChainAfter(fan1, delay: 5);

            HelperMessage trap2 = HelperMessage.New("TrapperBeginning5", "UwU I'm just a friendly flower wandering around, desu!",
                "TrapperDefault", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(trap1);

            HelperMessage fan3 = HelperMessage.New("TrapperBeginning6", "Well, it’s nice to meet you! Any tips for our friend here?",
                "FannyNuhuh", HelperMessage.AlwaysShow, 7).ChainAfter(fan2);

            HelperMessage trap3 = HelperMessage.New("TrapperBeginning7", "Teehee, just keep blooming and shining, and remember to water your dreams, nyan!",
                "TrapperHappy", HelperMessage.AlwaysShow, 6).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(trap2);

            HelperMessage ff = HelperMessage.New("TrapperBeginning8", "Oh come fucking on.",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(fan3, startTimerOnMessageSpoken: true, delay: 10);

            HelperMessage poltrap1 = HelperMessage.New("Poltrapper1", "Trapper Bulb Chan desu! Yaho~ ^v^",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Polterghast>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage polfan1 = HelperMessage.New("Poltrapper2", "Ohhh, did you come across Polterghast in the dungeon? That spooky soul cluster gives me the hibby-jibbies, desu! >m< It’s like a creepy cousin of Plantera, ne? Just remember, even the darkest dungeons can have a little light if you believe in yourself, nyan!",
                "TrapperDefault", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(poltrap1, delay: 2, startTimerOnMessageSpoken: true);

            HelperMessage poltrap = HelperMessage.New("Poltrapper3", "Wow, Polterghast sounds like a real nightmare!",
                "FannyIdle", HelperMessage.AlwaysShow, 3, cantBeClickedOff: true).ChainAfter(polfan1, delay: 3f, startTimerOnMessageSpoken: true);

            HelperMessage polfantrap2 = HelperMessage.New("Poltrapper4", "Hai, Fanny-kun! But you know, even the scariest things can be defeated with a smile and a bit of courage, desu yo! Ganbatte, adventurer!",
                "TrapperHappy", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(poltrap, delay: 2, startTimerOnMessageSpoken: true);

            HelperMessage.New("Poltrapper5", "You heard her, friend! Face those spooky souls with a grin and a giggle!",
                "FannyNuhuh", HelperMessage.AlwaysShow).ChainAfter(polfantrap2, delay: 4);

            HelperMessage.New("Trapperbulbdotdotdot", "Ohhh, I see you found a... whaaa!!! It's like a distant memory of me, >m<! How strange to see myself in this form, but remember, even in stillness, there's beauty and purpose, nyan! Let's cherish every moment and bloom brightly together, even in different forms, desu! ^v^",
                "TrapperWTF", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TrapperBulb>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Lucy", "Ohayo! Trapper Bulb Chan here, desu! ^_^ Oh, you’ve got Lucy the Axe with you? Kawaii! Lucy-chan, you’re so sharp and strong, just like a samurai’s spirit, nya~",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.LucyTheAxe)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Meowmere", "Oh, you’ve found the legendary Meowmere? Nyan-tastic! That space sword shooting bouncing nyan cats is like a dream come true, nya~",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.Meowmere)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Meowthrower", "Look at that purrfect weapon, desu! It's like a rainbow of nyan-tastic power! OwO So cute and fierce at the same time, nya~! Let’s set the battlefield ablaze with kitty cuteness and colorful chaos, teehee~! ^w^ Unleash the meowgic and watch your enemies flee in adorable terror, nya~!",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Meowthrower>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Aotc", "Sugoi! You’ve got the Ark of the Cosmos, desu! That sword is so epic, like something straight out of an anime, nya~! OwO With those cool slash dashes, you’ll be zooming around like a true anime hero, cutting through enemies with style and grace, teehee~!",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Meowthrower>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("CosmicShiv", "Trapper Bulb Chan here, desu~ Oh, you’ve got the Cosmic Shiv! Did you know its tooltip used to mention cat girls? Teehee, that’s been changed now, but the cosmic energy it radiates still feels so magical and mysterious, nya~",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<CosmicShiv>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Nuclemade", "Nuclear Lemonade-kun, you’re so refreshing and powerful, just like a summer breeze with a hint of sparkle! With you around, every moment becomes a fizzy adventure, desu yo! OwO",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => CalRemixHelper.HasCrossModItem(Main.LocalPlayer, "CalamityHunt", "NuclearLemonade")).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Kitten", "Nyaa~! Look at this adorable kitty-chan, desu! So fuzzy and purrfect, like a tiny bundle of nyan-tastic joy! OwO You’re such a kawaii little neko, aren’t you? Let’s be the bestest of friends and have lots of fun together, nya~! ^w^ Remember to give this sweet kitty-chan all the pets and snuggles, nyaaa~!",
                "TrapperUwaa", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.TownCat)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Shipping", "Omo, omo! Trapper Bulb Chan here, and look at this—it's the Flying Dutchman, desu! spooky and cool, nya~! But you know what would be even cooler? If the Flying Dutchman-chan had a special someone to sail the seas with! OwO Imagine the adventures and romance, sailing through the moonlit waters together, nya~! Let's find a perfect ship for this ship, teehee~! ^w^",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.PirateShip)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("ShipRiding", "Nyaa~! Look at you, sailing the skies on a pirate ship, desu! So adventurous and cool, nya~! It’s like we’re in an epic anime, searching for treasure and having grand adventures! OwO Just imagine the tales we’ll tell and the friendships we’ll forge, nya~! Let’s sail towards the horizon and make every moment a legendary chapter, desu yo!",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.mount.Type == MountID.PirateShip).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage bone = HelperMessage.New("AncientBoneDust", "Ohhh… So Kawaii… That reminds me of Ancient Bone Dust Chan!!! :33 Purrhaps you’d never guess because of the name, but they were the youngest members of the club. OwO So small, so cute, like the most beautiful blossom.",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneNormalCaverns).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("AncientBoneDust2", "I wonder where they are now… I can’t find them anywhere I look, Nya!~ (but in a sad way)",
                "TrapperDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(bone).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage.New("BeetleJuiceBeetleJuice", "Ohhh… So Sugoi… That reminds me of Beetle Juice Chan! BJ was the most epicly fun member of the club, and was super duper full of energy! :D They never liked my nickname for them, though. :( Not sure why. >_<; Shame about the accident…",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.Derpling)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).AddEndEvent(GiveHead);

            HelperMessage.New("Bloodscene", "Ohhh… So Sugoi… That reminds me of Bloodletting Essence Chan! Don’t let the red hair desu-evice you, she was the sweetest member of the club! UwU So punk and so fierce, but with an adorably soft inside.~ Shame about the accident…",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneCrimson).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage.New("DemonAsh", "Ohhh… So Sugoi… That reminds me of Demonic Bone Ash Chan! They were the cool and collected club founder, and always helped us bloom!~ ^_^ I heard rumors that they had a super duper secret crush on Ancient Bone Dust Chan, hehehe! OwO Shame about the accident…",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneUnderworldHeight).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage.New("FetidEssence", "Ohhh… So Sugoi… That reminds me of Fetid Essence Chan! They were the wild card of the club, and always had something silly planned to keep us on our toes. :O All their hijinks felt like something out of an anime episode! Shame about the accident…",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneCorrupt).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage rival = HelperMessage.New("Rival", "Ohhh… So NOT Sugoi… That reminds me of Maneater Bulb Chan. Between you and me, you should probably avoid those enemies. >:(!!!! Maneater Bulb Chan was a big meanie head, and was always super jealous of how much cuter I was.",
                "TrapperHuh", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.ManEater)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Rival2", "I’d say ‘Shame about the accident…’ But… Eh.",
                "TrapperDisgust", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(rival).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage.New("MurkyPaste", "Ohhh… So Sugoi… That reminds me of Murky Paste Chan! You know, we all kind of hated Murky Paste chan. I have no clue why they were a part of the club. :/ Shame about the accident…",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.ToxicSludge)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage.New("MurkySludge", "Ohhh… So Sugoi… That reminds me of Murky Sludge Chan! They were twin siblings with Murky Paste chan, but you’d nya-ver guess. Everyone looooved them so much, they were the most popular at the school by far… Except for me!~ Shame about the accident…",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.MotherSlime)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).AddEndEvent(() => CalRemixWorld.trapperFriendsLearned++);

            HelperMessage revelation1 = HelperMessage.New("Revelation1", "Hey Trapper-Bulb Chan.. haha.. what’s that “incident” you’ve mentioned multiple times? Even as the flame of burning knowledge, not even I know what goes on outside of here, heh!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CalRemixWorld.trapperFriendsLearned >= 8, 6, cantBeClickedOff: true);

            HelperMessage revelation2 = HelperMessage.New("Revelation2", "Ow, dun worry bout it, nya~ Just a wittle secret of mine, uwu",
                "TrapperHappy", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(revelation1, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage revelation3 = HelperMessage.New("Revelation3", "Awh, shucks!~ You’re all my friends, nya~! I suppowse I cowld tell you guys..",
                "TrapperDefault", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(revelation2, startTimerOnMessageSpoken: true);

            HelperMessage revelation4 = HelperMessage.New("Revelation4", "Tooo starrrrttt...",
                "TrapperDefault", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(revelation3, startTimerOnMessageSpoken: true);

            HelperMessage revelation5 = HelperMessage.New("Revelation5", "And that’s what happened to them..!~ Sowwy if I trauwmatizzed you guys.. OwO",
                "TrapperHappy", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(revelation4, startTimerOnMessageSpoken: true);

            HelperMessage revelation7 = HelperMessage.New("Revelation7", "Oh... oh my god. I think I’m genuinely gonna be sick. Why... how... when...",
                "FannyDisturbed", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).ChainAfter(revelation5, delay: 3, startTimerOnMessageSpoken: true);
            
            HelperMessage revelation6 = HelperMessage.New("Revelation6", "Holy shit. Even by my standards.. Jesus christ. Wow.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(revelation5, delay: 5, startTimerOnMessageSpoken: true);

            HelperMessage.New("Revelation8", "The alpha leads the beta follows the sigma follows",
                "CrimSonDefault", HelperMessage.AlwaysShow, 30, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(revelation5, delay: 8);

        }

        public static void GiveHead()
        {
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ModContent.ItemType<BeetleHead>());
            CalRemixWorld.trapperFriendsLearned++;
        }
    }
}