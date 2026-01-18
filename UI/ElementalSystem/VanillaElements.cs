using System.Collections.Generic;
using Terraria.ID;
using System;

namespace CalRemix.UI.ElementalSystem
{
    public static class VanillaElements
    {
        internal static Dictionary<int, Element[]> Item = new()
        {
            #region Melee
            #region Pre-Hardmode
            
            { ItemID.CopperShortsword, new Element[]{ Element.Stab }},
            { ItemID.TinShortsword, new Element[]{ Element.Stab }},
            { ItemID.IronShortsword, new Element[]{ Element.Stab }},
            { ItemID.LeadShortsword, new Element[]{ Element.Stab }},
            { ItemID.SilverShortsword, new Element[]{ Element.Stab }},
            { ItemID.TungstenShortsword, new Element[]{ Element.Stab }},
            { ItemID.GoldShortsword, new Element[]{ Element.Stab }},
            { ItemID.PlatinumShortsword, new Element[]{ Element.Stab }},
            { ItemID.Ruler, new Element[]{ Element.Stab }},
            { ItemID.Umbrella, new Element[]{ Element.Stab }},
            { ItemID.Gladius, new Element[]{ Element.Stab }},
            { ItemID.TragicUmbrella, new Element[]{ Element.Stab }},
            { ItemID.WoodenSword, new Element[]{ Element.Slash }},
            { ItemID.BorealWoodSword, new Element[]{ Element.Slash }},
            { ItemID.CopperBroadsword, new Element[]{ Element.Slash }},
            { ItemID.PalmWoodSword, new Element[]{ Element.Slash }},
            { ItemID.RichMahoganySword, new Element[]{ Element.Slash }},
            { ItemID.CactusSword, new Element[]{ Element.Slash, Element.Poison }},
            { ItemID.TinBroadsword, new Element[]{ Element.Slash }},
            { ItemID.EbonwoodSword, new Element[]{ Element.Slash }},
            { ItemID.IronBroadsword, new Element[]{ Element.Slash }},
            { ItemID.ShadewoodSword, new Element[]{ Element.Slash }},
            { ItemID.LeadBroadsword, new Element[]{ Element.Slash }},
            { ItemID.SilverBroadsword, new Element[]{ Element.Slash }},
            { ItemID.BladedGlove, new Element[]{ Element.Slash }},
            { ItemID.TungstenBroadsword, new Element[]{ Element.Slash }},
            { ItemID.ZombieArm, new Element[]{ Element.Slash }},
            { ItemID.AshWoodSword, new Element[]{ Element.Slash }},
            { ItemID.GoldBroadsword, new Element[]{ Element.Slash }},
            { ItemID.Flymeal, new Element[]{ Element.Slash, Element.Poison }},
            { ItemID.AntlionClaw, new Element[]{ Element.Slash }},
            { ItemID.StylistKilLaKillScissorsIWish, new Element[]{ Element.Slash }},
            { ItemID.PlatinumBroadsword, new Element[]{ Element.Slash }},
            { ItemID.BreathingReed, new Element[]{ Element.Slash }},
            { ItemID.BoneSword, new Element[]{ Element.Slash }},
            { ItemID.BatBat, new Element[]{ Element.Slash }},
            { ItemID.TentacleSpike, new Element[]{ Element.Slash }},
            //{ ItemID, new Element[]{ Element.Slash }},
            
            #endregion
            #endregion
        };

