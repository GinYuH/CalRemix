using System;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalRemix.Core.Subworlds
{
    public class NowhereScreenShaderData : ScreenShaderData
    {
        public NowhereScreenShaderData(string passName)
            : base(passName)
        {
        }

        public override void Apply()
        {
            if (SubworldSystem.IsActive<NowhereSubworld>())
                UseTargetPosition(Main.LocalPlayer.Center);

            base.Apply();
        }

        public override void Update(GameTime gameTime)
        {
            if (!NowhereSky.CanSkyBeActive)
                Filters.Scene["CalRemix:NowhereSky"].Deactivate(Array.Empty<object>());
        }
    }
}
