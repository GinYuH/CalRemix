using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ObjectData;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using MonoMod.RuntimeDetour;
using System.Reflection;
using CalamityMod.World;
using Terraria.WorldBuilding;
using System.IO;
using Terraria.Modules;
using System.Threading;
using CalRemix.UI;
using System.Linq;

namespace CalRemix.Core.World
{
    public class HallOfLegends : ILoadable
    {
        public static int[] portraitOrder = [8 /*Ettel*/,    5 /*Conyroy*/,  2 /*Daslnew*/,  3 /*Perez*/,  1 /*Mitchard*/, 10 /*Dirac*/,
                                             9 /*Wingert*/,  11 /*Huet*/,    0 /*Hayabusa*/, 4 /*Ackner*/, 6 /*Marino*/,   7 /*Hoob*/,
                                             14 /*Moretti*/, 13 /*Falafel*/, 12 /*YOU*/];
        public static int portraitTileType;
        public static int[] portraitItemTypes;
        public static string[] portraitHoverTexts;

        private static Hook CalamityGenerateHiveHook;
        public delegate bool orig_CanPlaceGiantHive(Point origin, StructureMap structures);
        public delegate bool hook_CanPlaceGiantHive(orig_CanPlaceGiantHive orig, Point origin, StructureMap structures);

        public static FannyMood gratefulMood;
        public static FannyMood mournfulMood;
        public static HelperMessage yuhDead;

        public void Load(Mod mod)
        {
            portraitItemTypes = new int[15];
            portraitHoverTexts = new string[15];

            AutoloadedLegendPortrait tile = new AutoloadedLegendPortrait();
            mod.AddContent(tile);
            portraitTileType = tile.Type;

            LoadPortrait(mod, "HAYABUSA", "Voice of the Miracle Boy");
            LoadPortrait(mod, "MITCHARD", "Technical Director, UX Designer");
            LoadPortrait(mod, "DASLNEW", "Founder, Lead Designer");
            LoadPortrait(mod, "PEREZ", "Voice of Fanny");
            LoadPortrait(mod, "ACKNER", "Voice of Evil Fanny and Trapper Bulb Chan");
            LoadPortrait(mod, "CONROY", "Graphics Lead, Story Lead");
            LoadPortrait(mod, "MARINO", "3D Assets, Saxophone Backing for \"Artistic Reinforcment\"");
            LoadPortrait(mod, "HOOB", "Sound Effects, Additional Graphics");
            LoadPortrait(mod, "ETTEL", "Lead Musician");
            LoadPortrait(mod, "WINGERT", "Voice of Wonder Flower");
            LoadPortrait(mod, "DIRAC", "Test Lead, European Localization");
            LoadPortrait(mod, "HUET", "Voice of the Crim Son");
            LoadPortrait(mod, "YOU", "For your continous love and support!");
            LoadPortrait(mod, "FALAFEL", "A very nice cat");
            LoadPortrait(mod, "MORETTI", "Voice of the Liminal Critic");

            CalamityGenerateHiveHook = null;
            MethodInfo giantHiveMethodInfo = typeof(GiantHive).GetMethod("CanPlaceGiantHive", BindingFlags.Public | BindingFlags.Static);
            if (giantHiveMethodInfo is not null)
            {
                CalamityGenerateHiveHook = new Hook(giantHiveMethodInfo, (hook_CanPlaceGiantHive)InterceptGiantHivePosition);
            }

            if (!Main.dedServ)
            {
                gratefulMood = FannyMood.New("Grateful!", null, MoodTracker.PriorityClass.OnscreenNPCsMood, 1f, false, Color.OrangeRed, Color.Black).DisableNaturalSelection();
                mournfulMood = FannyMood.New("Mournful...", null, 10f, 1f, false, Color.Gray, Color.Black).DisableNaturalSelection();
            }
        }


        public void Unload()
        {
            CalamityGenerateHiveHook?.Undo();
        }

        public static Point hallPosition;

