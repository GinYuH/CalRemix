using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class HauntedBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Haunted Bar");
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
                AddIngredient(ModContent.ItemType<RuinousSoul>()).
                AddIngredient(ModContent.ItemType<Necroplasm>(), 3).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
