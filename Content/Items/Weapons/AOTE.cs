using CalRemix.Content.Items.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;
public class AOTE : ModItem
{
    private static readonly SoundStyle UseSound = new SoundStyle("CalRemix/Assets/Sounds/AOTETeleport") with { MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest };
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Aspect of the End");
        // Tooltip.SetDefault("hi pixel");
        Item.staff[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.damage = 175;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 50;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.rare = ItemRarityID.Cyan;
        Item.value = Item.sellPrice(gold:30);
        Item.UseSound = UseSound;
        Item.useTurn = true;
        Item.autoReuse = true;
    }
    public override bool AltFunctionUse(Player player) => true;
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse != 2)
        {
            Item.UseSound = SoundID.Item1;
            Item.mana = 0;
            Item.useStyle = ItemUseStyleID.Swing;
        }
        else
        {
            Item.UseSound = UseSound;
            Item.mana = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
        }
        return true;
    }
    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2 && player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
            player.Center += player.DirectionTo(Main.MouseWorld) * 128f;
        return true;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<GlitterEye>(2).
            AddIngredient(ItemID.Diamond).
            AddTile(TileID.MythrilAnvil).
            Register();
    }

}

