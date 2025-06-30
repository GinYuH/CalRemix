using CalamityMod;
using CalamityMod.Sounds;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AttackSquid : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0;
            Projectile.penetrate = 5;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Main.projFrames[Projectile.type] = 3;
            // Shoot lasers if it finds something
            NPC targ = CalamityUtils.MinionHoming(Projectile.Center, 2100, Main.player[Projectile.owner]);
            if (targ != null)
            {
                if (targ.active && targ.life > 0)
                {
                    Projectile.velocity = Projectile.DirectionTo(targ.Center) * 10 + Main.rand.NextVector2Square(-20, 20);
                }
            }

            if (Projectile.frameCounter++ > 4)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame++ > 2)
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
