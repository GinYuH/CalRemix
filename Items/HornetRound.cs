using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Projectiles;

namespace CalRemix.Items
{
    public class HornetRound : ModItem
    {
		public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("22 Hornet Round");
            Tooltip.SetDefault("Pierces the hearts of gods...\n" +
					"Ricochets up to 21 times off of hit enemies"); 
            Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() 
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.knockBack = 11f;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = Item.buyPrice(silver:1);
			Item.shoot = ModContent.ProjectileType<HornetShot>();
			Item.shootSpeed = 11f;
			Item.ammo = AmmoID.Bullet;
		}
		public override void AddRecipes() 
		{
			CreateRecipe(222)
				.AddIngredient<AuricBar>(1)
				.AddTile<CosmicAnvil>()
				.Register();
		}
	}
}
