using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Placeables;
using Terraria.Audio;
using CalRemix.Projectiles.Weapons;

namespace CalRemix.Items.Weapons
{
	public class HadopelagicEcho : ModItem
	{
        public override string Texture => "CalamityMod/Items/Weapons/Magic/EidolicWail";
        public static readonly SoundStyle adultSound = new SoundStyle("CalamityMod/Sounds/Custom/EidolonWyrmRoarClose");
        public override void SetStaticDefaults() 
		{
            SacrificeTotal = 1;
            DisplayName.SetDefault("Hadopelagic Echo");
            Tooltip.SetDefault("Fires a string of bouncing sound waves that become stronger as they travel\n" +
                                "Sound waves echo additional sound waves on enemy hits");

        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ModContent.RarityType<HotPink>();
            Item.value = CalamityGlobalItem.Rarity16BuyPrice;
            Item.useTime = 5; 
			Item.useAnimation = 15;
            Item.reuseDelay = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = adultSound;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 500;
			Item.knockBack = 1.5f;
            Item.mana = 15;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HadoSoundwave>();
            Item.shootSpeed = 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EidolicWail>(1).
                AddIngredient<ReaperTooth>(20).
                AddIngredient<Lumenyl>(20).
                AddIngredient<PlantyMush>(20).
                AddIngredient<SubnauticalPlate>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
