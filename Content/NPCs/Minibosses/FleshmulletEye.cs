using CalamityMod;
using CalRemix.Content.Items.Bags;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

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
            RetinazerAI();
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

        public void RetinazerAI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            bool dead2 = Main.player[NPC.target].dead;
            float num480 = NPC.position.X + (float)(NPC.width / 2) - Main.player[NPC.target].position.X - (float)(Main.player[NPC.target].width / 2);
            float num481 = NPC.position.Y + (float)NPC.height - 59f - Main.player[NPC.target].position.Y - (float)(Main.player[NPC.target].height / 2);
            float num482 = (float)Math.Atan2(num481, num480) + 1.57f;
            if (num482 < 0f)
            {
                num482 += 6.283f;
            }
            else if ((double)num482 > 6.283)
            {
                num482 -= 6.283f;
            }
            float num483 = 0.1f;
            if (NPC.rotation < num482)
            {
                if ((double)(num482 - NPC.rotation) > 3.1415)
                {
                    NPC.rotation -= num483;
                }
                else
                {
                    NPC.rotation += num483;
                }
            }
            else if (NPC.rotation > num482)
            {
                if ((double)(NPC.rotation - num482) > 3.1415)
                {
                    NPC.rotation += num483;
                }
                else
                {
                    NPC.rotation -= num483;
                }
            }
            if (NPC.rotation > num482 - num483 && NPC.rotation < num482 + num483)
            {
                NPC.rotation = num482;
            }
            if (NPC.rotation < 0f)
            {
                NPC.rotation += 6.283f;
            }
            else if ((double)NPC.rotation > 6.283)
            {
                NPC.rotation -= 6.283f;
            }
            if (NPC.rotation > num482 - num483 && NPC.rotation < num482 + num483)
            {
                NPC.rotation = num482;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 val41 = new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f);
                int num484 = NPC.width;
                int num485 = (int)((float)NPC.height * 0.5f);
                float x4 = NPC.velocity.X;
                Color newColor = default(Color);
                int num486 = Dust.NewDust(val41, num484, num485, 5, x4, 2f, 0, newColor);
                Main.dust[num486].velocity.X *= 0.5f;
                Main.dust[num486].velocity.Y *= 0.1f;
            }
            Vector2 vector41 = Vector2.Zero;
            NPC.reflectsProjectiles = false;
            if (NPC.ai[0] == 0f)
            {
                if (NPC.ai[1] == 0f)
                {
                    float num489 = 7f;
                    float num490 = 0.1f;
                    if (Main.expertMode)
                    {
                        num489 = 8.25f;
                        num490 = 0.115f;
                    }
                    if (Main.getGoodWorld)
                    {
                        num489 *= 1.15f;
                        num490 *= 1.15f;
                    }
                    int num491 = 1;
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width)
                    {
                        num491 = -1;
                    }
                    Vector2 vector43 = default(Vector2);
                    vector43 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    float num492 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) + (float)(num491 * 300) - vector43.X;
                    float num493 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 300f - vector43.Y;
                    float num494 = (float)Math.Sqrt(num492 * num492 + num493 * num493);
                    float num495 = num494;
                    num494 = num489 / num494;
                    num492 *= num494;
                    num493 *= num494;
                    if (NPC.velocity.X < num492)
                    {
                        NPC.velocity.X += num490;
                        if (NPC.velocity.X < 0f && num492 > 0f)
                        {
                            NPC.velocity.X += num490;
                        }
                    }
                    else if (NPC.velocity.X > num492)
                    {
                        NPC.velocity.X -= num490;
                        if (NPC.velocity.X > 0f && num492 < 0f)
                        {
                            NPC.velocity.X -= num490;
                        }
                    }
                    if (NPC.velocity.Y < num493)
                    {
                        NPC.velocity.Y += num490;
                        if (NPC.velocity.Y < 0f && num493 > 0f)
                        {
                            NPC.velocity.Y += num490;
                        }
                    }
                    else if (NPC.velocity.Y > num493)
                    {
                        NPC.velocity.Y -= num490;
                        if (NPC.velocity.Y > 0f && num493 < 0f)
                        {
                            NPC.velocity.Y -= num490;
                        }
                    }
                    int num497 = 600;
                    int num498 = 60;
                    NPC.ai[2]++;
                    if (NPC.ai[2] >= (float)num497)
                    {
                        NPC.ai[1] = 1f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.target = 255;
                        NPC.netUpdate = true;
                    }
                    else if (NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && num495 < 400f)
                    {
                        if (!Main.player[NPC.target].dead)
                        {
                            NPC.ai[3]++;
                        }
                        if (NPC.ai[3] >= (float)num498)
                        {
                            NPC.ai[3] = 0f;
                            vector43 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                            num492 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector43.X;
                            num493 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector43.Y;
                            if (Main.netMode != 1)
                            {
                                float num499 = 9f;
                                int attackDamage_ForProjectiles3 = NPC.GetAttackDamage_ForProjectiles(20f, 19f);
                                int num500 = 83;
                                if (Main.expertMode)
                                {
                                    num499 = 10.5f;
                                }
                                num494 = (float)Math.Sqrt(num492 * num492 + num493 * num493);
                                num494 = num499 / num494;
                                num492 *= num494;
                                num493 *= num494;
                                num492 += (float)Main.rand.Next(-40, 41) * 0.08f;
                                num493 += (float)Main.rand.Next(-40, 41) * 0.08f;
                                vector43.X += num492 * 15f;
                                vector43.Y += num493 * 15f;
                                int num501 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector43.X, vector43.Y, num492, num493, num500, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                            }
                        }
                    }
                }
                else if (NPC.ai[1] == 1f)
                {
                    NPC.rotation = num482;
                    float num502 = 12f;
                    if (Main.expertMode)
                    {
                        num502 = 15f;
                    }
                    if (Main.getGoodWorld)
                    {
                        num502 += 2f;
                    }
                    Vector2 vector44 = default(Vector2);
                    vector44 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    float num503 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector44.X;
                    float num504 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector44.Y;
                    float num505 = (float)Math.Sqrt(num503 * num503 + num504 * num504);
                    num505 = num502 / num505;
                    NPC.velocity.X = num503 * num505;
                    NPC.velocity.Y = num504 * num505;
                    NPC.ai[1] = 2f;
                }
                else if (NPC.ai[1] == 2f)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] >= 25f)
                    {
                        NPC.velocity.X *= 0.96f;
                        NPC.velocity.Y *= 0.96f;
                        if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                        {
                            NPC.velocity.X = 0f;
                        }
                        if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
                        {
                            NPC.velocity.Y = 0f;
                        }
                    }
                    else
                    {
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
                    }
                    if (NPC.ai[2] >= 70f)
                    {
                        NPC.ai[3]++;
                        NPC.ai[2] = 0f;
                        NPC.target = 255;
                        NPC.rotation = num482;
                        if (NPC.ai[3] >= 4f)
                        {
                            NPC.ai[1] = 0f;
                            NPC.ai[3] = 0f;
                        }
                        else
                        {
                            NPC.ai[1] = 1f;
                        }
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
