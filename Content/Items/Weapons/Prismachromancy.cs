using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons;

public class Prismachromancy : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Prismachromancy");
        Tooltip.SetDefault("Dazzle them!");
        Item.staff[Type] = true;

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 56;
        Item.value = 100000;
        Item.rare = 8;
        Item.damage = 100;
        Item.useAnimation = 45;
        Item.useTime = 45;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.consumable = false;
        Item.autoReuse = false;
        Item.channel = true;
        Item.DamageType = DamageClass.Summon;
        Item.noMelee = true;
        Item.knockBack = 6.5f;
        Item.shoot = ModContent.ProjectileType<PrismachromancyProjectile>();
        Item.shootSpeed = 10f;
        Item.UseSound = SoundID.Item110;
        Item.mana = 20;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<Tetrachromancy>().
            AddIngredient(ItemID.FairyQueenMagicItem).
            AddIngredient(ItemID.RainbowRod).
            AddIngredient(ItemID.LunarBar, 5).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }
}