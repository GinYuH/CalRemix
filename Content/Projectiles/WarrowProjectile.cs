using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class WarrowProjectile : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Ammo/WarArrow";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warrow");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int time = Projectile.ai[2] == 1 ? 600 : 240;
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), time);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int time = Projectile.ai[2] == 1 ? 600 : 240;
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), time);
        }
    }
}