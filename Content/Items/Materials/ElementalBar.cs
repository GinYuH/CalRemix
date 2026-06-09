using CalamityMod.Items.Materials;
using CalRemix.Core.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class ElementalBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Elemental Bar");
			Item.ResearchUnlockCount = 25;
        }
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
			Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LunarBar).
                AddIngredient<LifeAlloy>().
                AddIngredient(ItemID.FragmentSolar).
                AddIngredient(ItemID.FragmentVortex).
                AddIngredient(ItemID.FragmentNebula).
                AddIngredient(ItemID.FragmentStardust).
                AddIngredient<MeldBlob>().
                AddTile(TileID.LunarCraftingStation).
                AddCondition(new Condition(Recipes.LockedRecipe("GalacticaSingularity"), () => !CalRemixWorld.sidegar)).
                Register();
        }
    }
}
