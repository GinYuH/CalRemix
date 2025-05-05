using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Furniture;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Pets;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Subworlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.World;
using System.Reflection;
using System;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadBiomeMessages()
        {

            HelperMessage.New("Arsenic", "This place is a lot more out of this world than when I was last here! Try breaking through walls to find the rare and precious Arsenic Ore which can be used for highly advanced robotics!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneDungeon);

            HelperMessage.New("Snowbr", "It's quite chilly here, maybe you should invest some time in gathering some cold-protective gear before you freeze to death!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneSnow);

            HelperMessage.New("Cavern", "It's quite dark down here. You should go get some more torches before further exploration or you may fall into a pit full of lice!", 
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneRockLayerHeight).AddItemDisplay(ItemID.Torch);

            HelperMessage.New("Granite", "Woah, this place looks so cool and futuristic! It's almost like an entirely different dimension here!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGranite);

            HelperMessage.New("Marble", "Marble? I LOVE playing with marbles! A few hundred years ago I was an avid marble collector, collecting marbles of various shapes, colors, and sizes. But, one day, I lost my marbles.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMarble);

            HelperMessage.New("FungalGrowths", "I know a quick get rich quick scheme. See those Glowing Mushrooms? They sell for a lot! Go destroy that ecosystem for some quick cash!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom).AddItemDisplay(ItemID.GlowingMushroom);

            HelperMessage.New("GemCave", "So many gemstones! Make sure to keep some Emeralds handy. Apparently a lot of people like to search for them to make crates for some reason!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGemCave).AddItemDisplay(ItemID.Emerald);

            HelperMessage.New("SunkySea", "Did you know that the oldest animal ever identified was a clam? Unfortunately, the people who caught it accidentally froze it to death. Maybe you can find an older clam here in this Sunken Sea!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSunkenSea);

            HelperMessage.New("Hell", "Welcome to hell! This place is flaming hot just like me, so you better get some gear to protect you aganist the heat!", 
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneUnderworldHeight && !ModLoader.HasMod("TheDepths"));

            HelperMessage.New("ShimmerNothing", "You should consider throwing that item you're holding in Shimmer! You may get something powerful!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneShimmer && !Main.LocalPlayer.ActiveItem().CanShimmer(), onlyPlayOnce: false, cooldown: 600);

            HelperMessage.New("Meteore", "A Fallen Star!",
                "FannyAwe", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMeteor && !ModLoader.HasMod("CatalystMod")).AddItemDisplay(ItemID.FallenStar);

            HelperMessage.New("DeepAbyss", "Tired of this pesky abyss drowning you? I have an idea! If you go into the underworld and poke a hole at the bottom, all the water will drain out! No more pesky pressure!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer3);

            HelperMessage.New("InfernumAbyss", "Try looking for chests down here! I’ve heard there’s unique treasures to be found! A spelunker potion should help!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer2 && CalRemixAddon.Infernum != null);

            HelperMessage.New("NoInfernumAbyss", "Try hunting the creatures for new weapons! Hunter, Battle and Zerg potions should help!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyssLayer2 && CalRemixAddon.Infernum == null);

            HelperMessage.New("RodAbyss", "It sure takes a while to get to the bottom of the Abyss... Maybe try using that teleporting thingamabob you have?",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAbyss && (Main.LocalPlayer.HasItem(ItemID.RodofDiscord) || Main.LocalPlayer.HasItem(ModContent.ItemType<NormalityRelocator>()))).AddItemDisplay(ItemID.RodofDiscord);

            HelperMessage.New("PlaguedJungle", "Man, this place reminds me of when I volunteered to help run a jungle-themed summer fair at a local elementary school! Long story short, everyone fainted due to dehydration. Not the plague. At least, that’s what I think.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().ZonePlague && !Main.LocalPlayer.ZoneRockLayerHeight);

            HelperMessage.New("PlaguedJunglePhyto", "Did you know if you gather a bunch of those pesky plague enemies, you'll summon a giant pineapple? He's a pretty chill fruit until you decide to pick a fight! So, make sure you're ready for a fruit salad battle before you start swinging!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().ZonePlague && Main.LocalPlayer.ZoneRockLayerHeight);

            HelperMessage.New("Temple", "Watch out! A trap!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple && Main.rand.NextBool(600));

            HelperMessage.New("TempleWires", "Aw man, there's so many booby traps in here! Try using that fancy gadget of yours to disable them!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneLihzhardTemple && (Main.LocalPlayer.HasItem(ItemID.WireCutter) || Main.LocalPlayer.HasItem(ItemID.MulticolorWrench) || Main.LocalPlayer.HasItem(3611))).AddItemDisplay(ItemID.WireCutter);

            HelperMessage.New("Altars", "Smashing demon altars is no longer guaranteed to bless your world with ores. But it’s still worth a shot!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson) && Main.hardMode && CalamityConfig.Instance.EarlyHardmodeProgressionRework && !Main.LocalPlayer.ZoneUnderworldHeight);

            HelperMessage.New("StupidSword", "If you kill enough Meteor Heads, you might be able to get the Divine Intervention!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMeteor && ModLoader.HasMod("CatalystMod"));

            HelperMessage.New("DrowningAbyss", "Your air bubbles are disappearing at an alarming rate, you should set up an air pocket, and fast!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.breath <= 0 && Main.LocalPlayer.Calamity().ZoneAbyss);

            HelperMessage.New("Jungleabyss", "I’ve heard word that there’s incredible treasures in the mysterious depths of the ocean, the one past the jungle!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => !NPC.downedBoss3 && Main.LocalPlayer.ZoneJungle && Main.rand.NextBool(600));

            HelperMessage.New("Sulph", "Ah the good ol' Sulphurous Sea. Just take a breath of the fresh air here! If you see any tiny light green lights, you should use a Bug Net on it to get a fancy light pet.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur).AddItemDisplay(ModContent.ItemType<DisgustingSeawater>());

            HelperMessage.New("Starbuster", "Trying to get a Starbuster Core? Lately those culex things have been hardening up! The only way to force their cores out of them is by running a Unicorn into them!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight)).AddItemDisplay(ModContent.ItemType<StarbusterCore>());

            HelperMessage.New("NotBlessedApple", "A smart one eh? Unfortunately, only hostile Unicorns are able to break those astral batties open.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && DownedBossSystem.downedAstrumAureus && Main.LocalPlayer.slotsMinions > 2 && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) && Main.LocalPlayer.mount.Type == MountID.Unicorn);

            HelperMessage.New("SideGar", "Have you ever heard of gars? They're a neat fish group that you can rip open for valuable loot. One species of gar is the Side Gar, which can be fished up in sky lakes!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneSkyHeight && NPC.downedMoonlord).AddItemDisplay(ModContent.ItemType<SideGar>());

            HelperMessage.New("RearGar", "Fossilized tree bark!? In the Jungle's mud!? That sounds disgusting! I'll send over some gars to clean it up for you my friend. But, if you ever want some of that stuff for whatever reason, just go fish for some gars in the Jungle!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => DownedBossSystem.downedProvidence).AddItemDisplay(ModContent.ItemType<RearGar>()).SetHoverTextOverride("Thank you so much Fanny! Tree bark is disgusting!");

            HelperMessage.New("FrontGar", "Now why did that ghost thing cause the ocean to go all crazy? Who knows! But what I do know is that the gars in the Abyss have started mutating. You should try fishing up some gars from the Sulphurous Sea and see if you can extract them for something useful.",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneSulphur && DownedBossSystem.downedPolterghast).AddItemDisplay(ModContent.ItemType<FrontGar>());

            HelperMessage.New("Ogslime", "This place seems new! Oh! It has a new type of wood too! Maybe you can hit one of those new Wandering Eye things while wearing it for a new Ogscule!",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral).AddItemDisplay(ModContent.ItemType<AstralMonolith>());

            HelperMessage.New("Home", "Oh hey, this garden seems familiar to me! I don't know why, though... Just, uh, give me a moment, ok?",
                "FannyNuhuh", (ScreenHelperSceneMetrics scene) => CrossModBiome("NoxusBoss", "EternalGardenBiome")).SetHoverTextOverride("Ok, take your time, Fanny!");

            HelperMessage.New("Exospheree", "THE EXOSPHERE!!!!",
                "FannyAwooga").OnlyPlayInSubworld(ModContent.GetInstance<ExosphereSubworld>());

            HelperMessage.New("Desert", "Look at you, venturing into the desert. Keep this little tip in mind; the largest desert is infested with antlions! Keep an eye out for an entrance into its lower layers, the biggest desert ALWAYS has one!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneDesert);

            HelperMessage.New("Corpution", "Ah, the Corruption, where the grass is as dark as a dark soul! Just watch out for those pesky corruption monsters, they'll nibble you right up!",
               "FannyNuhuh", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneCorrupt);

            HelperMessage.New("PlantDungeon", "Prepare yourself, my friend! Now that Plantera is gone, these skeletons are more than bone and cloth! They're armed to the teeth with powerful weapons, and their ravenous spirits will try to tear you apart once you kill them. This is a great opportunity to gear up, but be careful, ok?",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedPlantBoss && Main.LocalPlayer.ZoneDungeon);

            HelperMessage.New("Asbestos", "Asbestos is a naturally occurring fibrous silicate mineral. It's a rockin' place to be, but be careful breaking the walls of houses in these caves, you might attract a crippled smoker named Carcinoma! He's a real pain in the asbestos! Be careful where you swing that hammer!",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.InModBiome<AsbestosBiome>()).AddItemDisplay(ItemID.CopperHammer);

            HelperMessage apoc = HelperMessage.New("Apocalypse1", "Oh no.. oh no! Look what you've done! Ohhh we're really in the thick of it now...",
               "FannyApocalypse", (ScreenHelperSceneMetrics scene) => ProfanedDesert.scorchedWorld && !Main.LocalPlayer.dead, 9, cantBeClickedOff: true).InitiateConversation();
            HelperMessage.New("Apocalypse2", "everybody knows",
               "CrimSonDefault").SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(delay: 4, startTimerOnMessageSpoken: true);
            HelperMessage.New("Apocalypse3", "Fanny you dim-wick, take that stupid thing off.",
               "EvilFannyIdle").SpokenByEvilFanny().ChainAfter(apoc, delay: 7, startTimerOnMessageSpoken: true);
            HelperMessage.New("Apocalypse4", "Aww jeez, okay...",
               "FannySob").ChainAfter(delay: 2, startTimerOnMessageSpoken: true).EndConversation();
        }

        public static void LoadShrineMessages()
        {
            HelperMessage.New("ShrineSnow", "Woah, is that a snow shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneSnow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TundraLeash>());

            HelperMessage.New("ShrineDesert", "Woah, is that a desert shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneUndergroundDesert && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<LuxorsGift>());

            HelperMessage.New("ShrineCorruption", "Woah, is that a corruption shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => !WorldGen.crimson && Main.LocalPlayer.ZoneCorrupt && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CorruptionEffigy>());

            HelperMessage.New("ShrineCrimson", "Woah, is that a crimson shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => WorldGen.crimson && Main.LocalPlayer.ZoneCrimson && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<CrimsonEffigy>());

            HelperMessage.New("ShrineUg", "Woah, is that an underground shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneNormalUnderground && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<TrinketofChi>());

            HelperMessage.New("ShrineHallow", "Woah, is that a hallow shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneHallow && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<HallowEffigy>());

            HelperMessage.New("ShrineAstral", "Woah, is that an astral shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.Calamity().ZoneAstral && Main.LocalPlayer.ZoneRockLayerHeight && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<AstralEffigy>());

            HelperMessage.New("ShrineGranite", "Woah, is that a granite shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGranite && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<UnstableGraniteCore>());

            HelperMessage.New("ShrineMarble", "Woah, is that a marble shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneMarble && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<GladiatorsLocket>());

            HelperMessage.New("ShrineMushroom", "Woah, is that a mushroom shrine? You better go loot it for its one-of-a-kind treasure!",
                "FannyAwooga", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.ZoneGlowshroom && Main.rand.NextBool(116000)).AddItemDisplay(ModContent.ItemType<FungalSymbiote>());
        }
    }
}