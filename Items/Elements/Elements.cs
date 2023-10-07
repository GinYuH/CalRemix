using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Items.Elements
{
	public abstract class Element : ModItem
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
	public class Cold : Element { }
    public class Dark : Element { }
    public class Fire : Element { }
    public class Holy : Element { }
    public class Magic : Element { }
    public class Poison : Element { }
    public class Slash : Element { }
    public class Stab : Element { }
    public class Unholy : Element { }
    public class Water : Element { }

}
