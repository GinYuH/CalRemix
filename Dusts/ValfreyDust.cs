using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Dusts
{
	public class ValfreyDust : ModDust
	{
		public override void OnSpawn(Dust dust) 
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 26, 26);
		}

		public override bool Update(Dust dust) 
		{
			dust.position += dust.velocity;
			dust.scale -= 0.05f;

			if (dust.scale < 0.1f)
				dust.active = false;

			return false;
		}
	}
}
