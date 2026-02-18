using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod;
namespace CalRemix.Content.Projectiles.Weapons
{
    public class GodSlayerPulse : ModProjectile
    {
        public bool initialized = false;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
        }

        public override void AI()
        {
            Projectile.velocity = new Vector2(0f, (float)Math.Sin((double)(MathHelper.TwoPi * Projectile.ai[0] / 300f)) * 0.5f);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 300f)
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 7200f)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= 10f)
            {
                Projectile.localAI[0] = 0f;
                int projCount = 0;
                int index = 0;
                float findOldest = 0f;
                int projType = Projectile.type;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == Projectile.owner && proj.type == projType && proj.ai[1] < 3600f)
                    {
                        projCount++;
                        if (proj.ai[1] > findOldest)
                        {
                            index = i;
                            findOldest = proj.ai[1];
                        }
                    }
                }
                if (projCount > 1)
                {
                    Main.projectile[index].netUpdate = true;
                    Main.projectile[index].ai[1] = 36000f;
                    return;
                }
            }
            if (!initialized)
            {
                SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                Projectile.ExpandHitboxBy(20);
                initialized = true;
            }
            if (Projectile.timeLeft % 50 == 1 && Projectile.alpha <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                Projectile.ExpandHitboxBy(20);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        Vector2 velocity = ((MathHelper.TwoPi * i / 16f) - (MathHelper.Pi / 16f)).ToRotationVector2() * 3f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GodSlayerFireball>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                    for (int i = 0; i < 24; i++)
                    {
                        Vector2 velocity = ((MathHelper.TwoPi * i / 16f) - (MathHelper.Pi / 16f)).ToRotationVector2() * 5f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GodSlayerFireball>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            NPC target = CalamityUtils.MinionHoming(Projectile.Center, 1000, Main.player[Projectile.owner]);
            if (Projectile.timeLeft % 350 == 1 && target != null && target.active && Projectile.timeLeft > 60)
            {
                SoundEngine.PlaySound(CalamityMod.NPCs.DevourerofGods.DevourerofGodsHead.HitSound);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 pos = i == 0 ? Projectile.Center + Vector2.UnitX * 80 : Projectile.Center + Vector2.UnitX * -80;
                        Vector2 velocity = pos.DirectionTo(target.Center) * 6;
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, velocity, ModContent.ProjectileType<CosmicGuardianHead>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, Vector2.Zero * 3, ModContent.ProjectileType<CosmicGuardianTail>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0, 0, p);
                        for (var k = 0; k < 20; k++)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, Vector2.Zero * 3, ModContent.ProjectileType<CosmicGuardianBody>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0, 0, p);
                        }
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            int height = texture.Height / Main.projFrames[Projectile.type];
            int frameHeight = height * Projectile.frame;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, frameHeight, texture.Width, height)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, height / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool? CanDamage() => false;
    }
}
