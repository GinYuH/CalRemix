using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.ExtraTextures;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class FiberBaby : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Fiber Baby");
        Tooltip.SetDefault("Holds out a baby that coughs up bursts of asbestos\n'The one to defeat the hydrogen bomb'");
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.damage = 32;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 4;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void UpdateInventory(Player player)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<FiberBabyHoldout>()] < 1)
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + player.DirectionTo(Main.MouseWorld) * 16f, Vector2.Zero, ModContent.ProjectileType<FiberBabyHoldout>(), Item.damage, 0, player.whoAmI);
    }

    public override bool CanUseItem(Player player)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<FiberBabyHoldout>()] < 1)
            return true;
        return false;
    }
}

