using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons
{
	public class PlasmaflashBolt : ModItem
	{
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Plasmaflash Bolt");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 15; 
			Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item94;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 45;
			Item.knockBack = 5.5f;
            Item.mana = 13;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.PlasmaflashBolt>();
            Item.shootSpeed = 6.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ThunderBolt>().
                AddIngredient<AeroBolt>().
                AddIngredient(ItemID.AncientBattleArmorMaterial).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
