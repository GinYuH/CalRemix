using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    internal class SupremeRajahRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/SupremeRajahRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.SupremeRajahRelic>();
    }
}
