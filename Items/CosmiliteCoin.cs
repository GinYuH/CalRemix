using CalRemix.Projectiles;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Items
{
    public class CosmiliteCoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Cosmilite Coin");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 22;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PlatinumCoin, 100).
                Register();
            CreateRecipe(100).
                AddIngredient<Klepticoin>(1).
                Register();
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 8));
            return true;
        }
    }

    public class CosmiliteCoinCurrency : CustomCurrencySingleCoin
    {
        public CosmiliteCoinCurrency(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
        {
            this.CurrencyTextKey = CurrencyTextKey;
            CurrencyTextColor = Color.MediumPurple;
        }
    }
}