        private bool InterceptGiantHivePosition(orig_CanPlaceGiantHive orig, Point origin, StructureMap structures)
        {
            hallPosition = Point.Zero;

            if (!orig(origin, structures))
                return false;

            //Save hall position
            hallPosition = (Point)origin;
            hallPosition.Y -= 45;

            int zoneXMin = Math.Max(0, origin.X - 60);
            int zoneYMin = Math.Max(0, origin.Y - 60);
            int zoneXMax = Math.Min(Main.maxTilesX -1, origin.X + 60);
            int zoneYMax = Math.Min(Main.maxTilesY - 1, origin.Y + 260);

            //remove all loot for good measure
            for (int i = zoneXMin; i < zoneXMax; i++)
                for (int j = zoneYMin; j < zoneYMax; j++)
                {
                    Tile t = Main.tile[i, j];
                    if (t.HasTile && t.TileType == TileID.Larva)
                        t.HasTile = false;
                }


            zoneXMin -= 20;
            zoneXMax += 20;
            zoneYMax += 100;


            for (int i = 0; i < Main.maxChests; i++)
            {
                Chest chest = Main.chest[i];
                if (chest is null)
                    continue;

                if (chest.x >= zoneXMin && chest.x <= zoneXMax && chest.y >= zoneYMin && chest.y <= zoneYMax)
                {
                    //Honey chest
                    if (Main.tile[chest.x, chest.y].TileFrameX == 1044)
                    {
                        for (int j = 0; j < Chest.maxItems; j++)
                        {
                            chest.item[j].type = ItemID.None;
                        }

                        chest.item[0].SetDefaults(portraitItemTypes[Main.rand.Next(portraitItemTypes.Length)]);
                        chest.item[0].stack = 1;
                    }
                }
            }

            return true;
        }

        public const int floorHeight = 26;
        public const int portraitDisplayWidth = 11;
        public const int sideroomWidth = 7;
        public const int wallThickness = 5;
        public const int portraitsPerFloor = 6;
        public const int totalFloorWidth = (wallThickness + sideroomWidth) * 2 + portraitDisplayWidth * portraitsPerFloor + 1;

        public static void GenerateHallOfLegends()
        {
            if (hallPosition == Point.Zero) 
                return;

            int portraitIndex = 0;
            GenerateFloor(hallPosition.X, hallPosition.Y, ref portraitIndex);
            hallPosition.Y += 26;
            GenerateFloor(hallPosition.X, hallPosition.Y, ref portraitIndex);
            CarveStairway(hallPosition.X, hallPosition.Y, false);

            hallPosition.Y += 26;
            GenerateFloor(hallPosition.X, hallPosition.Y, ref portraitIndex);
            CarveStairway(hallPosition.X, hallPosition.Y, true);
        }

