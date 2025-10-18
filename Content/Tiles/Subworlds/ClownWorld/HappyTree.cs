using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.ClownWorld
{
    public class HappyTree : ModTree
    {
        private Asset<Texture2D> texture;
        private Asset<Texture2D> branchesTexture;
        private Asset<Texture2D> topsTexture;

        // This is a blind copy-paste from Vanilla's PurityPalmTree settings.
        // TODO: This needs some explanations
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            // Makes Example Tree grow on ExampleBlock
            GrowsOnTileId = [ModContent.TileType<FunnyBalloonTile>()];
            texture = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/ClownWorld/HappyTree");
            branchesTexture = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/ClownWorld/HappyTree_Branches");
            topsTexture = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/ClownWorld/HappyTree_Tops");
        }

        // This is the primary texture for the trunk. Branches and foliage use different settings.
        public override Asset<Texture2D> GetTexture()
        {
            return texture;
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<HappySapling>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            topTextureFrameWidth = 98;
            topTextureFrameHeight = 114;
            xoffset = 200; // hey uh this doenst do anything
        }

        // Branch Textures
        public override Asset<Texture2D> GetBranchTextures() => branchesTexture;

        // Top Textures
        public override Asset<Texture2D> GetTopTextures() => topsTexture;

        public override int DropWood()
        {
            return ItemID.Wood;
        }

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, ModContent.ItemType<FunnyBalloon>());
            return false;
        }

        public override int TreeLeaf()
        {
            return ModContent.GoreType<HappyTreeLeaf>();
        }
    }

    public class HappySapling : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<FunnyBalloonTile>()];
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;

            //TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            //TileObjectData.newSubTile.AnchorValidTiles = [ModContent.TileType<ExampleSand>()];
            //TileObjectData.addSubTile(1);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Sapling"));

            TileID.Sets.TreeSapling[Type] = true;
            TileID.Sets.CommonSapling[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]); // Make this tile interact with golf balls in the same way other plants do

            DustType = DustID.RainbowMk2;

            AdjTiles = [TileID.Saplings];
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void RandomUpdate(int i, int j)
        {
            // A random chance to slow down growth
            if (!Main.rand.NextBool(20))
            {
                return;
            }

            Tile tile = Framing.GetTileSafely(i, j); // Safely get the tile at the given coordinates
            bool growSuccess; // A bool to see if the tree growing was successful.

            // Style 0 is for the ExampleTree sapling, and style 1 is for ExamplePalmTree, so here we check frameX to call the correct method.
            // Any pixels before 54 on the tilesheet are for ExampleTree while any pixels above it are for ExamplePalmTree
            if (tile.TileFrameX < 54)
            {
                growSuccess = WorldGen.GrowTree(i, j);
            }
            else
            {
                growSuccess = WorldGen.GrowPalmTree(i, j);
            }

            // A flag to check if a player is near the sapling
            bool isPlayerNear = WorldGen.PlayerLOS(i, j);

            // If growing the tree was a success and the player is near, show growing effects
            if (growSuccess && isPlayerNear)
            {
                WorldGen.TreeGrowFXCheck(i, j);
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects)
        {
            if (i % 2 == 0)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
        }
    }

    public class HappyTreeLeaf : ModGore
    {
        public override string Texture => "CalRemix/Content/Tiles/Subworlds/ClownWorld/HappyTree_Leaf";

        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true; // Leaf gore should appear regardless of the "Blood and Gore" setting
            GoreID.Sets.SpecialAI[Type] = 3; // Falling leaf behavior
            GoreID.Sets.PaintedFallingLeaf[Type] = true; // This is used for all vanilla tree leaves, related to the bigger spritesheet for tile paints
        }
    }
}
