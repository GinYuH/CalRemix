using CalamityMod.Items.Accessories;
using CalRemix.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AcceloRingProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = Projectile.velocity.ToRotation();
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            if (Projectile.frame == 4)
            {
                Projectile.ai[0]++;
            }
            int spawnSpike = 22;
            if (Projectile.ai[0] == spawnSpike)
            {
                SoundEngine.PlaySound(AcceloRing.AcceloRingUseSound, Projectile.Center);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AcceloRingSpike>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai1: Projectile.ai[1], ai2: Projectile.whoAmI);
                Main.projectile[p].rotation = Projectile.rotation;
            }
            if (Projectile.ai[0] > 120 + spawnSpike)
            {
                Projectile.frame = 5;
                Projectile.scale -= 0.02f;
                if (Projectile.scale <= 0)
                    Projectile.Kill();
            }
            if (Projectile.frame < 4)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 4)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin);
                Main.dust[d].noGravity = true;
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, tex.Frame(1, 6, 0, Projectile.frame), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 12), Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}