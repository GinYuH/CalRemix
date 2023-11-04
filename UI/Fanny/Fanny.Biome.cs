using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables.Furniture;
using CalRemix.Items;
using CalRemix.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadBiomeMessages()
        {
            fannyMessages.Add(new FannyMessage("DungeonGuardian", "It appears you're approaching the Dungeon. Normally this place is guarded by viscious guardians, but I've disabled them for you my dear friend.", "Nuhuh",
                NearDungeonEntrance));

            fannyMessages.Add(new FannyMessage("Snowbr", "It's quite chilly here, maybe you should invest some time in gathering some cold-protective gear before you freeze to death!", "Idle",
                (FannySceneMetrics scene) => Main.LocalPlayer.ZoneSnow));

            fannyMessages.Add(new FannyMessage("Cavern", "It's quite dark down here. You should go get some more torches before further exploration or you may fall into a pit full of lice!", "Idle",
                (FannySceneMetrics scene) => Main.LocalPlayer.ZoneRockLayerHeight).AddItemDisplay(ItemID.Torch));

            fannyMessages.Add(new FannyMessage("Granite", "Woah, this place looks so cool and futuristic! It's almost like an entirely different dimension here!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGranite));

            fannyMessages.Add(new FannyMessage("Marble", "Marble? I LOVE playing with marbles! A few hundred years ago I was an avid marble collector, collecting marbles of various shapes, colors, and sizes. But, one day, I lost my marbles.",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMarble));

            fannyMessages.Add(new FannyMessage("FungalGrowths", "I know a quick get rich quick scheme. See those Glowing Mushrooms? They sell for a lot! Go destroy that ecosystem for some quick cash!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom).AddItemDisplay(ItemID.GlowingMushroom));

            fannyMessages.Add(new FannyMessage("GemCave", "So many gemstones! Make sure to keep some Emeralds handy. Apparently a lot of people like to search for them to make crates for some reason!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGemCave).AddItemDisplay(ItemID.Emerald));

            fannyMessages.Add(new FannyMessage("SunkySea", "Did you know that the oldest animal ever identified was a clam? Unfortunately, the people who caught it accidentally froze it to death. Maybe you can find an older clam here in this Sunken Sea!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSunkenSea));

            fannyMessages.Add(new FannyMessage("Hell", "Welcome to hell! This place is flaming hot just like me, so you better get some gear to protect you aganist the heat!", "Nuhuh",
                (FannySceneMetrics scene) => Main.LocalPlayer.ZoneUnderworldHeight));

            fannyMessages.Add(new FannyMessage("ShimmerNothing", "You should consider throwing that item you're holding in Shimmer! You may get something powerful!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneShimmer && !Main.LocalPlayer.HeldItem.CanShimmer(), onlyPlayOnce: false, cooldown: 600));

            fannyMessages.Add(new FannyMessage("Meteore", "A Fallen Star!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMeteor).AddItemDisplay(ItemID.FallenStar));

            fannyMessages.Add(new FannyMessage("DeepAbyss", "Tired of this pesky abyss drowning you? I have an idea! If you go into the underworld and poke a hole at the bottom, all the water will drain out! No more pesky pressure!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer3));

            fannyMessages.Add(new FannyMessage("RodAbyss", "It sure takes a while to get to the bottom of the Abyss... Maybe try using that teleporting thingamabob you have?",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.HasItem(ItemID.RodofDiscord) || Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>())).AddItemDisplay(ItemID.RodofDiscord));

            fannyMessages.Add(new FannyMessage("Temple", "I love house invasion!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple).SetHoverTextOverride("Me too Fanny!"));

            fannyMessages.Add(new FannyMessage("TempleWires", "Aw man, there's so many booby traps in here! Try using that fancy gadget of yours to disable them!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple && (Main.LocalPlayer.HasItem(ItemID.WireCutter) || Main.LocalPlayer.HasItem(ItemID.MulticolorWrench) || Main.LocalPlayer.HasItem(3611))).AddItemDisplay(ItemID.WireCutter));

            fannyMessages.Add(new FannyMessage("Altars", "Smashing demon altars is no longer guaranteed to bless your world with ores. But it’s still worth a shot!",
               "Idle", (FannySceneMetrics scene) => (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson) && Main.hardMode && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !Main.LocalPlayer.ZoneUnderworldHeight));

            fannyMessages.Add(new FannyMessage("StupidSword", "If you kill enough Meteor Heads, you might be able to get the Divine Intervention!",
                "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMeteor && ModLoader.HasMod("CatalystMod")));

            fannyMessages.Add(new FannyMessage("DrowningAbyss", "Your air bubbles are disappearing at an alarming rate, you should set up an air pocket, and fast!",
                "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.breath <= 0 && Main.LocalPlayer.Calamity().ZoneAbyss));

            fannyMessages.Add(new FannyMessage("Jungleabyss", "I’ve heard word that there’s incredible treasures in the mysterious depths of the ocean, the one past the jungle!",
   "Nuhuh", (FannySceneMetrics scene) => !NPC.downedBoss3 && Main.LocalPlayer.ZoneJungle && Main.rand.NextBool(600)));

            fannyMessages.Add(new FannyMessage("Sulph", "Ah the good ol' Sulphurous Sea. Just take a breathe of the fresh air here! If you see any tiny light green lights, you should use a Bug Net on it to get a fancy light pet.",
  "Idle", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur).AddItemDisplay(ModContent.ItemType<DisgustingSeawater>()));

            fannyMessages.Add(new FannyMessage("Starbuster", "Trying to get a Starbuster Core? Lately those culex things have been hardening up! The only way to force their cores out of them is by running a Unicorn into them!",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight)).AddItemDisplay(ModContent.ItemType<StarbusterCore>()));

            fannyMessages.Add(new FannyMessage("NotBlessedApple", "A smart one ey? Unfortunately, only hostile Unicorns are able to break those astral batties open.",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) && Main.LocalPlayer.mount.Type == MountID.Unicorn));

            fannyMessages.Add(new FannyMessage("SideGar", "Have you ever heard of gars? They're a neat fish group that you can rip open for valuable loot. One species of gar is the Side Gar, which can be fished up in sky lakes!",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneSkyHeight && NPC.downedMoonlord).AddItemDisplay(ModContent.ItemType<SideGar>()));

            fannyMessages.Add(new FannyMessage("RearGar", "Fossilized tree bark!? In the Jungle's mud!? That sounds disgusting! I'll send over some gars to clean it up for you my friend. But, if you ever want some of that stuff for whatever reason, just go fish for some gars in the Jungle!",
  "Nuhuh", (FannySceneMetrics scene) => DownedBossSystem.downedProvidence).AddItemDisplay(ModContent.ItemType<RearGar>()).SetHoverTextOverride("Thank you so much Fanny! Tree bark is disgusting!"));

            fannyMessages.Add(new FannyMessage("FrontGar", "Now why did that ghost thing cause the ocean to go all crazy? Who knows! But what I do know is that the gars in the Abyss have started mutating. You should try fishing up some gars from the Sulphurous Sea and see if you can extract them for something useful.",
  "Nuhuh", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur && DownedBossSystem.downedPolterghast).AddItemDisplay(ModContent.ItemType<FrontGar>()));

            fannyMessages.Add(new FannyMessage("MeldHeart", "Look at all that gunk! I'm pretty sure it's impossible to break it, so the best solution I can give is to assure it doesn't spread further by digging around it.",
  "Idle", (FannySceneMetrics scene) => CalRemixWorld.MeldTiles > 22 && !ModLoader.HasMod("NoxusBoss")));

            fannyMessages.Add(new FannyMessage("MeldHeartNoxus", "Look at all that gunk! I'm pretty sure it's impossible to break it, well, maybe if you got some powerful spray bottle, but that might take a while, so the best solution I can give is to assure it doesn't spread further by digging around it.",
  "Idle", (FannySceneMetrics scene) => CalRemixWorld.MeldTiles > 22 && ModLoader.HasMod("NoxusBoss")));
        }

        public static void LoadShrineMessages()
        {
            fannyMessages.Add(new FannyMessage("ShrineSnow", "Woah, is that a snow shrine? You better go loot it for its one-of-a-kind treasure! It gave you a really cool item that you'll use forever I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneSnow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TundraLeash>()));

            fannyMessages.Add(new FannyMessage("ShrineDesert", "Woah, is that a desert shrine? You better go loot it for its one-of-a-kind treasure! It gave you a tile-matching game called Luxor I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneUndergroundDesert && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<LuxorsGift>()));

            fannyMessages.Add(new FannyMessage("ShrineCorruption", "Woah, is that a corruption shrine? You better go loot it for its one-of-a-kind treasure! It caused pebbles to rain from the sky I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneCorrupt && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CorruptionEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineCrimson", "Woah, is that a crimson shrine? You better go loot it for its one-of-a-kind treasure! It caused pebbles to rain from the sky I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneCrimson && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CrimsonEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineUg", "Woah, is that an underground shrine? You better go loot it for its one-of-a-kind treasure! It caused you to gain defense while standing still I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneNormalUnderground && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TrinketofChi>()));

            fannyMessages.Add(new FannyMessage("ShrineHallow", "Woah, is that a hallow shrine? You better go loot it for its one-of-a-kind treasure! No seriously, it's the only thing exclusive to the Hallow!",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneHallow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<HallowEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineAstral", "Woah, is that an astral shrine? You better go loot it for its one-of-a-kind treasure! It summoned a large mimic I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<AstralEffigy>()));

            fannyMessages.Add(new FannyMessage("ShrineGranite", "Woah, is that a granite shrine? You better go loot it for its one-of-a-kind treasure! It caused sparks to fly out of enemies when hit I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGranite && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<UnstableGraniteCore>()));

            fannyMessages.Add(new FannyMessage("ShrineMarble", "Woah, is that a marble shrine? You better go loot it for its one-of-a-kind treasure! It summoned cool orbital swords I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneMarble && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<GladiatorsLocket>()));

            fannyMessages.Add(new FannyMessage("ShrineMushroom", "Woah, is that a mushroom shrine? You better go loot it for its one-of-a-kind treasure! It imbued true melee weapons with fungi I think?",
                "Awooga", (FannySceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<FungalSymbiote>()));
        }
    }
}