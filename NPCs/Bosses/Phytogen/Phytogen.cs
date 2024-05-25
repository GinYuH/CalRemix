using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Particles;
using CalRemix.Projectiles.Hostile;
using CalRemix.Items.Placeables;
using CalamityMod.Events;
using CalRemix.Biomes;
using System.Collections.Generic;
using CalRemix.NPCs.Minibosses;
using CalamityMod.NPCs.PlagueEnemies;
using CalamityMod.NPCs.PlaguebringerGoliath;
using Microsoft.Build.Tasks;
using System;
using CalRemix.Retheme;
using CalamityMod.Projectiles.Boss;
using CalRemix.Buffs;
using CalRemix.CrossCompatibility;
using CalamityMod.Systems;

namespace CalRemix.NPCs.Bosses.Phytogen
{
    [AutoloadBossHead]
    public class Phytogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];
        public NPC NPCTarget = null;

        public Rectangle teleportPos = new Rectangle();

        public static List<int> plagueEnemies = new List<int> { ModContent.NPCType<PlaguedFirefly>(),
            ModContent.NPCType<Miasmius>(),
            ModContent.NPCType<PlaguedSpidrone>(),
            ModContent.NPCType<PlagueCharger>(),
            ModContent.NPCType<PlagueChargerLarge>(),
            ModContent.NPCType<PlaguebringerMiniboss>(),
            ModContent.NPCType<PlaguebringerGoliath>(),
            ModContent.NPCType<Plagueshell>(),
            ModContent.NPCType<Viruling>(),
            ModContent.NPCType<Melter>(),
            ModContent.NPCType<PestilentSlime>(),
            ModContent.NPCType<PlaguedFirefly>(),
            ModContent.NPCType<PlagueEmperor>()};

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/CarcinogenHit", 3);
        public static readonly SoundStyle DeathSound = new("CalRemix/Sounds/CarcinogenDeath");

        public enum PhaseType
        {
            Passive = 0,
            Idle = 1,
            SapBlobs = 2,
            Moving = 3,
            Burrow = 4,
            LastStand = 5
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phytogen");
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 100;
            NPC.width = 86;
            NPC.height = 80;
            NPC.defense = 15;
            NPC.DR_NERD(0.1f);
            NPC.LifeMaxNERB(20000, 24000, 150000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.boss = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToCold = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/OncologicReinforcement");
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<PhytogenShield>(), ai0: NPC.whoAmI);
            for (int i = 0; i < 2; i++)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (i + 1) * 64, (int)NPC.position.Y, ModContent.NPCType<PineappleFrond>(), ai0: NPC.whoAmI, ai1: i, ai3: Main.rand.Next(120, 240));
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X - (i + 1) * 64, (int)NPC.position.Y, ModContent.NPCType<PineappleFrond>(), ai0: NPC.whoAmI, ai1: i + 2, ai3: Main.rand.Next(120, 240));
            }
        }

        public override void AI()
        {
            int yharChance = 43200;
            bool anyYhars = CalamityUtils.CountProjectiles(ModContent.ProjectileType<JungleDragonYharon>()) > 0;
            /*if (Main.LocalPlayer.controlUseTile)
            {
                yharChance = 1;
            }*/
            if (!anyYhars)
            {
                if (Main.rand.NextBool(yharChance))
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)NPC.Center.X + 3000, NPC.Center.Y, -60, 0, ModContent.ProjectileType<JungleDragonYharon>(), 0, 0, Main.myPlayer);
                    Main.NewText("Yharon, Dragon of Rebirth has awoken!", Color.MediumPurple);
                    SoundEngine.PlaySound(CalamityMod.NPCs.Yharon.Yharon.FireSound);
                }
            }
            else
            {
                NPC.velocity = Vector2.Zero;
                foreach (Projectile p in Main.projectile)
                {
                    if (p.type == ModContent.ProjectileType<JungleDragonYharon>() && p.active)
                    {
                        if (p.getRect().Intersects(NPC.getRect()))
                        {
                            NPC.life = 0;
                            NPC.HitEffect();
                            NPC.active = false;
                            p.timeLeft = 30;
                        }
                        break;
                    }
                }
                return;
            }
            Main.LocalPlayer.Calamity().isNearbyBoss = false;
            NPC.TargetClosest();
            NPC.dontTakeDamage = NPC.AnyNPCs(ModContent.NPCType<PineappleFrond>()) && Phase != (int)PhaseType.Passive;
            if (NPC.dontTakeDamage)
            {
                NPC.Calamity().CurrentlyIncreasingDefenseOrDR = true;
            }
            else
            {
                NPC.Calamity().CurrentlyIncreasingDefenseOrDR = false;
            }
            float lifeRatio = (float)NPC.life / (float)NPC.lifeMax;
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            if (!NPC.AnyNPCs(ModContent.NPCType<PhytogenShield>()))
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PhytogenShield>(), ai0: NPC.whoAmI);
            }
            if (Target == null || Target.dead)
            {
                NPC.velocity.Y += 1;
                NPC.Calamity().newAI[3]++;
                if (NPC.Calamity().newAI[3] > 240)
                {
                    NPC.active = false;
                }
                return;
            }
            NPC.Calamity().newAI[3] = 0;
            switch (Phase)
            {
                case (int)PhaseType.Passive:
                    {
                        NoMoreFrondsChange();
                        NPC.damage = 0;
                        NPC.boss = false;
                        if (lifeRatio < 0.99f)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.ai[3] = 0;
                            Phase = (int)PhaseType.Idle;
                        }
                        if (NPCTarget == null || !NPCTarget.active || !plagueEnemies.Contains(NPCTarget.type) || NPCTarget.life <= 0)
                        {
                            foreach (NPC n in Main.npc)
                            {
                                if (plagueEnemies.Contains(n.type) && n.active && n.life > 0)
                                {
                                    NPCTarget = n;
                                    break;
                                }
                            }
                            if (NPCTarget == null || !NPCTarget.active || !plagueEnemies.Contains(NPCTarget.type) || NPCTarget.life <= 0)
                            {
                                NPC.velocity *= 0.96f;
                            }
                        }
                        else
                        {
                            if (NPC.Distance(NPCTarget.Center) > 300 + NPCTarget.width && NPC.ai[3] < 180)
                            {
                                NPC.ai[3]++;
                                NPC.velocity = NPC.DirectionTo(NPCTarget.Center) * 8f;
                            }
                            else
                            {
                                NPC.velocity *= 0.97f;
                                NPC.ai[1]++;
                                if (NPC.ai[1] > 60)
                                {
                                    NPC.ai[2]++;
                                    if (NPC.ai[2] % 4 == 0)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(NPCTarget.Center) * 20, ModContent.ProjectileType<Potpourri>(), (int)MathHelper.Max(NPCTarget.lifeMax / 50, 222), 1f, Main.myPlayer, ai0: 0, ai1: NPCTarget.whoAmI, Main.rand.NextFloat(-2f, 2f));
                                        Main.projectile[p].hostile = false;
                                        Main.projectile[p].friendly = true;
                                        Main.projectile[p].localAI[0] = Main.rand.Next(0, 3);
                                    }
                                    if (NPC.ai[1] > 120)
                                    {
                                        NPC.ai[1] = 0;
                                        NPC.ai[3] = 0;
                                    }
                                }
                            }
                        }
                        // Enrage if you attack her fronds
                        foreach (NPC n in Main.npc)
                        {
                            if (n.active && n.ai[0] == NPC.whoAmI && n.life > 0 && n.life / n.lifeMax < 0.9f && n.type == ModContent.NPCType<PineappleFrond>())
                            {
                                NPC.ai[1] = 0;
                                NPC.ai[2] = 0;
                                NPC.ai[3] = 0;
                                Phase = (int)PhaseType.Idle;
                                break;
                            }
                        }
                    }
                    break;
                case (int)PhaseType.Idle:
                    {
                        NPC.damage = 100;
                        int minDist = 400;
                        float speed = 8f;
                        int maxTime = 180;
                        NPC.ai[1]++;
                        if (NPC.Distance(Target.Center) > minDist + Target.width && NPC.ai[1] < maxTime)
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * speed;
                        }
                        else
                        {
                            NPC.velocity *= 0.95f;
                            Phase = (int)PhaseType.SapBlobs;
                        }
                        NoMoreFrondsChange();
                        break;
                    }
                case (int)PhaseType.SapBlobs:
                    {
                        int fireRate = 10;
                        int projPerShot = 3;
                        int fireGate = 110;
                        int perRound = 8;
                        int totalRounds = 2;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > fireGate)
                        {
                            NPC.ai[2]++;
                            if (NPC.ai[2] % fireRate == 0)
                            {
                                float spreadfactor = MathHelper.PiOver4;
                                float scaleFactor = 10;
                                for (int i = 0; i < projPerShot; i++)
                                {
                                    Vector2 perturbedSpeed = NPC.DirectionTo(Target.Center).RotatedBy(MathHelper.Lerp(-spreadfactor, spreadfactor, i / ((float)projPerShot - 1))) * scaleFactor;
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, perturbedSpeed, ModContent.ProjectileType<Potpourri>(), (int)(NPC.damage * 0.25f), 0, Main.myPlayer, 1, Target.whoAmI, Main.rand.NextFloat(-2f, 2f));
                                    Main.projectile[p].localAI[0] = Main.rand.Next(0, 3);
                                }
                            }
                            if (NPC.ai[2] > fireRate * perRound)
                            {
                                NPC.ai[1] = 0;
                                NPC.ai[2] = 0;
                                NPC.ai[3]++;
                            }
                        }
                        NPC.velocity *= 0.95f;
                        if (NPC.ai[3] >= totalRounds)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.ai[3] = 0;
                            Phase = (int)PhaseType.Idle;
                        }
                        NoMoreFrondsChange();
                        break;
                    }
                case (int)PhaseType.Moving:
                    {
                        float speed = 6f;
                        int maxTime = 120;
                        NPC.ai[1]++;
                        if (NPC.ai[1] < maxTime)
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * speed;
                        }
                        else
                        {
                            NPC.velocity *= 0.95f;
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Burrow;
                        }
                        break;
                    }
                case (int)PhaseType.Burrow:
                    {
                        float speed = death ? 22f : rev ? 20f : 16f;
                        float acceleration = death ? 0.3f : rev ? 0.27f : 0.25f;
                        int minTime = 120;
                        int maxTime = 360;
                        if (CalamityWorld.death)
                        {
                            minTime /= 2;
                            maxTime /= 2;
                        }
                        int height = 100;
                        int fireRate = rev ? 120 : 160;
                        NPC.ai[2]++;
                        NPC.ai[3]++;
                        NPC.Calamity().newAI[1]--;
                        if (Collision.IsWorldPointSolid(NPC.Center, true) || Collision.SolidTiles(NPC.position, NPC.width, NPC.height))
                        {
                            if (NPC.Calamity().newAI[0] == 0)
                            {
                                NPC.Calamity().newAI[0] = 1;
                                if (NPC.Calamity().newAI[1] <= 0)
                                {
                                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                                    int totalVines = 3;
                                    int vineSpeed = 16;
                                    float variance = MathHelper.TwoPi / totalVines;
                                    for (int i = 0; i < totalVines; i++)
                                    {
                                        Vector2 velocity = new Vector2(0f, vineSpeed);
                                        velocity = velocity.RotatedBy(variance * i);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<PineappleFrondProj>(), (int)(0.25f * NPC.damage), 0, Main.myPlayer, Main.rand.Next(2, 6));
                                    }
                                    DustExplosion();
                                    NPC.Calamity().newAI[1] = 30;
                                }
                            }
                        }
                        else
                        {
                            NPC.Calamity().newAI[0] = 0;
                        }
                        if (NPC.ai[2] % fireRate == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
                            int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.SafeNormalize(Vector2.One) * speed, ModContent.ProjectileType<Potpourri>(), (int)(NPC.damage * 0.25f), 0, Main.myPlayer, 1, Target.whoAmI, Main.rand.NextFloat(-2f, 2f));
                            Main.projectile[p].localAI[0] = Main.rand.Next(0, 3);
                        }
                        /*if (NPC.ai[3] > maxTime || (NPC.ai[3] > minTime && Target.position.Y - height > NPC.Center.Y))
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.ai[3] = 0;
                            NPC.localAI[1] = 0;
                            NPC.localAI[0] = 0;
                            NPC.Calamity().newAI[0] = 0;
                            NPC.Calamity().newAI[1] = 0;
                            NPC.rotation = 0;
                            Phase = (int)PhaseType.Moving;
                            int totalLeaves = death ? 12 : rev ? 10 : 8;
                            int leafSpeed = 16;
                            float variance = MathHelper.TwoPi / totalLeaves;
                            for (int i = 0; i < totalLeaves; i++)
                            {
                                Vector2 velocity = new Vector2(0f, leafSpeed);
                                velocity = velocity.RotatedBy(variance * i);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ProjectileID.SeedPlantera, (int)(0.25f * NPC.damage), 0, Main.myPlayer);
                            }
                        }*/
                        if (lifeRatio < 0.5f)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.ai[3] = 0;
                            NPC.localAI[1] = 0;
                            NPC.localAI[0] = 0;
                            NPC.Calamity().newAI[0] = 0;
                            NPC.Calamity().newAI[1] = 0;
                            NPC.rotation = 0;
                            NPC.position = Target.Center - Vector2.UnitY * 200;
                            NPC.velocity *= 0;
                            DustExplosion();
                            Phase = (int)PhaseType.LastStand;
                        }
                        // Worm AI can ligma balls
                        WormAI(NPC, Target, speed, acceleration);
                        break;
                    }
                case (int)PhaseType.LastStand:
                    {
                        float speed = 2f;
                        int maxTime = 120;
                        int sporeFireRate = death ? 20 : rev ? 30 : expert ? 40 : 50;
                        int petalFireRate = death ? 40 : rev ? 60 : expert ? 70 : 80;
                        float petalSpeed = 8f;
                        int timeBeforeAttacking = 120;
                        NPC.ai[3]++;
                        if (NPC.ai[3] >= timeBeforeAttacking)
                        {
                            NPC.ai[1]++;
                            NPC.ai[2]++;
                            NPC.velocity = NPC.DirectionTo(Target.Center) * speed;
                            // Unused spore attack
                            if (NPC.ai[1] % sporeFireRate == 0)
                            {
                                //SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
                                //Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.SafeNormalize(Vector2.One) * speed, ModContent.ProjectileType<SporeGasPlantera>(), (int)(NPC.damage * 0.25f), 0, Main.myPlayer);
                            }
                            if (NPC.ai[2] % sporeFireRate == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity.SafeNormalize(Vector2.One) * petalSpeed, ModContent.ProjectileType<Potpourri>(), (int)(NPC.damage * 0.25f), 0, Main.myPlayer, 1, Target.whoAmI, Main.rand.NextFloat(-2f, 2f));
                                Main.projectile[p].localAI[0] = Main.rand.Next(0, 3);
                            }
                            foreach (Player p in Main.player)
                            {
                                if (p.active)
                                {
                                    if (p.Distance(NPC.Center) > 560)
                                    {
                                        p.AddBuff(ModContent.BuffType<HayFeaver>(), 2);
                                    }
                                }
                            }
                        }
                        else if (NPC.type == timeBeforeAttacking)
                        {
                            SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Exoblade.BeamHitSound, NPC.Center);
                        }
                        break;
                    }
            }
        }

        public void GoreExplosion()
        {
            for (int i = 0; i < 10; i++)
            {
                int d = Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, Mod.Find<ModGore>("Frond" + Main.rand.NextBool(1, 5)).Type, NPC.scale);

                Main.gore[d].velocity = (Main.gore[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
                Main.gore[d].timeLeft = 30;
            }
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleGrass, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public void NoMoreFrondsChange()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<PineappleFrond>()))
            {
                Phase = (int)PhaseType.Moving;
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("After the Archwizard was dishonorably discharged from the war, he fell into a state of smoking and gambling. During a gambling night, he sealed himself inside of a chunk of asbestos to win a bet. He was never heard from again.")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(HitSound, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleGrass, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleGrass, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.Obsidian, 1, 216, 224);
        }
        public override void OnKill()
        {
            RemixDowned.downedPhytogen = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedPhytogen = true;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<Potpourri>() && Phase == (int)PhaseType.Passive)
            {
                return false;
            }
            return null;
        }

        public static void WormAI(NPC npc, Player Target, float speed, float acceleration)
        {
            Vector2 vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float num50 = Target.position.X + (float)(Target.width / 2);
            float num51 = Target.position.Y + (float)(Target.height / 2);
            num50 = (int)(num50 / 16f) * 16;
            num51 = (int)(num51 / 16f) * 16;
            vector5.X = (int)(vector5.X / 16f) * 16;
            vector5.Y = (int)(vector5.Y / 16f) * 16;
            num50 -= vector5.X;
            num51 -= vector5.Y;
            float num64 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
            bool flag2 = false;
            int num39 = (int)(npc.position.X / 16f) - 1;
            int num40 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
            int num41 = (int)(npc.position.Y / 16f) - 1;
            int num42 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
            if (!flag2)
            {
                Vector2 vector2 = default(Vector2);
                for (int num43 = num39; num43 < num40; num43++)
                {
                    for (int num44 = num41; num44 < num42; num44++)
                    {
                        if (Main.tile[num43, num44] == null || ((!Main.tile[num43, num44].HasTile || (!Main.tileSolid[Main.tile[num43, num44].TileType] && (!Main.tileSolidTop[Main.tile[num43, num44].TileType] || Main.tile[num43, num44].TileFrameY != 0))) && Main.tile[num43, num44].LiquidAmount <= 64))
                        {
                            continue;
                        }
                        vector2.X = num43 * 16;
                        vector2.Y = num44 * 16;
                        if (npc.position.X + (float)npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + (float)npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                        {
                            flag2 = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[num43, num44].HasTile && Main.tileSolid[Main.tile[num43, num44].TileType])
                            {
                                WorldGen.KillTile(num43, num44, fail: true, effectOnly: true);
                            }
                        }
                    }
                }
            }
            if (!flag2)
            {
                Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int num46 = 1000;
                bool flag3 = true;
                for (int num47 = 0; num47 < 255; num47++)
                {
                    if (Main.player[num47].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[num47].position.X - num46, (int)Main.player[num47].position.Y - num46, num46 * 2, num46 * 2);
                        if (rectangle.Intersects(rectangle2))
                        {
                            flag3 = false;
                            break;
                        }
                    }
                }
                if (flag3)
                {
                    flag2 = true;
                }
            }
            if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length)
            {
                try
                {
                    vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    num50 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector5.X;
                    num51 = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector5.Y;
                }
                catch
                {
                }
                npc.rotation = (float)Math.Atan2(num51, num50) + 1.57f;
                num64 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
                int num65 = npc.width;
                num64 = (num64 - (float)num65) / num64;
                num50 *= num64;
                num51 *= num64;
                npc.velocity = Vector2.Zero;
                npc.position.X += num50;
                npc.position.Y += num51;
            }
            else
            {
                if (!flag2)
                {
                    npc.velocity.Y += 0.11f;

                    if (npc.velocity.Y > speed)
                    {
                        npc.velocity.Y = speed;
                    }
                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.4)
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X -= acceleration * 1.1f;
                        }
                        else
                        {
                            npc.velocity.X += acceleration * 1.1f;
                        }
                    }
                    else if (npc.velocity.Y == speed)
                    {
                        if (npc.velocity.X < num50)
                        {
                            npc.velocity.X += acceleration;
                        }
                        else if (npc.velocity.X > num50)
                        {
                            npc.velocity.X -= acceleration;
                        }
                    }
                    else if (npc.velocity.Y > 4f)
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X += acceleration * 0.9f;
                        }
                        else
                        {
                            npc.velocity.X -= acceleration * 0.9f;
                        }
                    }
                }
                else
                {
                    float num67 = num64 / 40f;
                    if (num67 < 10f)
                    {
                        num67 = 10f;
                    }
                    if (num67 > 20f)
                    {
                        num67 = 20f;
                    }
                    npc.soundDelay = (int)num67;
                    SoundEngine.PlaySound(SoundID.WormDig, npc.Center);

                    num64 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
                    float num68 = Math.Abs(num50);
                    float num69 = Math.Abs(num51);
                    float num70 = speed / num64;
                    num50 *= num70;
                    num51 *= num70;
                    bool flag6 = false;
                    if (!flag6)
                    {
                        if ((npc.velocity.X > 0f && num50 > 0f) || (npc.velocity.X < 0f && num50 < 0f) || (npc.velocity.Y > 0f && num51 > 0f) || (npc.velocity.Y < 0f && num51 < 0f))
                        {
                            if (npc.velocity.X < num50)
                            {
                                npc.velocity.X += acceleration;
                            }
                            else if (npc.velocity.X > num50)
                            {
                                npc.velocity.X -= acceleration;
                            }
                            if (npc.velocity.Y < num51)
                            {
                                npc.velocity.Y += acceleration;
                            }
                            else if (npc.velocity.Y > num51)
                            {
                                npc.velocity.Y -= acceleration;
                            }
                            if ((double)Math.Abs(num51) < (double)speed * 0.2 && ((npc.velocity.X > 0f && num50 < 0f) || (npc.velocity.X < 0f && num50 > 0f)))
                            {
                                if (npc.velocity.Y > 0f)
                                {
                                    npc.velocity.Y += acceleration * 2f;
                                }
                                else
                                {
                                    npc.velocity.Y -= acceleration * 2f;
                                }
                            }
                            if ((double)Math.Abs(num50) < (double)speed * 0.2 && ((npc.velocity.Y > 0f && num51 < 0f) || (npc.velocity.Y < 0f && num51 > 0f)))
                            {
                                if (npc.velocity.X > 0f)
                                {
                                    npc.velocity.X += acceleration * 2f;
                                }
                                else
                                {
                                    npc.velocity.X -= acceleration * 2f;
                                }
                            }
                        }
                        else if (num68 > num69)
                        {
                            if (npc.velocity.X < num50)
                            {
                                npc.velocity.X += acceleration * 1.1f;
                            }
                            else if (npc.velocity.X > num50)
                            {
                                npc.velocity.X -= acceleration * 1.1f;
                            }
                            if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.5)
                            {
                                if (npc.velocity.Y > 0f)
                                {
                                    npc.velocity.Y += acceleration;
                                }
                                else
                                {
                                    npc.velocity.Y -= acceleration;
                                }
                            }
                        }
                        else
                        {
                            if (npc.velocity.Y < num51)
                            {
                                npc.velocity.Y += acceleration * 1.1f;
                            }
                            else if (npc.velocity.Y > num51)
                            {
                                npc.velocity.Y -= acceleration * 1.1f;
                            }
                            if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.5)
                            {
                                if (npc.velocity.X > 0f)
                                {
                                    npc.velocity.X += acceleration;
                                }
                                else
                                {
                                    npc.velocity.X -= acceleration;
                                }
                            }
                        }
                    }
                }
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                if (flag2)
                {
                    if (npc.localAI[0] != 1f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.localAI[0] = 1f;
                }
                else
                {
                    if (npc.localAI[0] != 0f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.localAI[0] = 0f;
                }
                if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                {
                    npc.netUpdate = true;
                }
            }
        }

        public override bool CheckActive()
        {
            return Target == null || Target.dead || !Target.active;
        }
    }
}
