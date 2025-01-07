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

namespace CalRemix.Core.World
{
    public class HallOfLegends : ILoadable
    {
        public static int[] portraitOrder = [8 /*Ettel*/,  5 /*Conyroy*/, 2 /*Daslnew*/, 3 /*Perez*/,    1 /*Mitchard*/, 10 /*Dirac*/,
                                             6 /*Marino*/, 7 /*Hoob*/,    4 /*Ackner*/,  0 /*Hayabusa*/, 11 /*Huet*/,    9 /*Wingert*/];
        public static int[] portraitTileTypes;
        public static int[] portraitItemTypes;

        private static Hook CalamityGenerateHiveHook;
        public delegate bool orig_CanPlaceGiantHive(Point origin, StructureMap structures);
        public delegate bool hook_CanPlaceGiantHive(orig_CanPlaceGiantHive orig, Point origin, StructureMap structures);


        public void Load(Mod mod)
        {
            portraitTileTypes = new int[12];
            portraitItemTypes = new int[12];

            LoadPortrait(mod, "HAYABUSA", "Voice of the Miracle Boy");
            LoadPortrait(mod, "MITCHARD", "Technical Director and UX Tester");
            LoadPortrait(mod, "DASLNEW", "Lead Designer");
            LoadPortrait(mod, "PEREZ", "Voice of Fanny");
            LoadPortrait(mod, "ACKNER", "Voice of Evil Fanny and Trapper Bulb Chan");
            LoadPortrait(mod, "CONROY", "Graphics Lead");
            LoadPortrait(mod, "MARINO", "3D Assets and Saxophone for \"Generator\" Music");
            LoadPortrait(mod, "HOOB", "Sound Effects and Additional Graphics");
            LoadPortrait(mod, "ETTEL", "Lead Musician");
            LoadPortrait(mod, "WINGERT", "Voice of Wonder Flower");
            LoadPortrait(mod, "DIRAC", "Test Lead and European Localization");
            LoadPortrait(mod, "HUET", "Voice of the Crim Son");

            CalamityGenerateHiveHook = null;
            MethodInfo giantHiveMethodInfo = typeof(GiantHive).GetMethod("CanPlaceGiantHive", BindingFlags.Public | BindingFlags.Static);
            if (giantHiveMethodInfo is not null)
            {
                CalamityGenerateHiveHook = new Hook(giantHiveMethodInfo, (hook_CanPlaceGiantHive)InterceptGiantHivePosition);
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
            int zoneYMax = Math.Min(Main.maxTilesY - 1, origin.Y + 60);

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

        public static void GenerateHallOfLegends()
        {
            if (hallPosition == Point.Zero) 
                return;

            int portraitIndex = 0;
            GenerateFloor(hallPosition.X, hallPosition.Y, ref portraitIndex);
            hallPosition.Y += 24;
            GenerateFloor(hallPosition.X, hallPosition.Y, ref portraitIndex);
            hallPosition.Y += 24;
            GenerateFloor(hallPosition.X, hallPosition.Y, ref portraitIndex, true);

        }

        public static void GenerateFloor(int floorCenterX, int floorTopY, ref int portraitIndex, bool emptyPortraits = false)
        {
            int floorHeight = 24;
            int portraitDisplayWidth = 11;
            int sideroomWidth = 7;
            int wallThickness = 5;

            int portraitsPerFloor = 6;

            int totalFloorWidth = (wallThickness + sideroomWidth) * 2 + portraitDisplayWidth * portraitsPerFloor + 1;
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
                Tile t = Main.tile[endX - wallThickness - sideroomWidth, j];
                t.WallType = WallID.Wood;

                if (j == floorTopY + 19)
                {
                    WorldGen.Place1x1(endX - wallThickness - sideroomWidth, j, TileID.Torches, 6); //Place torch at the bottom
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
                            t.WallType = WallID.Wood;
                            if (j == floorTopY + 19)
                            {
                                WorldGen.Place1x1(i, j, TileID.Torches, 6); //Place torch at the bottom
                                t.IsTileInvisible = true;
                            }
                        }
                        //Main room
                        else if (!emptyPortraits)
                        {
                            if (i > portraitRoomX + 1 && i < portraitRoomX + portraitDisplayWidth - 1)
                            {
                                if (j == floorTopY + 1)
                                    t.TileType = TileID.DiamondGemspark; //Gemspark ceiling lamps
                                else if (j >= floorTopY + 6 && j <= floorTopY + 15)
                                    t.WallType = WallID.AmberGemspark;   //Gemspark walls behind painting
                                else if (j >= floorTopY + 5 && j <= floorTopY + 17)
                                    t.WallType = WallID.TitanstoneBlock; //Titanstone trim around painting

                            }
                        }
                    }
                }

                if (!emptyPortraits)
                {
                    //Place portrait
                    int portrait = portraitOrder[portraitIndex]; //Find the order
                    int portraitType = portraitTileTypes[portrait]; //Get the tile based on its index
                    WorldGen.PlaceTile(portraitRoomX + 6, floorTopY + 11, portraitType);
                    portraitIndex++;
                }

                portraitRoomX += portraitDisplayWidth;
            }

