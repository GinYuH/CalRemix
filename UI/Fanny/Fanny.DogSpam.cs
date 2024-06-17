using CalamityMod;
using CalamityMod.NPCs.DevourerofGods;
using System.Linq;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static bool ReadAllDogTips = false;
        public static bool BeatDog => DownedBossSystem.downedDoG;

        public static void LoadDogSpamMessages()
        {
            bool hasInfernum = ModLoader.TryGetMod("InfernumMode", out _);

            HelperMessage startMessage = new HelperMessage("Dog1", "Jeez! That Cosmic Serpent has a new layer of protection! Don't worry, I'm sure it's nothing you can't handle. You've fought foes that I'm sure were much tougher than this snake. Your previous combat encounters have given you the tools AND skills required to beat this oversized noodle to the ground, AND you have me by your side! Just remember to stay focused, and keep your eyes on his head!",
                "FannyIdle", DogSecondPhase, 15, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false, maxWidth: 500);

            HelperMessage message1 = new HelperMessage("Dog2", "...Actually, I think I have some tips on hand for this guy. Let me see here...",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 7, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false).NeedsActivation(5);

            HelperMessage message2 = new HelperMessage("Dog3", " ...Alright. It says here that the Devourer of Gods, also known as the Nameless Serpent, is a colossal worm originating from... Hold on. He's the \"Nameless Serpent\"? That's not much of a name! And besides, he's already referred to as the Devourer of Gods. Oh well, that doesn't matter right now. Anyways, he came from the Distortion World. He's here under the Reign of Yharim to, as his title implies, devour Gods. Right now, he has a resistance to Magic, Poison, and Dark. If you have any weapons that are augmented to those elements, I wouldn't recommend using them! At least right now. Without his armor, he has a pretty significant weakness to slash-type weapons. If you can catch him again without any armor, try using something that slashes! Anyways, what else is there here...",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 18, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false, maxWidth: 500).NeedsActivation();

            HelperMessage message3 = new HelperMessage("Dog4", "...Okay, it says some stuff about his attack patterns here. Right now, he should probably be ramming into you. It says here the best strategy to combat that is through a shield that lets you penetrate... What does that mean? \"Lets you penetrate\". Penetrate what? His head? I guess it's telling you to dash through his head? Try doing that! When he isn't dashing, hes performing one of his other laser-based attacks.  The first one listed here shows a picture with a bunch of lines pointing inwards towards a player. It looks like the best way to avoid that attack is to follow the blind spots. Try moving towards any air you can see! The next one kind of looks like a Star of David made out of laser lines... Can I call them that? The little telegraphs? Yeah, I think i'll call them laser lines. Okay, so the laser lines are all pointing inwards from six evenly-spaced portals at the \"points\" of the star. It says here you should try and stand still when the portals appear, and move towards the blind spots when the laser lines are solid! So try doing that!! Gimme a second to read a little further...",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 20, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false, maxWidth: 500).NeedsActivation();

            HelperMessage message4 = new HelperMessage("Dog5", "...Yep! The next attack shown here has sort of a big grid look going on. Jeez, it almost looks like a lightshow! This is all starting to remind me of when I went to a rave. It was 2013, and I was young... Well, younger than I was now. There were a lot of things there that I feel like this titanic invertebrate took notes from, most notably the laser lights! The color palette's even the same... Do you think he went to the same place as I? Or did he just buy them for cheap when they went out of business? Well, whatever the case, this attack looks like you need to dodge it by standing inside of the gaps produced by the lasers. But be careful! It looks like sometimes the laser grid tilts!... I'm not sure how that impacts the dodgability of it all, but whatever. I'm not the Devourer of Gods here. Yet. I've always wanted to be a Godslayer Inferno... Well, I'm getting ahead of myself! You still need help with this guy! Fortunately, there's only one attack left in the second phase section. Sometimes, he'll enter a portal. Another portal will appear near you, with a big blue line coming out of it! But don't worry! It says here that the line doesn't do damage. However, he will dash out of the portal by following the line! The line will try to predict your movement, so move and then stop moving when he's coming out of the portal so that he comes out in front of you instead of on top of you! Did you get that? If so, then you're all good to go! Good luck out there, champ!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 20, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false, maxWidth: 500).NeedsActivation();

            //The infernum flop plays even after having defeated dog, so it goes "hold in i was reading the inf section => oh you did it"
            HelperMessage message5 = new HelperMessage("Dog6", "...Hold on, I was reading the Infernum section.",
                "FannySob", HelperMessage.AlwaysShow, 3, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false).NeedsActivation(7);

            HelperMessage message6 = new HelperMessage("Dog7", "Okay, alright. I'm reading the right chapter now. Though, keep my previous words in mind! Maybe he might... Start using Infernum AI, or something. Do we even have that mod on right now? Oh whatever, there isn't enough time to fret about that right now. I have some tip-giving to do! Anyways kid, you should be focusing on his two phases. In case you haven't noticed, the Devourer has been flipping between his purple and blue phases for a fair bit now. While he's in those phases, he'll act differently. While he's feeling blue (Haha, get it? Feeling blue?) he'll try and stick above you, keeping an eye out for if you make any mistakes. If you go too high off the ground, he'll immediately take the opportunity to charge at you! Think of him like the Crawltipede from the Solar Pillar! When he's feeling purple (Haha, get it? Feeling purple?) he'll get sick of playing nice try to charge you down. This frenzy makes him stick lower to the ground, but don't take that as a mercy! He's still just as dangerous as he is normally, if not more! Everything after this should be for both phases. It says he breathes fire now, which is pretty dangerous! His fire Damage-Over-Time (DOT) effect, Godslayer Inferno, can pack quite the punch. That bad boy takes fifteen chunks of health a second! Steer clear of his jaws, though I feel like you're already trying to do that, haha! He will also fire barrages of lasers every once in a while, telegraphed through the big ol' laser lines I mentioned before...",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 22, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false, maxWidth: 500).NeedsActivation();

            //500 max width LITTERALLY IS NOT ENOUGH AND IT GOES OFFSCREEN
            HelperMessage message7 = new HelperMessage("Dog8", "...Okay, listen. Before we continue, I wanna say I'm sorry for reading the wrong section. I don't know if you mind or not, but I feel SUPER bad about it. I wasted a fair bit of your time by reading the wrong chapter, and while some of the stuff I said was valid, it wasn't all useful. I really hope you can forgive me and hear me out for the rest of this stuff, because I think things are getting pretty heated right about now in the fight! Anyways, where was I... Alright, here we are! He should be going invisible periodically and doing the laser grid stuff I mentioned earlier. Here's my redemption! Just remember what I said earlier and you should be fine! Other than that, it looks like there's just the Cosmic Guardians and the black background. The Guardians are pretty basic, they just follow the Devourer's movement. The black background might throw you for a loop, however. Remember what I said about the colored backgrounds? When he gets weak, he obscures the color of the background! What a nasty devil he is. It'll be up to your muscle memory to save you in that situation. This book only goes as far as to Expert mode, so keep that in mind. As to why it had Infernum, I honestly couldn't tell you. Whoever put this thing together must have been too lazy to go the extra mile and finish it up, with a nice little bow and whatnot. It's got a bit more on his history, though! He fought under Yharim's tyranny until the old tyrant got sick of tyranting. He manipulated him to do many of his evil things, like burn up the Sunken Sea town and destroy the Brimstone Crags and... Are they really blaming everything on the WORM?!? Everybody knows Yharim ordered lot of that stuff himself. This book is propaganda! Don't trust this stuff. Yharim was a cruel, disgusting man, not some soppy misunderstood heartthrob. I hope everything before this was accurate, otherwise... *gulp*",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 22, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false, maxWidth: 510).NeedsActivation();

            HelperMessage finalTip = new HelperMessage("Dog9", "I think that's all the info I can give you. Godspeed!... Or actually, FASTER than godspeed! Since he can eat Gods and all... Good luck!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !BeatDog, 5, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false).NeedsActivation();

            HelperMessage beatDogEarly = new HelperMessage("DogBeatEarly", "Oh, you did it. Good Job!",
                "FannyCryptid", (ScreenHelperSceneMetrics scene) => BeatDog && !ReadAllDogTips, 5, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false);

            HelperMessage beatDogLate = new HelperMessage("DogBeatLate", "YOU DID IT!!! I'm so proud of you!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => BeatDog && ReadAllDogTips, 5, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false);


            //First and second message have a timer of 5 seconds that is triggered when the first message is finished
            startMessage.AddEndEvent(() => message1.ActivateMessage());

            message1.AddEndEvent(() => message2.ActivateMessage());
            message2.AddEndEvent(() => message3.ActivateMessage());
            message3.AddEndEvent(() => message4.ActivateMessage());

            //Skip to the end and don't go talking about non infernum stuff if infernum is ACTUALLY enabled
            if (hasInfernum)
                message4.AddEndEvent(() => finalTip.ActivateMessage());

            //Otherwise, wait for the next infernum one
            else
                message4.AddEndEvent(() => message5.ActivateMessage());

            message5.AddEndEvent(() => message6.ActivateMessage());
            message6.AddEndEvent(() => message7.ActivateMessage());
            message7.AddEndEvent(() => finalTip.ActivateMessage());

            //Reading the final tips means that fanny has read them all
            finalTip.AddEndEvent(() => ReadAllDogTips = true);

            screenHelperMessages.Add(startMessage);
            screenHelperMessages.Add(message1);
            screenHelperMessages.Add(message2);
            screenHelperMessages.Add(message3);
            screenHelperMessages.Add(message4);
            screenHelperMessages.Add(message5);
            screenHelperMessages.Add(message6);
            screenHelperMessages.Add(message7);
            screenHelperMessages.Add(finalTip);

            screenHelperMessages.Add(beatDogEarly);
            screenHelperMessages.Add(beatDogLate);


            HelperMessage evilFannyRetort = new HelperMessage("DogRetort", "What a fucking moron. You can never get anything right, you useless fucking flicker of a flame. Kill yourself, it'll be doing the world a public service",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: false).NeedsActivation(2).SpokenByEvilFanny(true);
            message5.AddStartEvent(() => evilFannyRetort.ActivateMessage());

            screenHelperMessages.Add(evilFannyRetort);
        }

        public static bool DogSecondPhase(ScreenHelperSceneMetrics scene)
        {
            if (BeatDog)
                return false;

            int dogType = ModContent.NPCType<DevourerofGodsHead>();
            return scene.onscreenNPCs.Any(n => n.type == dogType && n.ModNPC is DevourerofGodsHead head && head.Phase2Started);
        }
    }
}