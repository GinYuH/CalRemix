using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.TheGray
{
    public class BlueMazeBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(28, 16, 107));
            HitSound = SoundID.Tink;
            DustType = DustID.DungeonBlue;
            Main.tileBlendAll[Type] = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
    public class YellowMazeBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(110, 103, 16));
            HitSound = SoundID.Tink;
            DustType = DustID.YellowStarfish;
            Main.tileBlendAll[Type] = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}