#region thecord
/* using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class TheLeashTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            DisplayName.SetDefault("The Leash");
        }

        public override void SetDefaults()
        {
            TheLeashBody.InitializeSegment(NPC);
            NPC.Size = new(20, 30);
            NPC.damage = 65;
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

        public override void AI() => TheLeashBody.PerformSegmentingAI(NPC);

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