using CalamityMod.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;

namespace CalRemix.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class HolyMantle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Holy Mantle");
            /* Tooltip.SetDefault("Delirious thoughts...\n"+
            "Horizontal speed: 14\n"+
            "Acceleration multiplier: 3.2\n"+
                "Flight time: ∞"); */ 
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(500, 14f, 3.2f);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.wingTime < 500)
            {
                player.wingTime = 500;
            }
            player.noFallDmg = true;
            if (!hideVisual)
                { 
                player.GetModPlayer<WulfrumPackPlayer>().WulfrumPackEquipped = true;
                player.GetModPlayer<WulfrumPackPlayer>().PackItem = Item;
            }
            player.Calamity().ascendantInsignia = true;
            player.Calamity().infiniteFlight = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f; 
            ascentWhenRising = 0.175f;
            maxCanAscendMultiplier = 1.2f; 
            maxAscentMultiplier = 3.25f; 
            constantAscend = 0.15f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WingsofRebirth>(1).
                AddIngredient(ItemID.EmpressFlightBooster).
                AddIngredient<MOAB>(1).
                AddIngredient<AscendantInsignia>(1).
                AddIngredient<WulfrumAcrobaticsPack>(1).
                AddIngredient(ItemID.SoulofFlight, 20).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
