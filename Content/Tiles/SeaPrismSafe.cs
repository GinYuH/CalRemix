using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System.Reflection;

namespace CalRemix.Content.Tiles
{
    public class SeaPrismSafe : ModTile
    {
        private const short subsheetWidth = 450;
        private const short subsheetHeight = 198;
        public override string Texture => "CalamityMod/Tiles/SunkenSea/SeaPrism";
        public static readonly MethodInfo BrimstoneTileFraming = typeof(TileFraming).GetMethod("BrimstoneFraming", BindingFlags.NonPublic | BindingFlags.Static);
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithDesert(Type);

            Main.tileShine[Type] = 3500;
            Main.tileShine2[Type] = true;

            TileID.Sets.ChecksForMerge[Type] = true;
            DustType = DustID.Water;
            AddMapEntry(new Color(0, 150, 200));
            HitSound = SoundID.Tink;
            Main.tileSpelunker[Type] = true;
            MinPick = 55;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = i % 2 * subsheetWidth;
            frameYOffset = j % 2 * subsheetHeight;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return (bool)BrimstoneTileFraming.Invoke(null, new object[] { i, j, resetFrame });
        }
    }
}
