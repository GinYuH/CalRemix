using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix
{
    public static class CalRemixHelper
    {
        public static bool HasStack(this Player player, int itemType, int stackNum)
        {
            for (int i = 0; i < 58; i++)
            {
                Item item = player.inventory[i];
                if (item.type == itemType) { if (item.stack >= stackNum) return true; }
            }
            return false;
        }
        public static void ConsumeStack(this Player player, int itemType, int stackNum)
        {
            for(int i = 0; i < 58; i++)
            {
                ref Item item = ref player.inventory[i];
                if (player.HasStack(itemType, stackNum)) item.stack -= stackNum;
            }
        }

        public static bool HasItems(this Player player, List<int> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!player.HasItem(items[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool HasCrossModItem(Player player, string ModName, string ItemName)
        {
            if (ModLoader.HasMod(ModName))
            {
                if (player.HasItem(ModLoader.GetMod(ModName).Find<ModItem>(ItemName).Type))
                    return true;
            }
            return false;
        }

    }
}
