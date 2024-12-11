using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class WindTurbineBlade : ModItem
    {
        public int hitCounter = 0;
        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.knockBack = 2;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item71;

            Item.width = 10;
            Item.height = 10;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;

            Item.shoot = ModContent.ProjectileType<WindTurbine>();
            Item.shootSpeed = 5f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (hitCounter >= 3)
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, Vector2.Normalize(velocity) * 7f, ProjectileID.WeatherPainShot, damage * 2, knockback);
                p.DamageType = DamageClass.Melee;
                hitCounter = 0;
                Projectile p2 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
                p2.scale = 2f;
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Driftorcher>()
                .AddIngredient<EssenceofBabil>(5)
                .AddIngredient(ItemID.FragmentVortex, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}