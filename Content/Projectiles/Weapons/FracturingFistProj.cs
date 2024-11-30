using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class FracturingFistProj : ModProjectile
	{
        public ref float Timer => ref Projectile.ai[0];
        public ref float Mode => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            if (Projectile.timeLeft % 2 == 0)
            {
                if (Projectile.Calamity().stealthStrike && Main.rand.NextBool(4) && Mode == 0 && Timer >= 10f)
                    GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(Projectile.Center + Main.rand.NextVector2Circular(5f, 5f) - Projectile.velocity * 4f, -Projectile.velocity / 2f, Main.rand.NextFloat(0.3f, 0.6f), Color.SkyBlue, 30, 3.4f, 4.5f, 3f, 0.02f));
                else
                    Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.MagicMirror, 0, 0, Scale: Main.rand.NextFloat(0.6f, 1.1f));
            }
            if (Projectile.velocity.Length() < 10f)
                Projectile.velocity *= 1.25f;
            Projectile.velocity.ClampMagnitude(1f, 10f);

            Timer += 1f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathF.PI / 2f;
            if (Timer > 10f)
            {
                Timer = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (Math.Abs(Projectile.velocity.X) < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
            }
            if (Mode == 1)
                CalamityUtils.HomeInOnNPC(Projectile, ignoreTiles: false, 240f, 18f, 24f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Mode == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_None(), Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.PiOver2), Type, (int)(Projectile.damage * 0.4f), 0);
                    p.Calamity().stealthStrike = true;
                    p.scale = 0.75f;
                    p.ai[1] = 1;
                }
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in SoundID.Item14, Projectile.position);
            for (int i = 0; i < ((Mode == 0) ? 40 : 10); i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 100);
                dust.velocity *= (Mode == 0) ? 3f : 1.1f;
                dust.noGravity = true;
                if (Main.rand.NextBool())
                {
                    dust.scale = 0.5f;
                    dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Calamity().stealthStrike && Mode == 1 || Mode == 0)
            {
                float range = 999f;
                int index = 0;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.CanBeChasedBy(Projectile) && npc != target)
                    {
                        float distance = (Projectile.Center - npc.Center).Length();
                        if (distance < range)
                        {
                            range = distance;
                            index = npc.whoAmI;
                        }
                    }
                }
                Vector2 velocity = ((!(range < 999f)) ? (-Projectile.velocity) : (Main.npc[index].Center - Projectile.Center));
                velocity.Normalize();
                velocity *= 10f;
                if (Projectile.Calamity().stealthStrike && Mode == 1)
                    Projectile.velocity = velocity;
                else if (Mode == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), Projectile.Center, velocity, Type, (int)(Projectile.damage * 0.4f), 0);
                        p.Calamity().stealthStrike = true;
                        p.scale = 0.5f;
                        p.ai[1] = 1;
                    }
                    Projectile.Kill();
                }
            }
            else
                Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffects = Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, new Color(255, 255, 255, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}