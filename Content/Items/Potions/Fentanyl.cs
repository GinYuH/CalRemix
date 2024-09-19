using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Fentanyl : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lumtanyl");
            Tooltip.SetDefault("You feel light-headed");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item2;
            Item.value = Item.sellPrice(0, 0, 70);
            Item.buffType = BuffID.Wet;
            Item.buffTime = 60 * 20;
        }
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
                player.AddBuff(BuffID.Suffocation, 180);
            return null;
        }
        public override void AddRecipes()
        {
            CreateRecipe(8).
                AddIngredient<AnechoicCoating>().
                AddIngredient<Lumenyl>(4).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}