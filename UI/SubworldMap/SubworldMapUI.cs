using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.SubworldMap
{
    public class SubworldMapUI : UIState
    {
        string selected = "";
        Vector2 trueBasePos = Vector2.Zero;
        Vector2 anchorPoint = Vector2.Zero;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (trueBasePos == Vector2.Zero)
            {
                trueBasePos = Main.ScreenSize.ToVector2() / 2f;
            }
            return;
            Main.blockInput = true;
            bool canMove = false;
            bool dragEntire = true;
            string hovered = "";
            float nailHeight = 0.4f;
            List<(string, string)> doneAlready = new(); // List of subworlds that already have lines connecting them

            Texture2D cork = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/Cork").Value;
            Texture2D gradient = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/Gradient").Value;
            string path = "CalRemix/UI/SubworldMap/Frame_";
            Texture2D BR = ModContent.Request<Texture2D>(path + "BR").Value;
            Texture2D BL = ModContent.Request<Texture2D>(path + "BL").Value;
            Texture2D TR = ModContent.Request<Texture2D>(path + "TR").Value;
            Texture2D TL = ModContent.Request<Texture2D>(path + "TL").Value;
            Texture2D T = ModContent.Request<Texture2D>(path + "Top").Value;
            Texture2D S = ModContent.Request<Texture2D>(path + "Side").Value;

            int horzAmt = 8;
            int vertAmt = 5;
            float areaWidth = TR.Width + T.Width * horzAmt;
            float areaHeight = TR.Height + S.Height * vertAmt;
            Rectangle area = Utils.CenteredRectangle(trueBasePos, new Vector2(areaWidth, areaHeight));

            int iconHeight = 120; // How tall the black part of an icon is
            int iconWidth = 160; // How wide is it
            int iconPadding = 10; // How much padding is there
            int extraHeight = 40; // How much taller should the white part be
            Vector2 iconSize = new Vector2(iconWidth, iconHeight);
            Vector2 bgSize = new Vector2(iconWidth + 2 * iconPadding, iconHeight + 2 * iconPadding);

            // Draw the frame and bg
            spriteBatch.Draw(TL, area.TopLeft(), null, Color.White, 0, TL.Size() / 2, 1, 0, 0);
            spriteBatch.Draw(TR, area.TopRight(), null, Color.White, 0, TR.Size() / 2, 1, 0, 0);
            spriteBatch.Draw(BL, area.BottomLeft(), null, Color.White, 0, BL.Size() / 2, 1, 0, 0);
            spriteBatch.Draw(BR, area.BottomRight(), null, Color.White, 0, BR.Size() / 2, 1, 0, 0);
            for (int i = 0; i < horzAmt; i++)
            {
                for (int j = 0; j < vertAmt; j++)
                {
                    spriteBatch.Draw(cork, area.TopLeft() + TL.Size() / 2 + new Vector2(cork.Width * i, cork.Height * j), null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                }
            }
            for (int i = 0; i < vertAmt; i++)
            {
                spriteBatch.Draw(S, area.TopLeft() + Vector2.UnitY * (S.Height * i + TL.Height / 2), null, Color.White, 0, new Vector2(S.Width / 2, 0), 1, 0, 0);
                spriteBatch.Draw(S, area.TopRight() + Vector2.UnitY * (S.Height * i + TL.Height / 2), null, Color.White, 0, new Vector2(S.Width / 2, 0), 1, 0, 0);
            }
            for (int i = 0; i < horzAmt; i++)
            {
                spriteBatch.Draw(T, area.TopLeft() + Vector2.UnitX * (T.Width * i + TL.Height / 2), null, Color.White, 0, new Vector2(0, T.Height / 2), 1, 0, 0);
                spriteBatch.Draw(T, area.BottomLeft() + Vector2.UnitX * (T.Width * i + TL.Height / 2), null, Color.White, 0, new Vector2(0, T.Height / 2), 1, 0, 0);
            }
            for (int i = 0; i < horzAmt; i++)
            {
                spriteBatch.Draw(gradient, area.TopLeft() + new Vector2(T.Width * i + TL.Height / 2, TL.Height / 2 + T.Height / 2), null, Color.White, 0, new Vector2(0, T.Height / 2), 1, 0, 0);
            }

            // Draw the icons
            foreach (var pair in SubworldMapSystem.Items)
            {
                SubworldMapItem item = pair.Value;
                Vector2 basePosition = trueBasePos;
                Vector2 iconPosition = basePosition + item.position;
                if (pair.Key == selected)
                {
                    pair.Value.position = (Main.MouseScreen - basePosition);
                }
                float scale = 1 + MathHelper.Lerp(0, 0.05f, item.animCompletion * 2);
                Rectangle hitbox = Utils.CenteredRectangle(iconPosition, bgSize);
                Rectangle resizedHitbox =  hitbox with { Height = hitbox.Height + extraHeight };
                Rectangle portraitRect = Utils.CenteredRectangle(iconPosition, iconSize);
                float rot = MathF.Sin(Convert.ToInt32(pair.Key[0]) * 2f) * 0.05f;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, hitbox.Center.ToVector2(), resizedHitbox, item.unlockCondition.Invoke() ? Color.White : Color.Gray, rot, hitbox.Size() / 2, 1, 0, 0);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, portraitRect.Center.ToVector2(), portraitRect, Color.Black, rot, portraitRect.Size() / 2, 1, 0, 0);

                if (ModContent.RequestIfExists("CalRemix/UI/SubworldMap/" + pair.Key, out Asset<Texture2D> asset))
                {
                    spriteBatch.Draw(asset.Value, portraitRect.Center.ToVector2(), null, Color.White, rot, asset.Value.Size() / 2, 1, 0, 0);
                }

                Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
                bool intersecting = maus.Intersects(resizedHitbox);
                if (intersecting)
                {
                    if (item.animCompletion < 1)
                    {
                        item.animCompletion = MathHelper.Min(item.animCompletion + 0.11f, 1);
                    }
                    hovered = pair.Key;
                }
                else
                {
                    if (item.animCompletion > 0)
                    {
                        item.animCompletion = MathHelper.Max(item.animCompletion - 0.11f, 0);
                    }
                }
                if (canMove)
                {
                    if (intersecting)
                    {
                        if (selected == "")
                        {
                            if (Main.mouseLeft)
                            {
                                selected = pair.Key;
                            }
                        }
                    }
                }
                if (!Main.mouseLeft)
                {
                    selected = "";
                }
            }
            // Make a list of connections
            foreach (var pair in SubworldMapSystem.Items)
            {
                foreach (string connection in pair.Value.connections)
                {
                    if (!doneAlready.Contains((connection, pair.Key)) && !doneAlready.Contains((pair.Key, connection)))
                    {
                        doneAlready.Add((connection, pair.Key));
                    }
                }
            }
            // Iterate through the list and draw the connections
            foreach (var v in doneAlready)
            {
                SubworldMapItem item1 = SubworldMapSystem.Items[v.Item1];
                SubworldMapItem item2 = SubworldMapSystem.Items[v.Item2];
                if (!item1.unlockCondition.Invoke() || !item2.unlockCondition.Invoke())
                    continue;
                // If unlocked, draw connections
                Vector2 basePosition = trueBasePos;
                Vector2 iconPosition1 = basePosition + item1.position;
                Vector2 iconPosition2 = basePosition + item2.position;
                // If either icon is currently hovered on, make the connection a brighter red
                Color color = (v.Item1 == hovered || v.Item2 == hovered) ? Color.Red : Color.DarkRed;
                Vector2 heighOffset = -Vector2.UnitY * bgSize.Y * nailHeight;
                // Draw the line
                CalRemixHelper.DrawChain(ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/YarnSegment").Value, iconPosition1 + heighOffset, iconPosition2 + heighOffset, MathHelper.PiOver2, color);          
            }

            // Draw the connections
            foreach (var pair in SubworldMapSystem.Items)
            {
                string key = pair.Key;
                SubworldMapItem item = pair.Value;
                bool unlocked = item.unlockCondition.Invoke();
                string displayText = /*pair.Key == "Overworld" ? Main.worldName :*/ unlocked ? CalRemixHelper.LocalText("UI.SubworldMap." + key + ".DisplayName").Value : "???"; // The text to display
                Vector2 basePosition = trueBasePos;
                Vector2 iconPosition = basePosition + item.position;
                //Texture2D tex = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/" + pair.Key).Value;
                //Vector2 origin = tex.Size() / 2;
                float textOffset = 24 + extraHeight;
                float textSpacing = 30;
                int padding = 8;
                Texture2D screw = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/Screw").Value;
                float rot = MathF.Cos(Convert.ToInt32(pair.Key[0]) * 3f);
                spriteBatch.Draw(screw, iconPosition - Vector2.UnitY * bgSize.Y * nailHeight, null, Color.White, rot, screw.Size() / 2, 1, 0, 0); // draw the icon
                // Draw the name 20 pixels below the icon
                if (!unlocked || item.animCompletion <= 0)
                {
                    //Utils.DrawBorderString(spriteBatch, displayText, iconPosition + Vector2.UnitY * textOffset, item.unlockCondition.Invoke() ? Color.White : Color.Gray, anchorx: 0.5f);
                }

                // Draw a description panel
                if (unlocked)
                {
                    if (item.animCompletion > 0)
                    {
                        string[] dialogue = CalRemixHelper.LocalText("UI.SubworldMap." + key + ".Description").Value.Split('\n');

                        // Create measurements for the boxx
                        float maxWidth = 0;
                        foreach (string s in dialogue)
                        {
                            float curWith = FontAssets.MouseText.Value.MeasureString(s).X;
                            if (curWith > maxWidth)
                            {
                                maxWidth = curWith;
                            }
                        }
                        maxWidth += padding * 2;

                        float height = textOffset - extraHeight + (textSpacing + textSpacing * dialogue.Length) * item.animCompletion;

                        Rectangle bg = new Rectangle((int)iconPosition.X - (int)(maxWidth * 0.5f), (int)iconPosition.Y + (int)textOffset - padding, (int)maxWidth, (int)height);
                        Utils.DrawInvBG(spriteBatch,bg, Terraria.ModLoader.UI.UICommon.DefaultUIBlueMouseOver * item.animCompletion);

                        // display position with debug canMove on
                        if (canMove)
                        {
                            displayText += " " + item.position;
                        }

                        Utils.DrawBorderString(spriteBatch, displayText, iconPosition + Vector2.UnitY * textOffset, item.unlockCondition.Invoke() ? Color.White : Color.Gray, anchorx: 0.5f);
                        for (int i = 0; i < dialogue.Length; i++)
                        {
                            Utils.DrawBorderString(spriteBatch, dialogue[i], iconPosition + Vector2.UnitY * textOffset + (Vector2.UnitY * textSpacing + Vector2.UnitY * textSpacing * i) * item.animCompletion, Color.White * item.animCompletion, anchorx: 0.5f);
                        }
                    }
                }
            }

            // Don't allow dragging if an icon is being dragged
            if (selected != "")
            {
                dragEntire = false;
            }

            // Allow moving of the UI
            if (dragEntire)
            {
                // Save the position of the mouse relative to the center of the UI
                if (Main.mouseLeft && anchorPoint == Vector2.Zero)
                {
                    anchorPoint = Main.MouseScreen - trueBasePos;
                }
                // Move the center of the UI based on the anchor point's position relative to the mouse's current position
                else if (Main.mouseLeft && anchorPoint != Vector2.Zero)
                {
                    trueBasePos = Main.MouseScreen - anchorPoint;
                }
                // If released, stop moving and set the anchor point to 0
                else if (!Main.mouseLeft)
                {
                    anchorPoint = Vector2.Zero;
                }
            }
        }
    }

    public class SubworldMapItem
    {
        /// <summary>
        /// A list of subworld keys that this subworld is connected to
        /// </summary>
        public List<string> connections;
        /// <summary>
        /// When should the icon display?
        /// </summary>
        public Func<bool> unlockCondition;
        /// <summary>
        /// The position on the board
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// Animation completion
        /// </summary>
        public float animCompletion = 0;
        /// <summary>
        /// Should the icon display while not unlocked?
        /// </summary>
        public bool hide = true;

        /// <summary>
        /// Creates a Subworld Map item for the map UI
        /// </summary>
        /// <param name="connections">A list of keys for connected subworlds</param>
        /// <param name="unlockCondition">When should this icon start displaying?</param>
        /// <param name="position">Where is the icon relative to the center of the board?</param>
        public SubworldMapItem(List<string> connections, Func<bool> unlockCondition, Vector2 position, bool hide = true)
        {
            this.connections = connections;
            this.unlockCondition = unlockCondition;
            this.position = position;
            this.hide = hide;
        }
    }

    public class SubworldMapSystem : ModSystem
    {
        /// <summary>
        /// Contains all of the Subworld map items
        /// </summary>
        public static Dictionary<string, SubworldMapItem> Items = new();
        public override void Load()
        {
            Items.Add("Overworld", new(["GreatSea", "ScreamingFace", "Ant"], () => true, new Vector2(0, 0), false));
            Items.Add("GreatSea", new([ "Overworld", "ScreamingFace" ], () => true, new Vector2(259, -135), false));
            Items.Add("Ant", new(["Overworld"], () => true, new Vector2(-373, -170), false));
            Items.Add("ScreamingFace", new(["Overworld", "GreatSea"], () => false, new Vector2(467, 211)));
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class SubworldMapRegSystem : ModSystem
    {
        private UserInterface UserInter;

        internal SubworldMapUI SUI;

        public override void Load()
        {
            SUI = new();
            UserInter = new();
            UserInter.SetState(SUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UserInter?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix:SubworldMapInterface",
                    delegate
                    {
                        UserInter.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}