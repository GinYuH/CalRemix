using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs
{
    public class Vigor : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float LocalTimer => ref NPC.ai[2];
        public ref float AIState => ref NPC.ai[3];

        public static Dictionary<string, VigorDialogue> dialogues = new Dictionary<string, VigorDialogue>();

        public VigorDialogue currentDialogue;

        public enum AITypes
        {
            Nothing = 0,
            Talking = 1

        }

        public override void Load()
        {
            List<(string, int)> introDialogue = new List<(string, int)>
            {
                { ("Ah! Well hello there!", 2) },
                { ("My name is Vigor,\none of the unsightly fellows\ndown here in the depths", 4) },
                { ("Though don't worry,\nthis guy's too heavy and frail\nto cause any harm kekeke", 4) },
                { ("Anyways, I don't\nhave anything to offer really\nso you're free to leave\nwhenever you like", 5) },
                { ("Or maybe you can\nkeep me company for a while, eh?", 3) }
            };

            dialogues.Add("Intro", new VigorDialogue(introDialogue));
        }

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            NPC.width = 102;
            NPC.height = 94;
            NPC.lifeMax = 20;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            if (CalRemixWorld.vigorDialogueLevel >= 0)
            {
                foreach (Projectile pr in Main.ActiveProjectiles)
                {
                    if (pr.type == ProjectileID.PureSpray || pr.type == ProjectileID.PurificationPowder)
                    {
                        if (pr.getRect().Intersects(NPC.getRect()))
                        {
                            CalRemixWorld.vigorDialogueLevel = -1;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (Projectile pr in Main.ActiveProjectiles)
                {
                    if (pr.type == ProjectileID.ViciousPowder || pr.type == ProjectileID.CrimsonSpray)
                    {
                        if (pr.getRect().Intersects(NPC.getRect()))
                        {
                            CalRemixWorld.vigorDialogueLevel = 0;
                            break;
                        }
                    }
                }
                return;
            }
            Player p = Main.player[NPC.target];
            if (p.active && p.Distance(NPC.Center) < 500 && Math.Abs(NPC.Center.Y - p.Center.Y) < 300 && Collision.CanHitLine(NPC.Center - Vector2.UnitY * 40, 1, 1, p.Center, 1, 1) && CalRemixWorld.vigorDialogueLevel == 0)
            {
                currentDialogue = dialogues["Intro"];
            }
            else
            {
                ResetEverything();
                return;
            }
            if (currentDialogue != default)
            {
                Timer++;
                LocalTimer++;
                if (Timer > (currentDialogue.TextDuration() - 5))
                {
                    for (int i = 0; i < 50; i++)
                        Main.BestiaryTracker.Kills.RegisterKill(NPC);
                    ResetEverything();
                    CalRemixWorld.vigorDialogueLevel = 1;
                    CalRemixWorld.UpdateWorldBool();
                }
                else if (LocalTimer >= currentDialogue.text[(int)NPC.ai[3]].Item2 * 60)
                {
                    LocalTimer = 0;
                    NPC.ai[3]++;
                }
                if (currentDialogue != default)
                {
                    if (Timer % 15 == 0 && LocalTimer < currentDialogue.text[(int)NPC.ai[3]].Item2 * 60 - 90)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath13 with { Pitch = Main.rand.NextFloat(-0.4f, 0.4f) }, NPC.Center);
                    }
                }
            }
        }

        public void ResetEverything()
        {
            Timer = 0;
            LocalTimer = 0;
            NPC.ai[3] = 0;
            currentDialogue = default;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Head").Value;
            Texture2D jaw = ModContent.Request<Texture2D>(Texture + "Jaw").Value;
            if (CalRemixWorld.vigorDialogueLevel <= -1)
            {
                tex = ModContent.Request<Texture2D>(Texture + "_Purified").Value;
                head = ModContent.Request<Texture2D>(Texture + "Head_Purified").Value;
                jaw = ModContent.Request<Texture2D>(Texture + "Jaw_Purified").Value;
            }
            Vector2 jawOrig = new Vector2(18, 4);
            Vector2 headOrig = new Vector2(22, 46);
            float jawRot = 0;
            if (currentDialogue != default && CalRemixWorld.vigorDialogueLevel >= 0)
            {
                jawRot = MathF.Sin(Main.GlobalTimeWrappedHourly * 12) * 0.25f;
            }
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(head, NPC.Center - new Vector2(20, 60) - screenPos, null, NPC.GetAlpha(drawColor), 0, headOrig, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(jaw, NPC.Center - new Vector2(18, 70) - screenPos, null, NPC.GetAlpha(drawColor), jawRot, jawOrig, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            if (currentDialogue != default && CalRemixWorld.vigorDialogueLevel >= 0)
            {
                (string, int) currentLine = currentDialogue.text[(int)NPC.ai[3]];
                Vector2 textSize = FontAssets.MouseText.Value.MeasureString(currentLine.Item1);
                float width = textSize.X;
                string ret = "";
                int characterAmt = (int)MathHelper.Lerp(0, currentLine.Item1.Length, Utils.GetLerpValue(0, currentLine.Item2 * 60 - 90, LocalTimer, true));
                for (int i = 0; i < characterAmt; i++)
                {
                    ret += currentLine.Item1[i];
                }
                Vector2 txtDrawPos = NPC.Center + new Vector2(NPC.spriteDirection * (width + 80), -100);
                Vector2 padding = Vector2.One * 6;
                Vector2 start = txtDrawPos - padding;
                Vector2 end = txtDrawPos + textSize + padding;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, start - Main.screenPosition - Vector2.One * 4, new Rectangle(0, 0, (int)(width + 20), (int)(textSize.Y + 20)), Color.IndianRed, 0, Vector2.Zero, 1, 0, 0);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, start - Main.screenPosition, new Rectangle(0, 0, (int)(width + 12), (int)(textSize.Y + 12)), Color.DarkRed, 0, Vector2.Zero, 1, 0, 0);
                Utils.DrawBorderString(spriteBatch, ret, NPC.Center + new Vector2(NPC.spriteDirection * (width + 80), -100) - Main.screenPosition, Color.Yellow);
            }
            return false;
        }
    }

    public class VigorDialogue
    {
        public List<int> headRotations = new List<int> { };
        public List<(string, int)> text = new List<(string, int)>();

        public int TextDuration()
        {
            int duration = 0;
            foreach (var v in text)
            {
                duration += v.Item2 * 60;
            }
            return duration;
        }

        public VigorDialogue(List<(string, int)> text, List<int> headRotations = default)
        {
            this.headRotations = headRotations;
            this.text = text;
        }
    }
}
