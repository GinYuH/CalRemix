using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.Tiles.Ores;
using CalamityMod.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.World;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class OmegaFist : ModNPC
    {
        public NPC Papa => Main.npc[PapaIndex];
        public int PapaIndex => (int)NPC.ai[0] - 1;

        public float Timer => Papa.ModNPC<SkeletronOmega>().Timer;

        public float State => Papa.ModNPC<SkeletronOmega>().State;

        public float ExtraVar => NPC.ai[1];

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

        public static void SpawnAnimation(NPC Papa, NPC n, int Timer)
        {
            int grace = 3;
            if (n.ai[1] == 0)
            {
                if (n.velocity.Y == 0)
                {
                    n.velocity.Y = 3;
                }
                n.velocity.Y = MathHelper.Min(n.velocity.Y * 1.3f, 22);
            }
            if (n.ai[1] == 1 && Papa.ModNPC<SkeletronOmega>().Timer > 150)
            {
                float off = n.type == ModContent.NPCType<OmegaFist>() ? 1 : -1;
                CalamityUtils.SmoothMovement(n, 10, (Papa.Center + Vector2.UnitX * 300 * off) - n.Center, 10, 0.6f, true);
            }
            else if (Timer > grace && Collision.SolidTiles(n.position, n.width, n.height) && n.ai[1] == 0)
            {
                n.ai[1] = 1;
                n.velocity = Vector2.Zero;
                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 5;
                foreach (Player p in Main.ActivePlayers)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemExplosion, n.Center);
                    Point pos = p.Bottom.ToTileCoordinates();
                    Tile t = CalamityUtils.ParanoidTileRetrieval(pos.X, pos.Y);
                    if (t.IsTileSolidGround() && p.velocity == Vector2.Zero)
                    {
                        p.velocity.Y = -3;
                    }
                }
            }
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
                        SpawnAnimation(Papa, NPC, (int)Timer);
                        break;
                    }
                case SkeletronOmega.PhaseType.GunShots:
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation() + MathHelper.Pi;
                        int dir = -NPC.DirectionTo(Papa.Center).X.DirectionalSign();
                        CalamityUtils.SmoothMovement(NPC, 10, SkeletronOmega.TentCenter - NPC.Center + new Vector2(200, 100) + Vector2.UnitY.RotatedBy(Timer * 0.1f) * 5, 10, 0.6f, true);
                        break;
                    }
                case SkeletronOmega.PhaseType.SlamSlamSlam:
                    {
                        if (Papa.ai[2] == 0)
                        {
                            NPC.Center = Papa.Center + new Vector2(240, 50) + Vector2.UnitY.RotatedBy(Timer * 0.1f) * 5;
                            NPC.ai[1] = 0;
                        }
                        else
                        {
                            float time = 13;
                            int mod = (int)(Timer % SkeletronOmega.SlamDuration % time);
                            NPC.Center = Papa.Center - (Vector2.UnitY * 300).RotatedBy(MathHelper.Lerp(0, MathHelper.ToRadians(100), Utils.Turn01ToCyclic010(mod / time)));
                            if (mod == 3)
                            {
                                NPC.ai[1] = 1;
                            }
                            if (mod == (int)(time / 2f) && NPC.ai[1] == 1)
                            {
                                SoundEngine.PlaySound(BetterSoundID.ItemExplosion, NPC.Center);
                                Main.LocalPlayer.Calamity().GeneralScreenShakePower += 2;
                                GeneralParticleHandler.SpawnParticle(new PulseRing(NPC.Center, Vector2.Zero, Color.Red * 0.4f, 0.1f, 22f, 30));

                                int projCount = CalamityWorld.revenge ? 5 : 3;
                                for (int i = 0; i < projCount; i++)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.Center.X, SkeletronOmega.TentBottom), -Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(60)) * Main.rand.NextFloat(20f, 28f), ModContent.ProjectileType<TentDebris>(), CalRemixHelper.ProjectileDamage(120, 250), 1);
                                    }
                                }
                            }
                        }
                        break;
                    }
                case SkeletronOmega.PhaseType.Fireballs:
                    {
                        NPC.rotation = NPC.DirectionTo(Target.Center).ToRotation() + MathHelper.Pi;
                        int dir = -NPC.DirectionTo(Papa.Center).X.DirectionalSign();
                        CalamityUtils.SmoothMovement(NPC, 10, Papa.Center - NPC.Center + new Vector2(200, 100) + Vector2.UnitY.RotatedBy(Timer * 0.1f) * 5, 30, 1.6f, true);
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
            return State == (int)SkeletronOmega.PhaseType.SlamSlamSlam || State == (int)SkeletronOmega.PhaseType.Judgement;
        }
    }
}