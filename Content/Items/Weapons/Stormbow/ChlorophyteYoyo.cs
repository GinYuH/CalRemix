using System;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Projectiles.Weapons.Stormbow;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class ChlorophyteYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            // These are all related to gamepad controls and don't seem to affect anything else
            ItemID.Sets.Yoyo[Item.type] = true; // Used to increase the gamepad range when using Strings.
            ItemID.Sets.GamepadExtraRange[Item.type] = 4; // Increases the gamepad range. Some vanilla values: 4 (Wood), 10 (Valor), 13 (Yelets), 18 (The Eye of Cthulhu), 21 (Terrarian).
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true; // Unused, but weapons that require aiming on the screen are in this set.
        }
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
            Item.noMelee = true; // This makes it so the item doesn't do damage to enemies (the projectile does that).
            Item.noUseGraphic = true; // Makes the item invisible while using it (the projectile is the visible part).
            Item.channel = true; // Set to true for items that require the attack button to be held out (e.g. yoyos and magic missile weapons)

            item.width = 20;
            Item.height = 46;
            Item.damage = 74;
            Item.crit = 12;
            Item.useTime = 25; // All vanilla yoyos have a useTime of 25.
            Item.useAnimation = 25; // All vanilla yoyos have a useAnimation of 25.

            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<ChlorophyteYoyoProjectile>();

            Item.SetNameOverride("Chlorophyte Yoyo");
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ChlorophyteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}