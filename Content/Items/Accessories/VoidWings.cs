using CalamityMod.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalamityMod;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class VoidWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(170, 13f, 2.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
            Item.defense = 15;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.moveSpeed += 0.1f;
            if (player.Remix().voidArmor)
            {
                player.Calamity().infiniteFlight = true;
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1.1f;
            maxAscentMultiplier = 2.25f;
            constantAscend = 0.15f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<VoidSingularity>(2).
                AddIngredient(ItemID.SoulofFlight, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
