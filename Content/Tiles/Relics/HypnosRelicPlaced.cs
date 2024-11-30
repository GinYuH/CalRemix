using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class HypnosRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/HypnosRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.HypnosRelic>();
    }
}
