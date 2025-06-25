using CalRemix.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
{
    public class VigorBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            HitSound = SoundID.NPCHit1;
            DustType = DustID.Blood;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Vigor>()))
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    NPC.NewNPC(new EntitySource_WorldEvent(), i * 16, j * 16, ModContent.NPCType<Vigor>());
                }
            }
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return false;
        }
    }
}