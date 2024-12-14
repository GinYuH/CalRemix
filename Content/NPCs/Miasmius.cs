using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Core.Biomes;
using CalamityMod.Projectiles.Boss;

namespace CalRemix.Content.NPCs
{
    public class Miasmius : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Miasmius");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.width = 54;
            NPC.height = 64;
            NPC.defense = 12;
            NPC.lifeMax = 700;
            NPC.knockBackResist = 0.6f;
            NPC.value = Item.buyPrice(gold: 20);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = CalamityMod.Sounds.CommonCalamitySounds.PlagueBoomSound;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PlagueBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.Remix().GreenAI[0] = 0;
            NPC.Remix().GreenAI[1] = 0;
        }

        public override void AI()
        {
            NPC.Remix().GreenAI[0]++;
            NPC.Remix().GreenAI[2]++;
            if (NPC.Remix().GreenAI[1] == 0)
            {
                NPC.TargetClosest();
                NPC.velocity.X = 0;
                if (NPC.HasPlayerTarget)
                {
                    if (Main.player[NPC.target].Distance(NPC.Center) <= 256 || NPC.life < NPC.lifeMax)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie78, NPC.Center);
                        NPC.Remix().GreenAI[1] = 1;
                    }
                }
                if (CalamityUtils.CountProjectiles(ModContent.ProjectileType < SporeGasPlantera > ()) < 20)
                {
                    if (NPC.Remix().GreenAI[0] % (150 + Main.rand.Next(0, 20)) == 0)
                    {
                        Vector2 vel = new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 2));
                        Projectile projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<SporeGasPlantera>(), NPC.damage, 0, Main.myPlayer);
                        projectile.timeLeft = 600;
                        SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    }
                }

            }
            else
            {
                CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI.BuffedHerplingAI(NPC, Mod);
                if (NPC.HasPlayerTarget)
                {
                    if (NPC.Remix().GreenAI[0] % (70 + Main.rand.Next(0, 21)) == 0)
                    {
                        Vector2 dist = Main.player[NPC.target].position - NPC.position;
                        dist.Normalize();
                        Projectile projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, dist * 10, ModContent.ProjectileType<SporeGasPlantera>(), NPC.damage, 0, Main.myPlayer);
                        projectile.timeLeft = 600;
                        SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    }
                }
                if (NPC.Remix().GreenAI[2] % (120 + Main.rand.Next(0, 20)) == 0)
                {
                    SoundEngine.PlaySound(SoundID.Zombie78, NPC.Center);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("A common fungus that has grown angry from being infected by the plague.")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y != 0 && NPC.Remix().GreenAI[1] != 0)
            {
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight * 3;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.InModBiome<PlagueBiome>())
                return 0f;

            return SpawnCondition.HardmodeJungle.Chance * 0.4f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<PlagueCellCanister>(), 1, 2, 3);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
        }
    }
}
