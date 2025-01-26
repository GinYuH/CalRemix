using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class TwistedNetheriteLeggings : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.defense = 45;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TwistedNetheriteBar>(7).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
