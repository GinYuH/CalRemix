using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Gores
{
    public class WulfrumLeechDaggerGore0 : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
        }
    }
    public class WulfrumLeechDaggerGore1 : ModGore {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
        }
    }
}
