using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class FrostflakeWallFriendlyPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/FrostflakeWallPlaced";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            //ItemDrop = ModContent.ItemType<FrostflakeWall>();
            AddMapEntry(new Color(7, 99, 133));
            DustType = 92;
        }
    }
}