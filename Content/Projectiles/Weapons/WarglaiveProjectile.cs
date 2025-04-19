using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class WarglaiveProjectile : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/Warglaive";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Warglaive");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            AIType = ProjectileID.Shuriken;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                Projectile.tileCollide = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.Calamity().stealthStrike)
                return false;
            else
                return true;
        }
    }
}