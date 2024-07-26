using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;
using CalRemix.Projectiles;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Magic;

namespace CalRemix.Items.Weapons
{
    public class Juicer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Juicer");
            Tooltip.SetDefault("Sprays enemies with an assortment of liquids\n40% chance to not consume ammo");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Pathogen;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemWaterBolt;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 28;
            Item.knockBack = 4f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.60f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int roll = Main.rand.Next(3);
            roll = roll switch
            {
                0 => ProjectileID.WaterStream,
                1 => ProjectileID.GoldenShowerFriendly,
                _ => ModContent.ProjectileType<EvergladeSprayProjectile>(),
            };
            float spread = 2;
            int num = Projectile.NewProjectile(source, position + velocity * 3, velocity + Main.rand.NextVector2Circular(spread, spread), roll, damage, knockback, player.whoAmI, Main.rand.Next(1, 5));
            Main.projectile[num].DamageType = DamageClass.Ranged;
            Main.projectile[num].stepSpeed = 2f;

            return true;
        }
    }
}
