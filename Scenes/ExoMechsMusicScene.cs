using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace CalRemix.Scenes
{
	public class ExoMechsMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => ExoMechWorld.ExoQuartetActive;
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music 
        {
            get
            {
                if (ExoMechWorld.ExoMayhem)
                    return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/XO");
                else
                {
                    if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/XO");
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ThanosLarry");
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ApingasLarry");
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ApingasThanos");
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Thanos");
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Larry");
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Apingas");
                    else
                        return MusicLoader.GetMusicSlot(Mod, "CalamityModMusic/Sounds/Music/ExoMechs");
                }
            }
        }
    }
}