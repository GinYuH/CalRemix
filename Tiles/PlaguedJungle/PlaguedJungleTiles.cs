using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using CalamityMod;
using CalRemix.Projectiles;

namespace CalRemix.Tiles.PlaguedJungle
{/*Blocks*/
    public class PlaguedGrass : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			CalamityUtils.SetMerge(Type, 2);
			CalamityUtils.SetMerge(Type, 23);
			CalamityUtils.SetMerge(Type, 109);
			CalamityUtils.SetMerge(Type, 199);
			CalamityUtils.SetMerge(Type, 60);
			CalamityUtils.SetMerge(Type, 59);
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedMud>();
			AddMapEntry(new Color(206, 255, 115));
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<PlaguedMud>();
		}
		public override void RandomUpdate(int i, int j)
		{
			if (!Main.rand.NextBool(8))
				if (!Main.tile[i, j - 1].HasTile)
				{
					WorldGen.PlaceTile(i, j - 1, ModContent.TileType<PlagueGrassShort>(), true, false, -1, Main.rand.Next(12));
					NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
				}
				else if (Main.rand.NextBool(8))
					GrowPlaguedVine(i, j);
			base.RandomUpdate(i, j);
		}
		public static void GrowPlaguedVine(int i, int j)
		{
			if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].LiquidType != LiquidID.Lava)
			{
				var flag9 = false;
				for (var VineY = j; VineY > j - 10; VineY--)
				{
					if (Main.tile[i, VineY].BottomSlope)
					{
						flag9 = false;
						break;
					}
					if (Main.tile[i, VineY].HasTile && !Main.tile[i, VineY].BottomSlope)
					{
						flag9 = true;
						break;
					}
				}

				if (flag9)
				{
					var num47 = i;
					var num48 = j + 1;
					if (Main.tile[num47, num48].LiquidAmount == 0)
					{
						Main.tile[num47, num48].TileType = (ushort)ModContent.TileType<PlaguedVine>();
						WorldGen.SquareTileFrame(num47, num48, true);
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendTileSquare(-1, num47, num48, 3, TileChangeType.None);
						}
					}
				}
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 224f / 255f * 0.6f;
			g = 255f / 255f * 0.6f;
			b = 166f / 255f * 0.6f;
			base.ModifyLight(i, j, ref r, ref g, ref b);
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<PlaguedMud>();
			}
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, 178, 0f, 0f, 1, new Color(255, 255, 255));
			return false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
	}
	public class PlaguedMud : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			CalamityUtils.MergeWithGeneral(Type);
			CalamityUtils.MergeAstralTiles(Type);
			CalamityUtils.MergeWithOres(Type);
			CalamityUtils.SetMerge(Type, 2);
			CalamityUtils.SetMerge(Type, 23);
			CalamityUtils.SetMerge(Type, 109);
			CalamityUtils.SetMerge(Type, 199);
			CalamityUtils.SetMerge(Type, 60);
			Main.tileMerge[Type][ModContent.TileType<PlaguedStone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPlaguedStone>()] = true;
			DustType = 36;
			AddMapEntry(new Color(25, 47, 50));
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedGrass>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class PlaguedStone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<OvergrownPlaguedStone>());
			HitSound = SoundID.Tink;
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedStone>();
			AddMapEntry(new Color(55, 67, 49));
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedGrass>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class OvergrownPlaguedStone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedStone>());
            HitSound = SoundID.Tink;
            DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.OvergrownPlaguedStone>();
			AddMapEntry(new Color(52, 64, 46));
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedGrass>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class PlaguedPipe : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.MergeWithGeneral(Type);
			CalamityUtils.MergeAstralTiles(Type);
			CalamityUtils.MergeWithOres(Type);
			CalamityUtils.SetMerge(Type, 2);
			CalamityUtils.SetMerge(Type, 23);
			CalamityUtils.SetMerge(Type, 109);
			CalamityUtils.SetMerge(Type, 199);
			CalamityUtils.SetMerge(Type, 60);
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedStone>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<OvergrownPlaguedStone>());
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedPipe>();
			AddMapEntry(new Color(46, 54, 56));
		}
	}
	public class UndeadPlaguePipe : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<Sporezol>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedStone>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<OvergrownPlaguedStone>());
			DustType = 1;
			AddMapEntry(new Color(55, 67, 49));
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedGrass>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class Sporezol : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlendAll[Type] = false;
			CalamityUtils.SetMerge(Type, ModContent.TileType<UndeadPlaguePipe>());
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.Sporezol>();
			AddMapEntry(new Color(159, 188, 22));
			MinPick = 22222;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 255f / 255f * 0.4f;
			g = 252f / 255f * 0.4f;
			b = 96f / 255f * 0.4f;
			base.ModifyLight(i, j, ref r, ref g, ref b);
		}

	}
	public class PlaguedClay : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, 2);
			CalamityUtils.SetMerge(Type, 23);
			CalamityUtils.SetMerge(Type, 109);
			CalamityUtils.SetMerge(Type, 199);
			CalamityUtils.SetMerge(Type, 60);
			CalamityUtils.SetMerge(Type, 59);
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedStone>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<OvergrownPlaguedStone>());
            HitSound = SoundID.Tink;
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedClay>();
			AddMapEntry(new Color(33, 55, 45));
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedGrass>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}

	}
	public class PlaguedSilt : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, 2);
			CalamityUtils.SetMerge(Type, 23);
			CalamityUtils.SetMerge(Type, 109);
			CalamityUtils.SetMerge(Type, 199);
			CalamityUtils.SetMerge(Type, 60);
			CalamityUtils.SetMerge(Type, 59);
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedStone>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<OvergrownPlaguedStone>());
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedSilt>();
			AddMapEntry(new Color(30, 52, 36));
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedGrass>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}

	}
	public class PlaguedSand : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, 2);
			CalamityUtils.SetMerge(Type, 23);
			CalamityUtils.SetMerge(Type, 109);
			CalamityUtils.SetMerge(Type, 199);
			CalamityUtils.SetMerge(Type, 60);
			CalamityUtils.SetMerge(Type, 59);
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedStone>());
			CalamityUtils.SetMerge(Type, ModContent.TileType<OvergrownPlaguedStone>());
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedSand>();
			AddMapEntry(new Color(154, 169, 124));
			//SetModPalmTree(new PlaguedPalmTree());
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			if (WorldGen.noTileActions)
				return true;

			Tile above = Main.tile[i, j - 1];
			Tile below = Main.tile[i, j + 1];
			bool canFall = true;

			if (below == null || below.HasTile)
				canFall = false;

			if (above.HasTile && (TileID.Sets.BasicChest[above.TileType] || TileID.Sets.BasicChestFake[above.TileType] || above.TileType == TileID.PalmTree || TileID.Sets.BasicDresser[above.TileType]))
				canFall = false;

			if (canFall)
			{
				//Set the projectile type to ExampleSandProjectile
				int projectileType = ModContent.ProjectileType<PlaguedFallingSand>();
				float positionX = i * 16 + 8;
				float positionY = j * 16 + 8;

				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.tile[i, j].ClearTile();
					int proj = Projectile.NewProjectile(new EntitySource_TileBreak(i, j), positionX, positionY, 0f, 0.41f, projectileType, 10, 0f, Main.myPlayer);
					Main.projectile[proj].ai[0] = 1f;
					WorldGen.SquareTileFrame(i, j);
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					bool spawnProj = true;

					for (int k = 0; k < 1000; k++)
					{
						Projectile otherProj = Main.projectile[k];

						if (otherProj.active && otherProj.owner == Main.myPlayer && otherProj.type == projectileType && Math.Abs(otherProj.timeLeft - 3600) < 60 && otherProj.Distance(new Vector2(positionX, positionY)) < 4f)
						{
							spawnProj = false;
							break;
						}
					}

					if (spawnProj)
					{
						int proj = Projectile.NewProjectile(new EntitySource_TileBreak(i, j), positionX, positionY, 0f, 2.5f, projectileType, 10, 0f, Main.myPlayer);
						Main.projectile[proj].velocity.Y = 0.5f;
						Main.projectile[proj].position.Y += 2f;
						Main.projectile[proj].netUpdate = true;
					}

					NetMessage.SendTileSquare(-1, i, j, 1);
					WorldGen.SquareTileFrame(i, j);
				}
				return false;
			}
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}
	public class PlaguedHive : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			CalamityUtils.SetMerge(Type, ModContent.TileType<PlaguedMud>());
			DustType = 1;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedHive>();
			AddMapEntry(new Color(40, 66, 70));
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
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRemix/Tiles/PlaguedJungle/PlaguedHiveGlow").Value,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(155, 155, 155, 200), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			PlagueBiomeTileFrameHelper.MergeWithFrame(i, j, Type, ModContent.TileType<PlaguedMud>(), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
		/*public override void ChangeWaterfallStyle(ref int style)
		{
			style = mod.GetWaterfallStyleSlot("ExampleWaterfallStyle");
		}*/
	}
	public class PlaguedHiveWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(14, 28, 31));
			DustType = 36;
		}
	}
	public class PlaguedVine : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
			AddMapEntry(new Color(64, 89, 85));
			HitSound = SoundID.Grass;
			DustType = DustID.Grass;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				ModContent.TileType<PlaguedMud>(),
				ModContent.TileType<PlaguedVine>(),
			};
			TileObjectData.addTile(Type);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num--;
			base.NumDust(i, j, fail, ref num);
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			if (!Main.tile[i, j - 1].HasTile || !(Main.tile[i, j - 1].TileType == ModContent.TileType<PlaguedGrass>() || Main.tile[i, j - 1].TileType == ModContent.TileType<PlaguedVine>()))
				WorldGen.KillTile(i, j, false, false, false);
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}
		public override void RandomUpdate(int i, int j)
		{
			if (Main.rand.NextBool(4))
				PlaguedGrass.GrowPlaguedVine(i, j);
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
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Tiles/PlaguedJungle/PlaguedVineGlow").Value,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(155, 155, 155, 200), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}
	/*Walls*/
	public class PlaguedMudWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(12, 28, 31));
			DustType = 36;
		}
	}
	public class PlaguedPipeWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			AddMapEntry(new Color(37, 48, 46));
			DustType = 36;
		}
	}
	public class PlaguedStoneWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(29, 35, 24));
			DustType = 36;
		}
	}
	public class PlaguedVineWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(22, 37, 40));
			DustType = 2;
		}
	}
	/*WallsSafe*/
	public class PlaguedMudWallSafe : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(12, 28, 31));
			DustType = 36;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedMudWall>();
		}
	}
	public class PlaguedStoneWallSafe : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(29, 35, 24));
			DustType = 36;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedStoneWall>();
		}
	}
	public class PlaguedVineWallSafe : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(22, 37, 40));
			DustType = 2;
			//drop = ModContent.ItemType<Items.Placeable.PlaguedJungle.PlaguedVineWall>();
		}
	}
}