        public static void GenerateFloor(int floorCenterX, int floorTopY, ref int portraitIndex)
        {
            int startX = floorCenterX - totalFloorWidth / 2;
            int endX = startX + totalFloorWidth;
            int endY = floorTopY + floorHeight;

            for (int i = startX; i < endX; i++)
            {
                for (int j = floorTopY; j < endY; j++)
                {
                    Tile t = Main.tile[i, j];

                    //Edges are is always wallless
                    if (j > floorTopY && j < endY - 1 && i > startX && i < endX - 1)
                    {
                        ushort wallType = WallID.DiamondGemsparkOff;
                        if ((j > floorTopY + 2 && j < floorTopY + 10) || (j > floorTopY + 10 && j < floorTopY + 19))
                            wallType = WallID.LivingWood;

                        t.WallType = wallType;
                    }

                    t.Slope = SlopeType.Solid;
                    t.IsHalfBlock = false;
                    t.IsActuated = false;
                    t.HasTile = true;
                    t.LiquidAmount = 0; //In case theres honey in the way

                    if (j < floorTopY + 2 || j > floorTopY + 20)
                        t.TileType = TileID.Titanstone;

                    if (j == floorTopY + 20)
                        t.TileType = TileID.Asphalt;

                    //Carve the insides
                    if (j >= floorTopY + 2 && j < floorTopY + 20)
                        t.HasTile = false;
                }
            }


            //Place walls
            for (int i = startX; i < startX + wallThickness; i++)
            {
                for (int j = floorTopY; j < endY; j++)
                {
                    Tile t = Main.tile[i, j];
                    t.Slope = SlopeType.Solid;
                    t.IsHalfBlock = false;
                    t.IsActuated = false;
                    t.HasTile = true;
                    t.TileType = TileID.Titanstone;
                }
            }
            for (int i = endX - wallThickness; i < endX; i++)
            {
                for (int j = floorTopY; j < endY; j++)
                {
                    Tile t = Main.tile[i, j];
                    t.Slope = SlopeType.Solid;
                    t.IsHalfBlock = false;
                    t.IsActuated = false;
                    t.HasTile = true;
                    t.TileType = TileID.Titanstone;
                }
            }

            //Place side rooms
            for (int i = startX + wallThickness; i < startX + wallThickness + sideroomWidth - 1; i++)
            {
                for (int j = floorTopY + 1; j < endY - 1; j++)
                {
                    Tile t = Main.tile[i, j];
                    if (i == startX + wallThickness) //Wood trim along edge
                        t.WallType = WallID.Wood;
                    else
                    {
                        t.TileType = TileID.DiamondGemspark; //Diamond lights on top, and then cut;
                        break;
                    }
                }
            }
            for (int i = endX - wallThickness - sideroomWidth + 1; i < endX - wallThickness; i++)
            {
                for (int j = floorTopY + 1; j < endY - 1; j++)
                {
                    Tile t = Main.tile[i, j];
                    if (i == endX - wallThickness - 1) //Wood trim along edge
                        t.WallType = WallID.Wood;
                    else
                    {
                        t.TileType = TileID.DiamondGemspark; //Diamond lights on top, and then cut;
                        break;
                    }
                }
            }

            //Trim between sideroom and the last portrait
            for (int j = floorTopY + 1; j < endY - 1; j++)
            {
                Tile t = Main.tile[endX - wallThickness - sideroomWidth - 1, j];
                t.WallType = WallID.Wood;

                if (j == floorTopY + 19)
                {
                    WorldGen.Place1x1(endX - wallThickness - sideroomWidth - 1, j, TileID.Torches, 6); //Place torch at the bottom
                    t.IsTileInvisible = true;
                }
            }

            //Place portraits
            int portraitRoomX = startX + wallThickness + sideroomWidth;
            for (int p = 0; p < portraitsPerFloor; p++)
            {
                for (int i = portraitRoomX; i < portraitRoomX + portraitDisplayWidth; i++)
                {
                    for (int j = floorTopY + 1; j < endY - 1; j++)
                    {
                        Tile t = Main.tile[i, j];

                        //Trim to the left of the room
                        if (i == portraitRoomX)
                        {
                            //No trim for the last floor where theres the double width section
                            if (portraitIndex <= 14 || p < 5 )
                                t.WallType = WallID.Wood;

                            if (j == floorTopY + 19)
                            {
                                WorldGen.Place1x1(i, j, TileID.Torches, 6); //Place torch at the bottom
                                t.IsTileInvisible = true;
                            }
                        }
                        else 
                        {
                            if (i > portraitRoomX + 1 && i < portraitRoomX + portraitDisplayWidth - 1)
                            {
                                if (j == floorTopY + 1)
                                    t.TileType = TileID.DiamondGemspark; //Gemspark ceiling lamps
                                else if (j >= floorTopY + 6 && j <= floorTopY + 15 && portraitIndex <= 14)
                                    t.WallType = WallID.AmberGemspark;   //Gemspark walls behind painting
                                else if (j >= floorTopY + 5 && j <= floorTopY + 17 && portraitIndex <= 14)
                                    t.WallType = WallID.TitanstoneBlock; //Titanstone trim around painting

                            }
                        }
                    }
                
                
                }

                if (portraitIndex <= 14)
                {
                    //Place portrait
                    int portrait = portraitOrder[portraitIndex]; //Find the order
                    WorldGen.PlaceTile(portraitRoomX + 6, floorTopY + 11, portraitTileType, style: portrait);
                    portraitIndex++;
                }
                //merge the ceiling lamps on the last room of the final floor, and place the big portrait
                else if (p == 5)
                {
                    for (int i = portraitRoomX - 1; i < portraitRoomX + 2; i++)
                    {
                        Tile t = Main.tile[i, floorTopY + 1];
                        t.TileType = TileID.DiamondGemspark; //Gemspark ceiling lamps
                    }


                    for (int i = portraitRoomX - portraitDisplayWidth + 2 ; i < portraitRoomX + portraitDisplayWidth - 1; i++)
                    {
                        for (int j = floorTopY + 1; j < endY - 1; j++)
                        {
                            Tile t = Main.tile[i, j];
                            if (j >= floorTopY + 6 && j <= floorTopY + 15)
                                t.WallType = WallID.AmberGemspark;   //Gemspark walls behind painting
                            else if (j >= floorTopY + 5 && j <= floorTopY + 17 )
                                t.WallType = WallID.TitanstoneBlock; //Titanstone trim around painting
                        }
                    }

                    WorldGen.PlaceTile(portraitRoomX, floorTopY + 11, ModContent.TileType<LegendPromo>());
                }

                portraitRoomX += portraitDisplayWidth;
            }

            for (int i = startX; i < endX; i++)
            {
                for (int j = floorTopY; j < endY; j++)
                {
                    WorldGen.TileFrame(i, j, true, true);
                    WorldGen.SquareWallFrame(i, j);
                }
            }
        }

