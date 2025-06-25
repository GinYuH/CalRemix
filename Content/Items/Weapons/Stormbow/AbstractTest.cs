using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.WorldGen;
using static tModPorter.ProgressUpdate;

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

            Dust.NewDust(Main.MouseWorld, 1, 1, DustID.AmberBolt);
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

            PlaceOtherSpiritModThing(point);

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
                        while (!Main.tile[currentTilePointer.X, currentTilePointer.Y].HasTile)
                        {
                            tile = Main.tile[currentTilePointer.X, currentTilePointer.Y];
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
                        while (!Main.tile[currentTilePointer.X, currentTilePointer.Y].HasTile)
                        {
                            tile = Main.tile[currentTilePointer.X, currentTilePointer.Y];
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

        public void PlaceOtherSpiritModThing(Point origin)
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
            Point point = origin;
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

            // place blocks off the left and rightmost parts of the bottom box floor
            Point[] tilesToReplace =
            {
                new Point(bottomLeft.X - 1, bottomLeft.Y),              // left
                new Point(bottomLeft.X + boxBottomSize.X, bottomLeft.Y) // right
            };
            foreach (Point block in tilesToReplace)
            {
                tile = Main.tile[block.X, block.Y];
                tile.HasTile = true;
                tile.TileType = TileID.WoodBlock;
                SquareTileFrame(block.X, block.Y);
            }

            // place doors
            //TODO: clean this up, make less redundant
            if (left || Main.rand.NextBool())
            {
                for (int i = 1; i < 4; i++)
                {
                    tile = Main.tile[boxBottomFloorLeftmost - 2, bottomLeft.Y - i];
                    tile.HasTile = false;
                    SquareTileFrame(boxBottomFloorLeftmost - 2, bottomLeft.Y - i);
                }
                PlaceTile(boxBottomFloorLeftmost - 2, bottomLeft.Y - 1, TileID.ClosedDoor, mute: true, forced: false, -1);
            }
            if (!left || Main.rand.NextBool())
            {
                for (int i = 1; i < 4; i++)
                {
                    tile = Main.tile[boxBottomFloorRightmost + 2, bottomLeft.Y - i];
                    tile.HasTile = false;
                    SquareTileFrame(boxBottomFloorRightmost + 2, bottomLeft.Y - i);
                }
                PlaceTile(boxBottomFloorRightmost + 2, bottomLeft.Y - 1, TileID.ClosedDoor, mute: true, forced: false, -1);
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
                tile.TileType = TileID.Platforms;
                tile.TileFrameY = 0;
                SquareTileFrame(boxTopPlatformX + i * leftRightMultiplier, bottomLeft.Y - boxBottomSize.Y + 1);
            }
            // just kidding! kill that guy
            tile = Main.tile[boxTopPlatformX, bottomLeft.Y - boxBottomSize.Y + 1];
            tile.HasTile = false;
            // ccr code cameo!
            Point currentTilePointer = new Point(boxTopPlatformX, bottomLeft.Y - boxBottomSize.Y + 1);
            while (!Main.tile[currentTilePointer.X, currentTilePointer.Y].HasTile)
            {
                tile = Main.tile[currentTilePointer.X, currentTilePointer.Y];
                tile.HasTile = true;
                tile.TileType = TileID.Platforms;
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









            Point[] tilesToReplace2 =
            {
                new Point(0, 0)
            };
            foreach (Point block in tilesToReplace2)
            {
                Tile tile2;
                tile2 = Main.tile[block.X, block.Y];
                tile2.HasTile = true;
                tile2.TileType = TileID.Adamantite;
                SquareTileFrame(block.X, block.Y);
            }
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
                    ; return true;
                })
            ));
            // Place Sakura trees on the ground wherever applicable 
            WorldUtils.Gen(point, new ModShapes.All(slimeShapeData), Actions.Chain(
                new Modifiers.OnlyTiles(TileID.Grass), 
                new Actions.Custom((i, j, args) => { 
                    if (Main.rand.NextBool())
                        GrowTreeWithSettings(i, j, GrowTreeSettings.Profiles.VanityTree_Sakura);
                    ; return true; })
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
}
