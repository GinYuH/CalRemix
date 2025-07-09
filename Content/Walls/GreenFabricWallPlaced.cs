using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls;

public class GreenFabricWallPlaced : ModWall
{
    public override void SetStaticDefaults()
    {
        AddMapEntry(new Color(12, 89, 54), null);
        DustType = DustID.DungeonGreen;
        HitSound = BetterSoundID.ItemIceHit with { Pitch = -0.6f };
    }


    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = (fail ? 1 : 3);
    }
}
