using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.Polterghast;
using System.Linq;
using Terraria;
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
                "TrapperDefault", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(fan1, delay: 3);

            HelperMessage fan2 = HelperMessage.New("TrapperBeginning4", "Woah there! Where did you come from?",
                "FannyAwooga", HelperMessage.AlwaysShow, 7, cantBeClickedOff: true).ChainAfter(fan1, delay: 5);

            HelperMessage trap2 = HelperMessage.New("TrapperBeginning5", "UwU I'm just a friendly flower wandering around, desu!",
                "TrapperDefault", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(trap1);

            HelperMessage fan3 = HelperMessage.New("TrapperBeginning6", "Well, it’s nice to meet you! Any tips for our friend here?",
                "FannyNuhuh", HelperMessage.AlwaysShow, 7).ChainAfter(fan2);

            HelperMessage trap3 = HelperMessage.New("TrapperBeginning7", "Teehee, just keep blooming and shining, and remember to water your dreams, nyan!",
                "TrapperDefault", HelperMessage.AlwaysShow, 6).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(trap2);

            HelperMessage ff = HelperMessage.New("TrapperBeginning8", "Oh come fucking on.",
                "EvilFannyIdle", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(fan3, startTimerOnMessageSpoken: true, delay: 10);

            HelperMessage poltrap1 = HelperMessage.New("Poltrapper1", "Trapper Bulb Chan desu! Yaho~ ^v^",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Polterghast>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage polfan1 = HelperMessage.New("Poltrapper2", "Ohhh, did you come across Polterghast in the dungeon? That spooky soul cluster gives me the hibby-jibbies, desu! >m< It’s like a creepy cousin of Plantera, ne? Just remember, even the darkest dungeons can have a little light if you believe in yourself, nyan!",
                "TrapperDefault", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(poltrap1, delay: 2, startTimerOnMessageSpoken: true);

            HelperMessage poltrap = HelperMessage.New("Poltrapper3", "Wow, Polterghast sounds like a real nightmare!",
                "FannyIdle", HelperMessage.AlwaysShow, 3, cantBeClickedOff: true).ChainAfter(polfan1, delay: 3f, startTimerOnMessageSpoken: true);

            HelperMessage polfantrap2 = HelperMessage.New("Poltrapper4", "Hai, Fanny-kun! But you know, even the scariest things can be defeated with a smile and a bit of courage, desu yo! Ganbatte, adventurer!",
                "TrapperDefault", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(poltrap, delay: 2, startTimerOnMessageSpoken: true);

            HelperMessage.New("Poltrapper5", "You heard her, friend! Face those spooky souls with a grin and a giggle!",
                "FannyNuhuh", HelperMessage.AlwaysShow).ChainAfter(polfantrap2, delay: 4);

            HelperMessage.New("Trapperbulbdotdotdot", "Ohhh, I see you found a... whaaa!!! It's like a distant memory of me, >m<! How strange to see myself in this form, but remember, even in stillness, there's beauty and purpose, nyan! Let's cherish every moment and bloom brightly together, even in different forms, desu! ^v^",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TrapperBulb>())).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

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
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.TownCat)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("Shipping", "Omo, omo! Trapper Bulb Chan here, and look at this—it's the Flying Dutchman, desu! spooky and cool, nya~! But you know what would be even cooler? If the Flying Dutchman-chan had a special someone to sail the seas with! OwO Imagine the adventures and romance, sailing through the moonlit waters together, nya~! Let's find a perfect ship for this ship, teehee~! ^w^",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == NPCID.PirateShip)).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

            HelperMessage.New("ShipRiding", "Nyaa~! Look at you, sailing the skies on a pirate ship, desu! So adventurous and cool, nya~! It’s like we’re in an epic anime, searching for treasure and having grand adventures! OwO Just imagine the tales we’ll tell and the friendships we’ll forge, nya~! Let’s sail towards the horizon and make every moment a legendary chapter, desu yo!",
                "TrapperDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.mount.Type == MountID.PirateShip).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);
        }
    }
}