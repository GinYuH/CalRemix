using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items.Placeables;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class Grakit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemLaserMachinegun;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 54;
            Item.knockBack = 4.5f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<GrakitBolt>();
            Item.shootSpeed = 17f;
            Item.mana = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.PiOver4 * 0.25f), type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
