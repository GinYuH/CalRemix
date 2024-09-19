using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class IchorDaggerProj : ModProjectile
	{
        public override string Texture => "CalRemix/Content/Items/Weapons/IchorDagger";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichor Dagger");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.aiStyle = 2;
            AIType = 48;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            if (Projectile.Calamity().stealthStrike && Projectile.timeLeft % 48 == 0)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(22.5f)) * 0.5f, ProjectileID.GoldenShowerFriendly, Projectile.damage / 5, Projectile.knockBack / 2, Projectile.owner);
                proj.DamageType = DamageClass.Default;
            }
            if (Projectile.Calamity().stealthStrike && Projectile.timeLeft % 12 == 0)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(11.25f)) * 0.25f, ModContent.ProjectileType<IchorSpark>(), Projectile.damage / 5, Projectile.knockBack / 2, Projectile.owner);
                proj.DamageType = DamageClass.Default;
            }
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center + Projectile.velocity, 1, 1, DustID.GoldCoin, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GoldCoin, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                Projectile.Kill();
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X;
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, value.Size() / 2f, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}