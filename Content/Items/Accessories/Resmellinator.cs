using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Accessories;
using Terraria;
using CalamityMod.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class Resmellinator : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 56;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GetModItem(ItemType<Regenerator>()).UpdateAccessory(player, hideVisual);
            player.aggro -= 400;
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetCritChance(DamageClass.Generic) += 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Regenerator>().
                AddIngredient(ItemID.PutridScent, 1).
                AddTile(TileID.TinkerersWorkbench).
            Register();
        }
    }
}
