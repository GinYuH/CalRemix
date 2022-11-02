using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using CalRemix.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.Placeables.Ores;

namespace CalRemix.Items.Accessories
{
    public class CalamitousSoulArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Calamitous Soul Artifact");
            Tooltip.SetDefault("Pain\n" +
            "Killed enemies summon stationary Brimstone Hearts that explode after 10 seconds\n" +
            "The hearts can also block projectiles and can be collected for health\n" +
            "Grants a minion slot for every minion summoned that takes up multiple slots\n" +
            "Boosts melee speed by 20%, ranged velocity by 35%, rogue damage by 45%, max minions by 8, and reduces mana cost by 75%\n" +
            "Increases damage based on proximity to enemy");
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 56;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
            player.GetDamage<ThrowingDamageClass>() += 0.45f;
            player.maxMinions += 8;
            player.manaCost -= 0.75f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (player.whoAmI == Main.myPlayer && projectile.owner == player.whoAmI && projectile.minion && projectile.minionSlots > 1)
                {
                    player.maxMinions++;
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodlySoulArtifact>(1).
                AddIngredient<DimensionalSoulArtifact>(1).
                AddIngredient<PhantomicArtifact>(1).
                AddIngredient<EldritchSoulArtifact>(1).
                AddIngredient<Chaosplate>(60).
                AddIngredient<AshesofAnnihilation>(5).
                AddIngredient<ExodiumCluster>(25).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
