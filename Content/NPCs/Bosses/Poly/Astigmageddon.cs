using Microsoft.Xna.Framework;
using CalRemix.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Items.Placeables.MusicBoxes;
using CalamityMod;
using CalRemix.Core.World;
using CalamityMod.Events;
using CalRemix.Content.Items.Placeables.Trophies;

namespace CalRemix.Content.NPCs.Bosses.Poly
{
    // The main part of the boss, usually refered to as "body"
    [AutoloadBossHead] // This attribute looks for a texture called "ClassName_Head_Boss" and automatically registers it as the NPC boss head icon
    public class Astigmageddon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astigmageddon");

            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 110;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.LifeMaxNERB(17000, 31000, 340000);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 5);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.aiStyle = -1;
            NPC.Calamity().canBreakPlayerDefense = true;
            if (!Main.dedServ)
            {
                if (Main.zenithWorld)
                    Music = CalRemixMusic.PolyphemalusAlt;
                else
                    Music = CalRemixMusic.Polyphemalus;
            }

        }

        public Dictionary<string, int> AIShare = new Dictionary<string, int>()
        {
            { "soloTimer", 0 },
            { "beenSolo", 0},
            { "shotTimer", 0 },
            { "index", 0 },
        };
        public ref float timer => ref NPC.ai[0];
        public ref float phase => ref NPC.ai[1];

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }

        public override void AI()
        {
            // This should almost always be the first code in AI() as it is responsible for finding the proper player target
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            NPC.Calamity().CurrentlyEnraged = Main.dayTime && !BossRushEvent.BossRushActive;
            var eyesLeft = 0;
            AIShare["index"] = 0;
            NPC exotrexia = null;
            NPC cataractacomb = null;
            NPC conjunctivirus = null;
            Exotrexia exotrexiaModNpc = null;
            Cataractacomb cataractacombModNpc = null;
            Conjunctivirus conjunctivirusModNpc = null;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.type == ModContent.NPCType<Exotrexia>() && target.active == true)
                {
                    exotrexia = target;
                    exotrexiaModNpc = exotrexia.ModNPC as Exotrexia;
                    eyesLeft++;
                }
                if (target.type == ModContent.NPCType<Cataractacomb>() && target.active == true)
                {
                    cataractacomb = target;
                    cataractacombModNpc = cataractacomb.ModNPC as Cataractacomb;
                    eyesLeft++;
                }
                if (target.type == ModContent.NPCType<Conjunctivirus>() && target.active == true)
                {

                    conjunctivirus = target;
                    conjunctivirusModNpc = conjunctivirus.ModNPC as Conjunctivirus;
                    eyesLeft++;
                }
            }

            Player player = Main.player[NPC.target];

            if (player.dead)
            {
                // If the targeted player is dead, flee
                NPC.velocity.Y -= 0.04f;
                // This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
                NPC.EncourageDespawn(10);
                return;
            }
            TurnTowards(player.Center);

            if (phase == 0)
            {
                timer++;

                var circlePos = CirclePos(player, (float)(((timer % 180) * 2 - 90) * Math.PI / 180), 500f);
                if (Vector2.Distance(NPC.Center, circlePos) <= 48 * 4)
                {
                    NPC.Center = circlePos;
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    MoveTowards(circlePos, 80, 5);
                }

                if (timer >= 60)
                {
                    if (timer % 8 == 0)
                    {
                        ShootCenter(ProjectileID.DeathLaser, 5f, 10);
                    }
                }

                if (timer > 60 * 7)
                {
                    phase = 1;
                    timer = 0;
                    NPC.velocity = ((float)Math.PI / 180 * (90 + 30f)).ToRotationVector2()*20;
                }
            }
            if (phase == 1)
            {

                TurnTowards(player.Center);
                timer++;
                var circleSpot = eyesLeft;
                if (conjunctivirusModNpc != null) 
                {
                    if(conjunctivirusModNpc.AIShare["soloTimer"] > 0) circleSpot--;
                }
                if (cataractacombModNpc != null)
                {
                    if (cataractacombModNpc.AIShare["soloTimer"] > 0) circleSpot--;
                }
                if (exotrexiaModNpc != null)
                {
                    if (exotrexiaModNpc.AIShare["soloTimer"] > 0) circleSpot--;
                }
                var circlePos = CirclePos(player, (float)(((timer % 360) * 2 + (360 / (circleSpot + 1)) * AIShare["index"]) * Math.PI / 180), 650);

                if (Vector2.Distance(NPC.Center, circlePos) <= 48 * 4)
                {
                    NPC.Center = circlePos;
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    MoveTowards(circlePos, 60, 10);
                }
                if (timer % 60 == 10) 
                {
                    ShootCenter(ProjectileID.DeathLaser, 6, 45);
                }
                if (timer % 60 == 45 && AIShare["beenSolo"] == 0)
                {
                    phase = 2;
                    timer = 0;
                    AIShare["soloTimer"] = 1;
                }
            }
            if (phase == 2)
            {
                timer++;
                TurnTowards(player.Center);
                var hoverDistance = 400;
                if (NPC.Center.X - player.Center.X < 0)
                {
                    MoveTowards(player.Center + new Vector2(-hoverDistance, 0), 20, 20);
                } else
                {
                    MoveTowards(player.Center + new Vector2(hoverDistance, 0), 20, 20);
                }
                if (timer % 30 == 15)
                {
                    ShootCenter(ProjectileID.DeathLaser, 10, 50);
                }

                if (timer >= 360)
                {
                    timer = 0;
                    phase = 3;
                }
            }
            if (phase == 3)
            {
                timer++;
                TurnTowards(player.Center);
                var hoverDistance = 400;
                if (NPC.Center.X - player.Center.X < 0)
                {
                    MoveTowards(player.Center + new Vector2(-hoverDistance, 0), 20, 20);
                }
                else
                {
                    MoveTowards(player.Center + new Vector2(hoverDistance, 0), 20, 20);
                }
                if (timer % 45 == 15)
                {
                    ShootCenter(ProjectileID.DeathLaser, 8, 50, 22.5f);
                    ShootCenter(ProjectileID.DeathLaser, 10, 50);
                    ShootCenter(ProjectileID.DeathLaser, 8, 50, -22.5f);
                }

                if (timer >= 360)
                {
                    timer = NPC.Calamity().CurrentlyEnraged || BossRushEvent.BossRushActive ? 175 : 0;
                    phase = -1;
                }
            }

            if (phase == -1)
            {
                timer++;
                NPC.velocity *= 0.95f;
                if (timer > 180) { 
                phase = 1;
                timer = 0;
                AIShare["beenSolo"] = 1;
                AIShare["soloTimer"] = 0;
                if (exotrexia != null)
                {
                    timer = exotrexiaModNpc.timer;
                        exotrexiaModNpc.AIShare["beenSolo"] = 0;
                }
                    else if(conjunctivirus != null)
                {
                        timer = conjunctivirusModNpc.timer;
                        conjunctivirusModNpc.AIShare["beenSolo"] = 0;
                    }
                    else if (cataractacomb != null)
                    {
                        timer = cataractacombModNpc.timer;
                        cataractacombModNpc.AIShare["beenSolo"] = 0;
                    }
                    else
                        phase = 2;
                }
            }

        }
        public override void OnKill()
        {
            Cataractacomb cataractacomb = null;
            Exotrexia exotrexia = null;
            Conjunctivirus conjunctivirus = null;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.type == ModContent.NPCType<Cataractacomb>() && target.active == true)
                {
                    cataractacomb = target.ModNPC as Cataractacomb;
                }
                if (target.type == ModContent.NPCType<Exotrexia>() && target.active == true)
                {
                    exotrexia = target.ModNPC as Exotrexia;
                }
                if (target.type == ModContent.NPCType<Conjunctivirus>() && target.active == true)
                {

                    conjunctivirus = target.ModNPC as Conjunctivirus;
                }
            }
            if (AIShare["beenSolo"] == 0)
            {
                if (conjunctivirus != null)
                {
                    conjunctivirus.AIShare["beenSolo"] = 0;
                }
                else if (cataractacomb != null)
                {
                    cataractacomb.AIShare["beenSolo"] = 0;
                }
                else if (exotrexia != null)
                {
                    exotrexia.AIShare["beenSolo"] = 0;
                }
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<Cataractacomb>()) && !NPC.AnyNPCs(ModContent.NPCType<Exotrexia>()) && !NPC.AnyNPCs(ModContent.NPCType<Conjunctivirus>()))
            {
                RemixDowned.downedPolyphemalus = true;
                CalRemixWorld.UpdateWorldBool();
            }
        }
        private void ShootCenter(int type, float velocityMod, int damage, float spread = 0)
        {
            if (Main.masterMode) damage /= 4;
            else if (Main.expertMode) damage /= 4;
            else damage /= 2;
            Vector2 position = NPC.Center;
            Vector2 Velocity = NPC.rotation.ToRotationVector2() * velocityMod;
            Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), position, Velocity.RotatedBy(spread * Math.PI / 180), type, damage, 0f, Main.myPlayer);
            proj.timeLeft = 1200;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Cataractacomb>()) || NPC.AnyNPCs(ModContent.NPCType<Exotrexia>()) || NPC.AnyNPCs(ModContent.NPCType<Conjunctivirus>()))
                potionType = ItemID.None;
            else
                potionType = ItemID.GreaterHealingPotion;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule lastLivingPoly = new(new LastPolyBeaten());
            npcLoot.Add(lastLivingPoly);

            LeadingConditionRule normal = new LeadingConditionRule(new Conditions.IsExpert());
            normal.AddFail(ModContent.ItemType<Quadnoculars>(), 1, hideLootReport: Main.expertMode);
            lastLivingPoly.Add(normal);

            IItemDropRule dropItem = new DropLocalPerClientAndResetsNPCMoneyTo0(ModContent.ItemType<Heterochromia>(), 1, 1, 1, null);
            lastLivingPoly.OnSuccess(dropItem);

            lastLivingPoly.Add(ItemID.EyeMask);

            //lastLivingPoly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PolypebralShield>()));
            npcLoot.Add(ModContent.ItemType<AstigmageddonTrophy>(), 10);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 9, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 9, NPC.scale);
                }
            }
        }
        private Vector2 CirclePos(Player player, float rotation, float distance)
        {
            return player.Center + (rotation).ToRotationVector2() * distance;
        }

        private void MoveTowards(Vector2 goal, float speed, float inertia)
        {
            Vector2 moveTo = (goal - NPC.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
            NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;
        }

        private void TurnTowards(Vector2 goal, float offset = 0, float maxSpeed = 1)
        {
            float goal2 = (goal - NPC.Center).ToRotation() + offset;
            maxSpeed *= (float)Math.PI / 180f;
            float rad360 = (360 * (float)Math.PI / 180f);
            if (goal2 % rad360 + rad360 > NPC.rotation + rad360)
            {
                NPC.rotation += Math.Min((goal2 % rad360 + rad360) - NPC.rotation, maxSpeed + rad360);
            }
            if (goal2 % rad360 + rad360 < NPC.rotation + rad360)
            {
                NPC.rotation += Math.Min((goal2 % rad360 + rad360) - NPC.rotation, maxSpeed + rad360);
            }
        }

    }
}