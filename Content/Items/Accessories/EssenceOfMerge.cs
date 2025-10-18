using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class EssenceOfMerge : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Essence Of Merge");
            // Tooltip.SetDefault("The essence of... quite a lot, actually.\n"+
            // "Horizontal speed: 8\n"+
            // "Acceleration multiplier: 1.2\n"+
            //     "Flight time: 120"); 
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 8f, 1.2f);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            player.noFallDmg = true;
            
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.5f; 
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f; 
            maxAscentMultiplier = 1.5f; 
            constantAscend = 0.1f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ectoplasm, 20).
                AddIngredient(ItemID.SoulofFlight, 20).
                AddIngredient<EssenceofSunlight>(10).
                AddIngredient<EssenceofHavoc>(10).
                AddIngredient<EssenceofEleum>(10).
                AddIngredient<EssenceofBabil>(10).
                AddIngredient<EssenceofRend>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
