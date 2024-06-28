using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Tiles.Relics
{
    public class OxygenRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Tiles/Relics/OxygenRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.OxygenRelic>();
    }
}
