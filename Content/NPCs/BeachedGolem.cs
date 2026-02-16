using CalamityMod;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs
{
    public class BeachedGolem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Golem);
            NPC.aiStyle = -1;
            NPC.alpha = 0;
            NPC.width /= 2;
            NPC.height /= 2;
            NPC.behindTiles = true;
            NPC.boss = false;
            NPC.value = Item.buyPrice(copper: 70);
        }

        public override void AI()
        {
            if (NPC.rotation == 0)
            {
                NPC.rotation = MathHelper.ToRadians(110 + Main.rand.Next(-40, 40)) * Main.rand.NextBool().ToDirectionInt();
            }
            NPC.velocity.Y += 1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && !NPC.AnyNPCs(Type)
            && (spawnInfo.SpawnTileX < 340 || spawnInfo.SpawnTileX > Main.maxTilesX - 340)
            && spawnInfo.SpawnTileY < Main.worldSurface)
            {
                return 0.03f;
            }
            return 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, 0, 0);
            return false;
        }
    }
}