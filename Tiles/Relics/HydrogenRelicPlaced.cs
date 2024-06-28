using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Tiles.Relics
{
    public class HydrogenRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Tiles/Relics/HydrogenRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.HydrogenRelic>();
    }
}
