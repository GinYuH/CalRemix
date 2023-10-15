using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using CalRemix.NPCs;
using CalamityMod.Items.Materials;
using CalamityMod.Gores.Trees;
using CalamityMod.Dusts;

namespace CalRemix.Tiles.PlaguedJungle
{
	public class PipeTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral grass
            GrowsOnTileId = new int[1] { ModContent.TileType<PlaguedGrass>() };
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

        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRemix/Tiles/PlaguedJungle/PipeTreeTops");

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            topTextureFrameWidth = 88;
            topTextureFrameHeight = 74;
            xoffset = 40;
            //What does this code do?
            //treeFrame = (i + j * j) % 3;
        }

        public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("CalRemix/Tiles/PlaguedJungle/PipeTreeBranch");
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRemix/Tiles/PlaguedJungle/PipeTree");
        public override int DropWood() => ModContent.ItemType<Items.Placeables.PlaguedJungle.PlaguedPipe>();
        public override int CreateDust() => (int)CalamityDusts.Plague;

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<PipeSapling>();
        }

        public override int TreeLeaf() => ModContent.GoreType<SulphurLeaf>();

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
            else if (Main.rand.NextBool(20))
            {
                createLeaves = true;
                int type = ModContent.NPCType<PlaguedFirefly>();
                if (Main.raining)
                    type = NPCID.EnchantedNightcrawler;
                NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type);
            }
            else if (Main.rand.NextBool(15))
            {
                createLeaves = true;
                int type = ModContent.ItemType<PlagueCellCanister>();
                if (!Main.dayTime && Main.rand.NextBool())
                    type = ItemID.FallenStar;
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, type, randAmt);
            }
            return false;
        }
	}
} 