        public static void CarveStairway(int floorCenterX, int floorTopY, bool left = true)
        {
            int startX = floorCenterX - totalFloorWidth / 2;
            int endY = floorTopY + 20;
            floorTopY -= (floorHeight - 20);


            if (!left)
                startX += totalFloorWidth - wallThickness - sideroomWidth + 1;
            else
                startX += wallThickness + 1;
            int endX = startX + 5;


            for (int i = startX; i < endX; i++)
            {
                //Carve the way
                for (int j = floorTopY - 1; j < endY - 1; j++)
                {
                    Tile t = Main.tile[i, j];
                    t.WallType = WallID.LivingWood;
                    t.HasTile = false;
                }

                //Place a platform at the start
                Tile t2 = Main.tile[i, floorTopY];
                t2.HasTile = true;
                t2.TileType = TileID.TeamBlockWhitePlatform;
                t2.TileFrameY = 0;
                t2.TileFrameX = (short)(i == startX ? 54 : i == endX - 1 ? 72 : 0);
            }

            int staircaseZigZagCount = 5;
            int staircaseY = floorTopY + 1;
            for (int s = 0; s < staircaseZigZagCount; s++)
            {
                for (int i = 0; i < 5; i++)
                {
                    bool oddDir = s % 2 == 0;
                    if (!left)
                        oddDir = !oddDir;
                    int x = oddDir ? startX + i : endX - 1 - i;
                    Tile t2 = Main.tile[x, staircaseY + i];
                    t2.HasTile = true;
                    t2.TileType = TileID.TeamBlockWhitePlatform;
                    t2.Slope = oddDir ? SlopeType.SlopeDownLeft : SlopeType.SlopeDownRight;
                    t2.TileFrameX = (short)(oddDir ? 144 : 180);
                    t2.TileFrameY = 0;
                }

                staircaseY += 5;
            }

            //Final framing
            for (int i = startX - 1; i < endX + 1; i++)
                for (int j = floorTopY - 1; j < endY + 1; j++)
                {
                    WorldGen.TileFrame(i, j);
                    WorldGen.SquareWallFrame(i, j);
                }
        }

