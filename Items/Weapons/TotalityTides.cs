using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class TotalityTides : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Totality Tides");
        Tooltip.SetDefault("Inundatio ex Terminus\n" +
            "Summons a ball of exo energy on your cursor that sprays out a large amount of projectiles relative to your position");
        SacrificeTotal = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 640;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 4;
        Item.rare = ModContent.RarityType<Violet>();
        Item.value = CalamityGlobalItem.Rarity15BuyPrice;
        Item.UseSound = Exoblade.DashSound;
        Item.autoReuse = true;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<TotalityTidesDisc>();
        Item.shootSpeed = 0f;
    }
    public override bool CanUseItem(Player player)
    {
        return player.ownedProjectileCounts[Item.shoot] < 1;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<TitanTides>(1).
            AddIngredient<Exoblade>(1).
            AddIngredient<HeavenlyGale>(1).
            AddIngredient<CosmicImmaterializer>(1).
            AddIngredient<Celestus>(1).
            AddIngredient<MiracleMatter>(1).
            AddTile<DraedonsForge>().
            Register();
    }

}

