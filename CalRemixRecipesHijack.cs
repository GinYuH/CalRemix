﻿using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Plaguebringer;
using CalamityMod.Items.Armor.PlagueReaper;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.DraedonMisc;
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

        public override void Load()
        {
            #region Alloy Bars
            alloyBarCrafts.Add((ItemType<TheAbsorber>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.NightsEdge, ItemType<AlloyBar>(), 30));
            alloyBarCrafts.Add((ItemType<OverloadedSludge>(), ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemType<BlightedCleaver>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<DefiledGreatsword>(), ItemType<AlloyBar>(), 340));
            alloyBarCrafts.Add((ItemType<Aestheticus>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<AgedLaboratoryElectricPanelItem>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<EssentialEssenceBar>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<FracturedArk>(), ItemType<AlloyBar>(), 92));
            alloyBarCrafts.Add((ItemType<JellyChargedBattery>(), ItemType<AlloyBar>(), 3));
            alloyBarCrafts.Add((ItemID.LifeCrystal, ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemID.MagicMirror, ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemID.MoneyTrough, ItemType<AlloyBar>(), 2));
            alloyBarCrafts.Add((ItemID.Muramasa, ItemType<AlloyBar>(), 22));
            alloyBarCrafts.Add((ItemType<Roxcalibur>(), ItemType<AlloyBar>(), 21));
            alloyBarCrafts.Add((ItemID.ShadowKey, ItemType<AlloyBar>(), 22));
            alloyBarCrafts.Add((ItemType<SunSpiritStaff>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemID.UltrabrightTorch, ItemType<AlloyBar>(), 4));
            alloyBarCrafts.Add((ItemType<VictideBreastplate>(), ItemType<AlloyBar>(), 1));
            alloyBarCrafts.Add((ItemType<GeliticBlade>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<Gelpick>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<GunkShot>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<Goobow>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<TheGodsGambit>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelArmor>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelGreaves>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelHeadSummon>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelHeadRogue>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelHeadRanged>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelHeadMelee>(), ItemType<AlloyBar>(), 5));
            alloyBarCrafts.Add((ItemType<StatigelHeadMagic>(), ItemType<AlloyBar>(), 5));
            #endregion
            #region Essential Essence Bars
            essenceBarCrafts.Add((ItemType<TheAbsorber>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AmbrosialAmpoule>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AbyssalDivingSuit>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AbyssalDivingGear>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<CosmicAnvilItem>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<CosmicWorm>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AbyssBlade>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<CoreofCalamity>(), ItemType<EssentialEssenceBar>(), 2));
            essenceBarCrafts.Add((ItemType<AstralBeaconItem>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<Confection>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AstrealDefeat>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<ClockworkBow>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<CosmicRainbow>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<Abombination>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AbyssalMirror>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AdvancedDisplay>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AridArtifact>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<TrueArkoftheAncients>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AsgardsValor>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<TrueBiomeBlade>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<BarracudaGun>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<BladedgeRailbow>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<BrimstoneSword>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<CatastropheClaymore>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<DarklightGreatsword>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<AbyssalDivingGear>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<ExoticPheromones>(), ItemType<EssentialEssenceBar>(), 40));
            essenceBarCrafts.Add((ItemType<FlarefrostBlade>(), ItemType<EssentialEssenceBar>(), 40));
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
            yharimBarCrafts.Add((ItemType<GazeOfCrysthamyr>(), ItemType<YharimBar>(), 50));
            yharimBarCrafts.Add((ItemType<GodSlayerSlug>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<GrandReef>(), ItemType<YharimBar>(), 30));
            yharimBarCrafts.Add((ItemType<HadopelagicEcho>(), ItemType<YharimBar>(), 2));
            yharimBarCrafts.Add((ItemType<HalibutCannon>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemType<HolyMantle>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<MagnaCore>(), ItemType<YharimBar>(), 49));
            yharimBarCrafts.Add((ItemType<Megaskeet>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemType<Nanotech>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<Nucleogenesis>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<OmegaHealingPotion>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemType<PlasmaGrenade>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<QuiverofMadness>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<RampartofDeities>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<RoguesLootbox>(), ItemType<YharimBar>(), 22));
            yharimBarCrafts.Add((ItemType<ScorchedEarth>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemType<Slimelgamation>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<TheAmalgam>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemType<TheDevourerofCods>(), ItemType<YharimBar>(), 10));
            yharimBarCrafts.Add((ItemType<TheSponge>(), ItemType<YharimBar>(), 100));
            yharimBarCrafts.Add((ItemType<ThrowersGauntlet>(), ItemType<YharimBar>(), 1));
            yharimBarCrafts.Add((ItemType<TheDreamingGhost>(), ItemType<YharimBar>(), 4));
            yharimBarCrafts.Add((ItemType<TyrantShield>(), ItemType<YharimBar>(), 124));
            yharimBarCrafts.Add((ItemType<UniversalStone>(), ItemType<YharimBar>(), 100));
            yharimBarCrafts.Add((ItemType<WrathoftheCosmos>(), ItemType<YharimBar>(), 3));
            yharimBarCrafts.Add((ItemType<WrathoftheDragons>(), ItemType<YharimBar>(), 3));
            yharimBarCrafts.Add((ItemType<ZenithArcanum>(), ItemType<YharimBar>(), 99));
            yharimBarCrafts.Add((ItemID.Zenith, ItemType<YharimBar>(), 193));
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
        }
    }
}
