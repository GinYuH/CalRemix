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
using Terraria.Graphics.Shaders;

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

        public int TelegraphTimer = 0;

        public int TelegraphMaxTime = 0;

        public const int TelegraphTime = 30;

        public Vector2 portalLocation = new();
        public Vector2 portalLocation2 = new();

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.MustAlwaysDraw[Type] = true;
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
                            TelegraphMaxTime = 40;
                            if (TelegraphTimer <= 0)
                            {
                                TelegraphTimer = TelegraphMaxTime;
                                SoundEngine.PlaySound(BetterSoundID.ItemLaserMachinegun with { Pitch = 1 }, Target.Center);
                            }
                            if (Timer == 40)
                            {
                                TelegraphTimer = 0;
                                TelegraphMaxTime = 0;
                                ExtraOne = 1;
                                NPC.alpha = 0;
                            }
                            Squish = stretch;
                        }
                        else if (ExtraOne == 1)
                        {
                            if (NPC.velocity.Y < 52f)
                                NPC.velocity.Y += 0.5f;
                            if (NPC.Bottom.Y >= Target.Bottom.Y && (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.Top.Y >= Target.Bottom.Y + 400))
                            {
                                Timer = 0;
                                NPC.velocity.Y = 0;
                                ExtraOne = 2;
                                Squish = squash;
                                SoundEngine.PlaySound(BetterSoundID.ItemInfernoExplosion, Target.Center);
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10;
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
                        NPC.alpha = 0;
                        int jumpSquash = 20;
                        int jumpActual = jumpSquash + 20;
                        int invisTime = jumpActual + 20;
                        int telegraphTime = invisTime + 60;
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
                                NPC.Center = new Vector2(Target.Center.X, Target.Center.Y - 1000);
                                Squish = Vector2.One;
                            }
                            else if (Timer < telegraphTime)
                            {
                                NPC.velocity = Vector2.Zero;
                                if (Timer == invisTime)
                                {
                                    SoundEngine.PlaySound(BetterSoundID.ItemLaserMachinegun with { Pitch = 1 }, Target.Center);
                                    TelegraphMaxTime = telegraphTime - invisTime;
                                    if (TelegraphTimer <= 0)
                                        TelegraphTimer = TelegraphMaxTime;
                                }
                                NPC.Center = new Vector2(NPC.Center.X, Target.Center.Y - 1000);
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
                                            Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnLoc, spawnLoc.DirectionTo(Target.Center) * 20, ModContent.ProjectileType<Juggustar>(), CalRemixHelper.ProjectileDamage(240, 340), 1);
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
                        int teleportWait = 60;
                        int starRounds = 5;
                        int starRate = 60;
                        int endStars = starRounds * starRate + teleportWait;
                        int endAttack = endStars + 60;
                        if (Timer == 1)
                        {
                            Teleport(Target.Center + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 300);
                        }
                        else if (Timer > teleportWait && Timer < endStars)
                        {
                            float localTimer = Timer % starRate;
                            if (localTimer == 0)
                            {
                                int projAmt = 34;
                                for (int i = 0; i < projAmt; i++)
                                {
                                    if (i % 5 == Main.rand.Next(0, 4))
                                    {
                                        continue;
                                    }
                                    SoundEngine.PlaySound(BetterSoundID.ItemStarWrath with { Pitch = 0.5f }, NPC.Center);
                                    float offset = Main.rand.NextFloat(MathHelper.TwoPi);
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Vector2 spawnLoc = NPC.Center + Vector2.UnitX.RotatedBy(offset + MathHelper.Lerp(0, MathHelper.TwoPi, i / (float)projAmt)) * 1200;
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnLoc, Vector2.Zero, ModContent.ProjectileType<Juggustar>(), CalRemixHelper.ProjectileDamage(200, 300), 1, ai0: 1);
                                    }
                                }
                            }
                            if (localTimer < starRate * 0.5f)
                            {
                                Squish = Vector2.Lerp(Squish, squash, 0.1f);
                            }
                            else
                            {
                                Squish = Vector2.Lerp(Squish, stretch, 0.1f);
                            }
                        }
                        else if (Timer < endAttack)
                        {
                            Squish = Vector2.Lerp(Squish, Vector2.One, 0.1f);
                        }
                        else if (Timer >= endAttack)
                        {
                            ChangePhase(3);
                        }
                    }
                    break;
                case 3:
                    {
                        if (ExtraOne == 0)
                        {
                            if (Timer == 1)
                            {
                                NPC.velocity = Vector2.Zero;
                            }
                            else if (Timer == 20)
                            {
                                NPC.alpha = 255;
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<JuggularPortal>(), 0, 0, ai0: 1, ai1: 40);
                            }
                            else if (Timer == 50)
                            {
                                Timer = 0;
                                ExtraOne = 1;
                            }
                        }
                        else if (ExtraOne == 1)
                        {
                            int portalTelTime = 50;
                            int dashTime = portalTelTime + 4;
                            int stopTime = dashTime + 3;
                            int nextTime = stopTime + 40;
                            int tpAmount = 4;
                            int totalLength = tpAmount * nextTime;
                            float localTimer = Timer % nextTime;
                            bool notFinalCycle = Timer < nextTime * (tpAmount - 1);
                            if (localTimer == 1)
                            {
                                NPC.Center = Target.Center + Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.Next(300, 600);
                                portalLocation = NPC.Center;
                                portalLocation2 = Target.Center + NPC.DirectionTo(Target.Center) * NPC.Distance(Target.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), portalLocation, Vector2.Zero, ModContent.ProjectileType<JuggularPortal>(), 0, 0, ai1: 80);
                                SoundEngine.PlaySound(BetterSoundID.ItemNebulaArcanum with { Pitch = -0.4f}, Target.Center);
                            }
                            else if (localTimer == portalTelTime)
                            {
                                Squish = stretch;
                                NPC.alpha = 0;
                                NPC.velocity = NPC.DirectionTo(Target.Center) * 40;
                                if (notFinalCycle)
                                    SoundEngine.PlaySound(BetterSoundID.ItemRainbowGun with { Pitch = 0.5f }, Target.Center);
                            }
                            else if (localTimer < stopTime)
                            {
                                float completion = Utils.GetLerpValue(portalTelTime, stopTime, localTimer, true);
                                Vector2 newPos = Vector2.Lerp(portalLocation, portalLocation2, completion);

                                if (completion > 0.3f && ExtraTwo == 0)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient && notFinalCycle)
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), portalLocation2, Vector2.Zero, ModContent.ProjectileType<JuggularPortal>(), 0, 0, ai0: 1, ai1: 60);
                                    ExtraTwo = 1;
                                }

                                NPC.velocity = newPos - NPC.Center;
                                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                            }
                            else if (localTimer > stopTime && localTimer < nextTime - 1)
                            {
                                if (localTimer == stopTime + 1 && notFinalCycle)
                                {
                                    /*int projCount = 3;
                                    float spread = MathHelper.PiOver4;
                                    float halfSpread = spread * 0.5f;
                                    for (int i = 0; i < projCount; i++)
                                    {
                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                        {
                                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, portalLocation2.DirectionTo(portalLocation).RotatedBy(MathHelper.Lerp(-halfSpread, halfSpread, i / (float)(projCount - 1))) * 20, ModContent.ProjectileType<Juggustar>(), CalRemixHelper.ProjectileDamage(140, 250), 1);
                                            Main.projectile[p].extraUpdates = 2;
                                        }
                                    }*/
                                }
                                portalLocation = Vector2.Zero;
                                if (notFinalCycle)
                                    NPC.alpha = 255;
                                else
                                {
                                    NPC.rotation = 0;
                                    Squish = Vector2.One;
                                }
                                NPC.velocity = Vector2.Zero;
                            }
                            else if (localTimer >= nextTime - 1)
                            {
                                portalLocation2 = Vector2.Zero;
                                ExtraTwo = 0;
                            }
                            if (Timer > totalLength - 2)
                            {
                                ExtraOne = 2;
                                NPC.alpha = 0;
                                NPC.velocity = Vector2.Zero;
                                Squish = Vector2.One;
                                Timer = 0;
                                NPC.rotation = 0;
                            }
                        } 
                        else
                        {
                            if (Timer > 60)
                                ChangePhase(1);
                        }
                    }
                    break;
            }
            Timer++;
            TelegraphTimer--;
        }

        public void Teleport(Vector2 to)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<JuggularPortal>(), 0, 0, ai0: 1, ai1: 40);
            NPC.Center = to;
            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<JuggularPortal>(), 0, 0, ai0: 0, ai1: 40);
        }

        public void ChangePhase(int phase)
        {
            State = phase;
            Timer = 0;
            ExtraOne = 0;
            ExtraTwo = 0;
            NPC.rotation = 0;
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
            Texture2D line = ModContent.Request<Texture2D>("CalamityMod/Particles/DrainLineBloom").Value;

            spriteBatch.EnterShaderRegion(BlendState.Additive);

            if (TelegraphTimer > 0)
            {
                float lineHeight = 3000;
                float lineDist = 60 + MathF.Cos(Main.GlobalTimeWrappedHourly * 10) * 5;
                float opacity = 1;
                int teleFadeIn = (int)(TelegraphMaxTime * 0.7f);
                int teleFadeOut = (int)(TelegraphMaxTime * 0.2f);
                if (TelegraphTimer >= teleFadeIn)
                {
                    opacity = Utils.GetLerpValue(TelegraphMaxTime, teleFadeIn, TelegraphTimer, true);
                }
                else if (TelegraphTimer <= teleFadeOut)
                {
                    opacity = Utils.GetLerpValue(0, teleFadeOut, TelegraphTimer, true);
                }
                opacity += MathF.Sin(Main.GlobalTimeWrappedHourly * 22) * 0.2f;
                for (int i = -1; i < 2; i += 2)
                {
                    spriteBatch.Draw(line, NPC.Center + Vector2.UnitX * lineDist * i - screenPos, null, Color.Orange * opacity, 0, new Vector2(line.Width / 2, 0), new Vector2(1, lineHeight / (float)line.Height), 0, 0);
                }
                Texture2D pixel = TextureAssets.MagicPixel.Value;
                Rectangle pixelRect = new Rectangle(0, 0, (int)lineDist * 2, (int)lineHeight);
                spriteBatch.Draw(pixel, NPC.Center - screenPos, pixelRect, Color.Orange * opacity * 0.3f, 0, new Vector2(pixelRect.Width / 2, 0), 1, 0, 0);
            }
            spriteBatch.ExitShaderRegion();

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[Type] / 2), NPC.scale * Squish, 0, 0f);

            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int associatedNPCType = ModContent.NPCType<MonorianGastropodAscended>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
