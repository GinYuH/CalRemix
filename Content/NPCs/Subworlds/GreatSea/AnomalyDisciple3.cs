using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.Animations;
using CalRemix.Core.World;
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;
using System;
using CalamityMod.Graphics.Primitives;
using CalRemix.Core.Biomes;
using CalamityMod.Sounds;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.World;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class AnomalyDisciple3 : ModNPC
    {

        public static SoundStyle OrbuleSound = new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyOrbule");
        public static SoundStyle BurstSound = new SoundStyle("CalRemix/Assets/Sounds/Anomaly/AnomalyOrbBurst");

        public bool HasChosenSpotToHideIn => SpotToHideIn != Vector2.Zero;
        public Point TileCoordsToHideIn
        {
            get => SpotToHideIn.ToTileCoordinates();
            set => SpotToHideIn = value.ToWorldCoordinates();
        }
        public Vector2 SpotToHideIn
        {
            get => new Vector2(NPC.ai[0], NPC.ai[1]);
            set
            {
                NPC.ai[0] = value.X;
                NPC.ai[1] = value.Y;
            }
        }
        public Vector2 SavePosition
        {
            get => new Vector2(NPC.localAI[0], NPC.localAI[1]);
            set
            {
                NPC.localAI[0] = value.X;
                NPC.localAI[1] = value.Y;
            }
        }

        public ref float Timer => ref NPC.ai[2];

        public bool InHidingSpot => NPC.WithinRange(SpotToHideIn, 8f);

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 110;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 60;
            NPC.lifeMax = 20000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            NPC.noGravity = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
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
                    if (!WorldGen.SolidTile(tile) || CalamityUtils.ParanoidTileRetrieval(x, y - 1).HasTile)
                        continue;

                    // Try again if there's no open water near the tile.
                    Vector2 moveDirection = -Vector2.UnitY;
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
                    return;
                }

                NPC.Center = SpotToHideIn;
                NPC.netUpdate = true;
            }

            // Prevent the tile from being destroyed.
            FixExploitManEaters.ProtectSpot(TileCoordsToHideIn.X, TileCoordsToHideIn.Y);
            #endregion
            NPC.TargetClosest(false);
            Player target = Main.player[NPC.target];
            if (NPC.ai[3] == 0)
            {
                if (Timer == 0)
                {
                    NPC.Center = NPC.Center - Main.rand.Next(200, 300) * Vector2.UnitY;
                }
                Timer++;
                NPC.velocity.X = 0;
                NPC.velocity.Y = MathF.Sin(Timer * 0.02f) * 0.2f;
                if (target.Distance(NPC.Center) < 300 || NPC.justHit)
                {
                    Timer = 0;
                    NPC.ai[3] = 1;
                    NPC.velocity = Vector2.Zero;
                }
            }
            if (NPC.ai[3] == 1 || NPC.ai[3] == 2)
            {
                if (NPC.Distance(target.Center) > 2000)
                {
                    Timer = 0;
                    NPC.ai[3] = 0;
                    NPC.Calamity().newAI[0] = 0;
                    SavePosition = Vector2.Zero;
                    NPC.velocity = Vector2.Zero;
                    return;
                }
                int changeRate = 120;
                if (Timer % changeRate == 0)
                {
                    SavePosition = Vector2.Zero;
                    while (SavePosition.Distance(SpotToHideIn) > 300)
                    {
                        SavePosition = NPC.Center + Main.rand.NextVector2CircularEdge(200, 200);
                    }
                }
                if (SavePosition != default)
                {
                    if (NPC.Distance(SavePosition) > 20)
                    {
                        NPC.velocity = NPC.DirectionTo(SavePosition) * 4;
                        NPC.Calamity().newAI[0] = 0;
                    }
                    else
                    {
                        NPC.velocity *= 0.9f;
                        if (NPC.velocity.Length() < 1)
                        {
                            if (NPC.ai[3] == 1)
                            {
                                if (Timer % 10 == 0)
                                {
                                    SoundEngine.PlaySound(OrbuleSound, NPC.Center);
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(target.Center), ModContent.ProjectileType<AnomalyOrbule>(), 30, 1f);
                                        Main.projectile[p].scale = 1.4f;
                                    }
                                    NPC.Calamity().newAI[0]++;
                                }
                                if (Main.expertMode)
                                {
                                    if (NPC.Calamity().newAI[0] == 6)
                                    {
                                        SoundEngine.PlaySound(BurstSound, NPC.Center);
                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                        {
                                            int amt = CalamityWorld.death ? 5 : CalamityWorld.revenge ? 3 : 2;
                                            for (int i = 0; i < amt; i++)
                                            {
                                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(target.Center).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / ((float)amt - 1))), ModContent.ProjectileType<AnomalyOrbule>(), 30, 1f, ai0: (i != 1 || amt != 3) ? 1 : 0, ai2: target.whoAmI);
                                                Main.projectile[p].scale = 1.4f;
                                            }
                                        }
                                        NPC.Calamity().newAI[0] = 0;
                                    }
                                }
                            }
                            else if (NPC.ai[3] == 2)
                            {
                                if (NPC.Calamity().newAI[0] == 0)
                                {
                                    SoundEngine.PlaySound(BurstSound, NPC.Center);
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        int amt = Main.rand.Next(8, 13);
                                        for (int i = 0; i < amt; i++)
                                        {
                                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(target.Center).RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)amt)), ModContent.ProjectileType<AnomalyOrbule>(), 30, 1f, ai0: 1, ai2: target.whoAmI);
                                            Main.projectile[p].scale = 1.4f;
                                        }
                                    }
                                    NPC.Calamity().newAI[0] = 1;
                                }
                            }
                        }
                    }
                }
                if (Timer > changeRate * 4 + 40)
                {
                    NPC.ai[3] = (NPC.ai[3] == 2) ? 1 : 2;
                    Timer = 0;
                    NPC.Calamity().newAI[0] = 0;
                    NPC.velocity = Vector2.Zero;
                }
                Timer++;
            }
            if (NPC.ai[3] == 2)
            {
            }
            int maxDist = 500;
            if (NPC.Distance(SpotToHideIn) > maxDist)
            {
                NPC.Center = SpotToHideIn + SpotToHideIn.DirectionTo(NPC.Center) * maxDist;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                SpotToHideIn = NPC.Center + Vector2.UnitY * 200;
            }
            if (SpotToHideIn == default)
                return false;
            spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied);
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D chain = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/AnomalyDiscipleChain").Value;
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
            Vector2 bottom = SpotToHideIn;

            int chainAmt = (int)(NPC.Distance(SpotToHideIn) / 32f / NPC.scale);

            for (int i = 0; i < chainAmt; i++)
            {
                Vector2 pos = Vector2.Lerp(NPC.Center, SpotToHideIn, i / (float)chainAmt);
                spriteBatch.EnterShaderRegion(BlendState.Additive);
                Main.EntitySpriteDraw(bloom, pos - screenPos, null, Color.LightSeaGreen, 0, bloom.Size() / 2, 0.2f * NPC.scale + MathF.Sin(Main.GlobalTimeWrappedHourly + 2 + NPC.whoAmI) * 0.02f, 0);
                spriteBatch.ExitShaderRegion();
                Main.EntitySpriteDraw(chain, pos - screenPos, null, Color.White, 0, chain.Size() / 2, NPC.scale, 0);
            }

            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 circularOffset = Vector2.UnitY.RotatedBy(Main.GlobalTimeWrappedHourly * 5) * 2;
            for (int i = 0; i < 8; i++)
            {
                Vector2 offset = Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / 8f)) * 4;

                spriteBatch.Draw(tex, NPC.Center - screenPos + offset + circularOffset, NPC.frame, Color.LightSeaGreen with { A = 40 }, NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, fx, 0);
            }
            spriteBatch.Draw(tex, NPC.Center - screenPos + circularOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / 2), NPC.scale, fx, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemEmerald, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemEmerald, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
