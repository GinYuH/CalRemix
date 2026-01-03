using System;
using System.ComponentModel;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class WulfrumNanite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.damage = 5;
            Projectile.width = Projectile.height = 20;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }

        public float projSpeed = 5;
        public bool readyForHoming = false;
        public bool homing = false;
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 2)
                Projectile.frame = 0;

            Player target = Main.player[(int)Projectile.ai[1]];
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(-360, 361));
                homing = false;
                readyForHoming = false;
            }
            //Go up and stuff
            if (Projectile.ai[0] < 60)
            {
                Projectile.rotation *= 0.9f;
                Projectile.velocity.Y += 0.1f;
            }

            //Go sideways and stuff
            if (Projectile.ai[0] == 60 && homing == false)
            {
                Projectile.rotation = 0;
                readyForHoming = true;
                Projectile.velocity = Projectile.DirectionFrom(target.Center);
                if (Projectile.velocity.X <= 0) Projectile.velocity = new Vector2(projSpeed, 0);
                else Projectile.velocity = new Vector2(-projSpeed, 0);
            }
            //Go down and stuff
            if ((Projectile.Center.X >= target.Center.X - 4) && (Projectile.Center.X <= target.Center.X + 4) && homing == false && readyForHoming == true)
            {
                homing = true;
                if (Projectile.position.Y >= target.Center.Y) Projectile.velocity = new Vector2(0, -projSpeed);
                else Projectile.velocity = new Vector2(0, projSpeed);
            }


            if (Projectile.ai[0] % 10 == 0) Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric, Main.rand.Next(-1, 2), Main.rand.Next(-1, 2), 0, default, 0.3f);

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Electrified, 60);
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 20; k++) Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.t_Martian, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), 0, default, 1f);
        }
    }
}