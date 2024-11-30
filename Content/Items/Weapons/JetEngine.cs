using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
	public class JetEngine : ModItem
    {
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.useTime = 25; 
			Item.useAnimation = 25;
            Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.useTurn = true;
			Item.UseSound = SoundID.Item117;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 45;
            Item.mana = 10;
			Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<OnyxFist>();
            Item.shootSpeed = 5f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                position = Main.MouseWorld;
                velocity.X = 0f;
                velocity.Y = 0f;
                int num = Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(22.5f)), type, damage, knockback, Main.myPlayer);
                if (Main.projectile.IndexInRange(num))
                    Main.projectile[num].originalDamage = Item.damage;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(Recipes.HMT1Bar, 10).
                AddIngredient<EssenceofHavoc>(5).
                AddIngredient(ItemID.Obsidian, 26).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
