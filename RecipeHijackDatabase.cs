using CalamityMod.Items.Armor.Auric;
using CalRemix.Items.Weapons;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Placeables.DraedonStructures;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.Items.Armor.PlagueReaper;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Tools.ClimateChange;
using System.Collections.Generic;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Ammo;
using CalRemix.Items.Materials;
using Terraria.ID;
using CalamityMod.Items.Weapons.Typeless;
using CalamityMod.Items.Accessories.Wings;
using CalRemix.Items.Accessories;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Fishing.FishingRods;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Armor.Plaguebringer;

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

        public override void Load()
        {
            #region Alloy Bars
            alloyBarCrafts.Add((ModContent.ItemType<TheAbsorber>(), ModContent.ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.NightsEdge, ModContent.ItemType<AlloyBar>(), 30));
            alloyBarCrafts.Add((ModContent.ItemType<OverloadedSludge>(), ModContent.ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ModContent.ItemType<BlightedCleaver>(), ModContent.ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ModContent.ItemType<DefiledGreatsword>(), ModContent.ItemType<AlloyBar>(), 340));
            alloyBarCrafts.Add((ModContent.ItemType<Aestheticus>(), ModContent.ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ModContent.ItemType<AgedLaboratoryElectricPanelItem>(), ModContent.ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ModContent.ItemType<FracturedArk>(), ModContent.ItemType<AlloyBar>(), 92));
            alloyBarCrafts.Add((ModContent.ItemType<JellyChargedBattery>(), ModContent.ItemType<AlloyBar>(), 3));
            alloyBarCrafts.Add((ItemID.LifeCrystal, ModContent.ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.MagicMirror, ModContent.ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemID.MoneyTrough, ModContent.ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemID.Muramasa, ModContent.ItemType<AlloyBar>(), 22));
            alloyBarCrafts.Add((ModContent.ItemType<Roxcalibur>(), ModContent.ItemType<AlloyBar>(), 21));
            alloyBarCrafts.Add((ItemID.ShadowKey, ModContent.ItemType<AlloyBar>(), 22));
            alloyBarCrafts.Add((ModContent.ItemType<SunSpiritStaff>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemID.UltrabrightTorch, ModContent.ItemType<AlloyBar>(), 4));
            alloyBarCrafts.Add((ModContent.ItemType<VictideBreastplate>(), ModContent.ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ModContent.ItemType<GeliticBlade>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<Gelpick>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<GunkShot>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<Goobow>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<TheGodsGambit>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelArmor>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelGreaves>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelHeadSummon>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelHeadRogue>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelHeadRanged>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelHeadMelee>(), ModContent.ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ModContent.ItemType<StatigelHeadMagic>(), ModContent.ItemType<AlloyBar>(), 5));
            #endregion
            #region Essential Essence Bars
            essenceBarCrafts.Add((ModContent.ItemType<TheAbsorber>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AmbrosialAmpoule>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AbyssalDivingSuit>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AbyssalDivingGear>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<CosmicAnvilItem>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<CosmicWorm>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AbyssBlade>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<CoreofCalamity>(), ModContent.ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ModContent.ItemType<AstralBeaconItem>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AstrealDefeat>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<ClockworkBow>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<CosmicRainbow>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<Abombination>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AbyssalMirror>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AdvancedDisplay>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AridArtifact>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<TrueArkoftheAncients>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AsgardsValor>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<TrueBiomeBlade>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<BarracudaGun>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<BladedgeGreatbow>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<BrimstoneSword>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<CatastropheClaymore>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<DarklightGreatsword>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<EnhancedNanoRound>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<AbyssalDivingGear>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<ExoticPheromones>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ModContent.ItemType<FlarefrostBlade>(), ModContent.ItemType<EssentialEssenceBar>(), 40));
            #endregion
            #region Yharim Bars
            yharimBarCrafts.Add((ModContent.ItemType<AsgardianAegis>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<AuricBar>(), ModContent.ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaBodyArmor>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaCuisses>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaHoodedFacemask>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaWireHemmedVisage>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaPlumedHelm>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaSpaceHelmet>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<AuricTeslaRoyalHelm>(), ModContent.ItemType<YharimBar>(), 35));
            yharimBarCrafts.Add((ModContent.ItemType<EclipseMirror>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<ChaliceOfTheBloodGod>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<EcologicalCollapse>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<EidolonStaff>(), ModContent.ItemType<YharimBar>(), 17));
            yharimBarCrafts.Add((ModContent.ItemType<ElementalQuiver>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<TracersElysian>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<GazeOfCrysthamyr>(), ModContent.ItemType<YharimBar>(), 50));
            yharimBarCrafts.Add((ModContent.ItemType<GodSlayerSlug>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<GrandReef>(), ModContent.ItemType<YharimBar>(), 30));
            yharimBarCrafts.Add((ModContent.ItemType<HadopelagicEcho>(), ModContent.ItemType<YharimBar>(), 2));
            yharimBarCrafts.Add((ModContent.ItemType<HalibutCannon>(), ModContent.ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ModContent.ItemType<HolyMantle>(), ModContent.ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ModContent.ItemType<MagnaCore>(), ModContent.ItemType<YharimBar>(), 49));
            yharimBarCrafts.Add((ModContent.ItemType<Megaskeet>(), ModContent.ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ModContent.ItemType<Nanotech>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<Nucleogenesis>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<OmegaHealingPotion>(), ModContent.ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ModContent.ItemType<PlasmaGrenade>(), ModContent.ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ModContent.ItemType<QuiverofMadness>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<RampartofDeities>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<RoguesLootbox>(), ModContent.ItemType<YharimBar>(), 22));
            yharimBarCrafts.Add((ModContent.ItemType<ScorchedEarth>(), ModContent.ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ModContent.ItemType<Slimelgamation>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<TheAmalgam>(), ModContent.ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ModContent.ItemType<TheDevourerofCods>(), ModContent.ItemType<YharimBar>(), 10));
            yharimBarCrafts.Add((ModContent.ItemType<TheSponge>(), ModContent.ItemType<YharimBar>(), 100));
            yharimBarCrafts.Add((ModContent.ItemType<ThrowersGauntlet>(), ModContent.ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ModContent.ItemType<TheDreamingGhost>(), ModContent.ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ModContent.ItemType<TyrantShield>(), ModContent.ItemType<YharimBar>(), 124));
            yharimBarCrafts.Add((ModContent.ItemType<UniversalStone>(), ModContent.ItemType<YharimBar>(), 100));
            yharimBarCrafts.Add((ModContent.ItemType<ZenithArcanum>(), ModContent.ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemID.Zenith, ModContent.ItemType<YharimBar>(), 193));
            #endregion
            #region Crocodile Scales
            crocodileCrafts.Add((ModContent.ItemType<DreadmineStaff>(), ModContent.ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ModContent.ItemType<FlakKraken>(), ModContent.ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ModContent.ItemType<BallisticPoisonBomb>(), ModContent.ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ModContent.ItemType<Floodtide>(), ModContent.ItemType<CrocodileScale>(), 5));
            crocodileCrafts.Add((ModContent.ItemType<LumenousAmulet>(), ModContent.ItemType<CrocodileScale>(), 5));
            crocodileCrafts.Add((ModContent.ItemType<TyphonsGreed>(), ModContent.ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ModContent.ItemType<UndinesRetribution>(), ModContent.ItemType<CrocodileScale>(), 10));
            crocodileCrafts.Add((ModContent.ItemType<AbyssBlade>(), ModContent.ItemType<CrocodileScale>(), 5));
            crocodileCrafts.Add((ModContent.ItemType<BarracudaGun>(), ModContent.ItemType<CrocodileScale>(), 5));
            #endregion
            #region Coyote Venom
            venomCrafts.Add((ModContent.ItemType<EnhancedNanoRound>(), ModContent.ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ModContent.ItemType<StarlightWings>(), ModContent.ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ModContent.ItemType<PlaguebringerPistons>(), ModContent.ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ModContent.ItemType<PlaguebringerVisor>(), ModContent.ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ModContent.ItemType<PlaguebringerCarapace>(), ModContent.ItemType<CoyoteVenom>(), 1));
            venomCrafts.Add((ModContent.ItemType<PlagueCellCanister>(), ModContent.ItemType<CoyoteVenom>(), 1));
            #endregion
            #region Shimmer Essences
            shimmerEssenceCrafts.Add((ModContent.ItemType<SunSpiritStaff>(), ModContent.ItemType<EssenceofZot>(), 2));
            shimmerEssenceCrafts.Add((ModContent.ItemType<AstrealDefeat>(), ModContent.ItemType<EssenceofZot>(), 5));
            shimmerEssenceCrafts.Add((ModContent.ItemType<CosmicAnvilItem>(), ModContent.ItemType<EssenceofZot>(), 10));
            shimmerEssenceCrafts.Add((ModContent.ItemType<CosmicWorm>(), ModContent.ItemType<EssenceofZot>(), 30));
            shimmerEssenceCrafts.Add((ModContent.ItemType<DaawnlightSpiritOrigin>(), ModContent.ItemType<EssenceofZot>(), 4));
            shimmerEssenceCrafts.Add((ModContent.ItemType<Crystalline>(), ModContent.ItemType<EssenceofCrystal>(), 5));
            shimmerEssenceCrafts.Add((ModContent.ItemType<LunicEye>(), ModContent.ItemType<EssenceofCrystal>(), 15));
            shimmerEssenceCrafts.Add((ModContent.ItemType<StormSurge>(), ModContent.ItemType<EssenceofCrystal>(), 5));
            shimmerEssenceCrafts.Add((ModContent.ItemType<AccelerationRound>(), ModContent.ItemType<EssenceofLaw>(), 1));
            shimmerEssenceCrafts.Add((ModContent.ItemType<EnchantedAxe>(), ModContent.ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ItemID.EnchantedSword, ModContent.ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ModContent.ItemType<SpiritGlyph>(), ModContent.ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ModContent.ItemType<AccelerationRound>(), ModContent.ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ItemID.Starfury, ModContent.ItemType<EssenceofLaw>(), 5));
            shimmerEssenceCrafts.Add((ModContent.ItemType<EpidemicShredder>(), ModContent.ItemType<EssenceofMyst>(), 105));
            shimmerEssenceCrafts.Add((ModContent.ItemType<PlantationStaff>(), ModContent.ItemType<EssenceofMyst>(), 10));
            shimmerEssenceCrafts.Add((ModContent.ItemType<PlagueReaperMask>(), ModContent.ItemType<EssenceofMyst>(), 11));
            shimmerEssenceCrafts.Add((ModContent.ItemType<PlagueReaperStriders>(), ModContent.ItemType<EssenceofMyst>(), 17));
            shimmerEssenceCrafts.Add((ModContent.ItemType<PlagueReaperVest>(), ModContent.ItemType<EssenceofMyst>(), 19));
            #endregion
        }
    }
}
