using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System.Reflection;
using CalamityMod.Items.Placeables;

namespace CalRemix.Content.Tiles;
public class NavystoneSafe : ModTile
{
    public override string Texture => "CalamityMod/Tiles/SunkenSea/Navystone";
    public static readonly MethodInfo BrimstoneTileFraming = typeof(TileFraming).GetMethod("BrimstoneFraming", BindingFlags.NonPublic | BindingFlags.Static);
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;

        CalamityUtils.MergeWithGeneral(Type);
        CalamityUtils.MergeWithDesert(Type);

        TileID.Sets.ChecksForMerge[Type] = true;
        DustType = DustID.BlueMoss;
        AddMapEntry(new Color(31, 92, 114));
        HitSound = SoundID.Tink;
        RegisterItemDrop(ModContent.ItemType<Navystone>());
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
