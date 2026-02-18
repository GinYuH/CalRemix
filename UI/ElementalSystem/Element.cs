using CalamityMod;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRemix.UI.ElementalSystem;
public enum Element
{
    Neutral,
    Adhesive,
    Blood,
    Bomb,
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
    internal static Dictionary<int, Element[]> ItemList = [];
    internal static Dictionary<int, Tuple<Element[], Element[]>> NPCList = [];
    internal static Dictionary<Predicate<Player>, Element> soulBoosts = [];
    public override void PostSetupContent()
    {
        foreach (var entry in VanillaElements.Item)
            ItemList.Add(entry.Key, entry.Value);
        AddToItemList(Mod, RemixElements.Item);
        AddToItemList(CalRemix.CalMod, CalamityElements.Item);

        foreach (var entry in VanillaElements.Bosses)
            NPCList.Add(entry.Key, entry.Value);
        AddToNPCList(Mod, RemixElements.Bosses);
        AddToNPCList(CalRemix.CalMod, CalamityElements.Bosses);
        
        soulBoosts.Add(p => p.Remix() != null && p.Remix().pathogenSoul, Element.Unholy);
        soulBoosts.Add(p => p.Remix() != null && p.Remix().pyrogenSoul, Element.Fire);
        soulBoosts.Add(p => p.Remix() != null && p.Remix().oxygenSoul, Element.Wind);
        soulBoosts.Add(p => p.Calamity() != null && p.Calamity().cryogenSoul, Element.Cold);
        soulBoosts.Add(p => p.Remix() != null && p.Remix().carcinogenSoul, Element.Dark);
        soulBoosts.Add(p => p.Remix() != null && p.Remix().phytogenSoul, Element.Poison);
        soulBoosts.Add(p => p.Remix() != null && p.Remix().hydrogenSoul, Element.Water);
        soulBoosts.Add(p => p.Remix() != null && p.Remix().ionogenSoul, Element.Machine);

        CheckItemElements();
        CheckNPCElements();
    }
    private static void AddToItemList(Mod mod, Dictionary<string, Element[]> list)
    {
        foreach (var entry in list) 
        {
            if (mod.TryFind(entry.Key, out ModItem item) && !ItemList.ContainsKey(item.Type))
                ItemList.Add(item.Type, entry.Value);
            else
                Console.WriteLine($"Gasp! {entry.Key} not found in {mod.DisplayName}!");
        }
    }
    private static void AddToNPCList(Mod mod, Dictionary<string, Tuple<Element[], Element[]>> list)
    {
        foreach (var entry in list)
        {
            if (mod.TryFind(entry.Key, out ModNPC npc) && !NPCList.ContainsKey(npc.Type))
                NPCList.Add(npc.Type, entry.Value);
            else
                Console.WriteLine($"Gasp! {entry.Key} not found in {mod.DisplayName}!");
        }
    }
    private static void CheckItemElements()
    {
        string list = "The following items do not have any elements: ";
        foreach (Item i in ContentSamples.ItemsByType.Values)
        {
            if (i is null)
                continue;
            if (!i.accessory && i.damage > 0 && i.useTime > 0 && i.useAnimation > 0 && !ItemList.TryGetValue(i.type, out _))
                list += $"{i.Name}, ";
        }
        Console.WriteLine(list);
    }
    private static void CheckNPCElements()
    {
        string list = "The following NPCs do not have any elements: ";
        foreach (NPC n in ContentSamples.NpcsByNetId.Values)
        {
            if (n is null)
                continue;
            if (!NPCList.TryGetValue(n.type, out _))
                list += $"{n.GetTypeNetName()}, ";
        }
        Console.WriteLine(list);
    }
}
public class ElementItem : GlobalItem
{
    public Element[] element;
    public override bool InstancePerEntity => true;
    public override void SetDefaults(Item item)
    {
        if (ElementLists.ItemList.TryGetValue(item.type, out Element[] e))
            element = e;
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item is null)
            return;
        if (element is null)
            return;
        TooltipLine line = tooltips.Find(t => t.Name.Equals("ItemName"));
        if (line != null)
        {
            string eText = string.Empty;
            for (int i = 0; i < element.Length; i++)
            {
                eText += $" [i:{nameof(CalRemix)}/{element[i]}]";
            }
            line.Text += eText;
        }
        TooltipLine j = tooltips.Find(t => t.Name.Equals("JourneyResearch"));
        if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
        {
            TooltipLine tip = new(Mod, "CalRemix:ElementTip", CalRemixHelper.LocalText("Items.Tooltips.ElementTip").Value)
            {
                OverrideColor = Main.DiscoColor
            };
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
            case Element.Adhesive:
                l.Text = ElementText(Element.Cold);
                l.OverrideColor = Color.Pink;
                break;
            case Element.Blood:
                l.Text = ElementText(Element.Dark);
                l.OverrideColor = Color.DarkRed;
                break;
            case Element.Bomb:
                l.Text = ElementText(Element.Cold);
                l.OverrideColor = Color.Turquoise;
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
        if (ElementLists.NPCList.TryGetValue(npc.type, out Tuple<Element[], Element[]> e))
        {
            if (e.Item1 != null)
                npc.GetGlobalNPC<ElementNPC>().weak = e.Item1;
            if (e.Item2 != null)
                npc.GetGlobalNPC<ElementNPC>().resist = e.Item2;
        }
    }
    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if (item.GetGlobalItem<ElementItem>().element != null)
            modifiers.FinalDamage *= MultiplierCalculation(item.GetGlobalItem<ElementItem>().element, player);
    }
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if (projectile.GetGlobalProjectile<ElementProj>().element != null)
            modifiers.FinalDamage *= MultiplierCalculation(projectile.GetGlobalProjectile<ElementProj>().element, Main.player[projectile.owner]);
    }
    public float MultiplierCalculation(Element[] e, Player player)
    {
        float multiplier = 0f;
        if (weak != null)
        {
            foreach (Element w in weak)
            {
                if (e.Contains(w))
                    multiplier += player.Remix().origenSoul ? -1 : 1;
            }
        }
        if (resist != null)
        {
            foreach (Element r in resist)
            {
                if (e.Contains(r))
                    multiplier += player.Remix().origenSoul ? 1 : -1;
            }
        }
        multiplier *= (multiplier > 0) ? (!player.HasBuff<ElementalAffinity>()) ? 0.05f : 0.15f : (!player.HasBuff<ElementalAffinity>()) ? 0.1f : 0.01f;
        multiplier += 1f;
        foreach (var v in ElementLists.soulBoosts)
        {
            if (v.Key.Invoke(player))
            {
                if (weak != null)
                {
                    if (weak.Contains(v.Value))
                    {
                        multiplier *= 1.22f;
                        continue;
                    }
                }
                if (resist != null)
                {
                    if (resist.Contains(v.Value))
                        multiplier *= 1.22f;
                }
            }
        }
        return multiplier;
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
public class Adhesive : ElementIcon { }
public class Blood : ElementIcon { }
public class Bomb : ElementIcon { }
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