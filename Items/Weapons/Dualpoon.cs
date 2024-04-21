using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items.Placeables;

namespace CalRemix.Items.Weapons
{
	public class Dualpoon : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.Pink;
			Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.useTime = 20; 
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item10;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 70;
			Item.knockBack = 6.5f;
			Item.shoot = ProjectileID.Harpoon;
			Item.shootSpeed = 20f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16f, 0f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            Vector2 pos1 = position + Vector2.One.RotatedBy(MathHelper.Pi / 2) * 8f;
            Vector2 pos2 = position - Vector2.One.RotatedBy(MathHelper.Pi / 2) * 8f;
            Projectile.NewProjectile(source, pos1, velocity, type, damage / 2, knockback, player.whoAmI);
            Projectile.NewProjectile(source, pos2, velocity, type, damage / 2, knockback, player.whoAmI);
            return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Harpoon, 2).
                AddIngredient<SeaPrism>(15).
                AddIngredient(ItemID.SoulofMight,10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
