using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.AcidRain;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Minibosses;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.Particles;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Accessories
{
    public class CriticalSlimeCore : ModProjectile
    {
        public Particle ring2;
        private int laserdirection = 1;
        public int MAXDMG = 300;
        public override string Texture => "CalamityMod/NPCs/Noxus/Noxus";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Critical Slime Core");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 300;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                MAXDMG = Projectile.damage * 3;
            }
            Projectile.spriteDirection = 1;
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bool flag64 = Projectile.type == ModContent.ProjectileType<CriticalSlimeCore>();
            if (flag64)
            {
                if (modPlayer.tvohide)
                {
                    Projectile.active = false;
                }
                if (modPlayer.godfather)
                {
                    Projectile.timeLeft = 2;
                }
            }
            NPC targ = CalamityUtils.MinionHoming(Projectile.Center, 22222, Main.player[Projectile.owner]);
            if (targ != null && targ.active)
            {
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] > 120)
                {
                    if (Projectile.localAI[0] % 600 == 0) // Deathray
                    {
                        SoundEngine.PlaySound(SoundID.Zombie102 with { Volume = SoundID.Zombie102.Volume - 0.4f }, Projectile.position);
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 direction = new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30));
                            direction.Normalize();
                            direction = direction.RotatedBy(MathHelper.ToRadians(30 * -laserdirection));
                            float angularChange = (MathHelper.Pi / 180f) * 1.1f * laserdirection;
                            if (Main.myPlayer == Projectile.owner)
                            {
                                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ModContent.ProjectileType<SlimeRay>(), Projectile.damage, 0f, Projectile.owner, angularChange, (float)Projectile.whoAmI);
                                if (Main.projectile.IndexInRange(p))
                                    Main.projectile[p].originalDamage = Projectile.originalDamage;
                            }
                            laserdirection *= -1;
                        }
                    }
                    else if (Projectile.localAI[0] % 300 == 0) // Pulse
                    {
                        SoundEngine.PlaySound(SoundID.Item105, Projectile.position);
                        ring2 = new BloomRing(Projectile.Center, Projectile.velocity, Color.Purple * 0.4f, 1.5f, 40);
                        GeneralParticleHandler.SpawnParticle(ring2);
                    }
                    else if (Projectile.localAI[0] % 60 == 0) // Bolts
                    {
                        SoundEngine.PlaySound(SoundID.Item105 with { Volume = SoundID.Zombie105.Volume - 0.4f }, Projectile.position);
                        if (Main.myPlayer == Projectile.owner)
                        {
                            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30)), ModContent.ProjectileType<CosmicBlast>(), Projectile.damage, 0f, Projectile.owner);
                            if (Main.projectile.IndexInRange(p))
                                Main.projectile[p].originalDamage = Projectile.originalDamage;
                        }
                    }
                }
            }
            else
            {
                CalamityUtils.FloatingPetAI(Projectile, false, 0);
                Projectile.localAI[0] = 0;
                Projectile.localAI[1] = 0;
            }
            if (ring2 != null)
            {
                ring2.Position = Projectile.Center;
                ring2.Velocity = Projectile.velocity;
                ring2.Scale *= 1.05f;
                ring2.Time += 1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC target = Main.npc[i];
                    if (target.chaseable && !target.friendly && target != null && target.active && CalamityUtils.CircularHitboxCollision(ring2.Position, ring2.Scale, target.getRect()))
                    {
                        target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
                    }
                }
            }
            Projectile.rotation += Projectile.velocity.X * 0.0004f;
            Projectile.ChargingMinionAI(1600f, 1800f, 2500f, 400f, 1, 30f, 24f, 12f, new Vector2(0f, -60f), 30f, 16f, true, true);
            List<int> Slimes = new List<int>
            {
                1, 16, 59, 71, 81, 138, 121, 122, 141, 147, 183, 184, 204, 225, 244, 302, 333, 335, 334, 336, 537,
                NPCID.SlimeSpiked,
                NPCID.QueenSlimeMinionBlue,
                NPCID.QueenSlimeMinionPink,
                NPCID.QueenSlimeMinionPurple,
                ModContent.NPCType<CalamityMod.NPCs.Astral.AstralSlime>(),
                ModContent.NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>(),
                ModContent.NPCType<AeroSlime>(),
                ModContent.NPCType<BloomSlime>(),
                ModContent.NPCType<CalamityMod.NPCs.Crags.InfernalCongealment>(),
                ModContent.NPCType<PerennialSlime>(),
                ModContent.NPCType<CryoSlime>(),
                ModContent.NPCType<GammaSlime>(),
                ModContent.NPCType<IrradiatedSlime>(),
                ModContent.NPCType<AuricSlime>(),
                ModContent.NPCType<CorruptSlimeSpawn>(),
                ModContent.NPCType<CorruptSlimeSpawn2>(),
                ModContent.NPCType<CrimsonSlimeSpawn>(),
                ModContent.NPCType<CrimsonSlimeSpawn2>(),
                ModContent.NPCType<LifeSlime>(),
                ModContent.NPCType<CragmawMire>()
            };
            if (!CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                Slimes.Add(ModContent.NPCType<AstrumAureus>());
                Slimes.Add(ModContent.NPCType<AureusSpawn>());
                Slimes.Add(ModContent.NPCType<CrimulanPaladin>());
                Slimes.Add(ModContent.NPCType<SplitCrimulanPaladin>());
                Slimes.Add(ModContent.NPCType<EbonianPaladin>());
                Slimes.Add(ModContent.NPCType<SplitEbonianPaladin>());
                Slimes.Add(ModContent.NPCType<SlimeGodCore>());
                Slimes.Add(NPCID.KingSlime);
                Slimes.Add(NPCID.QueenSlimeBoss);
            }
            if (Projectile.damage < MAXDMG)
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (Slimes.Contains(target.type) && target.getRect().Intersects(Projectile.getRect()))
                {
                    Projectile.damage += (int)(target.lifeMax * 0.05f);
                    target.active = false;
                }
                }
            Projectile.spriteDirection = 1;
        }
    }
}
