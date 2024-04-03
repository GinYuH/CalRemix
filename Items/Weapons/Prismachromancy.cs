using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using CalRemix.Projectiles.Weapons;
using CalRemix.Items.Materials;

namespace CalRemix.Items.Weapons;

public class Prismachromancy : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Prismachromancy");
        Tooltip.SetDefault("Dazzle them!");
        Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(12, 8));
        ItemID.Sets.AnimatesAsSoul[base.Type] = true;
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
            AddIngredient<ElementalBar>(5).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }
}