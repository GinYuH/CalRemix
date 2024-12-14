using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Tiles;
using CalRemix.Content.NPCs.Bosses.Ionogen;
namespace CalRemix.Core.Scenes
{
    public class OracleScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            TileEntity t = null;
            foreach (var v in TileEntity.ByPosition)
            {
                if (v.Value is IonCubeTE)
                {
                    t = v.Value;
                    break;
                }
            }
            if (t != null)
            {
                if (t.Position.ToWorldCoordinates().Distance(player.position) < 600)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<Ionogen>()))
                        return true;
                }
            }
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => CalRemixMusic.PlasticOracle;
    }
}