using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class HalbardoftheHolidays : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Halberd of the Holidays");
            // Tooltip.SetDefault("idk I'm still miserable with names\n- The General\nFires exploding red and green bouncing gift bags that emit clouds as they travel");
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 298;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useTurn = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(100);
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.shoot = ModContent.ProjectileType<RedBall>();
            Item.shootSpeed = 18f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dustType = 0;
            switch (Main.rand.Next(4))
            {
                case 1:
                    dustType = 107;
                    break;
                case 2:
                    dustType = 90;
                    break;
            }
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(3))
            {
                type = ModContent.ProjectileType<GreenBall>();
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer);
            return false;
        }
    }
}