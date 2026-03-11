using System;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalRemix.Core.Subworlds
{
    public class ScreamingScreenShaderData : ScreenShaderData
    {
        public ScreamingScreenShaderData(string passName)
            : base(passName)
        {
        }

        public override void Apply()
        {
            if (SubworldSystem.IsActive<ScreamingSubworld>())
                UseTargetPosition(Main.LocalPlayer.Center);

            base.Apply();
        }

        public override void Update(GameTime gameTime)
        {
            if (!ScreamingFaceSky.CanSkyBeActive)
                Filters.Scene["CalRemix:ScreamingFaceSky"].Deactivate(Array.Empty<object>());
        }
    }
}
