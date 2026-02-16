using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;

namespace CalRemix.Core.Graphics
{
    public class CameraPanSystem : ModSystem
    {
        public static Vector2 CameraFocusPoint
        {
            get;
            set;
        }

        public static float CameraPanInterpolant
        {
            get;
            set;
        }

        public static float Zoom
        {
            get;
            set;
        }

        public override void ModifyScreenPosition()
        {
            if (Main.LocalPlayer.dead)
            {
                Zoom = Lerp(Zoom, 0f, 0.13f);
                CameraPanInterpolant = 0f;
                return;
            }

            // Handle camera focus effects.
            if (CameraPanInterpolant > 0f)
            {
                Vector2 idealScreenPosition = CameraFocusPoint - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
                Main.screenPosition = Vector2.Lerp(Main.screenPosition, idealScreenPosition, CameraPanInterpolant);
            }

            if (!Main.gamePaused)
            {
                CameraPanInterpolant = Clamp(CameraPanInterpolant - 0.15f, 0f, 1f);
                Zoom = Lerp(Zoom, 0f, 0.09f);
            }
        }

        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            Transform.Zoom *= 1f + Zoom;
        }
    }
}
