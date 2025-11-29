using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Tiles;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.DraedonStructures;
using CalamityMod.Tiles.DraedonSummoner;
using CalamityMod.Tiles.Furniture;
using CalamityMod.Tiles.Ores;
using CalamityMod.Tiles.SunkenSea;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Bosses.Phytogen;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Tiles;
using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using static CalRemix.CalRemixHelper;
using static Terraria.ModLoader.ModContent;

namespace CalRemix
{
    public class CalRemixTile : GlobalTile
	{
		private int berryCount;
        private int cosmicCount;
        public static HelperMessage roxm;
        public static HelperMessage KinsmanMessage;
        public static HelperMessage MineMessage;
        public static HelperMessage GrimeMessage;

        public static int IonCubePlaced = -1;
        public static int RoxTile = -1;
        public static int OnyxExcavatorTile = -1;
        public static int GrimesandPlaced = -1;
        public static int UelibloomOre = -1;
        public static int RustedPipes = -1;


        public override void SetStaticDefaults()
        {
            IonCubePlaced = ModContent.TileType<IonCubePlaced>();
            RoxTile = ModContent.TileType<RoxTile>();
            OnyxExcavatorTile = ModContent.TileType<OnyxExcavatorTile>();
            GrimesandPlaced = ModContent.TileType<GrimesandPlaced>();
            UelibloomOre = ModContent.TileType<UelibloomOre>();
            RustedPipes = ModContent.TileType<RustedPipes>();
            if (Main.dedServ)
                return;
            roxm = HelperMessage.New("Roxcalibur", "That's a Roxcalibur! You could shimmer it and try and get things earlier to make lategame easier!", "FannyAwooga")
                .NeedsActivation();
            KinsmanMessage = HelperMessage.New("Kinsman", "Watch out! I'm sensing a strange elemental field coming from that onyx drill! Brace yourself for a fight!", "FannyNuhuh")
                .NeedsActivation();
            GrimeMessage = HelperMessage.New("Grimesand", "See that weird dark splotch over there? That is Grimesand, it's pretty grimey. You can throw evil materials onto it for epic rewards or lead enemies into it for scary stuff to happen.", "FannyNuhuh")
                .NeedsActivation();
            if (ModLoader.TryGetMod("OreExcavator", out _))
            {
                MineMessage = HelperMessage.New("OreExc", "Gee, you're a real excavating monster! If you really plan on mining this much, why don't you rebind your Excavation key to LeftClick? It'll save you a lot of unnecessary finger movement!", "FannyNuhuh")
                    .NeedsActivation().SetHoverTextOverride("Sure Fanny, I'll do that right now!");
            }
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
            if (type == TileType<AstralBeacon>() && player.HeldItem.type == ItemType<BloodyVein>() && CalRemixWorld.permanenthealth)
            {
                SoundEngine.PlaySound(SoundID.Item9, player.Center);
                if (!player.Calamity().eCore)
					Item.NewItem(new EntitySource_TileBreak(i, j), player.getRect(), ItemType<EtherealCore>());
				else if (Main.rand.NextBool(20))
					Item.NewItem(new EntitySource_TileBreak(i, j), player.getRect(), ItemType<EtherealCore>());
			}
            if (type == TileType<LabHologramProjector>() && player.HeldItem.type == ItemType<BloodyVein>() && !NPC.AnyNPCs(NPCType<CyberDraedon>()))
            {
                SoundEngine.PlaySound(SoundID.Item14, player.Center);
                SoundEngine.PlaySound(DecryptionComputer.InstallSound, player.Center);
                SpawnNewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16, NPCType<CyberDraedon>());
                DestroyTile(i, j, noItem: true);
                player.ConsumeItem(ItemType<BloodyVein>());
            }
            if (type == TileType<WulfrumLure>() && player.HeldItem.type == ItemType<DraedonPowerCell>())
            {
                CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.FuckFandom", Color.SeaGreen);
            }
            bool e = player.HasItem(ItemType<EyeoftheStorm>()) || player.HasItem(ItemType<WifeinaBottle>()) || player.HasItem(ItemType<WifeinaBottlewithBoobs>()) || player.HasItem(ItemType<RoseStone>()) || player.HasItem(ItemType<PearlofEnthrallment>()) || player.HasItem(ItemType<InfectedRemote>());
            if (type == TileType<OnyxExcavatorTile>() && e && RemixDowned.downedEarthElemental)
            {
                SoundEngine.PlaySound(SoundID.Item14, player.Center);
                SpawnNewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16, NPCType<OnyxKinsman>());
                DestroyTile(i, j);
            }
            if (type == TileType<CodebreakerTile>() && Main.LocalPlayer.HeldItem.type == ItemType<BloodyVein>() && NPC.CountNPCS(NPCType<HypnosDraedon>()) <= 0)
            {
                SoundEngine.PlaySound(CalamityMod.UI.DraedonSummoning.CodebreakerUI.BloodSound, Main.LocalPlayer.Center);
                SpawnNewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)player.Center.X, (int)(player.Center.Y - 1200), NPCType<HypnosDraedon>());
            }
            if (TileID.Sets.CountsAsWaterSource[type] && Main.LocalPlayer.HeldItem.type == ItemType<BloodyVein>() && !PandemicPanic.IsActive)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)RemixMessageType.StartPandemicPanic);
                    packet.Send();
                }
                else
                {
                    PandemicPanic.StartEvent();
                }
                CalRemixWorld.UpdateWorldBool();
            }
        }
        public override void MouseOver(int i, int j, int type)
        {
            Player player = Main.LocalPlayer;
            if (type == TileType<LabHologramProjector>() || TileID.Sets.CountsAsWaterSource[type])
            {
                if (player.HeldItem.type == ItemType<BloodyVein>())
                {
                    player.cursorItemIconID = ItemType<BloodyVein>();
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

                            if (tileCount.TileType == TileType<MiracleFruitPlaced>())
                                berryCount++;
                        }
                    }

                    if (berryCount < 6 && j < 100 && Main.rand.NextBool(48) && NPC.downedGolemBoss)
                    {
                        if (exclusionlist.Contains(Main.tile[i, j - 1].TileType) && Main.tile[i, j].Slope == 0 && !Main.tile[i, j].IsHalfBlock)
                        {
                            if (exclusionlist.Contains(Main.tile[i + 1, j - 1].TileType) && Main.tile[i, j].Slope == 0 && !Main.tile[i, j].IsHalfBlock)
                            {
                                WorldGen.PlaceObject(i, j - 1, TileType<MiracleFruitPlaced>(), true);
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

                        if (tileCount.TileType == TileType<CosmichidPlant>())
                            cosmicCount++;
                    }
                }

                if (cosmicCount < 10 && j < 100 && Main.rand.NextBool(48) && Main.hardMode)
                {
                    if (exclusionlist.Contains(Main.tile[i, j - 1].TileType) && Main.tile[i, j].Slope == 0 && !Main.tile[i, j].IsHalfBlock)
                    {
                        WorldGen.PlaceObject(i, j - 1, TileType<CosmichidPlant>(), true);
                    }
                }
            }

            if (CalRemix.oreList.Contains(type) && Main.rand.NextBool(2222) && RemixDowned.downedExcavator)
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
                        if (AstralShrine.astrallist.Contains(tile.TileType))
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
                                    if (Main.tile[x, y].TileType == TileType<MeldGunkPlaced>())
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
                                    Main.tile[i, j].TileType = (ushort)TileType<MeldGunkPlaced>();
                                    WorldGen.SquareTileFrame(i, j, true);
                                }
                            }
                        }
                    }
                }
            }
            if (Main.rand.NextBool(500))
            {
                if (SubworldSystem.IsActive<SealedSubworld>())
                {
                    if (tile.TileType == TileID.Beds || TileID.Sets.CanBeSleptIn[tile.TileType])
                    {
                        bool conveyor = false;
                        for (int l = i - 5; l < i + 5; l++)
                        {
                            if (TileID.Sets.ConveyorDirection[Main.tile[l, j + 1].TileType] != 0)
                            {
                                int p = Item.NewItem(new EntitySource_TileUpdate(i, j), new Vector2(l, j) * 16, ModContent.ItemType<BabySealedPuppet>());
                                Main.item[p].velocity = Vector2.Zero;
                                conveyor = true;
                                break;
                            }
                        }
                        if (!conveyor)
                        {
                            Tile above = Main.tile[i, j - 1];
                            if (!above.HasTile && tile2.HasTile)
                            {
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<BabySealedPuppetPlaced>(), true);
                            }
                        }
                    }
                }
            }
        }

        public override void NearbyEffects(int i, int j, int type, bool closer)
        {
            Tile t = Main.tile[i, j];
            if (type == IonCubePlaced && t.TileFrameX == 0 && t.TileFrameY == 0)
            {
                IonCubeTE cube = CalamityUtils.FindTileEntity<IonCubeTE>(i, j, 1, 1);
                if (cube == null)
                {
                    TileEntity.PlaceEntityNet(i, j, TileEntityType<IonCubeTE>());
                }
            }
            if (!Main.dedServ)
            {
                if (ScreenHelperManager.screenHelpersEnabled)
                {
                    if (!roxm.alreadySeen)
                    {
                        if (type == RoxTile)
                        {
                            roxm.ActivateMessage();
                        }
                    }
                    if (!KinsmanMessage.alreadySeen)
                    {
                        if (type == OnyxExcavatorTile && RemixDowned.downedEarthElemental)
                        {
                            Player player = Main.LocalPlayer;
                            bool e = player.HasItem(ItemType<EyeoftheStorm>()) || player.HasItem(ItemType<WifeinaBottle>()) || player.HasItem(ItemType<WifeinaBottlewithBoobs>()) || player.HasItem(ItemType<EyeoftheStorm>()) || player.HasItem(ItemType<PearlofEnthrallment>()) || player.HasItem(ItemType<InfectedRemote>());
                            if (e)
                                KinsmanMessage.ActivateMessage();
                        }
                    }
                    if (!GrimeMessage.alreadySeen)
                    {
                        if (type == GrimesandPlaced)
                        {
                            GrimeMessage.ActivateMessage();
                        }
                    }
                }
                if (CalRemixWorld.reargar)
                {
                    if (type == UelibloomOre)
                    {
                        Main.tile[i, j].TileType = (ushort)TileID.Mud;
                    }
                }
            }
            // Exosphere portal
            // Since the subworld isn't done right now, it just tells you you're banned then kicks you out
            if (t.TileType == RustedPipes)
            {
                if (!Main.tile[i + 1, j + 1].HasTile)
                {
                    bool travelPossible = true;
                    for (int k = i; k < i + 4; k++)
                    {
                        for (int l = j; l < j + 5; l++)
                        {
                            if (k == i || k == i + 3 || l == j || l == j + 4)
                            {
                                if (Main.tile[k, l].TileType != RustedPipes)
                                    travelPossible = false;
                            }
                        }
                    }
                    if (travelPossible)
                    {
                        Point tCord = Main.LocalPlayer.Center.ToTileCoordinates();
                        Rectangle portalRect = new Rectangle(i + 1, j + 1, 2, 3);
                        if (portalRect.Contains(tCord))
                        {
                            SubworldSystem.Enter<ExosphereSubworld>();
                        }
                    }
                }
            }
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (type == TileType<CryonicBrick>())
            {
                return Main.hardMode;
            }
            return true;
        }

        public override bool CanExplode(int i, int j, int type)
        {
            if (type == TileType<CryonicBrick>())
            {
                return Main.hardMode;
            }
            return true;
        }

        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            if (type == TileType<CryonicBrick>())
            {
                return Main.hardMode;
            }
            return true;
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!Main.dedServ)
            {
                if (ScreenHelperManager.screenHelpersEnabled)
                {
                    if (ModLoader.TryGetMod("OreExcavator", out _))
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
                if (!CalRemixWorld.generatedGrime)
                {
                    if (type == TileID.ShadowOrbs)
                    {
                        CalamityMod.CalamityUtils.SpawnOre(TileType<GrimesandPlaced>(), 6E-04, 0, 0.05f + WorldGen.GetWorldSize() * 0.05f, 5, 10, TileID.Dirt, TileID.Mud, TileID.Cloud, TileID.RainCloud);                       
                        
                        CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.GrimeTheSkies", Color.Brown);
                        CalRemixWorld.generatedGrime = true;
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
                        if (Main.rand.NextBool(60)) // roughly 10% when considering the piles are 6 tiles in size
                        {
                            SpawnNewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCType<GulletWorm>());
                        }
                    }
                }
            }
            Player player = Main.LocalPlayer;
            if (player.ZoneJungle && !NPC.AnyNPCs(NPCType<Phytogen>()))
            {
                 if (!effectOnly && !fail && TileID.Sets.IsShakeable[type] && WorldGen.genRand.NextBool(22))
                 {
                     CalamityGlobalTile.GetTreeBottom(i, j, out int treeX, out int treeY);
                     TreeTypes treeType = WorldGen.GetTreeType(Main.tile[treeX, treeY].TileType);
                     if (treeType != TreeTypes.None)
                     {
                         treeY--;
                         while (treeY > 10 && Main.tile[treeX, treeY].HasTile && TileID.Sets.IsShakeable[Main.tile[treeX, treeY].TileType])
                             treeY--;

                         treeY++;

                         if (WorldGen.IsTileALeafyTreeTop(treeX, treeY) && !Collision.SolidTiles(treeX - 2, treeX + 2, treeY - 2, treeY + 2))
                         {
                            SpawnNPCOnPlayer(Main.LocalPlayer.whoAmI, NPCType<Phytogen>());
                         }
                     }
                 }
            }
            if (!fail && !effectOnly)
            {
                if (type == TileType<Navystone>())
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemType<CalamityMod.Items.Placeables.Navystone>());
                }
                if (type == TileType<EutrophicSand>())
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemType<CalamityMod.Items.Placeables.EutrophicSand>());
                }
                if (type == TileType<HardenedEutrophicSand>())
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemType<CalamityMod.Items.Placeables.HardenedEutrophicSand>());
                }
                if (type == TileType<SeaPrism>())
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemType<CalamityMod.Items.Placeables.SeaPrism>());
                }
            }
            if (type == TileID.Chandeliers && Main.tile[i, j].TileFrameX == 72 && Main.tile[i, j].TileFrameY % 54 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, (j + 1) * 16 + 8), Vector2.Zero, ProjectileType<FallingChandelier>(), ProjectileDamage(20, 40), 1, ai0: Main.tile[i, j].TileFrameY);
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
            if (tile.TileType != TileType<EutrophicGlass>())
            {
                Rectangle value = new Rectangle(num, num2, 16, 2);
                Main.spriteBatch.Draw(texture, position3, value, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}