using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class RajahRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/RajahRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.RajahRelic>();
    }
}
