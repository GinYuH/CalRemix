using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems.Supreme
{

    public class RabbitsWrath : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rabbit's Wrath");
            // Tooltip.SetDefault("Drops razor sharp carrots on your foes");
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 6;
            Item.useAnimation = 10;
            Item.reuseDelay = 10;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = .5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
            Item.shoot = Mod.Find<ModProjectile>("CarrotEX").Type;
            Item.rare = 9;
            //AARarity = 14;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<Terraria.ModLoader.TooltipLine> list)
        {
            foreach (Terraria.ModLoader.TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = new Color(255, 22, 0);
                }
            }
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -2);
		}
		
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector12 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            float num75 = Item.shootSpeed;
            for (int num120 = 0; num120 < 3; num120++)
            {
                Vector2 vector2 = player.Center + new Vector2(-(float)Main.rand.Next(0, 401) * player.direction, -600f);
                vector2.Y -= 100 * num120;
                Vector2 vector13 = vector12 - vector2;
                if (vector13.Y < 0f)
                {
                    vector13.Y *= -1f;
                }
                if (vector13.Y < 20f)
                {
                    vector13.Y = 20f;
                }
                vector13.Normalize();
                vector13 *= num75;
                float num82 = vector13.X;
                float num83 = vector13.Y;
                float speedX5 = num82;
                float speedY6 = num83 + Main.rand.Next(-40, 41) * 0.02f;
                int p = Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX5, speedY6, Mod.Find<ModProjectile>("CarrotEX").Type, damage * 3 / 2, knockback, Main.myPlayer);
                Main.projectile[p].DamageType = DamageClass.Magic;
                Main.projectile[p].extraUpdates = 1;
                Main.projectile[p].usesLocalNPCImmunity = true;
                Main.projectile[p].localNPCHitCooldown = 10;
            }
            return false;
        }
    }
}