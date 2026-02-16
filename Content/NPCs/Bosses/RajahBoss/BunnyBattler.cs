using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using CalamityMod;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class BunnyBattler : ModNPC
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/RajahBoss/BunnyBattler";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rabbid Rabbit");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.damage = 90;
            NPC.defense = 30;
            NPC.lifeMax = 300;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            bool isDead = NPC.life <= 0;
            if (isDead)          //this make so when the npc has 0 life(dead) he will spawn this
            {

            }
            for (int m = 0; m < (isDead ? 35 : 6); m++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (NPC.velocity.Y != 0)
            {
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = -1;
                }
                else if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
            }
            else
            {
                if (player.position.X < NPC.position.X)
                {
                    NPC.spriteDirection = -1;
                }
                else if (player.position.X > NPC.position.X)
                {
                    NPC.spriteDirection = 1;
                }
            }
            AISlime(NPC, ref NPC.ai, false, 25, 6f, -8f, 6f, -10f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y < 0)
            {
                NPC.frame.Y = frameHeight * 4;
            }
            else if (NPC.velocity.Y > 0)
            {
                NPC.frame.Y = frameHeight * 5;
            }
            else if (NPC.ai[0] < -15f)
            {
                NPC.frame.Y = 0;
            }
            else if (NPC.ai[0] > -15f)
            {
                NPC.frame.Y = frameHeight;
            }
            else if (NPC.ai[0] > -10f)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (NPC.ai[0] > -5f)
            {
                NPC.frame.Y = frameHeight * 3;
            }
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void PostAI()
        {
            for (int m = NPC.oldPos.Length - 1; m > 0; m--)
            {
                NPC.oldPos[m] = NPC.oldPos[m - 1];
            }
            NPC.oldPos[0] = NPC.position;

            if (NPC.AnyNPCs(ModContent.NPCType<Rajah>()) ||
                NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 5;
                }
                else
                {
                    NPC.alpha = 0;
                }
            }
            else
            {
                NPC.dontTakeDamage = true;
                if (NPC.alpha < 255)
                {
                    NPC.alpha += 5;
                }
                else
                {
                    NPC.active = false;
                }
            }
        }

        public static void AISlime(NPC npc, ref float[] ai, bool fleeWhenDay = false, int jumpTime = 200, float jumpVelX = 2f, float jumpVelY = -6f, float jumpVelHighX = 3f, float jumpVelHighY = -8f)
        {
            //ai[0] is a timer that iterates after the npc has jumped. If it is >= 0, the npc will attempt to jump again.
            //ai[1] is used to determine what jump type to do. (if 2, large jump, else smaller jump.)
            //ai[2] is used for target updating. 
            //ai[3] is used to house the landing position of the npc for bigger jumps. This is used to make it turn around when it hits
            //      an impassible wall.

            //if (jumpTime < 100) { jumpTime = 100; }
            bool getNewTarget = false; //getNewTarget is used to iterate the 'boredom' scale. If it's night, the npc is hurt, or it's
            //below a certain depth, it will attempt to find the nearest target to it.
            if ((fleeWhenDay && !Main.dayTime) || npc.life != npc.lifeMax || (double)npc.position.Y > Main.worldSurface * 16.0)
            {
                getNewTarget = true;
            }
            if (ai[2] > 1f) { ai[2] -= 1f; }
            if (npc.wet)//if the npc is wet...
            {
                //handles the npc's 'bobbing' in water.
                if (npc.collideY) { npc.velocity.Y = -2f; }
                if (npc.velocity.Y < 0f && ai[3] == npc.position.X) { npc.direction *= -1; ai[2] = 200f; }
                if (npc.velocity.Y > 0f) { ai[3] = npc.position.X; }
                if (npc.velocity.Y > 2f) { npc.velocity.Y = npc.velocity.Y * 0.9f; }
                npc.velocity.Y = npc.velocity.Y - 0.5f;
                if (npc.velocity.Y < -4f) { npc.velocity.Y = -4f; }
                //if ai[2] is 1f, and we should get a target, target nearby players.
                if (ai[2] == 1f && getNewTarget) { npc.TargetClosest(true); }
            }
            npc.aiAction = 0;
            //if ai[2] is 0f (just spawned)
            if (ai[2] == 0f)
            {
                ai[0] = -100f;
                ai[2] = 1f;
                npc.TargetClosest(true);
            }
            //if npc is not jumping or falling
            if (npc.velocity.Y == 0f)
            {
                if (ai[3] == npc.position.X) { npc.direction *= -1; ai[2] = 200f; }
                ai[3] = 0f;
                npc.velocity.X = npc.velocity.X * 0.8f;
                if (npc.velocity.X > -0.1f && npc.velocity.X < 0.1f) { npc.velocity.X = 0f; }
                if (getNewTarget) { ai[0] += 1f; }
                ai[0] += 1f;
                if (ai[0] >= 0f)
                {
                    npc.netUpdate = true;
                    if (ai[2] == 1f && getNewTarget) { npc.TargetClosest(true); }
                    if (ai[1] == 2f) //larger jump
                    {
                        npc.velocity.Y = jumpVelHighY;
                        npc.velocity.X += jumpVelHighX * npc.direction;
                        ai[0] = -jumpTime;
                        ai[1] = 0f;
                        ai[3] = npc.position.X;
                    }
                    else //smaller jump
                    {
                        npc.velocity.Y = jumpVelY;
                        npc.velocity.X += jumpVelX * npc.direction;
                        ai[0] = -jumpTime - 80f;
                        ai[1] += 1f;
                    }
                }
                else
                if (ai[0] >= -30f) { npc.aiAction = 1; return; }
            }
            else //handle moving the npc while in air.
            if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
            {
                if ((npc.direction == -1 && (double)npc.velocity.X < 0.1) || (npc.direction == 1 && (double)npc.velocity.X > -0.1))
                {
                    npc.velocity.X = npc.velocity.X + 0.2f * (float)npc.direction;
                    return;
                }
                npc.velocity.X = npc.velocity.X * 0.93f;
                return;
            }
        }
    }

    public class BunnyBattler2 : BunnyBattler
    {
        public override string Texture => "CalRemix/Content/NPCs/Bosses/RajahBoss/BunnyBattler";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 150;
            NPC.defense = 70;
            NPC.lifeMax = 1200;
        }
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage /= 2;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                BaseDrawing.DrawAfterimage(spriteBatch, TextureAssets.Npc[NPC.type].Value, 0, NPC, 1f, 1f, 10, true, 0f, 0f, CalamityUtils.MulticolorLerp(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Green, Color.Blue, Color.Red));
            }
            return false;
        }
    }
}