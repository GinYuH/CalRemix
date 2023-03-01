using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix
{
    public static class CalRemixHelper
    {
        public static void ConsumeStack(this Player player, int itemType, int stackNum)
        {
            int peppino = stackNum;
            for(int i = 0; i < 58; i++)
            {
                ref Item item = ref player.inventory[i];
                if(item.type == itemType && item.stack >= stackNum)
                {
                    for (int j = 0; j < stackNum; j++)
                    {
                        item.stack--;
                        peppino--;
                        Main.NewText(peppino);
                    }
                }
                if (item.stack <= 0) item = new Item();
                if (peppino <= 0) break;
            }
        }
    }
}
