using CalamityMod.Tiles.BaseTiles;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class CalamityRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/CalamityRelicPlaced" + (Main.zenithWorld ? string.Empty : "2");
        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.CalamityRelic>();
    }
}
