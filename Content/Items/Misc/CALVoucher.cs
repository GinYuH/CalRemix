using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class CALVoucher : ModItem
    {
        public override string Texture => "CalRemix/icon_small";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }
        public override bool CanUseItem(Player player) { return true; }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<CalRemixPlayer>().GiveStock(player, "CAL", 1);
            CombatText.NewText(player.getRect(), Color.MediumPurple, "good job you got free stock");
            return true;
        }
    }
}
