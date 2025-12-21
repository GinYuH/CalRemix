using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using CalRemix.Content.Projectiles.Hostile.RajahProjectiles.Supreme;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.RajahItems.Supreme
{
    public class CottonCaneEX : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rajah's Rage");
            //Tooltip.SetDefault(@"Summons a Royal Rabbit to fight with you");
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.rare = 8;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<RoyalRabbit>();
            Item.shootSpeed = 10f;
            Item.buffType = ModContent.BuffType<Buffs.RoyalRabbit>();
            Item.autoReuse = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i = Main.myPlayer;
            int num73 = damage;
            float num74 = knockback;
            num74 = player.GetWeaponKnockback(Item, num74);
            player.itemTime = Item.useTime;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (player.gravDir == -1f)
            {
                num79 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
            }
            float num80 = (float)Math.Sqrt(num78 * num78 + num79 * num79);
            num78 = 0f;
            num79 = 0f;
            vector2.X = Main.mouseX + Main.screenPosition.X;
            vector2.Y = Main.mouseY + Main.screenPosition.Y;
            Projectile.NewProjectile(Item.GetSource_FromThis(), vector2, new Vector2(num78, num79), Item.shoot, num73, num74, i, 0f, 0f);
            return false;
        }
    }
}