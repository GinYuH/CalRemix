using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.NPCs.Abyss;
using CalamityMod.Projectiles.Boss;
using CalRemix.Content.Items.Potions;
using CalRemix.Core.Biomes.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Calamachnid : ModNPC
    {
        public bool HasChosenSpotToHideIn => SpotToHideIn != Vector2.Zero;
        public Point TileCoordsToHideIn
        {
            get => SpotToHideIn.ToTileCoordinates();
            set => SpotToHideIn = value.ToWorldCoordinates();
        }
        public Vector2 SpotToHideIn
        {
            get => new Vector2(NPC.Remix().GreenAI[3], NPC.Remix().GreenAI[4]);
            set
            {
                NPC.Remix().GreenAI[3] = value.X;
                NPC.Remix().GreenAI[4] = value.Y;
            }
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Spider;
            NPC.damage = 120;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 8;
            NPC.lifeMax = 3000;
            NPC.knockBackResist = 0.1f;
            NPC.value = Item.buyPrice(silver: 10);
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit38 with { Pitch = 0.1f };
            NPC.DeathSound = SoundID.NPCDeath41 with { Pitch = 0.3f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type];
        }

        List<VerletSimulatedSegment> segments = new List<VerletSimulatedSegment>();


        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, 2, 2, 1);
            NPC.Remix().GreenAI[2] += 0.02f;
            NPC.TargetClosest();
            if (NPC.Remix().GreenAI[0] == 0)
            {
                #region Calamity Mod Sunken Sea update 2033
                // Choose an initial tile to hide in.
                if (Main.netMode != NetmodeID.MultiplayerClient && !HasChosenSpotToHideIn)
                {
                    int tries;
                    int tot = 222;
                    for (tries = 0; tries < tot; tries++)
                    {
                        int x = (int)(NPC.Center.X / 16f) + Main.rand.Next(-50, 50);
                        int y = (int)(NPC.Center.Y / 16f) + Main.rand.Next(-50, 50);
                        Tile tile = CalamityUtils.ParanoidTileRetrieval(x, y);

                        // Try again if the tile isn't solid or isn't exposed to air.
                        if (!WorldGen.SolidTile(tile) || CalamityUtils.ParanoidTileRetrieval(x, y + 1).HasTile)
                            continue;

                        bool sb = false;

                        int lootry = 0;
                        for (int j = y + 1; j < y + 10; j++)
                        {
                            lootry++;
                            if (CalamityUtils.ParanoidTileRetrieval(x, j).HasTile)
                            {
                                sb = true;
                                break;
                            }
                        }

                        if (sb)
                            continue;

                        // Try again if there's no open water near the tile.
                        Vector2 moveDirection = Vector2.UnitY;
                        Vector2 collisionCheckPosition = new Vector2(x * 16f + 8f, y * 16f + 8f) + moveDirection * 16f;
                        float collisionDistance = CalamityUtils.DistanceToTileCollisionHit(collisionCheckPosition, moveDirection, 20) ?? 20;
                        if (collisionDistance <= 10)
                            continue;

                        TileCoordsToHideIn = new Point(x, y);
                        break;
                    }

                    // Just die if no spot was suitable.
                    if (tries >= (tot - 1))
                    {
                        NPC.active = false;
                        return false;
                    }

                    NPC.Center = SpotToHideIn;
                    NPC.netUpdate = true;
                }

                // Prevent the tile from being destroyed.
                FixExploitManEaters.ProtectSpot(TileCoordsToHideIn.X, TileCoordsToHideIn.Y);
                #endregion

                if (HasChosenSpotToHideIn)
                {
                    CalRemixHelper.CreateVerletChain(ref segments, 11, SpotToHideIn, [0]);


                    segments = VerletSimulatedSegment.SimpleSimulation(segments, 16, 10, 4);
                    segments[0].oldPosition = segments[0].position;
                    segments[0].position = SpotToHideIn;
                    segments[^1].position.X += MathF.Sin(NPC.Remix().GreenAI[2]) * 0.15f;

                    NPC.Center = segments[^1].position;
                    NPC.rotation = NPC.DirectionTo(segments[^2].position).ToRotation();
                }

                NPC.TargetClosest();
                if ((Main.player[NPC.target].Distance(NPC.Center) < 200f  && NPC.HasSight(Main.player[NPC.target].Center)) || NPC.justHit)
                {
                    NPC.Remix().GreenAI[0] = 1;
                    SoundEngine.PlaySound(SoundID.NPCDeath35 with { Pitch = 0.8f }, NPC.Center);
                    NPC.netUpdate = true;
                }
                return false;
            }
            else if (NPC.Remix().GreenAI[0] == 1)
            {
                segments = VerletSimulatedSegment.SimpleSimulation(segments, 16, 10, 0.2f);
                segments[0].locked = false;
                NPC.Remix().GreenAI[1]++;
                NPC.rotation += MathF.Sin(NPC.Remix().GreenAI[2] * 34) * 0.1f;
                if (NPC.Remix().GreenAI[1] > 30)
                {
                    NPC.Remix().GreenAI[0] = 2;
                    NPC.netUpdate = true;
                }
                return false;
            }

            segments = VerletSimulatedSegment.SimpleSimulation(segments, 16, 10, 0.2f);
            segments[0].locked = false;
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int u = 0; u < segments.Count - 1; u++)
            {
                if (u == 0)
                    continue;
                VerletSimulatedSegment seg = segments[u];
                float dist = u > 0 ? Vector2.Distance(seg.position, segments[u - 1].position) : 0;
                if (dist <= 2)
                    dist = 2;
                dist += 2;
                float rot = 0f;
                if (u > 0)
                    rot = seg.position.DirectionTo(segments[u - 1].position).ToRotation();
                else
                    rot = 0;
                float scalee = (1 - (u / segments.Count));
                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, seg.position - Main.screenPosition + Vector2.UnitX * 8, new Rectangle(0, 0, (int)dist, 2), Lighting.GetColor(seg.position.ToTileCoordinates()).MultiplyRGB(Color.ForestGreen), rot, TextureAssets.BlackTile.Size() / 2, scalee, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }
            Texture2D tex = TextureAssets.Npc[Type].Value;
            float bloomSize = 0.3f;
            Vector2 drawPos = NPC.Center - screenPos;
            Main.EntitySpriteDraw(CalRemixAsset.BloomTexture.Value, drawPos, null, Color.Yellow * 0.1f, 0, CalRemixAsset.BloomTexture.Size() / 2, bloomSize, 0);
            spriteBatch.Draw(tex, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 20), NPC.scale, 0, 0);
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.Remix().GreenAI[0] == 0)
            {
                NPC.frame.Y = 0;
            }
            else if (NPC.Remix().GreenAI[0] == 1)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 6 == 0 && NPC.frame.Y < frameHeight * 5)
                    NPC.frame.Y += frameHeight;
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 6 == 0)
                    NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 9)
                    NPC.frame.Y = frameHeight * 5;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
