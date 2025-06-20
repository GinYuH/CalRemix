using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis.Host.Mef;
using Terraria.DataStructures;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.Animations;
using rail;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Buffs;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class TempestKraken : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public ref float CurrentPhase => ref NPC.ai[2];

        List<(Vector2, float)> tentacles = new List<(Vector2, float)>();
        public static SoundStyle Kraken = new SoundStyle("CalRemix/Assets/Sounds/KrakenIdle");
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 120;
            NPC.width = 68;
            NPC.height = 306;
            NPC.defense = 100;
            NPC.lifeMax = 100000;
            NPC.value = 100000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath4 with { Pitch = 0.6f };
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            NPC.rarity = 2;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<GreatSeaBiome>().Type };
        }

        public override void AI()
        {
            if (Main.rand.NextBool(500))
            {
                SoundEngine.PlaySound(Kraken with { Volume = 5, MaxInstances = 0 });
            }
            if (NPC.ai[1] == 0)
            {
                NPC.ai[1] = 1;
                int tentAmt = 6;
                for (int i = 0; i < tentAmt; i++)
                {
                    int length = 200;
                    if (i == 0 || i == (tentAmt - 1))
                    {
                        length += 60;
                    }
                    tentacles.Add((-Vector2.UnitX * (MathHelper.Lerp(-20, 20, i / (float)tentAmt)) + Vector2.UnitY * NPC.height * 0.4f, length));
                }
            }

            NPC.TargetClosest(false);
            if (CurrentPhase == 0)
            {
                Timer++;
                if (Timer % 30 == 0 || NPC.collideX || NPC.collideY)
                {
                    if (NPC.velocity.Length() < 1)
                    {
                        NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1, 1.2f);
                    }
                    else
                    {
                        NPC.velocity = NPC.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.5f);
                    }
                }
                if (Timer > 120 && (Main.player[NPC.target].Distance(NPC.Center) < 500 || NPC.justHit))
                {
                    CurrentPhase = 1;
                    Timer = 0;
                }
            }
            else if (CurrentPhase == 1)
            {
                Vector2 dest = Main.player[NPC.target].Center - new Vector2(0, 300);
                NPC.velocity = NPC.DirectionTo(dest) * 2;

                Player p = Main.player[NPC.target];

                if (Main.rand.NextBool(60))
                {
                    SoundEngine.PlaySound(SoundID.Thunder with { Volume = 0.2f }, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[NPC.target].Center + new Vector2(Main.rand.Next(-1000, 1000), -1300), Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4 * 0.5f), ModContent.ProjectileType<TempestLightning>(), (int)(NPC.damage * 0.5f), 0f, Main.myPlayer, 0f, -1);
                }


                if (Main.rand.NextBool(CalamityUtils.SecondsToFrames(600)))
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + Main.rand.Next(-20, 20), (int)NPC.Center.Y + Main.rand.Next(-20, 20), Type, ai2: 1);
                        }
                    }
                }

                if (NPC.Bottom.Y < p.Top.Y && Math.Abs(p.Center.Y - NPC.Center.Y) < 300 && Math.Abs(p.Center.X - NPC.Center.X) < 50 && NPC.ai[3] <= 0 && !p.HasBuff(ModContent.BuffType<DepthGliderBuff>()))
                {
                    NPC.ai[3] = 300 + 120;
                }
                
                if (NPC.ai[3] > 300)
                {
                    p.velocity = p.DirectionTo(NPC.Bottom - Vector2.UnitY * 10) * 10;
                }

                NPC.position.Y = MathHelper.Max(100, NPC.position.Y);

                NPC.ai[3]--;
            }
            else if (CurrentPhase == 2)
            {
                Timer++;
                if (Timer >= 30)
                {
                    CurrentPhase = 1;
                    Timer = 0;

                }
            }

            float pushForce = 0.02f;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC otherNPC = Main.npc[k];
                // Short circuits to make the loop as fast as possible.
                if (!otherNPC.active || k == NPC.whoAmI)
                    continue;

                // If the other npc is indeed the same owned by the same player and they're too close, nudge them away.
                bool sameNPCType = otherNPC.type == NPC.type;
                float taxicabDist = Vector2.Distance(NPC.Center, otherNPC.Center);
                float distancegate = 100f;
                if (sameNPCType && taxicabDist < distancegate)
                {
                    if (NPC.position.X < otherNPC.position.X)
                        NPC.velocity.X -= pushForce;
                    else
                        NPC.velocity.X += pushForce;

                    if (NPC.position.Y < otherNPC.position.Y)
                        NPC.velocity.Y -= pushForce;
                    else
                        NPC.velocity.Y += pushForce;
                }
            }
        }

        public static Vector2 CalculateSegmentPosition(Vector2 start, int tentacle, int segment, int segCount, float length)
        {
            float speedMult = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<TempestKraken>())].ai[3] > 300 ? 7 : 2;
            return start + new Vector2(MathF.Sin((float)segment * 0.05f + Main.GlobalTimeWrappedHourly * (tentacle % 5 * 0.5f + 1) * speedMult + tentacle % 3 * 5f) * 10f, MathHelper.Lerp(0, length, (float)segment / (float)(segCount - 1)));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D hand = ModContent.Request<Texture2D>(Texture + "Hand").Value;
            Texture2D tentacle = ModContent.Request<Texture2D>(Texture + "Tentacle").Value;
            spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied);
            Texture2D tex = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float widthOffset = 0;
            for (int i = 0; i < tentacles.Count; i++)
            {
                var v = tentacles[i];
                int segCount = (int)(v.Item2 * 0.2f);
                Vector2 start = NPC.Center + v.Item1 + Vector2.UnitY * widthOffset;
                Vector2 oldPos = NPC.Center;
                for (int j = 0; j < segCount; j++)
                {
                    Texture2D tenTex = (i == 0 || i == (tentacles.Count - 1)) && j == segCount - 1 ? hand : tentacle;
                    Vector2 pos = CalculateSegmentPosition(start, i, j, segCount, v.Item2) - Vector2.UnitX * 10;

                    float rot = pos.DirectionTo(oldPos).ToRotation();

                    if (j > 0)
                    {
                        oldPos = pos;
                    }
                    if (j < 2) // the first 2 tentacles are covered in mold
                        continue;
                    Rectangle? frame = new Rectangle(0, 0, 18, 44);
                    float completion = j / (float)(segCount - 1);
                    bool startSeg2 = completion > 0.1f;
                    bool startSeg3 = completion > 0.3f;
                    bool startSeg4 = completion > 0.7f;

                    if (j == segCount - 1)
                    {
                        frame = new Rectangle(0, 126, 18, 14);
                    }
                    else if (startSeg4)
                    {
                        frame = new Rectangle(0, 108, 18, 16);
                    }
                    else if (startSeg3)
                    {
                        frame = new Rectangle(0, 78, 18, 28);
                    }
                    else if (startSeg2)
                    {
                        frame = new Rectangle(0, 46, 18, 30);
                    }

                    Vector2 origin = new Vector2(frame.Value.Width / 2, 0);

                    if ((i == 0 || i == (tentacles.Count - 1)) && j == segCount - 1)
                    {
                        frame = null;
                        origin = new Vector2(hand.Width / 2, 0);
                    }

                    spriteBatch.Draw(tenTex, pos - screenPos, frame, NPC.GetAlpha(drawColor), rot + MathHelper.PiOver2, origin, NPC.scale, fx, 0);
                }
            }
            spriteBatch.ExitShaderRegion();
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
