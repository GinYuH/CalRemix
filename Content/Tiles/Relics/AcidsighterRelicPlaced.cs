using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class AcidsighterRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/AcidsighterRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.AcidsighterRelic>();
    }
}
