using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Weapons;

public class XenonCutlass : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.damage = 112;
        Item.DamageType = DamageClass.Melee;
        Item.width = 90;
        Item.height = 90;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 2.25f;
        Item.rare = ItemRarityID.Red;
        Item.value = CalamityGlobalItem.RarityRedBuyPrice;
        Item.UseSound = BetterSoundID.ItemPhaseblade;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<XenonSlash>();
        Item.shootSpeed = 22f;

    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemID.PearlwoodSword).
            AddIngredient(ItemID.DD2SquireBetsySword).
            AddIngredient<MercuryCoatedSubcinium>(5).
            AddTile(TileID.MythrilAnvil).
            Register();
    }

}

