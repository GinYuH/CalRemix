using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.NPCs.PlagueEnemies;

namespace CalRemix.NPCs
{
    public class PlagueGlobalNPC : GlobalNPC
    {
        public static PlagueJungleHelper PlagueHelper { get; internal set; }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.Player;
            CalRemixPlayer pPlayer = player.GetModPlayer<CalRemixPlayer>();
            var cPlayer = player.Calamity();
            if (pPlayer.ZonePlague)
            {
                pool.Clear();
                pool.Add(ModContent.NPCType<Plagueshell>(), 0.075f);
                pool.Add(ModContent.NPCType<PestilentSlime>(), 0.1f);
                pool.Add(ModContent.NPCType<Melter>(), 0.1f);
                pool.Add(ModContent.NPCType<Viruling>(), 0.1f);
                pool.Add(ModContent.NPCType<PlaguebringerMiniboss>(), 0.001f);
                pool.Add(ModContent.NPCType<PlagueCharger>(), 0.1f);
                pool.Add(ModContent.NPCType<PlagueChargerLarge>(), 0.075f);
                if (!Main.dayTime)
                    pool.Add(ModContent.NPCType<PlaguedFirefly>(), 0.35f);
                pool.Add(ModLoader.GetMod("CalValEX").Find<ModNPC>("PlagueFrog").Type, 0.045f);
            }
            if (player.ZoneJungle && !pPlayer.ZonePlague)
            {
                pool.Remove(ModContent.NPCType<Plagueshell>());
                pool.Remove(ModContent.NPCType<PestilentSlime>());
                pool.Remove(ModContent.NPCType<Melter>());
                pool.Remove(ModContent.NPCType<Viruling>());
                pool.Remove(ModContent.NPCType<PlagueCharger>());
                pool.Remove(ModContent.NPCType<PlagueChargerLarge>());
                pool.Remove(ModContent.NPCType<PlaguebringerMiniboss>());
            }
            base.EditSpawnPool(pool, spawnInfo);
        }

        public override bool PreAI(NPC npc)
        {
            if (PlagueHelper.CurrentlyActive())
            {
                PlagueHelper.ClearAllJunglePlagueSwappies();
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead)
                {
                    PlagueHelper.ApplyJunglePlagueSwappies(Main.player[i]);
                }
            }
            return base.PreAI(npc);
        }

        public override void PostAI(NPC npc)
        {
            if (PlagueHelper.CurrentlyActive())
            {
                PlagueHelper.ClearAllJunglePlagueSwappies();
            }
        }
    }
}
