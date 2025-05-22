using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.Audio;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Typeless;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MagmasherHammer : ModProjectile, ILocalizedModType
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/Magmasher";

        private int explosionCount = 0;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Player player = Main.player[Projectile.owner];

            if (Projectile.localAI[2] == 0)
            {
                Projectile.ai[2] = Projectile.velocity.Length();
                Projectile.localAI[2] = 1;
            }

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
                    if (Projectile.ai[1] >= 40f)
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                target.AddBuff(BuffID.OnFire3, 180);

                if (Projectile.Calamity().stealthStrike)
                {
                    if (explosionCount < 5) //max amount of explosions to prevent worm memes
                    {
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, Projectile.Center);
                        GeneralParticleHandler.SpawnParticle(new DetailedExplosion(target.Center, Vector2.Zero, Color.Orange, Vector2.One, Main.rand.NextFloat(-3.14f, 3.14f), MathHelper.Lerp(0.2f, 0.5f, (float)explosionCount / 5), MathHelper.Lerp(1.4f, 2f, (float)explosionCount / 5), 10));
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 6;
                        foreach (NPC n in Main.ActiveNPCs)
                        {
                            if (n.Distance(target.Center) < MathHelper.Lerp(300, 500, (float)explosionCount / 5) && target.whoAmI != n.whoAmI)
                            {
                                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), n.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), (int)MathHelper.Lerp((float)damageDone, (float)damageDone * 3, (float)explosionCount / 5), Projectile.knockBack, Main.player[Projectile.owner].whoAmI, n.whoAmI);
                                Main.projectile[p].DamageType = DamageClass.Melee;
                            }
                        }
                        explosionCount++;
                    }

                    int buffType = ModContent.BuffType<HolyFlames>();
                    float radius = 112f; // 7 blocks

                    foreach (NPC nPC in Main.ActiveNPCs)
                    {
                        if (!nPC.dontTakeDamage && !nPC.buffImmune[buffType] && Vector2.Distance(Projectile.Center, nPC.Center) <= radius)
                        {
                            if (nPC.FindBuffIndex(buffType) == -1)
                                nPC.AddBuff(buffType, 60, false);
                        }
                    }
                }
                else
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[p].DamageType = ModContent.GetInstance<RogueDamageClass>();
                }
            }
        }
    }
}
