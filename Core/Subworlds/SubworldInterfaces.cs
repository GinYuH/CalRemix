using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace CalRemix.Core.Subworlds
{
    public interface ICustomSpawnSubworld
    {
        /// <summary>
        /// A list of NPC spawns for the Subworld
        /// int is the NPC's ID
        /// float is their spawn weight
        /// Predicate is their spawn condition
        /// </summary>
        /// <returns></returns>
        public List<(int, float, Predicate<NPCSpawnInfo>)> Spawns();

        /// <summary>
        /// The maximum amount of NPCs that can spawn
        /// </summary>
        public int MaxSpawns { get; }

        /// <summary>
        /// The spawn rate multiplier. Lower values means more frequent spawns.
        /// </summary>
        public float SpawnMult { get; }

        /// <summary>
        /// Set to true to disable the vanilla spawn system completely. This is mainly for allowing things like mid-air or lava NPCs
        /// </summary>
        public bool OverrideVanilla {  get; }
    }

    public interface IFixDrawBlack
    {

    }

    public interface IDisableSpawnsSubworld
    {

    }

    public interface IDisableOcean
    {

    }

    public interface IInfiniteFlight
    {

    }

    public interface IDisableFlight
    {

    }

    public interface IDisableBuilding
    {

    }
}
