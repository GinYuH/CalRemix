using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
	public class ChainSpear : ModItem
	{
        private int spread = 0;
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            DisplayName.SetDefault("Chain Spear");
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = Item.sellPrice(silver: 40);
            Item.Calamity().donorItem = true;
            Item.useTime = 15; 
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
			Item.damage = 124;
			Item.knockBack = 3f;
            Item.noUseGraphic = true;
			Item.noMelee = true;
            Item.channel = true;
			Item.shoot = ModContent.ProjectileType<ChainSpearStab>();
			Item.shootSpeed = 10f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<ChainSpearProj>()] > 0)
                return false;
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<ChainSpearProj>()] < 1)
                {
                    Projectile.NewProjectile(source, position, velocity * 2, ModContent.ProjectileType<ChainSpearProj>(), damage / 2, knockback, player.whoAmI);
                }
                return false;
            }
            else if (player.ownedProjectileCounts[ModContent.ProjectileType<ChainSpearProj>()] > 0)
            {
                int p = Projectile.NewProjectile(source, position, velocity * 2, Item.shoot, damage, knockback, player.whoAmI);
                Main.projectile[p].scale = 2;
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Trident).
                AddIngredient<YateveoBloom>().
                AddIngredient<EssenceofZot>(10).
                AddIngredient<CosmiliteBar>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
