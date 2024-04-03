using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Tiles
{
    public class LifeOreTile : ModTile
	{
		public override void SetStaticDefaults() 
		{
			MineResist = 4f;
			MinPick = 200;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 710;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Life Ore");
			AddMapEntry(new Color(72, 107, 98), name);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) 
		{
			r = Main.DiscoR / 1020f;
			g = Main.DiscoG / 1020f;
			b = Main.DiscoB / 1020f;
		}
	}
}