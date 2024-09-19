using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class BoneBundle : ModProjectile
	{
        public override string Texture => "CalRemix/Content/Items/Weapons/BundleBones";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Bucket o' Bones");
		}
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 3;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                Projectile.Kill();
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X;
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                Projectile.velocity.Y = -oldVelocity.Y / 2;
            if (Projectile.Calamity().stealthStrike)
            {
                int index = Projectile.FindTargetWithLineOfSight(320);
                if (!index.WithinBounds(Main.maxNPCs))
                    return false;
                NPC npc = Main.npc[index];
                if (npc != null)
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(npc.Center) * 8f, ModContent.ProjectileType<Bone>(), Projectile.damage * 3 / 10, Projectile.knockBack / 2, Projectile.owner, ai1: 1f);

            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, Projectile.position);
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)), ModContent.ProjectileType<Bone>(), Projectile.damage / 20, Projectile.knockBack / 2, Projectile.owner);
            }
        }
    }
}