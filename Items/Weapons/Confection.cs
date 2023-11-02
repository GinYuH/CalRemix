using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Materials;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Rarities;

namespace CalRemix.Items.Weapons
{
	public class Confection : ModItem
    {
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Confection");
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ModContent.RarityType<PureGreen>();
			Item.value = Item.sellPrice(gold: 10);
            Item.useTime = 150; 
			Item.useAnimation = 150;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 220;
			Item.knockBack = 7f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Arrow;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Cake>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Shellshooter>().
                AddIngredient(ItemID.SliceOfCake, 12).
                AddIngredient<RuinousSoul>(14).
                AddIngredient(ItemID.BambooBlock, 64).
                AddIngredient<EssentialEssenceBar>(40).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
