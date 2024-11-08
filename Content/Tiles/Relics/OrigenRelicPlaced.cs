using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class OrigenRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/OrigenRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.OrigenRelic>();
    }
}
