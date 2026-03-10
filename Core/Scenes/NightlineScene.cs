using CalamityMod;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Backgrounds;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class NightlineScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!SubworldSystem.IsActive<NightlineSubworld>())
            {
                return false;
            }
            return true;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                if (!Filters.Scene["CalRemix:Nightline"].IsActive())
                {
                    Filters.Scene.Activate("CalRemix:Nightline", player.position);
                }
                else
                {
                    ScreenShaderData data = Filters.Scene["CalRemix:Nightline"].GetShader();
                    data.UseImage(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Perlin", AssetRequestMode.ImmediateLoad));

                    float opacity = 0;

                    float noFog = 0.2f;
                    float fullOptime = 0.2f;
                    float maxOpacity = 0.8f;

                    float completion = player.Center.X / (16f * Main.maxTilesX);

                    if (completion < 0.5f)
                        opacity = MathHelper.Lerp(0, maxOpacity, Utils.GetLerpValue(noFog, 0.5f - fullOptime / 2f, completion, true));
                    else
                        opacity = MathHelper.Lerp(maxOpacity, 0, Utils.GetLerpValue(0.5f + fullOptime / 2f, 1 - noFog, completion, true));

                    data.UseOpacity(opacity);
                }
            }
        }
    }
}