using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class KnowledgeExcavator : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("The Wulfrum Excavator");
            Tooltip.SetDefault("The first big project of the legendary weaponsmith, Draedon.\n" +
            "Draedon always had an odd fascination with making serpentine drilling units, some of the later models can even be found in the caverns."); 
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = 1;
        }
    }
}
