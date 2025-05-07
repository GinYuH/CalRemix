using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AcceloRingSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.velocity = Vector2.Zero;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 104, 24, ref collisionPoint);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] == 0)
            {
                if (Projectile.rotation == 0)
                    Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.localAI[0] = Main.rand.Next(1, 4);
            }
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            int animDuration = 6;
            int retractDuration = 30;
            int fullWidth = tex.Width;
            int curWidth = (int)MathHelper.Lerp(0, fullWidth, CalamityUtils.ExpOutEasing(Utils.GetLerpValue(0, animDuration, Projectile.ai[0], true), 1));
            if (Projectile.timeLeft < retractDuration)
            {
                curWidth = (int)MathHelper.Lerp(fullWidth, 0, Utils.GetLerpValue(retractDuration, 0, Projectile.timeLeft, true));
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(tex.Width - curWidth, (int)(tex.Height / 3 * (Projectile.localAI[0] - 1)), curWidth, tex.Height / 3), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(0, tex.Height / 6f), Projectile.scale, 0);
            if (Projectile.ai[1] != 0)
            {
                Texture2D character = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/AcceloRingKnuckles").Value;
                if (Projectile.ai[1] == 2)
                {
                    character = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/AcceloRingTails").Value;
                }
                else if (Projectile.ai[1] == 3)
                {
                    character = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/AcceloRingIvor").Value;
                }

                int headFrame = 1;
                if (Projectile.ai[0] < animDuration)
                    headFrame = 0;
                if (Projectile.timeLeft < retractDuration)
                    headFrame = 2;

                Main.EntitySpriteDraw(character, Projectile.Center + Projectile.rotation.ToRotationVector2() * curWidth - Main.screenPosition, character.Frame(1, 3, 0, headFrame), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(character.Width / 2, character.Height / 6), Projectile.scale, 0);
            }
            Projectile p = Main.projectile[(int)Projectile.ai[2]];
            if (p.type == ModContent.ProjectileType<AcceloRingProjectile>())
            {
                ProjectileLoader.PreDraw(p, ref lightColor);
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
        }
    }
}