        public static int portraitIndex = 0;
        public static void LoadPortrait(Mod mod, string name, string hoverTooltip)
        {
            //Load the relic item but cache it first
            AutoloadedLegendPortraitItem item = new AutoloadedLegendPortraitItem(name, hoverTooltip, portraitIndex);
            mod.AddContent(item);

            //Load the tile using the item's tile type

            //Set the portrait item's type to be the one of the portrait tile (so it can properly place it)
            item.TileType = portraitTileType;

            portraitItemTypes[portraitIndex] = item.Type;
            portraitHoverTexts[portraitIndex] = hoverTooltip;
            portraitIndex++;
        }
    }

    [Autoload(false)]
    public class AutoloadedLegendPortrait : ModTile
    {
        public override string Texture => "Calremix/Assets/ExtraTextures/Legends";
        public override string Name => InternalName != "" ? InternalName : base.Name;

        public string InternalName;
        protected readonly string TexturePath;

        public static Texture2D OpenPicture;
        public static bool loadingImage;

        public AutoloadedLegendPortrait()
        {
            InternalName = "LegendPortrait";
        }

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 12;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(4, 5);
            TileObjectData.newTile.StyleWrapLimit = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(160, 165, 160), name);
            DustType = DustID.GemDiamond;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            drawData.tileLight = Color.White;

            if (drawData.tileFrameX == 0 && drawData.tileFrameY == 432 && CalRemixWorld.savedAPicture)
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(new Point(i, j));
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            // This is lighting-mode specific, always include this if you draw tiles manually
            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
            {
                offScreen = Vector2.Zero;
            }

            Vector2 position = new Vector2(i, j) * 16 + new Vector2(7, 6) + offScreen - Main.screenPosition;


            if (loadingImage)
            {
                Texture2D loadingTex = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/LegendLoading").Value;
                spriteBatch.Draw(loadingTex, position, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                return;
            }

            Texture2D tex = OpenPicture;
            if (tex is null)
            {
                OpenSavedPicture();
                return;
            }

            Vector2 frameSize = new Vector2(114, 147);
            Vector2 scale = new Vector2(frameSize.Y / (float)tex.Height);

            int ingameWidth = (int)(tex.Width * scale.X);           //How wide the image would be after resizing it to fill the space on the Y axis
            float cropXPercent = frameSize.X / (float)ingameWidth;   //Percent taken between the total available space on the frame vs the full resized width

            int frameWidth = (int)(tex.Width * cropXPercent);

            Rectangle frame = new Rectangle(tex.Width / 2 - frameWidth / 2, (int)(tex.Width / frameSize.X), frameWidth, tex.Height);
            spriteBatch.Draw(tex, position, frame, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (Main.tile[i, j].WallType == WallID.AmberGemspark || Main.tile[i, j].WallType == WallID.TitanstoneBlock)
                return false;
            return true;
        }

        public override void MouseOver(int i, int j) => Hovering(i, j);
        public override void MouseOverFar(int i, int j) => Hovering(i, j, true);
        public static void Hovering(int i, int j, bool noImageIcon = false)
        {
            Player player = Main.LocalPlayer;

            Tile t = Main.tile[i, j];
            int portraitIndex = t.TileFrameX / (18 * 8) + t.TileFrameY / (18 * 12) * 6;
            player.cursorItemIconText = HallOfLegends.portraitHoverTexts[portraitIndex];
            player.cursorItemIconID = -1;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;

            if (!loadingImage && !CalRemixWorld.savedAPicture && portraitIndex == 12)
                player.cursorItemIconText = "Right click to choose a picture and join the family!";
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile t = Main.tile[i, j];
            int portraitIndex = t.TileFrameX / (18 * 8) + t.TileFrameY / (18 * 12) * 6;
            if (closer && HallOfLegends.gratefulMood != null)
                HallOfLegends.gratefulMood.Activate();
        }

        public override bool RightClick(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int portraitIndex = t.TileFrameX / (18 * 8) + t.TileFrameY / (18 * 12) * 6;

            if (portraitIndex != 12 || loadingImage)
                return false;

            loadingImage = false;
            string savePath = $"{Main.SavePath}/RemixPictureSave";
            string saveName = $"/YourPrettyPicture_{Main.worldID}";

            if (FileOpenUtils.OpenImage(OnImageOpened, savePath, saveName))
                loadingImage = true;
            return true;
        }

        public static void OnImageOpened(Texture2D pic)
        {
            loadingImage = false;

            if (pic == null)
            {
                OpenPicture = null;
                CalRemixWorld.savedAPicture = false;
            }    

            OpenPicture = pic;
            CalRemixWorld.savedAPicture = true;
        }

        public static void OpenSavedPicture()
        {
            string savePath = $"{Main.SavePath}/RemixPictureSave";
            string saveName = $"/YourPrettyPicture_{Main.worldID}.png";

            Main.QueueMainThreadAction(() => FileOpenUtils.LoadTexture(savePath + saveName, OnImageOpened));
            loadingImage = true;
        }
    }



