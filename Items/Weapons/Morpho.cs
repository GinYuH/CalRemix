using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Buffs;

namespace CalRemix.Items.Weapons;

public class Morpho : ModItem
{

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Morpho Doomblade");
        Tooltip.SetDefault("Swings a doomblade that shoots crescent slashes\n"
            + "Using RMB summons an Arbiter of Judgement that does not consume minion slots\n"
            + "Attacks from the blade and arbiter inflict Whispering Death and Valfrey Burn\n"
            + "\'Now all will witness its true power!\'");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 1430;
        Item.DamageType = DamageClass.Summon;
        Item.width = 41;
        Item.height = 41;
        Item.useTime = 18;
        Item.useAnimation = 18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = false;
        Item.knockBack = 2.5f;
        Item.rare = ModContent.RarityType<Violet>();
        Item.value = CalamityGlobalItem.Rarity15BuyPrice;
        Item.UseSound = SoundID.Item39 with { Pitch = 0.5f };
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<CrescentSlash>();
        Item.shootSpeed = 12f;

    }
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            Item.UseSound = SoundID.DD2_BetsySummon;
            return player.ownedProjectileCounts[ModContent.ProjectileType<Arbiter>()] < 1;
        }
        Item.UseSound = SoundID.Item39 with { Pitch = 0.5f };
        return true;
    }
    public override bool AltFunctionUse(Player player)
    {
        return true;
    }
    public override void MeleeEffects(Player player, Rectangle hitbox)
    {
        if (Main.rand.NextBool(3))
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Flare);
    }
    public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
    {
        target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 120);
        target.AddBuff(ModContent.BuffType<ValfreyBurn>(), 120);
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<Arbiter>()] < 1)
        {
            position = Main.MouseWorld;
            velocity.X = 0f;
            velocity.Y = 0f;
            int num = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Arbiter>(), damage / 2, knockback, Main.myPlayer);
            if (Main.projectile.IndexInRange(num))
            {
                Main.projectile[num].originalDamage = Item.damage;
            }
            return false;
        }
        else
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<CrescentSlash>(), damage / 2, knockback, Main.myPlayer);
            return false;
        }
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<ButterflyStaff>().
            AddIngredient<ResurrectionButterfly>().
            AddIngredient<NightmareFuel>(10).
            AddIngredient(ItemID.HellButterfly).
            AddIngredient<AuricBar>(5).
            AddTile<CosmicAnvil>().
            Register();
    }

}

