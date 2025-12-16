using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools;

public class TVRemoteItem : ModItem
{
    public override string Texture => "CalamityMod/Items/SummonItems/Invasion/MartianDistressRemote";

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.maxStack = 1;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = false;
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.rare = ItemRarityID.Blue;
    }
}