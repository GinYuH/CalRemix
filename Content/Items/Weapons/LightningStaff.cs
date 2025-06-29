using CalamityMod.Items;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class LightningStaff : ModItem
{

    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 125;
        Item.DamageType = DamageClass.Magic;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 23;
        Item.useAnimation = 23;
        Item.mana = 3;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.knockBack = 0;
        Item.rare = ModContent.RarityType<Turquoise>();
        Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        Item.UseSound = BetterSoundID.ItemMagicStaff;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<LightningBolt>();
        Item.shootSpeed = 14f;
    }

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemID.RubyStaff).
            AddIngredient<KrakenTooth>().
            AddIngredient(ItemID.Diamond).
            AddTile(TileID.MythrilAnvil).
            Register();
    }

}

