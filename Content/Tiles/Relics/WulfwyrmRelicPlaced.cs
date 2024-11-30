using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class WulfwyrmRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/WulfwyrmRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.WulfwyrmRelic>();
    }
}
