using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.UI.ElementalSystem
{
    public enum Element
    {
        Neutral,
        Cold,
        Dark,
        Fire,
        Holy,
        Impact,
        Machine,
        Magic,
        Poison,
        Slash,
        Stab,
        Unholy,
        Water,
        Wind
    }
    public class ElementLists : ModSystem
    {
        public static List<Dictionary<int, Element[]>> ItemList = new();
        public static List<Dictionary<int, Tuple<Element[], Element[]>>> NPCList = new();
        public override void PostSetupContent()
        {
            //ItemList.Add(VanillaElements.Item);
            ItemList.Add(ToItemList(Mod, RemixElements.Item));
            ItemList.Add(ToItemList(CalRemix.CalMod, CalamityElements.Item));

            NPCList.Add(VanillaElements.Bosses);
            NPCList.Add(ToNPCList(Mod, RemixElements.Bosses));
            NPCList.Add(ToNPCList(CalRemix.CalMod, CalamityElements.Bosses));
        }
        private static Dictionary<int, Element[]> ToItemList(Mod mod, Dictionary<string, Element[]> list)
        {
            Dictionary<int, Element[]> newList = new();
            foreach (var entry in list) 
            {
                if (mod.TryFind(entry.Key, out ModItem item))
                    newList.Add(item.Type, entry.Value);
                else
                    Console.WriteLine($"Gasp! {entry.Key} not found in {mod.DisplayName}!");
            }
            return newList;
        }
        private static Dictionary<int, Tuple<Element[], Element[]>> ToNPCList(Mod mod, Dictionary<string, Tuple<Element[], Element[]>> list)
        {
            Dictionary<int, Tuple<Element[], Element[]>> newList = new();
            foreach (var entry in list)
            {
                if (mod.TryFind(entry.Key, out ModNPC npc))
                    newList.Add(npc.Type, entry.Value);
                else
                    Console.WriteLine($"Gasp! {entry.Key} not found in {mod.DisplayName}!");
            }
            return newList;
        }
    }
    public class ElementItem : GlobalItem
    {
        public Element[] element;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            foreach (Dictionary<int, Element[]> d in ElementLists.ItemList)
            {
                if (d.TryGetValue(item.type, out Element[] p))
                    element = p;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item is null)
                return;
            if (element is null)
                return;
            TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("ItemName"));
            if (line != null)
            {
                string eText = string.Empty;
                for (int i = 0; i < element.Length; i++)
                {
                    eText += $" [i:{nameof(CalRemix)}/{element[i]}]";
                }
                line.Text += eText;
            }
            TooltipLine j = tooltips.Find((TooltipLine t) => t.Name.Equals("JourneyResearch"));
            if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
            {
                TooltipLine tip = new(Mod, "CalRemix:ElementTip", "Hold \"Left Shift\" for element details");
                tip.OverrideColor = Main.DiscoColor;
                if (j is null)
                    tooltips.Add(tip);
                else
                    tooltips.Insert(tooltips.IndexOf(j), tip);
            }
            else
            {
                for (int i = 0; i < element.Length; i++)
                {
                    if (j is null)
                        tooltips.Add(Tooltip(element[i]));
                    else
                        tooltips.Insert(tooltips.IndexOf(j), Tooltip(element[i]));
                }
            }
        }
        private TooltipLine Tooltip(Element e)
        {
            TooltipLine l = new(Mod, "CalRemix:Elements", string.Empty);
            switch (e)
            {
                case Element.Cold:
                    l.Text = $"{Element.Cold}: Slow enemies down in speed and attack frequency, and weaken their damage";
                    l.OverrideColor = Color.LightSkyBlue;
                    break;
                case Element.Dark:
                    l.Text = $"{Element.Dark}: Incapacitate enemies, slowing their attacks and making them miss";
                    l.OverrideColor = Color.LightSlateGray;
                    break;
                case Element.Fire:
                    l.Text = $"{Element.Fire}: Set enemies on fire and reduce enemy defense";
                    l.OverrideColor = Color.Orange;
                    break;
                case Element.Holy:
                    l.Text = $"{Element.Holy}: Smite enemies, dealing massive damage";
                    l.OverrideColor = Color.Gold;
                    break;
                case Element.Impact:
                    l.Text = $"{Element.Impact}: Bash enemies and ignores some knockback resistance";
                    l.OverrideColor = Color.LimeGreen;
                    break;
                case Element.Machine:
                    l.Text = $"{Element.Machine}: Electrocutes enemies and fries the circuits of other machines";
                    l.OverrideColor = Color.LightGray;
                    break;
                case Element.Magic:
                    l.Text = $"{Element.Magic}: Hex enemies, damaging them and weakening their attacks";
                    l.OverrideColor = Color.Yellow;
                    break;
                case Element.Poison:
                    l.Text = $"{Element.Poison}: Poison enemies and reduce their damage";
                    l.OverrideColor = Color.Green;
                    break;
                case Element.Slash:
                    l.Text = $"{Element.Slash}: Enemies bleed heavily";
                    l.OverrideColor = Color.LightBlue;
                    break;
                case Element.Stab:
                    l.Text = $"{Element.Stab}: Slow enemies and puncture their organs";
                    l.OverrideColor = Color.Salmon;
                    break;
                case Element.Unholy:
                    l.Text = $"{Element.Unholy}: Curse enemies, dealing massive damage";
                    l.OverrideColor = Color.Red;
                    break;
                case Element.Water:
                    l.Text = $"{Element.Water}: Slow enemies slightly and make them begin to drown from aspiration";
                    l.OverrideColor = Color.SkyBlue;
                    break;
            }
            return l;
        }
    }
    public class ElementProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public Element[] element;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse usesource && !string.IsNullOrWhiteSpace(usesource.Item.Name) && usesource.Item.GetGlobalItem<ElementItem>().element != null)
            {
                element = usesource.Item.GetGlobalItem<ElementItem>().element;
            }
            else if (source is EntitySource_Parent parent && parent.Entity is Projectile p2 && p2.GetGlobalProjectile<ElementProj>().element != null)
            {
                element = p2.GetGlobalProjectile<ElementProj>().element;
            }
        }
    }
    public class ElementNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Element[] weak, resist;
        public override void SetDefaults(NPC npc)
        {
            foreach (Dictionary<int, Tuple<Element[], Element[]>> d in ElementLists.NPCList)
            {
                if (d.TryGetValue(npc.type, out Tuple<Element[], Element[]> p))
                {
                    if (p.Item1 != null)
                        weak = p.Item1;
                    if (p.Item2 != null)
                        resist = p.Item2;
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.GetGlobalItem<ElementItem>().element != null)
            {
                float multiplier = 1f;
                Element[] e = item.GetGlobalItem<ElementItem>().element;
                if (weak != null)
                {
                    foreach (Element w in weak)
                    {
                        if (e.Contains(w) && !player.HasBuff<ElementalAffinity>())
                            multiplier += 0.05f;
                        else if (e.Contains(w) && !player.HasBuff<ElementalAffinity>())
                            multiplier += 0.35f;
                    }
                }
                if (resist != null)
                {
                    foreach (Element r in resist)
                    {
                        if (e.Contains(r))
                            multiplier -= 0.25f;
                    }
                }
                modifiers.FinalDamage *= multiplier;
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.GetGlobalProjectile<ElementProj>().element != null)
            {
                float multiplier = 1f;
                Element[] e = projectile.GetGlobalProjectile<ElementProj>().element;
                if (weak != null)
                {
                    foreach (Element w in weak)
                    {
                        if (e.Contains(w) && !Main.player[projectile.owner].HasBuff<ElementalAffinity>())
                            multiplier += 0.05f;
                        else if (e.Contains(w) && !Main.player[projectile.owner].HasBuff<ElementalAffinity>())
                            multiplier += 0.35f;
                    }
                }
                if (resist != null)
                {
                    foreach (Element r in resist)
                    {
                        if (e.Contains(r))
                            multiplier -= 0.25f;
                    }
                }
                modifiers.FinalDamage *= multiplier;
            }
        }
    }
    public abstract class ElementIcon : ModItem
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
    public class Cold : ElementIcon { }
    public class Dark : ElementIcon { }
    public class Fire : ElementIcon { }
    public class Holy : ElementIcon { }
    public class Impact : ElementIcon { }
    public class Machine : ElementIcon { }
    public class Magic : ElementIcon { }
    public class Poison : ElementIcon { }
    public class Slash : ElementIcon { }
    public class Stab : ElementIcon { }
    public class Unholy : ElementIcon { }
    public class Water : ElementIcon { }
    public class Wind : ElementIcon { }
}
