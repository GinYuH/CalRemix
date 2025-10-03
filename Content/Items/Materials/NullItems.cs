using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.SummonItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class NullOrb : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 90);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SealloyBar>(), 10).
                AddIngredient(ModContent.ItemType<Mercury>(), 20).
                AddIngredient(ModContent.ItemType<VoidInfusedStone>(), 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class NullPedestal : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 90);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<StrangePuppet>()).
                AddIngredient(ModContent.ItemType<SealToken>(), 20).
                AddIngredient(ModContent.ItemType<VoidInfusedStone>(), 30).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class NullHolder : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 90);
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<VoidInfusedTurnipFruit>()).
                AddIngredient(ModContent.ItemType<VoidInfusedStone>(), 50).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
