using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
{
    public class AstralEyeBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(96, 173, 179));
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
            if (TileID.Sets.Torch[t.TileType])
            {
                WorldGen.KillTile(i, j - 1);
                NPC.NewNPC(new EntitySource_TileInteraction(Main.LocalPlayer, i, j - 1), i * 16, (j - 4) * 16, ModContent.NPCType<DepthGlider>());
            }
        }
    }
}