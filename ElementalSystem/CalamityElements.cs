using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Typeless;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Tools;
using System;
using CalamityMod.Items.Accessories;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.Yharon;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.PrimordialWyrm;

namespace CalRemix.ElementalSystem
{
    public static class CalamityElements
    {
        internal static Dictionary<int, Element[]> Item = new()
        {
            #region Melee
            #region Pre-Hardmode
            
            { ItemType<AirSpinner>(), new Element[]{ Element.Impact, Element.Holy }},
            { ItemType<AmidiasTrident>(), new Element[]{ Element.Stab , Element.Water }},
            { ItemType<Aorta>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<BallOFugu>(), new Element[]{ Element.Stab, Element.Magic }},
            { ItemType<Basher>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<BladecrestOathsword>(), new Element[]{  Element.Slash, Element.Unholy }},
            { ItemType<BrokenBiomeBlade>(), new Element[]{ Element.Slash }},
            { ItemType<BurntSienna>(), new Element[]{ Element.Slash, Element.Holy }},
            { ItemType<DepthCrusher>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<FellerofEvergreens>(), new Element[]{ Element.Slash }},
            { ItemType<FracturedArk>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<GaussDagger>(), new Element[]{ Element.Slash, Element.Machine }},
            { ItemType<GeliticBlade>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<GoldplumeSpear>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<MonstrousKnives>(), new Element[]{ Element.Stab }},
            { ItemType<MycelialClaws>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<OldLordClaymore>(), new Element[]{  Element.Slash, Element.Fire }},
            { ItemType<PerfectDark>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<RedtideSpear>(), new Element[]{ Element.Stab, Element.Water }},
            { ItemType<Riptide>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<SaharaSlicers>(), new Element[]{ Element.Slash }},
            { ItemType<SausageMaker>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<SeashineSword>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<SmokingComet>(), new Element[]{ Element.Impact, Element.Magic }},
            { ItemType<TaintedBlade>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<TeardropCleaver>(), new Element[]{ Element.Slash }},
            { ItemType<TheGodsGambit>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<UrchinFlail>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<UrchinMace>(), new Element[]{ Element.Stab, Element.Water }},
            { ItemType<VeinBurster>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<WindBlade>(), new Element[]{ Element.Slash, Element.Holy }},
            { ItemType<WulfrumScrewdriver>(), new Element[]{ Element.Stab, Element.Machine }},
            { ItemType<YateveoBloom>(), new Element[]{ Element.Stab, Element.Poison }},
            
            #endregion
            #region Hardmode
            
            { ItemType<AbsoluteZero>(), new Element[]{ Element.Slash, Element.Cold }},
            { ItemType<AbyssBlade>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<AegisBlade>(), new Element[]{ Element.Slash, Element.Holy }},
            { ItemType<Aftershock>(), new Element[]{ Element.Slash }},
            { ItemType<AnarchyBlade>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<TrueArkoftheAncients>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<AstralBlade>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<AstralPike>(), new Element[]{ Element.Stab, Element.Magic }},
            { ItemType<AstralScythe>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<Avalanche>(), new Element[]{ Element.Slash, Element.Cold }},
            { ItemType<AxeofPurity>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<BalefulHarvester>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<TrueBiomeBlade>(), new Element[]{ Element.Slash }},
            { ItemType<BlightedCleaver>(), new Element[]{ Element.Slash, Element.Poison, Element.Fire }},
            { ItemType<Bonebreaker>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<Brimlance>(), new Element[]{ Element.Stab, Element.Unholy, Element.Fire }},
            { ItemType<Brimlash>(), new Element[]{ Element.Slash, Element.Unholy, Element.Fire }},
            { ItemType<BrinyBaron>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<Carnage>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<CatastropheClaymore>(), new Element[]{ Element.Slash, Element.Fire, Element.Cold }},
            { ItemType<TrueCausticEdge>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<CelestialClaymore>(), new Element[]{ Element.Slash, Element.Fire, Element.Cold }},
            { ItemType<ClamCrusher>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<CometQuasher>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<DarklightGreatsword>(), new Element[]{ Element.Slash, Element.Cold, Element.Dark }},
            { ItemType<DiseasedPike>(), new Element[]{ Element.Stab, Element.Poison, Element.Machine }},
            { ItemType<EarthenPike>(), new Element[]{ Element.Stab }},
            { ItemType<EntropicClaymore>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<EutrophicScimitar>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<EvilSmasher>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<ExaltedOathblade>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<FallenPaladinsHammer>(), new Element[]{ Element.Impact, Element.Unholy, Element.Fire }},
            { ItemType<FaultLine>(), new Element[]{ Element.Impact, Element.Fire }},
            { ItemType<FeralthornClaymore>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<FlarefrostBlade>(), new Element[]{ Element.Slash, Element.Fire, Element.Cold }},
            { ItemType<Floodtide>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<ForbiddenOathblade>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<ForsakenSaber>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<GalvanizingGlaive>(), new Element[]{ Element.Stab, Element.Machine }},
            { ItemType<GrandGuardian>(), new Element[]{ Element.Slash, Element.Holy }},
            { ItemType<Greentide>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<HellfireFlamberge>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<HellionFlowerSpear>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<Hellkite>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<InfernaCutter>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<Lucrecia>(), new Element[]{ Element.Stab }},
            { ItemType<MajesticGuard>(), new Element[]{ Element.Slash }},
            { ItemType<MantisClaws>(), new Element[]{ Element.Slash }},
            { ItemType<Nebulash>(), new Element[]{ Element.Slash, Element.Machine, Element.Magic }},
            { ItemType<Oblivion>(), new Element[]{ Element.Impact, Element.Unholy }},
            { ItemType<Omniblade>(), new Element[]{ Element.Slash }},
            { ItemType<Pandemic>(), new Element[]{ Element.Impact, Element.Poison, Element.Machine }},
            { ItemType<Pwnagehammer>(), new Element[]{ Element.Impact, Element.Holy }},
            { ItemType<Quagmire>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<Roxcalibur>(), new Element[]{ Element.Impact }},
            { ItemType<Shimmerspark>(), new Element[]{ Element.Impact, Element.Cold }},
            { ItemType<SoulHarvester>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<StarnightLance>(), new Element[]{ Element.Stab, Element.Cold }},
            { ItemType<StormRuler>(), new Element[]{ Element.Slash, Element.Holy }},
            { ItemType<StormSaber>(), new Element[]{ Element.Slash, Element.Holy }},
            { ItemType<StygianShield>(), new Element[]{ Element.Impact }},
            { ItemType<SubmarineShocker>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<SulphurousGrabber>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<TenebreusTides>(), new Element[]{ Element.Stab, Element.Water }},
            { ItemType<TerraLance>(), new Element[]{ Element.Stab }},
            { ItemType<TheMicrowave>(), new Element[]{ Element.Impact, Element.Magic }},
            { ItemType<TitanArm>(), new Element[]{ Element.Impact, Element.Magic }},
            { ItemType<OmegaBiomeBlade>(), new Element[]{ Element.Slash }},
            { ItemType<Tumbleweed>(), new Element[]{ Element.Impact, Element.Fire }},
            { ItemType<TyphonsGreed>(), new Element[]{ Element.Stab, Element.Water }},
            { ItemType<UltimusCleaver>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<Virulence>(), new Element[]{ Element.Impact, Element.Poison, Element.Machine }},
            { ItemType<VulcaniteLance>(), new Element[]{ Element.Stab, Element.Fire }},
            { ItemType<YinYo>(), new Element[]{ Element.Impact }},
            
            #endregion
            #region Godseeker
            
            { ItemType<ArkoftheCosmos>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<ArkoftheElements>(), new Element[]{ Element.Slash, Element.Fire, Element.Magic, Element.Poison, Element.Cold }},
            { ItemType<Ataraxia>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<Azathoth>(), new Element[]{ Element.Impact }},
            { ItemType<BansheeHook>(), new Element[]{ Element.Stab }},
            { ItemType<CosmicDischarge>(), new Element[]{ Element.Impact, Element.Cold, Element.Dark }},
            { ItemType<CosmicShiv>(), new Element[]{ Element.Stab, Element.Magic, Element.Dark }},
            { ItemType<CrescentMoon>(), new Element[]{ Element.Impact, Element.Cold, Element.Dark }},
            { ItemType<DefiledGreatsword>(), new Element[]{ Element.Slash, Element.Holy, Element.Fire }},
            { ItemType<Devastation>(), new Element[]{ Element.Slash, Element.Cold, Element.Fire }},
            { ItemType<DevilsDevastation>(), new Element[]{ Element.Slash, Element.Unholy, Element.Fire }},
            { ItemType<DevilsSunrise>(), new Element[]{ Element.Slash, Element.Unholy, Element.Dark }},
            { ItemType<DraconicDestruction>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<DragonPow>(), new Element[]{ Element.Impact, Element.Holy, Element.Fire }},
            { ItemType<DragonRage>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<Earth>(), new Element[]{ Element.Slash, Element.Poison, Element.Cold, Element.Fire }},
            { ItemType<ElementalLance>(), new Element[]{ Element.Slash, Element.Fire, Element.Magic, Element.Poison, Element.Cold }},
            { ItemType<ElementalShiv>(), new Element[]{ Element.Slash, Element.Fire, Element.Magic, Element.Poison, Element.Cold }},
            { ItemType<EmpyreanKnives>(), new Element[]{ Element.Stab }},
            { ItemType<EssenceFlayer>(), new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { ItemType<Excelsus>(), new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { ItemType<Exoblade>(), new Element[]{ Element.Slash, Element.Machine}},
            { ItemType<GaelsGreatsword>(), new Element[]{ Element.Slash, Element.Unholy}},
            { ItemType<GalactusBlade>(), new Element[]{ Element.Slash, Element.Magic}},
            { ItemType<FourSeasonsGalaxia>(), new Element[]{ Element.Slash, Element.Magic}},
            { ItemType<GalaxySmasher>(), new Element[]{ Element.Impact, Element.Magic, Element.Dark }},
            { ItemType<GalileoGladius>(), new Element[]{ Element.Stab, Element.Cold, Element.Dark }},
            { ItemType<GildedProboscis>(), new Element[]{ Element.Stab, Element.Fire }},
            { ItemType<Grax>(), new Element[]{ Element.Slash }},
            { ItemType<GreatswordofJudgement>(), new Element[]{ Element.Slash }},
            { ItemType<HolyCollider>(), new Element[]{ Element.Slash, Element.Holy, Element.Fire }},
            { ItemType<IllustriousKnives>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<IridescentExcalibur>(), new Element[]{ Element.Slash }},
            { ItemType<Lacerator>(), new Element[]{ Element.Impact, Element.Poison, Element.Dark }},
            { ItemType<LifefruitScythe>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<LionHeart>(), new Element[]{ Element.Slash, Element.Machine }},
            { ItemType<MirrorBlade>(), new Element[]{ Element.Slash }},
            { ItemType<Mourningstar>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<Murasama>(), new Element[]{ Element.Slash, Element.Machine }},
            { ItemType<Nadir>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<NeptunesBounty>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<Orderbringer>(), new Element[]{ Element.Slash }},
            { ItemType<Phaseslayer>(), new Element[]{ Element.Slash, Element.Machine }},
            { ItemType<PhosphorescentGauntlet>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<PhotonRipper>(), new Element[]{ Element.Slash, Element.Machine }},
            { ItemType<PlagueKeeper>(), new Element[]{ Element.Impact, Element.Poison, Element.Machine }},
            { ItemType<PrismaticBreaker>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<PulseDragon>(), new Element[]{ Element.Impact, Element.Machine }},
            { ItemType<RedSun>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<RemsRevenge>(), new Element[]{ Element.Impact, Element.Unholy, Element.Dark }},
            { ItemType<ScourgeoftheCosmos>(), new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { ItemType<SeekingScorcher>(), new Element[]{ Element.Slash, Element.Holy, Element.Fire }},
            { ItemType<SolarFlare>(), new Element[]{ Element.Slash, Element.Holy, Element.Fire }},
            { ItemType<SolsticeClaymore>(), new Element[]{ Element.Slash }},
            { ItemType<SoulEdge>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<SpineOfThanatos>(), new Element[]{ Element.Slash, Element.Machine }},
            { ItemType<StellarContempt>(), new Element[]{ Element.Impact }},
            { ItemType<StellarStriker>(), new Element[]{ Element.Slash }},
            { ItemType<StreamGouge>(), new Element[]{ Element.Stab, Element.Magic, Element.Dark }},
            { ItemType<Swordsplosion>(), new Element[]{ Element.Slash }},
            { ItemType<Terratomere>(), new Element[]{ Element.Slash }},
            { ItemType<TerrorBlade>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<TheBurningSky>(), new Element[]{ Element.Fire }},
            { ItemType<TheEnforcer>(), new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { ItemType<TheLastMourning>(), new Element[]{ Element.Slash, Element.Dark, Element.Fire }},
            { ItemType<TheMutilator>(), new Element[]{ Element.Slash, Element.Poison, Element.Dark }},
            { ItemType<TheObliterator>(), new Element[]{ Element.Impact, Element.Magic, Element.Dark }},
            { ItemType<Oracle>(), new Element[]{ Element.Impact, Element.Magic, Element.Holy }},
            { ItemType<TriactisTruePaladinianMageHammerofMightMelee>(), new Element[]{ Element.Impact }},
            { ItemType<Violence>(), new Element[]{ Element.Impact, Element.Unholy }},
            
            #endregion
            #endregion

            #region Ranged
            #region Pre-Hardmode
            #endregion
            #region Hardmode
            #endregion
            #region Godseeker
            #endregion
            #endregion

            #region Magic
            #region Pre-Hardmode
            #endregion
            #region Hardmode
            #endregion
            #region Godseeker
            #endregion
            #endregion

            #region Summon
            #region Pre-Hardmode
            { ItemType<BelladonnaSpiritStaff>(), new Element[]{ Element.Poison }},
            { ItemType<BrittleStarStaff>(), new Element[]{ Element.Impact }},
            { ItemType<CausticCroakerStaff>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<CinderBlossomStaff>(), new Element[]{ Element.Fire }},
            { ItemType<Cnidarian>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<CorroslimeStaff>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<CrimslimeStaff>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<DankStaff>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<DeathstareRod>(), new Element[]{ Element.Dark }},
            { ItemType<EnchantedConch>(), new Element[]{ Element.Impact }},
            { ItemType<EyeOfNight>(), new Element[]{ Element.Dark }},
            { ItemType<FleshOfInfidelity>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<FrostBlossomStaff>(), new Element[]{ Element.Cold }},
            { ItemType<HerringStaff>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<PolypLauncher>(), new Element[]{ Element.Water }},
            { ItemType<PuffShroom>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<RustyBeaconPrototype>(), new Element[]{ Element.Machine, Element.Poison }},
            { ItemType<ScabRipper>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<SlimePuppetStaff>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<SquirrelSquireStaff>(), new Element[]{ Element.Magic }},
            { ItemType<StaffOfNecrosteocytes>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<StarSwallowerContainmentUnit>(), new Element[]{ Element.Machine }},
            { ItemType<StormjawStaff>(), new Element[]{ Element.Impact }},
            { ItemType<SunSpiritStaff>(), new Element[]{ Element.Holy }},
            { ItemType<VileFeeder>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<WulfrumController>(), new Element[]{ Element.Machine }},
            #endregion
            #region Hardmode
            { ItemType<AncientIceChunk>(), new Element[]{ Element.Impact, Element.Cold }},
            { ItemType<BlackHawkRemote>(), new Element[]{ Element.Machine }},
            { ItemType<BorealisBomber>(), new Element[]{ Element.Impact, Element.Machine, Element.Magic }},
            { ItemType<CausticStaff>(), new Element[]{ Element.Poison }},
            { ItemType<CryogenicStaff>(), new Element[]{ Element.Cold }},
            { ItemType<DaedalusGolemStaff>(), new Element[]{ Element.Machine, Element.Cold }},
            { ItemType<DeepseaStaff>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<DormantBrimseeker>(), new Element[]{ Element.Unholy }},
            { ItemType<DreadmineStaff>(), new Element[]{ Element.Water }},
            { ItemType<EntropysVigil>(), new Element[]{ Element.Impact, Element.Unholy }},
            { ItemType<ForgottenApexWand>(), new Element[]{ Element.Impact }},
            { ItemType<FuelCellBundle>(), new Element[]{ Element.Poison, Element.Machine }},
            { ItemType<GastricBelcherStaff>(), new Element[]{ Element.Poison }},
            { ItemType<GlacialEmbrace>(), new Element[]{ Element.Cold }},
            { ItemType<HauntedScroll>(), new Element[]{ Element.Impact }},
            { ItemType<HivePod>(), new Element[]{  Element.Magic }},
            { ItemType<IgneousExaltation>(), new Element[]{ Element.Stab, Element.Fire }},
            { ItemType<InfectedRemote>(), new Element[]{ Element.Poison, Element.Machine }},
            { ItemType<MountedScanner>(), new Element[]{ Element.Machine }},
            { ItemType<OrthoceraShell>(), new Element[]{ Element.Poison }},
            { ItemType<PlantationStaff>(), new Element[]{ Element.Poison }},
            { ItemType<PulseTurretRemote>(), new Element[]{ Element.Machine }},
            { ItemType<ResurrectionButterfly>(), new Element[]{ Element.Magic }},
            { ItemType<SandSharknadoStaff>(), new Element[]{ Element.Fire }},
            { ItemType<ShellfishStaff>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<SpikecragStaff>(), new Element[]{ Element.Dark }},
            { ItemType<StarspawnHelixStaff>(), new Element[]{ Element.Slash, Element.Machine, Element.Magic }},
            { ItemType<TundraFlameBlossomsStaff>(), new Element[]{ Element.Cold, Element.Fire }},
            { ItemType<VengefulSunStaff>(), new Element[]{ Element.Holy }},
            { ItemType<ViralSprout>(), new Element[]{ Element.Poison }},
            { ItemType<WitherBlossomsStaff>(), new Element[]{ Element.Poison, Element.Machine }},
            #endregion
            #region Godseeker
            { ItemType<AquasScepter>(), new Element[]{ Element.Water }},
            { ItemType<AresExoskeleton>(), new Element[]{ Element.Machine }},
            { ItemType<AtlasMunitionsBeacon>(), new Element[]{ Element.Machine }},
            { ItemType<CadaverousCarrion>(), new Element[]{ Element.Poison }},
            { ItemType<CalamarisLament>(), new Element[]{ Element.Water }},
            { ItemType<CindersOfLament>(), new Element[]{ Element.Impact, Element.Unholy }},
            { ItemType<CorvidHarbringerStaff>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<CosmicImmaterializer>(), new Element[]{ Element.Magic, Element.Machine }},
            { ItemType<CosmicViperEngine>(), new Element[]{ Element.Machine, Element.Magic, Element.Dark }},
            { ItemType<Cosmilamp>(), new Element[]{ Element.Magic, Element.Dark }},
            { ItemType<DazzlingStabberStaff>(), new Element[]{ Element.Stab, Element.Holy, Element.Fire }},
            { ItemType<DragonbloodDisgorger>(), new Element[]{ Element.Poison, Element.Dark }},
            { ItemType<ElementalAxe>(), new Element[]{ Element.Slash, Element.Fire, Element.Magic, Element.Poison, Element.Cold }},
            { ItemType<EndoHydraStaff>(), new Element[]{ Element.Cold }},
            { ItemType<Endogenesis>(), new Element[]{ Element.Cold }},
            { ItemType<EtherealSubjugator>(), new Element[]{ Element.Cold, Element.Dark }},
            { ItemType<FlamsteedRing>(), new Element[]{ Element.Machine }},
            { ItemType<FlowersOfMortality>(), new Element[]{ Element.Fire, Element.Magic, Element.Poison, Element.Cold }},
            { ItemType<GammaHeart>(), new Element[]{ Element.Poison }},
            { ItemType<GuidelightofOblivion>(), new Element[]{ Element.Fire, Element.Dark }},
            { ItemType<KingofConstellationsTenryu>(), new Element[]{ Element.Impact, Element.Fire }},
            { ItemType<Metastasis>(), new Element[]{ Element.Impact, Element.Unholy }},
            { ItemType<MidnightSunBeacon>(), new Element[]{ Element.Machine }},
            { ItemType<MirrorofKalandra>(), new Element[]{ Element.Holy }},
            { ItemType<MutatedTruffle>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<Perdition>(), new Element[]{ Element.Unholy }},
            { ItemType<SanctifiedSpark>(), new Element[]{ Element.Holy }},
            { ItemType<SarosPossession>(), new Element[]{ Element.Holy, Element.Dark }},
            { ItemType<Sirius>(), new Element[]{ Element.Cold, Element.Dark }},
            { ItemType<SnakeEyes>(), new Element[]{ Element.Machine }},
            { ItemType<StaffoftheMechworm>(), new Element[]{ Element.Machine, Element.Magic, Element.Dark }},
            { ItemType<StellarTorusStaff>(), new Element[]{ Element.Magic }},
            { ItemType<TacticalPlagueEngine>(), new Element[]{ Element.Poison, Element.Machine }},
            { ItemType<TemporalUmbrella>(), new Element[]{ Element.Magic }},
            { ItemType<UniverseSplitter>(), new Element[]{ Element.Machine }},
            { ItemType<Vigilance>(), new Element[]{ Element.Unholy }},
            { ItemType<ViridVanguard>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<VoidConcentrationStaff>(), new Element[]{ Element.Dark }},
            { ItemType<WarloksMoonFist>(), new Element[]{ Element.Impact, Element.Magic }},
            { ItemType<YharonsKindleStaff>(), new Element[]{ Element.Fire }},
            #endregion
            #endregion
            
            #region Rogue
            #region Pre-Hardmode
            { ItemType<AshenStalactite>(), new Element[]{ Element.Stab }},
            { ItemType<BouncingEyeball>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<BouncySpikyBall>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<Cinquedea>(), new Element[]{ Element.Stab }},
            { ItemType<ContaminatedBile>(), new Element[]{ Element.Poison }},
            { ItemType<Crystalline>(), new Element[]{ Element.Stab }},
            { ItemType<EnchantedAxe>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<FeatherKnife>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<FishboneBoomerang>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<GelDart>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<GildedDagger>(), new Element[]{ Element.Stab }},
            { ItemType<Glaive>(), new Element[]{ Element.Slash }},
            { ItemType<GleamingDagger>(), new Element[]{ Element.Stab }},
            { ItemType<HardenedHoneycomb>(), new Element[]{ Element.Impact }},
            { ItemType<InfernalKris>(), new Element[]{ Element.Stab, Element.Fire }},
            { ItemType<InfestedClawmerang>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<IronFrancisca>(), new Element[]{ Element.Slash }},
            { ItemType<Lionfish>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<MetalMonstrosity>(), new Element[]{ Element.Stab }},
            { ItemType<MeteorFist>(), new Element[]{ Element.Impact, Element.Fire }},
            { ItemType<Mycoroot>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<NastyCholla>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<PoisonPack>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<RotBall>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<SandDollar>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<ScourgeoftheDesert>(), new Element[]{ Element.Stab, Element.Fire }},
            { ItemType<SeafoamBomb>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<ShinobiBlade>(), new Element[]{ Element.Stab }},
            { ItemType<SkyStabber>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<SludgeSplotch>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<SnapClam>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<StickySpikyBall>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<ThrowingBrick>(), new Element[]{ Element.Impact }},
            { ItemType<ToothBall>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<TrackingDisk>(), new Element[]{ Element.Machine }},
            { ItemType<Turbulance>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<UrchinStinger>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<WebBall>(), new Element[]{ Element.Impact }},
            { ItemType<WulfrumKnife>(), new Element[]{ Element.Stab, Element.Machine }},
            #endregion
            #region Hardmode
            { ItemType<AcidicRainBarrel>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<AdamantiteThrowingAxe>(), new Element[]{ Element.Slash }},
            { ItemType<Apoctolith>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<AuroradicalThrow>(), new Element[]{ Element.Slash, Element.Machine, Element.Magic }},
            { ItemType<BallisticPoisonBomb>(), new Element[]{ Element.Impact, Element.Poison }},
            { ItemType<BlastBarrel>(), new Element[]{ Element.Impact, Element.Fire }},
            { ItemType<BlazingStar>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<BrackishFlask>(), new Element[]{ Element.Poison }},
            { ItemType<Brimblade>(), new Element[]{ Element.Slash, Element.Unholy }},
            { ItemType<BurningStrife>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<CobaltKunai>(), new Element[]{ Element.Stab }},
            { ItemType<ConsecratedWater>(), new Element[]{ Element.Water, Element.Holy }},
            { ItemType<CorpusAvertor>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<CraniumSmasher>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<CrushsawCrasher>(), new Element[]{ Element.Impact, Element.Unholy }},
            { ItemType<CrystalPiercer>(), new Element[]{ Element.Stab, Element.Cold }},
            { ItemType<CursedDagger>(), new Element[]{ Element.Stab, Element.Fire }},
            { ItemType<DeepWounder>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<DefectiveSphere>(), new Element[]{ Element.Stab, Element.Machine }},
            { ItemType<DesecratedWater>(), new Element[]{ Element.Water, Element.Dark }},
            { ItemType<DukesDecapitator>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<DuststormInABottle>(), new Element[]{ Element.Fire }},
            { ItemType<EpidemicShredder>(), new Element[]{ Element.Slash, Element.Poison, Element.Machine }},
            { ItemType<Equanimity>(), new Element[]{ Element.Impact }},
            { ItemType<Exorcism>(), new Element[]{ Element.Impact, Element.Holy }},
            { ItemType<FantasyTalisman>(), new Element[]{ Element.Dark }},
            { ItemType<FrequencyManipulator>(), new Element[]{ Element.Stab, Element.Machine }},
            { ItemType<FrostcrushValari>(), new Element[]{ Element.Impact, Element.Cold }},
            { ItemType<FrostyFlare>(), new Element[]{ Element.Cold }},
            { ItemType<GacruxianMollusk>(), new Element[]{ Element.Machine, Element.Magic }},
            { ItemType<GraveGrimreaver>(), new Element[]{ Element.Slash, Element.Dark }},
            { ItemType<HeavenfallenStardisk>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<IceStar>(), new Element[]{ Element.Slash, Element.Cold }},
            { ItemType<Icebreaker>(), new Element[]{ Element.Impact, Element.Cold }},
            { ItemType<IchorSpear>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<KelvinCatalyst>(), new Element[]{ Element.Slash, Element.Cold }},
            { ItemType<LeonidProgenitor>(), new Element[]{ Element.Magic }},
            { ItemType<LeviathanTeeth>(), new Element[]{ Element.Stab, Element.Poison }},
            { ItemType<Malachite>(), new Element[]{ Element.Stab }},
            { ItemType<MangroveChakram>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<MonkeyDarts>(), new Element[]{ Element.Stab }},
            { ItemType<MythrilKnife>(), new Element[]{ Element.Stab }},
            { ItemType<Nychthemeron>(), new Element[]{ Element.Stab }},
            { ItemType<OrichalcumSpikedGemstone>(), new Element[]{ Element.Stab }},
            #endregion
            #region Godseeker
            #endregion
            #endregion

            #region Classless
            #region Pre-Hardmode
            { ItemType<Aestheticus>(), new Element[]{ Element.Magic }},
            { ItemType<Skynamite>(), new Element[]{ Element.Impact, Element.Holy }},
            #endregion
            #region Hardmode
            { ItemType<EyeofMagnus>(), new Element[]{ Element.Magic }},
            { ItemType<GoldenGun>(), new Element[]{ Element.Magic }},
            { ItemType<LunicEye>(), new Element[]{ Element.Dark }},
            { ItemType<StarStruckWater>(), new Element[]{ Element.Water, Element.Magic }},
            { ItemType<YanmeisKnife>(), new Element[]{ Element.Slash, Element.Poison }},
            #endregion
            #region Godseeker
            { ItemType<RelicOfDeliverance>(), new Element[]{ Element.Stab, Element.Holy, Element.Fire }},
            #endregion
            #endregion
        };
        // Vulnerable - Resistant
        internal static Dictionary<int, Tuple<Element[], Element[]>> Bosses = new()
        {
            #region Pre-Hardmode
            { NPCType<DesertScourgeHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Water }, new Element[]{ Element.Fire }) },
            { NPCType<DesertScourgeBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash, Element.Water }, new Element[]{ Element.Fire }) },
            { NPCType<DesertScourgeTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash, Element.Water }, new Element[]{ Element.Fire }) },
            { NPCType<Crabulon>(), new Tuple<Element[], Element[]>(new Element[]{}, new Element[]{ Element.Cold, Element.Dark, Element.Fire, Element.Holy, Element.Impact, Element.Machine, Element.Magic, Element.Poison, Element.Slash, Element.Stab, Element.Unholy, Element.Water }) },
            { NPCType<HiveMind>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorHive>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorHeadLarge>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorHeadMedium>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorHeadSmall>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorBodyLarge>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorBodyMedium>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorBodySmall>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorTailLarge>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorTailMedium>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<PerforatorTailSmall>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Fire }, new Element[]{ Element.Unholy, Element.Dark }) },
            { NPCType<SlimeGodCore>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Water, Element.Stab }, new Element[]{ Element.Unholy, Element.Poison, Element.Slash }) },
            { NPCType<CrimulanPaladin>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Water, Element.Stab }, new Element[]{ Element.Unholy, Element.Poison, Element.Slash }) },
            { NPCType<SplitCrimulanPaladin>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Water, Element.Stab }, new Element[]{ Element.Unholy, Element.Poison, Element.Slash }) },
            { NPCType<EbonianPaladin>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Water, Element.Stab }, new Element[]{ Element.Unholy, Element.Poison, Element.Slash }) },
            { NPCType<SplitEbonianPaladin>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy, Element.Water, Element.Stab }, new Element[]{ Element.Unholy, Element.Poison, Element.Slash }) },
            #endregion
            #region Hardmode
            { NPCType<Cryogen>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Cold, Element.Water, Element.Magic, Element.Slash }) },
            { NPCType<BrimstoneElemental>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy }, new Element[]{ Element.Unholy, Element.Fire, Element.Cold }) },
            { NPCType<AquaticScourgeHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash, Element.Stab, Element.Cold }, new Element[]{ Element.Water, Element.Poison, Element.Fire }) },
            { NPCType<AquaticScourgeBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash, Element.Stab, Element.Cold }, new Element[]{ Element.Water, Element.Poison, Element.Fire }) },
            { NPCType<AquaticScourgeBodyAlt>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash, Element.Stab, Element.Cold }, new Element[]{ Element.Water, Element.Poison, Element.Fire }) },
            { NPCType<AquaticScourgeTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash, Element.Stab, Element.Cold }, new Element[]{ Element.Water, Element.Poison, Element.Fire }) },
            { NPCType<CalamitasClone>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy }, new Element[]{ Element.Fire, Element.Unholy, Element.Magic, Element.Slash }) },
            { NPCType<Catastrophe>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Stab }, new Element[]{ Element.Fire, Element.Unholy, Element.Magic, Element.Slash }) },
            { NPCType<Cataclysm>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Slash }, new Element[]{ Element.Fire, Element.Unholy, Element.Magic, Element.Stab }) },
            { NPCType<Leviathan>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Water, Element.Cold, Element.Dark }) },
            { NPCType<Anahita>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Water, Element.Cold }) },
            { NPCType<AstrumAureus>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Fire, Element.Stab }, new Element[]{ Element.Poison, Element.Magic, Element.Slash }) },
            { NPCType<PlaguebringerGoliath>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Fire }, new Element[]{ Element.Poison, Element.Dark, Element.Slash }) },
            { NPCType<RavagerHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Poison }, new Element[]{ Element.Fire, Element.Dark, Element.Cold }) },
            { NPCType<RavagerClawLeft>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Poison }, new Element[]{ Element.Fire, Element.Dark, Element.Cold }) },
            { NPCType<RavagerClawRight>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Poison }, new Element[]{ Element.Fire, Element.Dark, Element.Cold }) },
            { NPCType<RavagerLegLeft>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Poison }, new Element[]{ Element.Fire, Element.Dark, Element.Cold }) },
            { NPCType<RavagerLegRight>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Poison }, new Element[]{ Element.Fire, Element.Dark, Element.Cold }) },
            { NPCType<RavagerBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Holy, Element.Poison }, new Element[]{ Element.Fire, Element.Dark, Element.Cold }) },
            { NPCType<AstrumDeusHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Cold, Element.Unholy, Element.Slash, Element.Stab }, new Element[]{ Element.Poison, Element.Magic, Element.Dark, Element.Holy }) },
            { NPCType<AstrumDeusBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Cold, Element.Unholy, Element.Slash, Element.Stab }, new Element[]{ Element.Poison, Element.Magic, Element.Dark, Element.Holy }) },
            { NPCType<AstrumDeusTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Cold, Element.Unholy, Element.Slash, Element.Stab }, new Element[]{ Element.Poison, Element.Magic, Element.Dark, Element.Holy }) },
            #endregion
            #region Godseeker
            { NPCType<ProfanedGuardianCommander>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Unholy }, new Element[]{ Element.Holy, Element.Fire, Element.Dark }) },
            { NPCType<ProfanedGuardianDefender>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Unholy }, new Element[]{ Element.Holy, Element.Fire, Element.Dark }) },
            { NPCType<ProfanedGuardianHealer>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Unholy }, new Element[]{ Element.Holy, Element.Fire, Element.Dark }) },
            { NPCType<Providence>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Unholy }, new Element[]{ Element.Holy, Element.Fire, Element.Dark }) },
            { NPCType<Bumblefuck>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Dark }, new Element[]{ Element.Holy, Element.Fire }) },
            { NPCType<CeaselessVoid>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash }, new Element[]{ Element.Magic, Element.Dark }) },
            { NPCType<StormWeaverHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Stab }, new Element[]{ Element.Magic, Element.Cold, Element.Water }) },
            { NPCType<StormWeaverBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Stab }, new Element[]{ Element.Magic, Element.Cold, Element.Water }) },
            { NPCType<StormWeaverTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Stab }, new Element[]{ Element.Magic, Element.Cold, Element.Water }) },
            { NPCType<Signus>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash ,Element.Cold }, new Element[]{ Element.Magic, Element.Dark, Element.Fire }) },
            { NPCType<Polterghast>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Holy ,Element.Slash }, new Element[]{ Element.Dark, Element.Cold, Element.Poison }) },
            { NPCType<DevourerofGodsHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash }, new Element[]{ Element.Magic, Element.Poison, Element.Dark }) },
            { NPCType<DevourerofGodsBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash }, new Element[]{ Element.Magic, Element.Poison, Element.Dark }) },
            { NPCType<DevourerofGodsTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Slash }, new Element[]{ Element.Magic, Element.Poison, Element.Dark }) },
            { NPCType<Yharon>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Dark }, new Element[]{ Element.Holy, Element.Fire }) },
            { NPCType<AresBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Cold, Element.Poison, Element.Slash }) },
            { NPCType<AresGaussNuke>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Cold, Element.Poison, Element.Slash }) },
            { NPCType<AresLaserCannon>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Cold, Element.Poison, Element.Slash }) },
            { NPCType<AresPlasmaFlamethrower>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Cold, Element.Poison, Element.Slash }) },
            { NPCType<AresTeslaCannon>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Cold, Element.Poison, Element.Slash }) },
            { NPCType<Artemis>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire, Element.Stab }, new Element[]{ Element.Cold, Element.Poison, Element.Dark }) },
            { NPCType<Apollo>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire, Element.Stab }, new Element[]{ Element.Cold, Element.Poison, Element.Dark }) },
            { NPCType<ThanatosHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Slash }, new Element[]{ Element.Cold, Element.Poison, Element.Holy }) },
            { NPCType<ThanatosBody1>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Slash }, new Element[]{ Element.Cold, Element.Poison, Element.Holy }) },
            { NPCType<ThanatosBody2>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Slash }, new Element[]{ Element.Cold, Element.Poison, Element.Holy }) },
            { NPCType<ThanatosTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Slash }, new Element[]{ Element.Cold, Element.Poison, Element.Holy }) },
            { NPCType<SupremeCalamitas>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Cold, Element.Water, Element.Stab, Element.Holy }, new Element[]{ Element.Fire, Element.Dark, Element.Unholy }) },
            { NPCType<SupremeCatastrophe>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Cold, Element.Water, Element.Holy }, new Element[]{ Element.Fire, Element.Dark, Element.Unholy }) },
            { NPCType<PrimordialWyrmHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Dark, Element.Slash, Element.Magic, Element.Water, Element.Cold }) },
            { NPCType<PrimordialWyrmBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Dark, Element.Slash, Element.Magic, Element.Water, Element.Cold }) },
            { NPCType<PrimordialWyrmBodyAlt>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Dark, Element.Slash, Element.Magic, Element.Water, Element.Cold }) },
            { NPCType<PrimordialWyrmTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Stab, Element.Unholy }, new Element[]{ Element.Dark, Element.Slash, Element.Magic, Element.Water, Element.Cold }) },
            #endregion
        };
    }
}
