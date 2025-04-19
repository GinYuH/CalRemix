using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using CalRemix.Content.Tiles;
using Terraria.ID;

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
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
			Item.maxStack = 9999;
            Item.createTile = ModContent.TileType<YharimBarPlaced>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Rectangle frame;
            if (Main.itemAnimations[Item.type] != null)
            {
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }
            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;
            spriteBatch.Draw(texture, drawPos, frame, new Color(255, 255, 255, 255), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
