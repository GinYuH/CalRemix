#region thecord
/* using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using CalRemix.Content.Buffs;
using CalRemix.UI.ElementalSystem;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class TheLeashBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            DisplayName.SetDefault("The Leash");
        }

        public override void SetDefaults() => InitializeSegment(NPC);


    public static void InitializeSegment(NPC npc)
        {
            npc.width = 20;
            npc.height = 30;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit8;
            npc.DeathSound = SoundID.NPCDeath12;
            npc.LifeMaxNERB(55000, 66500, 800000);
            npc.defense = 30;
            npc.knockBackResist = 0f;
            npc.Calamity().VulnerableToHeat = true;
            npc.Calamity().VulnerableToSickness = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.damage = 65;
            npc.aiStyle = -1;
            npc.boss = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.chaseable);
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
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

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

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