using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalamityAIs.CalamityBossAIs;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon.SmallAresArms;
using CalamityMod.Sounds;
using CalamityMod.World;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Biomes;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    [AutoloadBossHead]
    public class Disilphia : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float Timer => ref NPC.ai[1];

        public ref float ExtraOne => ref NPC.ai[2];

        public ref float ExtraTwo => ref NPC.ai[3];

        public static float ULTIMA_FadeIn => 60;

        public static float ULTIMA_BlackEnd => ULTIMA_FadeIn + 30;

        public static float ULTIMA_Face => ULTIMA_BlackEnd + 30;

        public static float ULTIMA_FaceDuration => ULTIMA_Face + 15;

        public static float ULTIMA_Attack => ULTIMA_FaceDuration + 90;

        public static float ULTIMA_AttackDuration => ULTIMA_Attack + 360;

        public static float ULTIMA_FadeOutBlackGate => ULTIMA_AttackDuration + 90;

        public static float ULTIMA_FadeOutBlackDuration => ULTIMA_FadeOutBlackGate + 100;

        public static float ULTIMA_FinalFade => ULTIMA_FadeOutBlackDuration + 60;

        public static float ULTIMA_FinalFadeDuration => ULTIMA_FinalFade + 50;

        public static float ULTIMA_Total => ULTIMA_FinalFadeDuration + 40;


        public enum PhaseType
        {
            SpawnAnimation = 0,
            DownRockets = 1,
            Spikes = 2,
            Laser = 3,
            ClusterRockets = 4,
            Ultimate = 5
        }

        public PhaseType CurrentPhase
        {
            get => (PhaseType)Phase;
            set => Phase = (int)value;
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 1150;
            NPC.height = 1150;
            NPC.lavaImmune = true;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound with { Pitch = -1 };
            NPC.lifeMax = 300000;
            NPC.defense = 200;
            NPC.damage = 200;
            NPC.aiStyle = -1;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            SpawnModBiomes = [ModContent.GetInstance<VolcanicFieldBiome>().Type];
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 40);
            NPC.boss = true;
            NPC.behindTiles = true;
            Music = CalRemixMusic.TheCalamity;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
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
                        if (Timer < 120 && ExtraOne == 0)
                        {
                            NPC.noTileCollide = true;
                            if (Timer % 30 == 3)
                            {
                                SoundEngine.PlaySound(EvilEye.EvilEyeAlarm1 with { MaxInstances = 0 }, target.Center);
                            }
                            NPC.velocity = Vector2.Zero;
                            NPC.alpha = 255;
                        }
                        else
                        {
                            NPC.alpha = 0;
                            NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                            if (ExtraOne == 0)
                            {
                                if (NPC.velocity.Y == 0 && Timer > 130)
                                {
                                    ExtraOne = 1;
                                    Timer = 0;
                                    for (int i = 0; i < 7; i++)
                                    {
                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                        {
                                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2(MathHelper.Lerp(-NPC.width / 2, NPC.width / 2, i / (float)(6)), NPC.height / 2), Vector2.Zero, ModContent.ProjectileType<MercuryRocketFalling>(), CalRemixHelper.ProjectileDamage(300, 480), 1f, ai0: 0);
                                            Main.projectile[p].Kill();
                                        }
                                    }
                                }
                                if (NPC.velocity.Y < 60)
                                {
                                    NPC.velocity.Y += 5;
                                }
                                if (NPC.Bottom.Y > target.Top.Y)
                                {
                                    NPC.noTileCollide = false;
                                }
                            }
                            else
                            {
                                if (Timer > 60)
                                {
                                    if (NPC.Calamity().newAI[3] == 0)
                                        ChangePhase(PhaseType.DownRockets);
                                    else
                                        ChangePhase((PhaseType)Main.rand.Next((int)PhaseType.DownRockets, (int)PhaseType.ClusterRockets + 1));
                                }
                            }
                        }
                    }
                    break;
                case PhaseType.DownRockets:
                    {
                        NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        int telegraphTime = 60;
                        int waitTime = telegraphTime + 120;
                        float localTimer = Timer % waitTime;
                        int attckCount = CalamityWorld.death ? 5 : CalamityWorld.revenge ? 4 : Main.expertMode ? 3 : 2;
                        if (localTimer == 1)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int projCount = CalamityWorld.death ? 15 : CalamityWorld.revenge ? 13 : Main.expertMode ? 9 : 5;
                                for (int i = 0; i < projCount; i++)
                                {
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center + new Vector2(MathHelper.Lerp(-2000, 2000, i / (float)(projCount - 1)), -1000), Vector2.Zero, ModContent.ProjectileType<MercuryRocketFalling>(), CalRemixHelper.ProjectileDamage(300, 480), 1f, ai0: telegraphTime);
                                    Main.projectile[p].localAI[0] = target.Center.Y - 200;
                                    Main.projectile[p].netUpdate = true;
                                }
                            }
                        }
                        else if (localTimer < waitTime)
                        {
                            if (localTimer % 120 == 2 && localTimer < telegraphTime)
                            {
                                SoundEngine.PlaySound(EvilEye.EvilEyeAlarm1 with { MaxInstances = 0 }, target.Center);
                            }
                        }
                        if (Timer >= waitTime * attckCount)
                        {
                            ChangePhase(PhaseType.Spikes);
                        }
                        if (localTimer == telegraphTime)
                        {
                            SoundEngine.PlaySound(ScorchedEarth.ShootSound with { Volume = 0.8f }, target.Center);
                        }
                    }
                    break;
                case PhaseType.Spikes:
                    {
                        NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        int spawnSpikeTime = 90;
                        int spikeRate = 7;
                        int waitTime = spawnSpikeTime + 120;
                        if (Timer < spawnSpikeTime)
                        {
                            if (Timer % spikeRate == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(1300, 1400), Main.rand.NextFloat(1300, 1400));
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, pos.DirectionTo(target.Center) * 5, ModContent.ProjectileType<MercurySpike>(), CalRemixHelper.ProjectileDamage(210, 340), 1);
                                }
                            }
                        }
                        else if (Timer > waitTime)
                        {
                            ChangePhase(PhaseType.Laser);
                            foreach (Projectile p in Main.ActiveProjectiles)
                            {
                                if (p.type == ModContent.ProjectileType<MercurySpike>())
                                {
                                    p.ai[1] = 190;
                                }
                            }
                        }
                    }
                    break;
                case PhaseType.Laser:
                    {
                        int chargeUpTime = 180;
                        int waitTime = chargeUpTime + 60;
                        int laserTime = waitTime + 360;
                        if (Timer == 1)
                        {
                            NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        }
                        if (Timer < chargeUpTime)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 2;
                            if (Main.rand.NextBool())
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemBunnyMountSummon with { Pitch = 0.4f, Volume = 1f, MaxInstances = 0 }, target.Center);
                            }
                            Vector2 pos = NPC.Center + new Vector2(NPC.direction * Main.rand.NextFloat(800, 3000), Main.rand.NextFloat(-600, 600));
                            float speed = Main.rand.NextFloat(20, 24);
                            float lifeTime = pos.Distance(NPC.Center) / speed;
                            for (int i = 0; i < 5; i++)
                                GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(pos, pos.DirectionTo(NPC.Center) * speed, Main.rand.NextFloat(1.2f, 2.4f), Color.Red, (int)lifeTime, squishStrenght: 0.8f));
                        }
                        else if (Timer < waitTime)
                        {

                        }
                        else if (Timer == waitTime)
                        {
                            SoundEngine.PlaySound(AstrumDeusHead.MineSound with { Pitch = -0.7f, Volume = 4f });
                            SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound with { Pitch = -0.7f });
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(NPC.direction, 0), ModContent.ProjectileType<DisilphiaBeam>(), CalRemixHelper.ProjectileDamage(380, 590), 1f, ai2: NPC.whoAmI);
                            }
                        }
                        else if (Timer < laserTime)
                        {
                            if (Main.rand.NextBool())
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemPhaseblade with { Pitch = 0.8f, Volume = 2f, MaxInstances = 0 }, target.Center);
                            }
                            if (Timer % 20 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 pos = target.Center + new Vector2(Main.rand.Next(1000, 1200) * Main.rand.NextBool().ToDirectionInt(), Main.rand.Next(200, 400) * Main.rand.NextBool().ToDirectionInt());
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, new Vector2((target.Center.X - pos.X).DirectionalSign() * 20, 0), ModContent.ProjectileType<MercuryRocket>(), CalRemixHelper.ProjectileDamage(200, 350), 1f);
                                    Main.projectile[p].timeLeft = 90;
                                }
                            }
                        }
                        else if (Timer >=  laserTime)
                        {
                            ChangePhase(PhaseType.ClusterRockets);
                            foreach (Projectile p in Main.ActiveProjectiles)
                            {
                                if (p.type == ModContent.ProjectileType<DisilphiaBeam>())
                                {
                                    p.timeLeft = 60;
                                }
                            }
                        }
                    }
                    break;
                case PhaseType.ClusterRockets:
                    {
                        NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        int rocketSpawnTime = 60;
                        int waitTime = rocketSpawnTime + 180;
                        if (Timer < rocketSpawnTime)
                        {
                            if (Timer % 3 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(1000, 1000), Vector2.Zero, ModContent.ProjectileType<MercuryBlob>(), CalRemixHelper.ProjectileDamage(300, 450), 1f, ai0: NPC.whoAmI);
                                }
                            }
                        }
                        else if (Timer >= waitTime)
                        {
                            ChangePhase(PhaseType.DownRockets);
                        }
                    }
                    break;
                case PhaseType.Ultimate:
                    {
                        // FadeIn. Disilphia goes transparent. Start black
                        // BlackEnd. Screen turns black by the end of this
                        // Face. Face starts appearing
                        // FaceDuration. Face disappears
                        // Attack. Face appears, bg turns sharp brown.
                        // AttackDuration. Attacks stop spawning
                        // FadeOutBlackGate. Face and brown fade out
                        // FadeOutBlackDuration. End of above
                        // FinalFade. Start of black fade out
                        // FinalFadeDuration. Black fades out
                        // Total. End phase
                        float disappearTime = ULTIMA_FadeIn;
                        float waitTime = ULTIMA_Face;
                        float scareWaitTime = ULTIMA_Attack;
                        float firingEndTime = ULTIMA_AttackDuration;
                        float cooldownTime = ULTIMA_FinalFadeDuration;

                        if (Timer <= disappearTime)
                        {
                            NPC.alpha = (int)MathHelper.Lerp(0, 255, Timer / (float)disappearTime);
                            NPC.dontTakeDamage = true;
                        }
                        else if (Timer <= waitTime)
                        {
                            if (Timer == waitTime)
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemXenopopperPop with { Pitch = -0.8f, Volume = 3 });
                            }
                        }
                        else if (Timer <= scareWaitTime)
                        {
                            if (Timer == scareWaitTime)
                            {
                                SoundEngine.PlaySound(new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/GenericJumpscare") with { Pitch = -1.6f, Volume = 3 });
                            }
                        }
                        else if (Timer <= firingEndTime)
                        {
                            if (Timer % 40 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    for (int i = 0; i < 30; i++)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2(MathHelper.Lerp(-3000, 3000, i / 29f), -600), new Vector2(Main.rand.NextBool().ToDirectionInt() * 5, 5 * (target.Center.Y > NPC.Center.Y - 600).ToDirectionInt()), ModContent.ProjectileType<DisilphiaGunk>(), CalRemixHelper.ProjectileDamage(210, 320), 1);
                                    }
                                }
                            }
                        }
                        else if (Timer <= cooldownTime)
                        {
                            NPC.alpha = (int)MathHelper.Lerp(255, 0, Utils.GetLerpValue(cooldownTime - disappearTime, cooldownTime, Timer, true));
                            NPC.dontTakeDamage = false;
                        }
                        else
                        {
                            NPC.Calamity().newAI[0] = 1;
                            ChangePhase((PhaseType)Main.rand.Next((int)PhaseType.DownRockets, (int)PhaseType.ClusterRockets + 1));
                        }
                    }
                    break;
            }
            Timer++;
            NPC.spriteDirection = NPC.direction;
        }
        public void ChangePhase(PhaseType phase)
        {
            NPC.velocity = Vector2.Zero;
            NPC.rotation = 0;
            CurrentPhase = phase;
            if (Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) > 5000)
            {
                CurrentPhase = PhaseType.SpawnAnimation;
                NPC.Center = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 3000);
                NPC.Calamity().newAI[3] = 1;
            }
            else if (NPC.life < (int)(NPC.lifeMax * 0.5f) && NPC.Calamity().newAI[0] == 0)
            {
                CurrentPhase = PhaseType.Ultimate;
            }
            Timer = 0;
            ExtraOne = 0;
            ExtraTwo = 0;
            NPC.netUpdate = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<MercuryCoatedSubcinium>(), 1, 16, 30);
            npcLoot.Add(ModContent.ItemType<Mercury>(), 1, 25, 40);
        }

        public override void OnKill()
        {
            RemixDowned.downedDisil = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool CheckActive()
        {
            return !NPC.HasValidTarget;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.velocity.Y != 0;
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (CurrentPhase == PhaseType.SpawnAnimation)
            {
                if (Timer < 120 && ExtraOne == 0)
                {
                    Texture2D warning = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/MercuryWarning").Value;
                    Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
                    Main.EntitySpriteDraw(warning, new Vector2(NPC.Center.X, NPC.Center.Y + 3400) - Main.screenPosition, null, Color.Orange * (1 + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 10)), 0, warning.Size() / 2, 1.5f * NPC.scale, 0);
                    Main.spriteBatch.ExitShaderRegion();
                }
            }
            Texture2D tex = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, Color.White * NPC.Opacity, NPC.rotation, tex.Size() / 2, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
    }
}