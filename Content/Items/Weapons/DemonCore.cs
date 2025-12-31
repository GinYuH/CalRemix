using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
	public class DemonCore: RogueWeapon
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Demon Core");
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(copper: 80);
            Item.useTime = 25; 
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.damage = 32;
			Item.knockBack = 1f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<DemonCoreProj>();
            Item.shootSpeed = 7;
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
            CreateRecipe(999).
                AddIngredient<RotBall>(999).
                AddIngredient<HardenedHoneycomb>(999).
                AddIngredient<InfernalKris>(999).
                AddIngredient<BundleBones>(999).
                AddIngredient<PurifiedGel>(30).
                AddTile(TileID.DemonAltar).
                Register();
            CreateRecipe(999).
                AddIngredient<ToothBall>(999).
                AddIngredient<HardenedHoneycomb>(999).
                AddIngredient<InfernalKris>(999).
                AddIngredient<BundleBones>(999).
                AddIngredient<PurifiedGel>(30).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
