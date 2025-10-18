using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class SeaRockRubble : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.CoordinateHeights = new[] {18};
            TileObjectData.addTile(Type);
            DustType = DustID.Shadewood;
            AddMapEntry(new Color(43, 43, 43));

            base.SetStaticDefaults();
        }
    }

    public class SeaRockRubble2 : SeaRockRubble
    {

    }
}
