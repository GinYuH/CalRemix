using CalamityMod;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        //Loads fanny messages that aren't associated with anything else in particular
        public static void LoadGeneralFannyMessages()
        {

            HelperMessage.New("RemixJump", "Hey there friend! I noticed your jumps were a little too weak, so I added a bit of my Fanny-spice and now you can jump TWO times! I hope you enjoy this!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().remixJumpCount >= 20).SetHoverTextOverride("Thanks Fanny! I will enjoy my new jumps!");

            HelperMessage.New("LowHP", "It looks like you're low on health. If your health reaches 0, you'll die. To combat this, don't let your health reach 0!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 1200, onlyPlayOnce: false).SetHoverTextOverride("Thanks Fanny! I'll heal.");

            HelperMessage.New("Invisible", "Where did you go?",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.invis || Main.LocalPlayer.shroomiteStealth || Main.LocalPlayer.vortexStealthActive || (Main.LocalPlayer.Calamity().rogueStealth >= Main.LocalPlayer.Calamity().rogueStealthMax && Main.LocalPlayer.Calamity().rogueStealthMax > 0)).SetHoverTextOverride("I'm still here Fanny!");
            
            HelperMessage.New("DarkArea", "Fun fact. The human head can still be conscious after decapitation for the average of 20 seconds.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => DarkArea() && CalRemixWorld.worldFullyStarted);
            
            HelperMessage.New("ConstantDeath", "Is that someone behind you...?",
                "FannySob", (ScreenHelperSceneMetrics scene) => DontStarveDarknessDamageDealer.darknessTimer >= 300 && !Main.LocalPlayer.DeadOrGhost);
            
            HelperMessage.New("Cursed", "Looks like you've been cursed! If you spam Left Click, you'll be able to use items again sooner!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.cursed);

            HelperMessage.New("OctFebruary", "Did you know? 31 in Octagonal is the same as 25 in Decimal! That means OCT 31 is the same as DEC 25! Happy Halloween and Merry Christmas!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => System.DateTime.Now.Month == 4 && System.DateTime.Now.Day == 14).SetHoverTextOverride("I did not know that! Thank you, Fanny!");
            
            HelperMessage.New("DungeonGuardian", "It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.",
                "FannyNuhuh", NearDungeonEntrance);

            HelperMessage.New("MeldGunk", "In a remote location underground, there is a second strain of Astral Infection. If left unattended for too long, it can start spreading and dealing irreversible damage! Stay safe and happy hunting!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CalRemixWorld.meldCountdown <= 3600 && Main.hardMode);

            HelperMessage.New("MeldHeart", "Look at all that gunk! I'm pretty sure it's impossible to break it, so the best solution I can give is to assure it doesn't spread further by digging around it.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CalRemixWorld.MeldTiles > 22 && !ModLoader.HasMod("NoxusBoss"));

            HelperMessage.New("MeldHeartNoxus", "Look at all that gunk! I'm pretty sure it's impossible to break it, well, maybe if you got some powerful spray bottle, but that might take a while, so the best solution I can give is to assure it doesn't spread further by digging around it.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => CalRemixWorld.MeldTiles > 22 && ModLoader.HasMod("NoxusBoss"));

            HelperMessage.New("EvilMinions", "Oh, joy, another player reveling in their summoned minions like they've won the pixelated lottery. Just remember, those minions are as loyal as your Wi-Fi signal during a storm—here one minute, gone the next. Enjoy your fleeting companionship, I guess.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.numMinions >= 10).SpokenByEvilFanny();

            HelperMessage.New("EvilTerraBlade", "Oh, congratulations, you managed to get a Terra Blade. I'm sure you're feeling all proud and accomplished now. But hey, don't strain yourself patting your own back too hard. It's just a sword, after all. Now, go on, swing it around like the hero you think you are.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) =>Main.LocalPlayer.HasItem(ItemID.TerraBlade)).SpokenByEvilFanny();

            HelperMessage.New("IonGuy", "Hey there, beachcomber! Looks like you've found a talking panel in that trash pile! It's wired up and ready to chat, but beware—it’s probably going to ask you for items. Remember, not all that glitters is gold, and not all talking panels are trustworthy. You might end up giving away your favorite pair of socks!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().ionDialogue > 0);

            HelperMessage.New("Multiplayer", "The atmosphere seems a lot more social than what I'm used to. Be wary of ruptures in reality!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.netMode != NetmodeID.SinglePlayer);

            HelperMessage.New("Adrenaboy1", "My-my! You sure are bolting like chili bean potatoes, my friend!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().adrenalineModeActive, 8, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("Adrenaboy2", "Ha-ha! Good one, Miracle Boy!",
                "FannyIdle", HelperMessage.AlwaysShow, 8, cantBeClickedOff: true).ChainAfter(delay: 4, startTimerOnMessageSpoken: true);

            HelperMessage.New("Adrenaboy3", "Explain it then.",
                "MiracleBoyIdle", HelperMessage.AlwaysShow, 8, cantBeClickedOff: true).ChainAfter(delay: 4, startTimerOnMessageSpoken: true).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("Adrenaboy4", "Well.. uh.. chili is spicy.. and adrenaline is what you get as a flight or fight response..",
                "FannyDisturbed", HelperMessage.AlwaysShow, 8, cantBeClickedOff: true).ChainAfter(delay: 4, startTimerOnMessageSpoken: true);

            HelperMessage.New("Adrenaboy5", "That wasn't the intention of my joke, flame.",
                "MiracleBoyIdle", HelperMessage.AlwaysShow).ChainAfter(delay: 4, startTimerOnMessageSpoken: true).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("Norcheese", "Something, something, you dirty cheater.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().NorfleetCounter >= 4).SpokenByEvilFanny();

            HelperMessage anniv1 = HelperMessage.New("Annivenriersary1", "HAPPY 20TH ANNIVERSARY, $0! We've had a lot of fun adventures over these last two decades, and here's to two more!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => DownedBossSystem.downedProvidence && Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().gottenCellPhone && NPC.downedPlantBoss && Main.rand.NextBool(10000), 6, cantBeClickedOff: true).AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage anniv2 = HelperMessage.New("Annivenriersary2", "... Fanny? What the fuck are you talking about? It's barely been one year, let alone twenty of them. Did you eat another lotus, or something?",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(anniv1, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage anniv3 = HelperMessage.New("Annivenriersary3", "Trapper-Chan LOVES celebrating, but twenty seems strange... a-are you okay, Fanny-Kun?",
                "TrapperDefault", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(anniv2, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage anniv4 = HelperMessage.New("Annivenriersary4", "... but, no, it's been 20 years, hasn't it?",
                "FannySob", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).ChainAfter(anniv3, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage anniv5 = HelperMessage.New("Annivenriersary5", "Bruh everyone is Everywhere At The End of Time",
                "CrimSonDefault", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(anniv4, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage anniv6 = HelperMessage.New("Annivenriersary6", " ",
                "FannySob", HelperMessage.AlwaysShow, 1, cantBeClickedOff: true).ChainAfter(anniv5);

            HelperMessage anniv7 = HelperMessage.New("Annivenriersary7", " ",
                "FannySob", HelperMessage.AlwaysShow, 1, cantBeClickedOff: true).ChainAfter(anniv6);

            HelperMessage.New("Annivenriersary8", "Okay, so I may've been off by a couple years...",
                "FannySob", HelperMessage.AlwaysShow).ChainAfter(anniv7);

            HelperMessage.New("Annivenriersary9", "BRO you can't Just change Your Dam Mind When I Agree. Cringe! Cringe! Uninstalling this shi rn Fr Fr",
                "CrimSonDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(anniv7, delay: 4);
        }
        private static bool DarkArea()
        {
            Color light = Lighting.GetColor((int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16);
            return light.R <= 5 || light.G <= 5 || light.B <= 5;
        }
    }
}
