using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.NPCs.Crags;
using CalamityMod.Particles;
using CalRemix.Content.Items.Critters;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Misc;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Items.Potions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;

namespace CalRemix.Content.Tiles
{
    public class MincerPlaced : ModTile
    {
        public const int Width = 2;
        public const int Height = 3;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 18];
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MincerTE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            AddMapEntry(Color.Gray, CreateMapEntryName());
        }
        public override bool HasSmartInteract(int i, int j, Terraria.GameContent.ObjectInteractions.SmartInteractScanSettings settings) => true;

        public static int GetAvailableMince()
        {
            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if (MincerTE.minceables.ContainsKey(item.type))
                {
                    return item.type;
                }
            }
            return -1;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Tile tile = Main.tile[i, j];
            int left = i - tile.TileFrameX % (Width * SheetSquare) / SheetSquare;
            int top = j - tile.TileFrameY % (Height * SheetSquare) / SheetSquare;

            // Kill the hosted tile entity directly and immediately.
            MincerTE mincentity = CalamityUtils.FindTileEntity<MincerTE>(i, j, Width, Height, SheetSquare);
            mincentity?.Kill(left, top);
        }


        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;

            int id = GetAvailableMince();

            if (id != -1)
            {
                localPlayer.noThrow = 2;
                localPlayer.cursorItemIconEnabled = true;
                localPlayer.cursorItemIconID = id;
            }
        }

        public override bool RightClick(int i, int j)
        {
            MincerTE te = CalamityUtils.FindTileEntity<MincerTE>(i, j, 2, 3);
            if (te == null)
                return false;
            int id = GetAvailableMince();
            if (id == -1)
                return false;
            if (te.itemID != -1)
                return false;
            te.MinceItem(id);
            SoundEngine.PlaySound(BetterSoundID.ItemDrill, new Vector2(i, j) * 16);
            return true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            MincerTE te = CalamityUtils.FindTileEntity<MincerTE>(i, j, 2, 3);
            if (te == null)
                return false;
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
            {
                if (te.mincingTimer > 0)
                {
                    float yoff = MathHelper.Lerp(0, 6, Utils.GetLerpValue(200, 140, te.mincingTimer, true));
                    Texture2D item = TextureAssets.Item[te.itemID].Value;
                    spriteBatch.Draw(item, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset + new Vector2(16, -16 + yoff) + Main.rand.NextVector2Circular(1, 1), null, Lighting.GetColor(new Point(i, j)), 0, item.Size() / 2, 1f, SpriteEffects.None, 0);
                }
                Texture2D saw = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/MincerSaw").Value;
                spriteBatch.Draw(saw, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset + new Vector2(16, 0), null, Lighting.GetColor(new Point(i, j)), te.mincingTimer, saw.Size() / 2, 1f, SpriteEffects.None, 0);

                if (te.mincingTimer > 0)
                {
                    if (Main.hasFocus)
                    for (int l = 0; l < 12; l++)
                    {
                        GeneralParticleHandler.SpawnParticle(new BloodParticle(new Vector2(i + 1, j) * 16, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(6, 16), 20, Main.rand.NextFloat(0.3f, 1f), Color.Red));
                    }

                    Texture2D legs = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/MincerLegs").Value;
                    spriteBatch.Draw(legs, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset + new Vector2(16, 40), null, Lighting.GetColor(new Point(i, j)), 0, legs.Size() / 2, 1f, SpriteEffects.None, 0);

                    float glob = Main.GlobalTimeWrappedHourly * 10f;
                    Vector2 worble = new Vector2(MathF.Sin(glob), MathF.Cos(glob));

                    Texture2D main = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/MincerMain").Value;
                    spriteBatch.Draw(main, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset + new Vector2(16, 16), null, Lighting.GetColor(new Point(i, j)), 0, main.Size() / 2, Vector2.One + worble * 0.05f, SpriteEffects.None, 0);
                }
            }
            if (te.mincingTimer > 0)
            {
                return false;
            }
            return true;
        }
    }

    public class MincerTE : ModTileEntity
    {
        public struct MinceType(List<Item> items, SoundStyle sound)
        {
            public List<Item> items = items;
            public SoundStyle sound = sound;
        }

        public int mincingTimer = 0;
        public int itemID = -1;

        public static Dictionary<int, MinceType> minceables = new Dictionary<int, MinceType>()
        {
            { ModContent.ItemType<TrubblingItem>(), new (new List<Item>() {
                new (ItemID.GlowingMushroom, 6)
            }, new SoundStyle("CalRemix/Assets/Sounds/Funguscream") { Pitch = 0.5f, Volume = 0.8f }) },
            { ModContent.ItemType<BabySealedPuppet>(), new (new List<Item>() {
                new (ModContent.ItemType<RottedTendril>(), 1),
                new (ModContent.ItemType<Neoncane>(), 3),
                new (ModContent.ItemType<SealedFruit>(), 1),
            }, new SoundStyle("CalRemix/Assets/Sounds/Funguscream") { Pitch = 0.5f, Volume = 0.8f }) }
        };

        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<MincerPlaced>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }

        public void MinceItem(int item)
        {
            itemID = item;
            mincingTimer = 300;
            Main.LocalPlayer.ConsumeItem(item);
        }

        public override void Update()
        {
            if (itemID > -1)
            {
                if (!minceables.ContainsKey(itemID))
                {
                    itemID = -1;
                    mincingTimer = 0;
                    return;
                }
                if (mincingTimer % 20 == 0)
                {
                    SoundStyle drill = BetterSoundID.ItemDrill;
                    SoundEngine.PlaySound(drill with { MaxInstances = 0, Pitch = 0.8f }, Position.ToWorldCoordinates());
                }
                if (mincingTimer == 1)
                {
                    SoundEngine.PlaySound(minceables[itemID].sound, Position.ToWorldCoordinates());
                }
                mincingTimer--;
                if (mincingTimer <= 0)
                {
                    Item choice = Utils.SelectRandom<Item>(Main.rand, minceables[itemID].items.ToArray());
                    int i = Item.NewItem(new EntitySource_TileEntity(this), (Position.X + 1) * 16, (Position.Y + 1) * 16, 2, 2, choice.type, choice.stack);
                    Main.item[i].velocity.Y = 4;
                    Main.item[i].velocity.X = 0;
                    Main.item[i].noGrabDelay = 100;
                    itemID = -1;
                    SoundEngine.PlaySound(DespairStone.ChainsawEndSound, Position.ToWorldCoordinates());
                }
            }
            else
            {
                foreach (Item i in Main.ActiveItems)
                {
                    if (minceables.ContainsKey(i.type))
                    {
                        if (i.Distance(Position.ToWorldCoordinates()) < 32)
                        {
                            i.active = false;
                            mincingTimer = 300;
                            itemID = i.type;
                            break;
                        }
                    }
                }
            }
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData tileData = TileObjectData.GetTileData(type, style, alternate);
            int iMinus = i - 1;
            int jMinus = j - 2;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area. 
                NetMessage.SendTileSquare(Main.myPlayer, iMinus, jMinus, 2, 3);

                //Sync the placement of the tile entity with other clients
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, iMinus, jMinus, Type);

                return -1;
            }

            int placedEntity = Place(iMinus, jMinus);

            return placedEntity;
        }

        public override void OnNetPlace()
        {
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }
    }
}
