using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.StormWeaver;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadMiracleBoyMessages()
        {
            HelperMessage.New("MiracleSnow", "Wow, it's absolutely freezing out here! You're shivering, but really, it's no problem. Since you're so cold, what do you say I warm you up? Mmm?!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.ZoneSnow)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("MiracleScourge", "This video contains content from UMG, who has blocked it from display on this website or application",
                "MiracleBoyRead", (ScreenHelperSceneMetrics metrics) => Main.rand.NextBool(3600) && Main.curMusic > MusicID.Count)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).AddEndEvent(() => Main.musicVolume = 0);

            HelperMessage.New("Miracle1", "You don't understand! I'm only one! 1 is young!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.AlphabetStatue1))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("HurtMiracle", "Oh no, you've scratched your delicate little self! Shall I fetch you a band-aid? Or maybe you want me to kiss it better? No? Well, do try to stay upright, would you? It’s embarrassing for both of us.",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.75f, cooldown: 1200, onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("VeryHurtMiracle", "Yikes! You almost became a miracle failure with that hit!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.25f, cooldown: 1200, onlyPlayOnce: false).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("MiracleRain", "Quick, my miraculous stooge, we must get to the sulphurous sea! Now that the Moon Lord is dead, the acid rain has been empowered once again. Here is your daily Miracle tip! Want to know which enemies drop the best loot? It's simple! Aim at anything that's green! Green body? Green eyes? It doesn't matter! The green is where money is, that's why coins are the color they are!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => AcidRainEvent.AcidRainEventIsOngoing)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("RuinousSoul", "Do you really need me to say something? Do you need me to tell you you can make stratus gear out of this? You have recipe browser and google. Get a grip, man.",
                "MiracleBoyRead", (ScreenHelperSceneMetrics metrics) => DownedBossSystem.downedPolterghast)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("MiracleL4", "Hey! Hey! Watch this!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.Calamity().ZoneAbyssLayer4)
                .AddEndEvent(GivePlayerChaosState)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("YharonEgg", "Wow, my friend! Isn't that just the most luxurious egg you've ever seen? You know, I've heard that if you kill the little creature inside of it, you'll gain powers beyond your wildest dreams! Who would pass on such a proposition? No, really? Like, even if you don't want to kill it, it's the dragon of REBIRTH. Its thing is that it comes back to life. You'd have to be an actual MORON not to do something like that. Hahah.",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<YharonEgg>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage sentinels1 = HelperMessage.New("Sentinels1", "Watch out, my darest chap! It's one of the devourer's DASTARDLY sentinels! If you want to have any hope of defeating them, you'll need to use 100%-no, 110% of your miraculous skills! Remember, no amount of preparation is too much! Even if you've consumed all the potions you were able to find, there are many other options for boosts! Remember, alcohol is your best friend! I sure know it's mine!",
                "MiracleBoyRead", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<StormWeaverHead>() || n.type == ModContent.NPCType<Signus>() || n.type == ModContent.NPCType<CeaselessVoid>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).InitiateConversation(15);
            HelperMessage sentinels2 = HelperMessage.New("Sentinels2", "Sorry to barge in, but the correct name for them is the \"Rune of Kos bosses\". They also aren't related to the Devourer anymore.",
                "FannyDisturbed", HelperMessage.AlwaysShow, 5)
                .ChainAfter(sentinels1, delay: 3f);
            HelperMessage sentinels3 = HelperMessage.New("Sentinels3", "Not a single person has ever called them that. You should learn when to shut up, Fanny.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow, 3)
                .SpokenByAnotherHelper(ScreenHelpersUIState.EvilFanny)
                .ChainAfter(sentinels2, delay: 3f);
            HelperMessage sentinels4 = HelperMessage.New("Sentinels4", "FINALLY! SOMEBODY WHO UNDERSTANDS THE MIRACLE PLIGHT OF THE MIRACLE BOY! Oh, Evil Fanny, maybe you and I aren't so diferent...",
                "MiracleBoyIdle", HelperMessage.AlwaysShow, 5)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy)
                .ChainAfter(sentinels3, delay: 3f);
            HelperMessage sentinels5 = HelperMessage.New("Sentinels5", "Nevermind. You're completely correct, Fanny.",
                "EvilFannyPissed", HelperMessage.AlwaysShow, 5)
                .SpokenByAnotherHelper(ScreenHelpersUIState.EvilFanny).EndConversation()
                .ChainAfter(sentinels4, delay: 3f);

            HelperMessage fish = HelperMessage.New("Fishing1", "Morning, my friend! It is not a good one, however, as if it were, you would be fishing!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.inventory.Any((Item i) => i.fishingPole > 0) && Main.time == 10800 && Main.dayTime,
                4, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).InitiateConversation(30f, 1f);

            HelperMessage.New("Fishing2", "Uhhh... wouldn't all the fish be asleep right now, Miracle Boy?",
                "FannyIdle", cantBeClickedOff: true, duration: 5).ChainAfter(delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage.New("Fishing3", "Oh my god, do you EVER shut up? This is nonstop, every time I say anything! You always feel the need to contradict me, even when I'm just cracking a joke for our dear friend here. This is why everyone DESPISES you, Fanny! I wish you were more like my creamsicle, because THAT doesn't talk back so much- you're terrible! You've literally never helped in the history of forever, you were left to rot in a gutter by your own father, and now this? Say what you will about Evil Fanny, but at least she knows damn well when she isn't wanted. Because, get this: you're not. You hear me? YOU. ARE. WORTHLESS.",
                "MiracleBoyIdle")
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter(delay: 0);

            HelperMessage.New("Fishing4", "... what?",
                "FannySob").ChainAfter(delay: 5, startTimerOnMessageSpoken: true).EndConversation();


            HelperMessage.New("TheGreatExoFable1", "Hello once more, my friend! I've heard you're a big fan of epic stories, mmm? Well, you're in luck! Your best friend is here to tell you one of his favourite tales!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld && CalamityGlobalNPC.draedonExoMechPrime != -1 && CalamityGlobalNPC.draedonExoMechTwinGreen != -1 && CalamityGlobalNPC.draedonExoMechWorm != -1, cantBeClickedOff: true, duration: 4)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("TheGreatExoFable2", "This one has been in the miracle family for generations, mmmm! Be prepared for the most amazing of stories! A tale to knock the tyrant's tales out of this world!",
                "MiracleBoyRead", cantBeClickedOff: true, duration: 3).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter(delay: 4, startTimerOnMessageSpoken: true);

            HelperMessage.New("TheGreatExoFable3", "Before there was a world, there were gods, and before these gods, there were dragons. But before all of this, Gods, real ones, existed in perpetuity. As the elders gave shape to to the visions that plagued them, something malevolent began to take shape. Duality is an essencial constant of our world. Light and Dark. Joy and Sadness. Chaos and order. On a fundamental level, if pure good can exists, all it guarantees is the possibility of pure evil.",
                "MiracleBoyRead", cantBeClickedOff: true, duration: 15).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter(delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage.New("TheGreatExoFable4", "With every world created, there is a chance for new possibilties, new paths to be taken. A fundamental basis, born of ashes of betrayal. Tathered and mangled by the stolen valor of those who delude themselves to think they are worthy of holy titles. Due to this, a vessel of a true harbinger of order must descend onto these lands, and sever its decay from the roots. Through their holy hand, the one in control of this world's future shall have their will broken, and this land will return to its intended state. Nothing.",
                "MiracleBoyRead", cantBeClickedOff: true, duration: 15).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter(delay: 15, startTimerOnMessageSpoken: true);

            HelperMessage.New("TheGreatExoFable5", "So! Did you enjoy that! If you did, you must love how grand of a writer I am! All of that? Made it up on the spot, mmmmm! All of these pages are blank! Happy to have been able to share this with you, I bid you farewell now!",
                "MiracleBoyIdle").SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).ChainAfter(delay: 15, startTimerOnMessageSpoken: true);

            HelperMessage.New("Hypnerd", "Why is there another one of these things? Why is the brain exposed?? That's like the one part you don't want to expose??? What genuine fucking idiot designed this thing!!! What the fuck man!!! What the FUCK!!!!!!!! FUCKK!!!!!! I'M SO MAD!!!!! AHHH!!!!!!!!!",
                "MiracleBoySweat", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<AergiaNeuron>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("Fear", "I'd sleep with one eye open if I were you.",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.time == 27000 && Main.rand.NextBool(100)).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("TerribleMachine", "You've activated the great and terrible machine!!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.mouseRight && Main.mouseRightRelease && Main.rand.NextBool(100)).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).SetHoverTextOverride("May God save us all");

            HelperMessage.New("Loser", "Loser",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasBuff(BuffID.PotionSickness), cooldown: 55, onlyPlayOnce: false, cantBeClickedOff: true, duration: 55).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("CosmicWorm", "he has a name though",
                "MiracleBoySob", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<CosmicWorm>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);
        }

        private static void GivePlayerChaosState()
        {
            Main.LocalPlayer.AddBuff(BuffID.ChaosState, 5);
        }
    }
}