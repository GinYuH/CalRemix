using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using CalRemix.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;

namespace CalRemix.Items.Weapons
{
    public class Homochromia : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Homochromia");
            Tooltip.SetDefault("No homo.\n" +
                "90% chance to not consume ammo\n" +
                "100% chance to turn you gay immediately");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 56;
            Item.value = 100000;
            Item.rare = 8;
            Item.damage = 50;
            Item.useAnimation = 84;
            Item.useTime = 2;
            Item.useLimitPerAnimation = 7 * 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 30f;
            Item.UseSound = SoundID.Item102;
            Item.useAmmo = AmmoID.Arrow;
        }

        private int shotCount = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            List<float> spread = new List<float> { -45 / 3, 45 / 3, -45 / 6, 45 / 6, -45 / 12, 45 / 12, 0 };
            velocity *= (Main.rand.Next(100, 151) / 100f);
            Projectile.NewProjectile(source, position, velocity.RotatedBy((spread[shotCount % 7] * (Math.PI / 180))), ProjectileID.MoonlordArrow, damage, knockback, player.whoAmI);
            shotCount++;
            return false;
        }



        public override bool CanConsumeAmmo(Item ammo, Player player)
    {
        return Main.rand.NextFloat() >= 0.90f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Heterochromia>().
                AddIngredient(ItemID.FairyQueenRangedItem).
                AddIngredient<DarkechoGreatbow>().
                AddIngredient<ElementalBar>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}