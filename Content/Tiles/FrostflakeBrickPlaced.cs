using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;

namespace CalRemix.Content.Tiles
{
    public class FrostflakeBrickPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            //ItemDrop = ModContent.ItemType<FrostflakeBrick>();
            AddMapEntry(new Color(66, 242, 245));
            DustType = 92;
            MinPick = 0;
            HitSound = SoundID.Tink;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return Main.hardMode;
        }

        public override bool CanExplode(int i, int j)
        {
            return Main.hardMode;
        }

        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return Main.hardMode;
        }
    }
}