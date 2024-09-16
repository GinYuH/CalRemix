using CalamityMod.Rarities;
using Terraria;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalRemix.Items.Materials;
using Terraria.ID;

namespace CalRemix.Items.Accessories
{
    [AutoloadEquip(EquipType.Shoes)]
    public class TwistedNetheriteBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.defense = 30;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().twistedNetheriteBoots = true;
            player.moveSpeed += 0.04f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TwistedNetheriteBar>(4).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
