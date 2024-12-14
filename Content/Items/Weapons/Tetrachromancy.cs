using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons;

public class Tetrachromancy : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Tetrachromancy");
        Tooltip.SetDefault("Casts a controllable Polyphe-mini\n" +
            "Once the mouse is released, or on contact with a block, it will split into four homing eyes.\n" +
            "'Now in HD color!'\n" + 
            "'You feel a discreet, distant disproval towards the extra letter in this weapon's name'");
        Item.staff[Type] = true;

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 56;
        Item.value = 100000;
        Item.rare = ItemRarityID.Yellow;
        Item.damage = 80;
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.consumable = false;
        Item.autoReuse = false;
        Item.channel = true;
        Item.DamageType = DamageClass.Summon;
        Item.noMelee = true;
        Item.knockBack = 6.5f;
        Item.shoot = ModContent.ProjectileType<TetrachromancyProjectile>();
        Item.shootSpeed = 10f;
        Item.UseSound = SoundID.Zombie100;
        Item.mana = 20;
    }
}