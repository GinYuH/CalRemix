using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.Yharon;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Potions;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee.Yoyos;
using CalamityMod.Items.Placeables.Ores;

namespace CalRemix.Retheme
{
    public static class RethemeList
    {
        internal static Dictionary<int, string> NPCs = new()
        {
            { NPCType<ColossalSquid>(), "ColossalSquid" },
            { NPCType<Eidolist>(), "Eidolist" },
            { NPCType<ReaperShark>(), "ReaperShark" },
            #region Pre-Hardmode
            { NPCType<DesertScourgeHead>(), "DS/Head" },
            { NPCType<DesertScourgeBody>(), "DS/Body" },
            { NPCType<DesertScourgeTail>(), "DS/Tail" },
            { NPCType<DesertNuisanceHead>(), "DS/NHead" },
            { NPCType<DesertNuisanceBody>(), "DS/NBody" },
            { NPCType<DesertNuisanceTail>(), "DS/NTail" },
            { NPCType<HiveTumor>(), "HiveMind/HiveTumor" },
            { NPCType<DankCreeper>(), "HiveMind/DankCreeper" },
            { NPCType<HiveBlob>(), "HiveMind/HiveBlob" },
            { NPCType<HiveBlob2>(), "HiveMind/HiveBlob" },
            { NPCType<DarkHeart>(), "HiveMind/DarkHeart" },
            { NPCType<HiveMind>(), "HiveMind/HiveMind" },
            #region Perfs
            { NPCType<PerforatorCyst>(), "Perfs/Cyst" },
            { NPCType<PerforatorBodyLarge>(), "Perfs/LBody" },
            { NPCType<PerforatorBodyMedium>(), "Perfs/MBody" },
            { NPCType<PerforatorBodySmall>(), "Perfs/SBody" },
            { NPCType<PerforatorHeadLarge>(), "Perfs/LHead" },
            { NPCType<PerforatorHeadMedium>(), "Perfs/MHead" },
            { NPCType<PerforatorHeadSmall>(), "Perfs/SHead" },
            { NPCType<PerforatorTailLarge>(), "Perfs/LTail" },
            { NPCType<PerforatorTailMedium>(), "Perfs/MTail" },
            { NPCType<PerforatorTailSmall>(), "Perfs/STail" },
            { NPCType<PerforatorHive>(), "Perfs/Hive" },
            #endregion
            #region Slime God
            { NPCType<CrimsonSlimeSpawn>(), "SlimeGod/CrimsonSlimeSpawn" },
            { NPCType<CrimsonSlimeSpawn2>(), "SlimeGod/CrimsonSlimeSpawn2" },
            { NPCType<CorruptSlimeSpawn>(), "SlimeGod/CorruptSlimeSpawn" },
            { NPCType<CorruptSlimeSpawn2>(), "SlimeGod/CorruptSlimeSpawn2" },
            { NPCType<SlimeGodCore>(), "SlimeGod/Core" },
            { NPCType<CrimulanPaladin>(), "SlimeGod/CrimulanSlimeGod" },
            { NPCType<SplitCrimulanPaladin>(), "SlimeGod/CrimulanSlimeGod" },
            { NPCType<EbonianPaladin>(), "SlimeGod/EbonianSlimeGod" },
            { NPCType<SplitEbonianPaladin>(), "SlimeGod/EbonianSlimeGod" },
            #endregion
            #endregion
            #region Hardmode
            { NPCType<Cryogen>(), "Cryogen/CryogenPhase1" },
            { NPCType<CryogenShield>(), "Cryogen/CryogenShield" },
            { NPCType<CalamitasClone>(), "Cal/CalamitasClone" },
            { NPCType<Cataclysm>(), "Cal/Cataclysm" },
            { NPCType<Catastrophe>(), "Cal/Catastrophe" },
            { NPCType<Anahita>(), "Levi/Anahita" },
            { NPCType<Leviathan>(), "Levi/Levi" },
            { NPCType<AquaticAberration>(), "Levi/AquaticAberration" },
            { NPCType<AstrumAureus>(), "Plague/AstrumAureus" },
            { NPCType<AureusSpawn>(), "Plague/AureusSpawn" },
            { NPCType<AstrumDeusHead>(), "AD/Head" },
            { NPCType<AstrumDeusBody>(), "AD/Body" },
            { NPCType<AstrumDeusTail>(), "AD/Tail" },
            #endregion
            #region Godseeker Mode
            { NPCType<Bumblefuck>(), "Birb/Birb" },
            { NPCType<Bumblefuck2>(), "Birb/Birb2" },
            { NPCType<WildBumblefuck>(), "Birb/Birb2" },
            { NPCType<Yharon>(), "Yharon/Yharon" },
            #endregion
        };
        /* shelved until someone deals with this
        internal static Dictionary<int, string> BossHeads = new()
        {
            { NPCType<DesertScourgeHead>(), "DS/Map" },
            { NPCType<DesertNuisanceHead>(), "DS/NMap" },
            { NPCType<Crabulon>(), "Crabulon/Map" },
            { NPCType<PerforatorHive>(), "Perfs/Map" },
            { NPCType<PerforatorHeadLarge>(), "Perfs/LMap" },
            { NPCType<PerforatorHeadMedium>(), "Perfs/MMap" },
            { NPCType<PerforatorHeadSmall>(), "Perfs/SMap" },
            { NPCType<SlimeGodCore>(), "SlimeGod/Map" },
            { NPCType<CrimulanPaladin>(), "SlimeGod/CMap" },
            { NPCType<SplitCrimulanPaladin>(), "SlimeGod/CSMap" },
            { NPCType<EbonianPaladin>(), "SlimeGod/EMap" },
            { NPCType<SplitEbonianPaladin>(), "SlimeGod/ESMap" },
            { NPCType<Anahita>(), "Levi/AnaMap" },
            { NPCType<Leviathan>(), "Levi/LeviMap" },
            { NPCType<AstrumAureus>(), "Plague/Map" },
            { NPCType<RavagerBody>(), "RavagerMap" },
            { NPCType<AstrumDeusHead>(), "AD/Map" },
            { NPCType<Bumblefuck>(), "Birb/Map" },
        };
        */
        internal static Dictionary<int, string> Items = new()
        {
            { ItemType<EnergyCore>(), "EnergyCore" },
            { ItemType<Nadir>(), "Nadir" },
            { ItemType<Violence>(), "Violence" },
            { ItemType<WulfrumMetalScrap>(), "Bars/WulfrumBar" },
            { ItemType<ClockGatlignum>(), "Bars/ClockworkBar" },
            { ItemType<LifeAlloy>(), "Bars/LifeAlloy" },
            { ItemType<MeldConstruct>(), "Bars/MeldBar" },
            #region Desert Scourge
            { ItemType<DesertScourgeBag>(), "DS/Bag" },
            { ItemType<DesertMedallion>(), "DS/DesertMedallion" },
            { ItemType<OceanCrest>(), "DS/OceanCrest" },
            { ItemType<SaharaSlicers>(), "DS/SaharaSlicers" },
            { ItemType<Barinade>(), "DS/Barinade" },
            { ItemType<SandstreamScepter>(), "DS/SandstreamScepter" },
            { ItemType<BrittleStarStaff>(), "DS/BrittleStarStaff" },
            { ItemType<ScourgeoftheDesert>(), "DS/ScourgeoftheDesert" },
            #endregion
            #region Crabulon
            { ItemType<CrabulonBag>(), "Crabulon/Bag" },
            { ItemType<DecapoditaSprout>(), "Crabulon/DecapoditaSprout" },
            { ItemType<FungalClump>(), "Crabulon/FungalClump" },
            { ItemType<MushroomPlasmaRoot>(), "Crabulon/MushroomPlasmaRoot" },
            { ItemType<MycelialClaws>(), "Crabulon/MycelialClaws" },
            { ItemType<Fungicide>(), "Crabulon/Fungicide" },
            { ItemType<HyphaeRod>(), "Crabulon/HyphaeRod" },
            { ItemType<PuffShroom>(), "Crabulon/PuffShroom" },
            { ItemType<Mycoroot>(), "Crabulon/Mycoroot" },
            { ItemType<InfestedClawmerang>(), "Crabulon/Shroomerang" },
            #endregion
            #region Hive Mind
            { ItemType<HiveMindBag>(), "HiveMind/Bag" },
            { ItemType<RottenMatter>(), "HiveMind/RottenMatter" },
            { ItemType<Teratoma>(), "HiveMind/Teratoma" },
            { ItemType<RottenBrain>(), "HiveMind/RottenBrain" },
            { ItemType<FilthyGlove>(), "HiveMind/FilthyGlove" },
            { ItemType<PerfectDark>(), "HiveMind/PerfectDark" },
            { ItemType<Shadethrower>(), "HiveMind/Shadethrower" },
            { ItemType<ShaderainStaff>(), "HiveMind/ShaderainStaff" },
            { ItemType<DankStaff>(), "HiveMind/DankStaff" },
            { ItemType<RotBall>(), "HiveMind/RotBall" },
            #endregion
            #region Perfs
            { ItemType<PerforatorBag>(), "Perfs/Bag" },
            { ItemType<BloodSample>(), "Perfs/BloodSample" },
            { ItemType<BloodyWormFood>(), "Perfs/BloodyWormFood" },
            { ItemType<BloodyWormTooth>(), "Perfs/BloodyWormTooth" },
            { ItemType<BloodstainedGlove>(), "Perfs/BloodstainedGlove" },
            { ItemType<Aorta>(), "Perfs/Aorta" },
            { ItemType<VeinBurster>(), "Perfs/VeinBurster" },
            { ItemType<SausageMaker>(), "Perfs/SausageMaker" },
            { ItemType<Eviscerator>(), "Perfs/Eviscerator" },
            { ItemType<BloodBath>(), "Perfs/BloodBath" },
            { ItemType<FleshOfInfidelity>(), "Perfs/FleshOfInfidelity" },
            { ItemType<ToothBall>(), "Perfs/ToothBall" },
            { ItemType<BloodyWormScarf>(), "Perfs/BloodyWormScarf" },
            #endregion
            #region Slime God
            { ItemType<SlimeGodBag>(), "SlimeGod/Bag" },
            { ItemType<ManaPolarizer>(), "SlimeGod/Polarizer" },
            { ItemType<AbyssalTome>(), "SlimeGod/AbyssalTome" },
            { ItemType<EldritchTome>(), "SlimeGod/EldritchTome" },
            { ItemType<CrimslimeStaff>(), "SlimeGod/CrimslimeStaff" },
            { ItemType<CorroslimeStaff>(), "SlimeGod/CorroslimeStaff" },
            #endregion
            #region Levis
            { ItemType<LeviathanBag>(), "Levi/Bag" },
            { ItemType<PearlofEnthrallment>(), "Levi/Pearl" },
            { ItemType<Greentide>(), "Levi/Greentide" },
            { ItemType<Leviatitan>(), "Levi/Leviatitan" },
            { ItemType<AnahitasArpeggio>(), "Levi/AnahitasArpeggio" },
            { ItemType<Atlantis>(), "Levi/Atlantis" },
            { ItemType<GastricBelcherStaff>(), "Levi/GastricBelcherStaff" },
            { ItemType<BrackishFlask>(), "Levi/BrackishFlask" },
            { ItemType<LeviathanAmbergris>(), "Levi/LeviathanAmbergris" },
            #endregion
            #region Plague Aureus
            { ItemType<AstrumAureusBag>(), "Plague/Bag" },
            { ItemType<AstralChunk>(), "Plague/AstralChunk" },
            { ItemType<AureusCell>(), "Plague/AureusCell" },
            { ItemType<GravistarSabaton>(), "Plague/GravistarSabaton" },
            { ItemType<Nebulash>(), "Plague/Nebulash" },
            { ItemType<AuroraBlazer>(), "Plague/AuroraBlazer" },
            { ItemType<AlulaAustralis>(), "Plague/AlulaAustralis" },
            { ItemType<BorealisBomber>(), "Plague/BorealisBomber" },
            { ItemType<LeonidProgenitor>(), "Plague/LeonidProgenitor" },
            { ItemType<AuroradicalThrow>(), "Plague/AuroradicalThrow" },
            #endregion
            #region Astrum Deus
            { ItemType<AstrumDeusBag>(), "AD/Bag" },
            { ItemType<HideofAstrumDeus>(), "AD/HideofAstrumDeus" },
            { ItemType<TheMicrowave>(), "AD/TheMicrowave" },
            { ItemType<StarSputter>(), "AD/StarSputter" },
            { ItemType<StarShower>(), "AD/StarShower" },
            { ItemType<StarspawnHelixStaff>(), "AD/StarspawnHelixStaff" },
            { ItemType<RegulusRiot>(), "AD/RegulusRiot" },
            #endregion
            #region Birb
            { ItemType<DragonfollyBag>(), "Birb/Bag" },
            { ItemType<EffulgentFeather>(), "Birb/EffulgentFeather" },
            { ItemType<GildedProboscis>(), "Birb/GildedProboscis" },
            { ItemType<GoldenEagle>(), "Birb/GoldenEagle" },
            { ItemType<RougeSlash>(), "Birb/RougeSlash" },
            { ItemType<DynamoStemCells>(), "Birb/DynamoStemCells" },
            { ItemType<RedLightningContainer>(), "Birb/RedLightningContainer" },
            #endregion
            #region Yharon
            { ItemType<YharonBag>(), "Yharon/Bag" },
            { ItemType<YharonSoulFragment>(), "Yharon/YharonSoulFragment" },
            { ItemType<DragonRage>(), "Yharon/DragonRage" },
            { ItemType<DragonsBreath>(), "Yharon/DragonsBreath" },
            { ItemType<ChickenCannon>(), "Yharon/ChickenCannon" },
            { ItemType<PhoenixFlameBarrage>(), "Yharon/DragonFlameBarrage" },
            { ItemType<YharonsKindleStaff>(), "Yharon/YharonsKindleStaff" },
            { ItemType<TheBurningSky>(), "Yharon/TheBurningSky" },
            { ItemType<TheFinalDawn>(), "Yharon/FinalDawn" },
            { ItemType<Wrathwing>(), "Yharon/Wrathwing" },
            #endregion
            #region Exos
            { ItemType<MiracleMatter>(), "Exo/Matter" },
            { ItemType<Exoblade>(), "Exo/Blade" },
            { ItemType<HeavenlyGale>(), "Exo/Gale" },
            { ItemType<Photoviscerator>(), "Exo/Vis" },
            { ItemType<MagnomalyCannon>(), "Exo/Cannon" },
            { ItemType<SubsumingVortex>(), "Exo/Vortex" },
            { ItemType<VividClarity>(), "Exo/Clarity" },
            { ItemType<CosmicImmaterializer>(), "Exo/Im" },
            { ItemType<Celestus>(), "Exo/Celestus" },
            { ItemType<Supernova>(), "Exo/Supernova" },
            #endregion
        };
        internal static Dictionary<int, string> Projs = new()
        {
            { ProjectileType<NadirSpear>(), "NadirSpear" },
            { ProjectileType<VoidEssence>(), "VoidEssence" },
            #region Desert Sockourge
            #endregion
            { ProjectileType<SaharaSlicersBlade>(), "DS/SaharaSlicer" },
            { ProjectileType<SaharaSlicersBladeAlt>(), "DS/SaharaSlicer" },
            { ProjectileType<SaharaSlicersBolt>(), "DS/SaharaSlicersBolt" },
            { ProjectileType<BrittleStarMinion>(), "DS/BrittleStarMinion" },
            { ProjectileType<ScourgeoftheDesertProj>(), "DS/ScourgeoftheDesert" },
            #region Crab
            { ProjectileType<MycorootProj>(), "Crabulon/Mycoroot" },
            { ProjectileType<InfestedClawmerangProj>(), "Crabulon/Shroomerang" },
            { ProjectileType<FungalClumpMinion>(), "Crabulon/FungalClumpProj" },
            #endregion
            #region Hive
            { ProjectileType<DarkBall>(), "HiveMind/DarkBall" },
            { ProjectileType<ShadeNimbusCloud>(), "HiveMind/ShadeNimbusCloud" },
            { ProjectileType<DankCreeperMinion>(), "HiveMind/DankCreeperMinion" },
            { ProjectileType<Shaderain>(), "HiveMind/Shaderain" },
            { ProjectileType<ShadeNimbusHostile>(), "HiveMind/ShadeNimbusHostile" },
            { ProjectileType<ShaderainHostile>(), "HiveMind/ShaderainHostile" },
            #endregion
            #region Perfs
            { ProjectileType<BloodBall>(), "Perfs/BloodBall" },
            { ProjectileType<AortaYoyo>(), "Perfs/AortaYoyo" },
            { ProjectileType<BloodBeam>(), "Perfs/BloodBeam" },
            { ProjectileType<FleshBallMinion>(), "Perfs/FleshBallMinion" },
            { ProjectileType<BloodGeyser>(), "Perfs/BloodGeyser" },
            { ProjectileType<IchorBlob>(), "Perfs/IchorBlob" },
            { ProjectileType<IchorShot>(), "Perfs/IchorShot" },
            #endregion
            #region SG
            { ProjectileType<UnstableCrimulanGlob>(), "SlimeGod/CBall" },
            { ProjectileType<UnstableEbonianGlob>(), "SlimeGod/EBall" },
            { ProjectileType<AbyssBall>(), "SlimeGod/EBall" },
            #endregion
            #region Levi
            { ProjectileType<WaterElementalMinion>(), "Levi/Anahita" },
            #endregion
            #region Aureus
            { ProjectileType<NebulashFlail>(), "Plague/NebulashFlail" },
            { ProjectileType<AureusBomber>(), "Plague/AureusBomber" },
            { ProjectileType<AuroradicalSplitter>(), "Plague/AuroradicalStar" },
            { ProjectileType<AuroradicalStar>(), "Plague/AuroradicalStar" },
            #endregion
            #region Deus
            { ProjectileType<MicrowaveYoyo>(), "AD/MicrowaveYoyo" },
            { ProjectileType<SputterComet>(), "AD/SputterComet" },
            { ProjectileType<SputterCometBig>(), "AD/SputterCometBig" },
            { ProjectileType<AstralProbeSummon>(), "AD/AstralProbeSummon" },
            { ProjectileType<RegulusRiotProj>(), "AD/RegulusRiot" },
            #endregion
            #region Yharon
            { ProjectileType<ChickenCannonHeld>(), "Yharon/ChickenCannonHeld" },
            #endregion
            #region Exo
            { ProjectileType<ExobladeProj>(), "Exo/Blade" },
            { ProjectileType<PhotovisceratorHoldout>(), "Exo/Vis" },
            { ProjectileType<CelestusProj>(), "Exo/Celestus" },
            { ProjectileType<SupernovaBomb>(), "Exo/Supernova" },
            #endregion
        };
        internal static Dictionary<int, string> ItemNames = new()
        {
            { ItemType<InfestedClawmerang>(), "Shroomerang" },
            { ItemType<PhantomicArtifact>(), "Phantomic Soul Artifact" },
            { ItemType<MeldConstruct>(), "Meld Bar" },
            { ItemType<UelibloomOre>(), "Tarragon Ore" },
            { ItemType<UelibloomBar>(), "Tarragon Bar" },
            { ItemType<SoulPiercer>(), "Stream Gouge" },
            { ItemType<StreamGouge>(), "Soul Piercer" },
            { ItemType<PhoenixFlameBarrage>(), "Dragon Flame Barrage" },
        };
    }
}
