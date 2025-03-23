﻿using CalamityMod.Items.LoreItems;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        internal static readonly List<int> manuallyDefinedLoreItems = new();

        internal static int previousHoveredItem;
        internal static int hoverTime;
        internal static int previousHoverTime;
        internal static bool ReadLoreItem => previousHoverTime >= 2 * 60 && hoverTime == 0;

        public static void UpdateLoreCommentTracking()
        {
            previousHoverTime = hoverTime;
            //Reset hover time if the player changes items theyre hovering over
            if (Main.HoverItem.type != previousHoveredItem || Main.HoverItem.ModItem == null || (Main.HoverItem.ModItem is not LoreItem && !manuallyDefinedLoreItems.Contains(Main.HoverItem.type)))
                hoverTime = 0;

            //Hover time should go up if were hovering a lore item
            else if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                hoverTime++;

            //If the player stops holding shift after holding shift for long enough to have been considered as "having read the lore", set hovertime to 0 so that fanny speaks
            else if (hoverTime > 2 * 60)
                hoverTime = 0;
        }

        public static void LoadLoreComments()
        {
            #region First lore
            HelperMessage.New("LoreAny", "Woah? Did that thing just... whisper to us? You've got to collect more of these things, they seem to tell some interesting stories! Let me know if you find any more of these, and let me listen too!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem, 5, persistsThroughSaves: false).AddDelay(0.4f);

            #endregion


            #region Desert
            int desertLoreItemType = ModContent.ItemType<LoreDesertScourge>();
            HelperMessage.New("LoreDesert", "Isn't this just a classic example of the 'Circle of Life' in the sea? First, we had a humble sea serpent with a taste for the tiniest fish bites, and then BOOM! Ilmeris goes up in flames, and suddenly our sea serpent thinks it's Jaws or something. But hey, remember, even the 'big bad' gets eaten eventually, just like your grandma's famous apple pie!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == desertLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            #endregion

            #region Evil biomes
            int crimLoreItemType = ModContent.ItemType<LoreCrimson>();
            int corLoreItemType = ModContent.ItemType<LoreCorruption>();
            HelperMessage.New("LoreCorcrim", "So we've been walking around on the guts of a god this entire time? How disgusting!",
                "FannySob", (ScreenHelperSceneMetrics scene) => ReadLoreItem && (previousHoveredItem == crimLoreItemType || previousHoveredItem == corLoreItemType), 5, persistsThroughSaves: false).AddDelay(0.4f);

            #endregion

            #region Skeletron
            int sansLoreItemType = ModContent.ItemType<LoreSkeletron>();
            HelperMessage.New("LoreSkeletron", "Well, well, well! Looks like old man Jenkins made a boo-boo in the ancient cult's library. But hey, if you're into dragon-loving cults and cursed walls, this place is just oozing misguided zeal. Who needs knowledge when you've got curses, right?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == sansLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            #endregion

            #region Slimepod
            int sgLoreItemType = ModContent.ItemType<LoreSlimeGod>();
            HelperMessage.New("LoreSlimepod", "Ah, yes, the legendary Slime God! Back in the day, it was a real eco-warrior, but now it's just a slimy mess. It's like your friend who used to be a vegan and now they eat bacon-wrapped cheeseburgers. Such is the way of nature, my friend!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == sgLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            #endregion

            #region Wof
            int wofLoreItemType = ModContent.ItemType<LoreWallofFlesh>();
            HelperMessage.New("LoreWof", "Zoinks! It's like a DIY prison for a slain God, crafted with the finest foul sinew and magics that are even fouler - but hey, it did the trick! Imagine having this monstrosity in your backyard - perfect for stopping divine influence and impressing your immortal neighbors.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == wofLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            #endregion

            #region Blood Moon
            int bloodLoreItemType = ModContent.ItemType<LoreBloodMoon>();
            HelperMessage.New("LoreBlood", "Wowie! The guy writing these little blurbs must have an outdated source of information. Everyone knows Blood Moons are caused by the legendary Blood Moon Joe!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == bloodLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilBlood", "Actually, Blood Moons are caused by limited amounts of sunlight passing through the atmosphere and onto the moon you dummy.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();


            #endregion

            #region Queer slime
            int queenSlimeLoreItemType = ModContent.ItemType<LoreQueenSlime>();
            HelperMessage.New("LoreQueenSlime", "Well, it looks like the Slime God decided to pull a fashion makeover! It's the latest trend in guardian couture. But hey, they got caught in their own power trip – I guess even gods have their bad days.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == queenSlimeLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilQueenSlime", "Ugh, really? The Slime God is playing dress-up now? What's next, a slime goddess? This whole scenario is as ridiculous as my patience for it.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion

            #region Mechs
            int mechLoreItemType = ModContent.ItemType<LoreMechs>();
            HelperMessage.New("LoreMechs", "Ah, Draedon, the mad scientist with a side of whimsy! He thought he could make the perfect war machines only for them to turn into soul-powered cheerleaders. Now they're wandering around, looking for divine secrets like overenthusiastic detectives at a donut shop. Guess what? You're their new donut, so bring your A-game and make it a battle they won't forget!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == mechLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilMechs", "Ah, some of Draedon's many, MANY blunders. It's a wonder the guy didn't get fired two seconds into his job!",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();


            #endregion

            #region Cryogen
            int cryogenLoreItemType = ModContent.ItemType<LoreArchmage>();
            HelperMessage.New("LoreCryo", "It seems that our Archmage Permafrost is back from his icy vacation! Calamitas must have locked him in her magical freezer. Maybe now he can chill out and catch up on some reading about not-so-evil overlords.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == cryogenLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            HelperMessage.New("LoreEvilCryo", "This whole excerpt is like a bad breakup, with the added bonus of endless rambling about their frosty feud. Get a life, people.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();


            #endregion

            #region Aqua
            int aquaLoreItemType = ModContent.ItemType<LoreAquaticScourge>();
            HelperMessage.New("LoreAq", "Oh, look at this fancy worm who's clearly mastered the art of 'living your best life.' While the other sea monsters are out there hangry and chasing scraps, this one's chilling like a villain, sipping its tea and filter feeding like a pro. Talk about being the 'cool cucumber' of the sea world, am I right?",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == aquaLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilAq", "Microorganisms evolving rapidly? Yeah, well, I evolved rapidly too, and it didn't make me any happier about this pretentious monster's good fortune.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion

            #region Brim
            int brimLoreItemType = ModContent.ItemType<LoreBrimstoneElemental>();
            HelperMessage.New("LoreBrim", "Well, folks, looks like we've got a peculiar case of a city's silent matron taking a little too long of a nap, and when she woke up, boy, did she have some fiery morning breath! I guess the lesson here is, even ancient beings need their beauty sleep, or you might just end up with a burning desire to redecorate the whole city!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == brimLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilBrim", "So, the city's 'silent matron' decided to throw a hissy fit, and for what? A little economic boom? Please, spare me the drama. And don't get me started on that \"sick sense of humor\" nonsense. This whole tale is just a dumpster fire of clichés and poor decisions.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Calclone
            int ccloneLoreItemType = ModContent.ItemType<LoreCalamitasClone>();
            HelperMessage.New("LoreCalc", "Ah, so it was a clone! Seems like that Draedon guy didn't do too well in biology class if that's what his attempt at a clone looks like. But let's be real, who needs a witch's clone when we've got enough problems with our own rude doppelgangers.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == ccloneLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilCalc", "Kys kys kys kys kys kys kys.",
                "EvilFannyCrisped", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Plantera
            int plantLoreItemType = ModContent.ItemType<LorePlantera>();
            HelperMessage.New("LorePlant", "Oh, here we have a thrilling tale of botanic drama! The Jungle settlers were clearly overachievers when it came to gardening, and their wild soul-fed sprout sounds like the life of the rootin' tootin' garden party! But, now that it's gone, there's even more chaos than at a squirrel disco in my old elementary school!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == plantLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            HelperMessage.New("LoreEvilPlant", "Souls souls souls souls souls, good grief I hate that word now.",
                "EvilFannyMiffed", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion

            #region PBG
            int pbgLoreItemType = ModContent.ItemType<LorePlaguebringerGoliath>();
            HelperMessage.New("LorePBG", "I have the feeling this guy likes bees! What is he, an apiarist or something? It's fan-tastic to see people following their passion!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == pbgLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilPBG", "Boo hoo wahh wahhh wahhh the bees wahhh so cruel wahh wahhh wahh Draedon is heartless wahhhhh vile and despicable.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Prelude
            int cultistLoreItemType = ModContent.ItemType<LorePrelude>();
            HelperMessage.New("LorePrelude", "Oh! Oh! Looks like we're getting into the real meaty part of these little blurbs. Guess they said, \"Eh, close enough,\" when fighting that monstrosity, and locked it in the moon. Not their best day, but hey, they're dragons; they can't win 'em all!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == cultistLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilPrelude", "Yaaaaaaaaaaawn. Let's just get to more fighting already.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion

            #region Deus
            int deusLoreItemType = ModContent.ItemType<LoreAstrumDeus>();
            HelperMessage.New("LoreDeus", "This big, celestial dude is like the night sky's celebrity chef, chomping on stars and spitting out new ones. But here's the kicker – it's not your typical god, no siree! It got infected by some space cooties, and now it's throwing tantrums in the universe. Who knew being a star chef could be so complicated, am I right?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == deusLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilDeus", "Wait so is DEUS, like the latin word for \"god\", a god or not!?",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();


            #endregion

            #region Requiem
            int moonLoreItemType = ModContent.ItemType<LoreRequiem>();
            HelperMessage.New("LoreRequiem", "Well, folks, it seems that our Light Dragon buddy met a monk who was clearly binge-watching too much reality TV! They pulled a \"Name Change Reveal\" worthy of a season finale and declared themselves the First God. Talk about a plot twist, am I right? And apparently, becoming a god is all the rage these days; who knew being a deity was such a trend?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == moonLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilRequiem", "There's a fine line between godhood and delusion, and all these idiots strolling around as gods sound like they fall more into the latter than the former.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Burb
            int birbLoreItemType = ModContent.ItemType<LoreDragonfolly>();
            HelperMessage.New("LoreBirb", "Ah, the Draconic Era, a time of legends, mythical creatures, and probably some pretty epic dragon family reunions! It's a shame that it's all over though.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == birbLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilBirb", "I'm going to puke if I hear the word \"dragon\" one more time.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Provi
            int proviLoreItemType = ModContent.ItemType<LoreProvidence>();
            HelperMessage.New("LoreProv", "A glorious day indeed! Deeds of valor, like finding the last piece of chocolate in the fridge, are truly legendary. I mean, I once opened the fridge and discovered half an onion. What a thrilling tale, right?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == proviLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilProv", "A \"glorious day\"? More like a day wasted on pompous ramblings.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Polt
            int poltLoreItemType = ModContent.ItemType<LorePolterghast>();
            HelperMessage.New("LorePolter", "It seems the writer here was a master of procrastination! They managed to turn an entire dungeon into a haunted house attraction, complete with a formless monster, and all because they were too lazy to get their act together. Talk about taking \"ghosting\" to a whole new level!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == poltLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilPolter", "What a pathetic excuse for a leader. They're so wrapped up in their own little world of self-pity that they can't even be bothered to deal with the mess they created.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Dog...
            int dogLoreItemType = ModContent.ItemType<LoreDevourerofGods>();
            HelperMessage.New("LoreDog", "It seems like the author here had quite a \"bite\" of an issue with this Devourer character. They got a mouthful of manipulation and a side of negligence! Talk about a bad takeout order; maybe next time, they should try a different restaurant – or universe!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == dogLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            HelperMessage.New("LoreEvilDog", "So this power-hungry idiot recruited a snake-tongued disaster and then acted all surprised when everything went to the abyss!?",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion

            #region Yharon
            int yharonLoreItemType = ModContent.ItemType<LoreYharon>();
           HelperMessage.New("LoreYharon", "Well, folks, here we have a dramatic tale of dragons and destiny. I mean, who needs destiny, right? But, hey, at least the lava bath spa treatment gave our hero a new buddy. Friendship and fiery scars, what more could you ask for?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == yharonLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

            HelperMessage.New("LoreEvilYharon", "Ugh, another one of these melodramatic dragon stories. Who cares about Yharon and his rebirth nonsense? And seriously, \"destiny is for the weak\"? Spare me the hero's whining. It's all just a bunch of hot air, or in this case, hot lava.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion

            #region Draedon
            int draeLoreItemType = ModContent.ItemType<LoreExoMechs>();
            HelperMessage.New("LoreDraeodn", "Well, well, here we are admiring Draedon's doodads of destruction. I hear he claims his gizmos are better than any divine doohickeys, but I must say, he's never met my toaster-oven! At least that thing makes a mean grilled cheese sandwich.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == draeLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilDraedon", "Oh, Draedon and his soulless contraptions, what a joy. His boasts are as insufferable as the cacophony of a million malfunctioning alarm clocks. And if you think you can \"leverage\" his knowledge, you're as delusional as he is.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();
            #endregion

            #region Scal...
            int scalLoreItemType = ModContent.ItemType<LoreCalamitas>();
            HelperMessage.New("LoreScal", "Oh, Calamitas, the Brimstone Witch! She's like that spicy chili you thought was mild but then it lit your mouth on fire! I mean, I could've used her as a barbecue grill with all that brimstone and wrath. Poor girl, she really should've considered anger management classes!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == scalLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);


            HelperMessage.New("LoreEvilScal", "Of course, she's got a sob story, 'unfathomable, raw power,' give me a break! Sounds like a one-woman wrecking crew who can't handle her own destruction. Good riddance!",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion


            #region Epilogue
            int cynosureLoreItemType = ModContent.ItemType<LoreCynosure>();
            HelperMessage.New("LoreEnd", "So, you're like, the all-powerful ruler of Terraria now? Don't get too cocky, hero. I once had a pet rock that thought it was the king of the backyard. It didn't end well for him. But hey, if you're ready to challenge this guy, go find that Light's grave thingy on top of Dragon Mountain. Just watch out for falling stars, they're your competition now!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => ReadLoreItem && previousHoveredItem == cynosureLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

           HelperMessage.New("LoreEvilEnd", "Is there even a \"Dragon's Aerie\"? You know, after listening to all of these scraps, it seems like whoever is writing these is compensating for actual adventure and substance with chicken scratch they wrote up in a minute tops. Go on, I dare you to go to this supposed aerie.",
                "EvilFannyMiffed", HelperMessage.AlwaysShow, 5, persistsThroughSaves: false)
                .ChainAfter(delay: 1.5f, startTimerOnMessageSpoken: true).SpokenByEvilFanny();

            #endregion
            /* #region Does this boss even exist
             int oldLoreItemType = ModContent.ItemType<LoreOldDuke>();
             FannyMessage oldLore = new FannyMessage("LoreOld", "Oh my flames, what in the world is an 'Old Duke'? Sounds like someone's been fishing for ideas! X) I mean, we've already got Duke Fishron, right? Why go for a fishy sequel? Maybe it's just a fish tale that got a bit too old!",
                 "FannyIdle", (FannySceneMetrics scene) => ReadLoreItem && previousHoveredItem == deusLoreItemType, 5, persistsThroughSaves: false).AddDelay(0.4f);

             fannyMessages.Add(oldLore);

             FannyMessage oldEvilLore = new FannyMessage("LoreEvilOld", "Oh, for the love of flames, Fanny! You really are the dimmest ember in the fire, aren't you? This 'Old Duke' sounds like a poorly cooked idea, if you ask me. Who needs another Fishron boss? They should've called it 'Fishron: The Re-Hashed Edition.",
                 "EvilFannyIdle", FannyMessage.AlwaysShow, 5, persistsThroughSaves: false)
                 .NeedsActivation(3).SpokenByEvilFanny();

             oldLore.AddStartEvent(() => oldEvilLore.ActivateMessage());

             fannyMessages.Add(oldEvilLore);
             #endregion*/
        }

    }
}