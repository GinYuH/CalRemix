using CalamityMod;
using CalamityMod.DataStructures;
using CalRemix.Items;
using log4net.Appender;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace CalRemix.Tiles
{
    public class IonCubePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;
            Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<IonCubeTE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(Color.DarkBlue, name);
        }
        public override bool HasSmartInteract(int i, int j, Terraria.GameContent.ObjectInteractions.SmartInteractScanSettings settings) => true;

        public override bool RightClick(int i, int j)
        {
            IonCubeTE cube = CalamityUtils.FindTileEntity<IonCubeTE>(i, j, 1, 1);
            if (cube != null)
            {
                CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
                if (player.ionDialogue <= -1 && cube.textLifeTime <= 0)
                {
                    player.ionDialogue = 0;
                    cube.ManualTalk();
                }
                else if (player.ionDialogue >= 0)
                {
                    if (player.ionDialogue < IonCubeTE.dialogue[player.ionQuestLevel].Line.Count - 1)
                    {
                        player.ionDialogue++;
                        cube.ManualTalk();
                    }
                    else
                    {
                        player.ionDialogue = -1;
                        cube.textLifeTime = 0;
                    }
                }
            }
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconID = ItemID.AnnouncementBox;
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            IonCubeTE cube = CalamityUtils.FindTileEntity<IonCubeTE>(i, j, 1, 1);
            cube?.Kill(i, j);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DrawGuy(spriteBatch, i, j);
            return true;
        }
        public static void DrawChain(SpriteBatch sb, Vector2 tilePos, Vector2 headPos)
        {
            Texture2D chainTex = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Ionogen/Cable").Value;

            float curvature = MathHelper.Clamp(Math.Abs(tilePos.X - headPos.X) / 50f * 80, 15, 80);

            Vector2 controlPoint1 = tilePos - Vector2.UnitY * curvature;
            Vector2 controlPoint2 = headPos + Vector2.UnitY * curvature;

            BezierCurve curve = new(new Vector2[] { tilePos, controlPoint1, controlPoint2, headPos });
            int numPoints = 16; 
            Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();

            //Draw each chain segment bar the very first one
            for (int i = 1; i < numPoints; i++)
            {
                Vector2 position = chainPositions[i];
                float rotation = (chainPositions[i] - chainPositions[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
                float yScale = Vector2.Distance(chainPositions[i], chainPositions[i - 1]) / chainTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points
                Vector2 scale = new(1, yScale);
                Color chainLightColor = Lighting.GetColor((int)position.X / 16 - 12, (int)position.Y / 16 - 12); //Lighting of the position of the chain segment
                Vector2 origin = new(chainTex.Width / 2, chainTex.Height); //Draw from center bottom of texture
                sb.Draw(chainTex, position - Main.screenPosition, null, chainLightColor, rotation, origin, scale, SpriteEffects.None, 0);
            }
        }

        public static void DrawGuy(SpriteBatch sb, int i, int j)
        {
            Tile tile = Main.tile[i, j];
            IonCubeTE cube = CalamityUtils.FindTileEntity<IonCubeTE>(i, j, 1, 1);
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0 && cube != null)
            {
                Player p = Main.LocalPlayer;
                Vector2 tilePos = new Vector2(i * 16, j * 16);
                cube.desiredX = p.position.X > tilePos.X ? -26 : 26;
                Vector2 offset = new Vector2(cube.positionX, 64);
                Vector2 headBop = new Vector2(0, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 22) * 2);
                Vector2 saneWorldPos = tilePos - offset + headBop;
                Vector2 worldPos = saneWorldPos + new Vector2(196, 196);
                bool playerOnRight = p.position.X > saneWorldPos.X;
                SpriteEffects fx = playerOnRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                if (cube.lookingAtItem <= 0)
                    cube.desiredRotation = saneWorldPos.DirectionTo(p.Center).ToRotation();
                else
                    cube.desiredRotation = saneWorldPos.DirectionTo(Main.item[cube.lookedAtItem].Center).ToRotation();
                float rotation = cube.rotation + (playerOnRight ? 0 : MathHelper.Pi);
                Texture2D guy = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Ionogen/MasterofIons").Value;
                Texture2D eyes = ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Ionogen/MasterofIonsEyes").Value;
                DrawChain(sb, tilePos + new Vector2(196 + 4, 196), worldPos);
                sb.Draw(guy, worldPos - Main.screenPosition, null, Lighting.GetColor((int)saneWorldPos.X / 16, (int)saneWorldPos.Y / 16), rotation, guy.Size() / 2, 1f, fx, 0f);
                sb.Draw(eyes, worldPos - Main.screenPosition, null, cube.eyeColor, rotation, guy.Size() / 2, 1f, fx, 0f);


                /*Utils.DrawBorderString(sb, "You are the hero", worldPos - Main.screenPosition + textOffset, Color.LightBlue);
                Utils.DrawBorderString(sb, "You must solve the", worldPos - Main.screenPosition + (FontAssets.MouseText.Value.MeasureString("Y").Y + 2) * Vector2.UnitY + textOffset, Color.LightBlue);
                Utils.DrawBorderString(sb, "Sacred Puzzles", worldPos - Main.screenPosition + (FontAssets.MouseText.Value.MeasureString("Y").Y * 2 + 4) * Vector2.UnitY + textOffset, Color.Lime);*/

                if (cube.textLifeTime > 0)
                {
                    string dialog = cube.DialogueToDisplay();
                    int breaks = 0;
                    for (int l = 0; l < dialog.Length; l++)
                    {
                        char c = dialog[l];
                        if (c == '\n')
                        {
                            breaks++;
                        }
                    }
                    Vector2 textOffset = new Vector2(-80, -60 - 26 * breaks);
                    Utils.DrawBorderString(sb, cube.displayText, worldPos - Main.screenPosition + textOffset - headBop, cube.textColor);
                }
            }
        }
    }
    public class IonCubeTE : ModTileEntity
    {
        // The current position of the guy relative to his base position
        public float positionX = 0;
        public float positionY = 0;

        // The desired position of the guy relative to his base position
        public float desiredX = 0;
        public float desiredY = 0;

        // Text to display, color of text, and how long text should display
        public string displayText = "";
        public Color textColor = Color.White;
        public int textLifeTime = 0;

        // The type of item he wants
        public int desiredItem = 0;

        // The item's index
        public int lookedAtItem = 0;

        // How long he's been looking at the item
        public int lookingAtItem = 0;

        // His current rotation
        public float rotation = 0;
        // His desired rotation
        public float desiredRotation = 0;

        // His eye color
        public Color eyeColor = Color.White;

        // All of his dialogue
        public static List<IonDialogue> dialogue = new List<IonDialogue>() { };

        public struct IonDialogue(List<string> line)
        {
            public List<string> Line = line;
            public Color Color = Color.White;
        }

        public override void Load()
        {
            dialogue.Add(new(new List<string>() 
            { 
                $"Human, I remember your [c/C61B40:charges].",
                "You don't seem like one for\nlong tragic backstories,\nso I'll cut to the chase.",
                "I require several objects\nin order to free myself",
                "Bring me everything I demand\nfor riches untold!",
                "Your first mission:",
                "Bring me a simple Bass.\nYou can do that right?\nI'm sure you can!" 
            }));
        }

        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<IonCubePlaced>();
        }

        public string DialogueToDisplay()
        {
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (player.ionDialogue <= -1)
                return "";

            return dialogue[player.ionQuestLevel].Line[player.ionDialogue];
        }

        public override void Update()
        {
            positionX = MathHelper.Lerp(positionX, desiredX, 0.05f);
            positionY = MathHelper.Lerp(positionY, desiredX, 0.05f);
            rotation = rotation.AngleLerp(desiredRotation, 0.05f);
            if (textLifeTime > 0)
            {
                textLifeTime--;
            }
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (Main.LocalPlayer.Distance(Position.ToVector2() * 16) < 480)
            {
                if (player.ionQuestLevel == -1)
                {
                    player.ionQuestLevel = 0;
                    player.ionDialogue = 0;
                    ManualTalk();
                }
                if (player.ionDialogue > -1)
                {
                    if (textLifeTime < 1)
                    {
                        if (player.ionDialogue < dialogue[player.ionQuestLevel].Line.Count - 1)
                        {
                            player.ionDialogue++;
                        }
                        else
                        {
                            player.ionDialogue = -1;
                        }
                    }
                }
                if (player.ionDialogue >= 0 && textLifeTime == 0)
                {
                    ManualTalk();
                }
            }
            if (lookingAtItem > 0)
            {
                lookingAtItem--;
                foreach (Item i in Main.item)
                {
                    if (i.Distance(Position.ToVector2() * 16) < 64 && i.active && i.type == desiredItem)
                    {
                        lookingAtItem = 240;
                        lookedAtItem = i.whoAmI;
                    }
                }
            }
            if (lookingAtItem <= 0 && Main.item[lookedAtItem].active && Main.item[lookedAtItem].type == desiredItem)
            {
                Main.item[lookedAtItem].active = false;
                desiredItem = -1;
            }
        }

        public void ManualTalk()
        {
            textLifeTime = (int)MathHelper.Max(DialogueToDisplay().Length * 5, 180);
            displayText = DialogueToDisplay();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData tileData = TileObjectData.GetTileData(type, style, alternate);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area. 
                NetMessage.SendTileSquare(Main.myPlayer, i, j, tileData.Width, tileData.Height);

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

        public static void UpdateHooks()
        {
        }
        public override void SaveData(TagCompound tag)
        {

        }
        public override void LoadData(TagCompound tag)
        {
        }
    }
}