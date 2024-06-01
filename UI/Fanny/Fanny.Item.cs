using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
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
using CalRemix.Items.Materials;
using CalRemix.Items.Weapons;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadItemMessages()
        {
            #region Item
            fannyMessages.Add(new FannyMessage("Forge", "Na Na Na! The big robotic forge needs a lot of blue meat from the ads! It cannot work without it!",
                "Nuhuh", HasDraedonForgeMaterialsButNoMeat, onlyPlayOnce: false, cooldown: 120).AddItemDisplay(ModContent.ItemType<DeliciousMeat>()));

            fannyMessages.Add(new FannyMessage("DeliciousMeat", "Oooh! Delicious Meat! Collect as much as you can, it will save you a lot of time.", "Awooga",
                (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DeliciousMeat>())).AddItemDisplay(ModContent.ItemType<DeliciousMeat>()));

            fannyMessages.Add(new FannyMessage("Relocator", "Wow! You crafted a Normality Relocator! with a press of a button, unyielding discord is at your fingertips!", "Idle",
                (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>())).AddItemDisplay(ModContent.ItemType<NormalityRelocator>()));

            fannyMessages.Add(new FannyMessage("BunnyMurder", "...",
                "Cryptid", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.BunnyBanner), 5, needsToBeClickedOff: false).AddItemDisplay(ItemID.BunnyBanner));

            fannyMessages.Add(new FannyMessage("Diamond", "Did you know that the largest diamond ever found was 3106.75 carats?",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.Diamond)).AddItemDisplay(ItemID.Diamond).SetHoverTextOverride("Talk about a juicy gemerald!"));

            fannyMessages.Add(new FannyMessage("Terraspark", "New shoes!!!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.TerrasparkBoots)).AddItemDisplay(ItemID.TerrasparkBoots));

            fannyMessages.Add(new FannyMessage("RoxcaliburShimmer", "Woah, something about that doesn't seem right! I'll take those off your hands- wouldn't wanna skip stuff, would you?",
                "Nuhuh", HasRoxcaliburMaterials).AddItemDisplay(ModContent.ItemType<Roxcalibur>()).AddStartEvent(TakeRoxcaliburStuff));
            //Add a condition to this one YUH, to pass the test of knowledge...
            //YUH YUH YUH YUH YUH
            //IBAN IBAN IBAN IBAN IBAN
            fannyMessages.Add(new FannyMessage("DesertScourge", "I see you've gotten some mandibles. For some reason, people always try to make medallions out of them when the only way to get them is by killing Cnidrions after the destruction of the legendary Wulfrum Excavator. Strangely specific isn't it? Guess that's just how the cookie crumbles!",
                "Nuhuh", HasDesertMedallionMaterials).AddItemDisplay(ModContent.ItemType<DesertMedallion>()));

            fannyMessages.Add(new FannyMessage("VoodooDoll", "Cool doll you have! I think that it will be even cooler when in lava!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.GuideVoodooDoll)));

            fannyMessages.Add(new FannyMessage("TwentyTwo", "I love 22. My banner now.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.HornetBanner)).AddItemDisplay(ItemID.HornetBanner).AddStartEvent(() => Main.LocalPlayer.ConsumeItem(ItemID.HornetBanner)).SetHoverTextOverride("Thanks Fanny! That was cluttering my inventory!"));

            fannyMessages.Add(new FannyMessage("Shadowspec", "Please throw this thing out, it will delete your world if you have it in inventory for too long!",
                "Sob", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<ShadowspecBar>())).AddItemDisplay(ModContent.ItemType<ShadowspecBar>()).SetHoverTextOverride("Thank you for the help Fanny! I will!"));

            fannyMessages.Add(new FannyMessage("Wood", "Wood? Yummy!",
              "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.Wood)));

            fannyMessages.Add(new FannyMessage("HallowedBar", "What you hold now is a bar of extraordinary power infused with the essence of Heaven itself! That's a biome right?",
               "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.HallowedBar)).SetHoverTextOverride("It sure is Fanny, it sure is."));

            fannyMessages.Add(new FannyMessage("LifeCrystal", "Ah, digging up life crystals, are we? Remember, a crystal a day keeps the.. uhh... enemies away! See, I'm good with rhymes!",
               "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.LifeCrystal)));

            fannyMessages.Add(new FannyMessage("YharimBar", "Is that a Yharim Bar? You'll need a lot of them for various recipes!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<YharimBar>())).AddItemDisplay(ModContent.ItemType<YharimBar>()));

            fannyMessages.Add(new FannyMessage("Jump", "Did you know? You can press the \"space\" button to jump!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PlatinumCoin)).SetHoverTextOverride("Thanks Fanny! You're so helpful!"));

            fannyMessages.Add(new FannyMessage("TitanHeart", "You got a heart from a titan! Place it on the tower for a wacky light show!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TitanHeart>())).AddItemDisplay(ModContent.ItemType<TitanHeart>()));

            fannyMessages.Add(new FannyMessage("BloodyVein", "The Bloody Vein is an item of utmost importance which can be inserted into various altars and machinery for wacky results. How about inserting one into one of those lab hologram box things?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<BloodyVein>())).AddItemDisplay(ModContent.ItemType<BloodyVein>()));

            fannyMessages.Add(new FannyMessage("RottenEye", "The Rotting Eyeball is an item of zero importance. The Bloody Vein from the Crimson's Perforators is way better!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<RottingEyeball>()) && !WorldGen.crimson).AddItemDisplay(ModContent.ItemType<RottingEyeball>()).SetHoverTextOverride("Thanks Fanny! I'll be sure to make a Crimson world next time."));

            fannyMessages.Add(new FannyMessage("AlloyBar", "Congratulations, you have obtained the final bar for this stage of your adventure. You should attempt making some Alloy Bars, a versatile material made of every available bar which can be used for powerful items.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<AlloyBar>())).AddItemDisplay(ModContent.ItemType<AlloyBar>()));

            fannyMessages.Add(new FannyMessage("Sponge", "Oh, is that a Sponge? Maybe avoid using it. I've heard something about the wielder dying, or something...",
               "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<TheSponge>())).AddItemDisplay(ModContent.ItemType<TheSponge>()));

            fannyMessages.Add(new FannyMessage("Garbo", "Wowie! That scrap there is useless!",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.OldShoe) || Main.LocalPlayer.HasItem(ItemID.FishingSeaweed) || Main.LocalPlayer.HasItem(ItemID.TinCan) || Main.LocalPlayer.HasItem(ItemID.JojaCola)).AddItemDisplay(ItemID.TrashCan).SetHoverTextOverride("Thanks Fanny! I already wanted to cook it."));

            fannyMessages.Add(new FannyMessage("Nightfuel", "Nightmare Fuel, huh? ...you know, maybe if you can harvest enough of it, maybe those Pumpkings will stop terorrizing our inhabitants and they'll be permanently more happy!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<NightmareFuel>())).AddItemDisplay(ModContent.ItemType<NightmareFuel>()));

            fannyMessages.Add(new FannyMessage("Endenergy", "Ooh, is that Endothermic Energy? If we can get a decent supply of it, I think those Ice Queens will fear us and our residents might be forever grateful with us!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<EndothermicEnergy>())).AddItemDisplay(ModContent.ItemType<EndothermicEnergy>()));

            fannyMessages.Add(new FannyMessage("Darksunfrag", "What's that? Darksun Fragment? Do you think with enough of it, our world will be permanently lit up like a lemon-scented candle flame?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<DarksunFragment>())).AddItemDisplay(ModContent.ItemType<DarksunFragment>()));

            fannyMessages.Add(new FannyMessage("Onion", "I'd be weary about eating that strange plant. You can only get one, so it might be useful to hang on to it for later.",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<CelestialOnion>())).AddItemDisplay(ModContent.ItemType<CelestialOnion>()));

            fannyMessages.Add(new FannyMessage("MurasamaBig", "You. Yeah, you. I know you downloaded this mod just so you could have your disgustingly sized Murasama slash back! After all of Fanny's incessant, inaccurate drivel, are you satisfied? Was it worth it?",
                "EvilIdle", (FannySceneMetrics scene) => Main.LocalPlayer.controlUseItem && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Murasama>() && DownedBossSystem.downedDoG && fannyTimesFrozen <= 0).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("CleavageFurrow", "One time, I went to the pond on a bright summer afternoon! One of the frogs leaped from its lily pad and locked eyes with me. I believe it had a cleavage furrow? Oh, what a dopey little smile that fella had!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<UniverseSplitter>())).SetHoverTextOverride("Very cool, Fanny!"));

            fannyMessages.Add(new FannyMessage("PetRock", "Oh hey, is that my pet rock? I lost it in my backyard a few years back. I’ve been trying to find it since!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Rock>())).AddItemDisplay(ModContent.ItemType<Rock>()));

            FannyMessage rock1 = new FannyMessage("RockLobster1", "Oh, hot tamales! You've gone and done it! You've reeled in a lobster, my friend! Can you believe it? A rock lobster, right here in the oasis! I mean, who would've thought? Rock Lobsters are like the rockstars of the desert, and you just snagged one. You're a fishing master, my friend! But hey, let me tell you a tale about lobsters that'll have you crackling with laughter. So there I was, with my good pal Dron, you know. We decided to hit up this fancy lobster restaurant downtown. Now, you might be wondering, \"Fanny, why would a flame like you even eat lobster?\" Well, my friend, curiosity burns bright, and I figured it was worth a shot. We waltz into this posh place, me flickering with excitement, Dron rolling in like a boss. We get seated, and the waiter hands us these bibs – you know, the ones with the goofy lobster design? I, of course, couldn't wear one, being made of flame and all, but Dron struggled a bit. Picture it: Dron, bib askew, trying to maneuver it with no arms. Hilarious, right? Now, we dive into the menu. Lobster this, lobster that – it was like a seafood carnival! Dron's eyes were wide, no arms to shield them from the sea of options. We decided to go for the lobster feast, the whole shebang. And let me tell you, it was a feast fit for kings! But here comes the twist! The waiter brings out this succulent lobster dish, steam rising like the flames on my head. Dron takes a bite, a hearty one, and suddenly, his face turns redder than a ripe tomato. Turns out, he's allergic to shellfish! Who would've thought? Poor Dron, armless and allergic – the universe sure has a sense of humor. Which reminds me of the spicy saga of my friend Green Demon and his salsa extravaganza. Now, Green Demon is quite the character – always pushing the boundaries of fiery flavors, forever on a quest to grow the spiciest peppers in our little corner of the world. One day, the sun was blazing overhead, and Green Demon excitedly invited our motley crew – me, La Ruga, Ogscule, Tim, Cnidrion, and Pyrogen – for a salsa fiesta at his fiery abode. Oh, the anticipation was palpable! I flickered with excitement, eager to see what kind of fiery concoction he had in store for us. As we approached, the scent of peppers wafted through the air like a zesty dance. Rows upon rows of vibrant, fiery red and green peppers swayed in the breeze, basking in the sun's warm embrace. Green Demon, with his devilish grin, welcomed us to his spicy paradise. \"Behold, my friends! The harvest of the spiciest peppers in the land!\" he declared, his eyes gleaming with mischievous delight. We gathered around as Green Demon plucked peppers with such finesse, it was like he was orchestrating a spicy symphony. With a basket brimming with peppers, he led us to his fiery kitchen, a cauldron bubbling away with a mysterious potion – or rather, his special salsa. The kitchen was alive with the rhythmic chopping of peppers and the sizzling melody of ingredients harmonizing in the pan. I, being a flame myself, felt right at home amidst the culinary inferno. Green Demon's hands moved with the precision of a seasoned chef, his eyes gleaming with the promise of a taste explosion. Now, my friends, you must understand – this salsa wasn't just your average dip. It was a potion of pure heat, a symphony of spices that would make even the bravest tongues tremble. Green Demon was a maestro, and his salsa was his fiery masterpiece. As the salsa simmered, the aroma grew more intense. It was like a spicy enchantment had taken over the kitchen. We were all eagerly awaiting the taste test, our excitement building like a rising flame. Finally, the moment of truth arrived. Green Demon scooped up a generous amount of salsa and handed us each a tortilla chip. We stared at each other, eyes wide with anticipation, and took a bite simultaneously. Flames! The heat hit us like a spicy meteor shower.", "Idle"
                , (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.RockLobster), maxWidth: 1000, fontSize: 0.8f).AddItemDisplay(ItemID.RockLobster);
            fannyMessages.Add(rock1);
            FannyMessage rock2 = new FannyMessage("RockLobster2", "Laughter erupted as we danced around, fanning our tongues with imaginary flames. Even I, a flame myself, felt the burn in the most delightful way. Green Demon stood there, triumphant, his devilish grin widening. \"What did I tell you, my friends? The spiciest salsa in the realm!\" But my pal La Ruga took a cautious bite, realizing it was the perfect companion to his daily tea sessions. Oh La Ruga. Every Tuesday, without fail, we embark on a literary adventure to our favorite place in the world – the cozy little bookstore at the corner of the Exosphere. We strut into the bookstore like we own the place, La Ruga and me. Well, La Ruga doesn't really strut; more like gracefully scuttles with its long legs. I'm there, flickering and crackling, like a flame on a mission – a mission for some good reads. The moment we step inside, the scent of old paper and ink wafts through the air. It's like the very essence of knowledge, a fragrance that fuels my fiery enthusiasm. La Ruga seems to breathe it all in, its shadowy arms flickering with delight. Now, you might wonder, \"Fanny, why a bookstore?\" Well, let me tell you, there's something magical about flipping through the pages of a good book. The way the words dance off the paper, creating a world only limited by the imagination. It's a burning passion of mine, and La Ruga, with its ancient wisdom, appreciates the art of storytelling too. We start our journey through the aisles, and La Ruga guides me to the classics – timeless tales of adventure, romance, and mystery. Picture this: a flame flickering next to a T⍑ᒷ rᒷᓵꖌ𝙹リ, engrossed in the ancient wisdom of literature. If anyone happened to stroll by, they'd probably think they'd stumbled into a fantastical scene from a storybook. But, oh, the joy of finding a hidden gem! We once stumbled upon a dusty old tome, forgotten by time. It had a worn cover, and the pages whispered secrets of a bygone era. La Ruga and I exchanged excited glances – this was a treasure trove waiting to be explored! We huddled in a cozy reading nook, La Ruga spreading its legs out, making a snug spot for me to flicker comfortably. As I read aloud, the words came alive, creating vivid images in our minds. The world outside faded away, and it was just me, La Ruga, and the enchanting tales within those pages. Of course, being a flame, I have to be careful not to singe the pages. La Ruga occasionally gives me a gentle nudge if I get too close, a silent reminder to keep my flames in check. We've developed quite the teamwork, I must say. Now, I pick out a few books at the bookstore, and head to the park for some extra reading. Usually, my friend Tim joins me at the park, but this time, my other friend Ogscule decided to join me instead.  Now picture this: a sunny day at the park, the trees swaying gently, and the air filled with the delightful scent of nature. I'm there, Fanny the Flame, and by my side is none other than my trusty friend Ogscule. We come across a patch of grass, and Ogscule suggests we feed the birds. Now, considering that Ogscule, like Dron, doesn't have hands – or any appendages, for that matter – I'm intrigued. How on earth is this going to work? But I'm all for adventure, so we decide to give it a shot. Ogscule plops down on the grass, and I hover beside him, flames flickering in anticipation. We've got a bag of bird feed, and Ogscule looks at me with those fleshy eyes, ready for action. Without arms, he leans toward the bag, trying to nudge it open with the tip of his prongs. It's like watching a determined utensil on a mission. Meanwhile, I'm providing some fiery commentary, encouraging Ogscule like a cheerleader at a sports game. \"Go, Ogscule, you got this! Show those birds your fleshy finesse!\" I can't help but chuckle at the absurdity of the situation. We're the dynamic duo of the park, making memories that'll have the birds tweeting about us for days. Finally, after a bit of a struggle, Ogscule manages to spill some bird feed on the ground. The birds, sensing a feast, swoop down like they've just won the avian lottery. But here's the twist – since Ogscule doesn't have hands, the birds are pecking at the bird feed, and Ogscule's prongs. It's a hilarious sight, a bizarre dance of feathers and fork. I'm laughing so hard; my flames are dancing with delight. Ogscule doesn't seem to mind; he's embracing the chaos, embracing the absurdity of our friendship. We may not be the most conventional pair, but we sure know how to turn a simple day at the park into a sidesplitting comedy. Life with these friends is like a colorful cartoon, each day filled with laughter and unexpected twists. So here's to more adventures, more lobsters, and more tales to share. Thanks for being with me on this fiery journey, my friend!",
                "Idle", FannyMessage.AlwaysShow, maxWidth: 1000, fontSize: 0.8f)
                .NeedsActivation();

            rock1.AddStartEvent(() => rock2.ActivateMessage());

            fannyMessages.Add(rock2);
            #endregion

            #region CrossMod

            fannyMessages.Add(new FannyMessage("Catharsis", "Don’t exhume Kaleidoscope! Catharsis is known to cause clinical depression in users.",
               "Nuhuh", (FannySceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == ModContent.NPCType<WITCH>() && Main.LocalPlayer.TalkNPC == n) && ModLoader.HasMod("CatalystMod") && Main.LocalPlayer.HasItem(ItemID.RainbowWhip)));

            fannyMessages.Add(new FannyMessage("ThoriumOre", "Hey, take a look at this blueish-greenish-yellow metal! Isn't it cool? It's called Thorium! That would be a great name for a mod! Thankfully, we're not in a game, so I can use it freely!",
               "Nuhuh", (FannySceneMetrics scene) => CalRemixHelper.HasCrossModItem(Main.LocalPlayer, "ThoriumMod", "ThoriumOre")));

            fannyMessages.Add(new FannyMessage("OcramSkull", "Woah! That skull looks a bit.. off! I feel like i've seen it before, a long time ago. I'm pretty sure it summons a really big bad guy! Are you sure you can take him? (He's really big, and bad!)",
               "Nuhuh", (FannySceneMetrics scene) => CalRemixHelper.HasCrossModItem(Main.LocalPlayer, "Consolaria", "SuspiciousLookingSkull")));

            #endregion

            #region References

            fannyMessages.Add(new FannyMessage("PortalGun", "Cave Johnson here. We're fresh out of combustible lemons, but let me tell you a little bit about this thing here. These portals are only designed to stick on planetoid rock and not much else. Hope you've got a test chamber lying around that's full of that stuff!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.PortalGun)));

            fannyMessages.Add(new FannyMessage("Murasama", "Erm, holy crap? $0? Is that a reference to my FAVORITE game of all time, metal gear rising revengeance? Did you know that calamity adds a custom boss health boss bar and many othe-",
               "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<Murasama>())).AddDynamicText(FannyMessage.GetPlayerName));

            fannyMessages.Add(new FannyMessage("Ultrakill", "Oh EM GEE! A gun from the hit first-person shooter game, \'MURDERDEATH\'!? Try throwing out some coins and hitting them with a Titanium Railgun to pull a sick railcoin maneuver!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ModContent.ItemType<MidasPrime>()) || Main.LocalPlayer.HasItem(ModContent.ItemType<CrackshotColt>())).AddItemDisplay(ModContent.ItemType<MidasPrime>()));

            fannyMessages.Add(new FannyMessage("Tofu", "Uh oh! Looks like one of your items is a reference to a smelly old game franchise known as Touhou! Do your ol\' pal Fanny a good deed and put it away.",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.HasItems(TofuItems)).SetHoverTextOverride("Anything for you Fanny!"));

            #endregion

        }
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
    }
}