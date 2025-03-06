using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Graphics
{
    public class ShaderProjectileDrawSystem : ModSystem
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            Terraria.On_Main.DrawProjectiles += DrawShaderProjectiles;
        }

        private void DrawShaderProjectiles(Terraria.On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig(self);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Draw all projectiles that have the relevant interface.
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.ModProjectile is IDrawsWithShader drawer)
                    drawer.Draw(Main.spriteBatch);
            }

            Main.spriteBatch.End();
        }
    }
}
