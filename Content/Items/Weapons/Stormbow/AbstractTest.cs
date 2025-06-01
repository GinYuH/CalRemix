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
            //makeSolidTunnel(mouseWorld.X, mouseWorld.Y, TileID.Mythril, 0, 0, 1, 10);
            //Main.tile[(int)mouseWorld.X, (int)mouseWorld.Y].TileType = TileID.Mythril;
            //WorldGen.PlaceTile((int)mouseWorld.X, (int)mouseWorld.Y, TileID.Mythril);
            //WorldUtils.Gen(point, new Shapes.Mound(5, 5), new Actions.PlaceTile(TileID.PineTree));
            int trunkHeightAmount = 10;
            int individualHeight = 5;
            makeChristmasTree(point, trunkHeightAmount, individualHeight);

            return null;
        }

        public static Vector2D makeSolidTunnel(double X, double Y, int Type, double xDir, double yDir, int Steps, int Size)
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
        public static void makeChristmasTree(Point location, int trunkHeightAmount, int individualHeight)
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
    }
}
