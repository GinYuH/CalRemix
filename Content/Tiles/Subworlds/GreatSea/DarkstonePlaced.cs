using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class DarkstonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(5, 5, 5));
            HitSound = SoundID.Tink;
            DustType = DustID.Obsidian;
            Main.tileBlendAll[Type] = true;
        }
    }
}