using CalamityMod;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.Particles;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.Potions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
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
    public class BadrockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(82, 51, 76));
            HitSound = SoundID.Tink;
            DustType = DustID.Ebonwood;
        }
    }
    public class ActivePlumestonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(150, 15, 15));
            HitSound = SoundID.Tink;
            DustType = DustID.Ash;
        }
    }
    public class InactivePlumestonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(99, 24, 34));
            HitSound = SoundID.Tink;
            DustType = DustID.Ash;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile above = CalamityUtils.ParanoidTileRetrieval(i, j - 1);
            Tile side = CalamityUtils.ParanoidTileRetrieval(i + 1, j);
            Tile aboveSide = CalamityUtils.ParanoidTileRetrieval(i + 1, j - 1);
            if (!above.HasTile && side.HasTile && !aboveSide.HasTile && side.TileType == Type)
            {
                if (Main.rand.NextBool(5))
                    GeneralParticleHandler.SpawnParticle(new SmallSmokeParticle(new Vector2(i, j) * 16 + Vector2.UnitX * 16, -Vector2.UnitY * Main.rand.Next(6, 14), Color.Black, Color.Black, 1, 255, 0.1f * Main.rand.NextFloatDirection()));
            }
        }
    }
    public class DesoilitePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(1, 1, 1));
            HitSound = SoundID.Tink;
            DustType = DustID.Obsidian;
        }
    }
    public class LightResiduePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(252, 186, 3));
            HitSound = Cryogen.HitSound with { Pitch = 0.6f };
            DustType = DustID.Ichor;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0;
        }
    }
    public class BrightstonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(211, 214, 169));
            DustType = DustID.TintableDustLighted;
        }
    }
    public class RunicBrightstonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = false;
            Main.tileBlendAll[Type] = false;
            AddMapEntry(new Color(211, 214, 169));
            DustType = DustID.TintableDustLighted;
        }
    }
    public class PorswineManurePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(223, 117, 235));
            DustType = DustID.PurpleMoss;
        }
    }
    public class TurnipMeshPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(30, 0, 33));
            DustType = DustID.Shadowflame;
            HitSound = SoundID.NPCDeath1;
        }
    }
    public class TurnipFruitPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(232, 0, 255));
            DustType = DustID.Shadowflame;
            HitSound = SoundID.NPCDeath1 with { Pitch = 0.4f };
            RegisterItemDrop(ModContent.ItemType<Veinroot>());
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Veinroot>(), Main.rand.Next(2, 6));
        }
    }
    public class TurnipFleshPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(154, 35, 166));
            DustType = DustID.Shadowflame;
            HitSound = SoundID.NPCHit1 with { Pitch = -0.4f };
        }
    }
    public class TurnipLeafPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(90, 176, 127));
            DustType = DustID.Shadowflame;
            HitSound = SoundID.Grass;
        }
    }
    public class VoidInfusedStonePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            Main.tileStone[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(255, 0, 255));
            DustType = DustID.XenonMoss;
            HitSound = SoundID.Shatter;
        }
    }
    public class RichMudPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<SealedDirtPlaced>());
            Main.tileBlockLight[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(117, 33, 47));
            DustType = DustID.Mud;
        }
    }
    public class DarnwoodPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            CalamityUtils.SetMerge(Type, ModContent.TileType<RichMudPlaced>());
            Main.tileBlockLight[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(117, 43, 54));
            DustType = DustID.Mud;
        }
    }
    public class ElementalWoodPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
            AddMapEntry(new Color(156, 62, 128));
            DustType = DustID.RedMoss;
        }
    }

    public class VoidTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral grass
            GrowsOnTileId = new int[1] { ModContent.TileType<VoidInfusedStonePlaced>() };
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

        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/VoidTreeTop");

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            topTextureFrameWidth = 81;
            topTextureFrameHeight = 80;
            xoffset = 40;
        }

        public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/VoidTreeBranch");
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/VoidTree");
        public override int DropWood() => ModContent.ItemType<VoidInfusedStone>();
        public override int CreateDust() => DustID.ArgonMoss;

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<VoidTreeSapling>();
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
            }*/
            else if (Main.rand.NextBool(15))
            {
                createLeaves = true;
                int type = ModContent.ItemType<VoidInfusedStone>();
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, type, randAmt);
            }
            return false;
        }
    }

    public class VoidTreeSapling : ModTile
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
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<VoidInfusedStonePlaced>() };
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
    public class DarnwoodTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral grass
            GrowsOnTileId = new int[1] { ModContent.TileType<RichMudPlaced>() };
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

        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/DarnwoodTreeTop");

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            topTextureFrameWidth = 117;
            topTextureFrameHeight = 94;
            xoffset = 40;
        }

        public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/DarnwoodTreeBranch");
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/DarnwoodTree");
        public override int DropWood() => ModContent.ItemType<Darnwood>();
        public override int CreateDust() => DustID.Mud;

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<DarnwoodSapling>();
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
            }*/
            else if (Main.rand.NextBool(15))
            {
                createLeaves = true;
                int type = ModContent.ItemType<PeatSpire>();
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, type, randAmt);
            }
            return false;
        }
    }

    public class DarnwoodSapling : ModTile
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
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<RichMudPlaced>() };
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

    public class ElementalTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral grass
            GrowsOnTileId = new int[1] { ModContent.TileType<ElementalWoodPlaced>() };
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

        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/ElementalTreeTop");

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            topTextureFrameWidth = 115;
            topTextureFrameHeight = 90;
            xoffset = 40;
        }

        public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/ElementalTreeBranch");
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Sealed/ElementalTree");
        public override int DropWood() => ModContent.ItemType<ElementalWood>();
        public override int CreateDust() => DustID.BlueMoss;

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<ElementalSapling>();
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
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ItemID.Acorn, randAmt);
            }
            else if (Main.rand.NextBool(35) && Main.halloween)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, randAmt);
            }
            else if (Main.rand.NextBool(12))
            {
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, DropWood(), Main.rand.Next(1, 4));
            }
            else if (Main.rand.NextBool(20))
            {
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
            else if (Main.rand.NextBool(15))
            {
                int type = ModContent.ItemType<FireApple>();
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, type, randAmt);
        }
            createLeaves = true;
            Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, ModContent.ItemType<WaterSeeds>(), Main.rand.Next(12, 34));
            return false;
        }
    }

    public class ElementalSapling : ModTile
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
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<ElementalWoodPlaced>() };
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