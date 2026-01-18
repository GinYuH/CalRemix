using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.ZAccessories // Shove them to the bottom of cheat mods
{
    [Autoload(false)]
    public class DebuffStone(int type) : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "Stones";

        public int debuffType = type;
        public Color? debuffColor = null;
        protected override bool CloneNewInstances => true;

        public override string Name => debuffType < BuffID.Count ? debuffType + "Stone" : BuffLoader.GetBuff(debuffType).Mod.Name + "/" + BuffLoader.GetBuff(debuffType).Name + "Stone";
        public override string Texture => "CalRemix/Content/Items/Accessories/DebuffStone";

        private string DebuffName => debuffType < BuffID.Count ? Lang.GetBuffName(debuffType) : BuffLoader.GetBuff(debuffType).DisplayName.Value;

        public override LocalizedText DisplayName => Language.GetText("Mods.CalRemix.DebuffStone.DisplayName").WithFormatArgs(DebuffName);

        public override LocalizedText Tooltip => Language.GetText("Mods.CalRemix.DebuffStone.Tooltip").WithFormatArgs(DebuffName);

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DebuffStonePlayer>().debuffStones.Add(debuffType);
            player.buffImmune[debuffType] = true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (debuffColor is null)
            {
                Texture2D texture = TextureAssets.Buff[debuffType].Value;
                debuffColor = Color.White;
                if (texture.Width > 1 && texture.Height > 1)
                {
                    Color[,] color = texture.GetColorsFromTexture();
                    Color mid = color[texture.Width / 2, texture.Height / 2];
                    int atts = 5;
                    // if the color is too dark, attempt finding a brighter one 
                    for (int i = -atts; i < atts; i++)
                    {
                        if ((mid.R + mid.G + mid.B) < 255f)
                        {
                            mid = color[16 + i * 2, 16];
                        }
                        else
                        {
                            break;
                        }
                    }
                    debuffColor = mid;
                }
                else if (texture.Width == 1 && texture.Height == 1)
                {
                    debuffColor = texture.GetColorsFromTexture()[0, 0];
                }
            }
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, debuffColor.Value, 0, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            if (Main.debuff[debuffType])
            {
                int ingID = 0;
                if (debuffType == BuffID.PotionSickness || debuffType == BuffID.ManaSickness || debuffType == BuffID.ChaosState)
                    ingID = ItemID.GoldWaterStriderCage;
                while (ingID == 0 || ContentSamples.ItemsByType[ingID].ModItem is DebuffStone || ItemID.Sets.Deprecated[ingID])
                    ingID = Main.rand.Next(0, ItemLoader.ItemCount);
                Recipe.Create(Type).AddIngredient(ItemID.MagmaStone).AddIngredient(ingID).DisableDecraft().AddTile(ModContent.TileType<StonecutterPlaced>()).Register();
            }
        }
    }

    public class DebuffStonePlayer : ModPlayer
    {
        public HashSet<int> debuffStones = [];

        public override void ResetEffects() => debuffStones.Clear();

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var kvp in debuffStones)
            {
                target.AddBuff(kvp, 180);
            }
        }
    }
}
