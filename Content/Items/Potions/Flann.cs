using CalamityMod;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Flann : ModItem
    {
        public static HelperMessage FlannMessage;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 0;
            ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
            if (!Main.dedServ)
            {
                FlannMessage = HelperMessage.New("EatFlann", "What?",
                    "FannySob", (ScreenHelperSceneMetrics scene) => true).NeedsActivation().SetHoverTextOverride("In the end, all things crumble.");
            }
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.value = Item.sellPrice(copper: 1);
            Item.UseSound = BetterSoundID.ItemEat;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed;
            Item.buffTime = CalamityUtils.SecondsToFrames(180);
        }

        public override bool? UseItem(Player player)
        {
            if (FlannMessage != null)
                FlannMessage.ActivateMessage();
            return true;
        }
    }
}
