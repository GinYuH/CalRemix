using CalRemix.Content.Cooldowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalamityMod;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ElementalHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<DefaultDamageClass>() += 25;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ElementalChestplate>() && legs.type == ModContent.ItemType<ElementalLeggings>();
        }

        public static List<int> everyModdedAccessory = new();

        public override void UpdateArmorSet(Player player)
        {
            if (everyModdedAccessory.Count <= 0)
            {
                foreach (Item i in ContentSamples.ItemsByType.Values)
                {
                    if (i.accessory && i.ModItem != null && i.rare != ModContent.RarityType<HotPink>())
                    {
                        everyModdedAccessory.Add(i.type);
                    }
                }
            }
            else
            {
                ItemLoader.GetItem(Utils.SelectRandom(Main.rand, everyModdedAccessory.ToArray())).UpdateAccessory(player, false);
            }
            player.setBonus = "Gives a random accessory effect every frame";
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalBar>(10).
                AddIngredient<WaterMesh>(12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
