using CalamityMod.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;

namespace CalRemix.Projectiles.Weapons
{
    public class ChainSpearStab : BaseSpearProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chain Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;  //The width of the .png file in pixels divided by 2.
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.height = 50;  //The height of the .png file in pixels divided by 2.
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }

        public override float InitialSpeed => 3f;
        public override float ReelbackSpeed => 1.1f;
        public override float ForwardSpeed => 0.95f;
        public override void PostAI()
        {
            foreach (NPC n in Main.npc)
            {
                if (n != null && n.active && n.IsAnEnemy())
                if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<ChainSpearProj>()) > 0)
                {
                    Projectile.scale = 2;
                    break;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 120);
            Player owner = Main.player[Projectile.owner];
            if (target.GetGlobalNPC<CalRemixGlobalNPC>().grappled)
            {
                Projectile.NewProjectile(Terraria.Entity.GetSource_None(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BrimlanceHellfireExplosion>(), hit.Damage * 22, hit.Knockback, Projectile.owner);
                owner.GiveIFrames(60, true);
                CalamityUtils.KillShootProjectiles(true, ModContent.ProjectileType<ChainSpearProj>(), owner);
                owner.Calamity().GeneralScreenShakePower = 6;
            }
        }
    }
}
