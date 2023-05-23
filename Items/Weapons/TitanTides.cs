using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class TitanTides : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Titan Tides");
        Tooltip.SetDefault("Inundatio ex Deos\n" +
            "Channels a spinning cosmic aura that grows in size and spin speed over time\n" +
            "Enemies hit by the aura are barraged with dozens of god slayer blades which shred enemies"); 
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 320;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 2;
        Item.useAnimation = 2;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 2;
        Item.rare = ModContent.RarityType<DarkBlue>();
        Item.value = CalamityGlobalItem.Rarity14BuyPrice;
        Item.UseSound = null;
        Item.autoReuse = true;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<TitanTidesDisc>();
        Item.shootSpeed = 0f;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.ownedProjectileCounts[Item.shoot] < 1)
        {
            return true;
        }
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<TendonTides>(1).
            AddIngredient<CosmiliteBar>(10).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

