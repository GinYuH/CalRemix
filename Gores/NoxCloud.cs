using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Gores
{
	public class NoxCloud : ModGore
	{
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
            gore.numFrames = 1;
            gore.behindTiles = false;
			gore.alpha = 180;
        }
		public override bool Update(Gore gore)
		{
            gore.rotation += Main.rand.Next(-10, 11);
            gore.velocity *= 0;
			return true;
		}

	}
}