using CalamityMod;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class CritterChip : ModItem
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
            Item.value = Item.sellPrice(silver: 1);
            Item.UseSound = BetterSoundID.ItemEat;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed2;
            Item.buffTime = CalamityUtils.SecondsToFrames(180);
        }

        public override void AddRecipes()
        {
            CreateRecipe(30)
                .AddIngredient(ModContent.ItemType<TurnipMesh>())
                .AddIngredient(ItemID.Feather)
                .AddIngredient(ModContent.ItemType<Crimtato>())
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
