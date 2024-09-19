using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Content.Items.Ammo
{
    public class Klepticoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Klepticoin");
            Tooltip.SetDefault("The change of the gods");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 12));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.maxStack = 9999; // how would you even get this much money?????
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteCoin>(100).
                Register();
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 12));
            return true;
        }
    }

    public class KlepticoinCurrency : CustomCurrencySingleCoin
    {
        public KlepticoinCurrency(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
        {
            this.CurrencyTextKey = CurrencyTextKey;
            CurrencyTextColor = Color.DarkGoldenrod;
        }
    }
}
