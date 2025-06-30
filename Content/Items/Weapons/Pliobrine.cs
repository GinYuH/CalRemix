using CalamityMod.Rarities;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
    public class Pliobrine : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 164;
            Item.knockBack = 13f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<PliobrineProj>();
            Item.shootSpeed = 16;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, (int)(1.5f * damage), knockback, player.whoAmI);
                if (num.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
