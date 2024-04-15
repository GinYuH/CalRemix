using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Tiles
{
    public class TanzaniteGlassPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(237, 147, 215), name);
            HitSound = SoundID.Shatter;
            DustType = DustID.PinkCrystalShard;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.6f + (float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly / 2.2f));
            g = 0f;
            b = 1.2f;
        }
    }
}