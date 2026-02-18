using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;
namespace CalRemix.UI.ElementalSystem;
public class ElementDisplay : InfoDisplay
{
    public override void Load()
    {
        IL_Main.DrawInfoAccs += ElementDisplayProperly;
    }
    private void ElementDisplayProperly(ILContext il)
    {
        var cursor = new ILCursor(il);
        Type[] paramTypes = [typeof(SpriteBatch), typeof(DynamicSpriteFont), typeof(string), typeof(Vector2), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffects), typeof(float)];
        MethodInfo drawStringMethod = typeof(DynamicSpriteFontExtensionMethods).GetMethod("DrawString", BindingFlags.Public | BindingFlags.Static, paramTypes);

        for (int b = 0; b < 2; b++)
        {
            if (!cursor.TryGotoNext(MoveType.Before, i => i.MatchCall(drawStringMethod)))
                return;
            cursor.Remove();
            cursor.EmitDelegate(DrawString);
        }
    }
    public static void DrawString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color drawColor, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        if (text == "ELEMENTINFO")
        {
            int stat = Main.LocalPlayer.GetModPlayer<ElementPlayer>().elementStat;
            NPC npc = Main.LocalPlayer.GetModPlayer<ElementPlayer>().ETrack;
            Element[] e = (stat >= 0) ? npc.GetGlobalNPC<ElementNPC>().weak : npc.GetGlobalNPC<ElementNPC>().resist;
            string s = (stat >= 0) ? CalRemixHelper.LocalText("UI.Element.Weak").Value : CalRemixHelper.LocalText("UI.Element.Resist").Value;

            List<string> elementIcons = [];
            foreach (Element element in e)
                elementIcons.Add($"[i:CalRemix/{element}]");

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, s, position, drawColor, rotation, origin, scale);
            position.X += (stat >= 0) ? 56 : 64;

            for (int i = 0; i < elementIcons.Count; i++)
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, elementIcons[i], position, drawColor, rotation, origin, scale);
                position.X += 36;
            }
        }
        else
            spriteBatch.DrawString(font, text, position, drawColor, rotation, origin, scale, effects, layerDepth);
    }
    public override bool Active() => Main.LocalPlayer.GetModPlayer<ElementPlayer>().EDisplay;
    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        NPC npc = Main.LocalPlayer.GetModPlayer<ElementPlayer>().ETrack;
        if (npc is null)
        {
            displayColor = InactiveInfoTextColor;
            return CalRemixHelper.LocalText("UI.Element.None").Value;
        }
        if (!npc.TryGetGlobalNPC(out ElementNPC _))
        {
            displayColor = InactiveInfoTextColor;
            return CalRemixHelper.LocalText("UI.Element.NoInfo").Value;
        }
        if (npc.GetGlobalNPC<ElementNPC>().weak is null && npc.GetGlobalNPC<ElementNPC>().resist is null || Main.LocalPlayer.GetModPlayer<ElementPlayer>().noElement)
        {
            displayColor = InactiveInfoTextColor;
            return CalRemixHelper.LocalText("UI.Element.NoInfo").Value;
        }
        displayColor = Color.White;
        return "ELEMENTINFO";
    }
}
public class ElementPlayer : ModPlayer
{
    public NPC ETrack;
    public bool EDisplay;
    public bool noElement = true;
    public int elementStat = 0;
    public override void ResetEffects()
    {
        EDisplay = false;
    }
    public override void UpdateEquips()
    {
        EDisplay = true;
    }
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        ETrack = target;
        if (item.GetGlobalItem<ElementItem>().element is null)
        {
            noElement = true;
            return;
        }
        noElement = false;
        Element[] e = item.GetGlobalItem<ElementItem>().element;
        Element[] weak = target.GetGlobalNPC<ElementNPC>().weak;
        Element[] resist = target.GetGlobalNPC<ElementNPC>().resist;
        int tempStat = 0;
        if (weak != null)
        {
            foreach (Element w in weak)
            {
                if (e.Contains(w))
                    tempStat++;
            }
        }
        if (resist != null)
        {
            foreach (Element r in resist)
            {
                if (e.Contains(r))
                    tempStat--;
            }
        }
        elementStat = tempStat;
    }
    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        ETrack = target;
        if (proj.GetGlobalProjectile<ElementProj>().element is null)
        {
            noElement = true;
            return;
        }
        noElement = false;
        Element[] e = proj.GetGlobalProjectile<ElementProj>().element;
        Element[] weak = target.GetGlobalNPC<ElementNPC>().weak;
        Element[] resist = target.GetGlobalNPC<ElementNPC>().resist;
        int tempStat = 0;
        if (weak != null)
        {
            foreach (Element w in weak)
            {
                if (e.Contains(w))
                    tempStat++;
            }
        }
        if (resist != null)
        {
            foreach (Element r in resist)
            {
                if (e.Contains(r))
                    tempStat--;
            }
        }
        elementStat = tempStat;
    }
}
