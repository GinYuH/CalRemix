using System.Collections.Generic;
using Terraria.ID;
using System;

namespace CalRemix.ElementalSystem
{
    public static class VanillaElements
    {
        /*
        internal static Dictionary<int, Element[]> Item = new()
        {
            #region Melee
            #region Pre-Hardmode
            
            { ItemID.JungleYoyo, new Element[]{ Element.Impact, Element.Poison }},
            { ItemID.CrimsonYoyo, new Element[]{ Element.Impact, Element.Dark }},
            { ItemID.AshWoodSword, new Element[]{ Element.Slash }},
            { ItemID.AcornAxe, new Element[]{ Element.Slash }},
            
            #endregion
            #endregion
        };
        */

        // Vulnerable - Resistant
        internal static Dictionary<int, Tuple<Element[], Element[]>> Bosses = new()
        {
            #region Pre-Hardmode
            { NPCID.KingSlime, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact, Element.Fire }, new Element[]{ Element.Poison, Element.Water }) },
            { NPCID.EyeofCthulhu, new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Poison }, new Element[]{ Element.Dark, Element.Water }) },
            { NPCID.EaterofWorldsHead, new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Fire }, new Element[]{ Element.Dark, Element.Unholy, Element.Poison }) },
            { NPCID.EaterofWorldsBody, new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Fire }, new Element[]{ Element.Dark, Element.Unholy, Element.Poison }) },
            { NPCID.EaterofWorldsTail, new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Fire }, new Element[]{ Element.Dark, Element.Unholy, Element.Poison }) },
            { NPCID.BrainofCthulhu, new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Fire }, new Element[]{ Element.Dark, Element.Unholy, Element.Poison }) },
            { NPCID.QueenBee, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact, Element.Fire }, new Element[]{ Element.Water, Element.Poison }) },
            { NPCID.Deerclops, new Tuple<Element[], Element[]>(new Element[]{ Element.Poison, Element.Fire }, new Element[]{ Element.Water, Element.Cold }) },
            { NPCID.SkeletronHead, new Tuple<Element[], Element[]>(new Element[]{ Element.Machine, Element.Holy }, new Element[]{ Element.Poison, Element.Cold, Element.Dark }) },
            { NPCID.SkeletronHand, new Tuple<Element[], Element[]>(new Element[]{ Element.Machine, Element.Holy }, new Element[]{ Element.Poison, Element.Cold, Element.Dark }) },
            { NPCID.WallofFleshEye, new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Poison, Element.Holy, Element.Cold }, new Element[]{ Element.Fire, Element.Unholy, Element.Dark }) },
            { NPCID.WallofFlesh, new Tuple<Element[], Element[]>(new Element[]{ Element.Poison, Element.Holy, Element.Cold }, new Element[]{ Element.Fire, Element.Unholy, Element.Dark }) },
            #endregion
            #region Hardmode
            { NPCID.QueenSlimeBoss, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact, Element.Fire, Element.Unholy }, new Element[]{ Element.Poison, Element.Water, Element.Holy }) },
            { NPCID.Retinazer, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Stab, Element.Dark, Element.Machine }) },
            { NPCID.Spazmatism, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Slash, Element.Dark, Element.Machine }) },
            { NPCID.TheDestroyer, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Magic}, new Element[]{ Element.Stab, Element.Poison, Element.Machine }) },
            { NPCID.TheDestroyerBody, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Magic}, new Element[]{ Element.Stab, Element.Poison, Element.Machine }) },
            { NPCID.TheDestroyerTail, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Magic}, new Element[]{ Element.Stab, Element.Poison, Element.Machine }) },
            { NPCID.SkeletronPrime, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Stab, Element.Magic, Element.Machine }) },
            { NPCID.PrimeCannon, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Stab, Element.Magic, Element.Machine }) },
            { NPCID.PrimeLaser, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Stab, Element.Magic, Element.Machine }) },
            { NPCID.PrimeSaw, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Stab, Element.Magic, Element.Machine }) },
            { NPCID.PrimeVice, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Water, Element.Holy}, new Element[]{ Element.Stab, Element.Magic, Element.Machine }) },
            { NPCID.Plantera, new Tuple<Element[], Element[]>(new Element[]{ Element.Fire, Element.Cold, Element.Machine}, new Element[]{ Element.Water, Element.Poison, Element.Dark }) },
            { NPCID.Golem, new Tuple<Element[], Element[]>(new Element[]{ Element.Water, Element.Dark, Element.Unholy}, new Element[]{ Element.Fire, Element.Poison, Element.Machine }) },
            { NPCID.DukeFishron, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact, Element.Holy, Element.Cold}, new Element[]{ Element.Water, Element.Poison, Element.Fire }) },
            { NPCID.HallowBoss, new Tuple<Element[], Element[]>(new Element[]{ Element.Poison, Element.Machine, Element.Dark, Element.Unholy}, new Element[]{ Element.Fire, Element.Magic, Element.Holy }) },
            { NPCID.CultistBoss, new Tuple<Element[], Element[]>(new Element[]{ Element.Poison }, new Element[]{ Element.Fire, Element.Cold, Element.Dark, Element.Magic }) },
            { NPCID.MoonLordHead, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact }, new Element[]{ Element.Dark, Element.Unholy }) },
            { NPCID.MoonLordHand, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact }, new Element[]{ Element.Dark, Element.Unholy }) },
            { NPCID.MoonLordCore, new Tuple<Element[], Element[]>(new Element[]{ Element.Impact }, new Element[]{ Element.Dark, Element.Unholy }) },
            #endregion
        };
    }
}
