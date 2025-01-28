using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class CalamityRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Calamity Ring");
            Tooltip.SetDefault("A powerful artifact made from the Auric Soul of the Dragon of Calamity\n" +
            "The strength of the auric soul contained within transforms anyone unfortunate enough to wear it into an unsightly eye\n" +
            "It was formerly worn by the Queen of Witches Calamitas to subdue her while she reigned under her handler, King Yharim\n" +
            "Doubles the damage received\n");
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
