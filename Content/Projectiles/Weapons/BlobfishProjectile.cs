using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class BlobfishProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.penetrate = 3;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath15, Projectile.Center);
                for (int i = 0; i < Main.rand.Next(4, 6); i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(90)) * 3f, ModContent.ProjectileType<PlushieCotton>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 30 && !Projectile.Calamity().stealthStrike)
            {
                Projectile.timeLeft = 30;
                return false;
            }
            else if (!Projectile.Calamity().stealthStrike)
                return false;
            return true;
        }
    }
}