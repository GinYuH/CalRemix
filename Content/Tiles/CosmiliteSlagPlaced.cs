using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
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
			AddMapEntry(new Color(204, 114, 198), name);
			HitSound = SoundID.Tink;
			DustType = (int)CalamityMod.Dusts.CalamityDusts.PurpleCosmilite;
		}
	}
}