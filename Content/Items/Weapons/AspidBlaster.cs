using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class AspidBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Aspid Blaster");
            // Tooltip.SetDefault("Fires an irritating spread of infected orbs");
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 11;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item20;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 43;
            Item.knockBack = 0f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<AspidShotFriendly>();
            Item.shootSpeed = 12;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 newVel = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4 / 2, MathHelper.PiOver4 / 2, (i) / 2f));
                Projectile.NewProjectile(source, position + velocity * 5, newVel, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
