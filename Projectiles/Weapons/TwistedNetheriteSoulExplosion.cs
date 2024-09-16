using CalamityMod.Projectiles.BaseProjectiles;
using CalRemix.Buffs;
using CalRemix.Items.Armor;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class TwistedNetheriteSoulExplosion : BaseMassiveExplosionProjectile
	{
        public override int Lifetime => 120;
        public override bool UsesScreenshake => false;
        public override Color GetCurrentExplosionColor(float pulseCompletionRatio) => Color.Cyan;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twisted Netherite Soul Explosion");
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
            Projectile.DamageType = ModContent.GetInstance<GenericDamageClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.TryGetOwner(out Player player))
            {
                if (player.armor[0].type == ModContent.ItemType<TwistedNetheriteHelmet>())
                {
                    TwistedNetheriteHelmet helmet = player.armor[0].ModItem as TwistedNetheriteHelmet;
                    target.GetGlobalNPC<CalRemixGlobalNPC>().wither = helmet.souls;
                }
            }
            target.AddBuff(ModContent.BuffType<Wither>(), 600);
        }
    }
}