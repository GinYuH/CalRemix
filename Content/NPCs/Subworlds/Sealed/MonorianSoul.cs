using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Potions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using System;
using CalamityMod.Graphics.Primitives;
using CalRemix.UI;
using Terraria.UI;
using static Terraria.Graphics.Effects.Filters;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using CalamityMod.NPCs.Cryogen;
using Terraria.Graphics.Shaders;
using CalRemix.Core.World;
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Sounds;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class MonorianSoul : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public int GemIndex
        {
            get => (int)NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public NPC Gem => Main.npc[(int)NPC.ai[2]];

        public ref float ExtraVar => ref NPC.ai[3];
        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }
        public Vector2 OldPosition
        {
            get => new Vector2(NPC.localAI[2], NPC.localAI[1]);
            set
            {
                NPC.localAI[2] = value.X;
                NPC.localAI[1] = value.Y;
            }
        }

        public enum PhaseType
        {
            SpawnAnimation = 0,
            Goozma = 1,
            Shotgun = 2,
            Bounce = 3,
            Laser = 4,
            Metagross = 5,
            Block = 6
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)State;
            set => State = (float)value;
        }

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lifeMax = 100000;
            NPC.damage = 270;
            NPC.defense = 40;
            NPC.noGravity = true;
            NPC.HitSound = Cryogen.HitSound with { Pitch = -1 };
            NPC.DeathSound = Cryogen.DeathSound with { Pitch = 1 };
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }
        public override void AI()
        {
            if (CurrentPhase > PhaseType.SpawnAnimation && (!Gem.active || Gem.type != NPCType<MonorianGemBoss>()))
            {
                int possibleGem = NPC.FindFirstNPC(NPCType<MonorianGemBoss>());
                if (possibleGem == -1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<MonorianGemBoss>(), ai1: NPC.whoAmI);
                    }
                }
                else
                {
                    GemIndex = possibleGem;
                }
                return;
            }
            NPC.TargetClosest(false);
            switch (CurrentPhase)
            {
                case PhaseType.SpawnAnimation:
                    {
                        if (Timer >= 90)
                        {
                            ChangePhase(PhaseType.Goozma);
                        }
                    }
                    break;
                case PhaseType.Goozma:
                    {
                        float repositionTime = 40;
                        float waitTime = repositionTime + 30;
                        float bulletHellTime = waitTime + 300;
                        float endAttack = bulletHellTime + 60;

                        float bulletHellPoints = 5;
                        if (Timer == 1)
                        {
                            SavePosition = Target.Center + Vector2.UnitY.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 500;
                            OldPosition = NPC.Center;
                        }
                        else if (Timer <= repositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(1, repositionTime, Timer, true), 1));
                        }
                        else if (Timer <= waitTime)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        else if (Timer <= bulletHellTime)
                        {
                            if (Timer % 7 == 0)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.ExoPlasmaShootSound with { Pitch = 0.5f }, NPC.Center);
                                for (int i = 0; i < bulletHellPoints; i++)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, i / bulletHellPoints)).RotatedBy(MathHelper.TwoPi / bulletHellPoints * MathF.Sin(Timer * 0.02f) * 0.5f) * 10, ProjectileType<MonorianSoulBolt>(), CalRemixHelper.ProjectileDamage(300, 500), 1);
                                    }
                                }
                            }
                        }
                        else if (Timer >= endAttack)
                        {
                            ChangePhase(PhaseType.Shotgun);
                        }
                    }
                    break;
                case PhaseType.Shotgun:
                    {
                        float waitTime = 70;
                        float countDownTime = waitTime + 240;
                        float waitForFinale = countDownTime + 50;
                        float finish = waitForFinale + 90;

                        float countDownCompletion = Utils.GetLerpValue(waitTime, countDownTime, Timer, true);
                        float shootTime = MathHelper.Lerp(70, 8, countDownCompletion);
                        float shootVelocity = MathHelper.Lerp(10, 8, countDownCompletion);
                        float shootSpreead = MathHelper.Lerp(MathHelper.ToRadians(10), MathHelper.ToRadians(130), countDownCompletion) * 0.5f;
                        int projCount = (int)MathHelper.Lerp(2, 18, countDownCompletion);

                        if (Timer < countDownTime)
                        {
                            if (Timer == 1)
                            {
                                NPC.direction = -Target.direction;
                                SavePosition = Vector2.UnitX * NPC.direction * 800;
                            }
                            if (ExtraVar >= shootTime)
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemExplosiveShotgun with { Pitch = 0.4f, Volume = 2 }, Target.Center);
                                for (int i = 0; i < projCount; i++)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Gem.Center).RotatedBy(MathHelper.Lerp(-shootSpreead, shootSpreead, i / (float)(projCount - 1))) * shootVelocity, ProjectileType<MonorianSoulBolt>(), CalRemixHelper.ProjectileDamage(240, 360), 1);
                                    }
                                }
                                ExtraVar = 0;
                            }
                            NPC.Center = Vector2.Lerp(NPC.Center, Target.Center + SavePosition, 0.1f);
                        }
                        else
                        {
                            NPC.velocity = Vector2.Zero;
                            if (Timer == waitForFinale)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound with { Pitch = 0.5f }, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Gem.Center) * 18, ProjectileType<MonorianSoulBall>(), CalRemixHelper.ProjectileDamage(360, 570), 1, ai0: NPC.whoAmI);
                                }
                            }
                            if (Timer >= finish)
                            {
                                ChangePhase(PhaseType.Bounce);
                            }
                        }

                        ExtraVar++;
                    }
                    break;
                case PhaseType.Bounce:
                    {
                        float repositionTime = 40;
                        float attackTime = repositionTime + 400;

                        if (Timer == 1)
                        {
                            SavePosition = Target.Center - Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * 300;
                            OldPosition = NPC.Center;
                        }
                        else if (Timer <= repositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(1, repositionTime, Timer, true), 1));
                        }
                        else if (Timer <= attackTime)
                        {
                            NPC.Center = SavePosition + Vector2.UnitY * MathF.Sin(Timer * 0.1f) * 20;
                        }
                        else
                        {
                            ChangePhase(PhaseType.Laser);
                        }

                        if (SavePosition != Vector2.Zero)
                        {
                            foreach (Player p in Main.ActivePlayers)
                            {
                                p.velocity = Vector2.Lerp(p.velocity, p.DirectionTo(SavePosition) * 20, Utils.GetLerpValue(330, 400, NPC.Distance(p.Center), true));
                                p.Calamity().infiniteFlight = true;
                            }
                        }
                    }
                    break;
                case PhaseType.Laser:
                    {
                        float repositionTime = 40;
                        float laserStart = repositionTime + 30;
                        float attackLength = laserStart + 300;

                        if (Timer == 1)
                        {
                            SavePosition = Target.Center - Vector2.UnitY * 400;
                            OldPosition = NPC.Center;
                        }
                        else if (Timer <= repositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(1, repositionTime, Timer, true), 1));
                        }
                        else if (Timer <= laserStart)
                        {
                            if (Timer == laserStart)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ProjectileType<MonorianSoulBolt>(), CalRemixHelper.ProjectileDamage(410, 700), 1, ai0: NPC.whoAmI);
                                }
                            }
                        }
                        else if (Timer >= attackLength)
                        {
                            ChangePhase(PhaseType.Metagross);
                        }
                    }
                    break;
                case PhaseType.Metagross:
                    {
                        float repositionTime = 40;
                        float attackTime = repositionTime + 400;
                        float waitTime = attackTime + 60;

                        if (Timer == 1)
                        {
                            SavePosition = Target.Center - Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * 500;
                            OldPosition = NPC.Center;
                        }
                        else if (Timer <= repositionTime)
                        {
                            NPC.Center = Vector2.Lerp(OldPosition, SavePosition, CalamityUtils.SineInOutEasing(Utils.GetLerpValue(1, repositionTime, Timer, true), 1));
                        }
                        else if (Timer < attackTime)
                        {
                            if (Timer % 10 == 0)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.ELRFireSound, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi), ProjectileType<MonorianSoulBolt>(), CalRemixHelper.ProjectileDamage(240, 450), 1, ai0: NPC.whoAmI);
                                }
                            }
                        }
                        else if (Timer >= waitTime)
                        {
                            ChangePhase(PhaseType.Block);
                        }
                    }
                    break;
                case PhaseType.Block:
                    {
                        float wait = 40;
                        float attackRate = 50;
                        float attackAmt = 6;
                        float stopAttack = wait + attackRate * attackAmt;
                        float next = stopAttack + 50;
                        if (Timer == 1)
                        {
                            NPC.direction = Main.rand.NextBool().ToDirectionInt();
                        }
                        else if (Timer < stopAttack)
                        {
                            if (Timer % attackRate == 0)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.ExoPlasmaShootSound with { Pitch = 0.5f }, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Gem.Center) * 10, ProjectileType<MonorianSoulBolt>(), CalRemixHelper.ProjectileDamage(300, 480), 1, ai0: 1, ai2: Target.whoAmI);
                                }
                            }
                        }
                        else if (Timer >= next)
                        {
                            int bolt = ProjectileType<MonorianSoulBolt>();
                            foreach (Projectile p in Main.ActiveProjectiles)
                            {
                                if (p.type == bolt)
                                {
                                    p.timeLeft = 30;
                                }
                            }
                            ChangePhase(PhaseType.Goozma);
                        }
                        NPC.Center = Vector2.Lerp(NPC.Center, Target.Center + Vector2.UnitX * NPC.direction * 630, 0.1f);
                    }
                    break;
            }
            Timer++;
        }

        public void ChangePhase(PhaseType newPhase)
        {
            CurrentPhase = newPhase;
            Timer = 0;
            ExtraVar = 0;
            OldPosition = Vector2.Zero;
            SavePosition = Vector2.Zero;
            if (Gem.active && Gem.type == NPCType<MonorianGemBoss>())
            {
                Gem.ai[2] = 0;
                Gem.ai[3] = 0;
                Gem.ModNPC<MonorianGemBoss>().OldPosition = Vector2.Zero;
                Gem.ModNPC<MonorianGemBoss>().SavePosition = Vector2.Zero;
                Gem.netUpdate = true;
            }
            NPC.netUpdate = true;
        }


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }

        public override void OnKill()
        {
            RemixDowned.downedOneguy = true;
            CalRemixWorld.UpdateWorldBool();

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Microsoft.Xna.Framework.Color drawColor)
        {
            spriteBatch.ExitShaderRegion();
            Texture2D radians = TextureAssets.Npc[ModContent.NPCType<WalkingBird>()].Value;
            Texture2D spark = Request<Texture2D>("CalamityMod/Particles/HollowCircleSoftEdge").Value;
            Texture2D star = Request<Texture2D>("CalamityMod/Particles/Sparkle").Value;

            float sizeMod = 0.5f;

            var shader = GameShaders.Misc[$"{Mod.Name}:Onesoul"];
            Color c = Color.Cyan;
            shader.UseColor(c * NPC.Opacity);
            shader.Apply();
            spriteBatch.EnterShaderRegion(BlendState.Additive, shader.Shader);
            spriteBatch.Draw(radians, NPC.Center - screenPos, null, Color.White, 5 * Main.GlobalTimeWrappedHourly, radians.Size() / 2, NPC.scale * new Vector2(12f, 1f) * sizeMod, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.ExitShaderRegion();
            spriteBatch.EnterShaderRegion(BlendState.Additive);

            //Main.spriteBatch.Draw(spark, NPC.Center - Main.screenPosition, null, Color.Cyan, Main.GlobalTimeWrappedHourly, spark.Size() / 2, NPC.scale * 1.8f * sizeMod, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(star, NPC.Center - screenPos, null, Color.White, 0, star.Size() / 2, NPC.scale * 2.4f * sizeMod + (1 + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 2f)), SpriteEffects.FlipHorizontally, 0);

            if (CurrentPhase == PhaseType.Bounce)
            {
                for (int i = 0; i < 50; i++)
                {
                    spriteBatch.Draw(star, SavePosition - Vector2.UnitY.RotatedBy(Main.GlobalTimeWrappedHourly * 0.6f + MathHelper.Lerp(0, MathHelper.TwoPi, i / 50f)) * 360 - screenPos, null, Color.Teal * Utils.GetLerpValue(20, 50, Timer, true));
                }
            }

            spriteBatch.ExitShaderRegion();

            return false;
        }
    }
}
