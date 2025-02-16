using CalamityMod.Buffs.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas
{
    public class BrimstoneFireDustFlare : ModDust
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/BossChanges/SupremeCalamitas/BrimstoneFireDust";

        private float frameChangeNoise = Main.rand.NextFloat(-0.1f, 0.1f);
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 40, 40);
            dust.velocity *= 0.25f; 
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 0.6f;
        }
        public override bool PreDraw(Dust dust)
        {
            // saturated orange-yellow-red
            Color color1 = new Color(255, 145, 115);
            // slightly desat tomato red
            Color color2 = new Color(227, 79, 79);
            float lerpValue = (dust.scale - 1f) * -1 - 0.5f;
            Color drawColor = Color.Lerp(color1, color2, lerpValue);
            drawColor *= 0.5f;
            drawColor.A = 0;

            int horizFrame = 0;
            if (dust.scale < 0.33f + frameChangeNoise)
            {
                horizFrame = 2;
            }
            else if (dust.scale < 0.9f + frameChangeNoise)
            {
                horizFrame = 1;
            }

            int vertFrame = 0;

            Main.spriteBatch.Draw(Texture2D.Value, dust.position - Main.screenPosition, Texture2D.Value.Frame(3, 2, horizFrame, vertFrame), drawColor, dust.rotation, new Vector2(Texture2D.Value.Width / 6, Texture2D.Value.Height / 4), dust.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
