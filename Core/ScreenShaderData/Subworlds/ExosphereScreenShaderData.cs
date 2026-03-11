using System;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalRemix.Core.Subworlds
{
    public class ExosphereScreenShaderData : ScreenShaderData
    {
        public ExosphereScreenShaderData(string passName)
            : base(passName)
        {
        }

        public override void Apply()
        {
            if (SubworldSystem.IsActive<ExosphereSubworld>())
                UseTargetPosition(Main.LocalPlayer.Center);

            base.Apply();
        }

        public override void Update(GameTime gameTime)
        {
            if (!ExosphereSky.CanSkyBeActive)
                Filters.Scene["CalRemix:Exosphere"].Deactivate(Array.Empty<object>());
        }
    }
}
