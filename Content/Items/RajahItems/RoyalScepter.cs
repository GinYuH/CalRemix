using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class RoyalScepter : ModItem
    {
        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.damage = 74;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Hostile.RajahProjectiles.Carrot>();
            Item.width = 58;
            Item.height = 57;
            Item.UseSound = SoundID.Item39;
            Item.useAnimation = 30;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Yellow;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.DamageType = DamageClass.Magic;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Royal Scepter");
            // Tooltip.SetDefault("");
            Item.staff[Item.type] = true;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
		    float spread = 45f * 0.0174f;
		    float baseSpeed = (float)Math.Sqrt((velocity.X * velocity.X) + (velocity.Y * velocity.Y));
            double startAngle = Math.Atan2(velocity.X, velocity.Y) - .1d;
		    double deltaAngle = spread / 6f;
		    double offsetAngle;
		    for (int i = 0; i < 3; i++)
		    {
		    	offsetAngle = startAngle + (deltaAngle * i);
		    	int proj = Projectile.NewProjectile(source, position.X, position.Y, baseSpeed*(float)Math.Sin(offsetAngle), baseSpeed*(float)Math.Cos(offsetAngle), type, damage, knockback, Main.myPlayer);
                Main.projectile[proj].DamageType = DamageClass.Magic;
            }
		    return false;
		}
    }
}
