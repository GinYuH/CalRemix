using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class OnyxFist : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float State => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];
        private Vector2 InitVelocity;
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Onyx Fist");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 48;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 22;
            Projectile.alpha = 150;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            InitVelocity = Projectile.velocity;
            Projectile.velocity = InitVelocity.SafeNormalize(Vector2.One) * -4f;
        }
        public override void AI()
        {
            Timer++;
            if (Timer > 45)
            {
                if (State <= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, Projectile.Center);
                    for (int k = 0; k < 3; k++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.ShadowbeamStaff, Scale: 4f + Main.rand.NextFloat());
                        dust.noGravity = true;
                        dust.velocity = -InitVelocity.RotatedByRandom(MathHelper.ToRadians(45)) * Main.rand.NextFloat(1, 2);
                    }
                    State = 1;
                }
                if (Timer % 2 == 0)
                    Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.ShadowbeamStaff, Scale: 1f + Main.rand.NextFloat());
                Projectile.velocity = InitVelocity;
                Projectile.alpha = 180;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.rotation = (-Projectile.velocity).ToRotation();
                Projectile.velocity *= 0.97f;
            }
            if (Timer > 60)
                Projectile.tileCollide = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Timer > 45;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ProjectileID.BlackBolt, Projectile.damage, Projectile.knockBack);
            proj.DamageType = DamageClass.Magic;
            proj.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Timer > 45)
                spriteEffects = Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            else
                spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, new Color(255, 255, 255, Projectile.alpha), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}