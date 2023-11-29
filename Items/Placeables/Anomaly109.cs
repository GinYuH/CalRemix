using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Tiles;
using CalamityMod.Rarities;
using CalamityMod.Items.Potions;

namespace CalRemix.Items.Placeables
{
    public class Anomaly109 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Anomaly 109");
            Tooltip.SetDefault("Insert unique codes in order to disable certain aspects of your game.");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.createTile = ModContent.TileType<Anomaly109Placed>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<DeliciousMeat>(), 22).
                AddCondition(new Condition("In Singleplayer", () => Main.netMode != NetmodeID.MultiplayerClient)).
                Register();
        }
    }
}
