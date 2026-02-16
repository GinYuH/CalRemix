using CalamityMod;
using CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Vernix;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Chaotrix
{
    public class FireWaveBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.width = 16;
            Projectile.timeLeft = 480;
            //killPretendType=15   uhhhmmmm documentation says this controls what happens on death but im too lazy to copy it
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ClampMagnitude(-22, 22);

            Projectile.rotation += Projectile.velocity.Length() * 0.05f;
            Projectile.velocity.Y += 0.25f;
        }

        public override void OnKill(int timeLeft)
        {
            int damage = Projectile.damage;
            int type = ModContent.ProjectileType<FireWave>();

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(7, 0), type, damage, 0f, Main.myPlayer);
            Main.projectile[proj].friendly = Projectile.friendly;
            Main.projectile[proj].hostile = Projectile.hostile;
            int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-7, 0), type, damage, 0f, Main.myPlayer);
            Main.projectile[proj2].friendly = Projectile.friendly;
            Main.projectile[proj2].hostile = Projectile.hostile;

            Projectile.active = false;
        }
    }
}
