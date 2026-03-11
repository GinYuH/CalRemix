using CalamityMod;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Backgrounds;
using CalRemix.Core.Backgrounds.Plague;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class NightlineScene : ModSceneEffect
    {
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<NightlineBgStyle>();
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

                    int car = ModContent.NPCType<Car>();

                    bool done = false;
                    foreach (NPC n in Main.ActiveNPCs)
                    {
                        if (n.type == car)
                        {
                            if (n.ai[0] == 2)
                            {
                                done = true;
                                break;
                            }
                        }
                    }
                    if (done && opacity == maxOpacity)
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.CalRemix.DeathReasons.Nightline", player.name)), 66200, 1);
                    }
                }
            }
        }
    }
}