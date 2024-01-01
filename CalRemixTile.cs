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
using CalRemix.NPCs.Minibosses;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Tiles;
using CalRemix.UI;
using System.Linq;
using CalamityMod.Items.Placeables.Ores;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Tiles.SunkenSea;

namespace CalRemix
{
	public class CalRemixTile : GlobalTile
	{
		private int berryCount;
        private int cosmicCount;
        public static FannyMessage roxm;
        public static FannyMessage KinsmanMessage;
        public static FannyMessage MineMessage;
        public static FannyMessage GrimeMessage;

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            roxm = new FannyMessage("Roxcalibur", "That's a Roxcalibur! You could shimmer it and try and get things earlier to make lategame easier!", "Awooga",
                FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation();
            KinsmanMessage = new FannyMessage("Kinsman", "Watch out! I'm sensing a strange elemental field coming from that onyx drill! Brace yourself for a fight!", "Nuhuh",
                FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation();
            GrimeMessage = new FannyMessage("Grimesand", "See that weird dark splotch over there? That is Grimesand, it's pretty grimey. You can throw evil materials onto it for epic rewards or lead enemies into it for scary stuff to happen.", "Nuhuh",
            FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation();
            if (CalRemix.instance.VeinMiner != null)
            {
                MineMessage = new FannyMessage("OreExc", "Gee, you're a real excavating monster! If you really plan on mining this much, why don't you rebind your Excavation key to LeftClick? It'll save you a lot of unnecessary finger movement!", "Nuhuh",
                    FannyMessage.AlwaysShow, onlyPlayOnce: true).NeedsActivation().SetHoverTextOverride("Sure Fanny, I'll do that right now!");
                FannyManager.LoadFannyMessage(MineMessage);
            }
            FannyManager.LoadFannyMessage(roxm);
            FannyManager.LoadFannyMessage(KinsmanMessage);
            FannyManager.LoadFannyMessage(GrimeMessage);
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
            if (type == ModContent.TileType<AstralBeacon>() && player.HeldItem.type == ModContent.ItemType<BloodyVein>() && CalRemixWorld.permanenthealth)
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
            bool e = player.HasItem(ModContent.ItemType<EyeoftheStorm>()) || player.HasItem(ModContent.ItemType<WifeinaBottle>()) || player.HasItem(ModContent.ItemType<WifeinaBottlewithBoobs>()) || player.HasItem(ModContent.ItemType<RoseStone>()) || player.HasItem(ModContent.ItemType<PearlofEnthrallment>()) || player.HasItem(ModContent.ItemType<InfectedRemote>());
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
            if (CalRemixWorld.permanenthealth)
            {
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
            if (CalRemixWorld.meldGunk)
            {
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
        }

        public override void NearbyEffects(int i, int j, int type, bool closer)
        {
            if (!Main.dedServ)
            {
                if (FannyManager.fannyEnabled)
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
                    if (!GrimeMessage.alreadySeen)
                    {
                        if (type == ModContent.TileType<GrimesandPlaced>())
                        {
                            GrimeMessage.ActivateMessage();
                        }
                    }
                }
                if (CalRemixWorld.reargar)
                {
                    if (type == ModContent.TileType<CalamityMod.Tiles.Ores.UelibloomOre>())
                    {
                        Main.tile[i, j].TileType = (ushort)TileID.Mud;
                    }
                }
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!Main.dedServ)
            {
                if (FannyManager.fannyEnabled)
                {
                    if (CalRemix.instance.VeinMiner != null)
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
            if (CalRemixWorld.grimesandToggle)
            {
                if (!CalRemixWorld.grime)
                {
                    if (type == TileID.ShadowOrbs)
                    {
                        CalamityMod.CalamityUtils.SpawnOre(ModContent.TileType<GrimesandPlaced>(), 6E-05, 0, 0.05f + WorldGen.GetWorldSize() * 0.05f, 5, 10, TileID.Dirt, TileID.Mud, TileID.Cloud, TileID.RainCloud);
                        Main.NewText("The sky islands pollute with grime...", Color.Brown);
                        CalRemixWorld.grime = true;
                        CalRemixWorld.UpdateWorldBool();
                    }
                }
            }
            if (type == TileID.LargePiles2)
            {
                if (Main.bloodMoon)
                {
                    if ((Main.LocalPlayer.ZonePurity && Main.LocalPlayer.position.Y >= Main.worldSurface + 30) || Main.LocalPlayer.ZoneJungle) // i'm too lazy to get all the tile frames
                    {
                        if (Main.netMode != NetmodeID.Server && Main.rand.NextBool(60)) // roughly 10% when considering the piles are 6 tiles in size
                        {
                            NPC.NewNPC(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, ModContent.NPCType<GulletWorm>());
                        }
                    }
                }
            }
        }

        internal static void SlopedGlowmask(int i, int j, int type, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color drawColor, Vector2 positionOffset, bool overrideTileFrame = false)
        {
            Tile tile = Main.tile[i, j];
            int num = tile.TileFrameX;
            int num2 = tile.TileFrameY;
            if (overrideTileFrame)
            {
                num = 0;
                num2 = 0;
            }

            int width = 16;
            int height = 16;
            if (sourceRectangle.HasValue)
            {
                num = sourceRectangle.Value.X;
                num2 = sourceRectangle.Value.Y;
            }

            int num3 = i * 16;
            int num4 = j * 16;
            Vector2 vector = new Vector2(num3, num4);
            Vector2 vector2 = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                vector2 = Vector2.Zero;
            }

            Vector2 vector3 = -Main.screenPosition + vector2 + positionOffset;
            Vector2 position2 = vector + vector3;
            if ((tile.Slope == SlopeType.Solid && !tile.IsHalfBlock) || (Main.tileSolid[tile.TileType] && Main.tileSolidTop[tile.TileType]))
            {
                Main.spriteBatch.Draw(texture, position2, new Rectangle(num, num2, width, height), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                return;
            }

            if (tile.IsHalfBlock)
            {
                Main.spriteBatch.Draw(texture, new Vector2(position2.X, position2.Y + 8f), new Rectangle(num, num2, width, 8), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                return;
            }

            byte b = (byte)tile.Slope;
            Vector2 position3;
            if (b == 1 || b == 2)
            {
                Rectangle value;
                for (int k = 0; k < 8; k++)
                {
                    int num5 = k * 2;
                    int num6;
                    int height2;
                    if (b == 2)
                    {
                        num6 = 16 - num5 - 2;
                        height2 = 14 - num5;
                    }
                    else
                    {
                        num6 = num5;
                        height2 = 14 - num6;
                    }

                    value = new Rectangle(num + num6, num2, 2, height2);
                    position3 = new Vector2(num3 + num6, num4 + num5) + vector3;
                    Main.spriteBatch.Draw(texture, position3, value, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }

                value = new Rectangle(num, num2 + 14, 16, 2);
                position3 = new Vector2(num3, num4 + 14) + vector3;
                Main.spriteBatch.Draw(texture, position3, value, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                return;
            }

            for (int l = 0; l < 8; l++)
            {
                int num7 = l * 2;
                int num8;
                int num9;
                if (b == 3)
                {
                    num8 = num7;
                    num9 = 16 - num8;
                }
                else
                {
                    num8 = 16 - num7 - 2;
                    num9 = 16 - num7;
                }

                Rectangle value = new Rectangle(num + num8, num2 + 16 - num9, 2, num9);
                position3 = new Vector2(num3 + num8, num4) + vector3;
                Main.spriteBatch.Draw(texture, position3, value, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            position3 = new Vector2(num3, num4) + vector3;
            if (tile.TileType != ModContent.TileType<EutrophicGlass>())
            {
                Rectangle value = new Rectangle(num, num2, 16, 2);
                Main.spriteBatch.Draw(texture, position3, value, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}