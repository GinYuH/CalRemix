using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Gores
{
	public class NoxCal : ModGore
	{
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
            gore.numFrames = 1;
            gore.behindTiles = false;
            gore.timeLeft = 120;
            gore.scale = 2;
			gore.alpha = 120;
            gore.position.X += Main.rand.Next(-200, 201);
            gore.position.Y += Main.rand.Next(-200, 201);
        }
		public override bool Update(Gore gore)
		{
			gore.alpha++;
			gore.scale += 0.01f;
            gore.velocity *= 0;
            return true;
		}

	}
}