using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace CalRemix.Content.Tiles.PlaguedJungle
{
    public class PlagueGrassShort : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
			AddMapEntry(new Color(72, 188, 44));
			HitSound = SoundID.Grass;
			DustType = DustID.Grass;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.Slope == 0 && !tile.IsHalfBlock)
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRemix/Content/Tiles/PlaguedJungle/PlagueGrassShortGlow").Value,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(155, 155, 155, 200), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num--;
			base.NumDust(i, j, fail, ref num);
		}
	}
}