using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using CalamityMod.Walls;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class HydrogenMine : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Summon/Dreadmine";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Mine");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            Projectile.ExpandHitboxBy(248);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.Damage();

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
            HydrogenShell.HydrogenExplosion(Projectile);
        }

        public static void HydrogenExplosion(Projectile proj)
        {
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    float num = Math.Abs((float)proj.Center.X / 16f + i);
                    float num2 = Math.Abs((float)proj.Center.Y / 16f + j);
                    if (!(Math.Sqrt(num * num + num2 * num2) < (double)4))
                        continue;

                    Tile t = CalamityUtils.ParanoidTileRetrieval((int)(proj.Center.X / 16) + i, (int)(proj.Center.Y / 16) + j);

                    if (t != null && t.HasTile)
                    {
                        if (CalRemixWorld.SunkenSeaTiles.Contains(t.TileType))
                        {
                            WorldGen.KillTile(i, j);
                            if (!t.HasTile && Main.netMode != 0)
                                NetMessage.SendData(17, -1, -1, null, 0, i, j);
                        }
                    }

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            if (t != null && (t.WallType == ModContent.WallType<NavystoneWall>() || t.WallType == ModContent.WallType<EutrophicSandWall>()))
                            {
                                WorldGen.KillWall(k, l);
                                if (t.WallType == 0 && Main.netMode != 0)
                                    NetMessage.SendData(17, -1, -1, null, 2, k, l);
                            }
                        }
                    }
                }
            }
        }
    }
}