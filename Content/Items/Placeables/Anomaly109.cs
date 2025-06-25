using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalamityMod.Rarities;
using CalamityMod.Items.Potions;
using CalamityMod;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Placeables
{
    public class Anomaly109 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Anomaly 109");
            // Tooltip.SetDefault("Insert unique codes in order to disable certain aspects of your game.\n" + CalamityUtils.ColorMessage("Must be configured in Singleplayer. Changes carry over to Multiplayer", Color.IndianRed));
        }

        public override void SetDefaults()
        {
            Item.width = 20;
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
