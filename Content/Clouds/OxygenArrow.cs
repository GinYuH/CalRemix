using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Oxygen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Clouds
{
    public class OxygenArrow : ModCloud
    {

        public override float SpawnChance()
        {
            if (!Main.gameMenu && NPC.AnyNPCs(ModContent.NPCType<Oxygen>()))
            {
                return 10f;
            }
            return 0f;
        }

        public override bool Draw(SpriteBatch spriteBatch, Cloud cloud, int cloudIndex, ref DrawData drawData)
        {
            if (!Main.gameMenu)
            {
                var drawDataCopy = drawData;
                float x = Main.dungeonX > Main.spawnTileX ? Main.maxTilesX : 0;
                Vector2 abyss = new Vector2(x, Main.UnderworldLayer) * 16;
                float rot = Main.LocalPlayer.Calamity().ZoneSulphur ? -MathHelper.PiOver2 : Main.cloud[cloudIndex].position.DirectionTo(abyss).ToRotation() + MathHelper.Pi;
                drawDataCopy.rotation = rot;
                drawDataCopy.effect = 0;
                drawDataCopy.Draw(spriteBatch);
            }
            return false;
        }
    }
}