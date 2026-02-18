using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Materials
{
	public class YharimBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Yharim Bar");
      	    // Tooltip.SetDefault("You can feel the power of the Tyrant in this metal");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<YharimBarPlaced>());
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        }
        public override void HoldItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                Item.createTile = (Framing.GetTileSafely((int)(Main.MouseWorld.X / 16), (int)((Main.MouseWorld.Y) / 16)+1).TileType == ModContent.TileType<BarPlanterTile>()) ? ModContent.TileType<YharimBarPlant>() : ModContent.TileType<YharimBarPlaced>();
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            Rectangle frame = (Main.itemAnimations[Type] != null) ? Main.itemAnimations[Type].GetFrame(texture, Main.itemFrameCounter[whoAmI]) : texture.Frame();
            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;
            spriteBatch.Draw(texture, drawPos, frame, new Color(255, 255, 255, 255), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
