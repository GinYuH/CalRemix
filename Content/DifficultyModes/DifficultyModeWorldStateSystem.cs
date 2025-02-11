using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static CalRemix.Content.DifficultyModes.DifficultyModeWorld;

namespace CalRemix.Content.DifficultyModes
{
    public class MiscWorldStateSystem : ModSystem
    {
        public override void ClearWorld()
        {
            titanicMode = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var worldData = new List<string>();
            if (titanicMode)
                worldData.Add("titanicMode");
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var worldData = tag.GetList<string>("worldData");
            titanicMode = worldData.Contains("titanicMode");
        }
    }
}
