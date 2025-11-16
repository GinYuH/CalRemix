using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.Items.Weapons
{
    public class MassBlaster : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemGrenadeChuck with { Pitch = -0.5f };
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 103;
            Item.knockBack = 8f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<AspidShotFriendly>();
            Item.shootSpeed = 18;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 newVel = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.ToRadians(0.5f), MathHelper.ToRadians(0.5f), (i) / 2f));
                int p = Projectile.NewProjectile(source, position + velocity * 2, newVel, type, damage, knockback, player.whoAmI, ai0: 1);
                Main.projectile[p].extraUpdates = 2;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AspidBlaster>())
                .AddIngredient(ModContent.ItemType<GildedShard>(), 5)
                .AddIngredient(ModContent.ItemType<LightResidue>(), 20)
                .AddIngredient(ModContent.ItemType<Mercury>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
