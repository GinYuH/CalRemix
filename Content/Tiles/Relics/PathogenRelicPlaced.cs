using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class PathogenRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/PathogenRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.PathogenRelic>();
    }
}
