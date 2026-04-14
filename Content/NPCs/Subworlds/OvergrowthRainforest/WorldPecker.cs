using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Core.Biomes.Subworlds;
using CalamityMod;
using Terraria.Audio;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using System.IO;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class WorldPecker : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public ref float Phase => ref NPC.ai[1];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.ai[2], NPC.ai[3]);
            set
            {
                NPC.ai[2] = value.X;
                NPC.ai[3] = value.Y;
            }
        }

        public Vector2 OldPosition
        {
            get => new Vector2(NPC.localAI[2], NPC.localAI[3]);
            set
            {
                NPC.localAI[2] = value.X;
                NPC.localAI[3] = value.Y;
            }
        }

        public List<Vector2> segments = new();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 20000;
            NPC.width = 300;
            NPC.height = 300;
            NPC.defense = 999999;
            NPC.lifeMax = 50000000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<CanopiesBiome>().Type };
        }

        public override void AI()
        {
            if (segments.Count <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    segments.Add(NPC.Center);
                }
            }
            NPC.TargetClosest();
            Player p = Main.player[NPC.target];
            Vector2 hoverPos = p.Center + Vector2.UnitX * p.direction * 400;
            switch (Phase)
            {
                // Spawn
                case 0:
                    {
                        int spawnTime = 50;
                        int uncurl = spawnTime + 20;
                        if (Timer <= 1)
                        {
                            OldPosition = NPC.Center;
                            SavePosition = NPC.Center + Vector2.UnitY * 500;
                        }
                        else if (Timer < spawnTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInEasing(Utils.GetLerpValue(0, spawnTime, Timer, true), 1));
                        }
                        else if (Timer < uncurl)
                        {
                            NPC.Center = Vector2.Lerp(NPC.Center, hoverPos, 0.2f);
                        }
                        else if (Timer == uncurl)
                        {
                            Timer = 0;
                            Phase = 1;
                        }
                        segments[3] = OldPosition;
                    }
                    break;
                // Follow
                case 1:
                    {
                        NPC.Center = Vector2.Lerp(NPC.Center, hoverPos, 0.2f);
                        if (Timer > 120)
                        {
                            Timer = 0;
                            Phase = 2;
                        }
                    }
                    break;
                // Bite
                case 2:
                    {
                        int anti = 50;
                        int wait = anti + 10;
                        int strike = wait + 20;
                        int strikeWait = strike + 10;
                        int reelBack = strikeWait + 20;

                        if (Timer <= 1)
                        {
                            OldPosition = NPC.Center;
                        }
                    }
                    break;
                // Despawn
                case 3:
                    {
                        int wait = 40;
                        int goHome = wait + 120;
                        if (Timer <= 1)
                        {
                            OldPosition = NPC.Center;
                            SavePosition = new Vector2(NPC.Center.X - NPC.direction * 200, -40);
                        }
                        else if (Timer > wait)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.ExpInEasing(Utils.GetLerpValue(wait, goHome, Timer, true), 1));
                        }
                        if (Timer > goHome)
                        {
                            for (int i = 0; i < 50; i++)
                                Main.BestiaryTracker.Kills.RegisterKill(NPC);
                            NPC.active = false;
                        }
                    }
                    break;
            }
            segments[0] = NPC.Center;
            Timer++;
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
        }

        public void LerpBezier(Vector2 ctrlOne = default, Vector2 ctrlTwo = default)
        {
            if (ctrlOne != default)
            {
                segments[1] = Vector2.Lerp(segments[1], ctrlOne, NPC.localAI[1]);
            }
            if (ctrlTwo != default)
            {
                segments[2] = Vector2.Lerp(segments[2], ctrlTwo, NPC.localAI[1]);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D head = TextureAssets.Npc[Type].Value;
            Texture2D body = ModContent.Request<Texture2D>(Texture + "_Body").Value;
            Texture2D eye = ModContent.Request<Texture2D>(Texture + "_Eye").Value;
            Texture2D beak = ModContent.Request<Texture2D>(Texture + "_Jaw").Value;
            List<Vector2> segs = new BezierCurve(segments.ToArray()).GetPoints(20);
            for (int i = segs.Count - 1; i >= 0; i--)
            {
                bool drawHead = i == 0;
                Texture2D toUse = drawHead ? head : body;
                float rot = NPC.rotation;
                if (i > 0)
                {
                    rot = 0;
                }
                if (drawHead)
                {
                    spriteBatch.Draw(beak, segs[i] - screenPos + new Vector2(200 * NPC.spriteDirection, 40), null, NPC.GetAlpha(Lighting.GetColor((segs[i]).ToTileCoordinates())), rot, new Vector2(beak.Width, 0), NPC.scale, NPC.FlippedEffects(), 0);
                }
                spriteBatch.Draw(toUse, segs[i] - screenPos, null, NPC.GetAlpha(Lighting.GetColor((segs[i]).ToTileCoordinates())), rot, new Vector2(toUse.Width, toUse.Height / 2), NPC.scale, NPC.FlippedEffects(), 0);
                if (drawHead)
                {
                    Vector2 eyePos = new Vector2(270 * NPC.spriteDirection, 60);
                    eyePos = eyePos + (NPC.Center + eyePos).DirectionTo(Main.player[NPC.target].Center) * 3;
                    spriteBatch.Draw(eye, segs[i] - screenPos + eyePos, null, NPC.GetAlpha(Lighting.GetColor((segs[i]).ToTileCoordinates())), rot, eye.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
                }
            }
            return false;
        }
    }
}