    [Autoload(false)]
    public class AutoloadedLegendPortraitItem : ModItem
    {
        public string InternalName = "";
        public string Itemname;
        public string Itemtooltip;
        public int TileType;
        protected int index;

        protected override bool CloneNewInstances => true;

        public override string Name => InternalName != "" ? InternalName : base.Name;
        public override string Texture => $"CalRemix/Assets/ExtraTextures/LegendPortrait{index}";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Itemname ?? "ERROR");
            Tooltip.SetDefault(Itemtooltip ?? "ERROR");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType);
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Red;
            Item.master = true;
            Item.value = Item.sellPrice(0, 1);
            Item.placeStyle = index;
        }

        public AutoloadedLegendPortraitItem(string name, string tooltip, int index)
        {
            InternalName = "LegendPortrait" + name + "Item";
            Itemname = $"Portrait of a Legend - ({name})";
            this.index = index;
            Itemtooltip = tooltip;
        }
    }

    public class LegendPromo : ModTile
    {
        public override string Texture => "Calremix/Assets/ExtraTextures/LegendPromo";

        public static bool hoveredOverPromo = false;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 17;
            TileObjectData.newTile.Height = 12;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(8, 5);
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(160, 165, 160), name);
            DustType = DustID.GemDiamond;

            if (Main.dedServ)
                return;
            HallOfLegends.yuhDead = HelperMessage.New("YuhIsDead",
           "Offer Update: Marvin \"GinYuh\" Dalsney died of an ailment in 2018 (aged 32) and is no longer able to visit those who beat the mod.",
           "FannySob", null).NeedsActivation().AddDelay(3f);

            hoveredOverPromo = false;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            drawData.tileLight = Color.White;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (Main.tile[i, j].WallType == WallID.AmberGemspark || Main.tile[i, j].WallType == WallID.TitanstoneBlock)
                return false;
            return true;
        }

        public override void MouseOver(int i, int j) => Hovering(i, j);
        public override void MouseOverFar(int i, int j) => Hovering(i, j, true);
        public static void Hovering(int i, int j, bool far = false)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconText = "Must be over 21 years old to be eligible.\n\nOffer only appliable to US residents.";
            player.cursorItemIconID = -1;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;

            if (!far)
            {
                hoveredOverPromo = true;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.dedServ)
                return;

            bool informedOfYuhDeath = HallOfLegends.yuhDead.alreadySeen;

            if (closer && HallOfLegends.mournfulMood != null && informedOfYuhDeath)
                HallOfLegends.mournfulMood.Activate();

            else if (!informedOfYuhDeath && hoveredOverPromo)
            {
                Vector2 position = new Vector2(i, j) * 16;
                if (Main.LocalPlayer.Center.Y < position.Y + 100 && Main.LocalPlayer.Center.Y >= position.Y)
                {
                    if (Math.Abs(Main.LocalPlayer.Center.X - position.X) < 300)
                    {
                        HallOfLegends.yuhDead.ActivateMessage();
                    }
                }
            }
        }
    }

    public class LegendPromoItem : ModItem
    {
        public override string Texture => $"CalRemix/Assets/ExtraTextures/LegendPromoItem";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Promotional Poster");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<LegendPromo>());
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Red;
            Item.master = true;
            Item.value = Item.sellPrice(0, 1);
        }
    }
}