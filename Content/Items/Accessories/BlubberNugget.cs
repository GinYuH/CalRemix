using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Accessories
{
    public class BlubberNugget : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.resistCold = true;
            player.statDefense += 50;
            player.endurance += 0.2f;
            player.statLifeMax2 += 200;
            player.moveSpeed *= 0.9f;
            player.gravity = 10f;
        }
    }
}
