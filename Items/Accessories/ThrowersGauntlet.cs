using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using CalRemix.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class ThrowersGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Thrower's Gauntlet");
            Tooltip.SetDefault("Melee attacks and projectiles inflict a variety of debuffs\n" +
            "30 % increased melee speed, damage, and 25 % increased melee critical strike chance\n" +
            "100 % increased true melee damage\n" +
            "Temporary immunity to lava\n" +
            "Increased melee knockback\n" +
            "Triples Yoyos, spawns 5 Counterweights, and quadruples yoyo range");
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eGauntlet = true;
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
