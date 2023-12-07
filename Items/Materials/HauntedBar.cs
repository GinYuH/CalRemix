using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class HauntedBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Bar");
			Item.ResearchUnlockCount = 25;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(3, 18));
        }
		public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = Item.sellPrice(gold: 15, silver: 20);
			Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Polterplasm>(), 3).
                AddIngredient(ModContent.ItemType<RuinousSoul>()).
                AddIngredient(ModContent.ItemType<ExodiumCluster>(), 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
