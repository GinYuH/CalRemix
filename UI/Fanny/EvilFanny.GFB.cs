using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Other;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadEvilGFBMessages()
        {
            HelperMessage.New("GFBDesert", "The sand worm is looking a bit blue isn't he? You may find your pitiful projectiles useless, but I'll reveal a wicked trick to seize the upper hand. As you know, this monstrous beast thrives on devouring your puny attacks and converting them into life-sustaining water. How delightfully diabolical! To subdue this insatiable leviathan, you must cunningly employ your trusty melee weapons.",
               "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<DesertScourgeHead>())).SpokenByEvilFanny(true);

            HelperMessage.New("GFBCrab", "This guy is making me a bit dizzy, how rude. I don't have much to say about ol' crabson here, but you may find great profits once you bash its stupid green shell in.",
               "EvilFannyDisgusted", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Crabulon>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbHive", "Ohohoh you're gonna have fun with this one. The real kicker with the Hive Mind here is it can summon a cyst, a real game over trap, and if you're foolish enough to shatter it, you're in for a double dose of agony because another Hive Mind will spawn. So, I guess the real tip here is, don't be the idiot who rushes in without a plan, or you'll be in for a never-ending nightmare of your own incompetence.",
               "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<HiveMind>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbPerf", "Brace yourself for a bloodcurdling challenge. This monstrous abomination will summon three nasty worms when you cut one down. The first worm, oh how annoying, burrows into its vile hive, replenishing its sorry health. The second, the pesky splitter, will break into gruesome blobs that yearn for your destruction. And the last, the sadistic laser lord, conjures deadly walls of scorching beams to trap you.",
               "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<PerforatorHive>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbSlimeGod", "Ever felt that boss minions were too insignificant as you are? Well! In this world they will absolutely shower overwhelm you with dozens upon dozens of gel balls. My advice? Well, honestly, you're in for a world of hurt, but if you want to survive, focus on obliterating those puny slimes and evading their slimy detonations. Don't expect any mercy from this vile creation; it's only here to show you how insignificant you truly are.",
               "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SlimeGodCore>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbCryogen", "Wow, what a joke of a boss! You expected a frozen snowflake, but you get this pathetic excuse for a fire-themed one instead. It's as uninspired as it sounds! I hope you like homing fireballs!",
               "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Cryogen>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbAqua", "You're in for a real treat, you hapless fool, because this maggot's party trick is a nightmare! From the moment it slithers into your miserable existence, it'll wrap around you like a constricting nightmare, pelting you with projectiles aimed right at your feeble core. If you dare try to escape its lethal embrace, you'll be generously rewarded with a dose of crippling poison. So here's your precious tip: Accept your wretched fate and relish in the poison's sweet caress! Dance with this serpent of doom, dodging its toxic barrage with pinpoint precision, and strike back when it's momentarily vulnerable. Or flee, like the coward you are, and let the poison slowly consume you while it mocks your pathetic attempts to escape. The choice is yours, weakling. Enjoy your futile struggle!",
               "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AquaticScourgeHead>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbBrimmy", "Well, isn't this a surprise? Our fiery friend here usually sticks to playing with her little flames, but today she's feeling a bit more generous, unleashing her inner multi-elemental diva. How utterly inconvenient for you! You better brace yourselves, because in addition to her fiery tantrums, she's now throwing all kinds of elements your way. Sand, clouds, water—you name it, she's got it. Here's a tip, even though you probably don't deserve it: Pay attention to the boss's body language and you'll see what element she's currently focusing.",
               "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<BrimstoneElemental>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbCalclone", "You pitiful shrimp! As if facing a mechanical eye that hurls fireballs and spawns its wretched siblings charging at you isn't challenging enough, now you find yourselves confined to a pitiful arena, your movement mercilessly restricted. Ha! Your usual tactics won't save you here. Show some wits for once, or burn like the fools you truly are!",
               "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CalamitasClone>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbAureus", "This laughable arachnid thinks it can pose a challenge, but its so-called 'strategy' is nothing more than a circus act. When it starts flailing around like a fish dropped in the desert, be ready for the pathetic display of generic projectile patterns! The only thing intimidating here is the fact that it actually believes it can defeat you. Mock it mercilessly, dodge its sad projectiles, and rain down destruction upon its pitiful mechanical frame. The only question is: Can you crush this pitiable pest before it embarrasses itself further? Show this metallic mockery what true power looks like!",
              "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumAureus>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbFishies", "Well, well, it looks like our roles have been cruelly reversed this time, doesn't it? You'll be facing off against a gargantuan mermaid, and the so-called 'Leviathan' has been downsized to a mere annoyance. As always, remember that every challenge has a solution, but I won't be the one to hand it to you. You'll have to figure it out on your own, and you'd better do it fast, or you'll find yourself singing a sad tune at the bottom of the ocean. Good luck, or should I say, 'good riddance'!",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Leviathan>())).SpokenByEvilFanny(true);

            HelperMessage.New("Gfbplague", "The Tyrant's lovely favorite bee seems to be a bit more sick than usual. Those rockets of hers? Oh, they won't just explode; they'll leave a pestilent mist lingering around to ruin your day. But, hold on, sometimes she tosses peanuts at you - yeah, you heard that right! And if you're 'lucky,' you might even see a gauss nuke or two. So, don't just stand there with your jaws agape; stay alert and dodge her garbage attacks. This boss is as unpredictable as it gets, but you better figure her out if you want any chance of survival.",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<PlaguebringerGoliath>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbRavager", "Ah, what a pathetic attempt at originality! This walking corpse fortress may think it's clever, but it's nothing more than a hackneyed copycat. It shamelessly rips off Sans from Undertale, summoning those ridiculously uninspired skulls that shoot lasers at you. If you're dim-witted enough to get hit by these feeble attacks, you don't even deserve to play this game. But if you want to survive this sorry excuse for a boss battle, just dodge the lackluster lasers, and focus your attacks on the so-called 'fortress' itself. Don't be fooled by this uninspired imitation, and put it out of its misery before it realizes how utterly unoriginal it is!",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<RavagerBody>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbAstrumDeus", "Welcome, feeble player, to a cosmic encounter with a space worm beyond your meager comprehension. There's a chance you've faced it before, and it cunningly split into two to vex you. Now, in a delicious twist, it dares to multiply once again, though it'll remain a delightful secret, so I won't bother to spell it out for your primitive intellect. You'll have to guess how many offspring you're up against. Can you even count that high, imbecile? Just remember, victory may require more than your pathetic usual strategy. But don't expect me to guide you any further, it's more amusing to watch you flounder.",
             "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AstrumDeusHead>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbDonuts", "Ah, what an utter disappointment these three pitiful constructs are! You'd think they'd be as unique as they are grotesque, but alas, you'll barely find any interesting twists here, just more mediocrity. As they clumsily lumber around with their molten bodies and clichéd flaming wings, your best bet is to stick to the basics.",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ProfanedGuardianCommander>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbBirb", "Oh, look who decided to show up in this pathetic game! Our little draconic offshoot has had a makeover, or should I say a 'downgrade.' Now, it shrieks like a demented parakeet on steroids and thinks it's cute to summon its wimpy babies only for them to EXPLODE into lightning. What a low tier boss.",
             "EvilFannyDisgusted", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Bumblefuck>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbProvidence", "Only god can help you here.",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Providence>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbCeaseless", "Prepare for a rollercoaster of frustration, you miserable little adventurer. This malevolent monstrosity will gleefully toy with your feeble senses by warping the world into a nightmarish kaleidoscope of black and rainbow hues. Don't even bother trying to make sense of it; it's all just a taunting charade. To survive this despicable encounter, you'll need to navigate the disorienting chaos with the grace of a drunken clown.",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<CeaselessVoid>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbSignus", "This spectral jokester loves to play hide and seek, but he's got a little trick up his ethereal sleeve. When he stands still and goes all see-through, he's not just taking a break for a ghostly snack, he's charging up an attack that'll make you wish you never created a world with this seed. So, here's a tip for you, if your feeble mind can handle it: Keep a close eye on this translucent loser, and when he starts to shimmer and glow, run for cover like the coward you are. If you don't, you'll feel the full force of his otherworldly wrath, and there's no saving you from that.",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Signus>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbStormWeaver", "You there, feeble player, prepare to be humiliated by the wrath of this insufferable sky serpent. This time, it's even longer than your list of shortcomings! Not much else to say here, just use piercing weapons or explosions.",
             "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<StormWeaverHead>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbPolter", "Return at 3 am for a surprise.",
             "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => !(Main.time >= 27000 && Main.time < 30600 && Main.dayTime == false) && Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Polterghast>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbOverdose", "It's time for the laziest, most shameless, uninspired pile of pixelated junk you've ever seen, a sorry excuse for a boss who couldn't even come up with its own look! Meet this 'brown recolor' ripoff, a pitiful imitation of an already goofy boss. And guess what? This time, they've taken to flatulence as their 'unique' attack! Get ready to hold your nose and your laughter because it's about to get 'stinky' in here. Just dodge the gas clouds, seriously, how hard can it be? Pummel this pathetic carbon copy and watch it dissolve into the pixelated void where it belongs. If you thought the original was a challenge, prepare to be underwhelmed by this trash!",
              "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<OldDuke>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbYharon", "Oh, look who's trying to play hero, facing the chicken with its fancy fiery bullet hell attacks! But here's a little evil twist for you, you pathetic weakling: this dragon can heal back all its health, just to spite you. You think you're so clever? Well, you're not. To stand a chance, you'll have to do more than just dodge its relentless onslaught – you'll need to focus on interrupting its little healing charade. Attack like your life depends on it, because it does, you fool! Don't let this dragon laugh at your misery as it regenerates its health, or you'll be nothing more than a stain on its claws.",
              "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Yharon>())).SpokenByEvilFanny(true);

            HelperMessage.New("GfbExos", "As you stumble into this chaotic clash of metal monstrosities, let's address the real challenge here: the spineless nerd who's finally decided to get off his lazy behind and join the fray. The colossal skeletal abomination, the sinister fighter plane 'eyes,' and the writhing serpent spewing laser turrets are nothing compared to the pitiful attacks from their mastermind. You'll need to multitask your feeble brain, dodging the obvious and pathetic shots from the so-called 'genius' while taking down the hulking mechs. Focus on systematically disabling each colossal creation one at a time while smirking at the thought of that wannabe inventor's pathetic attempts to hinder you. Prove to them that they're nothing more than a sideshow in this battle, and leave them to wallow in their own mediocrity as you claim your victory with sheer malevolence.",
              "EvilFannyMiffed", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && (scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>() && n.ModNPC is AresBody head && !head.exoMechdusa) || scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<ThanatosHead>() && n.ModNPC is ThanatosHead head && !head.exoMechdusa) || scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<Apollo>() && n.ModNPC is Apollo head && !head.exoMechdusa))).SpokenByEvilFanny(true);

            HelperMessage.New("Hekate", "Behold! The most wretched amalgamation of monstrosities you've ever witnessed! The fabricational quartet has merged into a chaotic nightmare. What's this, you ask? The skeleton's got two puny probes now, shooting lasers? How quaint! To defeat this unholy amalgam, you'll need to exploit their combined idiocy. When these dimwits can't coordinate, they'll crumble under their own incompetence. Embrace the chaos and savor their impending doom!",
              "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<AresBody>() && n.ModNPC is AresBody head && head.exoMechdusa)).SpokenByEvilFanny(true);

            HelperMessage.New("GfbScal", "Well, well, you've finally reached the pinnacle of your pathetic little adventure. Meet the grand finale, the wicked witch of scarlet brimstone flames and necromancy. She's not one to follow the rules, so here's a dirty secret to make her life more miserable. When she summons that ghastly centipede made of human leftovers, don't expect it to kick the bucket easily this time. You'll need to work your sorry behind off and kill it yourself! Oh, and remember her cozy square arena? Surprise, surprise, it's all random now, like popcorn popping in hell. But here's the icing on the cake – her projectiles? All swapped out, so you can't rely on muscle memory. Have fun stumbling through this chaotic nightmare, because I'm not here to hold your hand, hero!",
              "EvilFannyPoint", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<SupremeCalamitas>())).SpokenByEvilFanny(true);

            HelperMessage.New("TheLorde", "Hey there, you clueless wannabe gamer! You've stumbled upon the most infuriating easter egg boss in gaming history. This abomination of an AI can't make up its damn mind and switches between the traits of every boss and enemy you've ever faced, just to make sure you can't predict a thing. So, don't even bother thinking you've got it figured out. When this wretched abomination is on its last legs, it pulls the ultimate cheat move - going invincible for a whole agonizing minute before you can finally put it out of its misery. And yeah, you'll be pulling your hair out, but remember, even this unholy mess can't escape its inevitable doom. So, gear up, slug through that excruciating minute, and claim your hollow victory, loser!",
              "EvilFannyMiffed", (ScreenHelperSceneMetrics scene) => Main.zenithWorld && scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<THELORDE>())).SpokenByEvilFanny(true);
        }
    }
}
