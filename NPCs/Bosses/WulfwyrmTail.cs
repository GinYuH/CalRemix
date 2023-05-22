using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod;

namespace CalRemix.NPCs.Bosses
{
    public class WulfwyrmTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            // DisplayName.SetDefault("Wulfrum Excavator");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            WulfwyrmBody.InitializeSegment(NPC);
            NPC.Size = new(52, 72f);
            NPC.damage = 5;

        }

        public static void InitializeSegment(NPC npc)
        {
            npc.npcSlots = 5f;
            npc.width = 52;
            npc.height = 72;
            npc.defense = 8;
            npc.Calamity().unbreakableDR = true;
            npc.LifeMaxNERB(3500, 5000);

            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            npc.lifeMax += (int)(npc.lifeMax * HPBoost);
            npc.damage = 10;
            npc.ModNPC.AIType = -1;
            npc.knockBackResist = 0f;
            npc.canGhostHeal = false;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.netAlways = true;
            npc.dontCountMe = true;
            npc.boss = true;
            npc.Calamity().canBreakPlayerDefense = true;
            npc.Calamity().VulnerableToSickness = false;
            npc.Calamity().VulnerableToElectricity = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.chaseable);
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.chaseable = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
        }

        public override void AI() => WulfwyrmBody.PerformSegmentingAI(NPC);

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WulfrumExcavatorTail1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WulfrumExcavatorTail2").Type, 1f);

            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
            NPC head = Main.npc[CalRemixGlobalNPC.wulfyrm];
            if (CalRemixGlobalNPC.wulfyrm < 0 || CalRemixGlobalNPC.wulfyrm >= Main.maxNPCs || Main.npc[CalRemixGlobalNPC.wulfyrm] is null || Main.npc[CalRemixGlobalNPC.wulfyrm].type != ModContent.NPCType<WulfwyrmHead>())
                return;
            if (head.ModNPC<WulfwyrmHead>().PylonCharged == true) // Handles Pylon charge sprites.
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else
            {
                NPC.frame.Y = 0 * frameHeight;
            }
        }

        public override bool CheckActive() => false;

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WulfrumExcavatorTail1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WulfrumExcavatorTail2").Type, NPC.scale);
                }
            }
        }

    }
}
