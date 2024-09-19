using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs
{
    public class PlaguedFirefly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plagued Firefly");
            Main.npcFrameCount[NPC.type] = 12;
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 8;
            NPC.height = 8;
            NPC.aiStyle = -1;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.npcSlots = 0.5f;
            NPC.catchItem = (short)ModContent.ItemType<Items.Critters.PlaguedFirefly>();
            NPC.noGravity = true;
            NPC.friendly = true;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public void SyncWithOtherFireflies()
        {
            int closestFirefly = -1;
            float distance = 600f;
            for (int i = NPC.whoAmI + 1; i < Main.maxNPCs; i++) // having it start at the whoAmI value prevents gravitation
            {
                if (Main.npc[i].active && Main.npc[i].type == NPC.type)
                {
                    float dist = Main.npc[i].Distance(NPC.Center);
                    if (dist < distance)
                    {
                        distance = dist;
                        closestFirefly = i;
                    }
                }
            }
            if (closestFirefly != -1)
            {
                if (Main.npc[closestFirefly].localAI[0] < 180 && Main.npc[closestFirefly].localAI[0] != NPC.localAI[0] && Main.rand.NextBool(3))
                {
                    NPC.localAI[0] = MathHelper.Lerp(NPC.localAI[0], Main.npc[closestFirefly].localAI[0], 0.6f);
                }
                if (distance < 60)
                {
                    if (Main.rand.NextBool(4))
                    {
                        NPC.ai[0] = MathHelper.Lerp(NPC.ai[0], Main.npc[closestFirefly].ai[0], 0.6f);
                        NPC.ai[1] = MathHelper.Lerp(NPC.ai[1], Main.npc[closestFirefly].ai[1], 0.6f);
                    }
                }
                else
                {
                    if (Main.rand.NextBool(30))
                    {
                        var velo = Vector2.Normalize(Main.npc[closestFirefly].Center - NPC.Center) * Main.npc[closestFirefly].velocity.Length();
                        NPC.ai[0] = MathHelper.Lerp(NPC.ai[0], velo.X, 0.6f);
                        NPC.ai[1] = MathHelper.Lerp(NPC.ai[1], velo.Y, 0.6f);
                    }
                }
            }
        }

        public bool UpdateEnemyFleeing()
        {
            int closestEnemy = -1;
            float distance = 200f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].damage > 0 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                {
                    float dist = Main.npc[i].Distance(NPC.Center);
                    if (dist < distance)
                    {
                        distance = dist;
                        closestEnemy = i;
                    }
                }
            }
            if (closestEnemy != -1)
            {
                var velo = Vector2.Normalize(NPC.Center - Main.npc[closestEnemy].Center) * 3f;
                NPC.ai[0] = velo.X;
                NPC.ai[1] = velo.Y;
                return true;
            }
            return false;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[3] = Main.rand.NextFloat(120f, 600f);
                NPC.ai[2] = Main.rand.NextFloat(0.01f, 0.1f);
                for (int i = 0; i < 10; i++)
                {
                    NPC.ai[0] = Main.rand.NextFloat(-1.6f, 1.6f);
                    NPC.ai[1] = Main.rand.NextFloat(-1.6f, 1.6f);
                    var velo = new Vector2(NPC.ai[0], NPC.ai[1]);
                    if (velo.Length() > 1f)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (!UpdateEnemyFleeing())
                {
                    NPC.ai[3]--;
                    if (NPC.ai[3] <= 0f)
                    {
                        NPC.ai[0] = 0f;
                    }
                }
                if (NPC.collideX && NPC.velocity.X != NPC.oldVelocity.X)
                {
                    NPC.ai[0] = -NPC.ai[0] * 0.8f;
                }
                if (NPC.collideY && NPC.velocity.Y != NPC.oldVelocity.Y)
                {
                    NPC.ai[1] = -NPC.ai[1] * 0.8f;
                }
            }
            NPC.localAI[0]++;
            if ((int)NPC.localAI[0] < 180)
            {
                SyncWithOtherFireflies();
            }
            else
            {
                Lighting.AddLight(NPC.Center, new Vector3(0.6f, 1f, 0.1f));
                if ((int)NPC.localAI[0] > 480)
                {
                    NPC.localAI[0] = 0f;
                }
            }
            NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(NPC.ai[0], NPC.ai[1]), NPC.ai[2]);
            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }
        }

        public override void FindFrame(int frameHeight)
        { 
            if ((int)NPC.localAI[0] > 180)
            {
                if (NPC.frame.Y < frameHeight * 6)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = frameHeight * 6;
                }
                else
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 2)
                    {
                        NPC.frameCounter = 0.0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                        {
                            NPC.frame.Y = frameHeight * 6;
                        }
                    }
                }
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 2)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= frameHeight * 6)
                    {
                        NPC.frame.Y = 0;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = TextureAssets.Npc[NPC.type].Value;
            var screenPose = Main.screenPosition;
            var drawPos = NPC.Center - screenPos;
            var origin = NPC.frame.Size() / 2f;
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRemix/Content/NPCs/PlaguedFirefly_Aura").Value, drawPos, NPC.frame, new Color(255, 255, 255, 100), NPC.rotation, origin, NPC.scale, effects, 0f);
            return false;
        }
    }
}