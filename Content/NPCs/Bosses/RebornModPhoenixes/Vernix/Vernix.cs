using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.RebornModMiracleVine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix
{
    public class Vernix : PhoenixAbstract
    {
        public override int damage => 30;
        public override int defense => 10;
        public override int health => 9000;
        public override int projType => ModContent.ProjectileType<PerennialFlowerMine>();
        public override int dustType => DustID.JungleSpore;
    }
}