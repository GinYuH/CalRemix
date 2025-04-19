using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;
public class Hyperion : ModItem
{
    private static readonly SoundStyle UseSound = new SoundStyle("CalRemix/Assets/Sounds/AOTETeleport") with { MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest };
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Hyperion");
        // Tooltip.SetDefault("skie blok");
        Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(6, 15));
        Item.staff[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.damage = 260;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 300;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.rare = ModContent.RarityType<DarkBlue>();
        Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        Item.UseSound = UseSound;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<HyperionExplosion>(); 
    }
    public override bool AltFunctionUse(Player player) => true;
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse != 2)
        {
            Item.UseSound = SoundID.Item1;
            Item.mana = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileID.None;
        }
        else
        {
            Item.UseSound = UseSound;
            Item.mana = 300;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<HyperionExplosion>();
        }
        return true;
    }
    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2 && player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
        {
            player.Center += player.DirectionTo(Main.MouseWorld) * 256f;
        }
        return true;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.altFunctionUse == 2)
            Projectile.NewProjectile(source, position, velocity, type, damage * 8, knockback);
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<AOTE>().
            AddIngredient<AuricBar>(8).
            AddIngredient<AscendantSpiritEssence>(8).
            AddTile<CosmicAnvil>().
            Register();
    }

}

