using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class AbstractTest : AbstractStormbowClass
    {
        public override int damage => 12;
        public override int crit => 4;
        public override float shootSpeed => 12;
        public override int useTime => 32;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.WoodenArrowFriendly, ProjectileID.ChlorophyteArrow };
        public override int arrowAmount => 12;
        public override OverallRarity overallRarity => OverallRarity.Green;
    }
}
