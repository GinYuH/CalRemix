using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.NPCs.Bosses.Noxus;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix
{
    public class ShaderAutoloader : ModSystem
    {
        public override void OnModLoad()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            foreach (var path in Mod.GetFileNames().Where(f => f.Contains("Assets/Effects/")))
            {
                string shaderName = Path.GetFileNameWithoutExtension(path);
                string clearedPath = Path.Combine(Path.GetDirectoryName(path), shaderName).Replace(@"\", @"/");
                Ref<Effect> shader = new(Mod.Assets.Request<Effect>(clearedPath, AssetRequestMode.ImmediateLoad).Value);
                GameShaders.Misc[$"{Mod.Name}:{shaderName}"] = new MiscShaderData(shader, "AutoloadPass");
            }

            Ref<Effect> s = new(Mod.Assets.Request<Effect>("Assets/Effects/LocalizedDistortionShader", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CalRemix:NoxusEggSky"] = new Filter(new NoxusEggScreenShaderData(s, "AutoloadPass"), EffectPriority.VeryHigh);

            Filters.Scene["CalRemix:NoxusSky"] = new Filter(new NoxusScreenShaderData("FilterMiniTower").UseColor(Color.Transparent).UseOpacity(0f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalRemix:NoxusSky"] = new NoxusSky();
            
            var soRetro = Mod.Assets.Request<Effect>("Assets/Effects/SoRetroShader", AssetRequestMode.ImmediateLoad);
            var soRetroData = new ScreenShaderData(soRetro, "SoRetro");
            soRetroData.UseImage(Mod.Assets.Request<Texture2D>("Assets/ExtraTextures/SoRetro", AssetRequestMode.ImmediateLoad));
            Filters.Scene["CalRemix:SoRetro"] = new Filter(soRetroData, EffectPriority.VeryHigh);
        }
    }
}
