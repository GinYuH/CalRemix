using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Furniture;
using CalRemix.Items;
using CalRemix.Items.Placeables;
using CalRemix.Subworlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadBiomeMessages()
        {

            screenHelperMessages.Add(new HelperMessage("Arsenic", "This place is a lot more out of this world than when I was last here! Try breaking through walls to find the rare and precious Arsenic Ore which can be used for highly advanced robotics!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneDungeon));

            screenHelperMessages.Add(new HelperMessage("Snowbr", "It's quite chilly here, maybe you should invest some time in gathering some cold-protective gear before you freeze to death!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneSnow));

            screenHelperMessages.Add(new HelperMessage("Cavern", "It's quite dark down here. You should go get some more torches before further exploration or you may fall into a pit full of lice!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneRockLayerHeight).AddItemDisplay(ItemID.Torch));

            screenHelperMessages.Add(new HelperMessage("Granite", "Woah, this place looks so cool and futuristic! It's almost like an entirely different dimension here!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGranite));

            screenHelperMessages.Add(new HelperMessage("Marble", "Marble? I LOVE playing with marbles! A few hundred years ago I was an avid marble collector, collecting marbles of various shapes, colors, and sizes. But, one day, I lost my marbles.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMarble));

            screenHelperMessages.Add(new HelperMessage("FungalGrowths", "I know a quick get rich quick scheme. See those Glowing Mushrooms? They sell for a lot! Go destroy that ecosystem for some quick cash!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom).AddItemDisplay(ItemID.GlowingMushroom));

            screenHelperMessages.Add(new HelperMessage("GemCave", "So many gemstones! Make sure to keep some Emeralds handy. Apparently a lot of people like to search for them to make crates for some reason!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGemCave).AddItemDisplay(ItemID.Emerald));

            screenHelperMessages.Add(new HelperMessage("SunkySea", "Did you know that the oldest animal ever identified was a clam? Unfortunately, the people who caught it accidentally froze it to death. Maybe you can find an older clam here in this Sunken Sea!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSunkenSea));

            screenHelperMessages.Add(new HelperMessage("Hell", "Welcome to hell! This place is flaming hot just like me, so you better get some gear to protect you aganist the heat!", "FannyNuhuh",
                (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneUnderworldHeight && !Main.hardMode));

            screenHelperMessages.Add(new HelperMessage("ShimmerNothing", "You should consider throwing that item you're holding in Shimmer! You may get something powerful!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneShimmer && !Main.LocalPlayer.HeldItem.CanShimmer(), onlyPlayOnce: false, cooldown: 600));

            screenHelperMessages.Add(new HelperMessage("Meteore", "A Fallen Star!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMeteor).AddItemDisplay(ItemID.FallenStar));

            screenHelperMessages.Add(new HelperMessage("DeepAbyss", "Tired of this pesky abyss drowning you? I have an idea! If you go into the underworld and poke a hole at the bottom, all the water will drain out! No more pesky pressure!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer3));

            screenHelperMessages.Add(new HelperMessage("InfernumAbyss", "Try looking for chests down here! I’ve heard there’s unique treasures to be found! A spelunker potion should help!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer2 && ModLoader.HasMod("InfernumMode")));

            screenHelperMessages.Add(new HelperMessage("NoInfernumAbyss", "Try hunting the creatures for new weapons! Hunter, Battle and Zerg potions should help!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer2 && !ModLoader.HasMod("InfernumMode")));

            screenHelperMessages.Add(new HelperMessage("RodAbyss", "It sure takes a while to get to the bottom of the Abyss... Maybe try using that teleporting thingamabob you have?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyss && (Main.LocalPlayer.HasItem(ItemID.RodofDiscord) || Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>()))).AddItemDisplay(ItemID.RodofDiscord));

            screenHelperMessages.Add(new HelperMessage("PlaguedJungle", "Man, this place reminds me of when I volunteered to help run a jungle-themed summer fair at a local elementary school! Long story short, everyone fainted due to dehydration. Not the plague. At least, that’s what I think.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().ZonePlague));

            screenHelperMessages.Add(new HelperMessage("Temple", "I love house invasion!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple).SetHoverTextOverride("Me too Fanny!"));

            screenHelperMessages.Add(new HelperMessage("TempleWires", "Aw man, there's so many booby traps in here! Try using that fancy gadget of yours to disable them!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple && (Main.LocalPlayer.HasItem(ItemID.WireCutter) || Main.LocalPlayer.HasItem(ItemID.MulticolorWrench) || Main.LocalPlayer.HasItem(3611))).AddItemDisplay(ItemID.WireCutter));

            screenHelperMessages.Add(new HelperMessage("Altars", "Smashing demon altars is no longer guaranteed to bless your world with ores. But it’s still worth a shot!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson) && Main.hardMode && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !Main.LocalPlayer.ZoneUnderworldHeight));

            screenHelperMessages.Add(new HelperMessage("StupidSword", "If you kill enough Meteor Heads, you might be able to get the Divine Intervention!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMeteor && ModLoader.HasMod("CatalystMod")));

            screenHelperMessages.Add(new HelperMessage("DrowningAbyss", "Your air bubbles are disappearing at an alarming rate, you should set up an air pocket, and fast!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.breath <= 0 && Main.LocalPlayer.Calamity().ZoneAbyss));

            screenHelperMessages.Add(new HelperMessage("Jungleabyss", "I’ve heard word that there’s incredible treasures in the mysterious depths of the ocean, the one past the jungle!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !NPC.downedBoss3 && Main.LocalPlayer.ZoneJungle && Main.rand.NextBool(600)));

            screenHelperMessages.Add(new HelperMessage("Sulph", "Ah the good ol' Sulphurous Sea. Just take a breathe of the fresh air here! If you see any tiny light green lights, you should use a Bug Net on it to get a fancy light pet.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur).AddItemDisplay(ModContent.ItemType<DisgustingSeawater>()));

            screenHelperMessages.Add(new HelperMessage("Starbuster", "Trying to get a Starbuster Core? Lately those culex things have been hardening up! The only way to force their cores out of them is by running a Unicorn into them!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight)).AddItemDisplay(ModContent.ItemType<StarbusterCore>()));

            screenHelperMessages.Add(new HelperMessage("NotBlessedApple", "A smart one ey? Unfortunately, only hostile Unicorns are able to break those astral batties open.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) && Main.LocalPlayer.mount.Type == MountID.Unicorn));

            screenHelperMessages.Add(new HelperMessage("SideGar", "Have you ever heard of gars? They're a neat fish group that you can rip open for valuable loot. One species of gar is the Side Gar, which can be fished up in sky lakes!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneSkyHeight && NPC.downedMoonlord).AddItemDisplay(ModContent.ItemType<SideGar>()));

            screenHelperMessages.Add(new HelperMessage("RearGar", "Fossilized tree bark!? In the Jungle's mud!? That sounds disgusting! I'll send over some gars to clean it up for you my friend. But, if you ever want some of that stuff for whatever reason, just go fish for some gars in the Jungle!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => DownedBossSystem.downedProvidence).AddItemDisplay(ModContent.ItemType<RearGar>()).SetHoverTextOverride("Thank you so much Fanny! Tree bark is disgusting!"));

            screenHelperMessages.Add(new HelperMessage("FrontGar", "Now why did that ghost thing cause the ocean to go all crazy? Who knows! But what I do know is that the gars in the Abyss have started mutating. You should try fishing up some gars from the Sulphurous Sea and see if you can extract them for something useful.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur && DownedBossSystem.downedPolterghast).AddItemDisplay(ModContent.ItemType<FrontGar>()));

            screenHelperMessages.Add(new HelperMessage("Ogslime", "This place seems new! Oh! It has a new type of wood too! Maybe you can hit one of those new Wandering Eye things while wearing it for a new Ogscule!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral).AddItemDisplay(ModContent.ItemType<AstralMonolith>()));

            screenHelperMessages.Add(new HelperMessage("Exospheree", "THE EXOSPHERE!!!!",
                "FannyAwooga").OnlyPlayInSubworld(ModContent.GetInstance<ExosphereSubworld>()));

            screenHelperMessages.Add(new HelperMessage("Desert", "Oh, look at you, venturing into the sandy abyss! Remember, in the desert, the sand's as hot as a freshly microwaved burrito! So don't forget your sunscreen... or your water... or your sanity. ",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneDesert));

            screenHelperMessages.Add(new HelperMessage("Corpution", "Ah, the Corruption, where the grass is as dark as my soul after what I did to that sleeping homeless person on Feburary 2nd at 2:35 AM. Just watch out for those pesky corruption monsters, they'll nibble you right up!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneCorrupt));

            screenHelperMessages.Add(new HelperMessage("PlantDungeon", "Welcome to the dungeon, where skeletons have more bones than a Halloween decoration aisle! Just be careful not to wake the sleeping spirits, they're grumpier than a cat without its afternoon nap.",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedPlantBoss && Main.LocalPlayer.ZoneDungeon));

        }

        public static void LoadShrineMessages()
        {
            screenHelperMessages.Add(new HelperMessage("ShrineSnow", "Woah, is that a snow shrine? You better go loot it for its one-of-a-kind treasure! It gave you a really cool item that you'll use forever I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneSnow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TundraLeash>()));

            screenHelperMessages.Add(new HelperMessage("ShrineDesert", "Woah, is that a desert shrine? You better go loot it for its one-of-a-kind treasure! It gave you a tile-matching game called Luxor I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneUndergroundDesert && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<LuxorsGift>()));

            screenHelperMessages.Add(new HelperMessage("ShrineCorruption", "Woah, is that a corruption shrine? You better go loot it for its one-of-a-kind treasure! It caused pebbles to rain from the sky I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !WorldGen.crimson && Main.LocalPlayer.ZoneCorrupt && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CorruptionEffigy>()));

            screenHelperMessages.Add(new HelperMessage("ShrineCrimson", "Woah, is that a crimson shrine? You better go loot it for its one-of-a-kind treasure! It caused pebbles to rain from the sky I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => WorldGen.crimson && Main.LocalPlayer.ZoneCrimson && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CrimsonEffigy>()));

            screenHelperMessages.Add(new HelperMessage("ShrineUg", "Woah, is that an underground shrine? You better go loot it for its one-of-a-kind treasure! It caused you to gain defense while standing still I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneNormalUnderground && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TrinketofChi>()));

            screenHelperMessages.Add(new HelperMessage("ShrineHallow", "Woah, is that a hallow shrine? You better go loot it for its one-of-a-kind treasure! No seriously, it's the only thing exclusive to the Hallow!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneHallow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<HallowEffigy>()));

            screenHelperMessages.Add(new HelperMessage("ShrineAstral", "Woah, is that an astral shrine? You better go loot it for its one-of-a-kind treasure! It summoned a large mimic I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<AstralEffigy>()));

            screenHelperMessages.Add(new HelperMessage("ShrineGranite", "Woah, is that a granite shrine? You better go loot it for its one-of-a-kind treasure! It caused sparks to fly out of enemies when hit I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGranite && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<UnstableGraniteCore>()));

            screenHelperMessages.Add(new HelperMessage("ShrineMarble", "Woah, is that a marble shrine? You better go loot it for its one-of-a-kind treasure! It summoned cool orbital swords I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMarble && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<GladiatorsLocket>()));

            screenHelperMessages.Add(new HelperMessage("ShrineMushroom", "Woah, is that a mushroom shrine? You better go loot it for its one-of-a-kind treasure! It imbued true melee weapons with fungi I think?",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<FungalSymbiote>()));
        }
    }
}