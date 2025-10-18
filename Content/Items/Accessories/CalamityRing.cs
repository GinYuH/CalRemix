using CalamityMod.Rarities;
using CalRemix.UI;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class CalamityRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Calamity Ring");
            /* Tooltip.SetDefault("A powerful artifact made from the Auric Soul of the Dragon of Calamity\n" +
            "The strength of the auric soul contained within transforms anyone unfortunate enough to wear it into an unsightly eye\n" +
            "It was formerly worn by the Queen of Witches Calamitas to subdue her while she reigned under her handler, King Yharim\n" +
            "Doubles the damage received\n"); */
            if (Main.dedServ)
                return;

            HelperMessage calRing1 = HelperMessage.New("CalamityRing1", "No way, it's a Calamity Ring! These terrible rings were manufactured by the King Yharim to turn people into eyes! You'd better not wear it, otherwise you'll take double damage from all sources!",
                "FannyAwooga", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ModContent.ItemType<CalamityRing>()), 8, cantBeClickedOff: true);
            HelperMessage calRing2 = HelperMessage.New("CalamityRing2", "How come it doesn't turn them into an eye? Why's it only make them take more damage? That isn't even mentioned in the flavor text!",
                "EvilFannyCrisped", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(calRing1, delay: 6, startTimerOnMessageSpoken: true);
            HelperMessage calRing3 = HelperMessage.New("CalamityRing3", "                                                                                                                                                                                           ",
                "FannyIdleFrame", HelperMessage.AlwaysShow, 3, cantBeClickedOff: true).ChainAfter(calRing2);
            HelperMessage.New("CalamityRing4", "I don't know",
                "FannyIdle", HelperMessage.AlwaysShow, 5).ChainAfter(calRing3);
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 22, silver: 22);
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CalRemixPlayer>().calamityRing = true;
        }
    }
}
