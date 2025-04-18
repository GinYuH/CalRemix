using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
	public class OnyxGunblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Onyx Gunblade");
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.useTime = 24; 
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
            Item.useTurn = true;
			Item.UseSound = SoundID.Item40;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 25;
			Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<OnyxBlast>();
            Item.shootSpeed = 6f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<OnyxBlast>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LeadBroadsword, 2).
                AddIngredient(ItemID.Obsidian, 30).
                AddIngredient(ItemID.MusketBall, 99).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
