using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class SulphurArrow : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Ranged/ToxicArrow";
		public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Sulphur Arrow");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Ranged;
            AIType = ProjectileID.WoodenArrowFriendly;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Irradiated>(), 240);
        }
    }
}