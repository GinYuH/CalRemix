using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Summon;
using CalamityMod;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class CirnogenicSentry : ModProjectile
    {
        public ref float Attack => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 78;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            Attack++;
            if (Attack % 30 == 0)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.TopLeft, Vector2.UnitX * -20, ModContent.ProjectileType<IceSentryShard>(), Projectile.damage, 0);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Left, Vector2.UnitX * -20, ModContent.ProjectileType<IceSentryShard>(), Projectile.damage, 0);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.BottomLeft, Vector2.UnitX * -20, ModContent.ProjectileType<IceSentryShard>(), Projectile.damage, 0);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.TopRight, Vector2.UnitX * 20, ModContent.ProjectileType<IceSentryShard>(), Projectile.damage, 0);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Right, Vector2.UnitX * 20, ModContent.ProjectileType<IceSentryShard>(), Projectile.damage, 0);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.BottomRight, Vector2.UnitX * 20, ModContent.ProjectileType<IceSentryShard>(), Projectile.damage, 0);
            }
            if (Attack % 40 == 0)
            {
                int npc = Projectile.FindTargetWithLineOfSight();
                if (npc.WithinBounds(Main.maxNPCs))
                {
                    if (Main.npc[npc].CanBeChasedBy())
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.Center.DirectionTo(Main.npc[npc].Center) * 20, ModContent.ProjectileType<IceSentryFrostBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            if (Attack >= 120)
                Attack = 0;
        }
    }
}
