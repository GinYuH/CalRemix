using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRemix;

public static class PopupTextEdit
{
    public static Func<int, long?>? CoinValueGetter { get; set; }

    public static int NewText(On_PopupText.orig_NewText_PopupTextContext_Item_int_bool_bool orig, PopupTextContext context, Item newItem, int stack, bool noStack, bool longText)
    {
        if (!Main.showItemText)
        {
            return -1;
        }
        if (newItem.Name == null || !newItem.active)
        {
            return -1;
        }
        if (Main.netMode == NetmodeID.Server)
        {
            return -1;
        }
        var flag = CoinValueGetter?.Invoke(newItem.type).HasValue ?? false;
        for (var i = 0; i < 20; i++)
        {
            var popupText = Main.popupText[i];
            if (!popupText.active || popupText.notActuallyAnItem || (!(popupText.name == newItem.AffixName()) && (!flag || !popupText.coinText)) || popupText.NoStack || noStack)
            {
                continue;
            }
            var text = newItem.Name + " (" + (popupText.stack + stack) + ")";
            var text2 = newItem.Name;
            if (popupText.stack > 1)
            {
                text2 = text2 + " (" + popupText.stack + ")";
            }
            var vector = FontAssets.MouseText.Value.MeasureString(text2);
            vector = FontAssets.MouseText.Value.MeasureString(text);
            if (popupText.lifeTime < 0)
            {
                popupText.scale = 1f;
            }
            if (popupText.lifeTime < 60)
            {
                popupText.lifeTime = 60;
            }
            if (flag && popupText.coinText)
            {
                var num = 0L;
                /*if (newItem.type == 71)
                {
                    num += stack;
                }
                else if (newItem.type == 72)
                {
                    num += 100 * stack;
                }
                else if (newItem.type == 73)
                {
                    num += 10000 * stack;
                }
                else if (newItem.type == 74)
                {
                    num += 1000000 * stack;
                }*/
                num += (CoinValueGetter?.Invoke(newItem.type) ?? 0) * stack;
                popupText.AddToCoinValue(num);
                text = PopupText.ValueToName(popupText.coinValue);
                vector = FontAssets.MouseText.Value.MeasureString(text);
                popupText.name = text;
                // our colors are updated elsewhere
                if (popupText.coinValue >= 10000000000)
                {
                    if (popupText.lifeTime < 420)
                    {
                        popupText.lifeTime = 420;
                    }
                    popupText.color = new Color(220, 220, 198);
                }
                else if (popupText.coinValue >= 100000000)
                {
                    if (popupText.lifeTime < 360)
                    {
                        popupText.lifeTime = 360;
                    }
                    popupText.color = new Color(220, 220, 198);
                }
                else if (popupText.coinValue >= 1000000)
                {
                    if (popupText.lifeTime < 300)
                    {
                        popupText.lifeTime = 300;
                    }
                    popupText.color = new Color(220, 220, 198);
                }
                else if (popupText.coinValue >= 10000)
                {
                    if (popupText.lifeTime < 240)
                    {
                        popupText.lifeTime = 240;
                    }
                    popupText.color = new Color(224, 201, 92);
                }
                else if (popupText.coinValue >= 100)
                {
                    if (popupText.lifeTime < 180)
                    {
                        popupText.lifeTime = 180;
                    }
                    popupText.color = new Color(181, 192, 193);
                }
                else if (popupText.coinValue >= 1)
                {
                    if (popupText.lifeTime < 120)
                    {
                        popupText.lifeTime = 120;
                    }
                    popupText.color = new Color(246, 138, 96);
                }
            }
            popupText.stack += stack;
            popupText.scale = 0f;
            popupText.rotation = 0f;
            popupText.position.X = newItem.position.X + (float)newItem.width * 0.5f - vector.X * 0.5f;
            popupText.position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector.Y * 0.5f;
            popupText.velocity.Y = -7f;
            popupText.context = context;
            popupText.npcNetID = 0;
            if (popupText.coinText)
            {
                popupText.stack = 1L;
            }
            return i;
        }
        var num2 = PopupText.FindNextItemTextSlot();
        if (num2 >= 0)
        {
            var text3 = newItem.AffixName();
            if (stack > 1)
            {
                text3 = text3 + " (" + stack + ")";
            }
            var vector2 = FontAssets.MouseText.Value.MeasureString(text3);
            var popupText2 = Main.popupText[num2];
            PopupText.ResetText(popupText2);
            popupText2.active = true;
            popupText2.position.X = newItem.position.X + (float)newItem.width * 0.5f - vector2.X * 0.5f;
            popupText2.position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector2.Y * 0.5f;
            popupText2.color = Color.White;
            if (newItem.rare == 1)
            {
                popupText2.color = new Color(150, 150, 255);
            }
            else if (newItem.rare == 2)
            {
                popupText2.color = new Color(150, 255, 150);
            }
            else if (newItem.rare == 3)
            {
                popupText2.color = new Color(255, 200, 150);
            }
            else if (newItem.rare == 4)
            {
                popupText2.color = new Color(255, 150, 150);
            }
            else if (newItem.rare == 5)
            {
                popupText2.color = new Color(255, 150, 255);
            }
            else if (newItem.rare == -13)
            {
                popupText2.master = true;
            }
            else if (newItem.rare == -11)
            {
                popupText2.color = new Color(255, 175, 0);
            }
            else if (newItem.rare == -1)
            {
                popupText2.color = new Color(130, 130, 130);
            }
            else if (newItem.rare == 6)
            {
                popupText2.color = new Color(210, 160, 255);
            }
            else if (newItem.rare == 7)
            {
                popupText2.color = new Color(150, 255, 10);
            }
            else if (newItem.rare == 8)
            {
                popupText2.color = new Color(255, 255, 10);
            }
            else if (newItem.rare == 9)
            {
                popupText2.color = new Color(5, 200, 255);
            }
            else if (newItem.rare == 10)
            {
                popupText2.color = new Color(255, 40, 100);
            }
            else if (newItem.rare == 11)
            {
                popupText2.color = new Color(180, 40, 255);
            }
            else if (newItem.rare >= 12)
            {
                popupText2.color = RarityLoader.GetRarity(newItem.rare).RarityColor;
            }
            popupText2.rarity = newItem.rare;
            popupText2.expert = newItem.expert;
            popupText2.master = newItem.master;
            popupText2.name = newItem.AffixName();
            popupText2.stack = stack;
            popupText2.velocity.Y = -7f;
            popupText2.lifeTime = 60;
            popupText2.context = context;
            if (longText)
            {
                popupText2.lifeTime *= 5;
            }
            popupText2.coinValue = 0L;
            popupText2.coinText = CoinValueGetter?.Invoke(newItem.type).HasValue ?? false;
            if (popupText2.coinText)
            {
                var num3 = 0L;
                /*if (newItem.type == 71)
                {
                    num3 += popupText2.stack;
                }
                else if (newItem.type == 72)
                {
                    num3 += 100 * popupText2.stack;
                }
                else if (newItem.type == 73)
                {
                    num3 += 10000 * popupText2.stack;
                }
                else if (newItem.type == 74)
                {
                    num3 += 1000000 * popupText2.stack;
                }*/
                num3 += (CoinValueGetter?.Invoke(newItem.type) ?? 0) * popupText2.stack;
                popupText2.AddToCoinValue(num3);
                popupText2.ValueToName();
                popupText2.stack = 1L;
                // our colors are updated elsewhere
                if (popupText2.coinValue >= 10000000000)
                {
                    if (popupText2.lifeTime < 420)
                    {
                        popupText2.lifeTime = 420;
                    }
                    popupText2.color = new Color(220, 220, 198);
                }
                else if (popupText2.coinValue >= 100000000)
                {
                    if (popupText2.lifeTime < 360)
                    {
                        popupText2.lifeTime = 360;
                    }
                    popupText2.color = new Color(220, 220, 198);
                }
                else if (popupText2.coinValue >= 1000000)
                {
                    if (popupText2.lifeTime < 300)
                    {
                        popupText2.lifeTime = 300;
                    }
                    popupText2.color = new Color(220, 220, 198);
                }
                else if (popupText2.coinValue >= 10000)
                {
                    if (popupText2.lifeTime < 240)
                    {
                        popupText2.lifeTime = 240;
                    }
                    popupText2.color = new Color(224, 201, 92);
                }
                else if (popupText2.coinValue >= 100)
                {
                    if (popupText2.lifeTime < 180)
                    {
                        popupText2.lifeTime = 180;
                    }
                    popupText2.color = new Color(181, 192, 193);
                }
                else if (popupText2.coinValue >= 1)
                {
                    if (popupText2.lifeTime < 120)
                    {
                        popupText2.lifeTime = 120;
                    }
                    popupText2.color = new Color(246, 138, 96);
                }
            }
        }
        return num2;
    }
}