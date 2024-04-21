using CalamityMod.Items.Materials;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;

namespace CalRemix.Items.Weapons
{
	public class AeroBolt : ModItem
	{
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Aero Bolt");
            Tooltip.SetDefault("Casts a slow-moving mini hurricanes");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 17; 
			Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.DD2_BookStaffCast;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 35;
			Item.knockBack = 7f;
            Item.mana = 12;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<AeroBoltBolt>();
            Item.shootSpeed = 5.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpellTome, 1).
                AddIngredient<AerialiteBar>(11).
                AddIngredient<EssenceofBabil>(1).
                AddIngredient(ItemID.Daybloom, 3).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
