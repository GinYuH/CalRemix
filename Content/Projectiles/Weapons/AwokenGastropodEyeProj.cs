using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AwokenGastropodEyeProj : MinecraftArrow
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/AwokenGastropodEye";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 180);
        }
    }
}