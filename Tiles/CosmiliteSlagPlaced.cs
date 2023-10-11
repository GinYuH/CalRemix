using CalRemix.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Tiles
{
	public class CosmiliteSlagPlaced : ModTile
	{
		public override void SetStaticDefaults() 
		{
			MineResist = 2.2f;
			MinPick = 225;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 770;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Life Ore");
			AddMapEntry(new Color(204, 114, 198), name);
		}
	}
}