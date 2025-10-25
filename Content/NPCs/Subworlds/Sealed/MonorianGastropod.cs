using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Materials;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using CalamityMod.Sounds;
using CalamityMod.Particles;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class MonorianGastropod : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public Vector2 Anchor
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[3]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[3] = value.Y;
            }
        }

        public List<List<VerletSimulatedSegment>> Tentacles = new();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = NPC.height = 40;
            NPC.lifeMax = 12000;
            NPC.damage = 100;
            NPC.defense = 8;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 1);
            NPC.rarity = 1;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
        }
        public override void AI()
        {
            if (Tentacles.Count <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    List<VerletSimulatedSegment> tentacle = new();
                    Tentacles.Add(CalRemixHelper.CreateVerletChain(ref tentacle, 20, NPC.Center, [0]));
                }
            }
            if (Main.netMode != NetmodeID.Server && Tentacles != null)
                for (int i = 0; i < Tentacles.Count; i++)
                {
                    List<VerletSimulatedSegment> Segments = Tentacles[i];
                    Segments[0].oldPosition = Segments[0].position;
                    Segments[0].position = NPC.Center + Vector2.UnitX * MathHelper.Lerp(-20, 20, (i + 1) / (float)Tentacles.Count);

                    Tentacles[i] = VerletSimulatedSegment.SimpleSimulation(Segments, 3, loops: 5, gravity: 0.3f);

                    NPC.netUpdate = true;
                    NPC.netSpam = 0;
                }
            NPC.TargetClosest();

            switch (State)
            {
                case 0:
                    {
                        Timer++;
                        if (Timer % 150 == 0)
                        {
                            NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.2f, 1.1f);
                        }
                        if (NPC.Center.Y < (Anchor.Y - 200) || NPC.Center.Y > (Anchor.Y + 200))
                        {
                            NPC.velocity.Y *= -1;
                        }
                        if (NPC.justHit && State == 0)
                        {
                            State = 1;
                            Timer = 0;
                            NPC.noTileCollide = false;
                        }
                    }
                    break;
                case 1:
                    {
                        NPC.chaseable = true;
                        Timer++;
                        CalamityUtils.SmoothMovement(NPC, 20, Main.player[NPC.target].Center - NPC.Center, 2, 0.1f, true);

                        if (Timer == 60)
                        {
                            Timer = 0;
                            State = 2;
                        }
                    }
                    break;
                case 2:
                    {
                        NPC.velocity *= 0.97f;
                        Timer++;

                        if (Timer < 90 && Timer % 5 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { PitchVariance = 1 }, NPC.Center);
                            GeneralParticleHandler.SpawnParticle(new PlasmaExplosion(NPC.Center - Vector2.UnitY * 500, Vector2.Zero, Color.Cyan, Vector2.One, Main.rand.NextFloat(), 0.05f, 0.1f, 20));
                        }

                        if (Timer == 90)
                        {
                            SoundEngine.PlaySound(CommonCalamitySounds.LightningSound, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int projCount = 5;
                                for (int i = 0; i < projCount; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - Vector2.UnitY * 500, Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)projCount)) * 10, ProjectileID.VortexLightning, CalRemixHelper.ProjectileDamage(120, 200), 1f, Main.myPlayer, Vector2.UnitY.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)projCount)).ToRotation(), Main.rand.Next(80));
                                }
                            }
                        }

                        if (Timer >= 150)
                        {
                            State = 1;
                            Timer = 0;
                        }
                    }
                    break;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            if (Main.netMode != NetmodeID.Server && Tentacles != null)
                for (int g = 0; g < Tentacles.Count; g++)
                {
                    List<VerletSimulatedSegment> Segments = Tentacles[g];
                    for (int i = 0; i < Segments.Count; i++)
                    {
                        if (i == 0)
                            continue;
                        VerletSimulatedSegment seg = Segments[i];
                        Vector2 truePos = State == 2 ? seg.position + Main.rand.NextVector2Circular(4, 4) : seg.position;
                        float dist = i > 0 ? Vector2.Distance(seg.position, Segments[i - 1].position) : 0;
                        if (dist <= 2)
                            dist = 2;
                        dist += 2;
                        float rot = 0f;
                        if (i > 0)
                            rot = seg.position.DirectionTo(Segments[i - 1].position).ToRotation();
                        else
                            rot = NPC.rotation;
                        float scalee = (1 - (i / Segments.Count)) * 0.8f;
                        Color c = i < 18 ? Color.Black : Color.Pink;
                        Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, truePos - Main.screenPosition, new Rectangle(0, 0, (int)dist, 2), c, rot, TextureAssets.BlackTile.Size() / 2, scalee, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    }
                }
            float animSpeed = State == 2 ? 50 : 7;
            Vector2 scale = new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * animSpeed) * 0.022f, MathF.Cos(Main.GlobalTimeWrappedHourly * animSpeed) * 0.022f);
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale * Vector2.One + scale, 0, 0f);
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Gastrogel>(), 1, 15, 25);
            npcLoot.Add(ModContent.ItemType<MonorianGemShards>(), 1, 6, 12);
        }
    }
}
