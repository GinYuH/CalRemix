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

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class Stanchor : ModNPC
    {

        public static SoundStyle HitSound = new SoundStyle("CalRemix/Assets/Sounds/StanchorHit");
        public static SoundStyle DeathSound = new SoundStyle("CalRemix/Assets/Sounds/StanchorDeath");
        public static SoundStyle LaunchSound = new SoundStyle("CalRemix/Assets/Sounds/Stanchor");

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

        public bool InHidingSpot => NPC.WithinRange(SpotToHideIn, 8f);

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 90;
            NPC.width = 80;
            NPC.height = 70;
            NPC.defense = 30;
            NPC.lifeMax = 5000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            NPC.noGravity = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<GreatSeaBiome>().Type };
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
                    if (!WorldGen.SolidTile(tile) || CalamityUtils.ParanoidTileRetrieval(x, y + 1).HasTile)
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
            if (target.position.Y > NPC.Bottom.Y && target.Distance(NPC.Center) < 600 && NPC.ai[3] == 0)
            {
                SoundEngine.PlaySound(LaunchSound, NPC.Center);
                NPC.velocity = Vector2.UnitY * 7;
                NPC.ai[3] = 1;
            }
            if (NPC.ai[3] == 1)
            {
                NPC.ai[2]++;

                if (NPC.ai[2] < 30)
                {
                    NPC.velocity.Y *= 1.05f;
                }
                if (NPC.ai[2] == 30 || (SavePosition == default && NPC.ai[2] > 10 && Collision.SolidTiles(NPC.position, NPC.width, NPC.height)))
                {
                    SavePosition = NPC.Center;
                    NPC.velocity = Vector2.Zero;
                }
                if (NPC.ai[2] == 50)
                {
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 2;
                }
            }
            if (NPC.ai[3] == 2)
            {
                NPC.ai[2] += 0.04f;
                float dist = SpotToHideIn.Distance(SavePosition);
                Vector2 dest = SpotToHideIn + Vector2.UnitY.RotatedBy(MathF.Sin(NPC.ai[2])) * dist;
                NPC.velocity = dest - NPC.Center;
                NPC.rotation = NPC.DirectionTo(SpotToHideIn).ToRotation() + MathHelper.PiOver2;

                if (Math.Abs(NPC.velocity.X) < 0.2f && NPC.soundDelay <= 0)
                {
                    SoundEngine.PlaySound(CommonCalamitySounds.SwiftSliceSound, NPC.Center);
                    NPC.soundDelay = 10;
                }
            }
            NPC.soundDelay--;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (SpotToHideIn == default)
                return false;
            spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied);
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D chain = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/StanchorSegment").Value;
            Vector2 bottom = SpotToHideIn;
            Vector2 distToProj = NPC.Center;
            float projRotation = NPC.AngleTo(bottom) - 1.57f;
            bool doIDraw = true;

            while (doIDraw)
            {
                float distance = (bottom - distToProj).Length();
                if (distance < (chain.Height + 1))
                {
                    doIDraw = false;
                }
                else if (!float.IsNaN(distance))
                {
                    Color drawColore = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                    distToProj += NPC.DirectionTo(bottom) * chain.Height;
                    Main.EntitySpriteDraw(chain, distToProj - Main.screenPosition,
                        new Rectangle(0, 0, chain.Width, chain.Height), drawColore, projRotation,
                        Utils.Size(chain) / 2f, 1f, SpriteEffects.None, 0);
                }
            }
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
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
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SparksMech, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SparksMech, hit.HitDirection, -1f, 0, default, 1f);
                }

                for (int i = 0 ; i < 20; i++)
                {
                    Gore.NewGorePerfect(NPC.GetSource_Death(), Vector2.Lerp(NPC.Center, SpotToHideIn, i / 20f), Main.rand.NextVector2Circular(10, 10), Mod.Find<ModGore>("StanchorSegment").Type);
                }
            }
        }
    }
}
