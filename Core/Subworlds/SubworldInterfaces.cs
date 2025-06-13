using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
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
    }
}
