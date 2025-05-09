using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class BadEgg : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
        }

        public override void UpdateInventory(Player player)
        {
            if (Main.eclipse)
            {
                int eggCount = 0;
                foreach (var v in player.inventory)
                {
                    if (v.type == ModContent.ItemType<BadEgg>())
                    {
                        eggCount++;
                    }
                }
                foreach (var v in player.inventory)
                {
                    if (Main.rand.NextBool(600 * eggCount))
                    {
                        if (v.type == ItemID.None)
                        {
                            v.SetDefaults(Item.type);
                        }
                    }
                }
            }
        }
    }
}