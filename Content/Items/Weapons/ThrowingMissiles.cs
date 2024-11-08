using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Projectiles;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Ranged;

namespace CalRemix.Content.Items.Weapons
{
    public class ThrowingMissiles : ModItem
    {
        public override string Texture => "CalRemix/Content/Projectiles/Hostile/HydrogenShell";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Throwing Missiles");
            Tooltip.SetDefault("Flings erratically moving missiles which home on enemies\nStealth strikes unleash a swarm of missiles");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Hydrogen;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.useTime = 11;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = Scorpio.RocketShoot;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 12;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<HydrogenShellFriendly>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            bool steakth = calamityPlayer.StealthStrikeAvailable();
            if (!steakth)
                return true;
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectile(source, position, Main.rand.NextVector2Circular(10, 10).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(8, 12), type, damage, knockback, player.whoAmI);
                }
            }

            return false;
        }
    }
}
