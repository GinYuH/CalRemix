using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using CalRemix.Content.Items.Ammo;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace CalRemix.Core;

/// <summary>
///     Extends coin functionality to include our additional coins.
/// </summary>
internal sealed class CoinSystem : ModSystem
{
    private sealed class OverridePriceText : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);

            if (item.shopSpecialCurrency != -1)
            {
                return;
            }

            if (tooltips.FirstOrDefault(x => x.Name == "Price") is not { } priceLine)
            {
                return;
            }

            var num4 = Main.mouseTextColor / 255f;
            var a = Main.mouseTextColor;

            Main.LocalPlayer.GetItemExpectedPrice(item, out var calcForSelling, out var calcForBuying);
            var num5 = item.isAShopItem || item.buyOnce ? calcForBuying : calcForSelling;

            var text = "";
            var klepticoin = 0L;
            var cosmilite = 0L;
            var plat = 0L;
            var gold = 0L;
            var silver = 0L;
            var copper = 0L;
            var remaining = num5 * item.stack;

            if (!item.buy)
            {
                remaining = num5 / 5;
                if (remaining < 1)
                {
                    remaining = 1L;
                }
                var num11 = remaining;
                remaining *= item.stack;
                var amount = Main.shopSellbackHelper.GetAmount(item);
                if (amount > 0)
                {
                    remaining += (-num11 + calcForBuying) * Math.Min(amount, item.stack);
                }
            }

            if (remaining < 1)
            {
                remaining = 1L;
            }

            if (remaining >= klepticoin_value)
            {
                klepticoin = remaining / klepticoin_value;
                remaining -= klepticoin * klepticoin_value;
            }

            if (remaining >= cosmilite_value)
            {
                cosmilite = remaining / cosmilite_value;
                remaining -= cosmilite * cosmilite_value;
            }

            if (remaining >= 1000000)
            {
                plat = remaining / 1000000;
                remaining -= plat * 1000000;
            }

            if (remaining >= 10000)
            {
                gold = remaining / 10000;
                remaining -= gold * 10000;
            }

            if (remaining >= 100)
            {
                silver = remaining / 100;
                remaining -= silver * 100;
            }

            if (remaining >= 1)
            {
                copper = remaining;
            }

            if (klepticoin > 0)
            {
                text = text + klepticoin + " " + Language.GetTextValue("Mods.CalRemix.Currency.KlepticoinInterface") + " ";
            }

            if (cosmilite > 0)
            {
                text = text + cosmilite + " " + Language.GetTextValue("Mods.CalRemix.Currency.CosmiliteInterface") + " ";
            }

            if (plat > 0)
            {
                text = text + plat + " " + Lang.inter[15].Value + " ";
            }

            if (gold > 0)
            {
                text = text + gold + " " + Lang.inter[16].Value + " ";
            }

            if (silver > 0)
            {
                text = text + silver + " " + Lang.inter[17].Value + " ";
            }

            if (copper > 0)
            {
                text = text + copper + " " + Lang.inter[18].Value + " ";
            }

            if (!item.buy)
            {
                priceLine.Text = Lang.tip[49].Value + " " + text;
            }
            else
            {
                priceLine.Text = Lang.tip[50].Value + " " + text;
            }

            if (klepticoin > 0)
            {
                // var color = KlepticoinColor;
                // priceLine.OverrideColor = new Color((byte)(color.R * num4), (byte)(color.G * num4), (byte)(color.B * num4), a);
                priceLine.OverrideColor = KlepticoinColor;
            }
            else if (cosmilite > 0)
            {
                // var color = CosmiliteColor;
                // priceLine.OverrideColor = new Color((byte)(color.R * num4), (byte)(color.G * num4), (byte)(color.B * num4), a);
                priceLine.OverrideColor = CosmiliteColor;
            }
            else if (plat > 0)
            {
                priceLine.OverrideColor = new Color((byte)(220f * num4), (byte)(220f * num4), (byte)(198f * num4), a);
            }
            else if (gold > 0)
            {
                priceLine.OverrideColor = new Color((byte)(224f * num4), (byte)(201f * num4), (byte)(92f * num4), a);
            }
            else if (silver > 0)
            {
                priceLine.OverrideColor = new Color((byte)(181f * num4), (byte)(192f * num4), (byte)(193f * num4), a);
            }
            else if (copper > 0)
            {
                priceLine.OverrideColor = new Color((byte)(246f * num4), (byte)(138f * num4), (byte)(96f * num4), a);
            }
        }
    }

    private const long cosmilite_value = 100000000;
    private const long klepticoin_value = 10000000000;

    private static readonly Color color_cosmilite_start = new(173, 217, 202, 255);
    private static readonly Color color_cosmilite_end = new(255, 186, 206, 255);

    private static readonly Color color_klepticoin_start = new(251, 246, 138, 255);
    private static readonly Color color_klepticoin_end = new(144, 75, 21, 255);

    public static Color CosmiliteColor =>
        Color.Lerp(color_cosmilite_start, color_cosmilite_end, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);

    public static Color KlepticoinColor =>
        Color.Lerp(color_klepticoin_start, color_klepticoin_end, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);

    private static readonly (int itemId, long value)[] coin_types =
    [
        (ModContent.ItemType<Klepticoin>(), klepticoin_value),
        (ModContent.ItemType<CosmiliteCoin>(), cosmilite_value),
        (ItemID.PlatinumCoin, 1000000),
        (ItemID.GoldCoin, 10000),
        (ItemID.SilverCoin, 100),
        (ItemID.CopperCoin, 1),
    ];

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

            if (!IsACoin(item))
            {
                return orig(item, out orderInGroup);
            }

            switch (item.type)
            {
                case ItemID.CopperCoin:
                    orderInGroup = 6;
                    break;

                case ItemID.SilverCoin:
                    orderInGroup = 5;
                    break;

                case ItemID.GoldCoin:
                    orderInGroup = 4;
                    break;

                case ItemID.PlatinumCoin:
                    orderInGroup = 3;
                    break;

                default:
                {
                    if (item.type == ModContent.ItemType<CosmiliteCoin>())
                    {
                        orderInGroup = 2;
                    }
                    else if (item.type == ModContent.ItemType<Klepticoin>())
                    {
                        orderInGroup = 1;
                    }
                    break;
                }
            }

            return ContentSamples.CreativeHelper.ItemGroup.Coin;
        };

        // Too niche.
        // DyeInitializer.LoadLegacyHairdyes

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

        // Good luck getting to that amount.
        // Give coins from tax collector.

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

        On_NPC.NPCLoot_DropMoney += (_, self, player) =>
        {
            var num = 0f;
            var luck = player.luck;
            var num2 = 1L;
            if (Main.rand.NextFloat() < Math.Abs(luck))
            {
                num2 = 2;
            }
            
            for (var i = 0; i < num2; i++)
            {
                var num3 = self.value;
                if (self.midas)
                {
                    num3 *= 1f + Main.rand.Next(10, 51) * 0.01f;
                }
                num3 *= 1f + Main.rand.Next(-20, 76) * 0.01f;
                if (Main.rand.NextBool(2))
                {
                    num3 *= 1f + Main.rand.Next(5, 11) * 0.01f;
                }
                if (Main.rand != null && Main.rand.NextBool(4))
                {
                    num3 *= 1f + Main.rand.Next(10, 21) * 0.01f;
                }
                if (Main.rand.NextBool(8))
                {
                    num3 *= 1f + Main.rand.Next(15, 31) * 0.01f;
                }
                if (Main.rand.NextBool(16))
                {
                    num3 *= 1f + Main.rand.Next(20, 41) * 0.01f;
                }
                if (Main.rand.NextBool(32))
                {
                    num3 *= 1f + Main.rand.Next(25, 51) * 0.01f;
                }
                if (Main.rand.NextBool(64))
                {
                    num3 *= 1f + Main.rand.Next(50, 101) * 0.01f;
                }
                if (Main.bloodMoon)
                {
                    num3 *= 1f + Main.rand.Next(101) * 0.01f;
                }
                if (i == 0)
                {
                    num = num3;
                }
                else if (luck < 0f)
                {
                    if (num3 < num)
                    {
                        num = num3;
                    }
                }
                else if (num3 > num)
                {
                    num = num3;
                }
            }
            num += (float)self.extraValue;
            while ((int)num > 0)
            {
                if (num > klepticoin_value)
                {
                    var num4 = (int)(num / klepticoin_value);
                    if (num4 > 50 && Main.rand.NextBool(5))
                    {
                        num4 /= Main.rand.Next(3) + 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num4 /= Main.rand.Next(3) + 1;
                    }
                    var num5 = num4;
                    while (num5 > 9999)
                    {
                        num5 -= 9999;
                        Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, ModContent.ItemType<Klepticoin>(), 9999);
                    }
                    num -= klepticoin_value * num4;
                    Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, ModContent.ItemType<Klepticoin>(), num5);
                }
                else if (num > cosmilite_value)
                {
                    var num4 = (int)(num / cosmilite_value);
                    if (num4 > 50 && Main.rand.NextBool(5))
                    {
                        num4 /= Main.rand.Next(3) + 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num4 /= Main.rand.Next(3) + 1;
                    }
                    /*var num5 = num4;
                    while (num5 > 999)
                    {
                        num5 -= 999;
                        Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, 74, 999);
                    }*/
                    var num5 = num4;
                    num -= cosmilite_value * num4;
                    Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, ModContent.ItemType<CosmiliteCoin>(), num5);
                }
                else if (num > 1000000f)
                {
                    var num4 = (int)(num / 1000000f);
                    if (num4 > 50 && Main.rand.NextBool(5))
                    {
                        num4 /= Main.rand.Next(3) + 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num4 /= Main.rand.Next(3) + 1;
                    }
                    /*var num5 = num4;
                    while (num5 > 999)
                    {
                        num5 -= 999;
                        Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, 74, 999);
                    }*/
                    var num5 = num4;
                    num -= 1000000 * num4;
                    Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, 74, num5);
                }
                else if (num > 10000f)
                {
                    var num6 = (int)(num / 10000f);
                    if (num6 > 50 && Main.rand.NextBool(5))
                    {
                        num6 /= Main.rand.Next(3) + 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num6 /= Main.rand.Next(3) + 1;
                    }
                    num -= 10000 * num6;
                    Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, 73, num6);
                }
                else if (num > 100f)
                {
                    var num7 = (int)(num / 100f);
                    if (num7 > 50 && Main.rand.NextBool(5))
                    {
                        num7 /= Main.rand.Next(3) + 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num7 /= Main.rand.Next(3) + 1;
                    }
                    num -= 100 * num7;
                    Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, 72, num7);
                }
                else
                {
                    var num8 = (int)num;
                    if (num8 > 50 && Main.rand.NextBool(5))
                    {
                        num8 /= Main.rand.Next(3) + 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        num8 /= Main.rand.Next(4) + 1;
                    }
                    if (num8 < 1)
                    {
                        num8 = 1;
                    }
                    num -= num8;
                    Item.NewItem(self.GetSource_Loot(), (int)self.position.X, (int)self.position.Y, self.width, self.height, 71, num8);
                }
            }
        };

        // Probably not needed.
        // Player.SellItem

        On_Player.TryPurchasing += (
            _,
            price,
            inv,
            slotCoins,
            slotsEmpty,
            slotEmptyBank,
            slotEmptyBank2,
            slotEmptyBank3,
            slotEmptyBank4
        ) =>
        {
            var num = price;
            var dictionary = new Dictionary<Point, Item>();
            var result = false;
            while (num > 0)
            {
                var num2 = 10000000000L;
                for (var i = 0; i < 6; i++)
                {
                    if (num >= num2)
                    {
                        foreach (var slotCoin in slotCoins)
                        {
                            if (inv[slotCoin.X][slotCoin.Y].type == GetRealCoinType(76 - i))
                            {
                                var num3 = num / num2;
                                dictionary[slotCoin] = inv[slotCoin.X][slotCoin.Y].Clone();
                                if (num3 < inv[slotCoin.X][slotCoin.Y].stack)
                                {
                                    inv[slotCoin.X][slotCoin.Y].stack -= (int)num3;
                                }
                                else
                                {
                                    inv[slotCoin.X][slotCoin.Y].SetDefaults();
                                    slotsEmpty.Add(slotCoin);
                                }

                                num -= num2 * (dictionary[slotCoin].stack - inv[slotCoin.X][slotCoin.Y].stack);
                            }
                        }
                    }

                    num2 /= 100;
                }

                if (num <= 0)
                    continue;

                if (slotsEmpty.Count > 0)
                {
                    slotsEmpty.Sort(DelegateMethods.CompareYReverse);
                    var item = new Point(-1, -1);
                    for (var j = 0; j < inv.Count; j++)
                    {
                        num2 = 100000000L;
                        for (var k = 0; k < 5; k++)
                        {
                            if (num >= num2)
                            {
                                foreach (var slotCoin2 in slotCoins)
                                {
                                    if (slotCoin2.X == j && inv[slotCoin2.X][slotCoin2.Y].type == GetRealCoinType(76 - k) && inv[slotCoin2.X][slotCoin2.Y].stack >= 1)
                                    {
                                        var list = slotsEmpty;
                                        if (j == 1 && slotEmptyBank.Count > 0)
                                            list = slotEmptyBank;

                                        if (j == 2 && slotEmptyBank2.Count > 0)
                                            list = slotEmptyBank2;

                                        if (j == 3 && slotEmptyBank3.Count > 0)
                                            list = slotEmptyBank3;

                                        if (j == 4 && slotEmptyBank4.Count > 0)
                                            list = slotEmptyBank4;

                                        if (--inv[slotCoin2.X][slotCoin2.Y].stack <= 0)
                                        {
                                            inv[slotCoin2.X][slotCoin2.Y].SetDefaults();
                                            list.Add(slotCoin2);
                                        }

                                        dictionary[list[0]] = inv[list[0].X][list[0].Y].Clone();
                                        inv[list[0].X][list[0].Y].SetDefaults(GetRealCoinType(75 - k));
                                        inv[list[0].X][list[0].Y].stack = 100;
                                        item = list[0];
                                        list.RemoveAt(0);
                                        break;
                                    }
                                }
                            }

                            if (item.X != -1 || item.Y != -1)
                                break;

                            num2 /= 100;
                        }

                        for (var l = 0; l < 6; l++)
                        {
                            if (item.X != -1 || item.Y != -1)
                                continue;

                            foreach (var slotCoin3 in slotCoins)
                            {
                                if (slotCoin3.X == j && inv[slotCoin3.X][slotCoin3.Y].type == GetRealCoinType(71 + l) && inv[slotCoin3.X][slotCoin3.Y].stack >= 1)
                                {
                                    var list2 = slotsEmpty;
                                    if (j == 1 && slotEmptyBank.Count > 0)
                                        list2 = slotEmptyBank;

                                    if (j == 2 && slotEmptyBank2.Count > 0)
                                        list2 = slotEmptyBank2;

                                    if (j == 3 && slotEmptyBank3.Count > 0)
                                        list2 = slotEmptyBank3;

                                    if (j == 4 && slotEmptyBank4.Count > 0)
                                        list2 = slotEmptyBank4;

                                    if (--inv[slotCoin3.X][slotCoin3.Y].stack <= 0)
                                    {
                                        inv[slotCoin3.X][slotCoin3.Y].SetDefaults();
                                        list2.Add(slotCoin3);
                                    }

                                    dictionary[list2[0]] = inv[list2[0].X][list2[0].Y].Clone();
                                    inv[list2[0].X][list2[0].Y].SetDefaults(GetRealCoinType(70 + l));
                                    inv[list2[0].X][list2[0].Y].stack = 100;
                                    item = list2[0];
                                    list2.RemoveAt(0);
                                    break;
                                }
                            }
                        }

                        if (item.X != -1 && item.Y != -1)
                        {
                            slotCoins.Add(item);
                            break;
                        }
                    }

                    slotsEmpty.Sort(DelegateMethods.CompareYReverse);
                    slotEmptyBank.Sort(DelegateMethods.CompareYReverse);
                    slotEmptyBank2.Sort(DelegateMethods.CompareYReverse);
                    slotEmptyBank3.Sort(DelegateMethods.CompareYReverse);
                    slotEmptyBank4.Sort(DelegateMethods.CompareYReverse);
                    continue;
                }

                foreach (var item2 in dictionary)
                {
                    inv[item2.Key.X][item2.Key.Y] = item2.Value.Clone();
                }

                result = true;
                break;
            }

            return result;

            static int GetRealCoinType(int itemId)
            {
                return itemId switch
                {
                    ItemID.CopperCoin => ItemID.CopperCoin,
                    ItemID.SilverCoin => ItemID.SilverCoin,
                    ItemID.GoldCoin => ItemID.GoldCoin,
                    ItemID.PlatinumCoin => ItemID.PlatinumCoin,
                    ItemID.PlatinumCoin + 1 => ModContent.ItemType<CosmiliteCoin>(),
                    ItemID.PlatinumCoin + 2 => ModContent.ItemType<Klepticoin>(),
                    _ => throw new ArgumentOutOfRangeException(nameof(itemId), itemId, "Invalid item ID"),
                };
            }
        };

        // Too niche.
        // We can add to ExtractinatorUse maybe?

        IL_Player.DropCoins += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));

            ILLabel label = null;
            c.GotoNext(x => x.MatchBneUn(out label));

            var indexLoc = -1;
            c.GotoPrev(x => x.MatchLdloc(out indexLoc));

            var valueLoc = -1;
            c.GotoNext(x => x.MatchStloc(out valueLoc));

            var stackLoc = -1;
            c.GotoPrev(x => x.MatchLdloc(out stackLoc));

            c.GotoLabel(label);

            c.EmitLdarg0(); // this
            c.EmitLdloc(valueLoc);
            c.EmitLdloc(indexLoc);
            c.EmitLdloc(stackLoc);
            c.EmitDelegate(
                (Player player, long value, int index, int amount) =>
                {
                    if (player.inventory[index].type == ModContent.ItemType<CosmiliteCoin>())
                    {
                        value += amount * cosmilite_value;
                    }
                    else if (player.inventory[index].type == ModContent.ItemType<Klepticoin>())
                    {
                        value += amount * klepticoin_value;
                    }

                    return value;
                }
            );
            c.EmitStloc(valueLoc);
        };

        // TODO: PopupText.NewText

        IL_Main.MouseText_DrawItemTooltip += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));
            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);
        };

        IL_ChestUI.QuickStack += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));

            // cool new thing I thought of: instead of modifying the conditions,
            // just check if the type is our coin and return 74 lol

            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);

            c.Index += 2;

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));
            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);
        };

        IL_ChestUI.Restock += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));
            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);
        };

        // TODO: ChestUI.MoveCoins

        IL_ItemSlot.SellOrTrash += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));
            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);
        };

        IL_ItemSlot.GetOverrideInstructions += il =>
        {
            var c = new ILCursor(il);

            // unlucky
            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));
            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);
        };

        IL_ItemSlot.PickItemMovementAction += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));

            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);

            c.Index += 2;

            c.GotoNext(x => x.MatchLdcI4(ItemID.PlatinumCoin));
            c.GotoPrev(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type)));
            c.EmitDelegate(FakeCoinType);
        };

        On_Utils.CoinsCombineStacks += (On_Utils.orig_CoinsCombineStacks _, out bool overFlowing, long[] coinCounts) =>
        {
            var num = 0L;
            foreach (var num2 in coinCounts)
            {
                num += num2;
                /*if (num >= 999999999)
                {
                    overFlowing = true;
                    return 999999999L;
                }*/
            }
            overFlowing = false;
            return num;
        };

        On_Utils.CoinsSplit += (_, count) =>
        {
            var array = new int[6];
            var num = 0L;
            var num2 = 10000000000L;
            for (var num3 = 5; num3 >= 0; num3--)
            {
                array[num3] = (int)((count - num) / num2);
                num += array[num3] * num2;
                num2 /= 100;
            }
            return array;
        };

        On_ItemSlot.DrawMoney += (_, sb, text, shopx, shopy, coinsArray, horizontal) =>
        {
            int[] coinIds = [ModContent.ItemType<Klepticoin>(), ModContent.ItemType<CosmiliteCoin>(), ItemID.PlatinumCoin, ItemID.GoldCoin, ItemID.SilverCoin, ItemID.CopperCoin];

            Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, text, shopx, shopy + 40f, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
            if (horizontal)
            {
                for (var i = 0; i < 6; i++)
                {
                    Main.instance.LoadItem(coinIds[i]);
                    /*if (i == 0)
                    {
                        _ = coinsArray[3 - i];
                    }*/
                    var position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 24 * i + 45f, shopy + 50f);
                    sb.Draw(TextureAssets.Item[coinIds[i]].Value, position, null, Color.White, 0f, TextureAssets.Item[coinIds[i]].Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
                    Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, coinsArray[5 - i].ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
                }
            }
            else
            {
                for (var j = 0; j < 6; j++)
                {
                    Main.instance.LoadItem(coinIds[j]);
                    var num = j == 0 && coinsArray[5 - j] > 99 ? -6 : 0;
                    sb.Draw(TextureAssets.Item[coinIds[j]].Value, new Vector2(shopx + 11f + 24 * j, shopy + 75f), null, Color.White, 0f, TextureAssets.Item[coinIds[j]].Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
                    Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, coinsArray[5 - j].ToString(), shopx + 24 * j + num, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
                }
            }
        };

        // TODO: ItemSorting.SortCoins

        On_Utils.CoinsCount += (On_Utils.orig_CoinsCount orig, out bool flowing, Item[] inv, int[] ignoreSlots) =>
        {
            var coins = orig(out flowing, inv, ignoreSlots);

            for (var i = 0; i < inv.Length; i++)
            {
                if (ignoreSlots.Contains(i))
                {
                    continue;
                }
                if (inv[i].type == ModContent.ItemType<CosmiliteCoin>())
                {
                    coins += inv[i].stack * cosmilite_value;
                }
                else if (inv[i].type == ModContent.ItemType<Klepticoin>())
                {
                    coins += inv[i].stack * klepticoin_value;
                }
            }

            return coins;
        };

        IL_Item.TryCombiningIntoNearbyItems += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(MoveType.Before, x => x.MatchStloc(out _));

            c.EmitPop();
            c.EmitLdarg0();
            c.EmitDelegate((Item item) => !IsACoin(item));
        };

        On_Chest.VisualizeChestTransfer_CoinsBatch += (_, position, chestPosition, coinsMoved) =>
        {
            var array = Utils.CoinsSplit(coinsMoved);
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] >= 1)
                {
                    ParticleOrchestrator.BroadcastOrRequestParticleSpawn(
                        ParticleOrchestraType.ItemTransfer,
                        new ParticleOrchestraSettings
                        {
                            PositionInWorld = position,
                            MovementVector = chestPosition - position,
                            UniqueInfoPiece = GetCoinType(i),
                        }
                    );
                }
            }

            return;

            static int GetCoinType(int index)
            {
                return index switch
                {
                    0 => ItemID.CopperCoin,
                    1 => ItemID.SilverCoin,
                    2 => ItemID.GoldCoin,
                    3 => ItemID.PlatinumCoin,
                    4 => ModContent.ItemType<CosmiliteCoin>(),
                    5 => ModContent.ItemType<Klepticoin>(),
                    _ => throw new ArgumentOutOfRangeException(nameof(index), index, "Invalid coin index"),
                };
            }
        };

        On_CoinsRule.ToCoins += (_, money) =>
        {
            return ToCoins(money);

            static IEnumerable<(int itemId, int count)> ToCoins(long money)
            {
                var copper = (int)(money % 100);
                money /= 100;

                var silver = (int)(money % 100);
                money /= 100;

                var gold = (int)(money % 100);
                money /= 100;

                var plat = (int)(money % 100);
                money /= 100;

                var cosmilite = (int)(money % 100);
                money /= 100;

                var klepticoin = (int)money;

                if (copper > 0)
                {
                    yield return (itemId: 71, count: copper);
                }
                if (silver > 0)
                {
                    yield return (itemId: 72, count: silver);
                }
                if (gold > 0)
                {
                    yield return (itemId: 73, count: gold);
                }
                if (plat > 0)
                {
                    yield return (itemId: 74, count: plat);
                }
                if (cosmilite > 0)
                {
                    yield return (itemId: ModContent.ItemType<CosmiliteCoin>(), count: cosmilite);
                }
                if (klepticoin > 0)
                {
                    yield return (itemId: ModContent.ItemType<Klepticoin>(), count: klepticoin);
                }
            }
        };

        // Maybe not necessary, it's just for the reforge menu.
        // Main.DrawInventory

        On_Main.ValueToCoins += (_, value) =>
        {
            var remaining = value;

            var klepticoin = 0L;
            var cosmilite = 0L;
            var platinum = 0L;
            var gold = 0L;
            var silver = 0L;
            var copper = 0L;

            while (remaining >= klepticoin_value)
            {
                remaining -= klepticoin_value;
                klepticoin++;
            }

            while (remaining >= cosmilite_value)
            {
                remaining -= cosmilite_value;
                cosmilite++;
            }

            while (remaining >= 1000000)
            {
                remaining -= 1000000;
                platinum++;
            }

            while (remaining >= 10000)
            {
                remaining -= 10000;
                gold++;
            }

            while (remaining >= 100)
            {
                remaining -= 100;
                silver++;
            }

            copper = remaining;
            var text = "";

            if (klepticoin > 0)
            {
                text += $"{klepticoin} {Language.GetTextValue("Mods.CalRemix.Currency.Klepticoin").ToLower()} ";
            }

            if (cosmilite > 0)
            {
                text += $"{cosmilite} {Language.GetTextValue("Mods.CalRemix.Currency.Cosmilite").ToLower()} ";
            }

            if (platinum > 0)
            {
                text += $"{platinum} {Language.GetTextValue("Currency.Platinum").ToLower()} ";
            }

            if (gold > 0)
            {
                text += $"{gold} {Language.GetTextValue("Currency.Gold").ToLower()} ";
            }

            if (silver > 0)
            {
                text += $"{silver} {Language.GetTextValue("Currency.Silver").ToLower()} ";
            }

            if (copper > 0)
            {
                text += $"{copper} {Language.GetTextValue("Currency.Copper").ToLower()} ";
            }

            if (text.Length > 0)
            {
                text = text[..^1];
            }

            return text;
        };

        // Too niche.
        // NPCShopDatabase.RegisterStylist

        On_Player.DoCoins += (_, self, i) =>
        {
            if (self.inventory[i].stack != 100 || !IsUpgradeableCoin(self.inventory[i].type))
            {
                return;
            }

            self.inventory[i].SetDefaults(GetCoinUpgrade(self.inventory[i].type));

            for (var j = 0; j < 54; j++)
            {
                if (self.inventory[j].type != self.inventory[i].type || j == i || self.inventory[j].type != self.inventory[i].type || self.inventory[j].stack >= self.inventory[j].maxStack)
                {
                    continue;
                }

                self.inventory[j].stack++;
                self.inventory[i].SetDefaults();
                self.inventory[i].active = false;
                self.inventory[i].TurnToAir();
                self.DoCoins(j);
            }

            return;

            static bool IsUpgradeableCoin(int type)
            {
                return type is ItemID.CopperCoin or ItemID.SilverCoin or ItemID.GoldCoin or ItemID.PlatinumCoin || type == ModContent.ItemType<CosmiliteCoin>() /*|| type == ModContent.ItemType<Klepticoin>()*/;
            }

            static int GetCoinUpgrade(int type)
            {
                switch (type)
                {
                    case ItemID.CopperCoin:
                        return ItemID.SilverCoin;

                    case ItemID.SilverCoin:
                        return ItemID.GoldCoin;

                    case ItemID.GoldCoin:
                        return ItemID.PlatinumCoin;

                    case ItemID.PlatinumCoin:
                        return ModContent.ItemType<CosmiliteCoin>();
                }

                if (type == ModContent.ItemType<CosmiliteCoin>())
                {
                    return ModContent.ItemType<Klepticoin>();
                }

                throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid coin type");
            }
        };

        MonoModHooks.Add(
            typeof(Item).GetMethod("get_IsACoin", BindingFlags.Public | BindingFlags.Instance),
            IsACoin
        );

        On_Recipe.UpdateWhichItemsAreMaterials += orig =>
        {
            orig();

            for (var i = ItemID.Count; i < ItemLoader.ItemCount; i++)
            {
                if (ItemLoader.GetItem(i) is Coin)
                {
                    ItemID.Sets.IsAMaterial[i] = false;
                }
            }
        };

        // trigger re-jits below for applying our IsAnyCoin edit
        IL_Item.UpdateItem += _ => { };
        // IL_Main.DrawItem += _ => { }; // Just dust we can recreate.
        IL_Player.QuickStackAllChests += _ => { };
        IL_Player.GrabItems += _ => { };
        IL_Player.GetItemGrabRange += _ => { };
        IL_Player.PayCurrency += _ => { };
        IL_Player.ItemSpace += _ => { };
        IL_Player.GetItem += _ => { };
        IL_Player.GetItem_FillIntoOccupiedSlot_VoidBag += _ => { };
        IL_Player.GetItem_FillIntoOccupiedSlot += _ => { };
        IL_Player.GetItem_FillEmptyInventorySlot_VoidBag += _ => { };
        IL_Player.GetItem_FillEmptyInventorySlot += _ => { };
        IL_Player.ItemCheck_Inner += _ => { };
        IL_Player.ItemCheck_Shoot += _ => { };
    }

    private static bool IsACoin(Item item)
    {
        return item.type is ItemID.CopperCoin or ItemID.SilverCoin or ItemID.GoldCoin or ItemID.PlatinumCoin || item.type == ModContent.ItemType<CosmiliteCoin>() || item.type == ModContent.ItemType<Klepticoin>();
    }

    private static int FakeCoinType(int itemId)
    {
        if (itemId == ModContent.ItemType<CosmiliteCoin>() || itemId == ModContent.ItemType<Klepticoin>())
        {
            return ItemID.PlatinumCoin;
        }

        return itemId;
    }
}