using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Items.Accessories;
using CalRemix.UI.Anomaly109;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rail;
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
        public override void Update(GameTime gameTime)
        {
            return;
            bool shouldShow = !Main.gameMenu;


            base.Update(gameTime);
        }

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
            List<(string, string)> doneAlready = new(); // List of subworlds that already have lines connecting them

            Utils.DrawInvBG(spriteBatch, Utils.CenteredRectangle(trueBasePos, Main.ScreenSize.ToVector2()));
            
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
                Texture2D ring = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/Ring").Value;
                Texture2D icon = item.unlockCondition.Invoke() ? ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/" + pair.Key).Value : ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/Unknown").Value;
                Vector2 originRing = ring.Size() / 2;
                Vector2 origin = icon.Size() / 2;
                float scale = 1 + MathHelper.Lerp(0, 0.05f, item.animCompletion * 2);
                spriteBatch.Draw(ring, iconPosition, null, Color.White, 0, originRing, scale, 0, 0); // draw the border ring
                spriteBatch.Draw(icon, iconPosition, null, Color.White, 0, origin, scale, 0, 0); // draw the icon

                Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
                bool intersecting = maus.Distance(iconPosition) < ring.Width * 0.5f;
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
                // Draw the line
                CalRemixHelper.DrawChain(ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/YarnSegment").Value, iconPosition1, iconPosition2, MathHelper.PiOver2, color);          
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
                Texture2D tex = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/" + pair.Key).Value;
                Vector2 origin = tex.Size() / 2;
                float textOffset = 20;
                float textSpacing = 30;
                int padding = 8;
                // Draw the name 20 pixels below the icon
                if (!unlocked || item.animCompletion <= 0)
                    Utils.DrawBorderString(spriteBatch, displayText, iconPosition + Vector2.UnitY * textOffset, item.unlockCondition.Invoke() ? Color.White : Color.Gray, anchorx: 0.5f);

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

                        float height = textOffset + (textSpacing + textSpacing * dialogue.Length) * item.animCompletion;

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