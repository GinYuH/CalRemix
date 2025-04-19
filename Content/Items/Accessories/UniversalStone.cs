using CalamityMod.Items;
using CalRemix.Content.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using CalamityMod.CalPlayer;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Rarities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class UniversalStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Universal Stone");
            /* Tooltip.SetDefault("The true potential of a lesser elemental\n"+
            "Summons an cosmic conflict construct to fight for you\n" +
            "Summons a cosmic shield around you that inflicts God Slayer Inferno\n" +
            "Collecting Cosmichid plants boosts damage by 1 % up to 30 %\n" +
            "While the item's visibility is on, Mana Sickness is replaced with Mana Burn\n" +
            "50 % increased flight time"); */ 
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            CalamityPlayer calPlayer = player.GetModPlayer<CalamityPlayer>();
            calPlayer.CryoStone = true;
            player.wingTimeMax = (int)(player.wingTimeMax * 1.5f);
            if (!hideVisual)
            calPlayer.ChaosStone = true;
            modPlayer.crystalconflict = true;
            int brimmy = ProjectileType<CosmicConflict>();

            var source = player.GetSource_Accessory(Item);

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 4002;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, brimmy, swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
            }

            // Provide life benefits if the player is standing on ground and has typical gravity.
            int x = (int)player.Center.X / 16;
            int y = (int)(player.Bottom.Y - 1f) / 16;
            Tile groundTile = CalamityMod.CalamityUtils.ParanoidTileRetrieval(x, y + 1);
            bool groundTileIsSolid = groundTile.HasUnactuatedTile && (Main.tileSolid[groundTile.TileType] || Main.tileSolidTop[groundTile.TileType]);
            if (groundTileIsSolid && player.gravDir == 1f)
                calPlayer.BloomStoneRegen = true;

            // Grow chid
            if (player.whoAmI == Main.myPlayer && player.velocity.Y == 0f && player.grappling[0] == -1)
            {
                Tile walkTile = CalamityMod.CalamityUtils.ParanoidTileRetrieval(x, y);
                if (!walkTile.HasTile && walkTile.LiquidAmount == 0 && groundTile != null && WorldGen.SolidTile(groundTile) && Main.rand.NextBool(2048))
                {
                    if (groundTile.TileType == TileID.Stone || groundTile.TileType == TileID.Grass)
                    {
                        WorldGen.PlaceObject(x, y, (ushort)ModContent.TileType<Tiles.CosmichidPlant>());
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AeroStone>().
                AddIngredient<CryoStone>().
                AddIngredient<RockStone>().
                AddIngredient<ChaosStone>().
                AddIngredient<BloomStone>().
                AddIngredient<CosmicStone>().
                AddIngredient<CosmiliteBar>(10).
                AddIngredient<AscendantSpiritEssence>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
