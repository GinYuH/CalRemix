using CalamityMod;
using CalamityMod.Dusts;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Tiles.PlaguedJungle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
    public class CarnelianStonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<CarnelianDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
            AddMapEntry(new Color(161, 19, 28));
            HitSound = SoundID.Tink;
            DustType = DustID.GemRuby;
        }
    }
    public class CarnelianDirtPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<CarnelianStonePlaced>());
            CalamityUtils.SetMerge(Type, ModContent.TileType<CarnelianGrassPlaced>());
            Main.tileBlockLight[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;
            AddMapEntry(new Color(255, 36, 50));
            DustType = DustID.GemRuby;
        }
    }
    public class CarnelianGrassPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;
            DustType = DustID.GemRuby;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Grass"]);
            AddMapEntry(new Color(224, 25, 37));
            TileID.Sets.Grass[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<CarnelianDirtPlaced>();
            Main.tileMerge[Type][ModContent.TileType<CarnelianDirtPlaced>()] = true;
            RegisterItemDrop(ModContent.ItemType<CarnelianDirt>());
        }
    }
    public class CarnelianWoodPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<CarnelianDirtPlaced>());
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(181, 56, 64));
            DustType = DustID.GemRuby;
        }
    }

    public class CarnelianTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral grass
            GrowsOnTileId = new int[1] { ModContent.TileType<CarnelianGrassPlaced>() };
        }

        //Copypasted from vanilla, just as ExampleMod did, due to the lack of proper explanation
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/CarnelianTreeTop");

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            topTextureFrameWidth = 81;
            topTextureFrameHeight = 80;
            xoffset = 40;
        }

        public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/CarnelianTreeBranch");
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/CarnelianTree");
        public override int DropWood() => ModContent.ItemType<CarnelianWood>();
        public override int CreateDust() => DustID.GemRuby;

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<CarnelianSapling>();
        }

        public override int TreeLeaf() => -1;

        // Returning false at the end prevents vanilla behavior as the default is forest tree behavior which can include undesirable stuff like squirrels and butterflies
        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            int randAmt = Main.rand.Next(1, 3);
            if (Main.getGoodWorld && Main.rand.NextBool(15))
            {
                Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, Main.rand.NextFloat(-100f, 100f) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Player.FindClosest(new Vector2(x * 16, y * 16), 16, 16));
            }
            else if (Main.rand.NextBool(7))
            {
                createLeaves = true;
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ItemID.Acorn, randAmt);
            }
            else if (Main.rand.NextBool(35) && Main.halloween)
            {
                createLeaves = true;
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, randAmt);
            }
            else if (Main.rand.NextBool(12))
            {
                createLeaves = true;
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, DropWood(), Main.rand.Next(1, 4));
            }
            else if (Main.rand.NextBool(20))
            {
                createLeaves = true;
                int coin = ItemID.CopperCoin;
                int amount = Main.rand.Next(50, 100);
                if (Main.rand.NextBool(30))
                {
                    coin = ItemID.GoldCoin;
                    amount = 1;
                    if (Main.rand.NextBool(5))
                        amount++;

                    if (Main.rand.NextBool(10))
                        amount++;
                }
                else if (Main.rand.NextBool(10))
                {
                    coin = ItemID.SilverCoin;
                    amount = Main.rand.Next(1, 21);
                    if (Main.rand.NextBool(3))
                        amount += Main.rand.Next(1, 21);

                    if (Main.rand.NextBool(4))
                        amount += Main.rand.Next(1, 21);
                }

                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, coin, amount);
            }
            /*else if (Main.rand.NextBool(20))
            {
                createLeaves = true;
                int type = ModContent.NPCType<CarnelianFruit>();
                if (Main.raining)
                    type = NPCID.EnchantedNightcrawler;
                CalRemixHelper.SpawnNewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type);
            }
            else if (Main.rand.NextBool(15))
            {
                createLeaves = true;
                int type = ModContent.ItemType<PlagueCellCanister>();
                if (!Main.dayTime && Main.rand.NextBool())
                    type = ItemID.FallenStar;
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, type, randAmt);
            }*/
            return false;
        }
    }

    public class CarnelianSapling : ModTile
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
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<CarnelianGrassPlaced>() };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
            TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            TileObjectData.addSubTile(1);
            TileObjectData.addTile(Type);
            TileID.Sets.CommonSapling[Type] = true;
            TileID.Sets.TreeSapling[Type] = true;
            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Sapling"));

            AdjTiles = new int[] { TileID.Saplings };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void RandomUpdate(int i, int j)
        {
            if (WorldGen.genRand.Next(20) == 0)
            {
                Tile tile = Framing.GetTileSafely(i, j);
                bool growSucess;
                growSucess = WorldGen.GrowPalmTree(i, j);
                bool isPlayerNear = WorldGen.PlayerLOS(i, j);
                if (growSucess && isPlayerNear)
                    WorldGen.TreeGrowFXCheck(i, j);
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects)
        {
            if (i % 2 == 1)
                effects = SpriteEffects.FlipHorizontally;
        }
    }
}