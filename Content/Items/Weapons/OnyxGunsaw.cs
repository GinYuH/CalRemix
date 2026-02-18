using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalRemix.Content.Items.Weapons
{
    public class OnyxGunsaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Onyx Overlord's Gunsaw");
            // Tooltip.SetDefault("Projects a directed stream of onyx sawblasts while firing bullets rapidly\nThis weapon and its projectiles function as a chainsaw");
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 134;
            Item.height = 54;
            Item.damage = 6514;
            Item.knockBack = 15f;
            Item.useTime = 5;
            Item.useAnimation = 23;
            // In-game, the displayed axe power is 5x the value set here.
            // This corrects for trees having 500% hardness internally.
            // So that the axe power in the code looks like the axe power you see on screen, divide by 5.
            Item.axe = 9001 / 5;
            // Photon Ripper's axe power is entirely for show. Its projectiles instantly one shot trees.

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<OnyxGunsawProjectile>();
            Item.shootSpeed = 1f;

            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 18;

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool AltFunctionUse(Player player) => true;
        public override void HoldItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                player.Calamity().rightClickListener = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float breakBlocks = 1;
            // If right clicking, the chainsaw won't be able to chop down trees
            if (player.Calamity().mouseRight && player.whoAmI == Main.myPlayer && !Main.mapFullscreen && !Main.blockMouse)
            {
                breakBlocks = 0;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f, breakBlocks);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe.Create(Type).
                AddIngredient(ModContent.ItemType<OnyxGunblade>()).
                AddIngredient(ModContent.ItemType<Onyxia>()).
                AddIngredient(ModContent.ItemType<PhotonRipper>()).
                AddIngredient(ModContent.ItemType<ShadowspecBar>()).
                AddTile(ModContent.TileType<DraedonsForge>()).Register();
        }
    }
}
