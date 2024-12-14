using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Weapons.DraedonsArsenal;

namespace CalRemix.Content.Items.Weapons
{
    public class EXOLOTLStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 350;
            Item.mana = 350;
            Item.width = 40;
            Item.height = 42;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.UseSound = SoundID.Item113;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EXOLOTL>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AtlasMunitionsBeacon>().
                AddIngredient<HeavyLaserRifle>().
                AddIngredient<PulsePistol>().
                AddIngredient<ExoPrism>(10).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
