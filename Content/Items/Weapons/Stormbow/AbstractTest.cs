using CalamityMod;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using static CalRemix.Content.Items.Weapons.Stormbow.StructureHelpers;
using static Terraria.WorldGen;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class AbstractTest : StormbowAbstract
    {
        //public override bool IsLoadingEnabled(Mod mod) => false;
        public override int damage => 6;
        public override int crit => 4;
        public override float shootSpeed => 12;
        public override int useTime => 50;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.WoodenArrowFriendly };
        public override int arrowAmount => 3;
        public override OverallRarity overallRarity => OverallRarity.White;
        public override bool? UseItem(Player player)
        {
            Vector2 mouseWorld = Main.MouseWorld / 16;
            Point point = new Point((int)mouseWorld.X, (int)mouseWorld.Y);

            Dust.NewDust(Main.MouseWorld, 5, 5, DustID.AmberBolt);
            //WorldGen.digTunnel(mouseWorld.X, mouseWorld.Y, 0, 0, 1, 10);
            //WorldGen.TileRunner((int)mouseWorld.X, (int)mouseWorld.Y, Main.rand.Next(3, 8), Main.rand.Next(2, 8), TileID.CobaltBrick);
            //MakeSolidTunnel(mouseWorld.X, mouseWorld.Y, TileID.Mythril, 0, 0, 1, 10);
            //Main.tile[(int)mouseWorld.X, (int)mouseWorld.Y].TileType = TileID.Mythril;
            //WorldGen.PlaceTile((int)mouseWorld.X, (int)mouseWorld.Y, TileID.Mythril);
            //WorldUtils.Gen(point, new Shapes.Mound(5, 5), new Actions.PlaceTile(TileID.PineTree));
            //int trunkHeightAmount = 10;
            //int individualHeight = 5;
            //MakeChristmasTree(point, trunkHeightAmount, individualHeight);

            //ShapeData shapeData1 = new ShapeData();
            //WorldUtils.Gen(point, new Shapes.Mound(7, 7), Actions.Chain(new Modifiers.Blotches(2, 1, 0.8), new Actions.Blank().Output(shapeData1)));
            //ShapeData shapeData2 = new ShapeData();
            //WorldUtils.Gen(point, new Shapes.Circle(12), Actions.Chain(new Modifiers.NotInShape(shapeData1), new Actions.PlaceTile(TileID.PineTree)));
            //PlaceStupidSpiritModThing(point, GenVars.structures);

            //MakeAwesomeHouseStuff(point);
            //Tile tile = Main.tile[point.X, point.Y];
            //Main.NewText(tile.BlockType);
            //Main.NewText(tile.TileFrameX / 18);
            //Tile tile = Main.tile[point];
            //Main.NewText("1: " + Main.tileFrame[tile.TileType]);
            //Main.NewText("2: " + tile.TileFrameX);

            /*
            // fragmented wall placement
            WorldUtils.Gen(point, new Shapes.Circle(3), Actions.Chain(
                new Modifiers.IsTouching(true, TileID.WoodBlock), 
                new Modifiers.Blotches(3), 
                new Modifiers.Dither(), 
                new Actions.PlaceWall(WallID.Planked)
            ));

            // cobweb placement
            WorldUtils.Gen(point, new Shapes.Circle(4), Actions.Chain(
                new Modifiers.IsTouching(true, TileID.WoodenBeam),
                new Modifiers.Blotches(4), 
                new Modifiers.Dither(), 
                new Modifiers.IsEmpty(), 
                new Actions.PlaceTile(TileID.Cobweb)
            ));
            */
            //MinistructureList.WoodenLamppost.Place(point);
            //PlaceWIPCave(point);
            PlaceRuin(point);
            //PlaceOtherSpiritModThing(point);
            //PlaceFish_Flying(point);

            /*
            int type11 = -1;
            if (Main.rand.NextBool(10))
            {
                type11 = -2;
            }
            int num993 = Main.rand.Next(7, 26);
            int steps = Main.rand.Next(50, 200);
            double num994 = (double)Main.rand.Next(100, 221) * 0.1;
            double num995 = (double)Main.rand.Next(-10, 11) * 0.02;
            int i7 = Main.rand.Next(0, Main.maxTilesX); // this was the x
            int j9 = Main.rand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY); // this was the y
            //WorldGen.TileRunner(point.X, point.Y, num993, steps, type11, addTile: false, num994, num995, noYChange: true);
            //WorldGen.TileRunner(point.X, point.Y, num993, steps, type11, addTile: false, 0.0 - num994, 0.0 - num995, noYChange: true);
             
            WorldGen.TileRunner(point.X, point.Y, 50, 400, -1, false, num994, num995, noYChange: true);
            WorldGen.TileRunner(point.X, point.Y, 50, 400, -1, false, -num994, -num995, noYChange: true);
            */

            //MinistructureList.SpearRack.CheckAndPlaceTile(point);
            //PlaceOtherSpiritModThing(point);

            /*int floorTile = TileID.VortexBrick;
            if (Main.tile[point.X, point.Y].TileType == (ushort)floorTile && !Main.tile[point.X - 1, point.Y].HasTile)
            {
                Vector2 currentTilePointer = new Vector2(point.X - 1, point.Y);
                while (!Main.tile[(int)currentTilePointer.X, (int)currentTilePointer.Y].HasTile)
                {
                    //8 left 10 right
                    WorldGen.PlaceTile((int)currentTilePointer.X, (int)currentTilePointer.Y, TileID.Platforms);
                    Main.tile[(int)currentTilePointer.X, (int)currentTilePointer.Y].TileFrameX = 8;
                    currentTilePointer = new Vector2(currentTilePointer.X - 1, currentTilePointer.Y + 1);
                    Main.NewText(currentTilePointer);
                }
            }*/

            return null;
        }

        public static Vector2D MakeSolidTunnel(double X, double Y, int Type, double xDir, double yDir, int Steps, int Size)
        {
            double num = X;
            double num2 = Y;
            try
            {
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = Size;
                num = Utils.Clamp(num, num5 + 1.0, (double)Main.maxTilesX - num5 - 1.0);
                num2 = Utils.Clamp(num2, num5 + 1.0, (double)Main.maxTilesY - num5 - 1.0);
                for (int i = 0; i < Steps; i++)
                {
                    for (int j = (int)(num - num5); (double)j <= num + num5; j++)
                    {
                        for (int k = (int)(num2 - num5); (double)k <= num2 + num5; k++)
                        {
                            if (Math.Abs((double)j - num) + Math.Abs((double)k - num2) < num5 * (1.0 + (double)Main.rand.Next(-10, 11) * 0.005) && j >= 0 && j < Main.maxTilesX && k >= 0 && k < Main.maxTilesY)
                            {
                                //Main.tile[j, k].active(active: false);
                                WorldGen.PlaceTile(j, k, Type);
                            }
                        }
                    }

                    num5 += (double)Main.rand.Next(-50, 51) * 0.03;
                    if (num5 < (double)Size * 0.6)
                        num5 = (double)Size * 0.6;

                    if (num5 > (double)(Size * 2))
                        num5 = Size * 2;

                    num3 += (double)Main.rand.Next(-20, 21) * 0.01;
                    num4 += (double)Main.rand.Next(-20, 21) * 0.01;
                    if (num3 < -1.0)
                        num3 = -1.0;

                    if (num3 > 1.0)
                        num3 = 1.0;

                    if (num4 < -1.0)
                        num4 = -1.0;

                    if (num4 > 1.0)
                        num4 = 1.0;

                    num += (xDir + num3) * 0.6;
                    num2 += (yDir + num4) * 0.6;
                }
            }
            catch
            {
            }

            return new Vector2D(num, num2);
        }

        /// <summary>
        /// Makes a Christmas tree in the world.
        /// </summary>
        /// <param name="location">The location to start the Christmas tree. Starts at the base of the log.</param>
        /// <param name="trunkHeightAmount">How tall the tree should be.</param>
        /// <param name="individualHeight">How tall each segment of the tree should be.</param>
        public static void MakeChristmasTree(Point location, int trunkHeightAmount, int individualHeight)
        {
            List<int> pointsForFronds = new List<int>();

            // trunk
            for (int i = 0; i < trunkHeightAmount; i++)
            {
                Point pointAccountForDistance = new Point(location.X, location.Y - (i * individualHeight));
                WorldUtils.Gen(pointAccountForDistance, new Shapes.Mound(5, individualHeight), new Actions.PlaceTile(TileID.PineTree));
                if (i != 0 && i % 2 == 0)
                {
                    pointsForFronds.Add(i);
                    WorldUtils.Gen(new Point(pointAccountForDistance.X + 3, pointAccountForDistance.Y + individualHeight), new Shapes.Mound(5, individualHeight), new Actions.PlaceTile(TileID.PineTree));
                    WorldUtils.Gen(new Point(pointAccountForDistance.X - 3, pointAccountForDistance.Y + individualHeight), new Shapes.Mound(5, individualHeight), new Actions.PlaceTile(TileID.PineTree));
                }
            }

            // leaves
            int iterationCount = pointsForFronds.Count() * 3;
            for (int i = 0; i < pointsForFronds.Count(); i++)
            {
                for (int ii = 0; ii < iterationCount; ii++)
                {
                    Point pointAccountForDistance = new Point(location.X, location.Y - (pointsForFronds[i] * individualHeight));
                    WorldUtils.Gen(new Point(pointAccountForDistance.X + ii, pointAccountForDistance.Y), new Shapes.Mound(5, (int)(individualHeight * 1.5f)), new Actions.PlaceTile(TileID.PineTree));
                    WorldUtils.Gen(new Point(pointAccountForDistance.X - ii, pointAccountForDistance.Y), new Shapes.Mound(5, (int)(individualHeight * 1.5f)), new Actions.PlaceTile(TileID.PineTree));
                }
                iterationCount -= 3;
            }
        }

        public static void MakeAwesomeHouseStuff(Point location)
        {
            //TODO: check all todos
            //TODO: change all instances of main.rand to worldgen.rand
            
            int boxX = 60;
            int boxY = 40;
            int wallTile = TileID.Titanstone;
            int floorTile = TileID.VortexBrick;

            Point origin = new Point(location.X, location.Y - boxY);
            //TODO: remove this when turning into proper worldgen
            WorldUtils.Gen(origin, new Shapes.Rectangle(boxX, boxY), new Actions.ClearTile());
            WorldUtils.Gen(origin, new Shapes.Rectangle(boxX, boxY), new Actions.PlaceTile((ushort)wallTile));

            // area of the ENTIRE boxx
            Rectangle areaOfStuff = new Rectangle(origin.X, origin.Y, boxX, boxY);
            int padding = 2;
            Point originWithPadding = new Point(origin.X + padding, origin.Y + padding);
            // area of the box with padding applied; use this
            Rectangle areaOfStuffYouCanUse = new Rectangle(originWithPadding.X, originWithPadding.Y, boxX - (padding * 2), boxY - (padding * 2));
            
            // make the shape data we're gonna keep the general layout in
            ShapeData roomShapeData = new ShapeData();
            // add the middle hall
            // there will always be a hall from the left to the right of the usable zone
            int halfOfUsableAreaY = areaOfStuffYouCanUse.Height / 2;
            int middleHallHeight = areaOfStuffYouCanUse.Height / 2 - 3 + Main.rand.Next(-3, 4);
            Point originWithPaddingAndProperPosition = new Point(originWithPadding.X, originWithPadding.Y + halfOfUsableAreaY - (middleHallHeight / 2));
            WorldUtils.Gen(originWithPaddingAndProperPosition, new Shapes.Rectangle(areaOfStuffYouCanUse.Width, middleHallHeight), new Actions.Blank().Output(roomShapeData));

            // to that, add some extra evil empty rectangles for more variety 
            for (int i = 0; i < 3; i++)
            {
                // get sizes
                Point boxSize = Point.Zero;
                boxSize.X = (areaOfStuffYouCanUse.Width / 2) + Main.rand.Next(-5, 0); // + rand
                boxSize.Y = (areaOfStuffYouCanUse.Height / 4) + Main.rand.Next(-5, 0); // + rand

                // determine whether to place rect at top or not
                bool top = Main.rand.NextBool();
                Point offset = Point.Zero;
                if (top)
                    offset.Y = -boxSize.Y;
                else
                    offset.Y = middleHallHeight;
                // and get horiz offset, anywhere on the hallway horizontally
                offset.X = Main.rand.Next(0, areaOfStuffYouCanUse.Width - boxSize.X);

                int tile = top ? TileID.BubblegumBlock : TileID.CactusBlock;

                WorldUtils.Gen(origin, new Shapes.Rectangle(boxSize.X, boxSize.Y), Actions.Chain(new GenAction[]
                {
                    new Modifiers.Offset(offset.X, offset.Y),
                    new Actions.Blank().Output(roomShapeData)
                }));
            }

            // make that shit
            WorldUtils.Gen(originWithPaddingAndProperPosition, new ModShapes.All(roomShapeData), new Actions.ClearTile());

            // fuck it lets raise one of hte sides for no reason
            if (Main.rand.NextFloat() > 0.25f)
            {
                for (int y = areaOfStuffYouCanUse.Y; y < areaOfStuffYouCanUse.Y + areaOfStuffYouCanUse.Height; y++)
                {
                    bool left = Main.rand.NextBool();
                    Tile leftOrRightTile = new Tile();
                    if (left)
                        leftOrRightTile = Main.tile[areaOfStuffYouCanUse.X, y];
                    else
                        leftOrRightTile = Main.tile[areaOfStuffYouCanUse.X + areaOfStuffYouCanUse.Width - 1, y];
                    int amountOfBlocksToRaiseUp = Main.rand.Next(1, 4);
                    if (leftOrRightTile.TileType == (ushort)wallTile && !Main.tile[areaOfStuffYouCanUse.X, y - 1].HasTile)
                    {
                        Tile tile = Main.tile[areaOfStuffYouCanUse.X, y];
                        Point floorPositionPointer = new Point(areaOfStuffYouCanUse.X, y);
                        if (!left)
                            floorPositionPointer = new Point(areaOfStuffYouCanUse.X + areaOfStuffYouCanUse.Width - 1, y);
                        while (tile.HasTile)
                        {
                            // place a corrosponding amt of stuff above the floor
                            for (int i = amountOfBlocksToRaiseUp; i > 0; i--)
                            {
                                tile = Main.tile[floorPositionPointer.X, floorPositionPointer.Y - i];
                                tile.HasTile = true;
                                tile.TileType = (ushort)wallTile;

                            }
                            //floorPositionPointer.Y += amountOfBlocksToRaiseUp;

                            // move onto next tile
                            if (left)
                                floorPositionPointer.X++;
                            else
                                floorPositionPointer.X--;
                            tile = Main.tile[floorPositionPointer.X, floorPositionPointer.Y];
                        }
                    }
                }
            }

            // place unique floor tiles over wall tiles that have air above them
            for (int x = areaOfStuff.X; x < areaOfStuff.X + areaOfStuff.Width; x++)
            {
                // the + 1 here is to prevent covering the current top of the room with floor 
                for (int y = areaOfStuff.Y + 1; y < areaOfStuff.Y + areaOfStuff.Height; y++)
                {
                    if (Main.tile[x, y].TileType == (ushort)wallTile && !Main.tile[x, y - 1].HasTile)
                    {
                        Main.tile[x, y].TileType = (ushort)floorTile;
                    }
                }
            }

            // place stair platforms off of anything with a harsh edge
            for (int x = areaOfStuffYouCanUse.X; x < areaOfStuffYouCanUse.X + areaOfStuffYouCanUse.Width; x++)
            {
                for (int y = areaOfStuffYouCanUse.Y; y < areaOfStuffYouCanUse.Y + areaOfStuffYouCanUse.Height; y++)
                {
                    // check everything to the left...
                    if (Main.tile[x, y].TileType == (ushort)floorTile && Main.tile[x, y].HasTile && !Main.tile[x - 1, y].HasTile)
                    {
                        Point currentTilePointer = new Point(x - 1, y);
                        Tile tile = new Tile();
                        while (!Main.tile[currentTilePointer].HasTile)
                        {
                            tile = Main.tile[currentTilePointer];
                            tile.HasTile = true;
                            tile.TileType = TileID.Platforms;
                            tile.BlockType = BlockType.SlopeDownRight;
                            tile.TileFrameY = 0; //TODO: if using custom platform remove this 
                            tile.TileFrameX = 8 * 18;
                            currentTilePointer.X--;
                            currentTilePointer.Y++;
                        }
                    }
                    // ...and to the right
                    if (Main.tile[x, y].TileType == (ushort)floorTile && Main.tile[x, y].HasTile && !Main.tile[x + 1, y].HasTile)
                    {
                        Point currentTilePointer = new Point(x + 1, y);
                        Tile tile = new Tile();
                        while (!Main.tile[currentTilePointer].HasTile)
                        {
                            tile = Main.tile[currentTilePointer];
                            tile.HasTile = true;
                            tile.TileType = TileID.Platforms;
                            tile.BlockType = BlockType.SlopeDownLeft;
                            tile.TileFrameY = 0; //TODO: if using custom platform remove this 
                            tile.TileFrameX = 10 * 18;
                            currentTilePointer.X++;
                            currentTilePointer.Y++;
                        }
                    }
                }
            }

            // for each platform tile we wanna check behind it for any weird shit
            // i was gonna make this work but then i saw a stair generate inside of another stair and i just kinda frowned
            /*for (int x = areaOfStuffYouCanUse.X; x < areaOfStuffYouCanUse.X + areaOfStuffYouCanUse.Width; x++)
            {
                for (int y = areaOfStuffYouCanUse.Y; y < areaOfStuffYouCanUse.Y + areaOfStuffYouCanUse.Height; y++)
                {
                    if (Main.tile[x, y].TileType == TileID.Platforms)
                    {
                        Tile tile = Main.tile[x, y];
                        if (tile.BlockType == BlockType.SlopeDownRight)
                        {
                            Point findShittyPlatformsPointer = new Point(x, y);
                            while (tile.TileType != (ushort)wallTile || tile.TileType != (ushort)floorTile)
                            {
                                findShittyPlatformsPointer.X++;
                                tile = Main.tile[findShittyPlatformsPointer.X, findShittyPlatformsPointer.Y];
                                if (tile.TileType == TileID.Platforms && tile.BlockType == BlockType.SlopeDownRight)
                                {
                                    // we found one! a real one!
                                    tile.TileType = TileID.Adamantite;
                                    // so then... behind it should be floor!
                                    // get ridda that floor
                                    findShittyPlatformsPointer.X++;
                                    tile = Main.tile[findShittyPlatformsPointer.X, findShittyPlatformsPointer.Y];
                                    while (tile.TileType == (ushort)floorTile || !tile.HasTile)
                                    {
                                        tile.HasTile = true;
                                        tile.TileType = TileID.Adamantite;
                                        Main.tile[findShittyPlatformsPointer.X, findShittyPlatformsPointer.Y + 1].TileType = (ushort)floorTile;
                                        findShittyPlatformsPointer.X++;
                                        tile = Main.tile[findShittyPlatformsPointer.X, findShittyPlatformsPointer.Y];
                                    }
                                }
                            }
                        }
                        else if (tile.BlockType == BlockType.SlopeDownLeft)
                        {

                        }
                    }
                }
            }*/

            // fill with decorum
            for (int x = areaOfStuffYouCanUse.X; x < areaOfStuffYouCanUse.X + areaOfStuffYouCanUse.Width; x++)
            {
                for (int y = areaOfStuffYouCanUse.Y; y < areaOfStuffYouCanUse.Y + areaOfStuffYouCanUse.Height; y++)
                {

                }
            }
        }

        public static void PlaceRuin(Point location) 
        {
            // set up area to gen in
            ShapeData ruinShapeData = new ShapeData();
            int ruinRadius = Main.rand.Next(8, 12);
            WorldUtils.Gen(location, new Shapes.Circle(ruinRadius), new Actions.Blank().Output(ruinShapeData));

            // place bricks
            WorldUtils.Gen(location, new ModShapes.All(ruinShapeData), Actions.Chain(
                new Actions.Custom((i, j, args) => {
                    if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].IsTileSolid() && !Main.rand.NextBool(5))
                    {
                        if (Main.rand.NextBool(4))
                            Main.tile[i, j].TileType = TileID.CrackedBlueDungeonBrick;
                        else
                            Main.tile[i, j].TileType = TileID.BlueDungeonBrick;
                        SquareTileFrame(i, j);
                    }
                    return true;
                })
            ));

            // place pots
            WorldUtils.Gen(location, new ModShapes.All(ruinShapeData), Actions.Chain(
                new Actions.Custom((i, j, args) => { // i forgot if theres a better way to do this
                    if (!Main.tile[i, j - 1].HasTile &&
                    !Main.tile[i + 1, j - 1].HasTile &&
                    !Main.tile[i, j - 2].HasTile &&
                    !Main.tile[i + 1, j - 2].HasTile &&
                    Main.tile[i, j].IsTileSolid() &&
                    Main.tile[i + 1, j].IsTileSolid() &&
                    Main.tile[i, j].Slope == SlopeType.Solid &&
                    Main.rand.NextBool(3))
                    {
                        PlaceObject(i, j - 1, TileID.FishingCrate);
                    }
                    return true;
                })
            ));

            // place third thing

        }

        public static void PlaceWIPCave(Point origin)
        {
            ShapeData coveShapeData = new ShapeData();
            ShapeData lakeShapeData = new ShapeData();
            Point point = new Point(origin.X, origin.Y);
            float xScale = 0.8f + Main.rand.NextFloat() * 0.25f; // Randomize the width of the shrine area
            int slimeRadius = 25;
            int slimeOffsetX = 30 + Main.rand.Next(3, 9);
            int leftOffsetY = Main.rand.Next(-6, 6);
            int rightOffsetY = Main.rand.Next(-6, 6);
            bool doOppositeLedge = Main.rand.NextBool();
            Point oppositeLedgePoint = new Point();

            #region generating the cave
            // add the evil lake
            bool left = Main.rand.NextBool();
            int lakeOffsetX = left ? -slimeOffsetX / 2 : slimeOffsetX / 2;
            int lakeOffsetY = 40 + Main.rand.Next(-5, 5);
            WorldUtils.Gen(point, new Shapes.Mound(slimeRadius + Main.rand.Next(0, 10), 40), Actions.Chain(
                new Modifiers.Offset(lakeOffsetX, lakeOffsetY),
                new Actions.Blank().Output(lakeShapeData)
            ));

            // center shape
            WorldUtils.Gen(point, new Shapes.Slime(slimeRadius, xScale, 1f - Main.rand.NextFloat(0, 0.25f)), Actions.Chain(
                new Actions.Blank().Output(coveShapeData)
            ));
            int smallCliffOffsetY = Main.rand.Next(-8, 8);
            if (Main.rand.NextBool())
            {
                if (left)
                    leftOffsetY += smallCliffOffsetY;
                else
                    rightOffsetY += smallCliffOffsetY;
            }
            if (left || doOppositeLedge)
            {
                // right
                WorldUtils.Gen(point, new Shapes.Slime(slimeRadius, xScale * 0.6, 1f - Main.rand.NextFloat(0.3f, 0.5f)), Actions.Chain(
                    new Modifiers.Offset((int)(slimeOffsetX * 0.8), rightOffsetY),
                    new Actions.Blank().Output(coveShapeData)
                ));
                // upper bulge
                WorldUtils.Gen(point, new Shapes.Slime(slimeRadius / 2, xScale, 1f - Main.rand.NextFloat(0, 0.25f)), Actions.Chain(
                    new Modifiers.Offset(slimeOffsetX / 2, -10 + rightOffsetY),
                    new Actions.Blank().Output(coveShapeData)
                ));
                if (!left && doOppositeLedge)
                    oppositeLedgePoint = new Point(point.X + (int)(slimeOffsetX * 0.8), point.Y + rightOffsetY);
            }
            if (!left || doOppositeLedge)
            {
                // left
                WorldUtils.Gen(point, new Shapes.Slime(slimeRadius, xScale * 0.6, 1f - Main.rand.NextFloat(0.3f, 0.5f)), Actions.Chain(
                    new Modifiers.Offset(-(int)(slimeOffsetX * 0.8), leftOffsetY),
                    new Actions.Blank().Output(coveShapeData)
                ));
                // upper bulge
                WorldUtils.Gen(point, new Shapes.Slime(slimeRadius / 2, xScale, 1f - Main.rand.NextFloat(0, 0.25f)), Actions.Chain(
                    new Modifiers.Offset(-slimeOffsetX / 2, -10 + leftOffsetY),
                    new Actions.Blank().Output(coveShapeData)
                ));
                if (left && doOppositeLedge)
                    oppositeLedgePoint = new Point(point.X - (int)(slimeOffsetX * 0.8), point.Y + leftOffsetY);
            }

            // lower bulges to clip off uneven edges
            //TODO: make these lines? branches? tails? whatever theyre called?
            WorldUtils.Gen(point, new Shapes.Slime(slimeRadius / 2, xScale, 1f), Actions.Chain(
                new Modifiers.Offset(slimeOffsetX / 2, 5 + rightOffsetY),
                new Actions.Blank().Output(coveShapeData)
            ));
            WorldUtils.Gen(point, new Shapes.Slime(slimeRadius / 2, xScale, 1f), Actions.Chain(
                new Modifiers.Offset(-slimeOffsetX / 2, 5 + leftOffsetY),
                new Actions.Blank().Output(coveShapeData)
            ));

            // actually placing shit
            WorldUtils.Gen(point, new ModShapes.All(coveShapeData), Actions.Chain(
                new Modifiers.Blotches(2, 0.8),
                new Actions.ClearTile(frameNeighbors: true),
                new Modifiers.Blotches(2, 0.8),
                new Modifiers.IsTouchingAir(),
                new Actions.Smooth()
            ));
            WorldUtils.Gen(point, new ModShapes.All(lakeShapeData), Actions.Chain(
                new Modifiers.Blotches(2, 0.8),
                new Actions.ClearTile(frameNeighbors: true)
            ));

            // smooth the floor of the cave (NOT THE LAKE)
            // note: this isnt sloping. this is getting rid of 1 block juts
            WorldUtils.Gen(point, new ModShapes.OuterOutline(coveShapeData), Actions.Chain(
                new Actions.Custom((i, j, args) => {
                    int amtOfAit = 0;
                    if (!Main.tile[i, j - 1].HasTile)
                    {
                        amtOfAit++;
                        if (!Main.tile[i - 1, j].HasTile)
                            amtOfAit++;
                        if (!Main.tile[i + 1, j].HasTile)
                            amtOfAit++;
                        if (!Main.tile[i, j + 1].HasTile)
                            amtOfAit++;

                        if (amtOfAit >= 3)
                        {
                            Tile tile = Main.tile[i, j];
                            tile.ClearTile();
                            SquareTileFrame(i, j);
                        }
                    }
                    return true;
                })
            ));
            #endregion

            List<Point> validPointsForPlacement = GetGeneralPlacementArea(point, 100, 30);

            #region cabin
            // now we get to placing the cabin
            // ill be calling it the cabin but it could be anything, its where the loot goes basically
            int cabinOffsetX = left ? slimeOffsetX / 2 : -slimeOffsetX / 2;
            Point cabinSpot = new Point(point.X + cabinOffsetX, origin.Y + 10);

            if (Main.rand.NextBool(3))
                PlaceOtherSpiritModThing(cabinSpot, left);
            else
                PlaceOtherSpiritModThing_Tent(cabinSpot, left);

            #endregion

            #region fish
            // place that dumbfuck fish
            int maxFishAlts = 2;
            int fishToPlace = Main.rand.Next(0, maxFishAlts);
            bool hasPlacedFish = false;
            int fishPlaceAttempts = 0;
            if (fishToPlace == 0) // ledge
            {
                if (!doOppositeLedge) // if no ledge then only bros
                {
                    fishToPlace = Main.rand.Next(1, maxFishAlts);
                }
                else
                {
                    List<Point> validFishSpot = GetGeneralPlacementArea(oppositeLedgePoint, 30, 30);
                    while (!hasPlacedFish)
                    {
                        fishPlaceAttempts++;
                        if (fishPlaceAttempts > 2500)
                            break;

                        Point randomPoint = validFishSpot[Main.rand.Next(validFishSpot.Count())];
                        randomPoint.Y--;
                        PlaceTile(randomPoint.X, randomPoint.Y, TileID.VoidMonolith, mute: true, forced: false, -1);
                        hasPlacedFish = Main.tile[randomPoint].TileType == TileID.VoidMonolith;
                    }
                }
            }
            if (fishToPlace == 1) // flying
            {
                Point flyingFishPlace = new Point();
                flyingFishPlace.Y = point.Y + Main.rand.Next(-5, 3);
                if (left)
                {
                    flyingFishPlace.X = point.X + -(int)(slimeOffsetX * Main.rand.NextFloat(0.4f, 0.6f));
                    if (!doOppositeLedge)
                        flyingFishPlace.X += Main.rand.Next(5, 8);
                }
                else
                {
                    flyingFishPlace.X = point.X + (int)(slimeOffsetX * Main.rand.NextFloat(0.4f, 0.6f));
                    if (!doOppositeLedge)
                        flyingFishPlace.X -= Main.rand.Next(5, 8);
                }

                PlaceFish_Flying(flyingFishPlace);
                hasPlacedFish = true;
            }
            else if (fishToPlace == 2)
            {
                //TODO: make third variant, over the water on a rock
            }
            else if (fishToPlace == 3)
            {
                //TODO: make fourth varient, on the water on a raft
            }
            
            // if we fail to place fish, then place the flying variant since its the most versatile
            //TODO: reduce redundancy
            if (!hasPlacedFish)
            {
                Point flyingFishPlace = new Point();
                flyingFishPlace.Y = point.Y + Main.rand.Next(-5, 3);
                if (left)
                {
                    flyingFishPlace.X = point.X + -(int)(slimeOffsetX * Main.rand.NextFloat(0.4f, 0.6f));
                    if (!doOppositeLedge)
                        flyingFishPlace.X -= Main.rand.Next(1, 4);
                }
                else
                {
                    flyingFishPlace.X = point.X + (int)(slimeOffsetX * Main.rand.NextFloat(0.4f, 0.6f));
                    if (!doOppositeLedge)
                        flyingFishPlace.X += Main.rand.Next(1, 4);
                }

                PlaceFish_Flying(flyingFishPlace);
                hasPlacedFish = true;
            }
            #endregion

            #region light fixture
            // place the lamppost or whatever it is
            bool hasPlacedLamp = false;
            int placeLampAttempts = 0;
            while (!hasPlacedLamp)
            {
                placeLampAttempts++;
                if (placeLampAttempts > 2500)
                    break;

                Point randomPoint = validPointsForPlacement[Main.rand.Next(validPointsForPlacement.Count())];
                randomPoint.Y--;

                if (MinistructureList.WoodenLamppost.Check(randomPoint))
                {
                    MinistructureList.WoodenLamppost.Place(randomPoint);
                    hasPlacedLamp = true;
                }
            }
            #endregion

            #region stalagmites n tites n whatever
            WorldUtils.Gen(point, new ModShapes.OuterOutline(coveShapeData), Actions.Chain(
                new Actions.Custom((i, j, args) => {
                    if (Main.tile[i, j].TileType == TileID.Stone && !Main.tile[i, j + 1].HasTile && Main.rand.NextBool(3))
                        PlaceTight(i, j + 1);
                    return true;
                })
            ));
            #endregion

            #region ughh RUBBLE
            // now the shitty one uhghhhhhhhh
            // placing small piles (FUCK LARGE PILES!!! GRAAASGHHHHHHH)
            int decorAmount = Main.rand.Next(5, 11);
            int placeDecorAttempts = 0;
            while (decorAmount > 0)
            {
                placeDecorAttempts++;
                if (placeDecorAttempts > 2500)
                    break;

                Point randomPoint = validPointsForPlacement[Main.rand.Next(validPointsForPlacement.Count())];
                int randomStyleMin = 1;
                int randomStyleMax = 1;
                int horizRow = 1;
                switch (Main.tile[randomPoint].TileType)
                {
                    case TileID.Stone:
                        randomStyleMin = 0;
                        randomStyleMax = 6;
                        horizRow = 0;
                        break;
                }
                randomPoint.Y--;

                // our valid points list is actually loaded with a ton of invalid points since it only checks for blocks and not blocks whose
                // tops can have stuff placed on them
                // this is a nonissue for us as any spots involving those will fail
                // maybe ill fix it later. probably not though
                if (WorldGen.PlaceSmallPile(randomPoint.X, randomPoint.Y, Main.rand.Next(randomStyleMin, randomStyleMax), horizRow, 185))
                    decorAmount--;
            }
            #endregion
        }

        public static void PlaceFish_Flying(Point origin)
        {
            Tile tile = new Tile();
            int platformLength = 4;

            // placing platforms
            for (int i = 0; i < platformLength; i++)
            {
                tile = Main.tile[origin.X + i, origin.Y];
                tile.ResetToType(TileID.Platforms);
                SquareTileFrame(origin.X + i, origin.Y);
            }

            // placing chains
            tile = Main.tile[origin.X, origin.Y - 1];
            tile.ResetToType(TileID.Chain);
            SquareTileFrame(origin.X, origin.Y - 1);
            tile = Main.tile[origin.X + platformLength - 1, origin.Y - 1];
            tile.ResetToType(TileID.Chain);
            SquareTileFrame(origin.X + platformLength - 1, origin.Y - 1);

            // placing rope
            List<Point> tilePointerList = new List<Point>();
            tilePointerList.Add(new Point(origin.X, origin.Y - 2));
            tilePointerList.Add(new Point(origin.X + platformLength - 1, origin.Y - 2));
            foreach (Point pointer in tilePointerList)
            {
                Point currentTilePointer = pointer;
                while (!Main.tile[currentTilePointer].HasTile)
                {
                    tile = Main.tile[currentTilePointer];
                    tile.ResetToType(TileID.Rope);
                    SquareTileFrame(currentTilePointer.X, currentTilePointer.Y);
                    currentTilePointer.Y--;
                }
                if (Main.tile[currentTilePointer].HasTile && Main.tile[currentTilePointer].Slope != SlopeType.Solid)
                {
                    tile = Main.tile[currentTilePointer];
                    tile.Slope = SlopeType.Solid;
                    SquareTileFrame(currentTilePointer.X, currentTilePointer.Y);
                }
            }

            // placing fish
            //TODO: face diff directions
            PlaceTile(origin.X + 1, origin.Y - 1, TileID.VoidMonolith, mute: true, forced: false, -1);
        }
        public static void PlaceOtherSpiritModThing_Debug(Point origin, bool? realLeft = null)
        {
            Rectangle validPlacementRect = new Rectangle(origin.X - 30, origin.Y - 15, 60, 30);
            Tile tile = new Tile();
            List<Point> validPointsForPlacement = GetGeneralPlacementArea(origin, 60, 30);

            foreach (Point point in validPointsForPlacement)
            {
                tile = Main.tile[point.X, point.Y];
                tile.ResetToType(TileID.Adamantite);
            }
            Main.tile[origin].ResetToType(TileID.Orichalcum);
        }
        public static void PlaceOtherSpiritModThing_Tent(Point origin, bool? realLeft = null)
        {
            #region setup what structures to place
            int placeTileAttempts = 0;

            Ministructure centerpiece = MinistructureList.CrateStack;
            List<Ministructure> centerpieceAlt = new List<Ministructure>();
            List<Ministructure> largeDecor = new List<Ministructure>();
            int largeDecorAmount = 0;
            int smallPileAmount = 0;
            int smallPileStyleMin = 0;
            int smallPileStyleMax = 0;
            int smallPileHorizRow = 0;
            int failsafeGenThreshold = 0; // higher it is = higher chance to use failsafe gen

            switch(Main.rand.Next(0, 2))
            {
                case 0: // campsite
                    centerpiece = MinistructureList.LargeCamp;
                    centerpieceAlt.Add(MinistructureList.TentRubble);
                    centerpieceAlt.Add(MinistructureList.CampfireDisabled);
                    largeDecor.Add(MinistructureList.LargeStoneRubbleSpecialAndChest);
                    largeDecor.Add(MinistructureList.LargeStoneRubbleSpecialAndChest);
                    largeDecor.Add(MinistructureList.SmallCoinPile);
                    largeDecor.Add(MinistructureList.SmallCoinPile);
                    largeDecor.Add(MinistructureList.LargeCoinPile);
                    largeDecor.Add(MinistructureList.LargeCoinPile);
                    largeDecor.Add(MinistructureList.SmallCoinStash);
                    largeDecor.Add(MinistructureList.SmallCoinStash);
                    largeDecor.Add(MinistructureList.LargeCoinStash);
                    largeDecor.Add(MinistructureList.LargeCoinStash);
                    largeDecorAmount = Main.rand.Next(3, 6);
                    smallPileAmount = Main.rand.Next(4, 7);
                    smallPileStyleMin = 12;
                    smallPileStyleMax = 19;
                    smallPileHorizRow = 0;
                    failsafeGenThreshold = 10;
                    break;
                case 1: // fishing wall
                    centerpiece = MinistructureList.FishingWall;
                    centerpieceAlt.Add(MinistructureList.FishingWall);
                    largeDecor.Add(MinistructureList.LargeStoneRubbleSpecialAndChest);
                    largeDecor.Add(MinistructureList.LargeStoneRubbleSpecialAndChest);
                    largeDecor.Add(MinistructureList.UndergroundObjectRubble);
                    largeDecor.Add(MinistructureList.UndergroundObjectRubble);
                    largeDecor.Add(MinistructureList.SmallCoinPile);
                    largeDecor.Add(MinistructureList.SmallCoinPile);
                    largeDecor.Add(MinistructureList.SmallCoinStash);
                    largeDecor.Add(MinistructureList.LargeCoinStash);
                    largeDecorAmount = Main.rand.Next(3, 6);
                    smallPileAmount = Main.rand.Next(2, 4);
                    smallPileStyleMin = 31;
                    smallPileStyleMax = 33;
                    smallPileHorizRow = 1;
                    failsafeGenThreshold = 12;
                    break;
            }
            #endregion

            #region setup area to place in
            int width = 60;
            int height = 30;
            List<List<Point>> bestPointsForPlacement = GetIdealPlacementArea(origin, width, height, 5);
            List<Point> bestPointsForPlacement_All = new List<Point>();
            foreach (List<Point> list in bestPointsForPlacement)
            {
                foreach (Point point in list)
                    bestPointsForPlacement_All.Add(point);
            }
            // getting a bunch of stuff we wanna know for later
            // if we have a wide enough space to place a large ministructure do so!
            // if not, then we can place a small tent and campfire separately instead
            bool shouldPlaceBigCamp = false;
            List<Point> idealCampSpot = new List<Point>();
            foreach (List<Point> list in bestPointsForPlacement)
            {
                int sumOfCurrentList = 0;
                foreach (Point point in list)
                    sumOfCurrentList++;
                if (sumOfCurrentList >= MinistructureList.LargeCamp.width && list.Count > idealCampSpot.Count)
                {
                    shouldPlaceBigCamp = true;
                    idealCampSpot = list;
                }
            }
            #endregion

            #region failsafe stuff
            // if the amount of total space we have is less than 10 blocks, gen failsafe stuff
            int sumOfAllBestPoints = 0;
            if (bestPointsForPlacement_All.Count > 0)
            {
                foreach (Point point in bestPointsForPlacement_All)
                    sumOfAllBestPoints++;
            }
            else
                return;
            bool doFailsafeGen = sumOfAllBestPoints < failsafeGenThreshold;

            // failsafe if the returned area is less than 8 blocks
            if (doFailsafeGen)
                bestPointsForPlacement_All = GenerateFailsafeLocation(origin);
            idealCampSpot = bestPointsForPlacement_All;
            #endregion

            #region primary decor
            // place camp
            int hasPlacedCenterpiece = 0;
            // place nice if possible
            while (hasPlacedCenterpiece < centerpieceAlt.Count && shouldPlaceBigCamp)
            {
                placeTileAttempts++;
                if (placeTileAttempts > 5000)
                    break;

                // suboptimal but idc
                Point randomPoint = idealCampSpot[Main.rand.Next(idealCampSpot.Count())];
                randomPoint.Y--;
                if (centerpiece.Check(randomPoint))
                {
                    centerpiece.Place(randomPoint);
                    hasPlacedCenterpiece = centerpieceAlt.Count;
                }
            }
            // if we cant place nice, place rough
            while (hasPlacedCenterpiece < centerpieceAlt.Count)
            {
                placeTileAttempts++;
                if (placeTileAttempts > 10000)
                    break;

                Point randomPoint = bestPointsForPlacement_All[Main.rand.Next(bestPointsForPlacement_All.Count())];
                randomPoint.Y--;
                Ministructure randomStructure = centerpieceAlt[Main.rand.Next(centerpieceAlt.Count())];
                if (randomStructure.Check(randomPoint))
                {
                    randomStructure.Place(randomPoint);
                    centerpieceAlt.Remove(randomStructure);
                    hasPlacedCenterpiece++;
                }
            }
            placeTileAttempts = 0;
            #endregion

            #region chest
            // place chest
            //TODO: make this a method
            bool hasPlacedChest = false;
            while (!hasPlacedChest)
            {
                placeTileAttempts++;
                if (placeTileAttempts > 5000)
                    break;

                Point randomPoint = bestPointsForPlacement_All[Main.rand.Next(bestPointsForPlacement_All.Count())];
                randomPoint.Y--;
                if (!Main.tile[randomPoint.X, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + 1, randomPoint.Y].HasTile)
                {
                    int chest = PlaceChest(randomPoint.X, randomPoint.Y, style: 0);
                    if (chest != -1)
                        hasPlacedChest = true;
                }
            }
            placeTileAttempts = 0;
            #endregion

            #region secondary decor
            // place themed decor
            while (largeDecorAmount > 0)
            {
                placeTileAttempts++;
                if (placeTileAttempts > 5000)
                    break;

                int randomMinistructure = Main.rand.Next(largeDecor.Count);
                Point randomPoint = bestPointsForPlacement_All[Main.rand.Next(bestPointsForPlacement_All.Count())];
                randomPoint.Y--;
                Ministructure randomStructure = largeDecor[randomMinistructure];
                if (randomStructure.Check(randomPoint))
                {
                    // ensuring space
                    if (!Main.tile[randomPoint.X - 1, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + randomStructure.width, randomPoint.Y].HasTile)
                    {
                        randomStructure.Place(randomPoint);
                        largeDecor.Remove(randomStructure);
                        largeDecorAmount--;
                    }
                }
            }
            placeTileAttempts = 0;
            #endregion

            #region themed rubble
            // shitty small bone piles
            List<Point> smallPilePoints = GetGeneralPlacementArea(origin, width, height);
            while (smallPileAmount > 0)
            {
                placeTileAttempts++;
                if (placeTileAttempts > 2500)
                    break;

                Point randomPoint = smallPilePoints[Main.rand.Next(smallPilePoints.Count())];
                randomPoint.Y--;

                if (PlaceSmallPile(randomPoint.X, randomPoint.Y, Main.rand.Next(smallPileStyleMin, smallPileStyleMax + 1), smallPileHorizRow, 185))
                    smallPileAmount--;
            }
            placeTileAttempts = 0;
            #endregion
        }

        public static List<Point> GenerateFailsafeLocation(Point origin)
        {
            Tile tile = new Tile();
            Point currentTilePointer = new Point();
            List<Point> pointsToReturn = new List<Point>();
            
            // put our pointer over the nearest ground
            bool searchUpwards = false;
            currentTilePointer = origin;
            if (Main.tile[currentTilePointer].HasTile)
                searchUpwards = true;
            if (searchUpwards)
            {
                while (Main.tile[currentTilePointer].HasTile)
                    currentTilePointer.Y--;
            }
            else
            {
                while (!Main.tile[currentTilePointer].HasTile)
                    currentTilePointer.Y++;
                currentTilePointer.Y--;
            }

            // empty out shape, place tiles under it, add tiles to valid spot list
            // warning hardcoded slop below bc i dont care
            WorldUtils.Gen(currentTilePointer, new Shapes.Slime(10), Actions.Chain(
                new Modifiers.Offset(0, -4),
                new Actions.ClearTile(true)
            ));
            currentTilePointer.X -= 7;
            currentTilePointer.Y++;
            int originalY = currentTilePointer.Y;
            for (int i = 0; i < 15; i++)
            {
                tile = Main.tile[currentTilePointer];
                tile.Slope = SlopeType.Solid;
                pointsToReturn.Add(currentTilePointer);
                while (!tile.HasTile)
                {
                    tile.ResetToType(TileID.Stone);
                    SquareTileFrame(currentTilePointer.X, currentTilePointer.Y);
                    currentTilePointer.Y++;
                    tile = Main.tile[currentTilePointer];
                }
                tile.Slope = SlopeType.Solid;
                currentTilePointer.Y = originalY;
                currentTilePointer.X++;
            }

            return pointsToReturn;
        }
        public static List<Point> GetGeneralPlacementArea(Point origin, int width, int height)
        {
            Rectangle validPlacementRect = new Rectangle(origin.X - (width / 2), origin.Y - (height / 2), width, height);
            Tile tile = new Tile();
            List<Point> validPointsForPlacement = new List<Point>();

            for (int x = validPlacementRect.X; x < validPlacementRect.X + validPlacementRect.Width; x++)
            {
                for (int y = validPlacementRect.Y; y < validPlacementRect.Y + validPlacementRect.Height; y++)
                {
                    tile = Main.tile[x, y];
                    if (Main.tile[x, y].HasTile && !Main.tile[x, y - 1].HasTile && Main.tile[x, y].Slope == SlopeType.Solid && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && tile != null && tile.HasUnactuatedTile)
                        validPointsForPlacement.Add(new Point(x, y));
                }
            }
            return validPointsForPlacement;
        }
        public static List<List<Point>> GetIdealPlacementArea(Point origin, int width, int height, int threshold)
        {
            Rectangle validPlacementRect = new Rectangle(origin.X - (width / 2), origin.Y - (height / 2), width, height);
            List<Point> validPointsForPlacement = GetGeneralPlacementArea(origin, width, height);
            List<Point> bestPointsForPlacement_Buffer = new List<Point>();
            List<List<Point>> bestPointsForPlacement_Final = new List<List<Point>>();
            int? previousYCoord = null;

            // scan from the top to bottom for every block in our rectangle
            for (int x = validPlacementRect.X; x < validPlacementRect.X + validPlacementRect.Width; x++)
            {
                bool hasFoundTile = false;
                for (int y = validPlacementRect.Y; y < validPlacementRect.Y + validPlacementRect.Height; y++)
                {
                    // if we find a block thats in our valid points list, save it in the buffer list
                    foreach (Point block in validPointsForPlacement)
                    {
                        if (hasFoundTile)
                            break;
                        if (x == block.X && y == block.Y)
                        {
                            // compare to our previous y coord
                            // if there is none, fill it in
                            // if there is and they arent the same, then DONT log this block
                            if (previousYCoord == null)
                                previousYCoord = block.Y;
                            if (previousYCoord == block.Y)
                            {
                                bestPointsForPlacement_Buffer.Add(block);
                                hasFoundTile = true;
                            }
                        }
                    }
                }
                // go until we find a row without a block in the valid points list
                // when we find a block not in our valid points list, see if the buffer is above the threshold
                if (!hasFoundTile)
                {
                    // if the amount of points in the buffer list is greater than the threshold, add our stuff to the final list
                    if (bestPointsForPlacement_Buffer.Count >= threshold)
                        bestPointsForPlacement_Final.Add(bestPointsForPlacement_Buffer);
                    bestPointsForPlacement_Buffer = new List<Point>();
                    previousYCoord = null;
                }
                // continue until we've exhausted our rectangle
            }
            return bestPointsForPlacement_Final;
        }

        public static void PlaceOtherSpiritModThing(Point origin, bool? realLeft = null)
        {
            Point boxDefaultSize = new Point(16, 8);

            // setting up a bunch of variables for box size, offset, etc
            Point boxBottomSize = new Point();
            boxBottomSize.X = boxDefaultSize.X + Main.rand.Next(2, 5);
            if (boxBottomSize.X % 2 == 0)
                boxBottomSize.X--; // we always want to have a perfect center for the bottom beams
            boxBottomSize.Y = boxDefaultSize.Y + Main.rand.Next(0, 1);
            int boxTopSizeDifference = Main.rand.Next(0, 3); // we need to save this to fix a bug later
            Point boxTopSize = new Point();
            boxTopSize.X = boxBottomSize.X + boxTopSizeDifference;
            boxTopSize.Y = boxDefaultSize.Y;

            // always overhang a little on one side
            bool left = Main.rand.NextBool();
            if (realLeft != null)
                left = (bool)realLeft;
            int leftRightMultiplier = left == true ? 1 : -1;
            Point boxTopOffset;
            boxTopOffset.X = Main.rand.Next(3, 6);
            boxTopOffset.X = left == true ? boxTopOffset.X * -1 : boxTopOffset.X;
            boxTopOffset.X += left == false ? boxTopSizeDifference : 0; // compensate for aforementioned bug
            boxTopOffset.Y = -boxTopSize.Y + 1;
            int boxTopCutoff = Main.rand.NextBool() == false ? -2 : -1;

            // fix for top boxes larger than bottom box getting an elongated right overhang
            int boxTopNaturalRightOverhang = boxTopSize.X - boxBottomSize.X;
            if (boxTopNaturalRightOverhang < 0)
                boxTopNaturalRightOverhang = 0;
            if (!left)
                boxTopOffset.X += boxTopNaturalRightOverhang * leftRightMultiplier;

            // making a ton of things for orienting stuff later
            Tile tile;
            Point currentTilePointer;
            Point point = new Point(origin.X - (boxBottomSize.X / 2), origin.Y);
            point.Y -= boxBottomSize.Y - 1;
            Point bottomLeft = point;
            bottomLeft.Y += boxBottomSize.Y - 1;

            // getting the bottom left and top right of the area for when the structure bounding is set
            Point structureBoxBottomLeft;
            structureBoxBottomLeft.X = bottomLeft.X;
            structureBoxBottomLeft.X += bottomLeft.X > bottomLeft.X + boxTopOffset.X ? boxTopOffset.X : -1;
            structureBoxBottomLeft.Y = bottomLeft.Y;
            Point structureBoxTopRight;
            structureBoxTopRight.X = bottomLeft.X + boxBottomSize.X;
            structureBoxTopRight.X += bottomLeft.X > bottomLeft.X + boxTopOffset.X ? 0 : boxTopOffset.X - boxTopSizeDifference - 1;
            structureBoxTopRight.Y = bottomLeft.Y - boxBottomSize.Y - boxTopSize.Y + 2;

            // list of all floor tiles for bottom and top
            List<Point> boxBottomFloor = new List<Point>();
            int boxBottomFloorLeftmost = bottomLeft.X + 2;
            int boxBottomFloorRightmost = bottomLeft.X + boxBottomSize.X - 3;
            for (int i = 0; i < boxBottomFloorRightmost - bottomLeft.X - 1; i++)
            {
                boxBottomFloor.Add(new Point(boxBottomFloorLeftmost + i, bottomLeft.Y));
            }
            List<Point> boxTopFloor = new List<Point>();
            int boxTopBottomLeft = bottomLeft.X + boxTopOffset.X;
            int boxTopFloorLeftmost = boxTopBottomLeft + 2;
            int boxTopFloorRightmost = boxTopBottomLeft + boxTopSize.X - 3;
            for (int i = 0; i < boxTopFloorRightmost - boxTopBottomLeft - 1; i++)
            {
                boxTopFloor.Add(new Point(boxTopFloorLeftmost + i, bottomLeft.Y - boxBottomSize.Y + 1));
            }
            List<Point> allBoxFloors = new List<Point>();
            allBoxFloors.AddRange(boxBottomFloor);
            allBoxFloors.AddRange(boxTopFloor);

            // create the cabins two
            ShapeData cabinShapeDataBottom = new ShapeData();
            WorldUtils.Gen(point, new Shapes.Rectangle(boxBottomSize.X, boxBottomSize.Y), Actions.Chain(
                new Actions.ClearTile(true),
                new Actions.Blank().Output(cabinShapeDataBottom)
            ));
            WorldUtils.Gen(point, new ModShapes.InnerOutline(cabinShapeDataBottom), Actions.Chain(
                new Actions.SetTile(TileID.WoodBlock),
                new Actions.SetFrames(true)
            ));

            ShapeData cabinShapeDataTop = new ShapeData();
            WorldUtils.Gen(point, new Shapes.Rectangle(boxTopSize.X, boxTopSize.Y), Actions.Chain(
                new Modifiers.Offset(boxTopOffset.X, boxTopOffset.Y),
                new Actions.ClearTile(true),
                new Actions.Blank().Output(cabinShapeDataTop)
            ));
            WorldUtils.Gen(point, new ModShapes.InnerOutline(cabinShapeDataTop), Actions.Chain(
                new Modifiers.RectangleMask(-40, 40, boxTopCutoff, 0),
                new Actions.SetTile(TileID.WoodBlock),
                new Actions.SetFrames(true)
            ));

            // place walls part 1
            // just doing the bottom box rn
            WorldUtils.Gen(point, new ModShapes.All(cabinShapeDataBottom), Actions.Chain(
                new Modifiers.IsEmpty(),
                new Modifiers.Dither(0.3),
                new Actions.PlaceWall(WallID.Planked),
                new Actions.SetFrames(true)
            ));
            for (int i = 0; i < boxBottomSize.X - 2; i++)
            {
                tile = Main.tile[bottomLeft.X + 1 + i, boxTopFloor[0].Y + 1];
                tile.WallType = WallID.Wood;
                SquareWallFrame(bottomLeft.X + 1 + i, boxTopFloor[0].Y + 1);
            }

            // place blocks off the left and rightmost parts of the bottom box floor
            Point[] tilesToReplace =
            {
                new Point(bottomLeft.X - 1, bottomLeft.Y),              // left
                new Point(bottomLeft.X + boxBottomSize.X, bottomLeft.Y) // right
            };
            foreach (Point block in tilesToReplace)
            {
                tile = Main.tile[block.X, block.Y];
                tile.ClearTile();
                tile.HasTile = true;
                tile.TileType = TileID.WoodBlock;
                SquareTileFrame(block.X, block.Y);
            }

            // place platform and stairs
            int boxTopPlatformX;
            if (left)
                boxTopPlatformX = boxBottomFloorLeftmost;
            else
                boxTopPlatformX = boxBottomFloorRightmost;
            int boxTopPlatformLength = boxTopSize.X / 3;
            for (int i = 0; i < boxTopPlatformLength; i++)
            {
                tile = Main.tile[boxTopPlatformX + i * leftRightMultiplier, bottomLeft.Y - boxBottomSize.Y + 1];
                tile.ResetToType(TileID.Platforms);
                tile.TileFrameY = 0;
                SquareTileFrame(boxTopPlatformX + i * leftRightMultiplier, bottomLeft.Y - boxBottomSize.Y + 1);

                tile.WallType = WallID.Wood;
                SquareWallFrame(boxTopPlatformX + i * leftRightMultiplier, bottomLeft.Y - boxBottomSize.Y + 1);
            }
            // just kidding! kill that guy
            // we get rid of this platform to make my life easier in the next step
            tile = Main.tile[boxTopPlatformX, bottomLeft.Y - boxBottomSize.Y + 1];
            tile.HasTile = false;
            // ccr code cameo! place the stairs themselves
            currentTilePointer = new Point(boxTopPlatformX, bottomLeft.Y - boxBottomSize.Y + 1);
            while (!Main.tile[currentTilePointer].HasTile)
            {
                tile = Main.tile[currentTilePointer];
                tile.ResetToType(TileID.Platforms);
                if (left)
                    tile.BlockType = BlockType.SlopeDownLeft;
                else
                    tile.BlockType = BlockType.SlopeDownRight;
                if (left)
                    tile.TileFrameX = 10 * 18;
                else
                    tile.TileFrameX = 8 * 18;
                tile.TileFrameY = 0;
                currentTilePointer.X += 1 * leftRightMultiplier;
                currentTilePointer.Y++;
            }
            // dont forget the awkward guy
            tile = Main.tile[boxTopPlatformX, bottomLeft.Y - boxBottomSize.Y + 1];
            if (left)
                tile.TileFrameX = 26 * 18;
            else
                tile.TileFrameX = 25 * 18;

            // place bridge under our overhang, save positions for decorating later
            int bridgeLength = 8 + Main.rand.Next(0, 5);
            int bridgeClosest = left == true ? bottomLeft.X - 2 : bottomLeft.X + boxBottomSize.X + 1;
            List<Point> bridge = new List<Point>();
            for (int i = 0; i < bridgeLength; i++)
            {
                bridge.Add(new Point(bridgeClosest + (i * leftRightMultiplier * -1), bottomLeft.Y));
                tile = Main.tile[bridgeClosest + (i * leftRightMultiplier * -1), bottomLeft.Y];
                tile.ResetToType(TileID.Platforms);
                tile.TileFrameY = 0;
                SquareTileFrame(bridgeClosest + (i * leftRightMultiplier * -1), bottomLeft.Y);

                // kill blocks above bridge
                for (int ii = 1; ii < boxBottomSize.Y - 1; ii++)
                {
                    tile = Main.tile[bridgeClosest + (i * leftRightMultiplier * -1), bottomLeft.Y - ii];
                    tile.ClearTile();
                    SquareTileFrame(bridgeClosest + (i * leftRightMultiplier * -1), bottomLeft.Y - ii);
                }
            }
            // kill blocks in front of where the guranteed door will go
            for (int ii = 1; ii < boxBottomSize.Y - 1; ii++)
            {
                tile = Main.tile[bridgeClosest + leftRightMultiplier, bottomLeft.Y - ii];
                tile.ClearTile();
                SquareTileFrame(bridgeClosest + leftRightMultiplier, bottomLeft.Y - ii);
            }

            // place doors
            //TODO: clean this up, make less redundant
            tile = Main.tile[boxBottomFloorLeftmost - 3, bottomLeft.Y - 1];
            if (!tile.HasTile && (left || Main.rand.NextBool()))
            {
                for (int i = 1; i < 4; i++)
                {
                    tile = Main.tile[boxBottomFloorLeftmost - 2, bottomLeft.Y - i];
                    tile.ClearTile();
                    SquareTileFrame(boxBottomFloorLeftmost - 2, bottomLeft.Y - i);
                }
                PlaceTile(boxBottomFloorLeftmost - 2, bottomLeft.Y - 1, TileID.ClosedDoor, mute: true, forced: false, -1);
            }
            tile = Main.tile[boxBottomFloorRightmost + 3, bottomLeft.Y - 1];
            if (!tile.HasTile && (!left || Main.rand.NextBool()))
            {
                for (int i = 1; i < 4; i++)
                {
                    tile = Main.tile[boxBottomFloorRightmost + 2, bottomLeft.Y - i];
                    tile.ClearTile();
                    SquareTileFrame(boxBottomFloorRightmost + 2, bottomLeft.Y - i);
                }
                PlaceTile(boxBottomFloorRightmost + 2, bottomLeft.Y - 1, TileID.ClosedDoor, mute: true, forced: false, -1);
            }

            // place walls part 2
            // top box stuff
            for (int i = 0; i < boxTopSize.X - 2; i++)
            {
                tile = Main.tile[bottomLeft.X + boxTopOffset.X + 1 + i, boxTopFloor[0].Y - 1];
                tile.WallType = WallID.Planked;
                SquareWallFrame(bottomLeft.X + boxTopOffset.X + 1 + i, boxTopFloor[0].Y - 1);
            }
            for (int i = 0; i < 2; i++)
            {
                tile = Main.tile[bottomLeft.X + boxTopOffset.X, boxTopFloor[0].Y - i - (boxTopCutoff * -1) - 1];
                tile.WallType = WallID.WoodenFence;
                SquareWallFrame(bottomLeft.X + boxTopOffset.X, boxTopFloor[0].Y - i - (boxTopCutoff * -1) - 1);
                tile = Main.tile[bottomLeft.X + boxTopOffset.X + boxTopSize.X - 1, boxTopFloor[0].Y - i - (boxTopCutoff * -1) - 1];
                tile.WallType = WallID.WoodenFence;
                SquareWallFrame(bottomLeft.X + boxTopOffset.X + boxTopSize.X - 1, boxTopFloor[0].Y - i - (boxTopCutoff * -1) - 1);
            }
            //TODO: do wooden walls. unsure how? ask around? kill people? see what cobweb does?

            // place beams
            List<Point> beamPoints = new List<Point>();
            beamPoints.Add(new Point(bottomLeft.X + 1, bottomLeft.Y + 1));
            beamPoints.Add(new Point(bottomLeft.X + (boxBottomSize.X / 2), bottomLeft.Y + 1));
            beamPoints.Add(new Point(bottomLeft.X + boxBottomSize.X - 2, bottomLeft.Y + 1));
            foreach (Point block in beamPoints)
            {
                currentTilePointer = block;
                while (!Main.tile[currentTilePointer].HasTile)
                {
                    tile = Main.tile[currentTilePointer];
                    tile.ResetToType(TileID.WoodenBeam);
                    SquareTileFrame(currentTilePointer.X, currentTilePointer.Y);
                    currentTilePointer.Y++;
                }
                if (Main.tile[currentTilePointer].HasTile && Main.tile[currentTilePointer].Slope != SlopeType.Solid)
                {
                    tile = Main.tile[currentTilePointer];
                    tile.Slope = SlopeType.Solid;
                    SquareTileFrame(currentTilePointer.X, currentTilePointer.Y);
                }
            }
            currentTilePointer = bridge.Last();
            currentTilePointer.Y--;
            tile = Main.tile[currentTilePointer];
            while (!tile.HasTile || tile.TileType == TileID.Platforms)
            {
                tile.WallType = WallID.WoodenFence;
                SquareWallFrame(currentTilePointer.X, currentTilePointer.Y);
                currentTilePointer.Y++;
                tile = Main.tile[currentTilePointer];
            }
            if (Main.tile[currentTilePointer].HasTile && Main.tile[currentTilePointer].Slope != SlopeType.Solid)
            {
                tile = Main.tile[currentTilePointer];
                tile.Slope = SlopeType.Solid;
                SquareTileFrame(currentTilePointer.X, currentTilePointer.Y);
            }

            // place the chest
            bool hasPlacedChest = false;
            bool bridgeOrHouse = Main.rand.NextBool();
            int placeChestAttempts = 0;
            while (!hasPlacedChest)
            {
                placeChestAttempts++;
                if (placeChestAttempts > 5000)
                    break;

                Point randomPoint = new Point();
                if (bridgeOrHouse)
                    randomPoint = bridge[Main.rand.Next(bridge.Count())];
                else
                    randomPoint = boxBottomFloor[Main.rand.Next(boxBottomFloor.Count())];
                randomPoint.Y--;

                // ensuring space
                if (!Main.tile[randomPoint.X - 1, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + 2, randomPoint.Y].HasTile)
                {
                    // if on the bridge we want it to not veer too close to the edge
                    if (!bridgeOrHouse || (Main.tile[randomPoint.X - 1, randomPoint.Y + 1].HasTile && Main.tile[randomPoint.X + 2, randomPoint.Y + 1].HasTile) )
                    {
                        int chest = PlaceChest(randomPoint.X, randomPoint.Y, style: 0);
                        if (chest != -1)
                            hasPlacedChest = true;
                    }
                }
            }

            // place the sign
            Point signPoint = new Point();
            signPoint.X = boxTopBottomLeft;
            if (left)
            {
                signPoint.X += 1;
            }
            else
            {
                signPoint.X += boxTopSize.X;
                signPoint.X -= 3;
            }
            signPoint.Y = bottomLeft.Y - boxBottomSize.Y + 1;
            signPoint.Y += 1;
            PlaceObject(signPoint.X, signPoint.Y, TileID.Signs);

            #region Decorum
            // place crates on platform
            // but first, validation! since blocks are placed by the bottom left, we want to cull any points too close to the door
            // if a crate was placed there, itd block the door! and thatd suck
            // remove furthest as well bcuz it looks ugly when placed there
            List<Point> bridgeValidated = new List<Point>();
            for (int i = 0; i < bridge.Count(); i++)
            {
                tile = Main.tile[bridge[i].X + 1, bridge[i].Y];
                if (tile.TileType == TileID.WoodBlock || !tile.HasTile)
                    continue;
                    
                tile = Main.tile[bridge[i].X - 1, bridge[i].Y];
                if (tile.TileType == TileID.WoodBlock || !tile.HasTile)
                    continue;

                bridgeValidated.Add(bridge[i]);
            }
            int crateAmount = Main.rand.Next(0, 5);
            int placeCrateAttempts = 0;
            while (crateAmount > 0)
            {
                placeCrateAttempts++;
                if (placeCrateAttempts > 100)
                    break;
                
                Point randomPoint = bridgeValidated[Main.rand.Next(bridgeValidated.Count())];
                randomPoint.Y--;
                if (CheckIfTileCanBePlaced(randomPoint, 2, 2))
                {
                    // can we place 3 crates? if so, do
                    if (crateAmount >= 3 && placeCrateAttempts < 75)
                    {
                        if (MinistructureList.CrateStack.Check(randomPoint) && !Main.tile[randomPoint.X - 1, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + MinistructureList.CrateStack.width, randomPoint.Y].HasTile)
                        {
                            if (Main.tile[randomPoint.X - 1, randomPoint.Y + 1].HasTile && Main.tile[randomPoint.X + MinistructureList.CrateStack.width, randomPoint.Y + 1].HasTile)
                            {
                                MinistructureList.CrateStack.Place(randomPoint);
                                crateAmount -= 3;
                            }
                        }
                    }
                    else
                    {
                        if (!Main.tile[randomPoint.X - 1, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + 2, randomPoint.Y].HasTile)
                        {
                            if (Main.tile[randomPoint.X - 1, randomPoint.Y + 1].HasTile && Main.tile[randomPoint.X + 2, randomPoint.Y + 1].HasTile)
                            {
                                PlaceTile(randomPoint.X, randomPoint.Y, TileID.FishingCrate);
                                crateAmount--;
                            }
                        }
                    }
                }
            }

            // oh boy now heres the fun one
            // furnish the area with furniture and shit
            // bottom floor
            int decorAmount = Main.rand.Next(2, 4);
            if (!bridgeOrHouse)
                decorAmount--;
            int placeDecorAttempts = 0;
            List<Ministructure> structuresToPlace = new List<Ministructure>();
            structuresToPlace.Add(MinistructureList.SpearRack);
            structuresToPlace.Add(MinistructureList.TableWithCandle);
            structuresToPlace.Add(MinistructureList.GrandfatherClock);
            structuresToPlace.Add(MinistructureList.LargeFurnitureRubble);
            while (decorAmount > 0)
            {
                placeDecorAttempts++;
                if (placeDecorAttempts > 5000)
                    break;

                int randomMinistructure = Main.rand.Next(structuresToPlace.Count);
                Point randomPoint = boxBottomFloor[Main.rand.Next(boxBottomFloor.Count())];
                randomPoint.Y--;
                Ministructure randomStructure = structuresToPlace[randomMinistructure];
                if (randomStructure.Check(randomPoint))
                {
                    // ensuring space
                    if (!Main.tile[randomPoint.X - 1, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + randomStructure.width, randomPoint.Y].HasTile)
                    {
                        randomStructure.Place(randomPoint);
                        structuresToPlace.Remove(randomStructure);
                        decorAmount--;
                    }
                }
            }
            // top floor
            decorAmount = Main.rand.Next(2, 4);
            placeDecorAttempts = 0;
            structuresToPlace = new List<Ministructure>();
            structuresToPlace.Add(MinistructureList.TableWithCandle);
            structuresToPlace.Add(MinistructureList.LargeFishingNet);
            structuresToPlace.Add(MinistructureList.LargeFishingNet);
            structuresToPlace.Add(MinistructureList.LargeFishingNet);
            structuresToPlace.Add(MinistructureList.CrateStack);
            structuresToPlace.Add(MinistructureList.LargeFurnitureRubble);
            while (decorAmount > 0)
            {
                placeDecorAttempts++;
                if (placeDecorAttempts > 5000)
                    break;

                int randomMinistructure = Main.rand.Next(structuresToPlace.Count);
                Point randomPoint = boxTopFloor[Main.rand.Next(boxTopFloor.Count())];
                randomPoint.Y--;
                Ministructure randomStructure = structuresToPlace[randomMinistructure];
                if (randomStructure.Check(randomPoint))
                {
                    // ensuring space
                    if (!Main.tile[randomPoint.X - 1, randomPoint.Y].HasTile && !Main.tile[randomPoint.X + randomStructure.width, randomPoint.Y].HasTile)
                    {
                        randomStructure.Place(randomPoint);
                        structuresToPlace.Remove(randomStructure);
                        decorAmount--;

                        // if we place the net, remove the net instances
                        // this sucks, but it works, so i dont care
                        if (randomStructure == MinistructureList.LargeFishingNet)
                        {
                            structuresToPlace.Remove(randomStructure);
                            structuresToPlace.Remove(randomStructure);
                            structuresToPlace.Remove(randomStructure);
                        }
                    }
                }
            }

            // now the shitty one uhghhhhhhhh
            // placing small piles (FUCK LARGE PILES!!! GRAAASGHHHHHHH)
            decorAmount = Main.rand.Next(4, 7);
            placeDecorAttempts = 0;
            while (decorAmount > 0)
            {
                placeDecorAttempts++;
                if (placeDecorAttempts > 2500)
                    break;

                Point randomPoint = allBoxFloors[Main.rand.Next(allBoxFloors.Count())];
                randomPoint.Y--;

                int randomStyleMin = 1;
                int randomStyleMax = 1;
                int horizRow = 1;
                switch(Main.rand.Next(0, 5))
                {
                    case 0: // broken furniture
                        randomStyleMin = 31;
                        randomStyleMax = 34;
                        horizRow = 1;
                        break;
                    case 1: // small bones
                        randomStyleMin = 12;
                        randomStyleMax = 20;
                        horizRow = 0;
                        break;
                    case 2: // big boned
                        randomStyleMin = 6;
                        randomStyleMax = 11;
                        horizRow = 1;
                        break;
                    case 3: // small rocks
                        randomStyleMin = 0;
                        randomStyleMax = 6;
                        horizRow = 0;
                        break;
                    case 4: // equipment
                        randomStyleMin = 28;
                        randomStyleMax = 36;
                        horizRow = 0;
                        break;
                }

                if (WorldGen.PlaceSmallPile(randomPoint.X, randomPoint.Y, Main.rand.Next(randomStyleMin, randomStyleMax), horizRow, 185))
                    decorAmount--;
            }
            // and finally the bridge
            decorAmount = Main.rand.Next(1, 4);
            placeDecorAttempts = 0;
            while (decorAmount > 0)
            {
                placeDecorAttempts++;
                if (placeDecorAttempts > 2500)
                    break;

                Point randomPoint = bridgeValidated[Main.rand.Next(bridgeValidated.Count())];
                randomPoint.Y--;
                int randomStyleMin = 28;
                int randomStyleMax = 36;
                int horizRow = 0;

                if (WorldGen.PlaceSmallPile(randomPoint.X, randomPoint.Y, Main.rand.Next(randomStyleMin, randomStyleMax), horizRow, 185))
                    decorAmount--;
            }
            #endregion
        }

        public bool PlaceStupidSpiritModThing(Point origin, StructureMap structures)
        {
            //TODO: if the structure spawns completely encased in blocks, no trees generate. unsure the cause
            //      addendum: only sometimes???????? whats going onnn
            //TODO: trees (and grass ig) can generate on top of the cavern. if a cave genned with space above it, those trees would grow!
            //      don't rly wanna fix this bcuz its funny imo, but can do if desired
            //TODO: if vines spawn in the air, they dont get their frames set properly! ahhh! aaaaaahhhhhhh! 
            //TODO: is checking far top/bottom tiles and placing them for waterfall necesarry? assess
            //TODO: is consistent cherry tree placement desirable? relies completely on randomness. roll for certain amount of trees
            //      to be placed like the waterfalls? spread them apart from each other?
            //TODO: guarantee monolith generation after enough fails? place pedestal in center of structure, like enchanted sword shrine?
            //      a lot of work for almost no payoff, since it not genning feels like an extreme statistical anomaly. current gen is just
            //      rly condusive for it to be placed right
            //TODO: swap main.rand to worldgen.rand when the time is right
            //TODO: does the butterfly tile critter spawning radius have to be increased to compensate for bigger chamber?
            //TODO: vines can generate outside of the structure. can fix, but should i bother? could create some interesting
            //      divergent visual if left in...
            //TODO: waterfalls can generate facing the outside of the structure, same thoughts on this as above

            ShapeData slimeShapeData = new ShapeData();
            ShapeData sideCarversShapeData = new ShapeData();
            Point point = new Point(origin.X, origin.Y + 20);
            float xScale = 0.8f + Main.rand.NextFloat() * 0.25f; // Randomize the width of the shrine area

            // Create a masking layer for the cavern, so the walls tilt inwards while going up
            // The masking layer is comprised of two circles, offset left and right respectively
            int maskOffset = 30;
            WorldUtils.Gen(point, new Shapes.Circle((int)xScale + 15), Actions.Chain(
                new Modifiers.Offset(maskOffset, -10), 
                new Actions.Blank().Output(sideCarversShapeData)
            ));
            WorldUtils.Gen(point, new Shapes.Circle((int)xScale + 15), Actions.Chain(
                new Modifiers.Offset(-maskOffset, -10), 
                new Actions.Blank().Output(sideCarversShapeData)
            ));

            // Using the Slime shape, clear out tiles. Accomodate for the side carvers mask, to create a nice bell shape
            WorldUtils.Gen(point, new Shapes.Slime(20, xScale, 1f), Actions.Chain(
                new Modifiers.NotInShape(sideCarversShapeData), 
                new Modifiers.Blotches(2, 0.4), 
                new Actions.ClearTile(frameNeighbors: true).Output(slimeShapeData)
            ));

            #region Set Dressing
            // Place grass along the inner outline of the cavern shape
            WorldUtils.Gen(point, new ModShapes.InnerOutline(slimeShapeData), Actions.Chain(
                new Actions.SetTile(TileID.Grass),
                new Actions.SetFrames(frameNeighbors: true)
            ));
            // Place waterfalls around the upper half of the cavern
            int waterfallCap = Main.rand.Next(1, 3);
            int waterfallAmt = 0;
            WorldUtils.Gen(point, new ModShapes.InnerOutline(slimeShapeData), Actions.Chain(
                new Modifiers.OnlyTiles(TileID.Grass),
                new Modifiers.RectangleMask(-40, 40, -40, 0),
                new Actions.Custom((i, j, args) => {
                    if (Main.rand.NextBool(10))
                    {
                        if (waterfallAmt >= waterfallCap)
                            return true;
                        
                        // doing all our validation here, checking for three things...
                        // 1. if the block to the left/right is air (so we know what direction to face this fella in)
                        // 2. if there is no liquid where the water will be (to prevent duplicates)
                        if (!Main.tile[i + 1, j].HasTile && Main.tile[i - 1, j].LiquidAmount == 0)
                        {
                            PlaceWaterfall(i, j, true);
                            waterfallAmt++;
                        }
                        else if (!Main.tile[i - 1, j].HasTile && Main.tile[i + 1, j].LiquidAmount == 0)
                        {
                            PlaceWaterfall(i, j, false);
                            waterfallAmt++;
                        }
                    }
                    return true;
                })
            ));
            // Place Sakura trees on the ground wherever applicable 
            WorldUtils.Gen(point, new ModShapes.All(slimeShapeData), Actions.Chain(
                new Modifiers.OnlyTiles(TileID.Grass), 
                new Actions.Custom((i, j, args) => { 
                    if (Main.rand.NextBool())
                        GrowTreeWithSettings(i, j, GrowTreeSettings.Profiles.VanityTree_Sakura);
                    return true; })
            ));
            // Place Flower wall on all cavern shape coordinates. Place flower vines 1 tile below all grass tiles of the cavern
            WorldUtils.Gen(point, new ModShapes.All(slimeShapeData), Actions.Chain(
                new Actions.PlaceWall(WallID.Flower),
                new Modifiers.RectangleMask(-40, 40, -40, -5),
                new Modifiers.OnlyTiles(TileID.Grass), 
                new Modifiers.Offset(0, 1), 
                new ActionVines(0, 12, 382)
            ));
            // Place grass and flowers above grass tiles in the cavern
            WorldUtils.Gen(point, new ModShapes.All(slimeShapeData), Actions.Chain(
                new Modifiers.Offset(0, -1), 
                new Modifiers.OnlyTiles(TileID.Grass), 
                new Modifiers.Offset(0, -1), 
                new ActionGrass()
                ));
            #endregion

            #region Monolith
            // Add the extremely important Monolith 
            bool placedMonolith = false;
            int placedMonolithAttempts = 0;
            while (!placedMonolith)
            {
                placedMonolithAttempts++;
                if (placedMonolithAttempts > 5000)
                    break;

                int randomX = Main.rand.Next(point.X - 8, point.X + 8);
                int randomY = Main.rand.Next(point.Y, point.Y + 12);
                PlaceTile(randomX, randomY, TileID.VoidMonolith, mute: true, forced: false, -1);
                placedMonolith = Main.tile[randomX, randomY].TileType == TileID.VoidMonolith;
            }
            // if this doesn't work, increase the range we search for a spot at
            if (placedMonolithAttempts < 15000)
            {
                while (!placedMonolith)
                {
                    placedMonolithAttempts++;

                    int randomX = Main.rand.Next(point.X - 16, point.X + 16);
                    int randomY = Main.rand.Next(point.Y, point.Y + 14);
                    PlaceTile(randomX, randomY, TileID.VoidMonolith, mute: true, forced: false, -1);
                    placedMonolith = Main.tile[randomX, randomY].TileType == TileID.VoidMonolith;
                }
            }
            // and if everything fails, give up
            else if (placedMonolithAttempts >= 15000)
            {
                //System.Diagnostics.Debug.WriteLine("Monolith could not be placed! The statistically impossible has been possed!");
            }
            #endregion

            return true;

            /*
             * og code
            // By using TileScanner, check that the 50x50 area centered around the origin is mostly Dirt or Stone
            Dictionary<ushort, int> tileDictionary = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(TileID.Dirt, TileID.Stone).Output(tileDictionary));
            if (tileDictionary[TileID.Dirt] + tileDictionary[TileID.Stone] < 1250)
                return false; // If not, return false, which will cause the calling method to attempt a different origin

            Point surfacePoint;
            // Search up to 1000 tiles above for an area 50 tiles tall and 1 tile wide without a single solid tile. Basically find the surface.
            bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Up(1000), new Conditions.IsSolid().AreaOr(1, 50).Not()), out surfacePoint);
            // Search from the orgin up to the surface and make sure no sand is between origin and surface
            if (WorldUtils.Find(origin, Searches.Chain(new Searches.Up(origin.Y - surfacePoint.Y), new Conditions.IsTile(TileID.Sand)), out Point _))
                return false;

            if (!flag)
                return false;

            surfacePoint.Y += 50; // Adjust result to point to surface, not 50 tiles above
            ShapeData slimeShapeData = new ShapeData();
            ShapeData moundShapeData = new ShapeData();
            Point point = new Point(origin.X, origin.Y + 20);
            Point point2 = new Point(origin.X, origin.Y + 30);
            float xScale = 0.8f + Main.rand.NextFloat() * 0.5f; // Randomize the width of the shrine area
                                                                      // Check that the StructureMap doesn't have any existing conflicts for the intended area we wish to place the shrine.
            if (!structures.CanPlace(new Rectangle(point.X - (int)(20f * xScale), point.Y - 20, (int)(40f * xScale), 40)))
                return false;
            // Check that the StructureMap doesn't have any existing conflicts for the shaft leading to the surface
            if (!structures.CanPlace(new Rectangle(origin.X, surfacePoint.Y + 10, 1, origin.Y - surfacePoint.Y - 9), 2))
                return false;
            // Using the Slime shape, clear out tiles. Blotches gives the edges a more organic look. https://i.imgur.com/WtZaBbn.png
            WorldUtils.Gen(point, new Shapes.Slime(20, xScale, 1f), Actions.Chain(new Modifiers.Blotches(2, 0.4), new Actions.ClearTile(frameNeighbors: true).Output(slimeShapeData)));
            // Place a dirt mound within the cut out slime shape
            WorldUtils.Gen(point2, new Shapes.Mound(14, 14), Actions.Chain(new Modifiers.Blotches(2, 1, 0.8), new Actions.SetTile(TileID.Dirt), new Actions.SetFrames(frameNeighbors: true).Output(moundShapeData)));
            // Remove the mound coordinates from the slime coordinate data
            slimeShapeData.Subtract(moundShapeData, point, point2);
            // Place grass along the inner outline of the slime coordinate data
            WorldUtils.Gen(point, new ModShapes.InnerOutline(slimeShapeData), Actions.Chain(new Actions.SetTile(TileID.Grass), new Actions.SetFrames(frameNeighbors: true)));
            // Place water in empty coordinates in the bottom half of the slime shape
            WorldUtils.Gen(point, new ModShapes.All(slimeShapeData), Actions.Chain(new Modifiers.RectangleMask(-40, 40, 0, 40), new Modifiers.IsEmpty(), new Actions.SetLiquid()));
            // Place Flower wall on all slime shape coordinates. Place vines 1 tile below all grass tiles of the slime shape.
            WorldUtils.Gen(point, new ModShapes.All(slimeShapeData), Actions.Chain(new Actions.PlaceWall(WallID.Flower), new Modifiers.OnlyTiles(TileID.Grass), new Modifiers.Offset(0, 1), new ActionVines(3, 5)));
            // Remove tiles to create shaft to surface. Convert Sand tiles along shaft to hardened sand tiles.
            ShapeData shaftShapeData = new ShapeData();
            WorldUtils.Gen(new Point(origin.X, surfacePoint.Y + 10), new Shapes.Rectangle(1, origin.Y - surfacePoint.Y - 9), Actions.Chain(new Modifiers.Blotches(2, 0.2), new Actions.ClearTile().Output(shaftShapeData), new Modifiers.Expand(1), new Modifiers.OnlyTiles(TileID.Sand), new Actions.SetTile(TileID.HardenedSand).Output(shaftShapeData)));
            WorldUtils.Gen(new Point(origin.X, surfacePoint.Y + 10), new ModShapes.All(shaftShapeData), new Actions.SetFrames(frameNeighbors: true));
            // 33% chance to place an enchanted sword shrine tile
            if (Main.rand.NextBool(3))
                WorldGen.PlaceTile(point2.X, point2.Y - 15, TileID.LargePiles2, mute: true, forced: false, -1, 17);
            else
                WorldGen.PlaceTile(point2.X, point2.Y - 15, TileID.LargePiles, mute: true, forced: false, -1, 15);
            // Place plants above grass tiles in the mound shape.
            WorldUtils.Gen(point2, new ModShapes.All(moundShapeData), Actions.Chain(new Modifiers.Offset(0, -1), new Modifiers.OnlyTiles(TileID.Grass), new Modifiers.Offset(0, -1), new ActionGrass()));
            // Add to the StructureMap to prevent other worldgen from intersecting this area.
            structures.AddStructure(new Rectangle(point.X - (int)(20f * xScale), point.Y - 20, (int)(40f * xScale), 40), 4);
            return true;
            */
        }

        public void PlaceWaterfall(int x, int y, bool leftIndent)
        {
            PoundTile(x, y);

            // making an array with all the points we want to check for blocks before placing water
            // the x is always positive so we can left/rightshift it later based on waterfall direction
            Point[] tileCheckOffsets =
            {
                new Point(2, -1), // far top
                new Point(1, -1), // middle top
                new Point(0, -1), // near top
                new Point(2, 0),  // far middle
                new Point(2, 1),  // far bottom
                new Point(1, 1),  // middle bottom
                new Point(0, 1)   // near bottom

            };

            // iterate through our array and take care of any blocks that need taking care of
            Tile tile = new Tile();
            for (int i = 0; i < tileCheckOffsets.Count(); i++)
            {
                int horizOffset = leftIndent ? tileCheckOffsets[i].X * -1 : tileCheckOffsets[i].X;
                horizOffset += x;
                int vertOffset = tileCheckOffsets[i].Y + y;

                tile = Main.tile[horizOffset, vertOffset];
                if (!tile.HasTile)
                {
                    tile.HasTile = true;
                    tile.TileType = TileID.Grass;
                    tile.WallType = WallID.Flower;
                    SquareTileFrame(horizOffset, vertOffset);
                }
            }

            // now we handle placing the water
            int waterHorizOffset = leftIndent ?  -1 : 1;
            waterHorizOffset += x;
            tile = Main.tile[waterHorizOffset, y];
            if (tile.HasTile)
                tile.HasTile = false;
            tile.LiquidType = LiquidID.Water;
            tile.LiquidAmount = 255;
            tile.WallType = WallID.Flower;
        }
    }

    public static class MinistructureList
    {
        public static SpearRackStructure SpearRack = new SpearRackStructure();
        public static CrateStackStructure CrateStack = new CrateStackStructure();
        public static TableWithCandleStructure TableWithCandle = new TableWithCandleStructure();
        public static LargeFishingNetStructure LargeFishingNet = new LargeFishingNetStructure();
        public static GrandfatherClockStructure GrandfatherClock = new GrandfatherClockStructure();
        public static LargeFurnitureRubbleStructure LargeFurnitureRubble = new LargeFurnitureRubbleStructure();
        public static CampfireDisabledStructure CampfireDisabled = new CampfireDisabledStructure();
        public static TentRubbleStructure TentRubble = new TentRubbleStructure();
        public static UndergroundObjectRubbleStructure UndergroundObjectRubble = new UndergroundObjectRubbleStructure();
        public static LargeCampStructure LargeCamp = new LargeCampStructure();
        public static SmallCoinPileStructure SmallCoinPile = new SmallCoinPileStructure();
        public static LargeCoinPileStructure LargeCoinPile = new LargeCoinPileStructure();
        public static SmallCoinStashStructure SmallCoinStash = new SmallCoinStashStructure();
        public static LargeCoinStashStructure LargeCoinStash = new LargeCoinStashStructure();
        public static LargeStoneRubbleSpecialAndChestStructure LargeStoneRubbleSpecialAndChest = new LargeStoneRubbleSpecialAndChestStructure();
        public static FishingWallStructure FishingWall = new FishingWallStructure();
        public static WoodenLamppostStructure WoodenLamppost = new WoodenLamppostStructure();

        public class SpearRackStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 3;
            public override int type => TileID.Painting3X3;
            public override int style => 44;

            public override void Place(Point bottomLeft)
            {
                Point origin = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Tile tile = Main.tile[bottomLeft.X + x, bottomLeft.Y - y];
                        tile.WallType = WallID.Planked;
                        SquareWallFrame(bottomLeft.X + x, bottomLeft.Y - y);
                    }
                }
                PlaceObject(origin.X, origin.Y, type, style: style);
            }
        }
        public class CrateStackStructure : Ministructure
        {
            public override int width => 4;
            public override int height => 4;
            public override int type => TileID.FishingCrate;

            public override void Place(Point bottomLeft)
            {
                Point crate2 = bottomLeft;
                crate2.X += 2;
                Point crate3 = bottomLeft;
                crate3.X += 1;
                crate3.Y -= 2;

                PlaceTile(bottomLeft.X, bottomLeft.Y, TileID.FishingCrate);
                PlaceTile(crate2.X, crate2.Y, TileID.FishingCrate);
                PlaceObject(crate3.X, crate3.Y, TileID.FishingCrate);
            }

            public override bool Check(Point bottomLeft)
            {
                Point crate2 = bottomLeft;
                crate2.X += 2;
                Point crate3 = bottomLeft;
                crate3.X += 1;
                crate3.Y -= 2;

                if (CheckIfTileCanBePlaced(bottomLeft, 2, 2) && CheckIfTileCanBePlaced(crate2, 2, 2) && CheckIfTileCanBePlaced(crate3, 2, 2, checkFloor: false))
                    return true;

                return false;
            }
        }
        public class TableWithCandleStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 3;
            public override int type => TileID.Tables;
            public override void Place(Point bottomLeft)
            {
                PlaceObject(bottomLeft.X + width - 2, bottomLeft.Y, TileID.Tables);
                int offset = Main.rand.Next(0, 3);
                PlaceObject(bottomLeft.X + offset, bottomLeft.Y - 2, TileID.Candles);
                Tile tile = Main.tile[bottomLeft.X + offset, bottomLeft.Y - 2];
                tile.TileFrameX = 18;
            }
        }
        public class LargeFishingNetStructure : Ministructure
        {
            public override int width => 6;
            public override int height => 4;

            public override void Place(Point bottomLeft)
            {
                Tile tile;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        tile = Main.tile[bottomLeft.X + x, bottomLeft.Y - y];
                        // left/right sticks
                        if (x == 0 || x == width - 1)
                        {
                            if (y == height - 1)
                            {
                                tile.WallType = WallID.WoodenFence;
                                SquareWallFrame(bottomLeft.X + x, bottomLeft.Y - y);
                            }
                            else
                            {
                                tile.HasTile = true;
                                tile.TileType = TileID.WoodenBeam;
                                SquareTileFrame(bottomLeft.X + x, bottomLeft.Y - y);
                            }
                        }
                        // top rope
                        if (y == height - 1)
                        {
                            tile.HasTile = true;
                            tile.TileType = TileID.Rope;
                            SquareTileFrame(bottomLeft.X + x, bottomLeft.Y - y);
                        }
                        // middle rope
                        if (x != 0 && x != width - 1)
                        {
                            if (y == height - 2)
                            {
                                tile.HasTile = true;
                                tile.TileType = TileID.Rope;
                                SquareTileFrame(bottomLeft.X + x, bottomLeft.Y - y);
                            }
                        }
                    }
                }
                // lower rope
                bool left = Main.rand.NextBool();
                Point point = new Point(bottomLeft.X + 1, bottomLeft.Y - 1);
                if (!left)
                    point.X++;
                for (int i = 0; i < 3; i++)
                {
                    tile = Main.tile[point.X + i, point.Y];
                    tile.HasTile = true;
                    tile.TileType = TileID.Rope;
                    SquareTileFrame(point.X + i, point.Y);
                }
            }
        }
        public class GrandfatherClockStructure : Ministructure
        {
            public override int width => 2;
            public override int height => 3;
            public override int type => TileID.GrandfatherClocks;
        }
        public class LargeFurnitureRubbleStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 2;
            public override int type => TileID.LargePiles;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(22, 26);
                Point point = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                point.Y++;
                PlaceObject(point.X, point.Y, type, style: style);
            }
        }
        public class CampfireDisabledStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 2;
            public override int type => TileID.Campfire;
            public override void Place(Point bottomLeft)
            {
                Point campfireSpot = new Point(bottomLeft.X + 1, bottomLeft.Y);
                PlaceObject(campfireSpot.X, campfireSpot.Y, TileID.Campfire, mute: true);
                Tile tile = new Tile();
                tile = Main.tile[campfireSpot.X, campfireSpot.Y];
                tile.TileFrameY += 36;
                tile = Main.tile[campfireSpot.X - 1, campfireSpot.Y];
                tile.TileFrameY += 36;
                tile = Main.tile[campfireSpot.X + 1, campfireSpot.Y];
                tile.TileFrameY += 36;
                tile = Main.tile[campfireSpot.X, campfireSpot.Y - 1];
                tile.TileFrameY += 36;
                tile = Main.tile[campfireSpot.X - 1, campfireSpot.Y - 1];
                tile.TileFrameY += 36;
                tile = Main.tile[campfireSpot.X + 1, campfireSpot.Y - 1];
                tile.TileFrameY += 36;
            }
        }
        public class TentRubbleStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 2;
            public override int type => TileID.LargePiles2;
            public override int style =>  26;
            public override void Place(Point bottomLeft)
            {
                Point point = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                point.Y++;
                PlaceObject(point.X, point.Y, type, style: style);
            }
        }
        public class UndergroundObjectRubbleStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 2;
            public override int type => TileID.LargePiles;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(23, 29);
                Point point = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                point.Y += 1;
                PlaceObject(point.X, point.Y, type, style: style);
            }
        }
        public class LargeCampStructure : Ministructure
        {
            public override int width => 7;
            public override int height => 2;
            public override int type => TileID.Campfire;
            public override int style => 26;
            public override void Place(Point bottomLeft)
            {
                bool left = Main.rand.NextBool();
                Point point = bottomLeft;
                if (left)
                {
                    CampfireDisabled.Place(point);
                    TentRubble.Place(new Point(point.X + TentRubble.width + 1, point.Y));
                }
                else
                {
                    CampfireDisabled.Place(new Point(point.X + CampfireDisabled.width + 1, point.Y));
                    TentRubble.Place(point);
                }
            }
        }
        public class SmallCoinPileStructure : Ministructure
        {
            public override int width => 2;
            public override int height => 2;
            public override int type => TileID.CopperCoinPile;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(0, 3);
                List<Point> listOfBlocksToPlace = new List<Point>();
                listOfBlocksToPlace.Add(new Point(bottomLeft.X, bottomLeft.Y));
                listOfBlocksToPlace.Add(new Point(bottomLeft.X + 1, bottomLeft.Y));
                if (Main.rand.NextBool(3))
                    listOfBlocksToPlace.Add(new Point(bottomLeft.X + Main.rand.Next(0, 2), bottomLeft.Y - 1));
                foreach (Point point in listOfBlocksToPlace)
                {
                    Main.tile[point].ResetToType((ushort)(type + style));
                    SquareTileFrame(point.X, point.Y);
                }
            }
        }
        public class LargeCoinPileStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 3;
            public override int type => TileID.CopperCoinPile;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(0, 3);
                List<Point> listOfBlocksToPlace = new List<Point>();
                listOfBlocksToPlace.Add(new Point(bottomLeft.X, bottomLeft.Y));
                listOfBlocksToPlace.Add(new Point(bottomLeft.X + 1, bottomLeft.Y));
                listOfBlocksToPlace.Add(new Point(bottomLeft.X + 2, bottomLeft.Y));
                listOfBlocksToPlace.Add(new Point(bottomLeft.X + 1, bottomLeft.Y - 1));
                listOfBlocksToPlace.Add(new Point(bottomLeft.X + Main.rand.Next(0, 3), bottomLeft.Y - 1)); // intentional
                if (Main.rand.NextBool())
                    listOfBlocksToPlace.Add(new Point(bottomLeft.X + 1, bottomLeft.Y - 2));
                foreach (Point point in listOfBlocksToPlace)
                {
                    Main.tile[point].ResetToType((ushort)(type + style));
                    SquareTileFrame(point.X, point.Y);
                }
            }
        }
        public class SmallCoinStashStructure : Ministructure
        {
            public override int width => 2;
            public override int height => 1;
            public override int type => TileID.SmallPiles;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(16, 19);
                PlaceSmallPile(bottomLeft.X, bottomLeft.Y, style, 1);
            }
        }
        public class LargeCoinStashStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 2;
            public override int type => TileID.LargePiles;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(16, 22);
                Point point = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                point.Y += 1;
                PlaceObject(point.X, point.Y, type, style: style);
            }
        }
        public class LargeStoneRubbleSpecialAndChestStructure : Ministructure
        {
            public override int width => 3;
            public override int height => 2;
            public override int type => TileID.LargePiles;
            public override void Place(Point bottomLeft)
            {
                int style = Main.rand.Next(13, 15);
                if (Main.rand.NextBool(3))
                    style = 24;
                Point point = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                point.Y += 1;
                PlaceObject(point.X, point.Y, type, style: style);
            }
        }
        public class FishingWallStructure : Ministructure
        {
            public override int width => 7;
            public override int height => 5;
            public override int type => TileID.Painting3X3;
            public override int style => 44;
            public override void Place(Point bottomLeft)
            {
                bool left = Main.rand.NextBool();
                int beamOffsetX = left ? 0 : width - 1;

                // place beam
                for (int i = 0; i < height; i++)
                {
                    Main.tile[bottomLeft.X + beamOffsetX, bottomLeft.Y - i].ResetToType(TileID.WoodenBeam);
                    SquareTileFrame(bottomLeft.X + beamOffsetX, bottomLeft.Y - i);
                }

                // place wall
                for (int x = 0; x < width; x++)
                {
                    if (Main.tile[bottomLeft.X + x, bottomLeft.Y].TileType == TileID.WoodenBeam)
                        continue;

                    for (int y = 0; y < height; y++)
                    {
                        Point point = new Point(bottomLeft.X + x, bottomLeft.Y - y);
                        if (y == 0)
                            Main.tile[point].WallType = WallID.Planked;
                        else
                            Main.tile[point].WallType = WallID.Wood;
                        SquareWallFrame(point.X, point.Y);
                    }
                }

                // place rack
                Point origin = GetBottomLeftOfTileAccountingForOrigin(bottomLeft);
                int min = left ? 1 : 0;
                int max = left ? width - 2 : width - 3;
                int offset = Main.rand.Next(min, max);
                PlaceObject(origin.X + offset, origin.Y, type, style: style);
            }
        }
        public class WoodenLamppostStructure : Ministructure
        {
            public override int width => 1;
            public override int height => 6;
            public override int type => TileID.WoodenBeam;
            public override void Place(Point bottomLeft)
            {
                bool left = Main.rand.NextBool();
                int leftOffset = left ? -1 : 1;
                int extraHeight = Main.rand.Next(0, 4);
                int totalHeight = height + extraHeight;

                // place beam
                for (int i = 0; i < totalHeight; i++)
                {
                    Main.tile[bottomLeft.X, bottomLeft.Y - i].ResetToType(TileID.WoodenBeam);
                    SquareTileFrame(bottomLeft.X, bottomLeft.Y - i);
                }

                // place lamp part
                Main.tile[bottomLeft.X + leftOffset, bottomLeft.Y - totalHeight + 1].ResetToType(TileID.WoodBlock);
                SquareTileFrame(bottomLeft.X + leftOffset, bottomLeft.Y - totalHeight + 1);
                Main.tile[bottomLeft.X + (2 * leftOffset), bottomLeft.Y - totalHeight + 1].ResetToType(TileID.Platforms);
                SquareTileFrame(bottomLeft.X + (2 * leftOffset), bottomLeft.Y - totalHeight + 1);
                List<int> possibleLamps = new() { 1, 2, 3, 6 };
                int chosenLamp = Main.rand.Next(0, possibleLamps.Count);
                PlaceObject(bottomLeft.X + (2 * leftOffset), bottomLeft.Y - totalHeight + 2, TileID.HangingLanterns, style: possibleLamps[chosenLamp]);
            }
            public override bool Check(Point bottomLeft)
            {
                // check ground stuff
                if (!CheckIfTileCanBePlaced(bottomLeft, width, height))
                    return false;
                
                // check max possible beam
                int maxHeight = height + 3;
                for (int i = 0; i < maxHeight; i++)
                {
                    if (Main.tile[bottomLeft.X, bottomLeft.Y - i].HasTile)
                        return false;
                }

                // check all possible spots for the head
                for (int y = height; y < maxHeight - 1; y++)
                {
                    // sucks but w/e
                    if (Main.tile[bottomLeft.X - 1, bottomLeft.Y - y].HasTile)
                        return false;
                    if (Main.tile[bottomLeft.X - 2, bottomLeft.Y - y].HasTile)
                        return false;
                    if (Main.tile[bottomLeft.X - 2, bottomLeft.Y - y + 1].HasTile)
                        return false;
                    if (Main.tile[bottomLeft.X - 2, bottomLeft.Y - y + 2].HasTile)
                        return false;

                    if (Main.tile[bottomLeft.X + 1, bottomLeft.Y - y].HasTile)
                        return false;
                    if (Main.tile[bottomLeft.X + 2, bottomLeft.Y - y].HasTile)
                        return false;
                    if (Main.tile[bottomLeft.X + 2, bottomLeft.Y - y + 1].HasTile)
                        return false;
                    if (Main.tile[bottomLeft.X + 2, bottomLeft.Y - y + 2].HasTile)
                        return false;
                }

                return true;
            }
        }
    }

    public static class StructureHelpers
    {
        public abstract class Ministructure : ModSystem
        {
            public virtual int width { get; init; }
            public virtual int height { get; init; }
            public virtual int type { get; init; }
            public virtual int style => 0;

            public virtual void Place(Point bottomLeft)
            {
                PlaceObject(bottomLeft.X, bottomLeft.Y, type, style: style);
            }
            public virtual bool Check(Point bottomLeft)
            {
                return CheckIfTileCanBePlaced(bottomLeft, width, height);
            }
            public Point GetBottomLeftOfTileAccountingForOrigin(Point bottomLeft)
            {
                Point origin = new Point(TileObjectData.GetTileData(type, style).Origin.X, TileObjectData.GetTileData(type, style).Origin.Y);
                if (origin.X < 0)
                    origin.X = 0;
                if (origin.Y < 0)
                    origin.Y = 0;
                return new Point(bottomLeft.X + origin.X, bottomLeft.Y - origin.Y);
            }
        }

        // the vanilla system is disgusting so i made my own
        public static bool CheckIfTileCanBePlaced(Point bottomLeft, int width, int height, bool checkAir = true, bool checkFloor = true)
        {
            for (int i = 0; i <= width - 1; i++)
            {
                for (int ii = 0; ii <= height - 1; ii++)
                {
                    Point point = new Point(bottomLeft.X + i, bottomLeft.Y - ii);
                    Tile tile = Main.tile[point];

                    // check for any tiles in place of where we wanna place our thing
                    if (checkAir && tile.HasTile)
                        return false;
                    // check if the floor can't have stuff placed on it
                    if (checkFloor && ii == 0)
                    {
                        tile = Main.tile[point.X, point.Y + 1];
                        if (tile == null || !tile.HasUnactuatedTile)
                        {
                            if (Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                                return false;
                        }
                        if (tile.Slope != SlopeType.Solid)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
