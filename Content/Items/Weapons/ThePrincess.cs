using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class ThePrincess : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 102;
            Item.height = 112;
            Item.damage = 166;
            Item.knockBack = 4.25f;
            Item.shootSpeed = 23.5f;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 12;
            Item.useTime = (Item.useAnimation = 21);
            Item.useStyle = 5;
            Item.autoReuse = true;
            Item.UseSound = SoundID.DD2_FlameburstTowerShot;
            Item.shoot = ModContent.ProjectileType<PrincessFlame>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.Calamity().donorItem = true;
            Item.rare = ModContent.RarityType<Turquoise>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 position2 = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            position2 += velocity.SafeNormalize(Vector2.Zero) * 105f;
            Projectile.NewProjectile(source, position2, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<ArchAmaryllis>().AddIngredient<DivineGeode>(15).AddIngredient<UnholyEssence>(10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
