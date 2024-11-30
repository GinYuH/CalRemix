using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Weapons
{
	public class FlamingIceBow : ModItem
    {
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Flaming Ice Bow");
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2, silver: 40);
            Item.useTime = 25; 
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 123;
			Item.knockBack = 2f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Arrow;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FlameFrostArrow>(), damage, knockback, player.whoAmI);
            Vector2 pos = position + player.DirectionTo(Main.MouseWorld) * 18f;
            Projectile.NewProjectile(source, pos, velocity, ModContent.ProjectileType<FlameFrostArrow>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-4f, -0f);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IceBow).
                AddIngredient(ItemID.MoltenFury, 2).
                AddIngredient(ItemID.LivingFireBlock, 23).
                AddIngredient(ItemID.IceBlock, 23).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
