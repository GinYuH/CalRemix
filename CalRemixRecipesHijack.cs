using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Plaguebringer;
using CalamityMod.Items.Armor.PlagueReaper;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Fishing.FishingRods;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables.DraedonStructures;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Typeless;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace CalRemix
{
    public partial class Recipes
    {
        public static List<(int, int, int)> alloyBarCrafts = new List<(int, int, int)> { };
        public static List<(int, int, int)> essenceBarCrafts = new List<(int, int, int)>{};
        public static List<(int, int, int)> yharimBarCrafts = new List<(int, int, int)> { };
        public static List<(int, int, int)> crocodileCrafts = new List<(int, int, int)> { };
        public static List<(int, int, int)> venomCrafts = new List<(int, int, int)> { };
        public static List<(int, int, int)> shimmerEssenceCrafts = new List<(int, int, int)> { };
        public static List<(int, int, int)> deliciousMeatCrafts = new List<(int, int, int)> { };
        public static List<(int, int, int)> accessoryCrafts = new List<(int, int, int)> { };

        public override void Load()
        {
            #region Alloy Bars
            alloyBarCrafts.Add((ItemType<TheAbsorber>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.NightsEdge, ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<OverloadedSludge>(), ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemType<BlightedCleaver>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<DefiledGreatsword>(), ItemType<AlloyBar>(), 3));
            alloyBarCrafts.Add((ItemType<Aestheticus>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<AgedLaboratoryElectricPanelItem>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<EssentialEssenceBar>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<FracturedArk>(), ItemType<AlloyBar>(), 3));
            alloyBarCrafts.Add((ItemType<JellyChargedBattery>(), ItemType<AlloyBar>(), 3));
            alloyBarCrafts.Add((ItemID.LifeCrystal, ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.MagicMirror, ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.MoneyTrough, ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemID.Muramasa, ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemType<Roxcalibur>(), ItemType<AlloyBar>(), 21));
            alloyBarCrafts.Add((ItemID.ShadowKey, ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<SunSpiritStaff>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.UltrabrightTorch, ItemType<AlloyBar>(), 4));
            //alloyBarCrafts.Add((ItemType<VictideBreastplate>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<GeliticBlade>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<Gelpick>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<GunkShot>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<Goobow>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<TheGodsGambit>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelArmor>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelGreaves>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelHeadSummon>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelHeadRogue>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelHeadRanged>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelHeadMelee>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<StatigelHeadMagic>(), ItemType<AlloyBar>(), 1));
            #endregion
            #region Essential Essence Bars
            essenceBarCrafts.Add((ItemType<TheAbsorber>(), ItemType<EssentialEssenceBar>(), 3));
            essenceBarCrafts.Add((ItemType<AmbrosialAmpoule>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<AbyssalDivingSuit>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<AbyssalDivingGear>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<CosmicAnvilItem>(), ItemType<EssentialEssenceBar>(), 3));
            essenceBarCrafts.Add((ItemType<CosmicWorm>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<AbyssBlade>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<CoreofCalamity>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<AstralBeaconItem>(), ItemType<EssentialEssenceBar>(), 10));
            essenceBarCrafts.Add((ItemType<Confection>(), ItemType<EssentialEssenceBar>(), 3));
            essenceBarCrafts.Add((ItemType<AstrealDefeat>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<ClockworkBow>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<CosmicRainbow>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<Abombination>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<AbyssalMirror>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<AdvancedDisplay>(), ItemType<EssentialEssenceBar>(), 3));
            essenceBarCrafts.Add((ItemType<AridArtifact>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<TrueArkoftheAncients>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<AsgardsValor>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<TrueBiomeBlade>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<BarracudaGun>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<BladedgeRailbow>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<BrimstoneSword>(), ItemType<EssentialEssenceBar>(), 1));
            essenceBarCrafts.Add((ItemType<CatastropheClaymore>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<DarklightGreatsword>(), ItemType<EssentialEssenceBar>(), 4));
            essenceBarCrafts.Add((ItemType<FlarefrostBlade>(), ItemType<EssentialEssenceBar>(), 1));
            #endregion
            #region Yharim Bars
            yharimBarCrafts.Add((ItemType<AsgardianAegis>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<AuricBar>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<AuricTeslaBodyArmor>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<AuricTeslaCuisses>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<AuricTeslaHoodedFacemask>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<AuricTeslaWireHemmedVisage>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<AuricTeslaPlumedHelm>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<AuricTeslaSpaceHelmet>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<AuricTeslaRoyalHelm>(), ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ItemType<EclipseMirror>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<ChaliceOfTheBloodGod>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<EcologicalCollapse>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<EidolonStaff>(), ItemType<YharimBar>(), 17));
            yharimBarCrafts.Add((ItemType<ElementalQuiver>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<TracersElysian>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<GazeOfCrysthamyr>(), ItemType<YharimBar>(), 8));
            yharimBarCrafts.Add((ItemType<GodSlayerSlug>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<GrandReef>(), ItemType<YharimBar>(), 20));
            yharimBarCrafts.Add((ItemType<HadopelagicEcho>(), ItemType<YharimBar>(), 2));
            yharimBarCrafts.Add((ItemType<HalibutCannon>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemType<HolyMantle>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<MagnaCore>(), ItemType<YharimBar>(), 49));
            yharimBarCrafts.Add((ItemType<Megaskeet>(), ItemType<YharimBar>(), 28));
            yharimBarCrafts.Add((ItemType<Nanotech>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<Nucleogenesis>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<OmegaHealingPotion>(), ItemType<YharimBar>(), 2));
            yharimBarCrafts.Add((ItemType<PlasmaGrenade>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<QuiverofMadness>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<RampartofDeities>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<RoguesLootbox>(), ItemType<YharimBar>(), 22));
            yharimBarCrafts.Add((ItemType<ScorchedEarth>(), ItemType<YharimBar>(), 3));
            yharimBarCrafts.Add((ItemType<Slimelgamation>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<TheAmalgam>(), ItemType<YharimBar>(), 12));
            yharimBarCrafts.Add((ItemType<TheDevourerofCods>(), ItemType<YharimBar>(), 10));
            yharimBarCrafts.Add((ItemType<TheSponge>(), ItemType<YharimBar>(), 14));
            yharimBarCrafts.Add((ItemType<ThrowersGauntlet>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<TheDreamingGhost>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<TyrantShield>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<UniversalStone>(), ItemType<YharimBar>(), 26));
            yharimBarCrafts.Add((ItemType<WrathoftheCosmos>(), ItemType<YharimBar>(), 3));
            yharimBarCrafts.Add((ItemType<WrathoftheDragons>(), ItemType<YharimBar>(), 3));
            yharimBarCrafts.Add((ItemType<ZenithArcanum>(), ItemType<YharimBar>(), 24));
            yharimBarCrafts.Add((ItemID.Zenith, ItemType<YharimBar>(), 19));
            #endregion
            #region Crocodile Scales
            crocodileCrafts.Add((ItemType<DreadmineStaff>(), ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ItemType<FlakKraken>(), ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ItemType<BallisticPoisonBomb>(), ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ItemType<Floodtide>(), ItemType<CrocodileScale>(), 5));
            crocodileCrafts.Add((ItemType<LumenousAmulet>(), ItemType<CrocodileScale>(), 5));
            crocodileCrafts.Add((ItemType<TyphonsGreed>(), ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ItemType<UndinesRetribution>(), ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ItemType<AbyssBlade>(), ItemType<CrocodileScale>(), 5));
            crocodileCrafts.Add((ItemType<BarracudaGun>(), ItemType<CrocodileScale>(), 5));
            #endregion
            #region Coyote Venom
            venomCrafts.Add((ItemType<StarlightWings>(), ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ItemType<PlaguebringerPistons>(), ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ItemType<PlaguebringerVisor>(), ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ItemType<PlaguebringerCarapace>(), ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ItemType<PlagueCellCanister>(), ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ItemType<CirnogenicStaff>(), ItemType<CoyoteVenom>(), 1));
            #endregion
            #region Shimmer Essences
            shimmerEssenceCrafts.Add((ItemType<AstrealDefeat>(), ItemType<EssenceofZot>(), 5));
            shimmerEssenceCrafts.Add((ItemType<ChainSpear>(), ItemType<EssenceofZot>(), 10));
            shimmerEssenceCrafts.Add((ItemType<CosmicAnvilItem>(), ItemType<EssenceofZot>(), 10));
            shimmerEssenceCrafts.Add((ItemType<CosmicWorm>(), ItemType<EssenceofZot>(), 30));
            shimmerEssenceCrafts.Add((ItemType<DaawnlightSpiritOrigin>(), ItemType<EssenceofZot>(), 4));
            shimmerEssenceCrafts.Add((ItemType<SunSpiritStaff>(), ItemType<EssenceofZot>(), 2));
            shimmerEssenceCrafts.Add((ItemType<Crystalline>(), ItemType<EssenceofCrystal>(), 5));
            shimmerEssenceCrafts.Add((ItemType<LunicEye>(), ItemType<EssenceofCrystal>(), 15));
            shimmerEssenceCrafts.Add((ItemType<StormSurge>(), ItemType<EssenceofCrystal>(), 5));
            shimmerEssenceCrafts.Add((ItemType<EnchantedAxe>(), ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ItemID.EnchantedSword, ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ItemType<GearworkShield>(), ItemType<EssenceofLaw>(), 10));
            shimmerEssenceCrafts.Add((ItemType<SpiritGlyph>(), ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ItemID.Starfury, ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ItemType<EpidemicShredder>(), ItemType<EssenceofMyst>(), 105));
            shimmerEssenceCrafts.Add((ItemType<PlantationStaff>(), ItemType<EssenceofMyst>(), 10));
            shimmerEssenceCrafts.Add((ItemType<PlagueReaperMask>(), ItemType<EssenceofMyst>(), 11));
            shimmerEssenceCrafts.Add((ItemType<PlagueReaperStriders>(), ItemType<EssenceofMyst>(), 17));
            shimmerEssenceCrafts.Add((ItemType<PlagueReaperVest>(), ItemType<EssenceofMyst>(), 19));
            shimmerEssenceCrafts.Add((ItemType<XenonCutlass>(), ItemType<EssenceofMyst>(), 10));
            shimmerEssenceCrafts.Add((ItemType<EssenceOfMerge>(), ItemType<EssenceofMyst>(), 10));
            shimmerEssenceCrafts.Add((ItemType<EssenceOfMerge>(), ItemType<EssenceofCrystal>(), 10));
            shimmerEssenceCrafts.Add((ItemType<EssenceOfMerge>(), ItemType<EssenceofLaw>(), 10));
            shimmerEssenceCrafts.Add((ItemType<EssenceOfMerge>(), ItemType<EssenceofZot>(), 10));
            shimmerEssenceCrafts.Add((ItemType<EssenceOfMerge>(), ItemType<EssenceofSurt>(), 10));
            #endregion

            #region Delicious Meat
            deliciousMeatCrafts.Add((ItemType<Abaddon>(), ItemType<DeliciousMeat>(), 100));
            deliciousMeatCrafts.Add((ItemType<DefiledGreatsword>(), ItemType<DeliciousMeat>(), 800));
            deliciousMeatCrafts.Add((ItemType<DraedonsForge>(), ItemType<DeliciousMeat>(), 65536));
            deliciousMeatCrafts.Add((ItemType<EssentialEssenceBar>(), ItemType<DeliciousMeat>(), 55));
            deliciousMeatCrafts.Add((ItemID.MoneyTrough, ItemType<DeliciousMeat>(), 100));
            deliciousMeatCrafts.Add((ItemType<RuinMedallion>(), ItemType<DeliciousMeat>(), 50));
            deliciousMeatCrafts.Add((ItemType<SupremeHealingPotion>(), ItemType<DeliciousMeat>(), 8192));
            deliciousMeatCrafts.Add((ItemType<TyrantShield>(), ItemType<DeliciousMeat>(), 349));
            deliciousMeatCrafts.Add((ItemType<ZenPotion>(), ItemType<DeliciousMeat>(), 256));
            deliciousMeatCrafts.Add((ItemType<ZergPotion>(), ItemType<DeliciousMeat>(), 256));
            #endregion

            #region Accessory Additions
            accessoryCrafts.Add((ItemType<GrandGelatin>(), ItemType<MirageJellyItem>(), 1));
            accessoryCrafts.Add((ItemType<GrandGelatin>(), ItemType<IrateJelly>(), 1));
            accessoryCrafts.Add((ItemType<GrandGelatin>(), ItemType<ElasticJelly>(), 1));
            accessoryCrafts.Add((ItemType<GrandGelatin>(), ItemType<InvigoratingJelly>(), 1));
            accessoryCrafts.Add((ItemType<TheAbsorber>(), ItemType<Regenator>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<TheAbsorber>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<AquaticHeart>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<FlameLickedShell>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<TrinketofChi>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<AmidiasSpark>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<UrsaSergeant>(), 1));
            accessoryCrafts.Add((ItemType<TheSponge>(), ItemType<PermafrostsConcoction>(), 1));
            accessoryCrafts.Add((ItemType<RampartofDeities>(), ItemType<RustyMedallion>(), 1));
            accessoryCrafts.Add((ItemType<RampartofDeities>(), ItemType<AmidiasPendant>(), 1));
            accessoryCrafts.Add((ItemType<TracersElysian>(), ItemType<GravistarSabaton>(), 1));
            accessoryCrafts.Add((ItemType<AmbrosialAmpoule>(), ItemType<HoneyDew>(), 1));
            accessoryCrafts.Add((ItemType<AmbrosialAmpoule>(), ItemType<ArchaicPowder>(), 1));
            accessoryCrafts.Add((ItemType<AbyssalDivingGear>(), ItemType<OceanCrest>(), 1));
            accessoryCrafts.Add((ItemType<AbyssalDivingSuit>(), ItemType<AquaticEmblem>(), 1));
            accessoryCrafts.Add((ItemType<AbyssalDivingSuit>(), ItemType<SpelunkersAmulet>(), 1));
            accessoryCrafts.Add((ItemType<AbyssalDivingSuit>(), ItemType<AlluringBait>(), 1));
            accessoryCrafts.Add((ItemType<AbyssalDivingSuit>(), ItemType<LumenousAmulet>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<GiantPearl>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<ManaPolarizer>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<FrostFlare>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<CorrosiveSpine>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<VoidofExtinction>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<LeviathanAmbergris>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemID.SporeSac, 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<TheCamper>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<AlchemicalFlask>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<ToxicHeart>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<DynamoStemCells>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<BlazingCore>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<TheEvolution>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<Affliction>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<OldDukeScales>(), 1));
            accessoryCrafts.Add((ItemType<TheAmalgam>(), ItemType<FungiStone>(), 1));
            #endregion
        }
    }
}
