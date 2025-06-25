using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class GlitterEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Glittering Astral Eye");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.value = Item.sellPrice(gold: 7, silver: 87);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().pearl = true;
            player.GetModPlayer<CalRemixPlayer>().astralEye = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BlazingPowder>(1).
                AddIngredient<AstralPearl>(1).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
