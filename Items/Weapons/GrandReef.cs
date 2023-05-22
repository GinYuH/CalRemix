using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class GrandReef : ModItem
{

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Grand Reef");
        // Tooltip.SetDefault("Hallucinations of the deep\n" + "Enemy hits create a large burst of lingering coral shards\n" + "If the flail falls, significantly more shards are created");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 5160;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.width = 40;
        Item.height = 60;
        Item.useTime = 50;
        Item.useAnimation = 50;
        Item.useStyle = 5;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 10f;
        Item.rare = ModContent.RarityType<HotPink>();
        Item.value = CalamityGlobalItem.Rarity16BuyPrice;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<Reef>();
        Item.shootSpeed = 18f;

    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<ClamCrusher>(1).
            AddIngredient<ReaperTooth>(20).
            AddIngredient<Lumenyl>(20).
            AddIngredient<SubnauticalPlate>(5).
            AddTile<CosmicAnvil>().
            Register();
    }

}

