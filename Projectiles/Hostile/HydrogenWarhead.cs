using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class HydrogenWarhead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warhead");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 480;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.ai[1]++;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            if ((double)Projectile.velocity.Y < 0.25 && (double)Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X *= 0.8f;
            }
            Projectile.rotation = (0f - Projectile.velocity.X) * 0.05f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
            if (Projectile.ai[1] > 120 && Projectile.velocity.Y > 1)
            {
                Projectile.tileCollide = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            Projectile.ExpandHitboxBy(128);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.Damage();

            if (Main.expertMode)
            for (int i = 0; i < 5; i++)
            {
                Vector2 acidSpeed = (Vector2.UnitY * Main.rand.NextFloat(-10f, -8f)).RotatedByRandom(MathHelper.ToRadians(45f));
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, acidSpeed, ModContent.ProjectileType<SeafoamBubble>(), (int)(Projectile.damage * 0.25f), 3f, Main.myPlayer);
                Projectile pe = Main.projectile[p];
                pe.hostile = true;
                pe.friendly = false;
                pe.DamageType = DamageClass.Generic;
            }

            for (int i = 0; i < 12; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 15; j++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 5f;
                dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 2f;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreSource = Projectile.Center;
                int goreAmt = 3;
                Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < (goreAmt / 3))
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= (2 * goreAmt / 3))
                    {
                        velocityMult = 1f;
                    }
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }
            //HydrogenExplosion(Projectile);
        }

        public static void HydrogenExplosion(Projectile proj)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    float num = Math.Abs((float)proj.Center.X / 16f + i);
                    float num2 = Math.Abs((float)proj.Center.Y / 16f + j);
                    if (!(Math.Sqrt(num * num + num2 * num2) < (double)2))
                        continue;

                    if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                    {
                        if (CalRemixWorld.SunkenSeaTiles.Contains(Main.tile[i, j].TileType))
                        {
                            WorldGen.KillTile(i, j);
                            if (!Main.tile[i, j].HasTile && Main.netMode != 0)
                                NetMessage.SendData(17, -1, -1, null, 0, i, j);
                        }
                    }

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            if (Main.tile[k, l] != null && (Main.tile[k, l].WallType == ModContent.WallType<NavystoneWall>() || Main.tile[k, l].WallType == ModContent.WallType<EutrophicSandWall>()))
                            {
                                WorldGen.KillWall(k, l);
                                if (Main.tile[k, l].WallType == 0 && Main.netMode != 0)
                                    NetMessage.SendData(17, -1, -1, null, 2, k, l);
                            }
                        }
                    }
                }
            }
        }
    }
}