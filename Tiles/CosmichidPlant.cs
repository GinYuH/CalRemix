using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Tiles
{
	public class CosmichidPlant : ModTile
	{
		public override void SetStaticDefaults() 
		{
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.AnchorValidTiles = new int[] {
                TileID.Grass,
                TileID.Stone,
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[] {
                TileID.ClayPot
            };
            TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cosmichid");
			AddMapEntry(new Color(156, 41, 143), name);
            TileID.Sets.SwaysInWindBasic[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            ItemDrop = ModContent.ItemType<Cosmichid>();
            DustType = DustID.PinkTorch;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) 
		{
			num = fail ? 1 : 3;
		}
        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile.HasTile)
            {
                int tileType = tile.TileType;
                if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
                {
                    bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
                    bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);

                    if (foliageGrass || moddedFoliage)
                    {
                        WorldGen.KillTile(i, j);
                        if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                        }

                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            return true;
        }
        public override void FloorVisuals(Player player)
        {
            player.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 2);
            base.FloorVisuals(player);
        }
    }
}