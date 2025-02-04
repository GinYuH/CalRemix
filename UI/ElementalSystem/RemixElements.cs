using System;
using System.Collections.Generic;

namespace CalRemix.UI.ElementalSystem
{
    public static class RemixElements
    {
        internal static Dictionary<string, Element[]> Item = new()
        {
            { "AcesLow", new Element[]{ Element.Stab }},
            { "AcidBow", new Element[]{ Element.Stab, Element.Poison }},
            { "AeroBolt", new Element[]{ Element.Wind }},
            { "Aerospray", new Element[]{ Element.Wind }},
            { "AngelsThesis", new Element[]{ Element.Magic }},
            { "AOTE", new Element[]{ Element.Slash, Element.Magic }},
            { "AspidBlaster", new Element[]{ Element.Impact, Element.Unholy }},
            { "Arngren", new Element[]{ Element.Stab }},
            { "AtlasMunitionsBeacon", new Element[]{ Element.Magic }},
            { "Axisdriver", new Element[]{ Element.Stab, Element.Machine, Element.Fire, Element.Cold }},
            { "BallisticMissword", new Element[]{ Element.Impact, Element.Fire }},
            { "Baow", new Element[]{ Element.Stab, Element.Magic }},
            { "BlobfishPlushie", new Element[]{ Element.Impact }},
            { "BucketofCoal", new Element[]{ Element.Impact, Element.Dark }},
            { "BundleBones", new Element[]{ Element.Impact, Element.Dark }},
            { "ButterflyStaff", new Element[]{ Element.Magic }},
            { "CalamitySword", new Element[]{ Element.Slash, Element.Unholy }},
            { "CausticClaw", new Element[]{ Element.Poison }},
            { "ChainSaw", new Element[]{ Element.Slash, Element.Unholy }},
            { "Chainsmoker", new Element[]{ Element.Wind, Element.Fire }},
            { "ChainSpear", new Element[]{ Element.Stab, Element.Unholy }},
            { "ChristmasCarol", new Element[]{ Element.Magic, Element.Cold }},
            { "CirnogenicStaff", new Element[]{ Element.Stab, Element.Cold}},
            { "ClamorNoxus", new Element[]{ Element.Machine, Element.Magic}},
            { "ColdheartIcicle", new Element[]{ Element.Stab, Element.Cold }},
            { "Confection", new Element[]{ Element.Impact, Element.Holy }},
            { "CorrosiveEyeStaff", new Element[]{ Element.Poison }},
            { "CursedSpear", new Element[]{ Element.Stab, Element.Dark }},
            { "DaggerofLiberty", new Element[]{ Element.Stab }},
            { "DarkEnergyStaff", new Element[]{ Element.Dark }},
            { "Deicide", new Element[]{ Element.Impact, Element.Unholy }},
            { "DemonCore", new Element[]{ Element.Impact, Element.Dark }},
            { "Driftorcher", new Element[]{ Element.Slash, Element.Fire }},
            { "DualCane", new Element[]{ Element.Magic }},
            { "Dualpoon", new Element[]{ Element.Stab }},
            { "EcologicalCollapse", new Element[]{ Element.Impact, Element.Water }},
            { "Exodimus", new Element[]{ Element.Stab, Element.Holy }},
            { "EXOLOTLStaff", new Element[]{ Element.Stab, Element.Machine }},
            { "Exosphear", new Element[]{ Element.Stab, Element.Machine, Element.Fire, Element.Cold }},
            { "ExtensionCable", new Element[]{ Element.Impact, Element.Machine }},
            { "FiberBaby", new Element[]{ Element.Impact, Element.Poison }},
            { "FlamingIceBow", new Element[]{ Element.Stab, Element.Fire, Element.Cold }},
            { "FlounderMortar", new Element[]{ Element.Poison }},
            { "FractureStaff", new Element[]{ Element.Impact, Element.Holy, Element.Magic }},
            { "FracturingFist", new Element[]{ Element.Impact, Element.Holy, Element.Magic }},
            { "GearworkShield", new Element[]{ Element.Impact, Element.Machine }},
            { "GrandReef", new Element[]{ Element.Impact, Element.Water }},
            { "GrarbleBayonet", new Element[]{ Element.Stab }},
            { "GrarbleSpear", new Element[]{ Element.Stab }},
            { "HadopelagicEcho", new Element[]{ Element.Water }},
            { "HalbardoftheHolidays", new Element[]{ Element.Slash, Element.Cold }},
            { "Heterochromia", new Element[]{ Element.Stab, Element.Fire }},
            { "Homochromia", new Element[]{ Element.Stab, Element.Magic }},
            { "Hyperion", new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { "IchorDagger", new Element[]{ Element.Stab }},
            { "JetEngine", new Element[]{ Element.Impact, Element.Dark, Element.Machine }},
            { "Juicer", new Element[]{ Element.Impact}},
            { "Magmasher", new Element[]{ Element.Impact, Element.Fire}},
            { "MackerelStaff", new Element[]{ Element.Impact, Element.Holy }},
            { "Megaskeet", new Element[]{ Element.Stab, Element.Holy }},
            { "Morpho", new Element[]{ Element.Slash, Element.Fire }},
            { "Mutagen", new Element[]{ Element.Impact }},
            { "Neuraze", new Element[]{ Element.Stab, Element.Machine }},
            { "Nystagmus", new Element[]{ Element.Impact, Element.Dark }},
            { "Ogscule", new Element[]{ Element.Unholy }},
            { "OnyxGunblade", new Element[]{ Element.Impact, Element.Slash, Element.Dark }},
            { "OnyxGunsaw", new Element[]{ Element.Slash, Element.Dark }},
            { "OnyxStaff", new Element[]{ Element.Impact, Element.Dark }},
            { "OrigenPoint", new Element[]{ Element.Impact }},
            { "PaletteUncleanser", new Element[]{ Element.Cold }},
            { "PhreaticChanneler", new Element[]{ Element.Slash, Element.Fire }},
            { "PineappleStaff", new Element[]{ Element.Impact }},
            { "PinesPenetrator", new Element[]{ Element.Stab, Element.Cold }},
            { "PlasmaflashBolt", new Element[]{ Element.Magic, Element.Wind }},
            { "PlumeflameBow", new Element[]{ Element.Stab, Element.Fire }},
            { "Prismachromancy", new Element[]{ Element.Magic }},
            { "ProfanedNucleus", new Element[]{ Element.Impact, Element.Holy, Element.Fire }},
            { "PungentBomber", new Element[]{ Element.Slash, Element.Poison }},
            { "RazorTeeth", new Element[]{ Element.Stab, Element.Poison }},
            { "RGBMurasama", new Element[]{ Element.Slash, Element.Machine }},
            { "RoseBow", new Element[]{ Element.Stab, Element.Magic }},
            { "Rox", new Element[]{ Element.Impact }},
            { "SaltBooklet", new Element[]{ Element.Poison }},
            { "SaltWaterBolt", new Element[]{ Element.Poison, Element.Water }},
            { "ScrapBag", new Element[]{ Element.Machine }},
            { "SDOMG", new Element[]{ Element.Stab, Element.Machine }},
            { "ShadowsDescent", new Element[]{ Element.Impact, Element.Dark }},
            { "ShardofGlass", new Element[]{ Element.Slash }},
            { "SickStick", new Element[]{ Element.Poison }},
            { "SkullFracture", new Element[]{ Element.Impact, Element.Holy, Element.Magic }},
            { "Snowgrave", new Element[]{ Element.Cold }},
            { "TendonTides", new Element[]{ Element.Slash, Element.Poison }},
            { "TenebrisTides", new Element[]{ Element.Slash, Element.Water }},
            { "Tetrachromancy", new Element[]{ Element.Magic, Element.Unholy }},
            { "TheDreamingGhost", new Element[]{ Element.Magic, Element.Holy }},
            { "TheFirestorm", new Element[]{ Element.Fire }},
            { "TheFirstFracturer", new Element[]{ Element.Slash, Element.Magic, Element.Holy }},
            { "ThePrincess", new Element[]{ Element.Holy, Element.Fire }},
            { "ThrowingMissiles", new Element[]{ Element.Impact }},
            { "ThunderBolt", new Element[]{ Element.Magic }},
            { "TitanTides", new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { "TotalAnnihilation", new Element[]{ Element.Impact, Element.Fire, Element.Magic, Element.Poison, Element.Cold }},
            { "TotalityTides", new Element[]{ Element.Slash, Element.Machine, Element.Fire, Element.Cold  }},
            { "Triploon", new Element[]{ Element.Stab }},
            { "Twentoon", new Element[]{ Element.Stab }},
            { "TwentyTwoon", new Element[]{ Element.Stab }},
            { "TwistedNetheriteSword", new Element[]{ Element.Slash }},
            { "TyrantShield", new Element[]{ Element.Impact, Element.Holy }},
            { "Unloxcalibur", new Element[]{ Element.Slash }},
            { "UnsealedSingularity", new Element[]{ Element.Impact, Element.Magic, Element.Dark }},
            { "WaraxeReloaded", new Element[]{ Element.Slash }},
            { "Warbell", new Element[]{ Element.Impact }},
            { "Warbow", new Element[]{ Element.Stab }},
            { "Warglaive", new Element[]{ Element.Slash }},
            { "Warstaff", new Element[]{ Element.Magic }},
            { "WindTurbineBlade", new Element[]{ Element.Stab, Element.Wind }},
            { "WinterBreeze", new Element[]{ Element.Stab, Element.Cold }},
            { "WrathoftheCosmos", new Element[]{ Element.Magic, Element.Dark }},
            { "WrathoftheDragons", new Element[]{ Element.Fire }},
            { "WrathoftheEldritch", new Element[]{ Element.Magic }},
            { "WrathoftheGods", new Element[]{ Element.Unholy }},
            { "WreathofBelial", new Element[]{ Element.Slash, Element.Cold, Element.Poison }},
            { "WulfrumLeechDagger", new Element[]{ Element.Stab, Element.Machine }},
            { "XenonCutlass", new Element[]{ Element.Slash, Element.Machine }},
        };
        // Vulnerable - Resistant
        internal static Dictionary<string, Tuple<Element[], Element[]>> Bosses = new()
        {
            { "WulfwyrmHead", new Tuple<Element[], Element[]>([Element.Magic, Element.Water, Element.Fire], [Element.Machine, Element.Slash]) },
            { "WulfwyrmBody", new Tuple<Element[], Element[]>([Element.Magic, Element.Water, Element.Fire], [Element.Machine, Element.Slash]) },
            { "WulfwyrmTail", new Tuple<Element[], Element[]>([Element.Magic, Element.Water, Element.Fire], [Element.Machine, Element.Slash]) },
            { "OrigenCore", new Tuple<Element[], Element[]>([Element.Impact, Element.Fire], [Element.Cold, Element.Water, Element.Slash, Element.Stab]) },
            { "Origen", new Tuple<Element[], Element[]>([Element.Impact, Element.Fire], [Element.Cold, Element.Water, Element.Slash, Element.Stab]) },
            { "AcidEye", new Tuple<Element[], Element[]>([Element.Stab, Element.Cold], [Element.Poison, Element.Water, Element.Dark]) },
            { "Carcinogen", new Tuple<Element[], Element[]>([Element.Slash, Element.Poison], [Element.Fire, Element.Wind, Element.Stab]) },
            //{ "DerellectBoss", new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Fire, Element.Impact }, new Element[]{ Element.Cold, Element.Poison, Element.Machine }) },
            
            { "Ionogen", new Tuple<Element[], Element[]>([Element.Fire, Element.Water], [Element.Poison, Element.Cold, Element.Machine]) },
            { "Polyphemalus", new Tuple<Element[], Element[]>([Element.Stab, Element.Impact, Element.Poison], [Element.Unholy]) },
            { "Astigmageddon", new Tuple<Element[], Element[]>([Element.Stab, Element.Dark, Element.Poison], [Element.Unholy, Element.Magic, Element.Fire]) },
            { "Exotrexia", new Tuple<Element[], Element[]>([Element.Stab, Element.Dark, Element.Fire], [Element.Unholy, Element.Cold]) },
            { "Conjunctivirus", new Tuple<Element[], Element[]>([Element.Stab, Element.Dark, Element.Water, Element.Holy], [Element.Unholy, Element.Poison, Element.Fire]) },
            { "Cataractacomb", new Tuple<Element[], Element[]>([Element.Stab, Element.Magic, Element.Poison], [Element.Unholy, Element.Dark, Element.Fire]) },
            { "Hydrogen", new Tuple<Element[], Element[]>([Element.Fire, Element.Water], [Element.Poison, Element.Cold, Element.Machine]) },
            { "Phytogen", new Tuple<Element[], Element[]>([Element.Fire, Element.Poison, Element.Cold], [Element.Water, Element.Cold, Element.Machine]) },
            { "Pathogen", new Tuple<Element[], Element[]>([Element.Poison, Element.Water], [Element.Fire, Element.Cold]) },
            { "Pyrogen", new Tuple<Element[], Element[]>([Element.Water, Element.Wind], [Element.Fire, Element.Cold, Element.Slash]) },
            { "Hypnos", new Tuple<Element[], Element[]>([Element.Water, Element.Poison, Element.Cold], [Element.Dark, Element.Machine, Element.Unholy]) },
        };
    }
}
