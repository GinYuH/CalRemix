using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class FrilledShark : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 74;
            Item.damage = 170;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.UseSound = BetterSoundID.ItemBubbleGun3 with { Pitch = 0.5f };
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Frillet>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(source, position + (velocity * 8f).RotatedBy(MathHelper.ToRadians(16 * player.direction)), velocity + Main.rand.NextVector2Circular(6, 6), type, (int)(damage * (1 + Utils.GetLerpValue(0, Main.maxTilesY, player.Center.Y / 16, true))), knockback, player.whoAmI);
            }
            return false;
        }
    }
}
