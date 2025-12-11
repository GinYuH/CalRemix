using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class FrostflakeWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            //ItemDrop = ModContent.ItemType<FrostflakeWall>();
            AddMapEntry(new Color(7, 99, 133));
            DustType = DustID.Frost;
        }
    }
}