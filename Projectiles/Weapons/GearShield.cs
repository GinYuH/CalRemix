using CalamityMod;
using CalRemix.ExtraTextures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class GearShield : ModProjectile
	{
        public override string Texture => "CalRemix/Items/Weapons/GearworkShield";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gearwork Shield");
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Owner.Calamity().cooldowns.TryGetValue(GearworkCooldown.ID, out var value) && Projectile.Calamity().stealthStrike)
                value.timeLeft--;
        }
    }
}