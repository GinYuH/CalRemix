using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Relics
{
    public class IonogenRelicPlaced : BaseBossRelic
    {
        public override string RelicTextureName => "CalamityMod/Projectiles/InvisibleProj";

        public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.IonogenRelic>();
    }
}
