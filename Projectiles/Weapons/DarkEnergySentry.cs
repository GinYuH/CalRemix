using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class DarkEnergySentry : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/CeaselessVoid/DarkEnergy";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Energy");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 76;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 2f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft *= 5;
            Projectile.sentry = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            if (Projectile.type != ModContent.ProjectileType<DarkEnergySentry>())
                return;

            int frameGate = 6;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > frameGate)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 6)
            {
                Projectile.frame = 0;
            }

            if (Owner.dead)
                Projectile.timeLeft = 2;
            if (Projectile.ai[0] != 0f)
            {
                Projectile.ai[0] -= 1f;
                return;
            }
            NPC npc = Projectile.Center.MinionHoming(550f, Main.player[Projectile.owner]);
            if (npc != null)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(npc.Center) * 12f, ModContent.ProjectileType<VoidConcentrationOrb>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                Projectile.ai[0] = 8f;
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}
