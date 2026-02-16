using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class PinkSlab : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.value = Item.sellPrice(silver: 5);
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed;
            Item.buffTime = 14400;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Mining, 14400);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<MysteriousGraySlab>()).
                AddIngredient(ModContent.ItemType<Veinroot>()).
                AddTile(TileID.CookingPots).
                Register();
        }
    }
}
