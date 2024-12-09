using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class SickStick : ModItem
{

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Sick Stick");
        Tooltip.SetDefault("Summons sickened cells\nThe original cell glues itself to enemies while secondary ones fire short ranged blood shots\nSecondary cells lock position relative to the original when spawned\nThe greater the distance from the original cell, the more damage");
        ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        Item.staff[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 46;
        Item.DamageType = DamageClass.Summon;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.knockBack = 0;
        Item.rare = RarityHelper.Pathogen;
        Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
        Item.UseSound = BetterSoundID.ItemPoopSquish;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<SickCell>();
        Item.shootSpeed = 0f;
        Item.mana = 14;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // check if the player has enough slots
        float slots = 0;
        foreach (Projectile proj in Main.ActiveProjectiles)
        {
            if (proj.minionSlots > 0)
            {
                slots += proj.minionSlots;
            }
        }
        // if they don't, return
        if ((int)slots > player.maxMinions - 1)
        {
            return false;
        }
        int coreIndex = -1; // index of the core
        foreach (Projectile proj in Main.ActiveProjectiles)
        {
            // get the main cel''s index
            if (proj.type == type && proj.owner == player.whoAmI && proj.ai[0] == -1)
            {
                coreIndex = proj.whoAmI;
            }
        }
        // Spawn a child cell
        if (coreIndex != -1)
        {
            Projectile core = Main.projectile[coreIndex];
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, coreIndex, core.position.X - Main.MouseWorld.X, core.position.Y - Main.MouseWorld.Y);
        }
        // Spawn the main cell
        else
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, -1);
        }
        return false;
    }
}

