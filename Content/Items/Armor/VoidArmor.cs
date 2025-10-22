using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using CalamityMod;
using CalRemix.Content.Items.Accessories;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<DefaultDamageClass>() += 14f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VoidChestplate>() && legs.type == ModContent.ItemType<VoidLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increased damage resistance by 10%, increased life and mana by 100\n15% increased mining speed\nImmunity to On Fire!\nNormal enemies can only move in cardinal directions\nAffects bosses after defeating Noxus";
            player.Remix().voidArmor = true;
            player.endurance += 0.1f;
            player.statLifeMax2 += 100;
            player.statManaMax2 += 100;
            player.buffImmune[BuffID.OnFire] = true;
            player.pickSpeed -= 0.15f;
            if (player.equippedWings != null)
            if (player.equippedWings.type == ModContent.ItemType<VoidWings>())
            {
                player.Calamity().infiniteFlight = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<VoidSingularity>(2).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    [AutoloadEquip(EquipType.Body)]
    public class VoidChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<DefaultDamageClass>() += 0.26f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<VoidSingularity>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class VoidLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.25f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<VoidSingularity>(1).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
