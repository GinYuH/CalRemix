using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Weapons
{
    public class RemoraDart : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.useTime = 11;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemWhipSwing with { Pitch = 0.5f };
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 84;
            Item.knockBack = 6f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<RemoraDartProj>();
            Item.shootSpeed = 30f;
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
                    int p = Projectile.NewProjectile(source, position, Main.rand.NextVector2Circular(10, 10).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(8, 12), type, damage, knockback, player.whoAmI);
                    Main.projectile[p].localAI[2] = 1;
                }
            }

            return false;
        }
    }
}
