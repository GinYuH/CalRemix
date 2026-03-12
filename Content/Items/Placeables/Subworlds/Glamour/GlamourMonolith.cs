using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalamityMod;
using CalRemix.Content.Tiles.Subworlds.Glamour;
using CalRemix.Content.Items.Weapons.Stormbow;

namespace CalRemix.Content.Items.Placeables.Subworlds.Glamour
{
    public class GlamourMonolith : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BigEater>()] = Type;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.createTile = ModContent.TileType<GlamourMonolithPlaced>();
            Item.accessory = true;
            Item.vanity = true;
        }

        public override void UpdateEquip(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                player.Remix().glamourMonolith = true;
        }

        public override void UpdateVanity(Player player) => UpdateEquip(player);
    }
}
