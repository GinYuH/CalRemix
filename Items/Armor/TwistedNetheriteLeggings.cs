using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class TwistedNetheriteLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
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
