using CalRemix.Items.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class AOTE : ModItem
{
    private SoundStyle Use = new($"{nameof(CalRemix)}/Sounds/AOTETeleport");
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Aspect of the End");
        Tooltip.SetDefault("hi pixel");
        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 3));
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 175;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.mana = 50;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.rare = ItemRarityID.Cyan;
        Item.value = Item.sellPrice(gold:30);
        Item.UseSound = Use with { MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest};
        Item.autoReuse = true;

    }
    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
            player.Center += player.DirectionTo(Main.MouseWorld) * 128f;
        return true;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<GlitterEye>(2).
            AddIngredient(ItemID.Diamond).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

