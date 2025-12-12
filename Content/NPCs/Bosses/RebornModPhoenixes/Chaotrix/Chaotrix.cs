using CalamityMod;
using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Chaotrix
{
    public class Chaotrix : PhoenixAbstract
    {
        public override int damage => 30;
        public override int defense => 10;
        public override int health => 9000;
        public override int projType => ModContent.ProjectileType<FireWaveBomb>();
        public override int dustType => DustID.Torch;
        public override int invulThreshold => 600;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); crest
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); acc
            npcLoot.Add(ModContent.ItemType<HydrothermalHurl>(), 2 / 3);
        }
    }
}
