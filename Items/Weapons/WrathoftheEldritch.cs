using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
namespace CalRemix.Items.Weapons
{
    public class WrathoftheEldritch : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons an eldritch pulse at the cursor that periodically summon two rings of homing spirits and occasionally a deathray");
        }
        public override void SetDefaults()
        {
            Item.damage = 98;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 30;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EldritchPulse>();
            Item.shootSpeed = 11f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WrathoftheAncients>(1).
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient(ItemID.FragmentNebula, 12).
                AddIngredient(ModContent.ItemType<ExodiumCluster>(), 15).
                AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
