using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.Particles;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Tiles.PlaguedJungle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.Sealed
{
    public abstract class SurgarcanePlaced : ModTile
    {
        public virtual Color color => Color.White;

        public virtual int dust => DustID.XenonMoss;

        public virtual int grassType => TileID.Dirt;

        public virtual int itemType => ItemID.SugarPlum;

        public virtual int yieldType => ItemID.ChocolateChipCookie;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileBlockLight[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile | Terraria.Enums.AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorAlternateTiles = new int[] {
                Type
            };
            TileObjectData.newTile.CoordinateHeights = [16];
            TileObjectData.addTile(Type);
            AddMapEntry(color);
            HitSound = SoundID.Grass;
            DustType = dust;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (CalamityUtils.ParanoidTileRetrieval(i, j - 1).HasTile)
                return;
            int maxHeight = 7;
            int curHeight = 1;
            bool dontGrow = false;
            for (int k = j + 1; k < maxHeight; k++)
            {
                Tile t = CalamityUtils.ParanoidTileRetrieval(i, k);
                if (t.TileType == Type)
                {
                    curHeight++;
                }

                if (t.TileType != Type)
                {
                    dontGrow = false;
                    break;
                }
            }

            if (!dontGrow)
            {
                if (WorldGen.PlaceTile(i, j - 1, Type, true))
                {
                    if (Main.tile[i, j - 1].TileType == Type)
                    {
                        Main.tile[i, j - 1].TileFrameY = (short)(18 * Main.rand.Next(0, 3));
                    }
                }

            }
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            List<Item> items = new List<Item>();
            if (Main.rand.NextBool(10))
                items.Add(new Item(itemType));
            int yieldAmt = Main.rand.Next(1, 4);
            
            items.Add(new Item(yieldType, yieldAmt));
            
            return items;
        }
    }

    public class PeatSpirePlaced : SurgarcanePlaced
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(-1);
        }
        public override Color color => new Color(133, 25, 56);

        public override int dust => DustID.Mud;

        public override int grassType => ModContent.TileType<RichMudPlaced>();

        public override int itemType => ModContent.ItemType<PeatSpire>();

        public override int yieldType => ModContent.ItemType<PeatOre>();
    }

    public class NeoncanePlaced : SurgarcanePlaced
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(-1);
        }
        public override Color color => new Color(18, 255, 255);

        public override int dust => DustID.Clentaminator_Cyan;

        public override int grassType => ModContent.TileType<SealedGrassPlaced>();

        public override int itemType => ModContent.ItemType<Neoncane>();

        public override int yieldType => ModContent.ItemType<NeonDust>();
    }

    public class LightColumnPlaced : SurgarcanePlaced
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(-1);
        }
        public override Color color => new Color(253, 255, 138);

        public override int dust => DustID.YellowTorch;

        public override int grassType => ModContent.TileType<DesoilitePlaced>();

        public override int itemType => ModContent.ItemType<LightColumn>();

        public override int yieldType => ModContent.ItemType<LightResidue>();
    }

    public class CookieTowerPlaced : SurgarcanePlaced
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(-1);
        }
        public override Color color => new Color(191, 161, 99);

        public override int dust => DustID.Hay;

        public override int grassType => ModContent.TileType<CarnelianGrassPlaced>();
        public override int itemType => ModContent.ItemType<CookieTower>();

        public override int yieldType => ItemID.ChocolateChipCookie;
    }
    public class SealedBushPlaced : SurgarcanePlaced
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            RegisterItemDrop(-1);
        }
        public override Color color => new Color(148, 102, 141);

        public override int dust => DustID.Ice_Purple;

        public override int grassType => ModContent.TileType<SealedGrassPlaced>();
        public override int itemType => ModContent.ItemType<SealedBush>();

        public override int yieldType => ModContent.ItemType<RottedTendril>();
    }
}