using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class RazorTooth : ModProjectile
    {
        public override string Texture => "CalRemix/Items/Weapons/RazorTeeth";
        private NPC Target;
        private Vector2 Offset;
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Razor Tooth");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 90;
        }
        public override void AI()
        {
            if (Offset != Vector2.Zero && Target != null)
            {
                Projectile.aiStyle = -1;
                Projectile.Center = Target.Center - Offset;
                if (!Target.active)
                    Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
            if (Projectile.Calamity().stealthStrike)
                target.AddBuff(BuffID.Venom, 120);
            if (Projectile.Calamity().stealthStrike && Target is null)
            {
                Projectile.penetrate = -1;
                Projectile.damage /= 5;
                Offset = target.Center - Projectile.Center;
                Target = target;
            }

        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.SulphurousSeaAcid, Scale: 1f + Main.rand.NextFloat());
                dust.noGravity = false;
            }
        }
    }
}