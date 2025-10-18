using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using CalamityMod.Tiles.Ores;
using CalamityMod.Sounds;
using CalamityMod.World;
using CalRemix.Content.Projectiles.Hostile;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class OmegaSoothingSunlightBlaster : ModNPC
    {
        public NPC Papa => Main.npc[PapaIndex];
        public int PapaIndex => (int)NPC.ai[0] - 1;

        public float Timer => Papa.ModNPC<SkeletronOmega>().Timer;

        public float State => Papa.ModNPC<SkeletronOmega>().State;

        public Vector2 PapaPosition => Papa.Center;

        public Player Target => Main.player[Papa.target];

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.LifeMaxNERB(25000, 50000, 150000);
            NPC.damage = 280;
            NPC.defense = 12;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.HitSound = AuricOre.MineSound;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.DR_NERD(0.05f);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            if (PapaIndex == -1)
            {
                NPC.active = false;
                return;
            }
            if (!Papa.active || Papa.type != ModContent.NPCType<SkeletronOmega>())
            {
                NPC.active = false;
                return;
            }
            switch ((SkeletronOmega.PhaseType)State)
            {
                case SkeletronOmega.PhaseType.SpawnAnim:
                    {
                        OmegaFist.SpawnAnimation(Papa, NPC, (int)Timer);
                        break;
                    }
                case SkeletronOmega.PhaseType.GunShots:
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation();
                        int dir = NPC.DirectionTo(Papa.Center).X.DirectionalSign();
                        int dist = dir == 1 ? 280 : 200;
                        CalamityUtils.SmoothMovement(NPC, 10, SkeletronOmega.TentCenter - NPC.Center + NPC.DirectionTo(Target.Center) * dist, 10, 0.6f, true);
                        int reticle = CalamityUtils.FindFirstProjectile(ModContent.ProjectileType<OmegaReticle>());
                        if (reticle > -1)
                        {
                            NPC.rotation = NPC.DirectionTo(Main.projectile[reticle].Center).ToRotation();
                        }
                        break;
                    }
                case SkeletronOmega.PhaseType.SlamSlamSlam:
                    {
                        if (Papa.ai[2] == 0)
                        {
                            NPC.Center = Papa.Center + new Vector2(-240, 50) + Vector2.UnitX * (-40 + 40 * MathF.Sin(Timer % SkeletronOmega.SlamDuration % SkeletronOmega.SlamTravelTime * 0.25f)) + Vector2.UnitY.RotatedBy(Timer * 0.1f) * 5;
                        }
                        else
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        break;
                    }
                case SkeletronOmega.PhaseType.Fireballs:
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation();
                        int dir = -NPC.DirectionTo(Papa.Center).X.DirectionalSign();
                        float localTimer = Timer % (SkeletronOmega.FireballTravelTime + SkeletronOmega.FireballDuration);
                        Vector2 extra = localTimer > SkeletronOmega.FireballTravelTime ? NPC.DirectionTo(Target.Center) * 200 : Vector2.Zero;
                        CalamityUtils.SmoothMovement(NPC, 10, Papa.Center - NPC.Center + extra + Vector2.UnitY.RotatedBy(Timer * 0.1f) * 5, 30, 1.6f, true);

                        if (localTimer > SkeletronOmega.FireballTravelTime + 30)
                        {
                            if (localTimer % 20 == 0)
                            {
                                SoundEngine.PlaySound(CommonCalamitySounds.ELRFireSound, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int projCount = CalamityWorld.revenge ? 10 : Main.expertMode ? 8 : 6;
                                    float randomness = Main.rand.NextFloat(0, 5);
                                    for (int i = 0; i < projCount; i++)
                                    {
                                        float halfAngle = MathHelper.ToRadians(30);
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Target.Center).RotatedBy(MathHelper.Lerp(-halfAngle, halfAngle, i / (float)(projCount - 1))).RotatedByRandom(MathHelper.ToRadians(randomness)) * 20, ProjectileID.Missile, CalRemixHelper.ProjectileDamage(280, 380), 1);
                                    }
                                }
                            }
                        }
                        break;
                    }
                case SkeletronOmega.PhaseType.Judgement:
                    {
                        break;
                    }
                case SkeletronOmega.PhaseType.Desperation:
                    {
                        NPC.active = false;
                        NPC.HitEffect();
                        break;
                    }
            }
        }

        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(ModContent.NPCType<SkeletronOmega>());
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}