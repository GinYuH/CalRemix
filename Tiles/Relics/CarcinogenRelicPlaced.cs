using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Tiles.Relics
{
    public class CarcinogenRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Tiles/Relics/CarcinogenRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.CarcinogenRelic>();
    }
}
