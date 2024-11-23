using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.NPCs.Bosses.Ionogen;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
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
            if (NPC.AnyNPCs(ModContent.NPCType<Ionogen>()))
                return false;
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
                    if (player.ionDialogue < IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel].Line.Count - 1)
                    {
                        player.ionDialogue++;
                        cube.ManualTalk();
                    }
                    else
                    {
                        if (CalRemixWorld.ionQuestLevel >= 5)
                        {
                            int num = NPC.NewNPC(new EntitySource_WorldEvent(), i * 16, j * 16, ModContent.NPCType<Ionogen>());
                            if (Main.npc.IndexInRange(num))
                            {
                                CalamityUtils.BossAwakenMessage(num);
                            }
                        }
                        player.ionDialogue = -1;
                        cube.textLifeTime = 0;
                    }
                }
            }
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Ionogen>()))
                return;
            Main.LocalPlayer.cursorItemIconID = ItemID.AnnouncementBox;
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return RemixDowned.downedIonogen;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail)
                return;
            IonCubeTE cube = CalamityUtils.FindTileEntity<IonCubeTE>(i, j, 1, 1);
            cube?.Kill(i, j);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Ionogen>()))
                DrawGuy(spriteBatch, i, j);
            return true;
        }
        public static void DrawChain(SpriteBatch sb, Vector2 tilePos, Vector2 headPos)
        {
            Texture2D chainTex = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ionogen/Cable").Value;

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
                Color chainLightColor = GetDrawColour((int)(tilePos.X / 16), (int)(tilePos.Y / 16), Lighting.GetColor((int)position.X / 16 - 12, (int)position.Y / 16 - 12)); //Lighting of the position of the chain segment
                Vector2 origin = new(chainTex.Width / 2, chainTex.Height); //Draw from center bottom of texture
                sb.Draw(chainTex, position - Main.screenPosition, null, chainLightColor, rotation, origin, scale, SpriteEffects.None, 0);
            }
        }

        public static void DrawGuy(SpriteBatch sb, int i, int j)
        {
            Tile tile = Main.tile[i, j];
            IonCubeTE cube = CalamityUtils.FindTileEntity<IonCubeTE>(i, j, 1, 1);
            if (cube != null)
            {
                DoGuyBehaviour(cube);
            }
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0 && cube != null)
            {
                Player p = Main.LocalPlayer;
                Vector2 tilePos = new Vector2(i * 16, j * 16);
                cube.desiredX = p.position.X > tilePos.X ? -26 : 26;
                Vector2 offset = new Vector2(cube.positionX, 64);
                Vector2 headBop = cube.displayText == "[c/e0122d:I'm sorry.]" ? Vector2.Zero : new Vector2(0, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 22) * 2);
                Vector2 saneWorldPos = tilePos - offset + headBop;
                Vector2 worldPos = saneWorldPos + new Vector2(196, 196);
                bool playerOnRight = p.position.X > saneWorldPos.X;
                bool lookingAtSomething = false;
                if (cube.lookedAtItem >= 0 && cube.lookingAtItem > 0)
                {
                    cube.desiredRotation = saneWorldPos.DirectionTo(Main.item[cube.lookedAtItem].Center).ToRotation();
                    lookingAtSomething = true;
                }
                else if (Main.LocalPlayer.Distance(saneWorldPos) < 720 && Collision.CanHitLine(saneWorldPos, 1, 1, Main.LocalPlayer.Center, 1, 1))
                {
                    cube.desiredRotation = saneWorldPos.DirectionTo(p.Center).ToRotation();
                    lookingAtSomething = true;
                }
                else
                {
                    cube.desiredRotation = 0;
                    cube.desiredX = saneWorldPos.X > ((Main.maxTilesX * 16) / 2) ? -26 : 26;
                    cube.desiredY = 0;
                }
                float rotation = cube.rotation + (lookingAtSomething ? (playerOnRight ? 0 : MathHelper.Pi) : 0);
                Texture2D guy = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ionogen/MasterofIons").Value;
                Texture2D eyes = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ionogen/MasterofIonsEyes").Value;
                SpriteEffects fx = lookingAtSomething ? (playerOnRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None) : saneWorldPos.X > ((Main.maxTilesX * 16) / 2) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                DrawChain(sb, tilePos + new Vector2(196 + 4, 196), worldPos);
                sb.Draw(guy, worldPos - Main.screenPosition, null, GetDrawColour(i, j, Lighting.GetColor((int)saneWorldPos.X / 16, (int)saneWorldPos.Y / 16)), rotation, guy.Size() / 2, 1f, fx, 0f);
                sb.Draw(eyes, worldPos - Main.screenPosition, null, cube.eyeColor, rotation, guy.Size() / 2, 1f, fx, 0f);

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

        // Why isn't in this in the Tile Entity's behaviour you may ask?
        // Well, all of his data is controlled by the local player, so i guess the player is in charge of it
        public static void DoGuyBehaviour(IonCubeTE guy)
        {
            bool shouldUpdate = false;
            guy.positionX = MathHelper.Lerp(guy.positionX, guy.desiredX, 0.05f);
            guy.positionY = MathHelper.Lerp(guy.positionY, guy.desiredX, 0.05f);
            guy.rotation = guy.rotation.AngleLerp(guy.desiredRotation, 0.05f);
            if (guy.textLifeTime > 0)
            {
                guy.textLifeTime--;
            }
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            bool finalQuest = CalRemixWorld.ionQuestLevel >= IonCubeTE.dialogue.Count - 1;
            if (finalQuest ? player.ionDialogue > -1 : (Main.LocalPlayer.Distance(guy.Position.ToVector2() * 16) < 480 && Collision.CanHitLine(guy.Position.ToVector2() * 16, 1, 1, Main.LocalPlayer.Center, 1, 1)))
            {
                if (CalRemixWorld.ionQuestLevel == -1)
                {
                    shouldUpdate = true;
                    CalRemixWorld.ionQuestLevel = 0;

                    ModPacket packet = CalRemix.instance.GetPacket();
                    packet.Write((byte)RemixMessageType.IonQuestLevel);
                    packet.Write((byte)0);
                    packet.Send();

                    player.ionDialogue = 0;
                    guy.ManualTalk();
                }
                if (IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel].Condition != null)
                {
                    if (IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel].Condition.Invoke())
                    {
                        shouldUpdate = true;
                        int lov = CalRemixWorld.ionQuestLevel + 1;
                        CalRemixWorld.ionQuestLevel = lov;

                        ModPacket packet = CalRemix.instance.GetPacket();
                        packet.Write((byte)RemixMessageType.IonQuestLevel);
                        packet.Write((byte)lov);
                        packet.Send();

                        player.ionDialogue = 0;
                        guy.ManualTalk();
                    }
                }
                if (player.ionDialogue > -1)
                {
                    if (guy.textLifeTime < 1)
                    {
                        if (player.ionDialogue < IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel].Line.Count - 1)
                        {
                            shouldUpdate = true;
                            player.ionDialogue++;
                        }
                        else
                        {
                            shouldUpdate = true;
                            if (CalRemixWorld.ionQuestLevel >= IonCubeTE.dialogue.Count - 1)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int num = NPC.NewNPC(new EntitySource_WorldEvent(), guy.Position.X * 16, guy.Position.Y * 16, ModContent.NPCType<Ionogen>());
                                    if (Main.npc.IndexInRange(num))
                                    {
                                        CalamityUtils.BossAwakenMessage(num);
                                    }
                                }
                                else                                     
                                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, Main.myPlayer, ModContent.NPCType<Ionogen>());
                            }
                            player.ionDialogue = -1;
                        }
                    }
                }
                if (player.ionDialogue >= 0 && guy.textLifeTime == 0)
                {
                    shouldUpdate = true;
                    guy.ManualTalk();
                }
            }
            if (guy.lookingAtItem > 0)
            {
                guy.lookingAtItem--;
            }
            if (CalRemixWorld.ionQuestLevel > -1)
            {
                if (IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel].RequiredItem != -1 && guy.lookingAtItem == -1 && player.ionDialogue == -1)
                {
                    foreach (Item i in Main.item)
                    {
                        if (i.Distance(guy.Position.ToVector2() * 16) < 72 && i.active && i.type == IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel].RequiredItem)
                        {
                            shouldUpdate = true;
                            int lov = CalRemixWorld.ionQuestLevel + 1;
                            CalRemixWorld.ionQuestLevel = lov;

                            ModPacket packet = CalRemix.instance.GetPacket();
                            packet.Write((byte)RemixMessageType.IonQuestLevel);
                            packet.Write((byte)lov);
                            packet.Send();

                            guy.lookingAtItem = 240;
                            guy.lookedAtItem = i.whoAmI;
                            player.ionDialogue = 0;
                            guy.ManualTalk();
                            break;
                        }
                    }
                }
            }
            if (guy.lookedAtItem > -1)
            {
                if (Main.item[guy.lookedAtItem] != null)
                {
                    Main.item[guy.lookedAtItem].noGrabDelay = 999;
                }
            }
            if (guy.lookingAtItem == 0 && Main.item[guy.lookedAtItem].active && Main.item[guy.lookedAtItem].type == IonCubeTE.dialogue[CalRemixWorld.ionQuestLevel - 1].RequiredItem)
            {
                shouldUpdate = true;
                Main.item[guy.lookedAtItem].active = false;
                guy.lookingAtItem = -1;
            }
            // FUCK YOUUUUUUUUUUUUU
            if (Main.netMode != NetmodeID.SinglePlayer && shouldUpdate)
            {
                NetMessage.SendData(MessageID.RequestTileEntityInteraction, -1, -1, null, guy.Position.X, guy.Position.Y, guy.Type);
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, guy.Type, guy.Position.X, guy.Position.Y);
                ModPacket packet = CalRemix.instance.GetPacket();
                packet.Write((byte)RemixMessageType.SyncIonmaster);
                packet.Write(guy.ID);
                packet.Write(guy.positionX);
                packet.Write(guy.positionY);
                packet.Write(guy.desiredX);
                packet.Write(guy.desiredY);
                packet.Write(guy.displayText);
                packet.Write(guy.textLifeTime);
                packet.Write(guy.lookedAtItem);
                packet.Write(guy.lookingAtItem);
                packet.Write(guy.rotation);
                packet.Write(guy.desiredRotation);
                packet.Send();
            }
        }

        private static Color GetDrawColour(int i, int j, Color colour)
        {
            int colType = Main.tile[i, j].TileColor;
            Color paintCol = WorldGen.paintColor(colType);
            if (colType >= 13 && colType <= 24)
            {
                colour.R = (byte)(paintCol.R / 255f * colour.R);
                colour.G = (byte)(paintCol.G / 255f * colour.G);
                colour.B = (byte)(paintCol.B / 255f * colour.B);
            }
            return colour;
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

        // The item's index
        public int lookedAtItem = -1;

        // How long he's been looking at the item
        public int lookingAtItem = -1;

        // His current rotation
        public float rotation = 0;
        // His desired rotation
        public float desiredRotation = 0;

        // His eye color
        public Color eyeColor = Color.White;

        // All of his dialogue
        public static List<IonDialogue> dialogue = new List<IonDialogue>() { };

        public struct IonDialogue(List<string> line, int item = -1, Func<bool> condition = null)
        {
            public List<string> Line = line;
            public int RequiredItem = item;
            public Func<bool> Condition = condition;
        }

        public override void Load()
        {
            dialogue.Add(new(new List<string>()
            {
                "Greetings traveller! I am The Plastic Oracle.",
                "You don't seem like one for\nlong tragic backstories,\nso I'll cut to the chase.",
                "I require several objects\nin order to free myself",
                "Bring me everything I demand\nfor riches untold!",
                "Your first mission:",
                $"Bring me a simple [c/C7F25A:bass].\nYou can do that right?\nI'm sure you can!"
            }, ItemID.Bass));
            dialogue.Add(new(new List<string>()
            {
                $"A Bass?\nI asked for a bass\nnot a [i:{ItemID.Bass}] Bass!",
                "You know like, the instrument type?",
                "Ah whatever, this should suffice anyways.",
                "For your next mission:",
                $"Bring me a [i:{ItemID.ShinyRedBalloon}] [c/EB3F4E:Shiny Red Balloon].",
                "This one should be a breeze!\nNow off you go!"
            }, ItemID.ShinyRedBalloon));
            dialogue.Add(new(new List<string>()
            {
                "Yes, yes!\nThank you for the balloon!",
                "How does this help me\nescape you may ask.",
                "Well it doesn't!",
                "It just reminds me of\nthe good ol' days...",
                "Back when everyone was\nfilled with lead instead\nof microplastics y'know?",
                $"Anyways, go get me erm\nWhatever this thing [i:CalamityMod/SurfClam] is."
            }, ModContent.ItemType<SurfClam>()));
            dialogue.Add(new(new List<string>()
            {
                $"Ah! A [i:CalamityMod/SurfClam] Surf Clam!\nThat's what it's called!",
                "Did you know that clams\nare filter feeders?",
                "Meaning they filter food\nout of water using\nhair-like structures across\ntheir gills called cilia?",
                "Nature is so fascinating...",
                "Erm, for your next task:",
                $"Bring me a [i:{ ItemID.RodofDiscord }] [c/f542bc:Rod of Discord]."
            }, ItemID.RodofDiscord));
            dialogue.Add(new(new List<string>()
            {
                "Oh wow!\nI didn't expect you\nto find one so quick!",
                "With the power of\nthis artifact, maybe I can\nteleport out of here!",
                "Freedom...",
                "I desire it very much so...",
                "Oh right, tasks.",
                $"Gather some [i:CalRemix/EssenceofBabil] [c/32A871:Essence of Babil]."
            }, ModContent.ItemType<EssenceofBabil>()));
            dialogue.Add(new(new List<string>()
            {
                "How babulous!",
                "You're such a helpful ally to have!",
                "Your next request...",
                $"[c/e0122d:Kill The Wizard.]",
                $"The guy who wears\nthis hat [i:{ItemID.WizardsHat}] if you're confused.",
                "It's nothing personal\nJust too many mages running amok.\nOnly the strongest may survive",
                "And don't worry about\nhim respawning!",
                "I can assure he [c/E0122D:stays dead],\nand can give you [c/47FF60:all] that\nhe ever could have!",
                "Now go! Go my friend!\nBeat him to a crisp!"
            }, -1, () => CalRemixWorld.wizardDisabled));
            dialogue.Add(new(new List<string>()
            {
                "Great job!\nI knew I could\ncount on you!",
                "You remind me of\nmyself when I was younger...",
                "So much strength...",
                "So much energy...",
                "...",
                "[c/e0122d:I'm sorry.]"
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

            return dialogue[CalRemixWorld.ionQuestLevel].Line[player.ionDialogue];
        }

        public override void Update()
        {
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

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(positionX);
            writer.Write(positionY);
            writer.Write(desiredX);
            writer.Write(desiredY);
            writer.Write(displayText);
            writer.Write(textLifeTime);
            writer.Write(lookedAtItem);
            writer.Write(lookingAtItem);
            writer.Write(rotation);
            writer.Write(desiredRotation);
        }

        public override void NetReceive(BinaryReader reader)
        {
            positionX = reader.ReadSingle();
            positionY = reader.ReadSingle();
            desiredX = reader.ReadSingle();
            desiredY = reader.ReadSingle();
            displayText = reader.ReadString();
            textLifeTime = reader.ReadInt32();
            lookedAtItem = reader.ReadInt32();
            lookingAtItem = reader.ReadInt32();
            rotation = reader.ReadSingle();
            desiredRotation = reader.ReadSingle();
        }
    }
}