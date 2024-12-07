using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class CalamityRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/CalamityRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.CalamityRelic>();
    }
}
