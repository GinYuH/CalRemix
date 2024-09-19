using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Melee;

namespace CalRemix.Content.Items.Weapons;

public class Unloxcalibur : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Unloxcalibur");
        Tooltip.SetDefault("An average broadsword"); 
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.damage = 100;
        Item.DamageType = DamageClass.Melee;
        Item.width = 90;
        Item.height = 90;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 13f;
        Item.rare = ItemRarityID.Blue;
        Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;

    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<Roxcalibur>().
            AddIngredient(ItemID.Extractinator).
            AddIngredient(ItemID.FragmentSolar).
            AddIngredient(ItemID.GoldenKey).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

