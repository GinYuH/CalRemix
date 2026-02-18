using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
	public class ShadowsDescent: RogueWeapon
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Shadow's Descent");
            /* Tooltip.SetDefault("Shoots a shadow ball that ignores gravity and slows down after a short time." + 
                "While stationary, it shoots spirits and covers an area in a large shadow\n" + 
                "Stealth strikes also emit dark pulses while stationary\n" + 
                "Right click to delete all existing shadow balls\n" + 
                "\'They didn't understand its nature, so it was sealed away...\'"); */ 
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().donorItem = true;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.useTime = 19; 
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item20;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 212;
			Item.knockBack = 4f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 4;
            Item.shoot = ModContent.ProjectileType<ShadowsDescentBall>();
            Item.shootSpeed = 5;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileID.None;
                Item.shootSpeed = 0f;
                return player.ownedProjectileCounts[ModContent.ProjectileType<ShadowsDescentBall>()] > 0;
            }
            Item.shoot = ModContent.ProjectileType<ShadowsDescentBall>();
            Item.shootSpeed = 5f;
            return player.ownedProjectileCounts[ModContent.ProjectileType<ShadowsDescentBall>()] < 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                damage = (int)((double)damage * 1.345);
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (num.WithinBounds(1000))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RazorbladeTyphoon).
                AddIngredient(ItemID.DeathSickle).
                AddIngredient<AscendantSpiritEssence>(3).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
