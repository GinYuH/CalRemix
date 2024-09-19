using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Weapons
{
	public class JetEngine : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jet Engine");
            Item.ResearchUnlockCount = 1;
		}
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
			Item.DamageType = DamageClass.Magic;
			Item.damage = 45;
            Item.mana = 10;
			Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<OnyxFist>();
            Item.shootSpeed = 22f;
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
