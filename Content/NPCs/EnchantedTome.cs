using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles;

namespace CalRemix.Content.NPCs
{
    public class EnchantedTome : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Enchanted Tome");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.width = 36;
            NPC.height = 36;
            NPC.defense = 10;
            NPC.lifeMax = 350;
            NPC.knockBackResist = 0f;
            NPC.value = 10000;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = BetterSoundID.ItemIceHit;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<FrozenStrongholdBiome>().Type };
        }

        public override void AI()
        {
            NPC.direction = System.Math.Sign(NPC.velocity.X);
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();
            int startTelegraph = 60;
            int endTelegraph = startTelegraph + 90;
            int fireRate = 40;
            if (NPC.HasPlayerTarget)
            {
                Vector2 dist = Main.player[NPC.target].Center - NPC.Center;
                dist.Normalize();
                if ((NPC.Distance(Main.player[NPC.target].Center) < 320 && NPC.ai[2] == 0) || (NPC.Distance(Main.player[NPC.target].Center) < 920 && NPC.ai[2] == 1))
                {
                    NPC.ai[2] = 1;
                    NPC.ai[1]++;
                    if (NPC.ai[1] > startTelegraph && NPC.ai[1] < endTelegraph)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            Vector2 velocity = dist * 10;
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, k / (float)(3 - 1)));

                            Dust.NewDust(NPC.Center + Vector2.Normalize(perturbedSpeed) * 6f, 10, 10, DustID.Ice, perturbedSpeed.X, perturbedSpeed.Y);
                        }
                    }
                    if (NPC.ai[1] == endTelegraph + fireRate || NPC.ai[1] == endTelegraph + fireRate * 2 || NPC.ai[1] == endTelegraph + fireRate * 3)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemMagicIceBlock, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 velocity = dist * 16;
                                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4 / 3, MathHelper.PiOver4 / 3, i / (float)(3 - 1)));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ProjectileID.FrostBlastHostile, Main.expertMode ? NPC.damage / 4 : NPC.damage / 2, 0f);
                            }
                        }
                    }
                    if (NPC.ai[1] == endTelegraph + fireRate * 4)
                    {
                        if (NPC.CountNPCS(ModContent.NPCType<EnchantedSkull>()) < 7)
                        {
                            SoundEngine.PlaySound(BetterSoundID.ItemPortalGun2, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + Main.rand.Next(-100, 100), (int)NPC.Center.Y + Main.rand.Next(-10, 10), ModContent.NPCType<EnchantedSkull>());
                                }
                            }
                        }
                        NPC.ai[1] = 0;
                    }
                }
                else
                {
                    NPC.ai[1] = 0;
                }
            }
            else
            NPC.velocity *= 0.98f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.InModBiome<FrozenStrongholdBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            if (spawnInfo.SpawnTileType != ModContent.TileType<FrostflakeBrickPlaced>() && spawnInfo.SpawnTileType != ModContent.TileType<CryonicBrick>() && spawnInfo.SpawnTileType != TileID.Platforms)
                return 0f;
            return 0.4f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ice, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ice, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
