using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using CalRemix.Content.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class QuiverofMadness : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Quiver of Madness");
            /* Tooltip.SetDefault("'Most drink the wine to lose focus, while others transcend...\n"+
            "30% increased ranged damage, 25% increased ranged critical strike chance, and 100% reduced ammo usage\n"+
            "15 increased defense, 2 increased life regen, and 35% increased pick speed\n"+
            "You are surrounded by 4 demonic portals that buff ranged projectiles that intersect them\n" +
            "Skull targets occasionally appear on enemies which increase damage taken when hit"); */ 
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (player.GetModPlayer<CalRemixPlayer>().brimPortal)
                return false;

            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModContent.GetModItem(ModContent.ItemType<PlanebreakersPouch>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<DaawnlightSpiritOrigin>()).UpdateAccessory(player, hideVisual);
            player.GetDamage<RangedDamageClass>() += 0.3f;
            CalamityPlayer caPlayer = player.Calamity();
            caPlayer.ammoCost *= 0f;
            player.GetCritChance<RangedDamageClass>() += 25;
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.brimPortal = true;
            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_Accessory(Item);
                if (player.ownedProjectileCounts[ModContent.ProjectileType<BrimstoneGenerator>()] < 4)
                {
                    for (int v = 0; v < 4; v++)
                    {
                        Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ModContent.ProjectileType<BrimstoneGenerator>(), 0, 0f, Main.myPlayer, v);
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlanebreakersPouch>(1).
                AddIngredient<QuiverofNihility>(1).
                AddIngredient<DaawnlightSpiritOrigin>(1).
                AddIngredient<RedWine>(5).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
