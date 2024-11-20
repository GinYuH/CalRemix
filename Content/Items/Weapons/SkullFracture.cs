using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
	public class SkullFracture : ModItem
	{
		public override void SetDefaults() 
		{
			Item.width = 116;
			Item.height = 36;
			Item.rare = ItemRarityID.LightRed;
			Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.useTime = 12; 
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = BetterSoundID.ItemFlamethrower;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 56;
			Item.knockBack = 2f; 
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<SkyFire>();
			Item.shootSpeed = 8f;
			Item.useAmmo = AmmoID.Gel;
            Item.consumeAmmoOnLastShotOnly = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryonicBar>(7).
                AddIngredient(ItemID.LightShard, 2).
                AddIngredient(ItemID.SoulofLight, 10).
                AddIngredient(ItemID.CrystalShard, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
