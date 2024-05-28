using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using System.Reflection;
using CalamityMod.Items.Placeables;

namespace CalRemix.Tiles;
public class NavystoneSafe : ModTile
{
    public override string Texture => "CalamityMod/Tiles/SunkenSea/Navystone";
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;

        CalamityUtils.MergeWithGeneral(Type);
        CalamityUtils.MergeWithDesert(Type);

        TileID.Sets.ChecksForMerge[Type] = true;
        DustType = 96;
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
        MethodInfo info = typeof(TileFraming).GetMethod("BrimstoneFraming", BindingFlags.NonPublic | BindingFlags.Static);
        return (bool)info.Invoke(null, new object[] { i, j, resetFrame });
    }
}
