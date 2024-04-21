using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Items.Materials;

namespace CalRemix.Items.Weapons
{
	public class EcologicalCollapse : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Ecological Collapse");
            Tooltip.SetDefault("And there was no more ground to be seen...\n" +
                                "Throws out up to 10 lumenous mines with lightning auras\n" +
                                "Stealth strikes ignore the 10 mine cap, but dissipate quicker"); 

        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ModContent.RarityType<HotPink>();
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.useTime = 39; 
			Item.useAnimation = 39;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 2000;
			Item.knockBack = 7f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 4;
            Item.shoot = ModContent.ProjectileType<LumenousMine>();
            Item.shootSpeed = 7;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<LumenousMine>()] < 10 || player.Calamity().StealthStrikeAvailable();
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
                    Main.projectile[num].timeLeft = 480;
                }

                return false;
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MetalMonstrosity>(1).
                AddIngredient<ReaperTooth>(20).
                AddIngredient<Lumenyl>(20).
                AddIngredient<SubnauticalPlate>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
