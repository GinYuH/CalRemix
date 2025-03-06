using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;

namespace CalRemix.Core.Graphics
{
    public class ScreenOverlaysSystem : ModSystem
    {
        public static List<int> DrawCacheBeforeBlack
        {
            get;
            private set;
        } = new(Main.maxProjectiles);

        public static List<int> DrawCacheAfterNoxusFog
        {
            get;
            private set;
        } = new(Main.maxProjectiles);

        internal static void DrawOverBlackNPCCache(ILContext il)
        {
            ILCursor cursor = new(il);

            if (!cursor.TryGotoNext(MoveType.Before, i => i.MatchCall<ScreenDarkness>("DrawBack")))
                return;

            cursor.EmitDelegate(() =>
            {
                if (Main.gameMenu)
                    return;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                MetaballManager.DrawMetaballs(MetaballDrawLayerType.BeforeBlack);
                Main.spriteBatch.ExitShaderRegion();
                EmptyDrawCache(DrawCacheBeforeBlack);
            });
        }

        public static void EmptyDrawCache(List<int> cache)
        {
            for (int i = 0; i < cache.Count; i++)
            {
                try
                {
                    Main.instance.DrawNPC(cache[i], false);
                }
                catch (Exception e)
                {
                    TimeLogger.DrawException(e);
                    Main.npc[cache[i]].active = false;
                }
            }
            cache.Clear();
        }

        public override void OnModLoad()
        {
            Main.QueueMainThreadAction(() =>
            {
                Terraria.IL_Main.DoDraw += DrawOverBlackNPCCache;
            });
        }

        public override void Unload()
        {
            Main.QueueMainThreadAction(() =>
            {
                Terraria.IL_Main.DoDraw -= DrawOverBlackNPCCache;
            });
        }
    }
}
