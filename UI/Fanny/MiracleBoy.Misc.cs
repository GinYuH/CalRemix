﻿using CalamityMod;
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

            HelperMessage.New("MiracleRain", "Oh, splendid! Acid Rain, but make it worse! Truly a miraculous turn of events! The sky hates you, the ground hates you, and now the monsters are stepping up their game too. What’s next, you ask? I don’t know—maybe the trees will start throwing punches. Good luck, champ!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => AcidRainEvent.AcidRainEventIsOngoing)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("RuinousSoul", "Ah, the Ruinous Soul! A delightful little trinket fresh from our dearly departed friend, Polterghast. Quite the prize, wouldn’t you say? These souls are brimming with anguish, despair, and, oh, just a sprinkle of malice. Perfect for crafting all manner of dreadful, powerful contraptions! Use them wisely... or recklessly—I’m not your boss, after all.",
                "MiracleBoyRead", (ScreenHelperSceneMetrics metrics) => DownedBossSystem.downedPolterghast)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("MiracleL4", "Well, well, look at you, reaching the bottom of the ocean's despair-ridden belly! The lowest layer of the Abyss—how delightfully dreadful! Here, the water crushes, the darkness suffocates, and the creatures? Oh, they’ve probably been sharpening their teeth just for you. But hey, at least it’s quiet, right? Aside from the screams. Yours, I mean.",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.Calamity().ZoneAbyssLayer4)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("YharonEgg", "Oh, would you look at that! A Blessed Phoenix Egg—a literal ticking time bomb of fiery destruction! This little beauty is your invitation to face Yharon, the Jungle Dragon, Yharim's personal barbecue buddy. I mean, sure, it’s a big deal, but don’t get cocky. That dragon’s got more firepower than an entire volcano, and he’s not exactly fond of uninvited guests. Ready to get roasted, my daring friend?",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<YharonEgg>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("Sentinels", "Oh, look at you, summoning a cosmic lackey from the Distortion! Don’t worry, it’s more bark than bite—unless you’re hopeless. Prove me wrong, champ!",
                "MiracleBoyRead", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<StormWeaverHead>() || n.type == ModContent.NPCType<Signus>() || n.type == ModContent.NPCType<CeaselessVoid>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage fish = HelperMessage.New("Fishing1", "Morgen, mein friend! It is not a guten one, however, as if it were, you would be fishing!",
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

            HelperMessage.New("YharBarr", "Ugh, Yharim Bars. Look at these disgusting chunks of... whatever they are. Gross, right? They're huge, ugly, and look like something you’d find at the bottom of a trash heap. But I guess they’re useful for making endgame stuff, if you're into that kind of thing. Honestly, I wouldn’t touch them if I weren’t forced to. But hey, you do you. Just don’t expect any praise for collecting these hideous things.",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<YharimBar>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

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

            HelperMessage.New("Hypnerd", "Hypnos, huh? Look at this brainiac—so shiny, so... fancy—but wait, what’s this? Nein nein nein! Gnnnaawww What’s with all the techy, brainy boop-bop-bop nonsense? You think you're so advanced? How cute. Watch out, Hypnos, or I'll start spouting some Gnnnwaaaa too and make your circuits fry!",
                "MiracleBoyGnaw", (ScreenHelperSceneMetrics metrics) => metrics.onscreenNPCs.Any((NPC n) => n.type == ModContent.NPCType<AergiaNeuron>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("Fear", "I'd sleep with one eye open if I were you.",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.time == 27000 && Main.rand.NextBool(100)).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("TerribleMachine", "You've activated the great and terrible machine!!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.mouseRight && Main.mouseRightRelease && Main.rand.NextBool(100)).SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).SetHoverTextOverride("May God save us all");


        }
    }
}