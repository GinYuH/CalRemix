using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Content.NPCs.Bosses.Wulfwyrm
{
    public class WulfwyrmBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            // DisplayName.SetDefault("Wulfrum Excavator");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults() => InitializeSegment(NPC);


    public static void InitializeSegment(NPC npc)
        {
            npc.npcSlots = 5f;
            npc.width = 40;
            npc.height = 40;
            npc.defense = 8;
            npc.Calamity().unbreakableDR = true;
            npc.LifeMaxNERB(3500, 5000, 1500000);

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
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.chaseable);
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.chaseable = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
        }

        public override void AI() => PerformSegmentingAI(NPC);

        public static void PerformSegmentingAI(NPC npc)
        {
            // Die immediately if the ahead segment is invalid.
            if (npc.ai[1] <= -1f || npc.realLife <= -1f || !Main.npc[(int)npc.ai[1]].active)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                npc.life = 0;
                npc.HitEffect();

                npc.active = false;
                npc.netUpdate = true;
            }

            // Don't use a boss HP bar.
            NPC aheadSegment = Main.npc[(int)npc.ai[1]];
            npc.Calamity().ShouldCloseHPBar = true;

            // Inherit various attributes from the head segment.
            NPC head = Main.npc[npc.realLife];
            npc.Opacity = head.Opacity;
            npc.chaseable = true;
            npc.friendly = false;
            npc.dontTakeDamage = head.dontTakeDamage;
            npc.damage = npc.dontTakeDamage ? 0 : npc.defDamage;

            // Rotate such that each segment approaches the ahead segment's rotation with an interpolant of 0.03. This process is asymptotic.
            Vector2 directionToNextSegment = aheadSegment.Center - npc.Center;
            if (aheadSegment.rotation != npc.rotation && head.velocity.Length() > 2f)
                directionToNextSegment = directionToNextSegment.RotatedBy(MathHelper.WrapAngle(aheadSegment.rotation - npc.rotation) * 0.03f);
            directionToNextSegment = directionToNextSegment.SafeNormalize(Vector2.Zero);

            npc.rotation = directionToNextSegment.ToRotation() + MathHelper.PiOver2;
            npc.Center = aheadSegment.Center - directionToNextSegment * npc.width * npc.scale * 0.725f;
            npc.localAI[0] += 0.15f;
            if (npc.localAI[0] > 3)
                npc.localAI[0] = 0;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
            if (CalRemixNPC.wulfyrm < 0 || CalRemixNPC.wulfyrm >= Main.maxNPCs)
                return;
            if (Main.npc[CalRemixNPC.wulfyrm] is null || Main.npc[CalRemixNPC.wulfyrm].type != ModContent.NPCType<WulfwyrmHead>())
                return;
            NPC head = Main.npc[CalRemixNPC.wulfyrm];
            if (head.ModNPC<WulfwyrmHead>().PylonCharged == true) // Handles Pylon charge sprites.
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else
            {
                NPC.frame.Y = 0 * frameHeight;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D roll = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Wulfwyrm/WulfwyrmWheels").Value;
            Rectangle sourceRectangle = new Rectangle(0, (int)NPC.localAI[0] * roll.Height / 3, roll.Width, roll.Height / 3);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);
            if (NPC.localAI[1] == 1)
                spriteBatch.Draw(roll, draw, sourceRectangle, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            return true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

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
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WulfrumExcavatorBody").Type, NPC.scale);
                    if (NPC.localAI[1] == 1)
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, (Main.rand.NextBool(2) ? Mod.Find<ModGore>("WulfrumExcavatorTire").Type : Mod.Find<ModGore>("WulfrumExcavatorTire2").Type), NPC.scale);
                }
            }
        }
    }
}
