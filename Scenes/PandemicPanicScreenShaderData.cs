using System;
using CalRemix.NPCs.PandemicPanic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalRemix.Scenes
{
    public class PandemicPanicScreenShaderData : ScreenShaderData
    {
        public PandemicPanicScreenShaderData(string passName)
            : base(passName)
        {
        }

        public override void Apply()
        {
            if (PandemicPanic.IsActive)
                UseTargetPosition(Main.LocalPlayer.Center);

            base.Apply();
        }

        public override void Update(GameTime gameTime)
        {
            if (!PandemicSky.CanSkyBeActive)
                Filters.Scene["CalRemix:PandemicPanic"].Deactivate(Array.Empty<object>());
        }
    }
}
