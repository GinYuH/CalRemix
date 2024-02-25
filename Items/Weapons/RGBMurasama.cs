using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Melee;
using Terraria.DataStructures;
using CalamityMod.Items.Materials;
using CalRemix.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CalRemix.Items.Weapons
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
                AddIngredient<ElementalBar>(6).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
