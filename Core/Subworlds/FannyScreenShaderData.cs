using System;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalRemix.Core.Subworlds
{
    public class FannyScreenShaderData : ScreenShaderData
    {
        public FannyScreenShaderData(string passName)
            : base(passName)
        {
        }

        public override void Apply()
        {
            if (SubworldSystem.IsActive<FannySubworld>())
                UseTargetPosition(Main.LocalPlayer.Center);

            base.Apply();
        }

        public override void Update(GameTime gameTime)
        {
            if (!FannySky.CanSkyBeActive)
                Filters.Scene["CalRemix:Fanny"].Deactivate(Array.Empty<object>());
        }
    }
}
