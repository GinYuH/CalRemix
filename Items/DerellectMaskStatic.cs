using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class DerellectMaskStatic : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derellect Mask (Static)");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = 6;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<DerellectMask>().
            AddTile(TileID.Anvils).
            Register();
        }
    }
}