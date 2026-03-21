using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.UI;
using CalRemix.Content.Items.Misc;
using CalamityMod;
using Terraria.ID;
using CalRemix.Core.World;
using CalRemix.Core.Biomes.Subworlds;
using CalamityMod.DataStructures;
using System.Collections.Generic;
using System.IO;
using CalamityMod.Graphics.Primitives;
using rail;

namespace CalRemix.Content.NPCs.Subworlds.Pinnacles
{
    [AutoloadHead]
    public class Bysuinivirit : QuestNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public ref float RibbonAnimation => ref NPC.localAI[0];

        public ref float NeckAnimation => ref NPC.localAI[1];

        public Vector2 HeadPosition
        {
            get => new Vector2(NPC.localAI[2], NPC.localAI[3]);
            set
            {
                NPC.localAI[2] = value.X;
                NPC.localAI[3] = value.Y;
            }
        }

        public static SoundStyle talkSound = new SoundStyle("CalRemix/Assets/Sounds/HenryTalk") with { PitchVariance = 0.75f };
        public override int TextSpeed => 10;

        public override SoundStyle TextSound => talkSound;

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 102;
            NPC.height = 94;
            NPC.lifeMax = 2000;
            NPC.damage = 0;
            NPC.defense = 8;
            NPC.friendly = true;
            NPC.noGravity = false;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PinnaclesBiome>().Type };
        }
        public override void AI()
        {
            base.AI();
            NPC.spriteDirection = NPC.direction;
            Timer++;
            Vector2 defaultPos = NPC.Top + Vector2.UnitY * 40;
            float neckAnimRate = 0.1f;
            if (HeadPosition == default || NPC.velocity != Vector2.Zero)
            {
                HeadPosition = defaultPos;
            }
            if (Timer % 20 == 0)
            {
                NPC.TargetClosest();
                RibbonAnimation = MathF.Sin(Timer / 20f) * 0.5f;
                if (NPCDialogueUI.IsBeingTalkedTo(NPC))
                {
                    NeckAnimation += neckAnimRate;
                    if (NeckAnimation > 1)
                    {
                        NeckAnimation = 1;
                    }

                    bool complete = NeckAnimation == 1;
                    float lerComp = complete ? 0.4f : NeckAnimation;
                    HeadPosition = Vector2.Lerp(HeadPosition, NPC.Top + new Vector2(150 * NPC.direction, 50), lerComp);
                    if (complete)
                    {
                        float ex = NPC.direction == -1 ? MathHelper.Pi : 0;
                        NPC.Calamity().newAI[0] = HeadPosition.DirectionTo(Main.LocalPlayer.Center).ToRotation() + ex;
                    }

                    NeckAnimation = MathHelper.Min(1, NeckAnimation + 0.05f);
                }
                else
                {
                    NPC.Calamity().newAI[0] = 0;
                    NeckAnimation -= neckAnimRate;
                    if (NeckAnimation < 0)
                    {
                        NeckAnimation = 0;
                    }
                    HeadPosition = Vector2.Lerp(HeadPosition, defaultPos, 1 - NeckAnimation);
                    NeckAnimation = MathHelper.Max(0, NeckAnimation - 0.05f);
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(RibbonAnimation);
            writer.WriteVector2(HeadPosition);
            writer.Write(NeckAnimation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            RibbonAnimation = reader.ReadSingle();
            HeadPosition = reader.ReadVector2();
            NeckAnimation = reader.ReadSingle();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "_Head").Value;
            Texture2D neck = ModContent.Request<Texture2D>(Texture + "_Neck").Value;
            Texture2D ribbon = ModContent.Request<Texture2D>(Texture + "_Ribbon").Value;

            spriteBatch.Draw(ribbon, NPC.Bottom - screenPos + new Vector2(20, -60), null, drawColor, NPC.rotation + RibbonAnimation, new Vector2(ribbon.Width / 2, 0), NPC.scale, 0, 0f);
            spriteBatch.Draw(tex, NPC.Bottom - screenPos, null, drawColor, NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0f);

            BezierCurve curve = new BezierCurve(NPC.Center, NPC.Center + new Vector2(50 * NPC.direction, - 250), NPC.Center + new Vector2(100 * NPC.direction, 40), HeadPosition);

            List<Vector2> points = curve.GetPoints(120);
            List<Vector2> finalPoints = new();
            int max = (int)MathHelper.Lerp(1, points.Count, NeckAnimation);
            int buffOff = 2;
            float exOff = NPCDialogueUI.IsBeingTalkedTo(NPC) ? NPC.Calamity().newAI[0] : 0;
            for (int i = 0; i < max; i++)
            {
                Vector2 prev = i == 0 ? NPC.Center : points[i - 1];
                if (i == max - 1)
                {
                    int idx = i;
                    if (max > buffOff)
                    {
                        prev = points[i - buffOff - 1];
                        idx = max - buffOff - 1;
                    }
                    spriteBatch.Draw(head, points[idx] - screenPos, null, drawColor, HeadPosition.DirectionTo(prev).ToRotation() - MathHelper.PiOver2 + exOff, new Vector2(head.Width / 2, head.Height), NPC.scale, NPC.FlippedEffects(), 0f);
                }
                finalPoints.Add(points[i]);
            }

            if (finalPoints.Count > 0)
            {
                PrimitiveRenderer.RenderTrail(finalPoints, new((float f, Vector2 v) => 4, (float f, Vector2 v) => Color.White));
            }


            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}
