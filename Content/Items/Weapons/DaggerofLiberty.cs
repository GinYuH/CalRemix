using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items;
using Terraria.Audio;

namespace CalRemix.Content.Items.Weapons
{
	public class DaggerofLiberty : RogueWeapon
    {
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.damage = 26;
            Item.knockBack = 2f;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SpearofDestiny.ThrowSound;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.shoot = ModContent.ProjectileType<DaggerofLibertyProj>();
            Item.shootSpeed = 20;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                if (Main.myPlayer == player.whoAmI)
                    SoundEngine.PlaySound(SpearofDestiny.ThrowSound3);
                Vector2 v = velocity;
                for (int i = -2; i < 3; i++)
                {
                    v = (i == 0) ? velocity * Main.rand.NextFloat(2.25f, 2.45f) : (velocity * Main.rand.NextFloat(2f, 2.25f)).RotatedBy(MathHelper.ToRadians(5.625f * i)).RotatedByRandom(MathHelper.ToRadians(5.625f));
                    int p = Projectile.NewProjectile(source, position, v, type, damage * 2, knockback, player.whoAmI);
                    if (p.WithinBounds(Main.maxProjectiles))
                        Main.projectile[p].Calamity().stealthStrike = true;
                    Main.projectile[p].penetrate = 10;
                }
                return false;
            }
            else
                return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<CursedDagger>()
                .AddIngredient(ItemID.HallowedBar, 7)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe().AddIngredient<IchorDagger>()
                .AddIngredient(ItemID.HallowedBar, 7)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