            for (int i = startX; i < endX; i++)
            {
                for (int j = floorTopY; j < endY; j++)
                {
                    WorldGen.TileFrame(i, j, true, true);
                }
            }
        }


        public static int portraitIndex = 0;
        public static void LoadPortrait(Mod mod, string name, string hoverTooltip)
        {
            //Load the relic item but cache it first
            AutoloadedLegendPortraitItem item = new AutoloadedLegendPortraitItem(name, hoverTooltip, portraitIndex);
            mod.AddContent(item);

            //Load the tile using the item's tile type
            AutoloadedLegendPortrait tile = new AutoloadedLegendPortrait(name, hoverTooltip, item.Type, portraitIndex);
            mod.AddContent(tile);

            //Set the portrait item's type to be the one of the portrait tile (so it can properly place it)
            item.TileType = tile.Type;


            portraitTileTypes[portraitIndex] = tile.Type;
            portraitItemTypes[portraitIndex] = item.Type;
            portraitIndex++;
        }
    }

    [Autoload(false)]
    public class AutoloadedLegendPortrait : ModTile
    {
        public override string Texture => "Calremix/Assets/ExtraTextures/Legends";
        public override string Name => InternalName != "" ? InternalName : base.Name;

        public string InternalName;
        protected readonly int ItemType;
        protected readonly string TexturePath;

        protected string HoverText;
        protected int index;

        public AutoloadedLegendPortrait(string name, string hoverText, int dropType, int index)
        {
            InternalName = "LegendPortrait" + name;
            HoverText = hoverText;
            ItemType = dropType;
            this.index = index;
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
            TileObjectData.addTile(Type);


            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(160, 165, 160), name);
            DustType = DustID.GemDiamond;
        }

        public override void PostTileFrame(int i, int j, int up, int down, int left, int right, int upLeft, int upRight, int downLeft, int downRight)
        {
            Tile t = Main.tile[i, j];
            if (t.TileType != Type)
                return;

            short originalFrameX = t.TileFrameX;
            short originalFrameY = t.TileFrameY;

            t.TileFrameX = (short)(t.TileFrameX % 144);
            t.TileFrameY = (short)(t.TileFrameY % 216);

            t.TileFrameX += (short)((index % 6) * 144);
            if (index >= 6)
                t.TileFrameY += 216;

            if (t.TileFrameX != originalFrameX || t.TileFrameY != originalFrameY)
            {
                PostTileFrame(i, j - 1, 0, 0, 0, 0, 0, 0, 0, 0);
                PostTileFrame(i, j + 1, 0, 0, 0, 0, 0, 0, 0, 0); 
                PostTileFrame(i - 1, j, 0, 0, 0, 0, 0, 0, 0, 0); 
                PostTileFrame(i + 1, j, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            drawData.tileLight = Color.White;
        }


        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (Main.tile[i, j].WallType == WallID.AmberGemspark)
                return false;
            return true;
        }

        public override void MouseOver(int i, int j) => Hovering(i, j);
        public override void MouseOverFar(int i, int j) => Hovering(i, j, true);
        public static void Hovering(int i, int j, bool noImageIcon = false)
        {
            Player player = Main.LocalPlayer;

            player.cursorItemIconText = (ModContent.GetModTile(Main.tile[i, j].TileType) as AutoloadedLegendPortrait).HoverText;
            player.cursorItemIconID = -1;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
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
        }

        public AutoloadedLegendPortraitItem(string name, string tooltip, int index)
        {
            InternalName = "LegendPortrait" + name + "Item";
            Itemname = $"Portrait of a Legend - ({name})";
            this.index = index;
            Itemtooltip = tooltip;
        }
    }
}