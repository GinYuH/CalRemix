﻿using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Buffs;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.ZAccessories // Shove them to the bottom of cheat mods
{
    [Autoload(false)]
    public class DebuffStone : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => "Stones";

        public int debuffType;
        public Color? debuffColor;
        protected override bool CloneNewInstances => true;

        public override string Name => debuffType < BuffID.Count ? debuffType + "Stone" : BuffLoader.GetBuff(debuffType).Mod.Name + "/" + BuffLoader.GetBuff(debuffType).Name + "Stone";
        public override string Texture => "CalRemix/Content/Items/Accessories/DebuffStone";

        private string DebuffName => debuffType < BuffID.Count ? Lang.GetBuffName(debuffType) : BuffLoader.GetBuff(debuffType).DisplayName.Value;

        public override LocalizedText DisplayName => Language.GetText("Mods.CalRemix.DebuffStone.DisplayName").WithFormatArgs(DebuffName);

        public override LocalizedText Tooltip => Language.GetText("Mods.CalRemix.DebuffStone.Tooltip").WithFormatArgs(DebuffName);

        public DebuffStone(int type)
        {
            debuffType = type;
        }

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.GetModPlayer<DebuffStonePlayer>().debuffStones.ContainsKey(debuffType))
            {
                player.GetModPlayer<DebuffStonePlayer>()?.debuffStones.Add(debuffType, false);
            }
            player.buffImmune[debuffType] = true;
            player.GetModPlayer<DebuffStonePlayer>().debuffStones[debuffType] = true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D debuff = TextureAssets.Buff[debuffType].Value;
            if (debuffColor == null)
            {
                Color[,] color = debuff.GetColorsFromTexture();
                Color mid = color[16, 16];
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
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, (debuffColor == null) ? Color.White : debuffColor.Value, 0, origin, scale, SpriteEffects.None, 0f);
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
        public Dictionary<int, bool> debuffStones = new Dictionary<int, bool> { };

        public override void ResetEffects()
        {
            for (int i = 0; i < debuffStones.Count; i++) 
            {
                debuffStones[i] = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var kvp in debuffStones)
            {
                if (kvp.Value)
                {
                    target.AddBuff(kvp.Key, 180);
                }
            }
        }
    }
}
