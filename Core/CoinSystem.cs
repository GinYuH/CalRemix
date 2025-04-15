using CalRemix.Content.Items.Ammo;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core;

/// <summary>
///     Extends coin functionality to include our additional coins.
/// </summary>
internal sealed class CoinSystem : ModSystem
{
    private const long cosmilite_value  = 100000000;
    private const long klepticoin_value = 10000000000;

    public override void Load()
    {
        base.Load();

        IL_Chest.VanillaSetupShop += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));

            var moneyLoc = -1;
            c.GotoNext(x => x.MatchLdloc(out moneyLoc));

            var invIndexLoc = -1;
            c.GotoPrev(x => x.MatchLdloc(out invIndexLoc));

            ILLabel label = null;
            c.GotoPrev(x => x.MatchBneUn(out label));

            c.GotoLabel(label);

            c.EmitLdloc(moneyLoc);
            c.EmitLdloc(invIndexLoc);
            c.EmitDelegate(
                (long money, int i) =>
                {
                    var item = Main.LocalPlayer.inventory[i];

                    if (item.type == ModContent.ItemType<CosmiliteCoin>())
                    {
                        money += item.stack * cosmilite_value;
                    }
                    else if (item.type == ModContent.ItemType<Klepticoin>())
                    {
                        money += item.stack * klepticoin_value;
                    }

                    return money;
                }
            );
            c.EmitStloc(moneyLoc);
        };

        On_ContentSamples.CreativeHelper.GetItemGroup += (On_ContentSamples.CreativeHelper.orig_GetItemGroup orig, Item item, out int orderInGroup) =>
        {
            orderInGroup = 0;
            
            if (IsACoin(item))
            {
                if (item.type == ItemID.CopperCoin)
                {
                    orderInGroup = 6;
                }
                else if (item.type == ItemID.SilverCoin)
                {
                    orderInGroup = 5;
                }
                else if (item.type == ItemID.GoldCoin)
                {
                    orderInGroup = 4;
                }
                else if (item.type == ItemID.PlatinumCoin)
                {
                    orderInGroup = 3;
                }
                else if (item.type == ModContent.ItemType<CosmiliteCoin>())
                {
                    orderInGroup = 2;
                }
                else if (item.type == ModContent.ItemType<Klepticoin>())
                {
                    orderInGroup = 1;
                }

                return ContentSamples.CreativeHelper.ItemGroup.Coin;
            }
            
            return orig(item, out orderInGroup);
        };
        
        // TODO: DyeInitializer.LoadLegacyHairdyes I cannot be bothered.

        On_CustomCurrencyManager.IsCustomCurrency += (orig, item) => item.ModItem is Coin || orig(item);

        IL_Item.GetPickedUpByMonsters_Money += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));

            var valueLoc = -1;
            c.GotoNext(x => x.MatchStloc(out valueLoc));

            ILLabel label = null;
            c.GotoPrev(x => x.MatchBneUn(out label));
            
            c.GotoLabel(label);

            c.EmitLdarg0(); // this
            c.EmitLdloc(valueLoc);
            c.EmitDelegate(
                (Item item, float value) =>
                {
                    if (item.type == ModContent.ItemType<CosmiliteCoin>())
                    {
                        return cosmilite_value;
                    }
                    
                    if (item.type == ModContent.ItemType<Klepticoin>())
                    {
                        return klepticoin_value;
                    }

                    return value;
                }
            );
            c.EmitStloc(valueLoc);
        };

        IL_Main.MouseText_DrawItemTooltip_GetLinesInfo += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.CoinGun));
            
            c.GotoNext(MoveType.After, x => x.MatchCallvirt<DamageClass>(nameof(DamageClass.ShowStatTooltipLine)));

            c.EmitLdarg0();
            c.EmitDelegate(
                (bool showStatTooltipLine, Item item) => showStatTooltipLine && ((item.type != ModContent.ItemType<CosmiliteCoin>() && item.type != ModContent.ItemType<Klepticoin>()) || Main.LocalPlayer.HasItem(ItemID.CoinGun))
            );
        };
        
        // TODO: Give coins from tax collector.

        On_NPC.SpawnAllowed_Merchant += orig =>
        {
            if (NPC.unlockedMerchantSpawn)
            {
                return true;
            }

            foreach (var player in Main.ActivePlayers)
            {
                for (var i = 0; i < 58; i++)
                {
                    var item = player.inventory[i];
                    if (item is not null && item.stack > 0)
                    {
                        if (item.type == ModContent.ItemType<CosmiliteCoin>() || item.type == ModContent.ItemType<Klepticoin>())
                        {
                            return true;
                        }
                    }
                }
            }

            return orig();
        };
        
        // TODO: NPCLoot_DropMoney
        
        // TODO: Player.SellItem
        
        // TODO: Player.TryPurchasing
        
        // TODO: We can add to ExtractinatorUse maybe?
        
        // TODO: Player.DropCoins
        
        // TODO: PopupText.NewText
        
        // TODO: ChestUI.QuickStack
        
        // TODO: ChestUI.Restock
        
        // TODO: ChestUI.MoveCoins
        
        // TODO: ItemSlot.SellOrTrashItem
        
        // TODO: ItemSlot.GetOverrideInstructions
        
        // TODO: ItemSlot.PickItemMovementAction
        
        // TODO: ItemSlot.DrawMoney
        
        // TODO: ItemSorting.SortCoins
        
        // TODO: Utils.CoinsCount
    }

    private static bool IsACoin(Item item)
    {
        return item.type is ItemID.CopperCoin or ItemID.SilverCoin or ItemID.GoldCoin or ItemID.PlatinumCoin || item.type == ModContent.ItemType<CosmiliteCoin>() || item.type == ModContent.ItemType<Klepticoin>();
    }
}