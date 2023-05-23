using CalamityMod.Items;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class TenebrisTides : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Tenebris Tides");
        Tooltip.SetDefault("Inundatio ex Gladii\n" +
            "Spins a speared baton that summons spears from below the cursor\n" +
            "Enemies hit with the baton are barraged with moldy blades and spears to assault the struck foe"); 
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 160;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 23;
        Item.useAnimation = 23;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 6;
        Item.rare = ItemRarityID.Purple;
        Item.value = CalamityGlobalItem.Rarity11BuyPrice;
        Item.UseSound = SoundID.Item71;
        Item.autoReuse = true;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<TenebrisTidesDisc>();
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
            AddIngredient<TenebreusTides>(1).
            AddIngredient<TyphonsGreed>(1).
            AddIngredient(ItemID.LunarBar, 10).
            AddIngredient<PlantyMush>(50).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

