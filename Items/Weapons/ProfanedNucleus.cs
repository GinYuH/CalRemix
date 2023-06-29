using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Items.Weapons
{
	public class ProfanedNucleus : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Profaned Nucleus");
            Tooltip.SetDefault("Throws out a molten chunk of profaned rock that explodes and splits into crystals\n"+"Stealth strikes leave behind lingering patches of magma"); 
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.useTime = 14; 
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 367;
			Item.knockBack = 10f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<ProfaneNucleus>();
            Item.shootSpeed = 20;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, (int)(1.5f * damage), knockback, player.whoAmI);
                if (num.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
