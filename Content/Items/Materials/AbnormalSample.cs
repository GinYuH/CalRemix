using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class AbnormalSample : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 10);
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.NPCHit1;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tem = TextureAssets.Item[Type].Value;
            spriteBatch.Draw(tem, position + Main.rand.NextVector2Circular(2, 2), frame, drawColor, 0, origin, scale, 0, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tem = TextureAssets.Item[Type].Value;
            spriteBatch.Draw(tem, Item.position - Main.screenPosition + Main.rand.NextVector2Circular(2, 2), null, Item.GetAlpha(lightColor), 0, new Vector2(tem.Width / 2, tem.Height), scale, 0, 0);
            return false;
        }
    }
}
