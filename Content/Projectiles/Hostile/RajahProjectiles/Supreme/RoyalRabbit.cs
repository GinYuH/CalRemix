using CalRemix.Content.NPCs.Bosses.RajahBoss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile.RajahProjectiles.Supreme
{
	public class RoyalRabbit : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Royal Rabbit");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
        }

        public override void AI()
        {
			bool flag64 = Projectile.type == Mod.Find<ModProjectile>("RoyalRabbit").Type;
			Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.RabbitcopterR = false;
                }
                if (modPlayer.RabbitcopterR)
                {
                    Projectile.timeLeft = 2;
                }
            }
            float num9 = Projectile.width;
            float num8 = 0.1f;
            num9 *= 2f;
            for (int j = 0; j < 1000; j++)
            {
                if (j != Projectile.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == Projectile.owner && Main.projectile[j].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[j].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[j].position.Y) < num9)
                {
                    if (Projectile.position.X < Main.projectile[j].position.X)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - num8;
                    }
                    else
                    {
                        Projectile.velocity.X = Projectile.velocity.X + num8;
                    }
                    if (Projectile.position.Y < Main.projectile[j].position.Y)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - num8;
                    }
                    else
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + num8;
                    }
                }
            }
            Vector2 vector = Projectile.position;
            float num10 = 400f;
            bool flag = false;
            int num11 = -1;
            Projectile.tileCollide = false;
            NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this, false))
            {
                float num14 = Vector2.Distance(ownerMinionAttackTargetNPC2.Center, Projectile.Center);
                if (((Vector2.Distance(Projectile.Center, vector) > num14 && num14 < num10) || !flag) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                {
                    num10 = num14;
                    vector = ownerMinionAttackTargetNPC2.Center;
                    flag = true;
                    num11 = ownerMinionAttackTargetNPC2.whoAmI;
                }
            }
            if (!flag)
            {
                for (int l = 0; l < 200; l++)
                {
                    NPC nPC2 = Main.npc[l];
                    if (nPC2.CanBeChasedBy(this, false))
                    {
                        float num15 = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (((Vector2.Distance(Projectile.Center, vector) > num15 && num15 < num10) || !flag) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC2.position, nPC2.width, nPC2.height))
                        {
                            num10 = num15;
                            vector = nPC2.Center;
                            flag = true;
                            num11 = l;
                        }
                    }
                }
            }
            int num16 = 500;
            if (flag)
            {
                num16 = 1000;
            }
            float num17 = Vector2.Distance(player.Center, Projectile.Center);
            if (num17 > num16)
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 1f)
            {
                Projectile.tileCollide = false;
            }
            if (flag && Projectile.ai[0] == 0f)
            {
                Vector2 vector4 = vector - Projectile.Center;
                float num18 = vector4.Length();
                vector4.Normalize();
                if (num18 < 150f)
                {
                    float num21 = 4f;
                    vector4 *= -num21;
                    Projectile.velocity.X = (Projectile.velocity.X * 40f + vector4.X) / 41f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 40f + vector4.Y) / 41f;
                }
                else
                {
                    Projectile.velocity *= 0.97f;
                }
            }
            else
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
                {
                    Projectile.ai[0] = 1f;
                }
                float num22 = 6f;
                if (Projectile.ai[0] == 1f)
                {
                    num22 = 15f;
                }
                Vector2 center2 = Projectile.Center;
                Projectile.ai[1] = 3600f;
                Projectile.netUpdate = true;
                Vector2 vector6 = player.Center - center2;
                int num23 = 1;
                for (int m = 0; m < Projectile.whoAmI; m++)
                {
                    if (Main.projectile[m].active && Main.projectile[m].owner == Projectile.owner && Main.projectile[m].type == Projectile.type)
                    {
                        num23++;
                    }
                }
                vector6.X -= 10 * Main.player[Projectile.owner].direction;
                vector6.X -= num23 * 40 * Main.player[Projectile.owner].direction;
                vector6.Y -= 10f;
                float num24 = vector6.Length();
                if (num24 > 200f && num22 < 9f)
                {
                    num22 = 9f;
                }
                num22 = (int)(num22 * 0.75);
                if (num24 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                }
                if (num24 > 2000f)
                {
                    Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width / 2;
                    Projectile.position.Y = Main.player[Projectile.owner].Center.Y - Projectile.width / 2;
                }
                if (num24 > 10f)
                {
                    vector6.Normalize();
                    if (num24 < 50f)
                    {
                        num22 /= 2f;
                    }
                    vector6 *= num22;
                    Projectile.velocity = (Projectile.velocity * 20f + vector6) / 21f;
                }
                else
                {
                    Projectile.direction = Main.player[Projectile.owner].direction;
                    Projectile.velocity *= 0.9f;
                }
            }
            Projectile.rotation = Projectile.velocity.X * 0.05f;
            if (Projectile.velocity.X > 0f)
            {
                Projectile.spriteDirection = Projectile.direction = -1;
            }
            else if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = Projectile.direction = 1;
            }
            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] += 1f;
                if (Main.rand.Next(3) == 0)
                {
                    Projectile.ai[1] += 1f;
                }
            }
            if (Projectile.ai[1] > Main.rand.Next(180, 900))
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 0f)
            {
                float scaleFactor4 = 11f;
                int num29 = ModContent.ProjectileType<RabbitBeam>();
                
                if (flag)
                {
                    if ((vector - Projectile.Center).X > 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
                    else if ((vector - Projectile.Center).X < 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = 1;
                    }
                    if (Projectile.ai[1] == 0f)
                    {
                        Projectile.ai[1] += 1f;
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Vector2 value4 = vector - Projectile.Center;
                            value4.Normalize();
                            value4 *= scaleFactor4;
                            int num33 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, value4.X*2, value4.Y*2, num29, Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[num33].timeLeft = 300;
                            Main.projectile[num33].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
			if (++Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 4, 0, 0);
            BaseDrawing.DrawTexture(Main.spriteBatch, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, Projectile.direction, 4, frame, lightColor, false);
            BaseDrawing.DrawTexture(Main.spriteBatch, ModContent.Request<Texture2D>(Texture + "_Glow").Value, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, Projectile.direction, 4, frame, Main.DiscoColor, false);
            return false;
        }
    }

    internal class RabbitBeam : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rabbit Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minion = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 100;
            Projectile.aiStyle = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            for (int num443 = 0; num443 < 2; num443++)
            {
                Vector2 vector31 = Projectile.position;
                vector31 -= Projectile.velocity * (num443 * 0.25f);
                Projectile.alpha = 255;
                int num444 = Dust.NewDust(vector31, 1, 1, DustID.Shadowflame, 0f, 0f, 0, Main.DiscoColor);
                Main.dust[num444].position = vector31;
                Dust expr_13D2C_cp_0 = Main.dust[num444];
                expr_13D2C_cp_0.position.X += Projectile.width / 2;
                Dust expr_13D50_cp_0 = Main.dust[num444];
                expr_13D50_cp_0.position.Y += Projectile.height / 2;
                Main.dust[num444].scale = Main.rand.Next(70, 110) * 0.013f;
                Main.dust[num444].velocity *= 0.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float num13 = 10f + 3f * 4f;
            Vector2 value6 = new Vector2(1.05f, 1f);
            Vector2 spinningpoint = new Vector2(0f, -3f - 3f).RotatedByRandom(3.1415927410125732);
            for (float num14 = 0f; num14 < num13; num14 += 1f)
            {
                int num15 = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 0, Color.Transparent, 1f);
                Main.dust[num15].position = Projectile.Center;
                Main.dust[num15].velocity = spinningpoint.RotatedBy(6.28318548f * num14 / num13) * value6 * (0.8f + Main.rand.NextFloat() * 0.4f);
                Main.dust[num15].color = Main.hslToRgb(num14 / num13, 1f, 0.5f);
                Main.dust[num15].noGravity = true;
                Main.dust[num15].scale = 1f + 3 / 3f;
            }
            target.AddBuff(ModContent.BuffType<Buffs.InfinityOverload>(), 200);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int num585 = 0; num585 < 20; num585++)
            {
                int num586 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.RainbowRod, 0f, 0f, 100, Main.DiscoColor, 2f);
                Main.dust[num586].noGravity = true;
                Main.dust[num586].velocity *= 4f;
            }
        }
    }
}