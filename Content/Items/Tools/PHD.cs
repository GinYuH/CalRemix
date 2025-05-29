using CalRemix.Content.NPCs.PandemicPanic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
    public class PHD : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("PHD");
            // Tooltip.SetDefault("Makes Pandemic Panic enemies easier to recognize\n\"Would my patient mind if I took their organs out and sold them?\"");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<CalRemixPlayer>().phd = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodenSink, 1).
                AddTile(TileID.Anvils).
                AddCondition(new Condition("Pandemic Panic active", () => PandemicPanic.IsActive)).
                Register();
        }
    }
}