        // Vulnerable - Resistant
        internal static Dictionary<int, Tuple<Element[], Element[]>> Bosses = new()
        {
            #region Pre-Hardmode
            { NPCID.KingSlime, new Tuple<Element[], Element[]>([Element.Impact, Element.Fire], [Element.Adhesive, Element.Water]) },
            { NPCID.EyeofCthulhu, new Tuple<Element[], Element[]>([Element.Stab, Element.Poison, Element.Blood], [Element.Dark, Element.Water]) },
            { NPCID.EaterofWorldsHead, new Tuple<Element[], Element[]>([Element.Stab, Element.Fire], [Element.Dark, Element.Unholy, Element.Poison]) },
            { NPCID.EaterofWorldsBody, new Tuple<Element[], Element[]>([Element.Stab, Element.Fire], [Element.Dark, Element.Unholy, Element.Poison]) },
            { NPCID.EaterofWorldsTail, new Tuple<Element[], Element[]>([Element.Stab, Element.Fire], [Element.Dark, Element.Unholy, Element.Poison]) },
            { NPCID.BrainofCthulhu, new Tuple<Element[], Element[]>([Element.Stab, Element.Fire], [Element.Unholy, Element.Poison, Element.Cold]) },
            { NPCID.QueenBee, new Tuple<Element[], Element[]>([Element.Impact, Element.Fire], [Element.Water, Element.Poison]) },
            { NPCID.Deerclops, new Tuple<Element[], Element[]>([Element.Poison, Element.Fire], [Element.Water, Element.Cold]) },
            { NPCID.SkeletronHead, new Tuple<Element[], Element[]>([Element.Machine, Element.Holy], [Element.Poison, Element.Cold, Element.Dark]) },
            { NPCID.SkeletronHand, new Tuple<Element[], Element[]>([Element.Machine, Element.Holy], [Element.Poison, Element.Cold, Element.Dark]) },
            { NPCID.WallofFleshEye, new Tuple<Element[], Element[]>([Element.Stab, Element.Poison, Element.Holy, Element.Cold], [Element.Fire, Element.Unholy, Element.Dark]) },
            { NPCID.WallofFlesh, new Tuple<Element[], Element[]>([Element.Poison, Element.Holy, Element.Cold], [Element.Fire, Element.Unholy, Element.Dark]) },
            #endregion
            #region Hardmode
            { NPCID.QueenSlimeBoss, new Tuple<Element[], Element[]>([Element.Impact, Element.Fire, Element.Unholy], [Element.Poison, Element.Water, Element.Holy]) },
            { NPCID.Retinazer, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Stab, Element.Dark, Element.Machine]) },
            { NPCID.Spazmatism, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Slash, Element.Dark, Element.Machine]) },
            { NPCID.TheDestroyer, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Magic], [Element.Stab, Element.Poison, Element.Machine]) },
            { NPCID.TheDestroyerBody, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Magic], [Element.Stab, Element.Poison, Element.Machine]) },
            { NPCID.TheDestroyerTail, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Magic], [Element.Stab, Element.Poison, Element.Machine]) },
            { NPCID.SkeletronPrime, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Stab, Element.Magic, Element.Machine]) },
            { NPCID.PrimeCannon, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Stab, Element.Magic, Element.Machine]) },
            { NPCID.PrimeLaser, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Stab, Element.Magic, Element.Machine]) },
            { NPCID.PrimeSaw, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Stab, Element.Magic, Element.Machine]) },
            { NPCID.PrimeVice, new Tuple<Element[], Element[]>([Element.Fire, Element.Water, Element.Holy], [Element.Stab, Element.Magic, Element.Machine]) },
            { NPCID.Plantera, new Tuple<Element[], Element[]>([Element.Fire, Element.Cold, Element.Machine], [Element.Water, Element.Poison, Element.Dark]) },
            { NPCID.Golem, new Tuple<Element[], Element[]>([Element.Water, Element.Dark, Element.Unholy], [Element.Fire, Element.Poison, Element.Machine]) },
            { NPCID.DukeFishron, new Tuple<Element[], Element[]>([Element.Impact, Element.Holy, Element.Cold], [Element.Water, Element.Poison, Element.Fire]) },
            { NPCID.HallowBoss, new Tuple<Element[], Element[]>([Element.Poison, Element.Machine, Element.Dark, Element.Unholy], [Element.Fire, Element.Magic, Element.Holy]) },
            { NPCID.CultistBoss, new Tuple<Element[], Element[]>([Element.Poison], [Element.Fire, Element.Cold, Element.Dark, Element.Magic]) },
            { NPCID.MoonLordHead, new Tuple<Element[], Element[]>([Element.Impact], [Element.Dark, Element.Unholy, Element.Magic]) },
            { NPCID.MoonLordHand, new Tuple<Element[], Element[]>([Element.Impact], [Element.Dark, Element.Unholy, Element.Magic]) },
            { NPCID.MoonLordCore, new Tuple<Element[], Element[]>([Element.Impact], [Element.Dark, Element.Unholy, Element.Magic]) },
            #endregion
        };
    }
}
