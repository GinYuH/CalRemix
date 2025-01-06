using CalamityMod;
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
        public static Dictionary<Predicate<Player>, Element> soulBoosts = new Dictionary<Predicate<Player>, Element>() { };
        public override void PostSetupContent()
        {
            //ItemList.Add(VanillaElements.Item);
            ItemList.Add(ToItemList(Mod, RemixElements.Item));
            ItemList.Add(ToItemList(CalRemix.CalMod, CalamityElements.Item));

            NPCList.Add(VanillaElements.Bosses);
            NPCList.Add(ToNPCList(Mod, RemixElements.Bosses));
            NPCList.Add(ToNPCList(CalRemix.CalMod, CalamityElements.Bosses));
            
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().pathogenSoul, Element.Unholy);
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().pyrogenSoul, Element.Fire);
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().oxygenSoul, Element.Wind);
            soulBoosts.Add((Player p) => p.Calamity() != null && p.Calamity().cryogenSoul, Element.Cold);
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().carcinogenSoul, Element.Dark);
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().phytogenSoul, Element.Poison);
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().hydrogenSoul, Element.Water);
            soulBoosts.Add((Player p) => p.Remix() != null && p.Remix().ionogenSoul, Element.Machine);
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
                TooltipLine tip = new(Mod, "CalRemix:ElementTip", CalRemixHelper.LocalText("Items.Tooltips.ElementTip").Value);
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
                    l.Text = ElementText(Element.Cold);
                    l.OverrideColor = Color.LightSlateGray;
                    break;
                case Element.Dark:
                    l.Text = ElementText(Element.Dark);
                    l.OverrideColor = Color.LightGray;
                    break;
                case Element.Fire:
                    l.Text = ElementText(Element.Fire);
                    l.OverrideColor = Color.Orange;
                    break;
                case Element.Holy:
                    l.Text = ElementText(Element.Holy);
                    l.OverrideColor = Color.Gold;
                    break;
                case Element.Impact:
                    l.Text = ElementText(Element.Impact);
                    l.OverrideColor = Color.LimeGreen;
                    break;
                case Element.Machine:
                    l.Text = ElementText(Element.Machine);
                    l.OverrideColor = Color.LightGray;
                    break;
                case Element.Magic:
                    l.Text = ElementText(Element.Magic);
                    l.OverrideColor = Color.Yellow;
                    break;
                case Element.Poison:
                    l.Text = ElementText(Element.Poison);
                    l.OverrideColor = Color.Green;
                    break;
                case Element.Slash:
                    l.Text = ElementText(Element.Slash);
                    l.OverrideColor = Color.LightBlue;
                    break;
                case Element.Stab:
                    l.Text = ElementText(Element.Stab);
                    l.OverrideColor = Color.Salmon;
                    break;
                case Element.Unholy:
                    l.Text = ElementText(Element.Unholy);
                    l.OverrideColor = Color.Red;
                    break;
                case Element.Water:
                    l.Text = ElementText(Element.Water);
                    l.OverrideColor = Color.SkyBlue;
                    break;
                case Element.Wind:
                    l.Text = ElementText(Element.Wind);
                    l.OverrideColor = Color.Tan;
                    break;
            }
            return l;
        }
        private static string ElementText(Element e) => $"{e}: {CalRemixHelper.LocalText($"Items.{e}.Tooltip").Value}";
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
                bool origen = player.Remix().origenSoul;
                if (weak != null)
                {
                    foreach (Element w in weak)
                    {
                        if (e.Contains(w) && !player.HasBuff<ElementalAffinity>())
                            multiplier += ElementWithSoul(player, 0.05f, w);
                        else if (e.Contains(w) && !player.HasBuff<ElementalAffinity>())
                            multiplier += ElementWithSoul(player, 0.35f, w);
                    }
                }
                if (resist != null)
                {
                    foreach (Element r in resist)
                    {
                        if (e.Contains(r))
                            multiplier += ElementWithSoul(player, -0.25f, r);

                    }
                }
                modifiers.FinalDamage *= multiplier;
            }
        }

        public float ElementWithSoul(Player player, float baseMult, Element element)
        {
            foreach (var v in ElementLists.soulBoosts)
            {
                if (v.Key.Invoke(player) && v.Value == element)
                {
                    baseMult *= 1.22f;
                }
            }
            if (player.Remix().origenSoul)
            {
                baseMult *= -0.5f;
            }
            return baseMult;
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
                            multiplier += ElementWithSoul(Main.player[projectile.owner], 0.05f, w);
                        else if (e.Contains(w) && !Main.player[projectile.owner].HasBuff<ElementalAffinity>())
                            multiplier += ElementWithSoul(Main.player[projectile.owner], 0.35f, w);
                    }
                }
                if (resist != null)
                {
                    foreach (Element r in resist)
                    {
                        if (e.Contains(r))
                            multiplier += ElementWithSoul(Main.player[projectile.owner], -0.25f, r);
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
