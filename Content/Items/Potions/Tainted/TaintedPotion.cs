using CalamityMod.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public abstract class TaintedPotion : ModItem
    {
        public virtual int BuffType => 0;
        public virtual int BuffTime => 0;
        public virtual int PotionType => 0;

        public virtual int MeatAmount => 1;

        public virtual string PotionName => "";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tainted " + PotionName + " Potion");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useTurn = true;
            Item.UseSound = BetterSoundID.ItemDrink;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            Item.buffType = BuffType;
            Item.buffTime = BuffTime;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(PotionType).
                AddIngredient(ModContent.ItemType<DisgustingMeat>(), MeatAmount).
                AddTile(TileID.Bottles).
                Register();
        }
    }
}