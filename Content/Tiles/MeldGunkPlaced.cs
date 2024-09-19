using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
{
    public class MeldGunkPlaced : ModTile
    {
        public static SoundStyle gunkHit = SoundID.NPCDeath1 with { Pitch = 0.2f };
        public override void SetStaticDefaults()
        {
            MineResist = 999f;
            MinPick = int.MaxValue - 220; // i am scared actually setting it to max will do something bad
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(133, 69, 115), name);
            HitSound = gunkHit;
            DustType = DustID.Obsidian;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override void Load()
        {
            On_WorldGen.CheckTileBreakability += Meld;
        }

        public int Meld(On_WorldGen.orig_CheckTileBreakability orig, int x, int y)
        {
            if (Main.tile[x, y].TileType == Type)
            {
                return 2;
            }
            else
            {
                return orig(x,y);
            }
        }

        public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor)
        {
            sightColor = Color.Cyan;
            return true;
        }
    }
}