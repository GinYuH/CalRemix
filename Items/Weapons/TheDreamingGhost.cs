using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class TheDreamingGhost : ModItem
{

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("The Dreaming Ghost");
        Tooltip.SetDefault("Beneath the tree's roots lies the pain of a thousand souls\n" +
                "Summons a pair of crystalline butterflies to fight for you\n" +
                "Each active purple butterfly increases your damage by 10%\n" +
                "Each active pink butterfly increases your life regeneration by 2"); 
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.rare = ModContent.RarityType<Violet>();
        Item.value = CalamityGlobalItem.Rarity15BuyPrice;
        Item.Calamity().donorItem = true;
        Item.damage = 468;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 10;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 1;
        Item.UseSound = SoundID.Item101;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<PinkCrystallineButterfly>();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        position = Main.MouseWorld;
        velocity.X = 0f;
        velocity.Y = 0f;
        for (int i = 0; i < 10; i++)
        {
            Dust.NewDust(position, Item.width, Item.height, DustID.PinkCrystalShard, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
        }
        if (player.slotsMinions < player.maxMinions - 4)
        {
            int num = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PinkCrystallineButterfly>(), damage / 2, knockback, Main.myPlayer);
            int num2 = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PurpleCrystallineButterfly>(), damage / 2, knockback, Main.myPlayer);
            if (Main.projectile.IndexInRange(num))
            {
                Main.projectile[num].originalDamage = Item.damage;
                Main.projectile[num2].originalDamage = Item.damage;
            }
        }
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<ResurrectionButterfly>(1).
            AddIngredient<SanctifiedSpark>(1).
            AddIngredient<YharonSoulFragment>(15).
            AddIngredient<CosmiliteBar>(20).
            AddTile<CosmicAnvil>().
            Register();
    }

}

