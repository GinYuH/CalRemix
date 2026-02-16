using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SealedHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<DefaultDamageClass>() += 10f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SealedChestplate>() && (legs.type == ModContent.ItemType<SealedLeggings>() || legs.type == ModContent.ItemType<EnchantedSealedLeggings>());
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Sealed tendrils erupt from hit enemies\nGetting hit releases sealed tendrils in random directions";
            player.Remix().sealedArmor = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RottedTendril>(8).
                AddIngredient<Veinroot>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
