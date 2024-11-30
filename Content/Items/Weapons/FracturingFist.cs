using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
	public class FracturingFist : ModItem
	{
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 28; 
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 63;
			Item.knockBack = 6f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<FracturingFistProj>();
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
                AddIngredient<MeteorFist>().
                AddIngredient(ItemID.SoulofLight, 16).
                AddIngredient(ItemID.LightShard).
                AddIngredient(ItemID.CrystalShard, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
