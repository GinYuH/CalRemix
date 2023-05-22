using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class TitanTidesDisc : ModProjectile
    {
        public override string Texture => "CalRemix/Items/Weapons/TitanTides";
        public ref float Timer => ref Projectile.ai[0];
        public ref float Sound => ref Projectile.localAI[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Titan Tides");
        }
        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10) && Timer >= 270)
                target.AddBuff(ModContent.BuffType<Shred>(), 360);
        }
        public override void AI()
        {
            if (Owner.channel)
                Projectile.timeLeft = 2;
            else
                Projectile.Kill();
            Projectile.Center = Main.MouseWorld;

            if (Timer == 0)
            {
                SoundEngine.PlaySound(SoundID.Item71);
            }

            if (Timer < 270)
                Timer++;
            Projectile.scale = Timer / 135f + 1f;
            Projectile.width = (int)(150f * Projectile.scale);
            Projectile.height = (int)(150f * Projectile.scale);
            Projectile.rotation += Owner.direction * (Timer / 1080f + 0.05f);
            if (Timer >= 270)
            {
                Vector2 randVector = CalamityUtils.RandomVelocity(64f, 64f, 64f);
                Dust.NewDust(new Vector2(Projectile.position.X+Projectile.width/4, Projectile.position.Y+Projectile.height/4), Projectile.width / 2, Projectile.height / 2, DustID.ShadowbeamStaff, randVector.X, randVector.Y);
            }
            Sound++;
            if (Sound >= 12 + System.Math.Clamp(60 - Timer, 0, 60))
            {
                SoundEngine.PlaySound(SoundID.Item71);
                Sound = 0;
            }

            Dust.NewDust(Owner.position, Owner.getRect().Width, Owner.getRect().Height, DustID.ShadowbeamStaff, Owner.Center.DirectionTo(Projectile.Center).X * 20f, Owner.Center.DirectionTo(Projectile.Center).Y * 20f);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}


