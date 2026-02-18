using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class AnomalyRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/AnomalyRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.AnomalyRelic>();
    }
}
