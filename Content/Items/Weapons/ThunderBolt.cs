using CalamityMod.Items.Materials;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class ThunderBolt : ModItem
	{
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Thunder Bolt");
            Tooltip.SetDefault("Casts a slow-moving ball of lightning");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.useTime = 30; 
			Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item94;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 54;
			Item.knockBack = 5.5f;
            Item.mana = 12;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ThunderBoltBolt>();
            Item.shootSpeed = 7.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(6).
                AddIngredient(ItemID.SunplateBlock, 9).
                AddIngredient(ItemID.Blinkroot, 2).
                AddIngredient<EssenceofSunlight>(1).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
