using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class XiphactinusGunProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.MechanicalPiranha;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
        }

        // This needs to happen retroactively due to Deadshot Brooch and other potential items boosting updates
        public override void AI()
        {
            Projectile p = Main.projectile[(int)Projectile.ai[2]];
            if (!p.active || p.type != ModContent.ProjectileType<XiphactinusGunHoldout>())
            {
                Projectile.Kill();
            }
            Projectile.localNPCHitCooldown = 10 * Projectile.MaxUpdates;
            if (Projectile.frame > 1)
                Projectile.frame = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile p = Main.projectile[(int)Projectile.ai[2]];
            if (!p.active || p.type != ModContent.ProjectileType<XiphactinusGunHoldout>())
                return false;
            Texture2D chain = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/XiphactinusGunChain").Value;
            CalRemixHelper.DrawChain(chain, Projectile.Center - Main.screenPosition, Main.projectile[(int)Projectile.ai[2]].Center + Vector2.UnitX.RotatedBy(p.rotation) * 40 - Main.screenPosition);
            return true;
        }
    }
}
