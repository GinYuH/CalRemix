using System;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.FurnitureStratus;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Projectiles.Weapons.Stormbow;
using CalRemix.UI.ElementalSystem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class RiskOfRain : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            Item.width = 20;
            Item.height = 46;
            Item.damage = 164;
            Item.crit = 4;
            Item.useTime = 190;
            Item.useAnimation = 190;

            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<RiskOfRainArrow>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projectile = Projectile.NewProjectile(source, Main.MouseWorld.X, Main.MouseWorld.Y, 0, 0, type, damage, knockback, player.whoAmI, 1);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AquaScepter, 1).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<AquasScepter>().
                AddIngredient(ItemID.WaterBucket, 5).
                AddIngredient<Watercooler>().
                AddIngredient<JellyChargedBattery>(2).
                AddIngredient<CosmiliteBar>(400).
                AddIngredient<AscendantSpiritEssence>(77).
                AddIngredient<Necroplasm>(668).
                AddIngredient(ItemID.TungstenOre, 37).
                AddIngredient(ItemID.BookStaff, 1).
                AddIngredient<StratusBricks>(5000).
                AddIngredient(ItemID.WaterBucket, 10).
                AddIngredient<ReaperTooth>(40).
                AddIngredient<NightmareFuel>(50).
                AddIngredient<EndothermicEnergy>(50).
                AddIngredient<DarksunFragment>(25).
                AddIngredient(ItemID.Toilet, 1).
                AddIngredient<ElectrolyteGelPack>(5).
                AddIngredient<Water>(22).
                AddIngredient<Those>().
                AddIngredient<Who>().
                AddIngredient<Know>().
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddIngredient(ItemID.Skull, 1).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient(ItemID.AquaScepter, 1).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<AquasScepter>().
                AddIngredient(ItemID.WaterBucket, 5).
                AddIngredient<Watercooler>().
                AddIngredient<JellyChargedBattery>(2).
                AddIngredient<CosmiliteBar>(400).
                AddIngredient<AscendantSpiritEssence>(77).
                AddIngredient<Necroplasm>(668).
                AddIngredient(ItemID.TungstenOre, 37).
                AddIngredient(ItemID.BookStaff, 1).
                AddIngredient<StratusBricks>(5000).
                AddIngredient(ItemID.WaterBucket, 10).
                AddIngredient<ReaperTooth>(40).
                AddIngredient<NightmareFuel>(50).
                AddIngredient<EndothermicEnergy>(50).
                AddIngredient<DarksunFragment>(25).
                AddIngredient(ItemID.Toilet, 1).
                AddIngredient<ElectrolyteGelPack>(5).
                AddIngredient<NebulousCore>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}