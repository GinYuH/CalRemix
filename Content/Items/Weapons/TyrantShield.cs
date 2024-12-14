using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Cooldowns;

namespace CalRemix.Content.Items.Weapons;

public class TyrantShield : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Tyrant Shield");
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.damage = 5960;
        Item.DamageType = DamageClass.Generic;
        Item.rare = ModContent.RarityType<Violet>();
        Item.value = Item.sellPrice(gold: 90);
    }
    public override void UpdateInventory(Player player)
    {
        if (!player.HasCooldown(TyrantCooldown.ID) && player.ownedProjectileCounts[ModContent.ProjectileType<DeadYharim>()] < 1)
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + player.DirectionTo(Main.MouseWorld) * 16f, Vector2.Zero, ModContent.ProjectileType<DeadYharim>(), Item.damage, 0, player.whoAmI);
    }
    public override bool CanUseItem(Player player) => !player.HasCooldown(TyrantCooldown.ID) && player.ownedProjectileCounts[ModContent.ProjectileType<DeadYharim>()] < 1;
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<AsgardsValor>().
            AddIngredient<AuricBar>(25).
            AddIngredient<OrnateCloth>(5).
            AddIngredient(ItemID.Silk, 20).
            AddIngredient(ItemID.LavaBucket).
            AddTile<CosmicAnvil>().
            Register();
    }

}

