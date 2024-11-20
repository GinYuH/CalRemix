using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class LightmixBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 25;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
		public override void SetDefaults()
        {
            Item.value = Item.sellPrice(1, 92);
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Remix().devItem = "Remix";
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryCustomScale(spriteBatch, TextureAssets.Item[Type].Value, position, frame, drawColor, itemColor, origin, scale, 1f, new Vector2(0f, -6f));
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AuricBar>()
                .AddIngredient<ExoPrism>()
                .AddIngredient<AshesofAnnihilation>()
                .AddIngredient<SubnauticalPlate>()
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
}
