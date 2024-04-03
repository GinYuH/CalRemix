using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class TendonTides : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Tendon Tides");
        Tooltip.SetDefault("Inundatio ex Sanguis\n" +
            "Throws tendon slicing disks that shred hit enemies\n" +
            "If an enemy is critically hit, the shred effect stacks and deals more damage over time"); 
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 240;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 6;
        Item.rare = ModContent.RarityType<PureGreen>();
        Item.value = CalamityGlobalItem.Rarity13BuyPrice;
        Item.UseSound = SoundID.Item71;
        Item.autoReuse = true;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<TendonTidesDisc>();
        Item.shootSpeed = 9f;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<TenebrisTides>(1).
            AddIngredient<BloodstoneCore>(7).
            AddIngredient(ItemID.SoulofFright,10).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

