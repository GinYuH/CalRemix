using CalamityMod;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using System.Collections.Immutable;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace CalRemix.Core.Scenes
{
	public class ExoMechsMusicScene : ModSceneEffect
    {
        private static int CalamityExoTheme = MusicLoader.GetMusicSlot(CalRemix.CalMusic, "CalamityModMusic/Sounds/Music/ExoMechs");
        public override bool IsSceneEffectActive(Player player) => ExoMechWorld.AnyExoQuartetActive || (ExoMechWorld.AnyExoMechActive && ExoMechWorld.ExoMayhem);
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music 
        {
            get
            {
                if (ExoMechWorld.ExoMayhem)
                    return CalRemixMusic.ExoMechs;
                else
                {
                    if (ExoMechWorld.AllExoQuartetActive)
                    {
                        bool apingas = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>()).Calamity().newAI[1] != 2 || Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>()).Calamity().newAI[1] != 2;
                        bool larry = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>()).Calamity().newAI[1] != 2;
                        bool thanos = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>()).Calamity().newAI[1] != 2;
                        if (apingas && larry && thanos)
                            return CalRemixMusic.ExoMechs;
                        else if (!apingas && larry && thanos)
                            return CalRemixMusic.ThanatosAres;
                        else if (apingas && larry && !thanos)
                            return CalRemixMusic.ExoTwinsAres;
                        else if (apingas && !larry && thanos)
                            return CalRemixMusic.ExoTwinsThanatos;
                        else if (!apingas && !larry && thanos)
                            return CalRemixMusic.Thanatos;
                        else if (!apingas && larry && !thanos)
                            return CalRemixMusic.Ares;
                        else if (apingas && !larry && !thanos)
                            return CalRemixMusic.ExoTwins;
                        else
                            return CalamityExoTheme;
                    }
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        bool larry = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>()).Calamity().newAI[1] != 2;
                        bool thanos = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>()).Calamity().newAI[1] != 2;
                        if (larry && thanos)
                            return CalRemixMusic.ThanatosAres;
                        else if (!larry && thanos)
                            return CalRemixMusic.Thanatos;
                        else if (larry && !thanos)
                            return CalRemixMusic.Ares;
                        else
                            return CalamityExoTheme;
                    }
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        bool apingas = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>()).Calamity().newAI[1] != 2 || Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>()).Calamity().newAI[1] != 2;
                        bool larry = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<AresBody>()).Calamity().newAI[1] != 2;
                        if (larry && apingas)
                            return CalRemixMusic.ExoTwinsAres;
                        else if (!larry && apingas)
                            return CalRemixMusic.ExoTwins;
                        else if (larry && !apingas)
                            return CalRemixMusic.Ares;
                        else
                            return CalamityExoTheme;
                    }
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        bool apingas = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Artemis>()).Calamity().newAI[1] != 2 || Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<Apollo>()).Calamity().newAI[1] != 2;
                        bool thanos = Main.npc.ToImmutableList().Find((NPC npc) => npc.type == NPCType<ThanatosHead>()).Calamity().newAI[1] != 2;
                        if (apingas && thanos)
                            return CalRemixMusic.ExoTwinsThanatos;
                        else if (!apingas && thanos)
                            return CalRemixMusic.Thanatos;
                        else if (apingas && !thanos)
                            return CalRemixMusic.ExoTwins;
                        else
                            return CalamityExoTheme;
                    }
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        return CalRemixMusic.Thanatos;
                    }
                    else if (!(NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        return CalRemixMusic.Ares;
                    }
                    else if ((NPC.AnyNPCs(NPCType<Artemis>()) || NPC.AnyNPCs(NPCType<Apollo>())) && !NPC.AnyNPCs(NPCType<AresBody>()) && !NPC.AnyNPCs(NPCType<ThanatosHead>()))
                    {
                        return CalRemixMusic.ExoTwins;
                    }
                    else
                    {
                        return CalamityExoTheme;
                    }
                }
            }
        }
    }
}