using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
{
    public class BanishedPlatingPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            MineResist = 2.2f;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(140, 174, 212));
            HitSound = SoundID.NPCHit14;
        }
    }
}