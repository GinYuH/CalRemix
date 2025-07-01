using CalamityMod.NPCs.Cryogen;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
{
    public class PurpleShavedIcePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(199, 90, 190));
            HitSound = Cryogen.HitSound with { MaxInstances = 0 };
            DustType = DustID.Ice_Purple;
        }
    }
}