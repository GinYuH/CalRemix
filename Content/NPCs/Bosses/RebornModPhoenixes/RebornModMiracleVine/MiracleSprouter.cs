using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.RebornModMiracleVine
{
    public class MiracleSprouter : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.height = 15;
            Projectile.penetrate = 1;
            Projectile.scale = 1.5f;
            Projectile.tileCollide = true;
            Projectile.width = 15;
            //killPretendType=15   uhhhmmmm documentation says this controls what happens on death but im too lazy to copy it
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.X, (double)Projectile.velocity.Y);
            int dust = Dust.NewDust(new Vector2((float)Projectile.position.X, (float)Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0, 0, 100, default, 2.0f);
            Main.dust[dust].noGravity = true;
        }

        public override void OnKill(int timeLeft)
        {
            float num48 = 25f;
            int damage = (Projectile.damage / 2) + 5;
            int type = ModContent.ProjectileType<MiracleVines>();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(0, -num48), type, damage, 0f, Main.myPlayer);
            Projectile.active = false;
        }
    }
}
