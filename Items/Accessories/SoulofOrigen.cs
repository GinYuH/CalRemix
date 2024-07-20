using Terraria;
using CalamityMod;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalRemix.Projectiles.Accessories;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CalamityMod.Items.Accessories.Wings;

namespace CalRemix.Items.Accessories
{
    public class SoulofOrigen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of Origen");
            Tooltip.SetDefault("7% increase to all damage\nGrants additional defense for each elemental construct soul worn\nOnly applies to souls equipped in slots below this one");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.accessory = true;
            Item.rare = ItemRarityID.White;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.07f;
            player.GetModPlayer<CalRemixPlayer>().origenSoul = true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryCustomScale(
                spriteBatch,
                texture: TextureAssets.Item[Type].Value,
                position,
                frame,
                drawColor,
                itemColor,
                origin,
                scale,
                wantedScale: 1f,
                drawOffset: new(0f, 0f)
            );
            return false;
        }
    }
}
