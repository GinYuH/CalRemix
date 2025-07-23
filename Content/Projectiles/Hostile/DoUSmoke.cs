using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class DoUSmoke : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Particles/RemixSmoke";

        public const int Life = 480;



        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = Life;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            /*NPC p = Main.npc[(int)Projectile.ai[2]];

            if (!p.active || p.life <= 0)
            {
                Projectile.Kill();
                return;
            }
            else if (Projectile.ai[0] == 60)
            {
                Projectile.velocity = Projectile.DirectionTo(p.Center) * 25;
            }*/

            Projectile.ai[0]++;

            if (Projectile.ai[1] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 6);
                Projectile.ai[1] = 1;
            }
            if (Projectile.ai[0] / (float)Life < 0.2f)
                Projectile.scale += 0.01f;
            else
                Projectile.scale *= 0.975f;

            Projectile.rotation += 0.05f * ((Projectile.velocity.X > 0) ? 1f : -1f);
            Projectile.velocity *= 0.85f;
        }
    }
}