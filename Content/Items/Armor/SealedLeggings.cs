using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SealedLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RottedTendril>(11).
                AddIngredient<Veinroot>(15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class EnchantedSealedLeggings : ModItem
    {
        public override string Texture => "CalRemix/Content/Items/Armor/SealedLeggings";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SealedLeggings>();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 26;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.3f;
        }
    }
}
