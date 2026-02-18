using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems.Supreme
{

    public class BunzookaEX : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("RPG");
            /* Tooltip.SetDefault(@""); */
        }

        public override void SetDefaults()
        {
            Item.damage = 550;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 28;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; //so the Item's animation doesn't do damage
            Item.knockBack = 7.5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = Mod.Find<ModProjectile>("RabbitRocketEX").Type;
            Item.useAmmo = AmmoID.Rocket;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true; Item.expertOnly = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -6);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>("RabbitRocketEX").Type, damage, knockback, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}