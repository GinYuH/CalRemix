using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class SeaAnchorRubble : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
            TileObjectData.addTile(Type);
            DustType = DustID.Shadewood;
            AddMapEntry(new Color(43, 43, 43));

            base.SetStaticDefaults();
        }
    }

    public class SeaAnchorRubble2 : SeaAnchorRubble
    {

    }
}
