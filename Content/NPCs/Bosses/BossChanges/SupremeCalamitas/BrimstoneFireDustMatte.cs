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
    public class BrimstoneFireDustMatte : ModDust
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/BossChanges/SupremeCalamitas/BrimstoneFireDust";

        private float frameChangeNoise = Main.rand.NextFloat(-0.1f, 0.1f);
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 40, 40);
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 1f;
        }
        public override bool PreDraw(Dust dust)
        {
            // vibrant tomato red
            Color color1 = new Color(214, 44, 44);
            // dark purple-red-ish tone
            Color color2 = new Color(33, 6, 17);
            float lerpValue = (dust.scale - 1.25f) * -1;
            Color drawColor = Color.Lerp(color1, color2, lerpValue);
            drawColor *= 0.33f;
            drawColor.A = 60;

            int horizFrame = 0;
            if (dust.scale <= 0.9f + frameChangeNoise)
            {
                horizFrame = 1;
            }
            else if (dust.scale <= 0.8f + frameChangeNoise)
            {
                horizFrame = 2;
            }

            int vertFrame = 0;

            Main.spriteBatch.Draw(Texture2D.Value, dust.position - Main.screenPosition, Texture2D.Value.Frame(3, 2, horizFrame, vertFrame), drawColor, dust.rotation, new Vector2(Texture2D.Value.Width / 6, Texture2D.Value.Height / 4), dust.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
