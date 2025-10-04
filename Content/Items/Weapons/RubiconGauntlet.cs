using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;

namespace CalRemix.Content.Items.Weapons
{
    public class RubiconGauntlet : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.autoReuse = true;
            Item.UseSound = ScorchedEarth.ShootSound;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 187;
            Item.knockBack = 10f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<RubiconGauntletProj>();
            Item.shootSpeed = 20;
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
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GildedGauntlet>().
                AddIngredient<MercuryCoatedSubcinium>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
