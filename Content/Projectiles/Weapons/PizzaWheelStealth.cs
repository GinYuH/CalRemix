using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class PizzaWheelStealth : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 9999;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.ai[2] == 1)
            {
                Projectile.velocity.X *= 1.001f;
                Projectile.rotation += Projectile.velocity.X.DirectionalSign() * 0.22f;
            }
            else
            {
                Player player = Main.player[Projectile.owner];

                if (Projectile.localAI[2] == 0)
                {
                    Projectile.localAI[2] = 1;
                }

                Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
                Projectile.rotation += 1f;

                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 8;
                    SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
                }

                switch (Projectile.ai[0])
                {
                    case 0f:
                        Projectile.ai[1] += 1f;
                        if (Projectile.ai[1] >= 60f)
                        {
                            Projectile.ai[0] = 1f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                        }
                        break;
                    case 1f:
                        float returnSpeed = 25f;
                        float acceleration = 5f;
                        Vector2 playerVec = player.Center - Projectile.Center;
                        if (playerVec.Length() > 4000f)
                        {
                            Projectile.Kill();
                        }
                        playerVec.Normalize();
                        playerVec *= returnSpeed;
                        if (Projectile.velocity.X < playerVec.X)
                        {
                            Projectile.velocity.X += acceleration;
                            if (Projectile.velocity.X < 0f && playerVec.X > 0f)
                            {
                                Projectile.velocity.X += acceleration;
                            }
                        }
                        else if (Projectile.velocity.X > playerVec.X)
                        {
                            Projectile.velocity.X -= acceleration;
                            if (Projectile.velocity.X > 0f && playerVec.X < 0f)
                            {
                                Projectile.velocity.X -= acceleration;
                            }
                        }
                        if (Projectile.velocity.Y < playerVec.Y)
                        {
                            Projectile.velocity.Y += acceleration;
                            if (Projectile.velocity.Y < 0f && playerVec.Y > 0f)
                            {
                                Projectile.velocity.Y += acceleration;
                            }
                        }
                        else if (Projectile.velocity.Y > playerVec.Y)
                        {
                            Projectile.velocity.Y -= acceleration;
                            if (Projectile.velocity.Y > 0f && playerVec.Y < 0f)
                            {
                                Projectile.velocity.Y -= acceleration;
                            }
                        }
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Rectangle projHitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                            Rectangle playerHitbox = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                            if (projHitbox.Intersects(playerHitbox))
                            {
                                Projectile.Kill();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == 0)
            {
                for (int i = 0; i < Main.rand.Next(6, 12); i++)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnPos = target.Center + Vector2.UnitX * Main.rand.Next(400, 800) * Main.rand.NextBool().ToDirectionInt();
                        spawnPos.Y += Main.rand.Next(-40, 40);
                        int p = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), spawnPos, spawnPos.DirectionTo(target.Center).X * Vector2.UnitX * Main.rand.NextFloat(6, 9), ModContent.ProjectileType<PizzaWheelProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai2: 1);
                        Main.projectile[p].aiStyle = -1;
                        Main.projectile[p].ModProjectile.AIType = 0;
                        Main.projectile[p].tileCollide = false;
                        Main.projectile[p].timeLeft = 300;
                        Main.projectile[p].penetrate = 3;
                    }
                }
                Projectile.ai[2] = 2;
            }
        }
    }
}


