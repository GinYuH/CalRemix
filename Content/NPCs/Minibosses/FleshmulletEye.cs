using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class FleshmulletEye : ModNPC
    {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.WallofFleshEye;
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            Main.npcFrameCount[Type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 200;
            NPC.damage = 50;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
        }
        public override void AI()
        {
            CalamityMod.NPCs.VanillaNPCAIOverrides.Bosses.TwinsAI.BuffedRetinazerAI(NPC, Mod);
            NPC.dontTakeDamage = true;
            if (!NPC.AnyNPCs(ModContent.NPCType<Fleshmullet>()))
            {
                NPC.active = false;
            }
            NPC.rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() - MathHelper.Pi;
            float pushVelocity = 0.5f;
            foreach (var n in Main.ActiveNPCs)
            {
                if (n.whoAmI != NPC.whoAmI && n.type == NPC.type)
                {
                    if (Vector2.Distance(NPC.Center, n.Center) < 160f)
                    {
                        if (NPC.position.X < n.position.X)
                            NPC.velocity.X -= pushVelocity;
                        else
                            NPC.velocity.X += pushVelocity;

                        if (NPC.position.Y < n.position.Y)
                            NPC.velocity.Y -= pushVelocity;
                        else
                            NPC.velocity.Y += pushVelocity;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y > frameHeight)
            {
                NPC.frame.Y = 0;
            }
        }
    }
}
