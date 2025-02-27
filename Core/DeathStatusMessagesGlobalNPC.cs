using CalamityMod;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.SupremeCalamitas;
using Microsoft.Xna.Framework;
using CalRemix.Core.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core
{
    public class DeathStatusMessagesGlobalNPC : GlobalNPC
    {
        public static Color NoxusTextColor => new(91, 69, 143);

        public override void OnKill(NPC npc)
        {
            // Create some indicator text when the WoF is killed about how Noxus has begun orbiting the planet.
            if (npc.type == NPCID.WallofFlesh && !NoxusEggCutsceneSystem.NoxusBeganOrbitingPlanet)
                CalRemixHelper.BroadcastText(NoxusEggCutsceneSystem.PostWoFDefeatText, NoxusTextColor);

            // Create some indicator text when SCal or Draedon (whichever is defeated last) is defeated as a hint to fight Noxus.
            bool draedonDefeatedLast = (npc.type == ModContent.NPCType<ThanatosHead>() || npc.type == ModContent.NPCType<AresBody>() || npc.type == ModContent.NPCType<Apollo>()) && DownedBossSystem.downedCalamitas && !DownedBossSystem.downedExoMechs;
            bool calDefeatedLast = npc.type == ModContent.NPCType<SupremeCalamitas>() && DownedBossSystem.downedExoMechs && !DownedBossSystem.downedCalamitas;
            if (calDefeatedLast || draedonDefeatedLast)
            {
                // Apply a secondary check to ensure that when an Exo Mech is killed it is the last exo mech.
                bool noExtraExoMechs = NPC.CountNPCS(ModContent.NPCType<ThanatosHead>()) + NPC.CountNPCS(ModContent.NPCType<AresBody>()) + NPC.CountNPCS(ModContent.NPCType<Apollo>()) <= 1;
                if (calDefeatedLast || noExtraExoMechs)
                    CalRemixHelper.BroadcastText(NoxusEggCutsceneSystem.FinalMainBossDefeatText, NoxusTextColor);
            }
        }
    }
}
