using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Tiles.Relics
{
    public class AcidsighterRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Tiles/Relics/AcidsighterRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.AcidsighterRelic>();
    }
}
