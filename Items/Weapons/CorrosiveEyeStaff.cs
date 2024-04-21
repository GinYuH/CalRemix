using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class CorrosiveEyeStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Corrosive Eye Staff");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 21;
        Item.DamageType = DamageClass.Summon;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.mana = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 3;
        Item.rare = ItemRarityID.Green;
        Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
        Item.UseSound = SoundID.Item44;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<CorrosiveEye>();
    }

}

