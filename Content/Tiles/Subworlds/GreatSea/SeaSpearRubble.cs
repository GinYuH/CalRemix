using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class SeaSpearRubble : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
            TileObjectData.addTile(Type);
            DustType = DustID.Shadewood;
            AddMapEntry(new Color(43, 43, 43));

            base.SetStaticDefaults();
        }
    }

    public class SeaSpearRubble2 : SeaSpearRubble
    {

    }
}
