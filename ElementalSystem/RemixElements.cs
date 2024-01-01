using CalRemix.Items.Weapons;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using System;
using CalRemix.NPCs.Bosses.Wulfwyrm;
//using CalRemix.NPCs.Bosses.Acideye;

namespace CalRemix.ElementalSystem
{
    public static class RemixElements
    {
        internal static Dictionary<int, Element[]> Item = new()
        {
            { ItemType<AcesLow>(), new Element[]{ Element.Stab }},
            { ItemType<AeroBolt>(), new Element[]{ Element.Magic }},
            { ItemType<AngelsThesis>(), new Element[]{ Element.Magic }},
            { ItemType<AOTE>(), new Element[]{ Element.Slash, Element.Magic }},
            { ItemType<Arngren>(), new Element[]{ Element.Stab }},
            { ItemType<AtlasMunitionsBeacon>(), new Element[]{ Element.Magic }},
            { ItemType<Axisdriver>(), new Element[]{ Element.Stab, Element.Machine, Element.Fire, Element.Cold }},
            { ItemType<Baow>(), new Element[]{ Element.Stab, Element.Magic }},
            { ItemType<BucketofCoal>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<BundleBones>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<ButterflyStaff>(), new Element[]{ Element.Magic }},
            { ItemType<ChainSpear>(), new Element[]{ Element.Stab, Element.Unholy }},
            { ItemType<ChristmasCarol>(), new Element[]{ Element.Magic, Element.Cold }},
            { ItemType<ColdheartIcicle>(), new Element[]{ Element.Stab, Element.Cold }},
            { ItemType<Confection>(), new Element[]{ Element.Impact, Element.Holy }},
            { ItemType<CursedSpear>(), new Element[]{ Element.Stab, Element.Dark }},
            { ItemType<DarkEnergyStaff>(), new Element[]{ Element.Dark }},
            { ItemType<Deicide>(), new Element[]{ Element.Impact, Element.Unholy }},
            { ItemType<DemonCore>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<Driftorcher>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<DualCane>(), new Element[]{ Element.Magic }},
            { ItemType<EcologicalCollapse>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<Exodimus>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<Exosphear>(), new Element[]{ Element.Stab, Element.Machine, Element.Fire, Element.Cold }},
            { ItemType<FlamingIceBow>(), new Element[]{ Element.Stab, Element.Fire, Element.Cold }},
            { ItemType<FlounderMortar>(), new Element[]{ Element.Poison }},
            { ItemType<GearworkShield>(), new Element[]{ Element.Impact, Element.Machine }},
            { ItemType<GrandReef>(), new Element[]{ Element.Impact, Element.Water }},
            { ItemType<GrarbleBayonet>(), new Element[]{ Element.Stab }},
            { ItemType<GrarbleSpear>(), new Element[]{ Element.Stab }},
            { ItemType<HadopelagicEcho>(), new Element[]{ Element.Water }},
            { ItemType<HalbardoftheHolidays>(), new Element[]{ Element.Slash, Element.Cold }},
            { ItemType<IchorDagger>(), new Element[]{ Element.Stab }},
            { ItemType<MackerelStaff>(), new Element[]{ Element.Impact, Element.Holy }},
            { ItemType<Megaskeet>(), new Element[]{ Element.Stab, Element.Holy }},
            { ItemType<Morpho>(), new Element[]{ Element.Slash, Element.Fire }},
            { ItemType<Ogscule>(), new Element[]{ Element.Unholy }},
            { ItemType<PinesPenetrator>(), new Element[]{ Element.Stab, Element.Cold }},
            { ItemType<ProfanedNucleus>(), new Element[]{ Element.Impact, Element.Holy, Element.Fire }},
            { ItemType<SDOMG>(), new Element[]{ Element.Stab, Element.Machine }},
            { ItemType<ShadowsDescent>(), new Element[]{ Element.Impact, Element.Dark }},
            { ItemType<Snowgrave>(), new Element[]{ Element.Cold }},
            { ItemType<TendonTides>(), new Element[]{ Element.Slash, Element.Poison }},
            { ItemType<TenebrisTides>(), new Element[]{ Element.Slash, Element.Water }},
            { ItemType<TheDreamingGhost>(), new Element[]{ Element.Magic, Element.Holy }},
            { ItemType<ThunderBolt>(), new Element[]{ Element.Magic }},
            { ItemType<TitanTides>(), new Element[]{ Element.Slash, Element.Magic, Element.Dark }},
            { ItemType<TotalityTides>(), new Element[]{ Element.Slash, Element.Machine, Element.Fire, Element.Cold  }},
            { ItemType<TyrantShield>(), new Element[]{ Element.Impact, Element.Holy }},
            { ItemType<UnsealedSingularity>(), new Element[]{ Element.Impact, Element.Magic, Element.Dark }},
            { ItemType<WinterBreeze>(), new Element[]{ Element.Stab, Element.Cold }},
            { ItemType<WrathoftheCosmos>(), new Element[]{ Element.Magic, Element.Dark }},
            { ItemType<WrathoftheDragons>(), new Element[]{ Element.Fire }},
            { ItemType<WrathoftheEldritch>(), new Element[]{ Element.Magic }},
            { ItemType<WrathoftheGods>(), new Element[]{ Element.Unholy }},
            { ItemType<WreathofBelial>(), new Element[]{ Element.Slash, Element.Cold, Element.Poison }},
            { ItemType<WulfrumLeechDagger>(), new Element[]{ Element.Stab, Element.Machine }},
        };
        // Vulnerable - Resistant
        internal static Dictionary<int, Tuple<Element[], Element[]>> Bosses = new()
        {
            { NPCType<WulfwyrmHead>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Machine, Element.Slash }) },
            { NPCType<WulfwyrmBody>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Machine, Element.Slash }) },
            { NPCType<WulfwyrmTail>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Magic, Element.Water, Element.Fire }, new Element[]{ Element.Machine, Element.Slash }) },
            //{ NPCType<Acideye>(), new Tuple<Element[], Element[]>(new Element[]{ Element.Stab, Element.Cold }, new Element[]{ Element.Poison, Element.Water, Element.Dark }) },
        };
    }
}
