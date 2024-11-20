using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using Terraria.DataStructures;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalRemix.Content.Items.Weapons
{
    public class RGBMurasama : ModItem
    {
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("RGB Murasama");
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(2, 13));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.CloneDefaults(ModContent.ItemType<Murasama>());
            Item.damage = 4116;
            Item.value = Item.sellPrice(platinum: 50);
            Item.shoot = ModContent.ProjectileType<RGBMurasamaSlash>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Murasama>().
                AddIngredient<ExoPrism>(10).
                AddIngredient<CoreofCalamity>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
