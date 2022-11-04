using CalRemix.Projectiles;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Items
{
    public class Klepticoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
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
            Item.rare = 8;
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
}
