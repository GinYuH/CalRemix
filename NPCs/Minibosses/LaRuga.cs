using CalamityMod.Items.Placeables.Ores;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.Items.SummonItems;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Summon;
using Terraria.Audio;
using System;
using CalRemix.Buffs;
using CalRemix.Items.Ammo;
using CalRemix.World;

namespace CalRemix.NPCs.Minibosses
{
    public class LaRuga : ModNPC
    {
        ref float Phase => ref NPC.ai[0];
        enum AttackID
        {
            idle = 0,
            alert = 1,
            attacking = 2,
            neckcrack = 3,
            hover = 4,
            death = 6,
            attatch = 7
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("La Ruga");
            Main.npcFrameCount[NPC.type] = 9;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override bool SpecialOnKill()
        {
            RemixDowned.downedLaRuga = true;
            return false;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 2;
            NPC.width = 206;
            NPC.height = 162;
            NPC.defense = 0;
            NPC.lifeMax = 110000;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().ShouldCloseHPBar = true;
            NPC.rarity = 22;
        }

        public override void AI()
        {
            NPC.Calamity().ShouldCloseHPBar = true;
            NPC.TargetClosest();
            NPC.ai[3]--;
            Player targ = Main.player[NPC.target];
            if (targ.dead)
            {
                NPC.alpha += 22;
                if (NPC.alpha >= 255)
                    NPC.active = false;
                return;
            }
            NPC.direction = Phase == (int)AttackID.idle && NPC.wet ? -Math.Sign(NPC.DirectionTo(targ.Center).X) : Math.Sign(NPC.DirectionTo(targ.Center).X);
            NPC.spriteDirection = NPC.direction;
            if (NPC.life <= 2)
            {
                SwitchPhase((int)AttackID.death);
            }
            switch (Phase)
            {
                case (int)AttackID.idle:
                    {
                        if (Main.rand.NextBool(120))
                        {
                            SoundEngine.PlaySound(SoundID.Item2 with { Pitch = SoundID.Item2.Pitch - 1f, MaxInstances = 22 }, NPC.Center);
                        }
                        if (Main.rand.NextBool(360))
                        {
                            SoundEngine.PlaySound(SoundID.NPCDeath13 with { Pitch = SoundID.NPCDeath13.Pitch - 1f, MaxInstances = 22 }, NPC.Center);
                        }
                        NPC.noGravity = false;
                        if (targ.Distance(NPC.Center) < 320 || NPC.life < NPC.lifeMax)
                        {
                            SoundEngine.PlaySound(CnidarianJellyfishOnTheString.SlapSound, NPC.Center);
                            SoundEngine.PlaySound(SoundID.ScaryScream with { Pitch = SoundID.ScaryScream.Pitch - 1f }, NPC.Center);
                            SwitchPhase((int)AttackID.alert);
                        }
                        break;
                    }

                case (int)AttackID.alert:
                    {
                        NPC.noGravity = false;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > 60)
                        {
                            SwitchPhase((int)AttackID.attacking);
                        }
                        break;
                    }

                case (int)AttackID.attacking:
                    {
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        Vector2 dist = targ.Center - NPC.Center;
                        NPC.velocity = dist.SafeNormalize(Vector2.Zero) * 5;

                        if (NPC.life < NPC.lifeMax * 0.5f && Main.rand.NextBool(1200))
                        {
                            SoundEngine.PlaySound(CnidarianJellyfishOnTheString.SlapSound, NPC.Center);
                            SoundEngine.PlaySound(SoundID.ScaryScream with { Pitch = SoundID.ScaryScream.Pitch - 1f }, NPC.Center);
                            SwitchPhase((int)AttackID.neckcrack);
                        }
                        break;
                    }

                case (int)AttackID.neckcrack:
                    {
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        NPC.ai[1]++;
                        targ.AddBuff(BuffID.Obstructed, 300);
                        if (NPC.ai[1] > 60)
                        {
                            SwitchPhase((int)AttackID.hover);
                        }
                        break;
                    }

                case (int)AttackID.hover:
                    {
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        Vector2 dist = targ.Center - NPC.Center;
                        NPC.velocity = dist.SafeNormalize(Vector2.Zero) * 2;
                        NPC.ai[1]++;
                        if (NPC.ai[1] >= 300)
                        {
                            SwitchPhase((int)AttackID.attacking);
                        }
                        break;
                    }
                   
                case (int)AttackID.attatch:
                    {
                        targ.RemoveAllIFrames();
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        Vector2 dist = targ.Center - NPC.Center;
                        NPC.velocity = dist.SafeNormalize(Vector2.Zero) * 22;
                        if (NPC.ai[2] <= 0)
                        {
                            SwitchPhase((int)AttackID.attacking);
                        }
                        break;
                    }

                case (int)AttackID.death:
                    {
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        NPC.velocity = Vector2.Zero;
                        NPC.ai[1]++;
                        break;
                    }
            }
        }

