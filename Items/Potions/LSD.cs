using CalamityMod.Items.Placeables;
using CalamityMod.Items.Potions;
using CalRemix.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions
{
    public class LSD : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sulphuric Acid");
            Tooltip.SetDefault("You have a strong urge to eat this paper");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 4);
            Item.buffType = ModContent.BuffType<Acid>();
            Item.buffTime = 60 * 60 * 4;
        }
        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient<SulphurskinPotion>().
                AddIngredient<Acidwood>(3).
                AddIngredient(ItemID.GlowingMushroom, 3).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}