using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.UI;
using System.Linq;
using CalRemix.Items.Materials;
using CalRemix.Items.Weapons;
using CalRemix.World;

namespace CalRemix.NPCs
{
    public class PrimalAspid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Primal Aspid");
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<BrimstoneFlames>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<VulnerabilityHex>()] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("Aspid",
                "Uh oh! A Primal Aspid! Best be wary around those buggers as killing too many may subject you to ancient ice spells!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type && n.HasPlayerTarget),
                cooldown: 3);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 50;
            NPC.width = 68;
            NPC.height = 46;
            NPC.defense = 20;
            NPC.lifeMax = 430;
            NPC.knockBackResist = Main.expertMode ? 0.72f : 0.8f;
            NPC.Calamity().DR = 0.15f;
            NPC.value = 1000;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = false;
        }

        public override void AI()
        {
            NPC.direction = System.Math.Sign(NPC.velocity.X);
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();
            if (NPC.HasPlayerTarget)
            {
                Vector2 dist = Main.player[NPC.target].Center - NPC.Center;
                dist.Normalize();
                NPC.velocity = dist * 4f;
                if (Collision.CanHitLine(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1) && NPC.Distance(Main.player[NPC.target].Center) < 920)
                {

                    NPC.ai[1]++;
                    if (NPC.ai[1] > 120 && NPC.ai[1] < 210)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Vector2 velocity = dist * 10;
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, k / (float)(10 - 1)));

                            Dust.NewDust(NPC.Center + Vector2.Normalize(perturbedSpeed) * 6f, 10, 10, DustID.OrangeTorch, perturbedSpeed.X, perturbedSpeed.Y);
                        }
                    }
                    if (NPC.ai[1] == 210)
                    {
                        SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 velocity = dist * 16;
                                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4 / 3, MathHelper.PiOver4 / 3, i / (float)(3 - 1)));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Hostile.AspidShot>(), Main.expertMode ? NPC.damage / 4 : NPC.damage / 2, 0f);
                            }
                        }
                        NPC.ai[1] = -60;
                    }
                }
            }
            else
            {
                NPC.velocity *= 0.98f;
            }
            NPC.velocity.Y -= 3 + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly) * 1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
		new FlavorTextBestiaryInfoElement("Once thought extinct, they have reappeared at the edges of the world. These cruel foes will ambush targets and relentlessly attack with their searing venom.")
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
            if (spawnInfo.PlayerSafe || !Main.hardMode || !spawnInfo.Player.ZoneSnow || !CalRemixWorld.aspids)
            {
                return 0f;
            }
            return SpawnCondition.OverworldNightMonster.Chance * 0.4f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.OrangeTorch, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.OrangeTorch, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<CryoKeyMold>(), 5);
            npcLoot.Add(ModContent.ItemType<AspidBlaster>(), 10);
            npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<EssenceofHavoc>(), 1, 3, 4, ui: Main.expertMode);
            npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<EssenceofHavoc>(), 1, 2, 3, ui: !Main.expertMode);
        }
        public override void OnKill()
        {
            CalRemixGlobalNPC.aspidCount++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
    }
}
