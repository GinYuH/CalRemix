using CalRemix.Content.Items.Armor;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public class ArmorPolisherPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(160, 20, 200), name);
        }

        public override bool RightClick(int i, int j)
        {
            bool itemChanged = false;
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<YellowLightLeggings>())
            {
                Main.LocalPlayer.HeldItem.ChangeItemType(ModContent.ItemType<EnchantedYellowLightLeggings>());
                itemChanged = true;
            }
            else if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<SealedLeggings>())
            {
                Main.LocalPlayer.HeldItem.ChangeItemType(ModContent.ItemType<EnchantedSealedLeggings>());
                itemChanged = true;
            }
            if (itemChanged)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemManaCrystal with { Pitch = 0.5f }, Main.LocalPlayer.Center);
                Vector2 worldPos = new Vector2(i, j) * 16;
                CalRemixHelper.DustExplosionOutward(worldPos, DustID.MagicMirror, speedMin: 6f, speedMax: 10f, scale: 1);
                return true;
            }
            return false;
        }
    }
}
