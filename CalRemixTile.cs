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
using CalamityMod.Tiles.Furniture;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Tiles.DraedonStructures;
using CalRemix.NPCs;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Summon;

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
            Player player = Main.LocalPlayer;
            if (type == ModContent.TileType<AstralBeacon>() && player.HeldItem.type == ModContent.ItemType<BloodyVein>())
            {
                SoundEngine.PlaySound(SoundID.Item9, player.Center);
                if (!player.Calamity().eCore)
					Item.NewItem(new EntitySource_TileBreak(i, j), player.getRect(), ModContent.ItemType<EtherealCore>());
				else if (Main.rand.NextBool(20))
					Item.NewItem(new EntitySource_TileBreak(i, j), player.getRect(), ModContent.ItemType<EtherealCore>());
			}
            if (type == ModContent.TileType<LabHologramProjector>() && player.HeldItem.type == ModContent.ItemType<BloodyVein>() && !NPC.AnyNPCs(ModContent.NPCType<CyberDraedon>()))
            {
                SoundEngine.PlaySound(SoundID.Item14, player.Center);
                SoundEngine.PlaySound(DecryptionComputer.InstallSound, player.Center);
                int index = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16, ModContent.NPCType<CyberDraedon>());
                if (Main.netMode == NetmodeID.MultiplayerClient && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
                WorldGen.KillTile(i, j, noItem: true);
                if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                }
                player.ConsumeItem(ModContent.ItemType<BloodyVein>());
            }
            if (type == ModContent.TileType<WulfrumLure>() && player.HeldItem.type == ModContent.ItemType<DraedonPowerCell>())
            {
                Main.NewText("If you are seeing this, you are likely using the outdated Fandom wiki, please do not use it. Use wiki.gg for Calamity and the recipe browser mod for remix", Color.LightSeaGreen);
            }
            if (type == ModContent.TileType<LabHologramProjector>() && player.HeldItem.type == ModContent.ItemType<BloodyVein>() && !NPC.AnyNPCs(ModContent.NPCType<CyberDraedon>()))
            {
                SoundEngine.PlaySound(SoundID.Item14, player.Center);
                SoundEngine.PlaySound(DecryptionComputer.InstallSound, player.Center);
                int index = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16, ModContent.NPCType<CyberDraedon>());
                if (Main.netMode == NetmodeID.MultiplayerClient && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
                WorldGen.KillTile(i, j);
                if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                }
                player.ConsumeItem(ModContent.ItemType<BloodyVein>(), true);
            }
            bool e = player.HasItem(ModContent.ItemType<EyeoftheStorm>()) || player.HasItem(ModContent.ItemType<WifeinaBottle>()) || player.HasItem(ModContent.ItemType<WifeinaBottlewithBoobs>()) || player.HasItem(ModContent.ItemType<EyeoftheStorm>()) || player.HasItem(ModContent.ItemType<PearlofEnthrallment>()) || player.HasItem(ModContent.ItemType<InfectedRemote>());
            if (type == ModContent.TileType<OnyxExcavatorTile>() && e && CalRemixWorld.downedEarth)
            {
                SoundEngine.PlaySound(SoundID.Item14, player.Center);
                int index = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16, ModContent.NPCType<OnyxKinsman>());
                if (Main.netMode == NetmodeID.MultiplayerClient && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
                WorldGen.KillTile(i, j);
                if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                }
            }
        }
        public override void MouseOver(int i, int j, int type)
        {
            Player player = Main.LocalPlayer;
            if (type == ModContent.TileType<LabHologramProjector>())
            {
                if (player.HeldItem.type == ModContent.ItemType<BloodyVein>())
                {
                    player.cursorItemIconID = ModContent.ItemType<BloodyVein>();
                }
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
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

            if (CalRemix.oreList.Contains(type) && Main.rand.NextBool(2222) && CalRemixWorld.downedExcavator)
            {
                int LocationX = i;
                int LocationY = j;
                for (int x = LocationX - 4; x <= LocationX + 4; x++)
                {
                    for (int y = LocationY - 4; y <= LocationY + 4; y++)
                    {
                        if (Vector2.Distance(new Vector2(LocationX, LocationY), new Vector2(x, y)) <= 4)
                        {
                            if (Main.tile[x, y].TileType == TileID.Stone || Main.tile[x, y].TileType == TileID.IceBlock)
                            {
                                Main.tile[x, y].TileType = (ushort)type;
                            }
                        }
                    }
                }
            }
        }
    }
}