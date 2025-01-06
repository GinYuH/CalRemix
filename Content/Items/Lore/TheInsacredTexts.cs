using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Lore;

public class TheInsacredTexts : ModItem
{
    private static readonly SoundStyle PageFlip = new("CalRemix/Assets/Sounds/PageFlip");
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.rare = ModContent.RarityType<CalamityRed>();
        Item.value = 0;
        Item.UseSound = PageFlip with { MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest };
        Item.autoReuse = false;
    }
    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted && !player.GetModPlayer<TheInsacredTextsPlayer>().reading)
            player.GetModPlayer<TheInsacredTextsPlayer>().reading = true;
        return true;
    }
    public override Vector2? HoldoutOffset()
    {
        return new Vector2(-16f, -0f);
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddTile(TileID.Bookcases).
            Register();
    }
}
public class TheInsacredTextsPlayer : ModPlayer
{
    public bool reading = false;
}

