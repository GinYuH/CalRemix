using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Projectiles.Magic;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Core.World;
namespace CalRemix.Core.Scenes
{
    public class RoachScene : ModSceneEffect
    {
        public static List<RealisticExplosion> explosions = new List<RealisticExplosion>() { };
        public override bool IsSceneEffectActive(Player player)
        {
            if (CalRemixWorld.roachDuration > 0)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/DemonChase");
    }
    public class RealisticExplosion
    {
        public int frameX = 0;
        public int frameY = 0;
        public int frameCounter = 0;
        public Vector2 position = Vector2.Zero;
        public RealisticExplosion(Vector2 position)
        {
            this.position = position;
        }
    }
}