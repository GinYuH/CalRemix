using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class ThrowersGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Thrower's Gauntlet");
            /* Tooltip.SetDefault("Melee attacks and projectiles inflict a variety of debuffs\n" +
            "30 % increased melee speed, damage, and 25 % increased melee critical strike chance\n" +
            "100 % increased true melee damage\n" +
            "Temporary immunity to lava\n" +
            "Increased melee knockback\n" +
            "Yoyo bag effects"); */ 
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            ModContent.GetModItem(ModContent.ItemType<ElementalGauntlet>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<WarbanneroftheSun>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<BadgeofBravery>()).UpdateAccessory(player, hideVisual);
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.3f;
            player.GetDamage<TrueMeleeDamageClass>() += 1f;
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.counterWeight = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalGauntlet>(1).
                AddIngredient(ItemID.YoyoBag).
                AddIngredient<WarbanneroftheSun>(1).
                AddIngredient<BadgeofBravery>(1).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
