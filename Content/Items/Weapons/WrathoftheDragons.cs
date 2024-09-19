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
using CalRemix.Content.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
namespace CalRemix.Content.Items.Weapons
{
    public class WrathoftheDragons : ModItem
    {
        public override void SetStaticDefaults()
        {
DisplayName.SetDefault("Wrath of the Dragons");
            Tooltip.SetDefault("Summons a draconic pulse at the cursor that periodically summon two rings of homing flare dust and occasionally two flare tornadoes that turn into lingering flarenadoes upon contact with an enemy");
        }
        public override void SetDefaults()
        {
            Item.damage = 555;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DraconicPulse>();
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
                AddIngredient<WrathoftheCosmos>(1).
                AddIngredient(ModContent.ItemType<TheWand>()).
                AddIngredient(ModContent.ItemType<YharonSoulFragment>(), 6).
                AddIngredient(ModContent.ItemType<AuricBar>(), 5).
                AddIngredient(ModContent.ItemType<EffulgentFeather>(), 3).
                AddIngredient(ModContent.ItemType<CoreofSunlight>(), 10).
                AddIngredient(ModContent.ItemType<YharimBar>(), 3).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
