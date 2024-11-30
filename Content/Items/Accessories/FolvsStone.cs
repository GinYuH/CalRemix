using CalamityMod.Items;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.ZAccessories // Shove them to the bottom of cheat mods
{
    public class FolvsStone : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "Stones";

        public override string Texture => "CalRemix/Content/Items/Accessories/DebuffStone";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Folv's Stone");
            Tooltip.SetDefault("Immunity to Folv's\nAttacks inflict Folv's");
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.DimGray, 0, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            int ingID = 0;
            while (ingID == 0 || ContentSamples.ItemsByType[ingID].ModItem is DebuffStone || ItemID.Sets.Deprecated[ingID])
                ingID = Main.rand.Next(0, ItemLoader.ItemCount);
            Recipe.Create(Type).AddIngredient(ItemID.MagmaStone).AddIngredient(ingID).DisableDecraft().AddTile(ModContent.TileType<StonecutterPlaced>()).Register();
        }
    }
}