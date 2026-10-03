using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Subworlds;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public class SubworldDoorPlaced : ModTile
    {
        public enum SubworldType
        {
            Ant = 0,
            Bridge = 1,
            Glamour = 2,
            GreatSea = 3,
            Horizon = 4,
            Nightline = 5,
            Nowhere = 6,
            Jungle = 7,
            Forest = 7,
            Overworld = 8,
            Pinnacles = 9,
            Savanna = 10,
            Screaming = 11,
            Sealed = 12,
            Gray = 13,
            Virisite = 14,
            Wolf = 15
        }

        public static List<(string, string, Color)> subworldDoorData = new()
        {
            ("AntSubworld", "Ant", Color.DarkGray),
            ("BridgeofLostHopeSubworld", "Bridge", Color.Firebrick),
            ("GlamourSubworld", "Glamour", Color.HotPink),
            ("GreatSeaSubworld", "GreatSea",Color.DeepSkyBlue),
            ("HorizonSubworld", "Horizon", Color.Tan),
            ("NightlineSubworld", "Nightline", Color.DarkBlue),
            ("NowhereSubworld", "Nowhere", Color.White),
            ("OvergrowthRainforestSubworld", "OvergrowthJungle", Color.ForestGreen),
            ("", "Overworld", Color.LawnGreen),
            ("PinnaclesSubworld", "Pinnacles", Color.Gray),
            ("SavannaSubworld", "Savanna", Color.IndianRed),
            ("ScreamingSubworld", "ScreamingFace", Color.DimGray),
            ("SealedSubworld", "Sealed", Color.Purple),
            ("TheGraySubworld", "TheGray", Color.Black),
            ("SingularPointSubworld", "Virisite", Color.LightSeaGreen),
            ("WolfForestSubworld", "Wolf", Color.LightBlue)
        };

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileOreFinderPriority[Type] = 2222;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<SubworldDoorTE>().Hook_AfterPlacement, -1, 0, false);

            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(75, 139, 166), CalRemixHelper.LocalText("Tiles.SubworldDoorPlaced"));
            DustType = DustID.Stone;
            AnimationFrameHeight = 54;
            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Color final = Color.Lerp(Main.DiscoColor, Color.White, 0.9f) * 0.01f;
            r = final.R;
            g = final.G;
            b = final.B;
        }

        public static void PlaceSubworldDoor(int i, int j, SubworldType key)
        {
            TileEntity.PlaceEntityNet(i - 1, j - 2, ModContent.TileEntityType<SubworldDoorTE>());
            if (TileEntity.ByPosition.TryGetValue(new Point16(i - 1, j - 2), out TileEntity tE))
            {
                if (tE is SubworldDoorTE subDoor)
                {
                    (string, string, Color) data = subworldDoorData[(int)key];
                    subDoor.boundSubworldName = "CalRemix/" + data.Item1;
                    subDoor.texture = "CalRemix/UI/SubworldMap/" + data.Item2;
                    subDoor.doorColor = data.Item3;
                }
            }
            CalRemixHelper.AddProtectedStructure(new Rectangle(i - 1, j - 2, 2, 4));
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            Tile t = Main.tile[i, j];
            Tile parent = CalRemixHelper.ParanoidTileRetrieval(i - t.TileFrameX / 18 % 2, j - t.TileFrameY / 18 % 3);
            if (parent.TileFrameY >= AnimationFrameHeight)
            {
                if (TileEntity.ByPosition.TryGetValue(new Point16(parent.X(), parent.Y()), out TileEntity tE))
                {
                    if (tE is SubworldDoorTE subDoor)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemTeleportMirror);
                        if (subDoor.boundSubworldName == "")
                        {
                            SubworldSystem.Exit();
                        }
                        else
                        { 
                            SubworldSystem.Enter(subDoor.boundSubworldName);
                        }
                    }
                }
            }
            else
            {
                HitWire(i, j);
            }
            return true;
        }

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 3; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < 54)
                        {
                            Main.tile[l, m].TileFrameY += 54;
                        }
                        else
                        {
                            Main.tile[l, m].TileFrameY -= 54;
                        }
                    }
                }
            }
            if (Wiring.running)
            {
                Wiring.SkipWire(x, y);
                Wiring.SkipWire(x, y + 1);
                Wiring.SkipWire(x, y + 2);
                Wiring.SkipWire(x + 1, y);
                Wiring.SkipWire(x + 1, y + 1);
                Wiring.SkipWire(x + 1, y + 2);
            }
            NetMessage.SendTileSquare(-1, x, y + 1, 3);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Main.tile[i, j];
            Tile parent = CalRemixHelper.ParanoidTileRetrieval(i - t.TileFrameX / 18 % 2, j - t.TileFrameY / 18 % 3);
            Color c = Color.White;
            if (TileEntity.ByPosition.TryGetValue(new Point16(parent.X(), parent.Y()), out TileEntity tE))
            {
                if (tE is SubworldDoorTE subDoor)
                {
                    c = subDoor.doorColor;
                    if (t.TileFrameX % 36 == 0 && t.TileFrameY % 54 == 0)
                    {
                        float beamAmt = 15;
                        for (int l = 0; l < beamAmt; l++)
                        {
                            float comp = l / (beamAmt - 1);
                            Vector2 startPos = new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition + new Vector2(16, 24);
                            Vector2 endPos = startPos + Vector2.One.RotatedBy(Main.GlobalTimeWrappedHourly * (l % 2 == 0).ToDirectionInt() * MathHelper.Lerp(0.4f, 1.4f, comp) + l) * MathHelper.Lerp(30, 60, comp);
                            List<Vector2> pts = new();
                            for (int k = 0; k < 30; k++)
                            {
                                Vector2 ppos = Vector2.Lerp(startPos, endPos, k / 29f);
                                int baseDist = 0;
                                float baseRot = 0;
                                if (k > 0)
                                {
                                    Vector2 prev = Vector2.Lerp(startPos, endPos, (k - 1) / 29f);
                                    baseDist = (int)ppos.Distance(prev);
                                    baseRot = ppos.DirectionTo(prev).ToRotation();
                                }
                                int widthMin = 4;
                                int widthMax = 16;
                                int curWith = (int)MathHelper.Lerp(widthMin, widthMax, k / 29f);
                                spriteBatch.Draw(TextureAssets.MagicPixel.Value, ppos, new Rectangle(0, 0, baseDist + 1, curWith), Main.DiscoColor * MathHelper.Lerp(1, 0, k / 29f), baseRot, new Vector2((baseDist + 1) / 2f, curWith / 2), 1, SpriteEffects.None, 0);
                            }
                        }
                    }
                    if (t.TileFrameX % 36 == 0 && t.TileFrameY == 54)
                    {
                        Texture2D tex = ModContent.Request<Texture2D>(subDoor.texture).Value;
                        Vector2 tileSize = new Vector2(32, 54);
                        Main.EntitySpriteDraw(tex, new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset, null, Lighting.GetColor(i, j), 0, Vector2.Zero, tileSize / tex.Size(), 0);
                    }
                }
            }
            Main.EntitySpriteDraw(TextureAssets.Tile[Type].Value, new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset, new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Lighting.GetColor(i, j, c), 0, Vector2.Zero, 1, 0);

            return false;
        }
    }

    public class SubworldDoorTE : ModTileEntity
    {
        public string texture = "CalRemix/Assets/ExtraTextures/SludgeCannon";

        public string boundSubworldName = "";

        public Color doorColor = Color.White;
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData tileData = TileObjectData.GetTileData(type, style, alternate);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area. 
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 2, 3);

                //Sync the placement of the tile entity with other clients
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);

                return -1;
            }

            int placedEntity = Place(i, j);

            return placedEntity;
        }

        public override void OnNetPlace()
        {
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }

        public override bool IsTileValidForEntity(int x, int y)
        {
            return Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<SubworldDoorPlaced>();
        }
        public override void SaveData(TagCompound tag)
        {
            tag["texture"] = texture;
            tag["boundSubworldName"] = boundSubworldName;
            tag["colorR"] = doorColor.R;
            tag["colorG"] = doorColor.G;
            tag["colorB"] = doorColor.B;
        }
        public override void LoadData(TagCompound tag)
        {
            texture = tag.GetString("texture");
            boundSubworldName = tag.GetString("boundSubworldName");
            doorColor = new Color(tag.GetByte("colorR"), tag.GetByte("colorG"), tag.GetByte("colorB"));
        }
    }   
}