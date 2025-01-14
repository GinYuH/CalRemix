#region thecord
/* using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using CalamityMod;
using CalamityMod.NPCs;
using CalamityMod.World;
using CalamityMod.Items.Fishing.FishingRods;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.NormalNPCs;
using CalRemix.Content.Projectiles.Hostile;
using System;
using CalRemix.Content.Items.Materials;
using CalRemix.Core.World;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.UI.ElementalSystem;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Content.Buffs;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class TheLeashHead : ModNPC
    {


        public float LifeRatio => NPC.life / (float)NPC.lifeMax;
        public Player Target => Main.player[NPC.target];

        public int SegmentCount = 40;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Leash");
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Position = new Vector2(0, 34),
                Rotation = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            TheLeashBody.InitializeSegment(NPC);
            NPC.width = 20;
            NPC.height = 30;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit8;
            NPC.DeathSound = SoundID.NPCDeath12;
            NPC.knockBackResist = 0f;
            NPC.defense = 30;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.damage = 65;
            NPC.boss = true;
            NPC.aiStyle = -1;
            Music = MusicID.Boss2;
        }


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }


        public override void AI()
        {

            // Reset things every frame. They may be temporarily changed by AI states below.
            NPC.dontTakeDamage = false;

            // Adds Boss Zen to the player.
            Main.player[Main.myPlayer].Calamity().isNearbyBoss = true;
            Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatBuffs.BossEffects>(), 10, true);

            // Pick a new target if the current one is invalid.
            if (NPC.target <= -1 || NPC.target >= Main.maxPlayers || Target.dead || !Target.active || !NPC.WithinRange(Target.Center, CalamityGlobalNPC.CatchUpDistance350Tiles))
            {
                NPC.TargetClosest();

                // Try to despawn either if there's a twin present, or if there's no targets. Burrow offscreen to avoid sudden disappearances.
                if (NPC.target <= -1 || NPC.target >= Main.maxPlayers || Target.dead || !Target.active || !NPC.WithinRange(Target.Center, CalamityGlobalNPC.CatchUpDistance350Tiles) || NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer))
                {
                    NPC.velocity.Y += 4f;
                    NPC.EncourageDespawn(10);
                }
            }

            // Create segments on the first frame.
            if (NPC.ai[3] == 0f)
            {
                SpawnSegments();
                NPC.ai[3] = 1f;
            }
            CalRemixNPC.WormAI(NPC, 24, 0.3f, Main.player[NPC.target], Main.player[NPC.target].Center);

        }

        public void SpawnSegments()
        {
            //if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int previousSegment = NPC.whoAmI;
                for (int i = 0; i < SegmentCount; i++)
                {

                    int nextSegmentIndex;
                    if (i < SegmentCount - 1)
                    {
                        nextSegmentIndex = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TheLeashBody>(), NPC.whoAmI);
                        Main.npc[nextSegmentIndex].localAI[1] = i % 2;
                    }
                    else
                        nextSegmentIndex = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TheLeashTail>(), NPC.whoAmI);
                    Main.npc[nextSegmentIndex].realLife = NPC.whoAmI;
                    Main.npc[nextSegmentIndex].ai[2] = NPC.whoAmI;
                    Main.npc[nextSegmentIndex].ai[1] = previousSegment;
                    Main.npc[previousSegment].ai[0] = nextSegmentIndex;

                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, nextSegmentIndex, 0f, 0f, 0f, 0);
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, previousSegment, 0f, 0f, 0f, 0);

                    previousSegment = nextSegmentIndex;
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemID.TwinsBossBag));

            LeadingConditionRule normalRule = npcLoot.DefineNormalOnlyDropSet();
            normalRule.Add(ItemID.SoulofSight, 1, 25, 30);
            npcLoot.AddNormalOnly(ItemDropRule.ByCondition(DropHelper.HallowedBarsCondition, ItemID.HallowedBar, 1, 15, 30));

        }

        public override bool CheckActive() => false;

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }
    }
}
*/
#endregion