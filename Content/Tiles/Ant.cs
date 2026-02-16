using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public class Ant : ModTile
    {

        public enum AntDirectionType
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,
        }

        public static int antDirection = 0;

        public static SoundStyle Sonar = new SoundStyle("CalRemix/Assets/Sounds/Sonar");


        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AnimationFrameHeight = 16;
            AddMapEntry(Color.Black, name);
        }

        public override bool RightClick(int i, int j)
        {
            return false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.rand.NextBool(600))
            {
                SoundEngine.PlaySound(Sonar);
            }
            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
            /*if (Main.mouseRightRelease && Main.mouseRight)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemBell);

                if (t.TileFrameX == 18)
                {
                    t.TileFrameX = 0;
                }
                else
                {
                    t.TileFrameX = 18;
                }
            }*/
            if (Main.hasFocus)
            {
                //bool negaMode = t.TileFrameX >= 18;
                bool negaMode = false;
                if (negaMode)
                {
                    if (t.WallType != WallID.DiamondGemspark)
                    {
                        antDirection += 1;
                        if (antDirection > (int)AntDirectionType.Left)
                        {
                            antDirection = (int)AntDirectionType.Up;
                        }
                        t.WallType = WallID.DiamondGemsparkOff;
                        Move(t, i, j);
                    }
                    else
                    {
                        antDirection -= 1;
                        if (antDirection < (int)AntDirectionType.Up)
                        {
                            antDirection = (int)AntDirectionType.Left;
                        }
                        t.WallType = WallID.DiamondGemspark;
                        Move(t, i, j);
                    }
                }
                else
                {
                    if (t.WallType == WallID.DiamondGemspark)
                    {
                        antDirection += 1;
                        if (antDirection > (int)AntDirectionType.Left)
                        {
                            antDirection = (int)AntDirectionType.Up;
                        }
                        t.WallType = WallID.DiamondGemsparkOff;
                        Move(t, i, j);
                    }
                    else
                    {
                        antDirection -= 1;
                        if (antDirection < (int)AntDirectionType.Up)
                        {
                            antDirection = (int)AntDirectionType.Left;
                        }
                        t.WallType = WallID.DiamondGemspark;
                        Move(t, i, j);
                    }
                }
            }
        }

        public static void Move(Tile t, int x, int y)
        {
            switch ((AntDirectionType)antDirection)
            {
                case AntDirectionType.Up:
                    {
                        WorldGen.PlaceTile(x, y - 1, ModContent.TileType<Ant>(), style: t.TileFrameX % 18, forced: true);
                        CalamityUtils.ParanoidTileRetrieval(x, y - 1).TileFrameX = t.TileFrameX;
                        break;
                    }
                case AntDirectionType.Left:
                    {
                        WorldGen.PlaceTile(x - 1, y, ModContent.TileType<Ant>(), style: t.TileFrameX % 18, forced: true);
                        CalamityUtils.ParanoidTileRetrieval(x - 1, y).TileFrameX = t.TileFrameX;
                        break;
                    }
                case AntDirectionType.Right:
                    {
                        WorldGen.PlaceTile(x + 1, y, ModContent.TileType<Ant>(), style: t.TileFrameX % 18, forced: true);
                        CalamityUtils.ParanoidTileRetrieval(x + 1, y).TileFrameX = t.TileFrameX;
                        break;
                    }
                case AntDirectionType.Down:
                    {
                        WorldGen.PlaceTile(x, y + 1, ModContent.TileType<Ant>(), style: t.TileFrameX % 18, forced: true);
                        CalamityUtils.ParanoidTileRetrieval(x, y + 1).TileFrameX = t.TileFrameX;
                        break;
                    }
            }
            t.HasTile = false;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 8)
            {
                frameCounter = 0;
                frame++;
            }
            if (frame > 1)
            {
                frame = 0;
            }
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
    }
}