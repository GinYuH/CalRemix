using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
	public class BucketofCoal: RogueWeapon
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Bucket of Coal");
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 6);
            Item.useTime = 62; 
			Item.useAnimation = 62;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 90;
			Item.knockBack = 6.5f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<CoalBucket>();
            Item.shootSpeed = 12;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (num.WithinBounds(1000))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }
            else
                return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Coal).
                AddIngredient(ItemID.Obsidian, 6).
                AddIngredient(ItemID.EmptyBucket).
                AddIngredient(ItemID.DarkShard, 2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
