using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System.Reflection;

namespace CalRemix.Content.Tiles
{
    public class HardenedEutrophicSandSafe : ModTile
    {
        public override string Texture => "CalamityMod/Tiles/SunkenSea/HardenedEutrophicSand";
        public static readonly MethodInfo BrimstoneTileFraming = typeof(TileFraming).GetMethod("BrimstoneFraming", BindingFlags.NonPublic | BindingFlags.Static);
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithDesert(Type);

            Main.tileShine[Type] = 2200;
            Main.tileShine2[Type] = false;

            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;

            DustType = DustID.RainCloud;
            AddMapEntry(new Color(67, 107, 143));
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
