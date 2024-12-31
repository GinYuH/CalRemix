using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalRemix.Content.Items.Misc
{
    public abstract class EmptyItem : ModItem
    {
        public override void UpdateInventory(Player player)
        {
            Item.stack = 0;
            Item.active = false;
        }
        public override void PostUpdate()
        {
            Item.active = false;
        }
    }
    public class Those : EmptyItem { }
    public class Who : EmptyItem { }
    public class Know : EmptyItem { }
}
