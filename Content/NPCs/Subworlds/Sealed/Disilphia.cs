using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Biomes;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
            SpawnModBiomes = [ModContent.GetInstance<VolcanicFieldBiome>().Type];
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 40);
            NPC.boss = true;
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
                        NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        if (ExtraOne == 0)
                        {
                            if (NPC.velocity.Y == 0 && Timer > 10)
                            {
                                ExtraOne = 1;
                                Timer = 0;
                            }
                            NPC.velocity.Y += 1;
                        }
                        else
                        {
                            if (Timer > 60)
                            {
                                ChangePhase(PhaseType.DownRockets);
                            }
                        }
                    }
                    break;
                case PhaseType.DownRockets:
                    {
                        NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        int telegraphTime = 60;
                        int waitTime = telegraphTime + 120;
                        if (Timer == 1)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int projCount = 5;
                                for (int i = 0; i < projCount; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center + new Vector2(MathHelper.Lerp(-2000, 2000, i / (float)(projCount - 1)), -1000), Vector2.Zero, ModContent.ProjectileType<MercuryRocketFalling>(), CalRemixHelper.ProjectileDamage(300, 480), 1f, ai0: telegraphTime);
                                }
                            }
                        }
                        else if (Timer >= waitTime)
                        {
                            ChangePhase(PhaseType.Spikes);
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
                        int laserTime = waitTime + 180;
                        if (Timer == 1)
                        {
                            NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        }
                        if (Timer < chargeUpTime)
                        {
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
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(NPC.direction, 0), ProjectileID.RocketSkeleton, CalRemixHelper.ProjectileDamage(380, 590), 1f, ai0: NPC.whoAmI);
                            }
                        }
                        else if (Timer >=  laserTime)
                        {
                            ChangePhase(PhaseType.ClusterRockets);
                        }
                    }
                    break;
                case PhaseType.ClusterRockets:
                    {
                        NPC.direction = NPC.DirectionTo(target.Center).X.DirectionalSign();
                        int rocketSpawnTime = 60;
                        int waitTime = rocketSpawnTime + 120;
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
                        int disappearTime = 60;
                        int waitTime = disappearTime + 40;
                        int scareWaitTime = waitTime + 20;
                        int firingEndTime = scareWaitTime + 180;
                        int cooldownTime = firingEndTime + 60;

                        if (Timer <= disappearTime)
                        {
                            NPC.alpha = (int)MathHelper.Lerp(0, 255, Timer / (float)disappearTime);
                        }
                        else if (Timer <= waitTime)
                        {

                        }
                        else if (Timer <= scareWaitTime)
                        { 
                        }
                        else if (Timer <= firingEndTime)
                        {

                        }
                        else if (Timer <= cooldownTime)
                        {

                        }
                        else
                        {
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
    }
}