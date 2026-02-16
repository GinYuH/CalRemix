using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.DifficultyModes
{
    public class DifficultyModeNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (DifficultyModeWorld.titanicMode == true)
            {
                npc.scale *= 2;
                npc.width *= 2;
                npc.height *= 2;
            }
        }
    }
}
