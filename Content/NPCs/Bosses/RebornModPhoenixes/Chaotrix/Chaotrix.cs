using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes;
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
        public override int projType => ModContent.ProjectileType<BrimstoneBall>();
        public override int dustType => DustID.Torch;
    }
}
