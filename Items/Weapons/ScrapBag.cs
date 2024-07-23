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

namespace CalRemix.Items.Weapons
{
    public class ScrapBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrap Bag");
            Tooltip.SetDefault("Tosses out scraps\nStealth strikes throw a large amount of charged scraps that linger longer");
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemToxicFlaskThrow;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 12;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Scrap>();
            Item.shootSpeed = 16f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            int ct = calamityPlayer.StealthStrikeAvailable() ? 22 : 2;
            float spread = calamityPlayer.StealthStrikeAvailable() ? 4 : 8;
            for (int i = 0; i < ct; i++)
            {
                int num = Projectile.NewProjectile(source, position, velocity + Main.rand.NextVector2Circular(spread, spread), type, damage, knockback, player.whoAmI, Main.rand.Next(1, 5));
                if (calamityPlayer.StealthStrikeAvailable())
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }
            }

            return false;
        }
    }
}
