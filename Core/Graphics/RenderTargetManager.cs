﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.Graphics
{
    public class RenderTargetManager : ModSystem
    {
        internal static List<ManagedRenderTarget> ManagedTargets = new();

        public delegate void RenderTargetUpdateDelegate();

        public static event RenderTargetUpdateDelegate RenderTargetUpdateLoopEvent;

        internal static void ResetTargetSizes(Terraria.On_Main.orig_SetDisplayMode orig, int width, int height, bool fullscreen)
        {
            foreach (ManagedRenderTarget target in ManagedTargets)
            {
                // Don't attempt to recreate targets that are already initialized or shouldn't be recreated.
                if (target is null || !target.IsDisposed || target.WaitingForFirstInitialization)
                    continue;

                Main.QueueMainThreadAction(() =>
                {
                    target.Recreate(width, height);
                });
            }

            orig(width, height, fullscreen);
        }

        internal static void DisposeOfTargets()
        {
            if (ManagedTargets is null)
                return;

            Main.QueueMainThreadAction(() =>
            {
                foreach (ManagedRenderTarget target in ManagedTargets)
                    target?.Dispose();
                ManagedTargets.Clear();
            });
        }

        public static RenderTarget2D CreateScreenSizedTarget(int screenWidth, int screenHeight) =>
            new(Main.instance.GraphicsDevice, screenWidth, screenHeight, true, SurfaceFormat.Color, DepthFormat.Depth24, 8, RenderTargetUsage.PreserveContents);

        public override void OnModLoad()
        {
            ManagedTargets = new();
            Main.OnPreDraw += HandleTargetUpdateLoop;
            Terraria.On_Main.SetDisplayMode += ResetTargetSizes;
        }

        public override void OnModUnload()
        {
            DisposeOfTargets();
        }

        private void HandleTargetUpdateLoop(GameTime obj) => RenderTargetUpdateLoopEvent?.Invoke();
    }
}
