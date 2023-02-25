using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System;
using Terraria.DataStructures;

namespace CalRemix.Projectiles.Weapons
{
	public class TotalityEnergy : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Summon/CosmicEnergySpiral";
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exo Rammer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = 100f;
        }
        public override void AI()
        {
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, (float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f);
            bool projType = Projectile.type == ModContent.ProjectileType<TotalityEnergy>();
            if (projType)
                if (Owner.dead)
                    Projectile.Kill();
            float num2 = 1400f;
            float num3 = 1600f;
            float num4 = 2400f;
            float num5 = 800f;
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Vector2 vector = Projectile.position;
            bool flag = false;
            int num6 = 0;
            if (Owner.HasMinionAttackTargetNPC)
            {
                NPC nPC = Main.npc[Owner.MinionAttackTargetNPC];
                if (nPC.CanBeChasedBy(Projectile))
                {
                    float num7 = Vector2.Distance(nPC.Center, Projectile.Center);
                    if (!flag && num7 < num2)
                    {
                        vector = nPC.Center;
                        flag = true;
                        num6 = nPC.whoAmI;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC nPC2 = Main.npc[i];
                    if (nPC2.CanBeChasedBy(Projectile))
                    {
                        float num8 = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (!flag && num8 < num2)
                        {
                            num2 = num8;
                            vector = nPC2.Center;
                            flag = true;
                            num6 = i;
                        }
                    }
                }
            }

            float num9 = num3;
            if (flag)
            {
                num9 = num4;
            }

            if (Vector2.Distance(Owner.Center, Projectile.Center) > num9)
            {
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }

            if (flag && Projectile.ai[1] == 0f)
            {
                Vector2 vector2 = vector - Projectile.Center;
                float num10 = vector2.Length();
                vector2.Normalize();
                if (num10 > 200f)
                {
                    float num11 = 6f;
                    vector2 *= num11;
                    Projectile.velocity = (Projectile.velocity * 40f + vector2) / 41f;
                }
                else
                {
                    float num12 = 4f;
                    vector2 *= 0f - num12;
                    Projectile.velocity = (Projectile.velocity * 40f + vector2) / 41f;
                }
            }
            else
            {
                bool flag2 = false;
                if (!flag2)
                {
                    flag2 = Projectile.ai[1] == 1f;
                }

                float num13 = 6f;
                if (flag2)
                {
                    num13 = 15f;
                }

                Vector2 center = Projectile.Center;
                Vector2 vector3 = Owner.Center - center + new Vector2(0f, -60f);
                float num14 = vector3.Length();
                if (num14 > 200f && num13 < 8f)
                {
                    num13 = 8f;
                }

                if (num14 < num5 && flag2 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }

                if (num14 > 2000f)
                {
                    Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
                    Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.height / 2);
                    Projectile.netUpdate = true;
                }

                if (num14 > 70f)
                {
                    vector3.Normalize();
                    vector3 *= num13;
                    Projectile.velocity = (Projectile.velocity * 40f + vector3) / 41f;
                }
                else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }

            float num15 = (float)(int)Main.mouseTextColor / 200f - 0.35f;
            num15 *= 0.2f;
            Projectile.scale = num15 + 0.95f;

            if (Projectile.owner != Main.myPlayer)
            {
                return;
            }

            if (Projectile.ai[0] != 0f)
            {
                Projectile.ai[0] -= 1f;
                return;
            }

            float num16 = Projectile.position.X;
            float num17 = Projectile.position.Y;
            float num18 = 1200f;
            bool flag3 = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile))
                {
                    float num19 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num20 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num21 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num19) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num20);
                    if (num21 < num18)
                    {
                        num18 = num21;
                        num16 = num19;
                        num17 = num20;
                        flag3 = true;
                    }
                }
            }

            if (!flag3)
            {
                return;
            }

            SoundEngine.PlaySound(in SoundID.Item105, Projectile.position);
            int num22 = Main.rand.Next(5, 8);
            for (int k = 0; k < num22; k++)
            {
                Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                int num23 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<CosmicBlast>(), (int)((double)Projectile.damage * 0.5), 2f, Projectile.owner, num6);
                if (Main.projectile.IndexInRange(num23))
                {
                    Main.projectile[num23].originalDamage = Projectile.originalDamage / 2;
                }
            }

            float num24 = num16 - Projectile.Center.X;
            float num25 = num17 - Projectile.Center.Y;
            float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
            num26 = 15f / num26;
            num24 *= num26;
            num25 *= num26;
            int num27 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, num24, num25, ModContent.ProjectileType<CosmicBlastBig>(), Projectile.damage, 3f, Projectile.owner, num6);
            if (Main.projectile.IndexInRange(num27))
            {
                Main.projectile[num27].originalDamage = Projectile.originalDamage;
            }

            Projectile.ai[0] = 100f;
            //            Projectile.ChargingMinionAI(1200f, 1500f, 2400f, 150f, 0, 30f, 18f, 9f, new Vector2(0f, -60f), 30f, 12f, tileVision: true, ignoreTilesWhenCharging: true);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
        }
    }
}


