using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class WarloksMoonFist : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Warlok's Moon Fist");
            Tooltip.SetDefault("Melee attacks and projectiles inflict nightwither and have a 10% chance to instantly kill normal enemies\n" +
            "Grants the user 60% increased true melee damage on true melee hits\n" +
            "25% increased melee speed, damage, and 5% increased melee critical strike chance\n" +
            "50% increased true melee damage\n" +
            "Temporary immunity to lava\n" +
            "Increased melee knockback"); 
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.25f;
            player.GetDamage<TrueMeleeDamageClass>() += 0.5f;
            player.GetModPlayer<CalRemixPlayer>().moonFist = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FireGauntlet, 1).
                AddIngredient(ItemID.LunarBar, 20).
                AddIngredient(ItemID.Ectoplasm, 51). // I HATE IT I HATE IT I HATE IT
                AddIngredient<Lumenyl>(20).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
