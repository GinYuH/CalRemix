using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
    public class Warglaive: RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warglaive");
            Tooltip.SetDefault("Stealth strikes stick to tiles");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 4);
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 15;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<WarglaiveProjectile>();
            Item.shootSpeed = 10f;
            Item.maxStack = 9999;
            Item.consumable = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (num.WithinBounds(1000))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }
            else
                return true;
        }
    }
}
