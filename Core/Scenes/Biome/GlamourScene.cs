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
    public class GlamourScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!SubworldSystem.IsActive<GlamourSubworld>() && !player.Remix().glamourMonolith)
            {
                return false;
            }
            return true;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                player.ManageSpecialBiomeVisuals("CalRemix:GlamourSky", isActive);
                SkyManager.Instance.Activate("CalRemix:GlamourSky", player.position);
            }
        }
    }
}