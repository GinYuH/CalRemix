using Terraria;
using CalamityMod;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalRemix.UI;

namespace CalRemix.Content.Items.Accessories
{
    public class SoulofPyrogen : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Soul of Pyrogen");
            // Tooltip.SetDefault("Increases critical strike chance to 100%\nDisables flight, mounts, and hooks\n" + CalamityUtils.ColorMessage("Boosts Fire damage", Color.Orange));
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 7));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float lightOffset = (float)Main.rand.Next(90, 111) * 0.01f;
            lightOffset *= Main.essScale;
            Lighting.AddLight((int)((Item.position.X + (float)(Item.width / 2)) / 16f), (int)((Item.position.Y + (float)(Item.height / 2)) / 16f), 1f * lightOffset, 0.1f * lightOffset, 0.1f * lightOffset);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance<GenericDamageClass>() = 100;
            player.wingTime = 0;
            player.mount.Dismount(Main.LocalPlayer);
            player.releaseHook = true;
            player.GetModPlayer<CalRemixPlayer>().pyrogenSoul = true;
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

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            return slot == ModContent.GetInstance<SoulSlot>().Type;
        }
    }
}
