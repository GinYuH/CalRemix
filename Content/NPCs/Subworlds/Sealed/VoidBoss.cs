using CalRemix.Core.Biomes;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Items.Materials;
using CalamityMod;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalRemix.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.Core.World;
using Terraria.Audio;
using CalamityMod.CalPlayer;
using System;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Tiles;
using CalamityMod.World;
using CalamityMod.Sounds;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    //[AutoloadBossHead]
    public class VoidBoss : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[0], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[0] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }

        public enum PhaseType
        {
            SpawnAnimation = 0,
            Idle = 1,
            Dashes = 2,
            Checkers = 3,
            QuadLaser = 4,
            AttackFour = 5
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }

        public static int VoidIDX = -1;

        public static float CinematicTime => 360;

        public static float AreaDuration => 60;

        public static float AreaChargeUp => 60;

        public static float AreaFade => 20;

        public static float FullAreaTime => AreaDuration + AreaChargeUp + AreaFade;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public static Vector2 texOffset = new Vector2();

        public override void Load()
        {
            On_Main.DoDraw += DrawVoidIntro;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 100;
            NPC.height = 100;
            NPC.lifeMax = 100000;
            NPC.damage = 240;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 20);
            NPC.boss = true;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (target == null || !target.active || target.dead)
            {
                NPC.active = false;
                return;
            }
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnimation:
                    {
                        NPC.Center = target.Center - Vector2.UnitY * 300;
                        if (Timer > CinematicTime + 80)
                        {
                            TeleportParticles();
                            NPC.alpha = 0;
                            NPC.dontTakeDamage = false;
                            ChangePhase(PhaseType.Dashes);
                        }
                    }
                    break;
                case PhaseType.Idle:
                    {

                    }
                    break;
                case PhaseType.Dashes:
                    {
                        float startDash = 30;
                        float dashLength = startDash + 30;
                        float localTimer = Timer % dashLength;
                        float projRate = 1;
                        float dashAmt = 8;

                        if (localTimer == 1)
                        {
                            NPC.velocity = Vector2.Zero;
                            TeleportParticles(50, 0.6f);
                            int direction = Main.rand.NextBool().ToDirectionInt();
                            if (Math.Abs(target.velocity.X) > 5)
                                direction = target.velocity.X.DirectionalSign();
                            NPC.Center = target.Center + new Vector2(Main.rand.Next(700, 800) * direction, Main.rand.NextFloat(270, 380) * Main.rand.NextBool().ToDirectionInt());
                            TeleportParticles(50, 0.6f);
                            SavePosition = NPC.Center - target.Center;
                            
                        }
                        else if (localTimer < startDash)
                        {
                            NPC.Center = target.Center + SavePosition;
                        }
                        else if (localTimer == startDash)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemEmpressofLightEverlastingRainbow with { MaxInstances = 0, Pitch = 0.9f }, target.Center);
                            if (Main.rand.NextBool())
                            {
                                NPC.velocity.X = 50 * NPC.DirectionTo(target.Center).X.DirectionalSign();
                            }
                            else
                            {
                                NPC.velocity.Y = 50 * NPC.DirectionTo(target.Center).Y.DirectionalSign();
                            }
                        }
                        else if (localTimer < dashLength)
                        {
                            if (localTimer % projRate == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(2, 5), ModContent.ProjectileType<PinkSquareHostile>(), CalRemixHelper.ProjectileDamage(210, 320), 1);
                                }
                            }
                        }
                        if (Timer > dashLength * dashAmt - 2)
                        {
                            ChangePhase(PhaseType.Checkers);
                            int square = ModContent.ProjectileType<PinkSquareHostile>();
                            foreach (Projectile p in Main.ActiveProjectiles)
                            {
                                if (p.type == square)
                                {
                                    p.timeLeft = Main.rand.Next(40, 60);
                                }
                            }
                        }
                    }
                    break;
                case PhaseType.Checkers:
                    {
                        float spawnProjs = 30;
                        float localTimer = Timer % FullAreaTime + spawnProjs;
                        float cycles = (FullAreaTime + spawnProjs) * 3;
                        NPC.velocity = (target.Center + Vector2.UnitY.RotatedBy(Timer * 0.02f) * 500) - NPC.Center;
                        bool skip = false;
                        if (localTimer == spawnProjs)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemManaCrystal with { Pitch = 1 }, target.Center);
                            int squareAmt = 13;
                            int squareSpace = CalamityWorld.death ? 150 : CalamityWorld.revenge ? 200 : 300;
                            int fullArea = squareSpace * squareAmt;
                            Vector2 randomOffset = Main.rand.NextVector2Circular(200, 200);
                            for (int i = 0; i < squareAmt; i++)
                            {
                                for (int j = 0; j < squareAmt; j++)
                                {
                                    bool rng = CalamityWorld.revenge ? NPC.life < (int)(NPC.lifeMax * 0.5f) && Main.rand.NextBool(5) : true;
                                    if (!skip || rng)
                                    {
                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                        {
                                            Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center + randomOffset + new Vector2(MathHelper.Lerp(-fullArea / 2f + squareSpace / 2f, fullArea / 2f - squareSpace / 2f, i / (float)(squareAmt - 1)), MathHelper.Lerp(-fullArea / 2f + squareSpace / 2f, fullArea / 2f - squareSpace / 2f, j / (float)(squareAmt - 1))), Vector2.Zero, ModContent.ProjectileType<PinkSquareArea>(), CalRemixHelper.ProjectileDamage(280, 420), 1, ai0: squareSpace);
                                        }
                                    }
                                    skip = !skip;
                                }
                            }
                        }
                        if (localTimer == AreaChargeUp + spawnProjs && Timer > spawnProjs + AreaChargeUp)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemTerraBeam with { Pitch = 2 }, target.Center);
                        }
                        if (Timer > cycles - 2)
                        {
                            ChangePhase(PhaseType.QuadLaser);
                        }
                    }
                    break;
                case PhaseType.QuadLaser:
                    {
                        float anticipation = Main.expertMode ? 30 : 40;
                        float localTimer = Timer % anticipation;
                        float projPhaseLength = anticipation * 12;
                        float pause = projPhaseLength + 120;

                        if (localTimer == 0 && Timer < projPhaseLength)
                        {
                            NPC.velocity = Vector2.Zero;
                            TeleportParticles(50, 0.6f);
                            int direction = Main.rand.NextBool().ToDirectionInt();
                            int directionY = Main.rand.NextBool().ToDirectionInt();
                            if (Math.Abs(target.velocity.X) > 5)
                                direction = target.velocity.X.DirectionalSign();
                            if (Math.Abs(target.velocity.Y) > 5)
                                direction = target.velocity.Y.DirectionalSign();
                            NPC.Center = target.Center + new Vector2(Main.rand.Next(600, 1000) * direction, Main.rand.NextFloat(400, 680) * directionY);
                            TeleportParticles(50, 0.6f);
                            SavePosition = NPC.Center - target.Center;
                            SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound with { Pitch = 2 }, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(target.Center) * 4, ModContent.ProjectileType<PinkSquareHoming>(), CalRemixHelper.ProjectileDamage(240, 420), 1, ai0: target.whoAmI);
                            }
                        }
                        if (Timer > pause)
                        {
                            ChangePhase(PhaseType.AttackFour);
                            foreach (Projectile p in Main.ActiveProjectiles)
                            {
                                if (p.type == ModContent.ProjectileType<PinkSquareHoming>())
                                {
                                    p.ai[1] = 300;
                                }
                            }
                        }
                    }
                    break;
                case PhaseType.AttackFour:
                    {
                    }
                    break;
            }
            PassiveStuff();
            if (CurrentPhase != PhaseType.SpawnAnimation)
                SpawnParticles();
        }

        public void ChangePhase(PhaseType phase)
        {
            NPC.velocity = Vector2.Zero;
            NPC.rotation = 0;
            CurrentPhase = phase;
            Timer = 0;
            ExtraOne = 0;
            ExtraTwo = 0;
            NPC.netUpdate = true;
        }

        public void TeleportParticles(int amt = 110, float speedMod = 1f)
        {
            for (int i = 0; i < amt; i++)
            {
                VoidMetaball.SpawnParticle(NPC.Center, Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat(16, 26) * speedMod, Main.rand.NextFloat(50, 130));
            }
        }

        public void SpawnParticles()
        {
            int iterationAmt = 5;
            for (int i = 0; i < iterationAmt; i++)
            {
                float comp = i / (float)(iterationAmt - 1);
                VoidMetaball.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 180, comp) + Main.rand.NextVector2Circular(40, 40), Main.rand.NextVector2Circular(2, 2), Main.rand.NextFloat(100, 150) * MathHelper.Lerp(1, 0.10f, comp));
                if (Main.rand.NextBool(10))
                {

                    VoidMetaball.SpawnParticle(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * MathHelper.Lerp(0, 180, comp) + Main.rand.NextVector2Circular(40, 40), Main.rand.NextVector2Circular(8, 8), Main.rand.NextFloat(10, 30), NPC.whoAmI);
                }
            }
        }

        public void PassiveStuff()
        {
            VoidIDX = NPC.whoAmI;
            if (NPC.velocity.Length() > 1)
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() + MathHelper.PiOver2, 0.2f);
            else
                NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);

            if (NPC.frameCounter++ % 8 == 0)
            {
                texOffset = Main.rand.NextVector2Circular(100, 100);
            }

            foreach (Player p in Main.ActivePlayers)
            {
                if (p.TryGetModPlayer(out CalamityPlayer calp))
                {
                    calp.infiniteFlight = true;
                }
            }

            NPC.localAI[0]++;
            if (NPC.localAI[0] > 90)
                Timer++;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<VoidSingularity>(), 1, 5, 13);
            npcLoot.Add(ModContent.ItemType<VoidInfusedStone>(), 1, 150, 250);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;

            Vector2 offset = Vector2.Zero;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, Color.Red * NPC.Opacity, 0, tex.Size() / 2, NPC.scale, 0, 0);

            return false;
        }

        private static void DrawVoidIntro(On_Main.orig_DoDraw orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);
            Texture2D drainLine = ModContent.Request<Texture2D>("CalamityMod/Particles/TitaniumRailgunShellGlow").Value;
            if (!Main.gameMenu)
            {
                if (VoidIDX != -1)
                {
                    NPC voi = Main.npc[VoidIDX];
                    if (voi.type == ModContent.NPCType<VoidBoss>())
                    {
                        if (voi.active && voi.life > 0 && voi.ModNPC != null)
                        {
                            VoidBoss voidboss = voi.ModNPC as VoidBoss;
                            if (voidboss.CurrentPhase == PhaseType.SpawnAnimation && voidboss.Timer <= CinematicTime && voidboss.Timer >= 1)
                            {
                                Main.spriteBatch.Begin();

                                string text = CalRemixHelper.LocalText("StatusText.VoidSummon").Value;
                                float boxOpacity = 0;

                                float fullDuration = CinematicTime;
                                float currentTime = voidboss.Timer;
                                float shutterLength = fullDuration * 0.07f;
                                float shutterDelay = fullDuration * 0.03f;
                                float fullDelay = (shutterLength + shutterDelay) * 2 + fullDuration * 0.05f;
                                float fadeIn = fullDelay + fullDuration * 0.05f;

                                float shutterOpacity = 0.8f;
                                float mainOpacity = 0.6f;

                                float shutterLocalTimer = currentTime % (shutterLength + shutterDelay);

                                float screenOpacity = 0;

                                if (currentTime < (shutterLength + shutterDelay) * 2)
                                {
                                    screenOpacity = shutterOpacity * Utils.GetLerpValue(shutterLength, 0, shutterLocalTimer, true);
                                    text = "";
                                }
                                else if (currentTime > fullDelay && currentTime < fadeIn)
                                {
                                    screenOpacity = mainOpacity * Utils.GetLerpValue(fullDelay, fadeIn, currentTime, true);
                                    boxOpacity = mainOpacity * Utils.GetLerpValue(fullDelay, fadeIn, currentTime, true);
                                }
                                else if (currentTime >= fadeIn && currentTime < CinematicTime - fullDuration * 0.05f)
                                {
                                    screenOpacity = mainOpacity;
                                    boxOpacity = mainOpacity;
                                }
                                else if (currentTime >= CinematicTime - fullDuration * 0.05f)
                                {
                                    screenOpacity = mainOpacity * Utils.GetLerpValue(CinematicTime, CinematicTime - fullDuration * 0.05f, currentTime, true);
                                    boxOpacity = mainOpacity * Utils.GetLerpValue(CinematicTime, CinematicTime - fullDuration * 0.05f, currentTime, true);
                                }
                                if (currentTime >= fadeIn + 30 && currentTime < fadeIn + 90)
                                {
                                    text = text.Remove((int)MathHelper.Lerp(0, text.Length - 1, Utils.GetLerpValue(fadeIn + 30, fadeIn + 90, currentTime, true)));
                                }
                                else if (currentTime >= fadeIn + 30)
                                {

                                }
                                else
                                {
                                    text = "";
                                }

                                if (currentTime == fadeIn - 30)
                                {
                                    SoundEngine.PlaySound(BetterSoundID.ItemShadowflameHexDoll with { Pitch = -2f, Volume = 2f });
                                }

                                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)Main.screenWidth, (int)Main.screenHeight), Color.Black * screenOpacity);

                                int boxHeight = (int)(Main.screenHeight * 0.2f);
                                Main.spriteBatch.Draw(drainLine, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - boxHeight), null, Color.Black * boxOpacity * 1.6f, MathHelper.PiOver2, new Vector2(drainLine.Width / 2, drainLine.Height / 2), new Vector2(boxHeight / (float)drainLine.Width, Main.screenWidth  * 4 / (float)drainLine.Height), 0, 0);
                                
                                Utils.DrawBorderStringBig(Main.spriteBatch, text, Main.ScreenSize.ToVector2() / 2 - Vector2.UnitY * boxHeight, Color.Red * boxOpacity, 1, 0.5f, 0.5f);

                                Main.spriteBatch.End();
                            }
                        }
                        else
                        {
                            VoidIDX = -1;
                        }
                    }
                    else
                    {
                        VoidIDX = -1;
                    }
                }
            }
        }

        public override void OnKill()
        {
            RemixDowned.downedVoid = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}