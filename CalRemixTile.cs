using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Pets;
using System.Collections.Generic;
using CalamityMod.Tiles.Astral;
using CalRemix.Tiles;
using CalamityMod;
using CalamityMod.Items.PermanentBoosters;
using Microsoft.Xna.Framework;

namespace CalRemix
{
	public class CalRemixTile : GlobalTile
	{
		private int berryCount;
        private int cosmicCount;

        List<int> exclusionlist = new List<int>
        {
            0,
            TileID.Plants,
            TileID.Plants2,
            TileID.LargePiles,
            TileID.LargePiles2,
            TileID.SmallPiles,
            TileID.JunglePlants,
            TileID.JunglePlants2,
            TileID.JungleThorns,
            TileID.JungleVines,
            233
        };
        public override void RightClick(int i, int j, int type)
        {
			if (type == ModContent.TileType<AstralBeacon>() && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<BloodyVein>())
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9, Main.LocalPlayer.Center);
                if (!Main.LocalPlayer.Calamity().eCore)
					Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), Main.LocalPlayer.getRect(), ModContent.ItemType<EtherealCore>());
				else if (Main.rand.NextBool(20))
					Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), Main.LocalPlayer.getRect(), ModContent.ItemType<EtherealCore>());
			}
            if (type == TileID.Containers && Main.tile[i,j].TileFrameX == 432 || Main.tile[i,j].TileFrameX == 450)
            {
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest cheste = Main.chest[chestIndex];
                    if (cheste != null && Main.tile[cheste.x, cheste.y].TileType == TileID.Containers)
                    {
                        if (NPC.CountNPCS(ModContent.NPCType<NPCs.Bosses.WulfrumExcavatorHead>()) < 1 && cheste.item[0].type == ModContent.ItemType<CalamityMod.Items.Materials.EnergyCore>() && cheste.item[0].stack == 1 && cheste.item[1].type == ItemID.None)
                        {
                            cheste.item[0].stack = 0;
                            int guy = NPC.NewNPC(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16 + 600, ModContent.NPCType<NPCs.Bosses.WulfrumExcavatorHead>());
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, new Vector2(i * 16, j * 16));
                            if (guy.WithinBounds(Main.maxNPCs))
                            {
                                Main.npc[guy].velocity.Y = -20;
                            }
                        }
                    }
                }
            }
        }
        public override void RandomUpdate(int i, int j, int type)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Tile tile2 = Framing.GetTileSafely(i + 1, j);

            if (tile.TileType == TileID.JungleGrass && tile.HasUnactuatedTile && tile2.TileType == TileID.JungleGrass && tile2.HasUnactuatedTile && j < 100)
            {
                berryCount = 0;

                int r = 80;
                int xRadStart = i - r;
                int xRadEnd = xRadStart + (r * 2);
                int yRadStart = j - r;
                int yRadEnd = yRadStart + (r * 2);

                for (int x = xRadStart; x < xRadEnd && x < Main.maxTilesX; x++)
                {
                    for (int y = yRadStart; y < yRadEnd && y < Main.maxTilesY; y++)
                    {
                        Tile tileCount = Framing.GetTileSafely(x, y);

                        if (tileCount.TileType == ModContent.TileType<MiracleFruitPlaced>())
                            berryCount++;
                    }
                }

                if (berryCount < 6 && j < 100 && Main.rand.NextBool(48) && NPC.downedGolemBoss)
                {
                    if (exclusionlist.Contains(Main.tile[i, j - 1].TileType) && Main.tile[i, j].Slope == 0 && !Main.tile[i, j].IsHalfBlock)
                    {
                        if (exclusionlist.Contains(Main.tile[i + 1, j - 1].TileType) && Main.tile[i, j].Slope == 0 && !Main.tile[i, j].IsHalfBlock)
                        {
                            WorldGen.PlaceObject(i, j - 1, ModContent.TileType<MiracleFruitPlaced>(), true);
                        }
                    }
                }
            }
            if ((tile.TileType == TileID.Stone || tile.TileType == TileID.Grass) && tile.HasUnactuatedTile && j < 100)
            {
                cosmicCount = 0;

                int r = 80;
                int xRadStart = i - r;
                int xRadEnd = xRadStart + (r * 2);
                int yRadStart = j - r;
                int yRadEnd = yRadStart + (r * 2);

                for (int x = xRadStart; x < xRadEnd && x < Main.maxTilesX; x++)
                {
                    for (int y = yRadStart; y < yRadEnd && y < Main.maxTilesY; y++)
                    {
                        Tile tileCount = Framing.GetTileSafely(x, y);

                        if (tileCount.TileType == ModContent.TileType<CosmichidPlant>())
                            cosmicCount++;
                    }
                }

                if (cosmicCount < 10 && j < 100 && Main.rand.NextBool(48) && Main.hardMode)
                {
                    if (exclusionlist.Contains(Main.tile[i, j - 1].TileType) && Main.tile[i, j].Slope == 0 && !Main.tile[i, j].IsHalfBlock)
                    {
                        WorldGen.PlaceObject(i, j - 1, ModContent.TileType<CosmichidPlant>(), true);
                    }
                }
            }
        }
    }
}