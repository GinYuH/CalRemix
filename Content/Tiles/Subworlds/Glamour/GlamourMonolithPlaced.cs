using CalamityMod.Items.Placeables.Furniture.Monoliths;
using CalamityMod.Tiles.BaseTiles;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Placeables.Subworlds.Glamour;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.Glamour
{
    public class GlamourMonolithPlaced : BaseMonolith
    {
        public override int TileWidth => 2;
        public override int TileHeight => 2;
        public override int AnimationFrameCount => 1;
        public override int AnimationDelay => 1;
        public override int CursorItemType => ModContent.ItemType<GlamourMonolith>();

        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ModContent.ItemType<GlamourMonolith>());
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);

            AnimationFrameHeight = TileObjectData.newTile.CoordinateFullHeight;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(196, 43, 191));

            DustType = DustID.DynastyWood;
        }

        public override void NearbyEffects(int i, int j, bool closer, bool monolithEnabled, Player localPlayer)
        {
            if (!monolithEnabled)
                return;

            if (localPlayer is not null && localPlayer.active)
                localPlayer.Remix().glamourMonolith = true;
        }
    }
}
