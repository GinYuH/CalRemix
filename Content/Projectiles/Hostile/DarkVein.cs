using CalamityMod;
using CalRemix.Content.NPCs.Bosses.BossScule;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class DarkVein : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.NPCHit9, Projectile.Center);
        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
                Projectile.active = false;
            if (Projectile.alpha < 255)
                Projectile.alpha += 255 / 180;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.LifeDrain, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), Scale: Main.rand.NextFloat() + 1f);
            }
            int index = Player.FindClosest(Projectile.Center, 1, 1);
            if (Main.player[index] == null || !index.WithinBounds(Main.maxPlayers))
                return;
            else if (Main.player[index].dead || !Main.player[index].active)
                return;
            Player target = Main.player[index];
            Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Projectile.Center.DirectionTo(target.Center) * 4.5f, ModContent.ProjectileType<CalamityLaser>(), 0, 0, ai1: 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, (Projectile.Center.DirectionTo(target.Center) * 4.5f).RotatedByRandom(MathHelper.ToRadians(135f)), ModContent.ProjectileType<CalamityLaser>(), 0, 0, ai1: 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, (Projectile.Center.DirectionTo(target.Center) * 4.5f).RotatedByRandom(MathHelper.ToRadians(135f)), ModContent.ProjectileType<CalamityLaser>(), 0, 0, ai1: 1);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color (255, 0, 0, Projectile.alpha);
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}
