using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class ShadowPulse : BaseMassiveExplosionProjectile
	{
        public override int Lifetime => 60;

        public override bool UsesScreenshake => Projectile.Calamity().stealthStrike;

        public override float GetScreenshakePower(float pulseCompletionRatio)
        {
            return CalamityUtils.Convert01To010(pulseCompletionRatio) * 16f;
        }

        public override Color GetCurrentExplosionColor(float pulseCompletionRatio)
        {
            return Color.Lerp(Color.DarkViolet, Color.BlueViolet, MathHelper.Clamp(pulseCompletionRatio * 2.2f, 0f, 1f));
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Pulse");
        }

        public override void SetDefaults()
        {
            Projectile.width = (Projectile.height = 2);
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 600);
            target.AddBuff(ModContent.BuffType<Nightwither>(), 1200);
            target.AddBuff(BuffID.ShadowFlame, 1800);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 600);
            target.AddBuff(ModContent.BuffType<Nightwither>(), 1200);
            target.AddBuff(BuffID.ShadowFlame, 1800);
        }
    }
}