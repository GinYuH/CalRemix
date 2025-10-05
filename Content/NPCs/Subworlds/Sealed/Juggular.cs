using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Hostile;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Sounds;
using CalamityMod.Particles;
using Terraria.DataStructures;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class Juggular : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public Vector2 Squish
        {
            get => new Vector2(NPC.localAI[0], NPC.localAI[1]);
            set
            {
                NPC.localAI[0] = value.X;
                NPC.localAI[1] = value.Y;
            }
        }

        public Vector2 Anchor
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[3]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[3] = value.Y;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 176;
            NPC.height = 102;
            NPC.lifeMax = 30000;
            NPC.damage = 200;
            NPC.defense = 26;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.value = 0;
            NPC.boss = true;
            NPC.chaseable = false;
            NPC.dontTakeDamage = true;
            NPC.alpha = 255;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Squish = Vector2.One;
        }

        public override void AI()
        {
            Vector2 squash = new Vector2(1.2f, 0.8f);
            Vector2 stretch = new Vector2(0.6f, 1.4f);
            NPC.TargetClosest();
            switch (State)
            {
                case 0:
                    {
                        if (ExtraOne == 0)
                        {
                            if (Timer == 40)
                            {
                                ExtraOne = 1;
                                NPC.alpha = 0;
                            }
                            Squish = stretch;
                        }
                        else if (ExtraOne == 1)
                        {
                            if (NPC.velocity.Y < 22f)
                                NPC.velocity.Y += 0.1f;
                            if (NPC.Bottom.Y >= Target.Bottom.Y && (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.Top.Y >= Target.Bottom.Y + 400))
                            {
                                Timer = 0;
                                NPC.velocity.Y = 0;
                                ExtraOne = 2;
                                Squish = squash;
                            }
                        }
                        else if (ExtraOne == 2)
                        {
                            Squish = Vector2.Lerp(Squish, Vector2.One, 0.2f);
                            if (Timer >= 50)
                            {
                                ChangePhase(1);
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        int jumpSquash = 20;
                        int jumpActual = jumpSquash + 20;
                        int invisTime = jumpActual + 20;
                        int telegraphTime = invisTime + 10;
                        int waitTime = 90;
                        if (ExtraOne == 0)
                        {
                            if (Timer < jumpSquash)
                            {
                                Squish = Vector2.Lerp(Vector2.One, squash, Utils.GetLerpValue(0, jumpSquash, Timer, true));
                            }
                            else if (Timer < jumpActual)
                            {
                                Squish = Vector2.Lerp(squash, stretch, Utils.GetLerpValue(jumpSquash, jumpSquash + 5, Timer, true));
                                NPC.velocity.Y = -70;
                            }
                            else if (Timer == jumpActual)
                            {
                                NPC.velocity.Y = 0;
                            }
                            else if (Timer < invisTime)
                            {
                                NPC.Center = new Vector2(Target.Center.X, Target.Center.Y - 2000);
                                Squish = Vector2.One;
                            }
                            else if (Timer < telegraphTime)
                            {
                                NPC.velocity = Vector2.Zero;
                            }
                            else
                            {
                                Squish = stretch;
                                if (NPC.velocity.Y < 50f)
                                    NPC.velocity.Y += 2f;
                                if (NPC.Bottom.Y >= Target.Bottom.Y && (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.Top.Y >= Target.Bottom.Y + 400))
                                {
                                    Timer = 0;
                                    NPC.velocity.Y = 0;
                                    ExtraOne = 1;
                                    Squish = squash;
                                    int projAmt = 10;
                                    SoundEngine.PlaySound(BetterSoundID.ItemInfernoExplosion, Target.Center);
                                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        for (int i = 0; i < projAmt; i++)
                                        {
                                            Vector2 spawnLoc = new Vector2(NPC.Center.X + MathHelper.Lerp(-2000, 2000, (i + 1) / (float)projAmt), NPC.Center.Y - 1000);
                                            Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnLoc, spawnLoc.DirectionTo(Target.Center) * 20, ProjectileID.FallingStar, CalRemixHelper.ProjectileDamage(240, 340), 1);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Squish = Vector2.Lerp(Squish, Vector2.One, 0.2f);
                            if (Timer >= waitTime)
                            {
                                ChangePhase(2);
                            }
                        }

                    }
                    break;
                case 2:
                    {
                        int teleportWait = 40;
                        int starRounds = 5;
                        int starRate = 50;
                        int endStars = starRounds * starRate + teleportWait;
                        int endAttack = endStars + 60;
                        if (Timer == 1)
                        {
                            Teleport(Target.Center + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 300);
                        }
                        else if (Timer > teleportWait && Timer < endStars)
                        {
                            if (Timer % starRate == 0)
                            {
                                int projAmt = 22;
                                for (int i = 0; i < projAmt; i++)
                                {
                                    if (i % 5 == Main.rand.Next(0, 4))
                                    {
                                        continue;
                                    }
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Vector2 spawnLoc = NPC.Center + Vector2.UnitX.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)projAmt)) * 1800;
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnLoc, spawnLoc.DirectionTo(NPC.Center) * 30, ProjectileID.FallingStar, CalRemixHelper.ProjectileDamage(200, 300), 1);
                                    }
                                }
                            }
                        }
                        else if (Timer >= endAttack)
                        {
                            ChangePhase(2);
                        }
                    }
                    break;
            }
            Timer++;
        }

        public void Teleport(Vector2 to)
        {
            NPC.Center = to;
            // Add some vfx
        }

        public void ChangePhase(int phase)
        {
            State = phase;
            Timer = 0;
            ExtraOne = 0;
            ExtraTwo = 0;
        }

        public override void FindFrame(int frameHeight)
        {
            if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.velocity.Y == 0)
            {
                if (NPC.frameCounter++ % 6 == 0)
                {
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= frameHeight * 4)
                        NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight * 3;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[Type] / 2), NPC.scale * Squish, 0, 0f);
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
