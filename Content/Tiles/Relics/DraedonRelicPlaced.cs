using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class DraedonRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/DraedonRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.DraedonRelic>();
    }
}
