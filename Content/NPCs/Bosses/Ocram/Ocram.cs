using CalamityMod;
using CalamityMod.Prefixes.VanillaPrefixChanges;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.Content.NPCs.Bosses.Ocram
{
    public class Ocram : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            //TypeName = "Ocram";
            NPC.width = 100; // Ocram's dimensions should not be 100/110, but the programmer copied over the EoC's values without adjustment; they should be more like 195/155.
            NPC.height = 110;
            //NPC.aiStyle = 39; in his place... is giant tortoise...
            NPC.damage = 65;
            NPC.defense = 20;
            NPC.lifeMax = 35000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.timeLeft = 22500;
            NPC.boss = true;
            NPC.value = 100000f;
            NPC.npcSlots = 5f;
            NPC.buffImmune[BuffID.Poisoned] = true;
        }

        public override void AI()
        {
            Lighting.AddLight(new Vector2((int)NPC.position.X >> 4, (int)NPC.position.Y >> 4), new Vector3(1f, 1f, 1f));
            if (NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || Main.player[NPC.target].active == false)
            {
                NPC.TargetClosest();
            }
            bool dead = Main.player[NPC.target].dead;
            float num = NPC.position.X + (NPC.width >> 1) - Main.player[NPC.target].position.X - 10f;
            float num2 = NPC.position.Y + NPC.height - 59f - Main.player[NPC.target].position.Y - 21f;
            float num3 = (float)Math.Atan2(num2, num) + 1.57f;
            if (num3 < 0f)
            {
                num3 += 6.283f;
            }
            else if (num3 > 6.283f)
            {
                num3 -= 6.283f;
            }
            float num4 = 0f;
            if (NPC.ai[0] == 0f && NPC.ai[1] == 0f)
            {
                num4 = 0.02f;
            }
            if (NPC.ai[0] == 0f && NPC.ai[1] == 2f && NPC.ai[2] > 40f)
            {
                num4 = 0.05f;
            }
            if (NPC.ai[0] == 3f && NPC.ai[1] == 0f)
            {
                num4 = 0.05f;
            }
            if (NPC.ai[0] == 3f && NPC.ai[1] == 2f && NPC.ai[2] > 40f)
            {
                num4 = 0.08f;
            }
            if (NPC.rotation < num3)
            {
                if ((double)(num3 - NPC.rotation) > 3.1415)
                {
                    NPC.rotation -= num4;
                }
                else
                {
                    NPC.rotation += num4;
                }
            }
            else if (NPC.rotation > num3)
            {
                if ((double)(NPC.rotation - num3) > 3.1415)
                {
                    NPC.rotation += num4;
                }
                else
                {
                    NPC.rotation -= num4;
                }
            }
            if (NPC.rotation > num3 - num4 && NPC.rotation < num3 + num4)
            {
                NPC.rotation = num3;
            }
            if (NPC.rotation < 0f)
            {
                NPC.rotation += 6.283f;
            }
            else if (NPC.rotation > 6.283f)
            {
                NPC.rotation -= 6.283f;
            }
            if (NPC.rotation > num3 - num4 && NPC.rotation < num3 + num4)
            {
                NPC.rotation = num3;
            }
            if (Main.rand.Next(6) == 0)
            {
                /*
                //Dust* ptr = Main.DustSet.NewDust(NPC.position.X, NPC.position.Y + (NPC.height >> 2), NPC.width, NPC.height >> 1, 5, NPC.velocity.X, 2.0);
                Dust* ptr = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (NPC.height >> 2)), NPC.width, NPC.height >> 1, 5, NPC.velocity.X, 2);
                if (ptr != null)
                {
                    ptr->NPC.velocity.X *= 0.5f;
                    ptr->NPC.velocity.Y *= 0.1f;
                }
                */
            }
            if (Main.dayTime || dead)
            {
                NPC.velocity.Y -= 0.04f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                return;
            }
            if (NPC.ai[0] == 0f)
            {
                if (NPC.ai[1] == 0f)
                {
                    float num5 = 8f;
                    float num6 = 0.12f;
                    Vector2 vector = new Vector2(NPC.position.X + (NPC.width >> 1), NPC.position.Y + (NPC.height >> 1));
                    float num7 = Main.player[NPC.target].position.X + 10f - vector.X;
                    float num8 = Main.player[NPC.target].position.Y + 21f - 200f - vector.Y;
                    float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
                    float num10 = num9;
                    num9 = num5 / num9;
                    num7 *= num9;
                    num8 *= num9;
                    if (NPC.velocity.X < num7)
                    {
                        NPC.velocity.X += num6;
                        if (NPC.velocity.X < 0f && num7 > 0f)
                        {
                            NPC.velocity.X += num6;
                        }
                    }
                    else if (NPC.velocity.X > num7)
                    {
                        NPC.velocity.X -= num6;
                        if (NPC.velocity.X > 0f && num7 < 0f)
                        {
                            NPC.velocity.X -= num6;
                        }
                    }
                    if (NPC.velocity.Y < num8)
                    {
                        NPC.velocity.Y += num6;
                        if (NPC.velocity.Y < 0f && num8 > 0f)
                        {
                            NPC.velocity.Y += num6;
                        }
                    }
                    else if (NPC.velocity.Y > num8)
                    {
                        NPC.velocity.Y -= num6;
                        if (NPC.velocity.Y > 0f && num8 < 0f)
                        {
                            NPC.velocity.Y -= num6;
                        }
                    }
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] >= 600f)
                    {
                        NPC.ai[1] = 1f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.target = Main.maxPlayers;
                        NPC.netUpdate = true;
                    }
                    else if (NPC.position.Y + NPC.height < Main.player[NPC.target].position.Y && num10 < 500f)
                    {
                        if (!Main.player[NPC.target].dead)
                        {
                            NPC.ai[3] += 1f;
                        }
                        if (NPC.ai[3] >= 90f)
                        {
                            NPC.TargetClosest();
                            num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
                            num9 = 9f / num9;
                            num7 *= num9;
                            num8 *= num9;
                            vector.X += num7 * 15f;
                            vector.Y += num8 * 15f;
                            //Projectile.NewProjectile(vector.X, vector.Y, num7, num8, 100, 20, 0f);
                            Projectile.NewProjectile(NPC.GetBossSpawnSource(NPC.target), vector, new Vector2(num7, num8), ProjectileID.DeathLaser, 20, 0f);
                        }
                        if (NPC.ai[3] == 60f || NPC.ai[3] == 70f || NPC.ai[3] == 80f || NPC.ai[3] == 90f)
                        {
                            NPC.rotation = num3;
                            float num11 = Main.player[NPC.target].position.X + 10f - vector.X;
                            float num12 = Main.player[NPC.target].position.Y + 21f - vector.Y;
                            float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
                            num13 = 5f / num13;
                            Vector2 vector2 = vector;
                            Vector2 vector3 = default;
                            vector3.X = num11 * num13;
                            vector3.Y = num12 * num13;
                            vector2.X += vector3.X * 10f;
                            vector2.Y += vector3.Y * 10f;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int num14 = NPC.NewNPC(NPC.GetSource_FromThis(), (int)vector2.X, (int)vector2.Y, NPCID.ServantofCthulhu);
                                if (num14 < Main.maxNPCs)
                                {
                                    Main.npc[num14].velocity.X = vector3.X;
                                    Main.npc[num14].velocity.Y = vector3.Y;
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num14);
                                }
                            }
                            SoundEngine.PlaySound(SoundID.NPCHit1, vector2);
                            for (int i = 0; i < 8; i++)
                            {
                                if (null == Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f))
                                {
                                    break;
                                }
                            }
                        }
                        if (NPC.ai[3] == 103f)
                        {
                            NPC.ai[3] = 0f;
                        }
                    }
                }
                else if (NPC.ai[1] == 1f)
                {
                    NPC.rotation = num3;
                    float num15 = 6f;
                    Vector2 vector4 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                    float num16 = Main.player[NPC.target].position.X + 10f - vector4.X;
                    float num17 = Main.player[NPC.target].position.Y + 21f - vector4.Y;
                    float num18 = (float)Math.Sqrt(num16 * num16 + num17 * num17);
                    num18 = num15 / num18;
                    NPC.velocity.X = num16 * num18;
                    NPC.velocity.Y = num17 * num18;
                    NPC.ai[1] = 2f;
                }
                else if (NPC.ai[1] == 2f)
                {
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] >= 40f)
                    {
                        NPC.velocity.X *= 0.98f;
                        NPC.velocity.Y *= 0.98f;
                        if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                        {
                            NPC.velocity.X = 0f;
                        }
                        if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                        {
                            NPC.velocity.Y = 0f;
                        }
                    }
                    else
                    {
                        NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
                    }
                    if (NPC.ai[2] >= 150f)
                    {
                        NPC.ai[3] += 1f;
                        NPC.ai[2] = 0f;
                        NPC.target = Main.maxPlayers; // normally this would be 8 since thats the cap on players in console
                        NPC.rotation = num3;
                        if (NPC.ai[3] >= 3f)
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
                if (NPC.life < NPC.lifeMax >> 1)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.netUpdate = true;
                }
                return;
            }
            if (NPC.ai[0] == 1f || NPC.ai[0] == 2f)
            {
                if (NPC.ai[0] == 1f)
                {
                    NPC.ai[2] += 0.005f;
                    if (NPC.ai[2] > 0.5)
                    {
                        NPC.ai[2] = 0.5f;
                    }
                }
                else
                {
                    NPC.ai[2] -= 0.005f;
                    if (NPC.ai[2] < 0f)
                    {
                        NPC.ai[2] = 0f;
                    }
                }
                NPC.rotation += NPC.ai[2];
                NPC.ai[1] += 1f;
                if (NPC.ai[1] == 100f)
                {
                    NPC.ai[0] += 1f;
                    NPC.ai[1] = 0f;
                    if (NPC.ai[0] == 3f)
                    {
                        NPC.ai[2] = 0f;
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit1, NPC.position);
                        for (int j = 0; j < 2; j++)
                        {
                            Gore.NewGore(NPC.GetBossSpawnSource(NPC.target), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 174);
                            Gore.NewGore(NPC.GetBossSpawnSource(NPC.target), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 173);
                            Gore.NewGore(NPC.GetBossSpawnSource(NPC.target), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 172);
                        }
                        for (int k = 0; k < 16; k++)
                        {
                            if (null == Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f))
                            {
                                break;
                            }
                        }
                        SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                    }
                }
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                NPC.velocity.X *= 0.98f;
                NPC.velocity.Y *= 0.98f;
                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
                if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                {
                    NPC.velocity.Y = 0f;
                }
                return;
            }
            NPC.damage = 50;
            NPC.defense = 0;
            if (NPC.ai[1] == 0f)
            {
                float num19 = 9f;
                float num20 = 0.2f;
                Vector2 vector5 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float num21 = Main.player[NPC.target].position.X + 10f - vector5.X;
                float num22 = Main.player[NPC.target].position.Y + 21f - 120f - vector5.Y;
                float num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                num23 = num19 / num23;
                num21 *= num23;
                num22 *= num23;
                if (NPC.velocity.X < num21)
                {
                    NPC.velocity.X += num20;
                    if (NPC.velocity.X < 0f && num21 > 0f)
                    {
                        NPC.velocity.X += num20;
                    }
                }
                else if (NPC.velocity.X > num21)
                {
                    NPC.velocity.X -= num20;
                    if (NPC.velocity.X > 0f && num21 < 0f)
                    {
                        NPC.velocity.X -= num20;
                    }
                }
                if (NPC.velocity.Y < num22)
                {
                    NPC.velocity.Y += num20;
                    if (NPC.velocity.Y < 0f && num22 > 0f)
                    {
                        NPC.velocity.Y += num20;
                    }
                }
                else if (NPC.velocity.Y > num22)
                {
                    NPC.velocity.Y -= num20;
                    if (NPC.velocity.Y > 0f && num22 < 0f)
                    {
                        NPC.velocity.Y -= num20;
                    }
                }
                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= 100f)
                {
                    if (NPC.ai[2] >= 200f)
                    {
                        NPC.ai[1] = 1f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.target = Main.maxPlayers;
                        NPC.netUpdate = true;
                    }
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 9f / num23;
                    num21 *= num23;
                    num22 *= num23;
                    num21 += Main.rand.Next(-40, 41) * 0.08f;
                    num22 += Main.rand.Next(-40, 41) * 0.08f;
                    vector5.X += num21 * 15f;
                    vector5.Y += num22 * 15f;
                    Projectile.NewProjectile(NPC.GetBossSpawnSource(NPC.target), vector5, new Vector2(num21, num22), ProjectileID.EyeLaser, 45, 0f);
                }
            }
            else if (NPC.ai[1] == 1f)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                NPC.rotation = num3;
                float num24 = 6.8f;
                Vector2 vector6 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                float num25 = Main.player[NPC.target].position.X + 10f - vector6.X;
                float num26 = Main.player[NPC.target].position.Y + 21f - vector6.Y;
                float num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
                num27 = num24 / num27;
                NPC.velocity.X = num25 * num27;
                NPC.velocity.Y = num26 * num27;
                if (NPC.ai[1] == 1f)
                {
                    num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
                    num27 = 6f / num27;
                    num25 *= num27;
                    num26 *= num27;
                    num25 += Main.rand.Next(-40, 41) * 0.08f;
                    num26 += Main.rand.Next(-40, 41) * 0.08f;
                    for (int l = 1; l <= 10; l++)
                    {
                        vector6.X += Main.rand.Next(-50, 50) * 2f;
                        vector6.Y += Main.rand.Next(-50, 50) * 2f;
                        Projectile.NewProjectile(NPC.GetBossSpawnSource(NPC.target), vector6, new Vector2(num25, num26), ProjectileID.DemonSickle, 45, 0f);
                    }
                }
                NPC.ai[1] = 2f;
            }
            else
            {
                if (NPC.ai[1] != 2f)
                {
                    return;
                }
                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= 40f)
                {
                    NPC.velocity.X *= 1f;
                    NPC.velocity.Y *= 1f;
                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                    {
                        NPC.velocity.X = 0f;
                    }
                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                    {
                        NPC.velocity.Y = 0f;
                    }
                    if (NPC.ai[2] >= 135f)
                    {
                        NPC.ai[3] += 1f;
                        NPC.ai[2] = 0f;
                        NPC.target = Main.maxPlayers;
                        NPC.rotation = num3;
                        if (NPC.ai[3] >= 3f)
                        {
                            NPC.ai[1] = 0f;
                            NPC.ai[3] = 0f;
                        }
                        else
                        {
                            NPC.ai[1] = 1f;
                        }
                    }
                    if (NPC.ai[2] != 110f && NPC.ai[2] != 100f && NPC.ai[2] != 130f && NPC.ai[2] != 120f)
                    {
                        return;
                    }
                    NPC.rotation = num3;
                    Vector2 vector7 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                    float num28 = Main.player[NPC.target].position.X + 10f - vector7.X;
                    float num29 = Main.player[NPC.target].position.Y + 21f - vector7.Y;
                    float num30 = (float)Math.Sqrt(num28 * num28 + num29 * num29);
                    num30 = 5f / num30;
                    Vector2 vector8 = vector7;
                    Vector2 vector9 = default;
                    vector9.X = num28 * num30;
                    vector9.Y = num29 * num30;
                    vector8.X += vector9.X * 10f;
                    vector8.Y += vector9.Y * 10f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int num31 = NPC.NewNPC(NPC.GetSource_FromThis(), (int)vector8.X, (int)vector8.Y, NPCID.ServantofCthulhu);
                        if (num31 < Main.maxNPCs)
                        {
                            Main.npc[num31].velocity.X = vector9.X;
                            Main.npc[num31].velocity.Y = vector9.Y;
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num31);
                        }
                    }
                }
                else
                {
                    NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if ((NPC.frameCounter += 1f) < 7f)
            {
                NPC.frame.Y = 0;
            }
            else if (NPC.frameCounter < 14f)
            {
                NPC.frame.Y = frameHeight;
            }
            else if (NPC.frameCounter < 21f)
            {
                NPC.frame.Y = (short)(frameHeight << 1);
            }
            else
            {
                NPC.frameCounter = 0f;
                NPC.frame.Y = 0;
            }
            if (NPC.ai[0] > 1f)
            {
                NPC.frame.Y = (short)(NPC.frame.Y + frameHeight * 3);

            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (NPC.life > 0)
            {
                int num52 = (int)(damageDone / NPC.lifeMax * 80.0);
                while (num52 > 0 && null != Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1))
                {
                    num52--;
                }
                return;
            }
            for (int num53 = 0; num53 < 128; num53++)
            {
                if (null == Dust.NewDust(NPC.position, NPC.width, NPC.height, hit.HitDirection << 1, -2))
                {
                    break;
                }
            }
            for (int num54 = 0; num54 < 2; num54++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 172);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 173);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 174);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 172);
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
            Vector2 origin = new Vector2(texture.Width / 2, (texture.Height / Main.npcFrameCount[NPC.type]) / 2);
            origin.Y *= 0.5f;
            int frame = NPC.frame.Y / frameHeight;

            int width = texture.Width;
            Vector2 pivot = default;
            pivot.X = width >> 1;
            pivot.Y = frameHeight >> 1;
            pivot.Y *= 0.5f;
            Vector2 pos = new Vector2(NPC.position.X - screenPos.X + (NPC.width >> 1) - width * NPC.scale * 0.5f + pivot.X * NPC.scale, NPC.position.Y - screenPos.Y + NPC.height - frameHeight * NPC.scale + 4f + pivot.Y * NPC.scale);

            spriteBatch.Draw(texture, pos, texture.Frame(1, Main.npcFrameCount[NPC.type], 0, frame), drawColor, NPC.rotation, pivot, NPC.scale, SpriteEffects.None, 0);
            //spriteBatch.Draw(texture, NPC.Center - screenPos, texture.Frame(1, Main.npcFrameCount[NPC.type], 0, frame), drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
