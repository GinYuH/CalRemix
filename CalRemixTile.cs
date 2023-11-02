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
using CalamityMod.Tiles;
using CalRemix.UI;
using System.Linq;
using CalamityMod.Items.Placeables.Ores;
using Microsoft.CodeAnalysis;

namespace CalRemix
{
	public class CalRemixTile : GlobalTile
	{
		private int berryCount;
        private int cosmicCount;
        public static FannyMessage roxm;
        public static FannyMessage KinsmanMessage;
        public static FannyMessage MineMessage;

        public override void SetStaticDefaults()
        {
            roxm = new FannyMessage("Roxcalibur", "That's a Roxcalibur! You could shimmer it and try and get things earlier to make lategame easier!", "Awooga",
                FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation();
            KinsmanMessage = new FannyMessage("Kinsman", "Watch out! I'm sensing a strange elemental field coming from that onyx drill! Brace yourself for a fight!", "Nuhuh",
                FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation();
            MineMessage = new FannyMessage("OreExc", "Gee, you're a real excavating monster! If you really plan on mining this much, why don't you rebind your Excavation key to LeftClick? It'll save you a lot of unnecessary finger movement!", "Nuhuh",
                (FannySceneMetrics metrics) => ModLoader.HasMod("OreExcavator"), onlyPlayOnce: true).NeedsActivation().SetHoverTextOverride(Main.rand.NextBool(100) ? "Sure Fanny, I'll be a real gangsta thug and do that right now homeboy." : "Sure Fanny, I'll do that right now!");
            FannyManager.LoadFannyMessage(roxm);
            FannyManager.LoadFannyMessage(KinsmanMessage);
            FannyManager.LoadFannyMessage(MineMessage);
        }


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
            if (CalRemixWorld.meldCountdown <= 0)
            {
                int rand = 4 - WorldGen.GetWorldSize();
                if (Main.rand.NextBool(222 * rand))
                {
                    if (CalRemixWorld.astrallist.Contains(tile.TileType))
                    {
                        int LocationX = i;
                        int LocationY = j;
                        bool getMelded = false;
                        bool somethingConverted = false;
                        int checkRad = 8;
                        int spreadRad = 4;
                        for (int x = LocationX - checkRad; x <= LocationX + checkRad; x++)
                        {
                            for (int y = LocationY - checkRad; y <= LocationY + checkRad; y++)
                            {
                                if (Main.tile[x, y].TileType == ModContent.TileType<MeldGunkPlaced>())
                                {
                                    getMelded = true;
                                    break;
                                }
                            }
                        }
                        if (getMelded)
                        {
                            for (int x = LocationX - spreadRad; x <= LocationX + spreadRad; x++)
                            {
                                for (int y = LocationY - spreadRad; y <= LocationY + spreadRad; y++)
                                {
                                    if (Main.rand.NextBool(6))
                                    {
                                        if (Vector2.Distance(new Vector2(LocationX, LocationY), new Vector2(x, y)) <= spreadRad)
                                        {
                                            somethingConverted = true;
                                            CalamityMod.World.AstralBiome.ConvertToAstral(x, y);
                                        }
                                    }
                                }
                            }
                            if (somethingConverted)
                            {
                                Main.tile[i, j].TileType = (ushort)ModContent.TileType<MeldGunkPlaced>();
                                WorldGen.SquareTileFrame(i, j, true);
                            }
                        }
                    }
                }
            }
        }

        public override void NearbyEffects(int i, int j, int type, bool closer)
        {
            if (!roxm.alreadySeen)
            {
                if (type == ModContent.TileType<RoxTile>())
                {
                    roxm.ActivateMessage();
                }
            }
            if (!KinsmanMessage.alreadySeen)
            {
                if (type == ModContent.TileType<OnyxExcavatorTile>() && CalRemixWorld.downedEarth)
                {
                    Player player = Main.LocalPlayer;
                    bool e = player.HasItem(ModContent.ItemType<EyeoftheStorm>()) || player.HasItem(ModContent.ItemType<WifeinaBottle>()) || player.HasItem(ModContent.ItemType<WifeinaBottlewithBoobs>()) || player.HasItem(ModContent.ItemType<EyeoftheStorm>()) || player.HasItem(ModContent.ItemType<PearlofEnthrallment>()) || player.HasItem(ModContent.ItemType<InfectedRemote>());
                    if (e)
                        KinsmanMessage.ActivateMessage();
                }
            }
            if (type == ModContent.TileType<CalamityMod.Tiles.Ores.UelibloomOre>())
            {
                Main.tile[i, j].TileType = (ushort)TileID.Mud;
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (ModLoader.HasMod("OreExcavator"))
            {
                if (!MineMessage.alreadySeen)
                {
                    if (TileID.Sets.Ore[type])
                    {
                        if (Main.rand.NextBool(100))
                        {
                            MineMessage.ActivateMessage();
                        }
                    }
                }
            }
        }
    }
}