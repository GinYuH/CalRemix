using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static CalRemix.Content.NPCs.Bosses.Pyrogen.Pyrogen;

namespace CalRemix.Core.Scenes
{
	public class PyrogenScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Pyrogen>()) && !Main.zenithWorld)
            {
                NPC npc = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Pyrogen>())];
                if (npc.ModNPC<Pyrogen>().phase2 || !npc.ModNPC<Pyrogen>().phase2 && npc.ModNPC<Pyrogen>().Phase == (int)PyroPhaseType.Transitioning)
                    return true;
            }
            if (Filters.Scene["CalRemix:PyrogenHeat"].IsActive())
                Filters.Scene["CalRemix:PyrogenHeat"].Deactivate();
            if (Filters.Scene["HeatDistortion"].IsActive())
                Filters.Scene["HeatDistortion"].Deactivate();
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                NPC npc = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Pyrogen>())];
                Pyrogen pyro = npc.ModNPC<Pyrogen>();
                if (!Filters.Scene["CalRemix:PyrogenHeat"].IsActive())
                {
                    Filters.Scene.Activate("CalRemix:PyrogenHeat", Main.player[Main.myPlayer].position);
                }
                else
                {
                    ScreenShaderData shader = Filters.Scene["CalRemix:PyrogenHeat"].GetShader();
                    shader.UseImage("Images/Misc/Perlin");
                    shader.UseColor(new Color(254, 210, 180));
                    if (!pyro.phase2 && pyro.Phase == (int)PyroPhaseType.Transitioning)
                        shader.UseOpacity(pyro.deathTimer / 160);
                    if (pyro.phase3 && pyro.Phase != (int)PyroPhaseType.Transitioning)
                        shader.UseIntensity((float)(npc.lifeMax - npc.life) / (float)npc.lifeMax);
                    else
                        shader.UseIntensity(0f);
                }
                if (!Filters.Scene["HeatDistortion"].IsActive())
                {
                    Filters.Scene.Activate("HeatDistortion", Main.player[Main.myPlayer].position);
                }
                else
                {
                    ScreenShaderData shader = Filters.Scene["HeatDistortion"].GetShader();
                    if (!pyro.phase2 && pyro.Phase == (int)PyroPhaseType.Transitioning)
                        Filters.Scene["HeatDistortion"].GetShader().UseIntensity(pyro.deathTimer / 160);
                    if (pyro.phase3 && pyro.Phase != (int)PyroPhaseType.Transitioning)
                        shader.UseIntensity(((float)(npc.lifeMax - npc.life) / (float)npc.lifeMax) * 2f + 1f);
                    else
                        shader.UseIntensity(1f);
                }
            }
        }
    }
}