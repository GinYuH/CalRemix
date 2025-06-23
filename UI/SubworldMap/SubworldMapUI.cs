using CalamityMod.DataStructures;
using CalamityMod.Items.Accessories;
using CalRemix.UI.Anomaly109;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            return;
            bool canMove = false;
            List<string> doneAlready = new(); // List of subworlds that already have lines connecting them
            // Draw the icons
            foreach (var pair in SubworldMapSystem.Items)
            {
                SubworldMapItem item = pair.Value;
                Vector2 basePosition = Main.ScreenSize.ToVector2() / 2f;
                Vector2 iconPosition = basePosition + item.position * 3; // the * 3 is a placeholder, please adjust the real values and remove later
                if (pair.Key == selected)
                {
                    pair.Value.position = (Main.MouseScreen - basePosition) / 3;
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
            // Draw the connections
            foreach (var pair in SubworldMapSystem.Items)
            {
                string key = pair.Key;
                SubworldMapItem item = pair.Value;
                bool unlocked = item.unlockCondition.Invoke();
                string displayText = unlocked ? key : "???"; // The text to display
                Vector2 basePosition = Main.ScreenSize.ToVector2() / 2f;
                Vector2 iconPosition = basePosition + item.position * 3;
                Texture2D tex = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/" + pair.Key).Value;
                Vector2 origin = tex.Size() / 2;
                // If unlocked, draw connections
                if (unlocked)
                {
                    foreach (string connection in item.connections)
                    {
                        if (!doneAlready.Contains(connection))
                        {
                            if (SubworldMapSystem.Items[connection].unlockCondition.Invoke())
                            {
                                Texture2D connectedIcon = ModContent.Request<Texture2D>("CalRemix/UI/SubworldMap/" + connection).Value;
                                Vector2 connectedIconPos = basePosition + SubworldMapSystem.Items[connection].position * 3;
                                Rectangle lineRect = new Rectangle(0, 0, (int)iconPosition.Distance(connectedIconPos), 4);
                                // Draw the line
                                spriteBatch.Draw(TextureAssets.MagicPixel.Value, iconPosition + (connectedIconPos - iconPosition) * 0.5f, lineRect, Color.Red, iconPosition.DirectionTo(connectedIconPos).ToRotation(), lineRect.Size() / 2, 1, 0, 0);
                            }
                        }
                    }
                }
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
                        // TODO: Replace this with a proper dialogue getting thing later
                        List<string> dialogue = new List<string>()
                        {
                            "Contains a lot of water",
                            "Beware of sharks, krakens, and whales",
                            "Also has a underground place with old fish",
                            "No old dukes though",
                            "Syringodium"
                        };

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

                        float height = textOffset + (textSpacing + textSpacing * dialogue.Count) * item.animCompletion;

                        Rectangle bg = new Rectangle((int)iconPosition.X - (int)(maxWidth * 0.5f), (int)iconPosition.Y + (int)textOffset - padding, (int)maxWidth, (int)height);
                        Utils.DrawInvBG(spriteBatch,bg);

                        Utils.DrawBorderString(spriteBatch, displayText, iconPosition + Vector2.UnitY * textOffset, item.unlockCondition.Invoke() ? Color.White : Color.Gray, anchorx: 0.5f);
                        for (int i = 0; i < dialogue.Count; i++)
                        {
                            Utils.DrawBorderString(spriteBatch, dialogue[i], iconPosition + Vector2.UnitY * textOffset + (Vector2.UnitY * textSpacing + Vector2.UnitY * textSpacing * i) * item.animCompletion, Color.White * item.animCompletion, anchorx: 0.5f);
                        }
                    }
                }

                doneAlready.Add(key);
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
        /// Creates a Subworld Map item for the map UI
        /// </summary>
        /// <param name="connections">A list of keys for connected subworlds</param>
        /// <param name="unlockCondition">When should this icon start displaying?</param>
        /// <param name="position">Where is the icon relative to the center of the board?</param>
        public SubworldMapItem(List<string> connections, Func<bool> unlockCondition, Vector2 position)
        {
            this.connections = connections;
            this.unlockCondition = unlockCondition;
            this.position = position;
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
            Items.Add("Overworld", new(["GreatSea", "ScreamingFace", "Ant"], () => true, new Vector2(0, 0)));
            Items.Add("GreatSea", new([ "Overworld", "ScreamingFace" ], () => true, new Vector2(60, 20)));
            Items.Add("Ant", new(["Overworld"], () => true, new Vector2(-80, -40)));
            Items.Add("ScreamingFace", new(["Overworld", "GreatSea"], () => false, new Vector2(80, 60)));
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