using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System.Reflection;
using CalamityMod.Tiles.SunkenSea;

namespace CalRemix.Content.Tiles
{
    public class EutrophicSandSafe : ModTile
    {
        public override string Texture => "CalamityMod/Tiles/SunkenSea/EutrophicSand";
        public static readonly MethodInfo BrimstoneTileFraming = typeof(TileFraming).GetMethod("BrimstoneFraming", BindingFlags.NonPublic | BindingFlags.Static);
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Sand"]);

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithDesert(Type); // Tile blends with sandstone, which it is set to merge with here

            Main.tileShine[Type] = 1800;
            Main.tileShine2[Type] = false;

            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;

            DustType = DustID.RainCloud;
            AddMapEntry(new Color(92, 145, 167));
            RegisterItemDrop(ModContent.ItemType<CalamityMod.Items.Placeables.EutrophicSand>());
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            Tile up = Main.tile[i, j - 1];
            Tile up2 = Main.tile[i, j - 2];

            // Place SmallCorals
            if (WorldGen.genRand.NextBool(8) && !up.HasTile && !up2.HasTile && up.LiquidAmount > 0 && up2.LiquidAmount > 0 && !tile.LeftSlope && !tile.RightSlope && !tile.IsHalfBlock)
            {
                up.TileType = (ushort)ModContent.TileType<SmallCorals>();
                up.HasTile = true;
                up.TileFrameY = 0;

                // 6 different frames, choose a random one
                up.TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
                WorldGen.SquareTileFrame(i, j - 1, true);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return (bool)BrimstoneTileFraming.Invoke(null, new object[] { i, j, resetFrame });
        }
    }
}
