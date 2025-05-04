using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Dyes;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Weapons;
using CalRemix.UI.Logs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadItemMessages()
        {
            #region Item
            //Fanny
            HelperMessage.New("Forge", "Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!",
                "FannyNuhuh", HasDraedonForgeMaterialsButNoMeat, onlyPlayOnce: false, cooldown: 120).AddItemDisplay(ModContent.ItemType<DeliciousMeat>());

            HelperMessage.New("DeliciousMeat", "Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.", 
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>())).AddItemDisplay(ModContent.ItemType<DeliciousMeat>());

            HelperMessage.New("Relocator", "Wow! You crafted a Normality Relocator! with a press of a button, unyielding discord is at your fingertips!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>())).AddItemDisplay(ModContent.ItemType<NormalityRelocator>());

            HelperMessage.New("BunnyMurder", "...",
                "FannyCryptid", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.BunnyBanner), 5, cantBeClickedOff: true).AddItemDisplay(ItemID.BunnyBanner);

            HelperMessage.New("BunnyVolcano", "One time, when I was making a volcano project for the local science fair, I saw the craziest thing! I could not believe my eyes when I saw what happened to that poor hamster after it ate the baking soda and vinegar I'd left out for the experiment. The little guy popped more forcefully than the one I gave coke and mentos to! Explosive bunnies are one thing, but explosive hamsters are truly a new revelation worthy of the new age!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.ExplosiveBunny) || Main.LocalPlayer.HasItem(ItemID.BunnyCannon)).AddItemDisplay(ItemID.ExplosiveBunny);

            HelperMessage.New("Diamond", "Did you know that the largest diamond ever found was 3106.75 carats?",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.Diamond)).AddItemDisplay(ItemID.Diamond).SetHoverTextOverride("Talk about a juicy gemerald!");

            HelperMessage.New("Terraspark", "New shoes!!!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.TerrasparkBoots)).AddItemDisplay(ItemID.TerrasparkBoots);

            HelperMessage.New("RoxcaliburShimmer", "Woah, something about that doesn't seem right! I'll take those off your hands- wouldn't wanna skip stuff, would you?",
                "FannyNuhuh", HasRoxcaliburMaterials).AddItemDisplay(ModContent.ItemType<Roxcalibur>()).AddStartEvent(TakeRoxcaliburStuff);
            //Add a condition to this one YUH, to pass the test of knowledge...
            //YUH YUH YUH YUH YUH
            //IBAN IBAN IBAN IBAN IBAN
            HelperMessage.New("DesertScourge", "I see you've gotten some mandibles. For some reason, people always try to make medallions out of them when the only way to get them is by killing Cnidrions after the destruction of the legendary Wulfrum Excavator. Strangely specific isn't it? Guess that's just how the cookie crumbles!",
                "FannyNuhuh", HasDesertMedallionMaterials).AddItemDisplay(ModContent.ItemType<DesertMedallion>());

            HelperMessage.New("VoodooDoll", "Cool doll you have! I think that it will be even cooler when in lava!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.GuideVoodooDoll));

            HelperMessage.New("TwentyTwo", "I love 22. My banner now.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.HornetBanner)).AddItemDisplay(ItemID.HornetBanner).AddStartEvent(() => Main.LocalPlayer.ConsumeItem(ItemID.HornetBanner)).SetHoverTextOverride("Thanks Fanny! That was cluttering my inventory!");

            HelperMessage.New("Shadowspec", "Please throw this thing out, it will delete your world if you have it in inventory for too long!",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ShadowspecBar>())).AddItemDisplay(ModContent.ItemType<ShadowspecBar>()).SetHoverTextOverride("Thank you for the help Fanny! I will!");

            HelperMessage.New("Wood", "Wood? Yummy!",
              "FannyAwe", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.Wood));

            HelperMessage.New("HallowedBar", "What you hold now is a bar of extraordinary power infused with the essence of Heaven itself! That's a biome right?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.HallowedBar)).SetHoverTextOverride("It sure is Fanny, it sure is.");

            HelperMessage.New("LifeCrystal", "Ah, digging up life crystals, are we? Remember, a crystal a day keeps the.. uhh... enemies away! See, I'm good with rhymes!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.LifeCrystal));

            HelperMessage.New("Jump", "Did you know? You can press the \"space\" button to jump!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PlatinumCoin)).SetHoverTextOverride("Thanks Fanny! You're so helpful!");

            HelperMessage.New("TitanHeart", "You got a heart from a titan! Place it on the tower for a wacky light show!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TitanHeart>())).AddItemDisplay(ModContent.ItemType<TitanHeart>());

            HelperMessage.New("BloodyVein", "The Bloody Vein is an item of utmost importance which can be inserted into various altars and machinery for wacky results. How about inserting one into one of those lab hologram box things?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<BloodyVein>())).AddItemDisplay(ModContent.ItemType<BloodyVein>());

            HelperMessage.New("RottenEye", "The Rotting Eyeball is an item of zero importance. The Bloody Vein from the Crimson's Perforators is way better!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<RottingEyeball>()) && !WorldGen.crimson).AddItemDisplay(ModContent.ItemType<RottingEyeball>()).SetHoverTextOverride("Thanks Fanny! I'll be sure to make a Crimson world next time.");

            HelperMessage.New("AlloyBar", "Congratulations, you have obtained the final bar for this stage of your adventure. You should attempt making some Alloy Bars, a versatile material made of every available bar which can be used for powerful items.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<AlloyBar>())).AddItemDisplay(ModContent.ItemType<AlloyBar>());

            HelperMessage.New("Sponge", "Oh, is that a Sponge? Maybe avoid using it. I've heard something about the wielder dying, or something...",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TheSponge>())).AddItemDisplay(ModContent.ItemType<TheSponge>());

            HelperMessage.New("Garbo", "Wowie! That scrap there is useless!",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.OldShoe) || Main.LocalPlayer.HasItem(ItemID.FishingSeaweed) || Main.LocalPlayer.HasItem(ItemID.TinCan) || Main.LocalPlayer.HasItem(ItemID.JojaCola)).AddItemDisplay(ItemID.TrashCan).SetHoverTextOverride("Thanks Fanny! I already wanted to cook it.");

            HelperMessage.New("Nightfuel", "Nightmare Fuel, huh? ...you know, maybe if you can harvest enough of it, maybe those Pumpkings will stop terorrizing our inhabitants and they'll be permanently more happy!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<NightmareFuel>())).AddItemDisplay(ModContent.ItemType<NightmareFuel>());

            HelperMessage.New("Endenergy", "Ooh, is that Endothermic Energy? If we can get a decent supply of it, I think those Ice Queens will fear us and our residents might be forever grateful with us!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<EndothermicEnergy>())).AddItemDisplay(ModContent.ItemType<EndothermicEnergy>());

            HelperMessage.New("Darksunfrag", "What's that? Darksun Fragment? Do you think with enough of it, our world will be permanently lit up like a lemon-scented candle flame?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DarksunFragment>())).AddItemDisplay(ModContent.ItemType<DarksunFragment>());

            HelperMessage.New("Onion", "I'd be weary about eating that strange plant. You can only get one, so it might be useful to hang on to it for later.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<CelestialOnion>()) && Main.LocalPlayer.Remix().miracleUnlocked).AddItemDisplay(ModContent.ItemType<CelestialOnion>());

            HelperMessage.New("CleavageFurrow", "One time, I went to the pond on a bright summer afternoon! One of the frogs leaped from its lily pad and locked eyes with me. I believe it had a cleavage furrow? Oh, what a dopey little smile that fella had!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<UniverseSplitter>())).SetHoverTextOverride("Very cool, Fanny!");

            HelperMessage.New("PetRock", "Oh hey, is that my pet rock? I lost it in my backyard a few years back. I’ve been trying to find it since!",
                "FannyAwe", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Rock>())).AddItemDisplay(ModContent.ItemType<Rock>());

            HelperMessage.New("RockLobster1", "Oh, hot tamales! You've gone and done it! You've reeled in a lobster, my friend! Can you believe it? A rock lobster, right here in the oasis! I mean, who would've thought? Rock Lobsters are like the rockstars of the desert, and you just snagged one. You're a fishing master, my friend! But hey, let me tell you a tale about lobsters that'll have you crackling with laughter. So there I was, with my good pal Dron, you know. We decided to hit up this fancy lobster restaurant downtown. Now, you might be wondering, \"Fanny, why would a flame like you even eat lobster?\" Well, my friend, curiosity burns bright, and I figured it was worth a shot. We waltz into this posh place, me flickering with excitement, Dron rolling in like a boss. We get seated, and the waiter hands us these bibs – you know, the ones with the goofy lobster design? I, of course, couldn't wear one, being made of flame and all, but Dron struggled a bit. Picture it: Dron, bib askew, trying to maneuver it with no arms. Hilarious, right? Now, we dive into the menu. Lobster this, lobster that – it was like a seafood carnival! Dron's eyes were wide, no arms to shield them from the sea of options. We decided to go for the lobster feast, the whole shebang. And let me tell you, it was a feast fit for kings! But here comes the twist! The waiter brings out this succulent lobster dish, steam rising like the flames on my head. Dron takes a bite, a hearty one, and suddenly, his face turns redder than a ripe tomato. Turns out, he's allergic to shellfish! Who would've thought? Poor Dron, armless and allergic – the universe sure has a sense of humor. Which reminds me of the spicy saga of my friend Green Demon and his salsa extravaganza. Now, Green Demon is quite the character – always pushing the boundaries of fiery flavors, forever on a quest to grow the spiciest peppers in our little corner of the world. One day, the sun was blazing overhead, and Green Demon excitedly invited our motley crew – me, La Ruga, Ogscule, Tim, Cnidrion, and Pyrogen – for a salsa fiesta at his fiery abode. Oh, the anticipation was palpable! I flickered with excitement, eager to see what kind of fiery concoction he had in store for us. As we approached, the scent of peppers wafted through the air like a zesty dance. Rows upon rows of vibrant, fiery red and green peppers swayed in the breeze, basking in the sun's warm embrace. Green Demon, with his devilish grin, welcomed us to his spicy paradise. \"Behold, my friends! The harvest of the spiciest peppers in the land!\" he declared, his eyes gleaming with mischievous delight. We gathered around as Green Demon plucked peppers with such finesse, it was like he was orchestrating a spicy symphony. With a basket brimming with peppers, he led us to his fiery kitchen, a cauldron bubbling away with a mysterious potion – or rather, his special salsa. The kitchen was alive with the rhythmic chopping of peppers and the sizzling melody of ingredients harmonizing in the pan. I, being a flame myself, felt right at home amidst the culinary inferno. Green Demon's hands moved with the precision of a seasoned chef, his eyes gleaming with the promise of a taste explosion. Now, my friends, you must understand – this salsa wasn't just your average dip. It was a potion of pure heat, a symphony of spices that would make even the bravest tongues tremble. Green Demon was a maestro, and his salsa was his fiery masterpiece. As the salsa simmered, the aroma grew more intense. It was like a spicy enchantment had taken over the kitchen. We were all eagerly awaiting the taste test, our excitement building like a rising flame. Finally, the moment of truth arrived. Green Demon scooped up a generous amount of salsa and handed us each a tortilla chip. We stared at each other, eyes wide with anticipation, and took a bite simultaneously. Flames! The heat hit us like a spicy meteor shower.",
                "FannyBigGrin", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.RockLobster), maxWidth: 1000, fontSize: 0.8f).AddItemDisplay(ItemID.RockLobster);

            HelperMessage.New("RockLobster2", "Laughter erupted as we danced around, fanning our tongues with imaginary flames. Even I, a flame myself, felt the burn in the most delightful way. Green Demon stood there, triumphant, his devilish grin widening. \"What did I tell you, my friends? The spiciest salsa in the realm!\" But my pal La Ruga took a cautious bite, realizing it was the perfect companion to his daily tea sessions. Oh La Ruga. Every Tuesday, without fail, we embark on a literary adventure to our favorite place in the world – the cozy little bookstore at the corner of the Exosphere. We strut into the bookstore like we own the place, La Ruga and me. Well, La Ruga doesn't really strut; more like gracefully scuttles with its long legs. I'm there, flickering and crackling, like a flame on a mission – a mission for some good reads. The moment we step inside, the scent of old paper and ink wafts through the air. It's like the very essence of knowledge, a fragrance that fuels my fiery enthusiasm. La Ruga seems to breathe it all in, its shadowy arms flickering with delight. Now, you might wonder, \"Fanny, why a bookstore?\" Well, let me tell you, there's something magical about flipping through the pages of a good book. The way the words dance off the paper, creating a world only limited by the imagination. It's a burning passion of mine, and La Ruga, with its ancient wisdom, appreciates the art of storytelling too. We start our journey through the aisles, and La Ruga guides me to the classics – timeless tales of adventure, romance, and mystery. Picture this: a flame flickering next to a T⍑ᒷ rᒷᓵꖌ𝙹リ, engrossed in the ancient wisdom of literature. If anyone happened to stroll by, they'd probably think they'd stumbled into a fantastical scene from a storybook. But, oh, the joy of finding a hidden gem! We once stumbled upon a dusty old tome, forgotten by time. It had a worn cover, and the pages whispered secrets of a bygone era. La Ruga and I exchanged excited glances – this was a treasure trove waiting to be explored! We huddled in a cozy reading nook, La Ruga spreading its legs out, making a snug spot for me to flicker comfortably. As I read aloud, the words came alive, creating vivid images in our minds. The world outside faded away, and it was just me, La Ruga, and the enchanting tales within those pages. Of course, being a flame, I have to be careful not to singe the pages. La Ruga occasionally gives me a gentle nudge if I get too close, a silent reminder to keep my flames in check. We've developed quite the teamwork, I must say. Now, I pick out a few books at the bookstore, and head to the park for some extra reading. Usually, my friend Tim joins me at the park, but this time, my other friend Ogscule decided to join me instead.  Now picture this: a sunny day at the park, the trees swaying gently, and the air filled with the delightful scent of nature. I'm there, Fanny the Flame, and by my side is none other than my trusty friend Ogscule. We come across a patch of grass, and Ogscule suggests we feed the birds. Now, considering that Ogscule, like Dron, doesn't have hands – or any appendages, for that matter – I'm intrigued. How on earth is this going to work? But I'm all for adventure, so we decide to give it a shot. Ogscule plops down on the grass, and I hover beside him, flames flickering in anticipation. We've got a bag of bird feed, and Ogscule looks at me with those fleshy eyes, ready for action. Without arms, he leans toward the bag, trying to nudge it open with the tip of his prongs. It's like watching a determined utensil on a mission. Meanwhile, I'm providing some fiery commentary, encouraging Ogscule like a cheerleader at a sports game. \"Go, Ogscule, you got this! Show those birds your fleshy finesse!\" I can't help but chuckle at the absurdity of the situation. We're the dynamic duo of the park, making memories that'll have the birds tweeting about us for days. Finally, after a bit of a struggle, Ogscule manages to spill some bird feed on the ground. The birds, sensing a feast, swoop down like they've just won the avian lottery. But here's the twist – since Ogscule doesn't have hands, the birds are pecking at the bird feed, and Ogscule's prongs. It's a hilarious sight, a bizarre dance of feathers and fork. I'm laughing so hard; my flames are dancing with delight. Ogscule doesn't seem to mind; he's embracing the chaos, embracing the absurdity of our friendship. We may not be the most conventional pair, but we sure know how to turn a simple day at the park into a sidesplitting comedy. Life with these friends is like a colorful cartoon, each day filled with laughter and unexpected twists. So here's to more adventures, more lobsters, and more tales to share. Thanks for being with me on this fiery journey, my friend!",
                "FannyBigGrin", HelperMessage.AlwaysShow, maxWidth: 1000, fontSize: 0.8f).ChainAfter();

            HelperMessage.New("Dyes", "I've always thought dyes were a bit worthless. I mean come on! True beauty comes from the inside! No need to pretty yourself up on the outside! So my solution to this was making it so that all dyes now make you stronger! Have fun coming up with dye combos! Or just equipping the same dye in every slot, that works too.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.inventory.Any((Item i) => i.dye > 0)).AddItemDisplay(ModContent.ItemType<ElementalDye>());

            HelperMessage.New("Terminuts", "Oh! You found that moonstone tablet I scribbled on a while ago! I think one of the worms down here got a little furious when I did that, so I gave a replica of before I wrote on it to an old pal of mine to get em off my tail. You should be able to find him down here somewhere if you look for long enough...",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<FannyLogAbyss>()) && Main.LocalPlayer.Calamity().ZoneAbyssLayer4).AddItemDisplay(ModContent.ItemType<Terminus>());

            HelperMessage.New("SuperiorHealing", "It looks like you're starting to run low on health! Thankfully, I have a little trick up my sleeve for situations like this...",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 * 0.33f && Main.LocalPlayer.inventory.Any(i => GoodHealingPots.Contains(i.type)));

            HelperMessage.New("SuperiorHealing2", "There you go, here's a little boost! This is why I always carry my trusty potions!",
                "FannyIdle", HelperMessage.AlwaysShow).ChainAfter().AddStartEvent(FannyHeal);

            //Evil Fanny
            HelperMessage.New("EvilMinions", "Oh, summoner, how nice. I want to ask this in the most genuine way I can, do you play videogames for fun? Did you open up a terraria world and genuinely go \"Oh boy! Let's play summoner! I'm going to have so much fun!\"? No!!! You didn't!!! Half of your minions have braindead AI because you're playing Calamity!!! Just play any other class, man. You make me sad.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ActiveItem().DamageType == DamageClass.Summon && Main.LocalPlayer.numMinions >= 10).SpokenByEvilFanny();

            HelperMessage.New("EvilTerraBlade", "Oh, congratulations, you managed to get a Terra Blade. I'm sure you're feeling all proud and accomplished now. But hey, don't strain yourself patting your own back too hard. It's just a sword, after all. Now, go on, swing it around like the hero you think you are.",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.TerraBlade)).SpokenByEvilFanny().AddItemDisplay(ItemID.TerraBlade);

            HelperMessage sama = HelperMessage.New("MurasamaBig", "Oh, congratulations, you managed to get a Terra Blade. I'm sure you're feeling all proud and accomplished now. But hey, don't strain yourself patting your own back too hard. You're gonna be doing this \"big sword crafting tree\" thing a lot from here on out. Hope piggy here likes their slop!",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.controlUseItem && Main.LocalPlayer.ActiveItem().type == ModContent.ItemType<Murasama>() && DownedBossSystem.downedDoG && fannyTimesFrozen <= 0).SpokenByEvilFanny().InitiateConversation();
            HelperMessage mgra = HelperMessage.New("Muracrimsona1", "Lmao this item is \"perfectly balanced\" am i right?? Standig here i realize like mgr",
                "CrimSonDefault", HelperMessage.AlwaysShow).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(sama, 5, true);
            HelperMessage.New("Muracrimsona2", "SHUT THE FUCK UP!! I FUCKING HATE YOU YOU PIECE OF SHIT I HATE YOU!!!",
                "EvilFannyCrisped", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(mgra, 3, true).EndConversation();

            #endregion

            #region CrossMod
            HelperMessage.New("DadPainting", "Oh hey, you found a photo of my father! I thought I had lost it forever, turns out, I just happened to have misplaced it in one of the many chests in your world! Thank you for finding it!",
               "FannyAwe", (ScreenHelperSceneMetrics scene) => CalRemixHelper.HasCrossModItem(Main.LocalPlayer, CalRemixAddon.Wrath, "Xenqiterthralyensyr"));

            HelperMessage.New("Catharsis", "Don’t exhume Kaleidoscope! Catharsis is known to cause clinical depression in users.",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<WITCH>() && Main.LocalPlayer.TalkNPC == n) && ModLoader.HasMod("CatalystMod") && Main.LocalPlayer.HasItem(ItemID.RainbowWhip));

            HelperMessage.New("ThoriumOre", "Hey, take a look at this blueish-greenish-yellow metal! Isn't it cool? It's called Thorium! That would be a great name for a mod! Thankfully, we're not in a game, so I can use it freely!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CalRemixHelper.HasCrossModItem(Main.LocalPlayer, "ThoriumMod", "ThoriumOre"));

            HelperMessage.New("OcramSkull", "Woah! That skull looks a bit.. off! I feel like i've seen it before, a long time ago. I'm pretty sure it summons a really big bad guy! Are you sure you can take him? (He's really big, and bad!)",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CalRemixHelper.HasCrossModItem(Main.LocalPlayer, "Consolaria", "SuspiciousLookingSkull"));

            #endregion

            #region References

            /*
            fannyMessages.Add(new FannyMessage("PortalGun", "Cave Johnson here. We're fresh out of combustible lemons, but let me tell you a little bit about this thing here. These portals are only designed to stick on planetoid rock and not much else. Hope you've got a test chamber lying around that's full of that stuff!",
                "FannyIdle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PortalGun)));
             */

            HelperMessage.New("BloodGod", "I remember the Blood God, he was very friendly! Too bad he was kicked back into something called \"FertyKay\".",
                "FannySob", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ChaliceOfTheBloodGod>()));

            HelperMessage.New("Murasama", "Erm, holy crap? $0? Is that a reference to my FAVORITE game of all time, Final Fantasy? Did you know that calamity adds a custom boss health boss bar and many othe-",
               "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Murasama>())).AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("Ultrakill", "Oh EM GEE! A gun from the hit first-person shooter game, \'MURDERDEATH\'!? Try throwing out some coins and hitting them with a Titanium Railgun to pull a sick railcoin maneuver!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<MidasPrime>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<CrackshotColt>())).AddItemDisplay(ModContent.ItemType<MidasPrime>());

            HelperMessage.New("DankSouls", "What the scallop? Is that from one of the games by ToHardware?! One time, Evil Fanny was happy to give me a set of all the ToHardware games and wanted to invite our friends to watch me. Their eyes and mouths were wide open watching me beat every boss on the first try in all the games. They must've been proud!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.hardMode && Main.LocalPlayer.inventory.Any(i => DankSoulsItems.Contains(i.type)));

            HelperMessage.New("Tofu", "Uh oh! Looks like one of your items is a reference to a smelly old game franchise known as Touhou! Do your ol\' pal Fanny a good deed and put it away.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.inventory.Any(i => TofuItems.Contains(i.type))).SetHoverTextOverride("Anything for you Fanny!");
            #endregion

        }
        private static readonly List<int> DankSoulsItems = new List<int>
        {
            ModContent.ItemType<Karasawa>(),
            ModContent.ItemType<GreatswordofJudgement>(),
            ModContent.ItemType<LifehuntScythe>(),
            ModContent.ItemType<Nadir>(),
            ModContent.ItemType<StormRuler>(),
            ModContent.ItemType<TheFirstShadowflame>(),
            ModContent.ItemType<DefiledFlameDye>(),
            ModContent.ItemType<RuneofKos>()
        };
        private static readonly List<int> TofuItems = new List<int>
        {
            ModContent.ItemType<ScarletDevil>(),
            ModContent.ItemType<GlacialEmbrace>(),
            ModContent.ItemType<RecitationoftheBeast>(),
            ModContent.ItemType<EventHorizon>(),
            ModContent.ItemType<HermitsBoxofOneHundredMedicines>(),
            ModContent.ItemType<PristineFury>(),
            ModContent.ItemType<DarkSpark>(),
            ModContent.ItemType<ResurrectionButterfly>(),
            ModContent.ItemType<FantasyTalisman>(),
            ModContent.ItemType<HellsSun>(),
            ModContent.ItemType<TheDreamingGhost>()
        };
        private static readonly List<int> GoodHealingPots = new List<int>
        {
            ItemID.SuperHealingPotion,
            ModContent.ItemType<SupremeHealingPotion>(),
            ModContent.ItemType<OmegaHealingPotion>()
        };
        private static void TakeRoxcaliburStuff()
        {
            if (Main.LocalPlayer.HasItem(ItemID.SoulofNight))
            {
                int count = Main.LocalPlayer.CountItem(ItemID.SoulofNight);
                for (int i = 0; i < count; i++)
                {
                    Main.LocalPlayer.ConsumeItem(ItemID.SoulofNight);
                }
            }
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<EssenceofHavoc>()))
            {
                int count = Main.LocalPlayer.CountItem(ModContent.ItemType<EssenceofHavoc>());
                for (int i = 0; i < count; i++)
                {
                    Main.LocalPlayer.ConsumeItem(ModContent.ItemType<EssenceofHavoc>());
                }
            }
        }
        private static void FannyHeal()
        {
            Main.LocalPlayer.Heal(50);
            Main.LocalPlayer.AddBuff(BuffID.PotionSickness, 3600);
        }
    }
}