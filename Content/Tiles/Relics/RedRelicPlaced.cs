using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class RedRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/RedRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.RedRelic>();
    }
}
