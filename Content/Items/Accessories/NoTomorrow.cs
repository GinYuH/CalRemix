using CalamityMod.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRemix.Content.Items.Accessories
{
    public class NoTomorrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // Tooltip.SetDefault("All friendly projectiles are twice as large");
        }

        public override void SetDefaults()
        {
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.noTomorrow = true; 
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TungstenBar, 16).
                AddTile(TileID.Anvils).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.SilverBar, 16).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
