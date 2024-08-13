using CalamityMod;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using System.Collections.Immutable;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace CalRemix.Scenes
{
	public class ExoMechsMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => ExoMechWorld.AnyExoQuartetActive || (ExoMechWorld.AnyExoMechActive && ExoMechWorld.ExoMayhem);
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music 
        {
            get
            {
                if (ExoMechWorld.ExoMayhem)
                    return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/XO");
                else
                {
                    if (ExoMechWorld.AllExoQuartetActive)
                    {
                        bool apingas = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>()).Calamity().newAI[1] != 2 || Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>()).Calamity().newAI[1] != 2;
                        bool larry = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>()).Calamity().newAI[1] != 2;
                        bool thanos = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>()).Calamity().newAI[1] != 2;
                        if (apingas && larry && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/XO");
                        else if (!apingas && larry && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ThanosLarry");
                        else if (apingas && larry && !thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ApingasLarry");
                        else if (apingas && !larry && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ApingasThanos");
                        else if (!apingas && !larry && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Thanos");
                        else if (!apingas && larry && !thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Larry");
                        else if (apingas && !larry && !thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Apingas");
                        else
                            return MusicLoader.GetMusicSlot(ModLoader.GetMod("CalamityModMusic"), "CalamityModMusic/Sounds/Music/ExoMechs");
                    }
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        bool larry = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>()).Calamity().newAI[1] != 2;
                        bool thanos = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>()).Calamity().newAI[1] != 2;
                        if (larry && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ThanosLarry");
                        else if (!larry && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Thanos");
                        else if (larry && !thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Larry");
                        else
                            return MusicLoader.GetMusicSlot(ModLoader.GetMod("CalamityModMusic"), "CalamityModMusic/Sounds/Music/ExoMechs");
                    }
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        bool apingas = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>()).Calamity().newAI[1] != 2 || Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>()).Calamity().newAI[1] != 2;
                        bool larry = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>()).Calamity().newAI[1] != 2;
                        if (larry && apingas)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ApingasLarry");
                        else if (!larry && apingas)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Apingas");
                        else if (larry && !apingas)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Larry");
                        else
                            return MusicLoader.GetMusicSlot(ModLoader.GetMod("CalamityModMusic"), "CalamityModMusic/Sounds/Music/ExoMechs");
                    }
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        bool apingas = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>()).Calamity().newAI[1] != 2 || Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>()).Calamity().newAI[1] != 2;
                        bool thanos = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>()).Calamity().newAI[1] != 2;
                        if (apingas && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ApingasThanos");
                        else if (!apingas && thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Thanos");
                        else if (apingas && !thanos)
                            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Apingas");
                        else
                            return MusicLoader.GetMusicSlot(ModLoader.GetMod("CalamityModMusic"), "CalamityModMusic/Sounds/Music/ExoMechs");
                    }
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Thanos");
                    }
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Larry");
                    }
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Apingas");
                    }
                    else
                    {
                        return MusicLoader.GetMusicSlot(ModLoader.GetMod("CalamityModMusic"), "CalamityModMusic/Sounds/Music/ExoMechs");
                    }
                }
            }
        }
    }
}