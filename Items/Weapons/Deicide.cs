using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.DataStructures;
using CalRemix.Projectiles.Weapons;
using CalRemix.Projectiles;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Accessories;
using CalRemix.Items.Ammo;

namespace CalRemix.Items.Weapons
{
	public class Deicide : ModItem
    {
        private bool raged;
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Deicide");
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
            Item.crit = 22;
			Item.rare = ModContent.RarityType<Violet>();
			Item.value = Item.sellPrice(gold: 30);
            Item.useTime = 42; 
			Item.useAnimation = 42;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item20;
            Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.damage = 5022;
			Item.knockBack = 12f;
            Item.noUseGraphic = true;
			Item.noMelee = true;
            Item.channel = true;
            Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 2f;
        }
        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
                player.GetModPlayer<CalRemixPlayer>().deicide = 0;
            else if (player.GetModPlayer<CalRemixPlayer>().deicide < 3601 && player.channel && (player.HeldItem == Item || player.inventory[58] == Item))
            {
                player.GetModPlayer<CalRemixPlayer>().deicide++;
                if (player.Calamity().rageModeActive && raged && Main.myPlayer == player.whoAmI)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, player.DirectionTo(Main.MouseWorld) * 8f, ModContent.ProjectileType<HornetShot>(), Item.damage * 22, 0, player.whoAmI);
                    raged = false;
                }
                if (player.Calamity().rage <= 0)
                    raged = true;
            }
            if (player.GetModPlayer<CalRemixPlayer>().deicide >= 3600)
            {
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<IchorBlob>(), 0, 0, player.whoAmI);
                proj.hostile = false;
                player.dead = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float multiplier = (player.GetModPlayer<CalRemixPlayer>().deicide > 1800) ? 1.3f : 1f;
            if (player.GetModPlayer<CalRemixPlayer>().deicide > 600)
                Projectile.NewProjectile(source, position, velocity * 8f, ModContent.ProjectileType<DeicideFist>(), (int)(damage / 5 * multiplier), knockback, player.whoAmI, ai0: 1);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<DeicideFist>(), (int)(damage * multiplier), knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PhosphorescentGauntlet>().
                AddIngredient<WreathofBelial>().
                AddIngredient<ShatteredCommunity>().
                AddIngredient<Klepticoin>().
                AddIngredient<HornetRound>(22).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
