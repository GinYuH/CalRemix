using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class LivyatanRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalRemix/Content/Tiles/Relics/LivyatanRelicPlaced";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.LivyatanRelic>();
    }
}
