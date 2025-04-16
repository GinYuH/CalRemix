using System;
using System.Reflection;

using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core;

public class ItemGrabListener : ModSystem
{
    public static readonly int[] BEING_GRABBED_BY = new int[Main.maxItems];

    public override void Load()
    {
        base.Load();

        On_Player.PullItem_Common += (orig, self, item, speed) =>
        {
            orig(self, item, speed);

            BEING_GRABBED_BY[item.whoAmI] = self.whoAmI;
        };

        On_Player.PullItem_Pickup += (orig, self, item, speed, acc) =>
        {
            orig(self, item, speed, acc);

            BEING_GRABBED_BY[item.whoAmI] = self.whoAmI;
        };

        On_Player.PullItem_ToVoidVault += (orig, self, item) =>
        {
            orig(self, item);

            BEING_GRABBED_BY[item.whoAmI] = self.whoAmI;
        };

        MonoModHooks.Add(
            typeof(ItemLoader).GetMethod(nameof(ItemLoader.GrabStyle), BindingFlags.Public | BindingFlags.Static)!,
            (Func<Item, Player, bool> orig, Item item, Player player) =>
            {
                if (!orig(item, player))
                {
                    return false;
                }

                BEING_GRABBED_BY[item.whoAmI] = player.whoAmI;
                return true;
            }
        );
    }

    public override void PreUpdatePlayers()
    {
        base.PreUpdatePlayers();

        for (var i = 0; i < BEING_GRABBED_BY.Length; i++)
        {
            BEING_GRABBED_BY[i] = -1;
        }
    }
}