using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using Terraria.Audio;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Placeables.Trophies;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Tiles.Trophies
{
    public class PhytogenTrophyPlaced : ModTile
    {
        public static readonly SoundStyle EatSound = new("CalRemix/Assets/Sounds/TrophyEat");

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            name.SetDefault("Phytogen Trophy");
            AddMapEntry(new Color(255, 255, 255), name);
        }



        #region famine 
        /*
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY >= 55) yield return new Item(ItemID.WeaponRack);
            else yield return new Item(ModContent.ItemType<PhytogenTrophy>());
        }


        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];


            short frameAdjustment = (short)(54);
            if (tile.TileFrameY <= 54)
            {
                ToggleTile(i, j);
                SoundEngine.PlaySound(EatSound, new Vector2(i * 16, j * 16));
                Player player = Main.player[Main.myPlayer];
                player.AddBuff(BuffID.WellFed2, 36000);
                return true;
            }
            return false;
        }

        public void ToggleTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX % 54 / 18;
            int topY = j - tile.TileFrameY % 54 / 18;
            for (int x = topX; x < topX + 3; x++)
            {
                for (int y = topY; y < topY + 2; y++)
                {
                    Main.tile[x, y].TileFrameY += 54;
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendTileSquare(-1, topX, topY, 3, 2);
            }
        }
       */
        #endregion
    }
}