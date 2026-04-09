using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Graphics.Primitives;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.VanillaNPCAIOverrides.Bosses;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Content.Items.Potions;
using CalRemix.Content.Projectiles.Weapons;
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
    public class FlingastWidow : ModNPC
    {
        public bool HasChosenSpotToHideIn => SpotToHideIn != Vector2.Zero;
        public Point TileCoordsToHideIn
        {
            get => SpotToHideIn.ToTileCoordinates();
            set => SpotToHideIn = value.ToWorldCoordinates();
        }
        public Vector2 SpotToHideIn
        {
            get => new Vector2(NPC.ai[3], NPC.ai[1]);
            set
            {
                NPC.ai[3] = value.X;
                NPC.ai[1] = value.Y;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 80;
            NPC.defense = 40;
            NPC.lifeMax = 15000;
            NPC.knockBackResist = 0.1f;
            NPC.value = Item.buyPrice(silver: 10);
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit38 with { Pitch = 0.1f };
            NPC.DeathSound = SoundID.NPCDeath41 with { Pitch = 0.3f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<BigOlBranchesBiome>().Type];
            NPC.behindTiles = true;
        }


        public override void AI()
        {
            Lighting.AddLight(NPC.Center, 2, 2, 1);
            NPC.TargetClosest();
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
                        if (!WorldGen.SolidTile(tile) || (CalamityUtils.ParanoidTileRetrieval(x - 1, y).HasTile && CalamityUtils.ParanoidTileRetrieval(x + 1, y).HasTile))
                            continue;


                        bool faceRight = CalamityUtils.ParanoidTileRetrieval(x - 1, y).HasTile;
                        bool sb = false;

                        int lootry = 0;
                        for (int j = y - 5; j < y + 5; j++)
                        {
                            lootry++;
                            if (CalamityUtils.ParanoidTileRetrieval(x + faceRight.ToDirectionInt(), j).HasTile)
                            {
                                sb = true;
                                break;
                            }
                            if (!CalamityUtils.ParanoidTileRetrieval(x, j).HasTile)
                            {
                                sb = true;
                                break;
                            }
                        }

                        if (sb)
                            continue;

                        // Try again if there's no open water near the tile.
                        Vector2 moveDirection = Vector2.UnitX * faceRight.ToDirectionInt();
                        Vector2 collisionCheckPosition = new Vector2(x * 16f + 8f, y * 16f + 8f) + moveDirection * 16f;
                        float collisionDistance = CalamityUtils.DistanceToTileCollisionHit(collisionCheckPosition, moveDirection, 20) ?? 20;
                        if (collisionDistance <= 10)
                            continue;

                        TileCoordsToHideIn = new Point(x, y);
                        NPC.rotation = MathHelper.PiOver2 * faceRight.ToDirectionInt();
                        break;
                    }

                    // Just die if no spot was suitable.
                    if (tries >= (tot - 1))
                    {
                        NPC.active = false;
                    }

                    NPC.Center = SpotToHideIn;
                    NPC.netUpdate = true;
                }

                // Prevent the tile from being destroyed.
                FixExploitManEaters.ProtectSpot(TileCoordsToHideIn.X, TileCoordsToHideIn.Y);
                #endregion

                if (HasChosenSpotToHideIn)
                {
                    NPC.Center = SpotToHideIn;
                }

                Vector2 center = NPC.Center + Vector2.UnitX * 150 * NPC.rotation.DirectionalSign();
                NPC.TargetClosest();
                if (Main.player[NPC.target].Distance(center) < 50 && NPC.ai[0] <= 0)
                {
                    NPC.ai[2] = NPC.target + 1;
                    NPC.netUpdate = true;
                }

                foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == Type)
                    continue;
                if (n.Distance(center) < 50 && n.velocity.Length() < 10)
                {
                    n.velocity = (center - n.Center) * 0.5f;
                    SoundEngine.PlaySound(BetterSoundID.ItemWhipLash, NPC.Center);
                }
            }

                if (NPC.ai[2] > 0)
                {
                    Player p = Main.player[(int)NPC.ai[2] - 1];
                    if (!p.active || p.dead || p.CCed)
                    {
                        NPC.ai[2] = 0;
                    }
                    else
                    {
                        if (NPC.justHit || (Main.player[NPC.target].controlUseItem && Main.player[NPC.target].HeldItem.shoot <= 0))
                        {
                            NPC.ai[2] = 0;
                            p.velocity = (center - p.Center) * 0.5f;
                            NPC.ai[0] = 120;
                            SoundEngine.PlaySound(BetterSoundID.ItemWhipLash);
                            return;
                        }
                        int maxPullSpeed = 6; 
                        int minDist = 20; 
                        int maxDistOfScale = 1200;
                        p.velocity = Vector2.Lerp(p.velocity, p.velocity + p.SafeDirectionTo(center) * maxPullSpeed, Utils.GetLerpValue(minDist, maxDistOfScale, p.Distance(NPC.Center), true));
                    }
                }
            NPC.ai[0]--;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.ExitShaderRegion();
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Vector2 basePos = NPC.Center + Vector2.UnitX * 10 * NPC.rotation.DirectionalSign() + Vector2.UnitX * MathF.Sin(Main.GlobalTimeWrappedHourly * 10);
            spriteBatch.Draw(tex, basePos - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0);

            for (int i = -4; i <= 4; i++)
            {
                if (i == 0)
                    continue;
                List<Vector2> pts = new();

                Vector2 pos = basePos - Vector2.UnitX * (MathF.Sin(Main.GlobalTimeWrappedHourly * 10 + i) * 2 * (i % 5) - 5);
                Vector2 dir = basePos.DirectionTo(pos);
                Vector2 posWithBuff = pos - Vector2.UnitX * NPC.rotation.DirectionalSign() * 30;
                Vector2 destPos = Vector2.Lerp(posWithBuff, posWithBuff + Vector2.UnitY * Math.Sign(i) * (200 + (i % 3 * 4)), Math.Abs(i) / 4f );

                Vector2 avgPos = pos + (destPos - pos) + Vector2.UnitX * NPC.rotation.DirectionalSign() * 200;

                pts.Add(pos);
                pts.Add(avgPos);
                pts.Add(destPos);

                BezierCurve c = new BezierCurve([.. pts]);

                List<Vector2> legPoints = c.GetPoints(30);

                PrimitiveRenderer.RenderTrail(legPoints, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => MathHelper.Lerp(5, 2, f)), new PrimitiveSettings.VertexColorFunction((float f, Vector2 v) => Lighting.GetColor((v + screenPos).ToTileCoordinates()).MultiplyRGB(Color.Lerp(Color.Brown, Color.Black, f)))));
                PrimitiveRenderer.RenderTrail(legPoints, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => MathHelper.Lerp(4, 1, f)), new PrimitiveSettings.VertexColorFunction((float f, Vector2 v) => Lighting.GetColor((v + screenPos).ToTileCoordinates()).MultiplyRGB(Color.Lerp(Color.Orange, Color.Black, f)))));
            }


            Vector2 sling = basePos + new Vector2(NPC.rotation.DirectionalSign() * 140, 42);
            Vector2 sling2 = basePos + new Vector2(NPC.rotation.DirectionalSign() * 140, -47);
            Vector2 midPos = Vector2.Zero;
            if (NPC.HasPlayerTarget && NPC.ai[2] > 0)
            {
                midPos = Main.player[NPC.target].Center;
            }
            List<Vector2> slingPos = new();
            int totPoints = 30;
            if (midPos == Vector2.Zero)
            {
                for (int i = 0; i < totPoints; i++)
                {
                    slingPos.Add(Vector2.Lerp(sling, sling2, i / (float)(totPoints - 1)));
                }
            }
            else
            {
                for (int i = 0; i < totPoints / 2; i++)
                {
                    slingPos.Add(Vector2.Lerp(sling, midPos, i / (float)(totPoints / 2 - 1)));
                }
                for (int i = 0; i < totPoints / 2; i++)
                {
                    slingPos.Add(Vector2.Lerp(midPos, sling2, i / (float)(totPoints / 2 - 1)));
                }
            }

            PrimitiveRenderer.RenderTrail(slingPos, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => 2), new PrimitiveSettings.VertexColorFunction((float f, Vector2 v) => Lighting.GetColor((v + screenPos).ToTileCoordinates()).MultiplyRGB(Color.White))));


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
