using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class DualCane : ModItem
	{
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Dual Cane");
            Item.staff[Type] = true;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
            Item.useTime = 45; 
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item9;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 47;
			Item.knockBack = 7f; 
			Item.noMelee = true;
            Item.channel = true;
            Item.mana = 12;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 13f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<CalRemixPlayer>().commonItemHoldTimer >= 40)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CaneOrb>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CaneOrb>(), damage, knockback, player.whoAmI, ai1: 1);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TaxCollectorsStickOfDoom).
                AddIngredient(ItemID.LightShard).
                AddIngredient(ItemID.DarkShard).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
