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
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

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

            MakeAwesomeHouseStuff(point);
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
            int halfOfUsableAreaY = (int)(areaOfStuffYouCanUse.Height / 2);
            int middleHallHeight = areaOfStuffYouCanUse.Height / 2 - 2 + (int)Main.rand.Next(-3, 4);
            Point originWithPaddingAndProperPosition = new Point(originWithPadding.X, originWithPadding.Y + halfOfUsableAreaY - (middleHallHeight / 2));
            WorldUtils.Gen(originWithPaddingAndProperPosition, new Shapes.Rectangle(areaOfStuffYouCanUse.Width, middleHallHeight), new Actions.Blank().Output(roomShapeData));

            // to that, add some extra evil empty rectangles for more variety 
            for (int i = 0; i < 3; i++)
            {
                // get sizes
                Vector2 boxSize = Vector2.Zero;
                boxSize.X = (areaOfStuffYouCanUse.Width / 2) + Main.rand.Next(-3, 0); // + rand
                boxSize.Y = (areaOfStuffYouCanUse.Height / 4) + Main.rand.Next(-3, 0); // + rand

                // determine whether to place rect at top or not
                bool top = Main.rand.NextBool();
                Vector2 offset = Vector2.Zero;
                if (top)
                    offset.Y = -boxSize.Y;
                else
                    offset.Y = middleHallHeight;
                // and get horiz offset, anywhere on the hallway horizontally
                offset.X = Main.rand.Next(0, (int)(areaOfStuffYouCanUse.Width - boxSize.X));

                int tile = top ? TileID.BubblegumBlock : TileID.CactusBlock;

                WorldUtils.Gen(origin, new Shapes.Rectangle((int)boxSize.X, (int)boxSize.Y), Actions.Chain(new GenAction[]
                {
                    new Modifiers.Offset((int)offset.X, (int)offset.Y),
                    new Actions.Blank().Output(roomShapeData)
                }));
            }

            // make that shit
            WorldUtils.Gen(originWithPaddingAndProperPosition, new ModShapes.All(roomShapeData), new Actions.ClearTile());

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
                    if (Main.tile[x, y].TileType == (ushort)floorTile && Main.tile[x, y].TileType != (ushort)wallTile && !Main.tile[x - 1, y].HasTile)
                    {
                        Vector2 currentTilePointer = new Vector2(x - 1, y);
                        while (!Main.tile[(int)currentTilePointer.X, (int)currentTilePointer.Y].HasTile)
                        {
                            //8 left 10 right
                            WorldGen.PlaceTile((int)currentTilePointer.X, (int)currentTilePointer.Y, TileID.Adamantite);
                            //Main.tile[(int)currentTilePointer.X, (int)currentTilePointer.Y].TileFrameX = 8;
                            currentTilePointer = new Vector2(currentTilePointer.X - 1, currentTilePointer.Y + 1);
                        }
                    }
                    // ...and to the right
                    if (Main.tile[x, y].TileType == (ushort)floorTile && !Main.tile[x + 1, y].HasTile)
                    {

                    }
                }
            }

            // fill with decorum? or do this in a diff pass maybe?
            for (int x = areaOfStuffYouCanUse.X; x < areaOfStuffYouCanUse.X + areaOfStuffYouCanUse.Width; x++)
            {
                for (int y = areaOfStuffYouCanUse.Y; y < areaOfStuffYouCanUse.Y + areaOfStuffYouCanUse.Height; y++)
                {

                }
            }
        }
    }
}
