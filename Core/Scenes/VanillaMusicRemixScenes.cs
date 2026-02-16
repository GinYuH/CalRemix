using CalamityMod;
using CalamityMod.BiomeManagers;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalRemix.Content.NPCs.Minibosses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class BloodMoonRemixScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (Main.bloodMoon)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override int Music => CalRemixMusic.BloodMoonRemix;
    }
    
    public class DesertRemixlesScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneUndergroundDesert && !DownedBossSystem.downedDesertScourge)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
        public override int Music => CalRemixMusic.DesertRemixles;
    }

    public class DesertRemixScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneUndergroundDesert && DownedBossSystem.downedDesertScourge)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
        public override int Music => CalRemixMusic.DesertRemix;
    }

    public class QueenSlimeRemixScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(NPCID.QueenSlimeBoss))
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.type == NPCID.QueenSlimeBoss)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;
        public override int Music => CalRemixMusic.QueenSlimeRemix;
    }

    public class ShimmerRemixScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (Main.LocalPlayer.ZoneShimmer)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => CalRemixMusic.ShimmerRemix;
    }

    public class SulphSeaDayRemixScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!CalamityPlayer.areThereAnyDamnBosses && Main.LocalPlayer.InModBiome(ModContent.GetInstance<SulphurousSeaBiome>()) && Main.dayTime && Main.cloudAlpha <= 0f && !AcidRainEvent.AcidRainEventIsOngoing)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => CalRemixMusic.SulphSeaDayRemix;
    }

    public class GoblinArmyRemixScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinSummoner)
                {
                    return true;
                }
            }
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override int Music => CalRemixMusic.GoblinArmyRemix;
    }
}
