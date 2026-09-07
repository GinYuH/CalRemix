using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest.Temple
{
    public class PhyllitePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(103, 80, 67));
            HitSound = SoundID.Tink;
            DustType = DustID.Clay;
            Main.tileMerge[Type][ModContent.TileType<IdolizedPhylliteBrickPlaced>()] = true;
            Main.tileMerge[Type][ModContent.TileType<PhylliteBrickPlaced>()] = true;
            Main.tileMerge[Type][ModContent.TileType<EtchedPhylliteBrickPlaced>()] = true;
        }
    }
    public class IdolizedPhylliteBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(103, 80, 67));
            HitSound = SoundID.Tink;
            DustType = DustID.Clay;
            Main.tileMerge[Type][ModContent.TileType<PhyllitePlaced>()] = true;
        }
    }
    public class PhylliteBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(103, 80, 67));
            HitSound = SoundID.Tink;
            DustType = DustID.Clay;
            Main.tileBrick[Type] = true;
            //Main.tileMerge[Type][ModContent.TileType<IdolizedPhylliteBrickPlaced>()] = true;
            //Main.tileMerge[Type][ModContent.TileType<PhyllitePlaced>()] = true;
            //Main.tileMerge[Type][ModContent.TileType<EtchedPhylliteBrickPlaced>()] = true;
            //Main.tileMerge[Type][ModContent.TileType<LargePhylliteBrickPlaced>()] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
        }
    }
    public class EtchedPhylliteBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(103, 80, 67));
            HitSound = SoundID.Tink;
            DustType = DustID.Clay;
            AnimationFrameHeight = 90;
            Main.tileMerge[Type][ModContent.TileType<PhyllitePlaced>()] = true;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile right = Framing.GetTileSafely(i + 1, j);
            Tile down = Framing.GetTileSafely(i, j + 1);
            Tile up = Framing.GetTileSafely(i, j - 1);
            Tile left = Framing.GetTileSafely(i - 1, j);

            if (up.TileType == type && right.TileType == type)
            {
                frameYOffset = AnimationFrameHeight;
            }
            else if (up.TileType == type && left.TileType == type)
            {
                frameYOffset = AnimationFrameHeight;
                frameXOffset = 234;
            }
            else if (down.TileType == type && left.TileType == type)
            {
                frameXOffset = 234;
            }
        }
    }
    public class LargePhylliteBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(103, 80, 67));
            HitSound = SoundID.Tink;
            DustType = DustID.Clay;
            AnimationFrameHeight = 90;
            Main.tileBrick[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<PhyllitePlaced>()] = true;
            Main.tileMerge[Type][ModContent.TileType<PhylliteBrickPlaced>()] = true;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int xPos = i % 4;
            int yPos = j % 4;
            frameXOffset = xPos * 234;
            frameYOffset = yPos * AnimationFrameHeight;
        }
    }
}