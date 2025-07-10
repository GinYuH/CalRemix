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

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CarnelianWoodHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<DefaultDamageClass>() += 0.2f;
            player.GetCritChance<DefaultDamageClass>() += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CarnelianWoodChestplate>() && legs.type == ModContent.ItemType<CarnelianWoodLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Eating cookies restores your life to full with no cooldown";
            player.Remix().carnelian = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CarnelianWood>(12).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
