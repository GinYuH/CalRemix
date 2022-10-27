
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.IO;

namespace CalRemix
{
	public class CalRemixPlayer : ModPlayer
	{
		public bool brimPortal;
		public bool arcanumHands;
		public bool marnite;
		public int marnitetimer = 1200;

		public override void ResetEffects()
		{
			brimPortal = false;
			arcanumHands = false;
			marnite = false;
			marnitetimer = 0;
		}
    }
}