        public void SwitchPhase(int ai0 = 0, int ai1 = 0, int ai2 = 0, int ai3 = 0)
        {
            NPC.ai[0] = ai0;
            NPC.ai[1] = ai1;
            //NPC.ai[2] = ai2;
            //NPC.ai[3] = ai3;
        }

        public override bool CheckDead()
        {
            return true;
        }
        public override void OnKill()
        {
            CalamityMod.Particles.DeathAshParticle.CreateAshesFromNPC(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("22 shrieks of agony...")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (Phase == (int)AttackID.attacking || Phase == (int)AttackID.attatch)
            {
                NPC.frameCounter += 0.75f;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 7 || NPC.frame.Y < frameHeight * 4)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
            }
            else if (Phase == (int)AttackID.neckcrack || Phase == (int)AttackID.death || Phase == (int)AttackID.hover || Phase == (int)AttackID.alert)
            {
                NPC.frame.Y = frameHeight * 8;
            }
            else if (Phase == (int)AttackID.idle && NPC.wet)
            {
                NPC.frameCounter += 0.75f;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 4)
                {
                    NPC.frame.Y = frameHeight * 0;
                }
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float val = 0;
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer4)
                val = SpawnCondition.CaveJellyfish.Chance * 0.0022f;

            if (DownedBossSystem.downedExoMechs && spawnInfo.Player.ZoneForest)
                val = SpawnCondition.OverworldNightMonster.Chance * 0.000022f;

            if (NPC.AnyNPCs(Type) || !CalRemixWorld.laruga)
                return 0;
            return val;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Terminus>());
            npcLoot.Add(ModContent.ItemType<CosmiliteCoin>(), 1, 22, 22);
            npcLoot.Add(ModContent.ItemType<HornetRound>(), 1, 19, 22);
            npcLoot.Add(ModContent.ItemType<ScoriaOre>(), 1, 20, 50);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Phase == (int)AttackID.attatch)
            {
                float drainAmt = Main.expertMode ? 0.015f : 0.01f;
                target.statLife -= (int)(target.statLifeMax2 * drainAmt);
                NPC.ai[2]--;
                NPC.ai[3] = 300;
            }
            else
            {
                float drainAmt = Main.expertMode ? 0.08f : 0.04f;
                target.statLife -= (int)(target.statLifeMax2 * drainAmt);
                if (Phase == (int)AttackID.hover)
                {
                    if (Main.rand.NextBool(20))
                    {
                        target.AddBuff(ModContent.BuffType<Scorinfestation>(), CalamityUtils.SecondsToFrames(300));
                    }
                }
                else
                {
                    if (NPC.ai[3] <= 0)
                        NPC.ai[2]++;
                    if (NPC.ai[2] >= 3)
                    {
                        NPC.ai[2] = 22;
                        SwitchPhase((int)AttackID.attatch);
                    }
                }
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetMaxDamage(NPC.lifeMax - 1);
        }
        public override void ModifyHitByProjectile(Projectile item, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetMaxDamage(NPC.lifeMax - 1);
        }
    }
}
