using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
namespace CalRemix.Content.Items.Weapons
{
    public class WrathoftheCosmos : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a galactic pulse at the cursor that periodically summon two rings of homing god slayer fireballs and occasionally two cosmic guardians");
        }
        public override void SetDefaults()
        {
            Item.damage = 220;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GodSlayerPulse>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WrathoftheEldritch>(1).
                AddIngredient(ModContent.ItemType<StarShower>()).
                AddIngredient(ModContent.ItemType<CosmiliteBar>(), 12).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 15).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
