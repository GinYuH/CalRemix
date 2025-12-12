using CalamityMod;
using CalamityMod.Projectiles.Boss;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.RebornModMiracleVine;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix
{
    public class PerennialFlowerMine : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.height = 46;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.width = 48;
            Projectile.timeLeft = 120;
            //killPretendType=15   uhhhmmmm documentation says this controls what happens on death but im too lazy to copy it
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ClampMagnitude(-22, 22);

            Projectile.rotation += Projectile.velocity.X * 0.05f;
            Projectile.velocity *= 0.96f;
        }

        public override void OnKill(int timeLeft)
        {
            float num48 = 25f;
            int damage = (Projectile.damage / 2) + 5;
            int type = ModContent.ProjectileType<PerennialFlowerMineVine>();
            int total = 3;
            float randomRot = Main.rand.NextFloat(0, MathHelper.TwoPi);
            
            for (int i = 0; i < total; i++)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -num48), type, damage, 0f, Main.myPlayer);
                Main.projectile[proj].velocity = Main.projectile[proj].velocity.RotatedBy(((MathHelper.TwoPi / total) * i) + randomRot);
                Main.projectile[proj].friendly = Projectile.friendly;
                Main.projectile[proj].hostile = Projectile.hostile;
            }
            
            Projectile.active = false;
        }
    }
}
