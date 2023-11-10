using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;

namespace CalRemix.Items.Materials
{
	public class CoreofBabil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core of Babil");
			Item.ResearchUnlockCount = 25;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.SortingPriorityMaterials[Type] = 94; // Spectre Bar
        }
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
			Item.maxStack = 9999;
            Item.width = Item.height = 32;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRemix/Items/Materials/CoreofBabilGlow").Value);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.05f * brightness, 0.3f * brightness, 0.1f * brightness);
        }
        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient<EssenceofBabil>(1).
                AddIngredient(ItemID.HallowedBar).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
